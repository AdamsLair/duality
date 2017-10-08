using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Resources;
using Duality.Cloning;
using Duality.Serialization;
using Duality.Editor;
using Duality.Properties;

namespace Duality
{
	/// <summary>
	/// Retrieves, processes and caches type information about the order in which initialization, 
	/// shutdown and update of different <see cref="Component"/> types are executed.
	/// </summary>
	public class ComponentExecutionOrder
	{
		private struct IndexedTypeItem
		{
			public Type Type;
			public int TypeIndex;
			public int ItemIndex;

			public override string ToString()
			{
				return string.Format("Item #{0} [{1}] ({2})", this.ItemIndex, this.TypeIndex, this.Type);
			}
		}
		private enum ConstraintPriority
		{
			ImplicitWeak,
			ImplicitStrong,
			ExplicitWeak,
			ExplicitStrong
		}
		private struct OrderConstraint
		{
			public Type FirstType;
			public Type LastType;
			public ConstraintPriority Priority;

			public OrderConstraint(Type first, Type last, ConstraintPriority priority)
			{
				this.FirstType = first;
				this.LastType = last;
				this.Priority = priority;
			}
			public OrderConstraint(Type typeA, ExecutionRelation relation, Type typeB, ConstraintPriority priority)
			{
				if (relation == ExecutionRelation.Before)
				{
					this.FirstType = typeA;
					this.LastType = typeB;
				}
				else
				{
					this.FirstType = typeB;
					this.LastType = typeA;
				}
				this.Priority = priority;
			}

			public override int GetHashCode()
			{
				return unchecked(
					this.FirstType.GetHashCode() * 17 + 
					this.LastType.GetHashCode() * 13 + 
					(int)this.Priority);
			}
			public override bool Equals(object obj)
			{
				if (!(obj is OrderConstraint)) return false;
				OrderConstraint other = (OrderConstraint)obj;
				return
					other.FirstType == this.FirstType &&
					other.LastType == this.LastType &&
					other.Priority == this.Priority;
			}
			public override string ToString()
			{
				return string.Format("{0} before {1} [{2}]", 
					this.FirstType != null ? this.FirstType.Name : "null", 
					this.LastType != null ? this.LastType.Name : "null", 
					this.Priority);
			}
		}
		private struct OrderConstraintComparer : IEqualityComparer<OrderConstraint>
		{
			public bool IgnorePriority;

			public bool Equals(OrderConstraint x, OrderConstraint y)
			{
				return
					x.FirstType == y.FirstType &&
					x.LastType == y.LastType &&
					(x.Priority == y.Priority || this.IgnorePriority);
			}
			public int GetHashCode(OrderConstraint obj)
			{
				return unchecked(
					obj.FirstType.GetHashCode() * 17 + 
					obj.LastType.GetHashCode() * 13 + 
					(this.IgnorePriority ? 0 : (int)obj.Priority));
			}
		}

		private Dictionary<Type,int> sortIndexCache = new Dictionary<Type,int>();
		private HashSet<Type> componentTypes = new HashSet<Type>();


		/// <summary>
		/// Sorts a list of <see cref="Component"/> types according to their execution order.
		/// </summary>
		/// <param name="types"></param>
		/// <param name="reverse"></param>
		public void SortTypes(IList<Type> types, bool reverse)
		{
			IndexedTypeItem[] indexedTypes = new IndexedTypeItem[types.Count];
			for (int i = 0; i < indexedTypes.Length; i++)
			{
				indexedTypes[i].Type = types[i];
				indexedTypes[i].TypeIndex = this.GetSortIndex(indexedTypes[i].Type);
			}

			if (reverse)
				Array.Sort(indexedTypes, (a, b) => b.TypeIndex - a.TypeIndex);
			else
				Array.Sort(indexedTypes, (a, b) => a.TypeIndex - b.TypeIndex);

			for (int i = 0; i < indexedTypes.Length; i++)
			{
				types[i] = indexedTypes[i].Type;
			}
		}
		/// <summary>
		/// Sorts a list of items according to the execution order of each items
		/// associated <see cref="Component"/> type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <param name="typeOfItem"></param>
		/// <param name="reverse"></param>
		public void SortTypedItems<T>(IList<T> items, Func<T,Type> typeOfItem, bool reverse)
		{
			T[] itemArray = items.ToArray();
			IndexedTypeItem[] indexedTypes = new IndexedTypeItem[items.Count];
			for (int i = 0; i < indexedTypes.Length; i++)
			{
				indexedTypes[i].Type = typeOfItem(itemArray[i]);
				indexedTypes[i].ItemIndex = i;
				indexedTypes[i].TypeIndex = this.GetSortIndex(indexedTypes[i].Type);
			}

			if (reverse)
				Array.Sort(indexedTypes, (a, b) => b.TypeIndex - a.TypeIndex);
			else
				Array.Sort(indexedTypes, (a, b) => a.TypeIndex - b.TypeIndex);

			for (int i = 0; i < indexedTypes.Length; i++)
			{
				items[i] = itemArray[indexedTypes[i].ItemIndex];
			}
		}

		/// <summary>
		/// Retrieves the sorting index of the specified <see cref="Component"/> type.
		/// </summary>
		/// <param name="componentType"></param>
		/// <returns></returns>
		public int GetSortIndex(Type componentType)
		{
			int index;
			if (!this.sortIndexCache.TryGetValue(componentType, out index))
			{
				this.CheckTypeParameter(componentType);

				// When uninitialized, build the sorting index
				if (this.componentTypes.Count == 0)
					this.InitSortIndex();

				// If the requested type was unaccounted for, add it and rebuild the index.
				// This avoids accidentally affecting sort order depending on which type
				// was queried first. We still want to keep unconstrained type order somewhat stable.
				if (this.componentTypes.Add(componentType))
					this.InitSortIndex();

				return this.sortIndexCache[componentType];
			}
			return index;
		}
		/// <summary>
		/// Clears the internal type data that this class has been storing internally.
		/// </summary>
		public void ClearTypeCache()
		{
			this.sortIndexCache.Clear();
			this.componentTypes.Clear();
		}

		private void CheckTypeParameter(Type componentType)
		{
			TypeInfo componentTypeInfo = componentType.GetTypeInfo();
			if (!typeof(Component).GetTypeInfo().IsAssignableFrom(componentTypeInfo))
				throw new ArgumentException("This class only deal with Component execution order. All queried types must derive from Component.");
			if (componentTypeInfo.IsAbstract || componentTypeInfo.IsInterface)
				throw new ArgumentException("The execution order of an abstract class is undefined. All queried types must be concrete types.");
		}
		private void InitSortIndex()
		{
			// Gather a list of all available component types to minimize rebuild of the sort index
			GatherComponentTypes(this.componentTypes);

			// Gather constraints to apply on execution order
			List<OrderConstraint> constraints = GatherConstraints(this.componentTypes);

			// Prepare constraint data for fast graph access
			Dictionary<Type,List<OrderConstraint>> graph = CreateConstraintGraph(this.componentTypes, constraints);

			// Resolve all loops in the constraint graph
			ResolveConstraintLoops(graph);

			// Score all nodes in the graph according to their top-level reachability
			Dictionary<Type,int> scores = ScoreGraphNodes(graph, this.componentTypes);

			// Extract unique sorting indices from the previously scored constraint graph
			this.sortIndexCache = ExtractSortIndices(scores);
		}

		/// <summary>
		/// Gathers all currently available <see cref="Component"/> types and stores them inside
		/// the provided collection. This will iterate over all relevant core and core plugin types 
		/// that are currently known, but since users can load plugins at any time, this list
		/// should never assumed to be final.
		/// </summary>
		/// <param name="typeSet"></param>
		private static void GatherComponentTypes(HashSet<Type> typeSet)
		{
			foreach (TypeInfo typeInfo in DualityApp.GetAvailDualityTypes(typeof(Component)))
			{
				if (typeInfo.IsAbstract) continue;
				typeSet.Add(typeInfo.AsType());
			}
		}
		/// <summary>
		/// Generates a list of normalized execution order constraints from a set of <see cref="Component"/>
		/// types.
		/// </summary>
		/// <param name="typeSet"></param>
		/// <returns></returns>
		private static List<OrderConstraint> GatherConstraints(HashSet<Type> typeSet)
		{
			// This hashset will store a distinct set of constraints, but allow constraining
			// the same two component types with different priorities. We'll resolve this later.
			HashSet<OrderConstraint> constraintSet = new HashSet<OrderConstraint>(new OrderConstraintComparer());

			foreach (Type type in typeSet)
			{
				TypeInfo typeInfo = type.GetTypeInfo();

				// When specifying constraints on an abstract type or interface, they will
				// be gathered when asking a concrete type for its (derived) attributes.
				if (typeInfo.IsAbstract || typeInfo.IsInterface) continue;

				// Gather explicit execution order statements made by the user via attribute
				foreach (ExecutionOrderAttribute orderAttrib in typeInfo.GetAttributesCached<ExecutionOrderAttribute>())
				{
					if (orderAttrib.Anchor == null) continue;

					TypeInfo anchorInfo = orderAttrib.Anchor.GetTypeInfo();
					if (anchorInfo.IsAbstract || anchorInfo.IsInterface)
					{
						foreach (Type typeB in typeSet)
						{
							TypeInfo typeInfoB = typeB.GetTypeInfo();
							if (!anchorInfo.IsAssignableFrom(typeInfoB)) continue;
							
							constraintSet.Add(new OrderConstraint(
								type, 
								orderAttrib.Relation,
								typeB, 
								ConstraintPriority.ExplicitWeak));
						}
					}
					else if (typeSet.Contains(orderAttrib.Anchor))
					{
						constraintSet.Add(new OrderConstraint(
							type, 
							orderAttrib.Relation,
							orderAttrib.Anchor, 
							ConstraintPriority.ExplicitStrong));
					}
				}

				// Gather implicit execution order statements derived from Component requirements
				foreach (RequiredComponentAttribute requireAttrib in typeInfo.GetAttributesCached<RequiredComponentAttribute>())
				{
					Type anchor = requireAttrib.RequiredComponentType;
					TypeInfo anchorInfo = anchor.GetTypeInfo();
					if (anchorInfo.IsAbstract || anchorInfo.IsInterface)
					{
						foreach (Type typeB in typeSet)
						{
							TypeInfo typeInfoB = typeB.GetTypeInfo();
							if (!anchorInfo.IsAssignableFrom(typeInfoB)) continue;
							
							constraintSet.Add(new OrderConstraint(
								type, 
								ExecutionRelation.After, 
								typeB,
								ConstraintPriority.ImplicitWeak));
						}
					}
					else if (typeSet.Contains(anchor))
					{
						constraintSet.Add(new OrderConstraint(
							type, 
							ExecutionRelation.After, 
							anchor,
							ConstraintPriority.ImplicitStrong));
					}
				}
			}

			// Resolve duplicate connections using each item's differing priority
			OrderConstraintComparer keyComparer = new OrderConstraintComparer { IgnorePriority = true };
			Dictionary<OrderConstraint,ConstraintPriority> maxPriorityMap = new Dictionary<OrderConstraint,ConstraintPriority>(keyComparer);
			foreach (OrderConstraint constraint in constraintSet)
			{
				ConstraintPriority priority;
				if (!maxPriorityMap.TryGetValue(constraint, out priority) || (int)priority < (int)constraint.Priority)
					maxPriorityMap[constraint] = constraint.Priority;
			}

			// Collect results
			List<OrderConstraint> resultList = new List<OrderConstraint>(maxPriorityMap.Count);
			foreach (var pair in maxPriorityMap)
			{
				resultList.Add(new OrderConstraint(
					pair.Key.FirstType, 
					pair.Key.LastType, 
					pair.Value));
			}

			return resultList;
		}
		/// <summary>
		/// Generates a graph-like data structure that organizes a list of execution order constraints
		/// in a way that allows to swiftly access them per-type. There's room for optimization, but
		/// since the number of <see cref="Component"/> types will likely stay below a few thousands
		/// this is probably not necessary and can be skipped for convenience and maintenance reasons.
		/// </summary>
		/// <param name="constraints"></param>
		/// <returns></returns>
		private static Dictionary<Type,List<OrderConstraint>> CreateConstraintGraph(IEnumerable<Type> types, List<OrderConstraint> constraints)
		{
			Dictionary<Type,List<OrderConstraint>> graph = new Dictionary<Type,List<OrderConstraint>>();

			foreach (OrderConstraint constraint in constraints)
			{
				List<OrderConstraint> list;
				if (!graph.TryGetValue(constraint.FirstType, out list))
				{
					list = new List<OrderConstraint>();
					graph.Add(constraint.FirstType, list);
				}
				list.Add(constraint);
			}

			return graph;
		}
		/// <summary>
		/// Clears the specified constraint graph of all loops.
		/// </summary>
		/// <param name="graph"></param>
		private static void ResolveConstraintLoops(Dictionary<Type,List<OrderConstraint>> graph)
		{
			while (true)
			{
				List<OrderConstraint> loop = FindConstraintLoop(graph);
				if (loop == null) return;

				// Found a loop? Find the weakest link in it
				OrderConstraint weakestLink = loop[0];
				for (int i = 1; i < loop.Count; i++)
				{
					OrderConstraint link = loop[i];
					if ((int)link.Priority < (int)weakestLink.Priority)
					{
						weakestLink = link;
					}
				}

				// If the loops weakest link was an explicit constraint, log a warning
				if ((int)weakestLink.Priority >= (int)ConstraintPriority.ExplicitWeak)
				{
					Log.Core.WriteWarning(
						"Found a loop in the component execution order constraint graph. Ignoring the weakest constraint " + 
						"({0} must be executed before {1}). Please check your ExecutionOrder attributes.",
						Log.Type(weakestLink.FirstType),
						Log.Type(weakestLink.LastType));
				}

				// Remove the weakest link
				List<OrderConstraint> links = graph[weakestLink.FirstType];
				links.Remove(weakestLink);
				if (links.Count == 0) graph.Remove(weakestLink.FirstType);
			}
		}
		/// <summary>
		/// Searches for constraint loops in the specified constraint graph and returns the first one.
		/// The result is not necessarily the smallest loop. Returns null if no loop was found.
		/// </summary>
		/// <param name="graph"></param>
		/// <returns></returns>
		private static List<OrderConstraint> FindConstraintLoop(Dictionary<Type,List<OrderConstraint>> graph)
		{
			if (graph.Count == 0) return null;

			// Note that in our specific case of a constraint graph, all valid graphs share
			// the property that there is a maximum of one connection between each two nodes,
			// and there are no loops within the graph, so it forms a propert tree.
			//
			// Thus, we can traverse a valid constraint graph in its entirety and never
			// encounter the same node twice.
			
			HashSet<Type> visitedNodes = new HashSet<Type>();
			Stack<Type> visitSchedule = new Stack<Type>();
			Dictionary<Type,OrderConstraint> prevLinks = new Dictionary<Type,OrderConstraint>();

			foreach (Type startNode in graph.Keys)
			{
				visitedNodes.Clear();
				visitSchedule.Clear();
				prevLinks.Clear();

				// Do a breadth-first graph traversal to see if we'll end up at the start
				visitSchedule.Push(startNode);
				while (visitSchedule.Count > 0)
				{
					Type node = visitSchedule.Pop();

					// Already visited this node in a previous run? Can't be part of a loop then.
					if (!visitedNodes.Add(node))
						continue;

					List<OrderConstraint> links;
					if (!graph.TryGetValue(node, out links))
						continue;

					for (int i = 0; i < links.Count; i++)
					{
						OrderConstraint link = links[i];
						Type nextNode = link.LastType;

						// Found the starting node again through traversal? Found a loop then!
						if (nextNode == startNode)
						{
							List<OrderConstraint> loopPath = new List<OrderConstraint>();
							while (true)
							{
								loopPath.Add(link);
								if (!prevLinks.TryGetValue(link.FirstType, out link))
									break;
							}
							return loopPath;
						}

						prevLinks[nextNode] = link;
						visitSchedule.Push(nextNode);
					}
				}
			}

			return null;
		}
		/// <summary>
		/// Assigns every node in the constraint graph a score that corresponds to its absolute position
		/// in the desired execution order. Score collisions are possible but unlikely.
		/// </summary>
		/// <param name="graph"></param>
		/// <returns></returns>
		private static Dictionary<Type,int> ScoreGraphNodes(Dictionary<Type,List<OrderConstraint>> graph, IEnumerable<Type> types)
		{
			Dictionary<Type,int> graphScores = new Dictionary<Type,int>(graph.Count);

			// Initialize graph scores for every node. Has to be done explicitly, so we
			// have proper scores for types that are unconstrained / not part of the graph.
			int baseScore = 0;
			foreach (Type type in types)
			{
				graphScores.Add(type, baseScore++);
			}

			Stack<Type> visitSchedule = new Stack<Type>();

			// For every node, traverse all other reachable nodes and propagate a score
			// value through them that increases with the number of visited nodes in
			// the chain.
			foreach (Type startNode in graph.Keys)
			{
				visitSchedule.Clear();
				visitSchedule.Push(startNode);
				while (visitSchedule.Count > 0)
				{
					Type node = visitSchedule.Pop();
					
					List<OrderConstraint> links;
					if (!graph.TryGetValue(node, out links))
						continue;

					int nodeScore = graphScores[node];

					// Push the current node's score to its neighbour nodes and schedule
					// them for traversal.
					for (int i = 0; i < links.Count; i++)
					{
						OrderConstraint link = links[i];

						Type nextNode = link.LastType;
						int nextNodeScore = graphScores[nextNode];

						// Determine the minimum score the neighbour node should have
						int minNextNodeScore = nodeScore + 1;

						// Propagate scores when out of order
						if (nextNodeScore < minNextNodeScore)
						{
							graphScores[nextNode] = minNextNodeScore;
							visitSchedule.Push(nextNode);
						}
					}
				}
			}

			return graphScores;
		}
		/// <summary>
		/// Extracts a unique sorting index for each type in a previously scored constraint graph.
		/// </summary>
		/// <param name="graphScores"></param>
		/// <returns></returns>
		private static Dictionary<Type,int> ExtractSortIndices(Dictionary<Type,int> graphScores)
		{
			List<KeyValuePair<Type,int>> scoreItems = graphScores.ToList();
			scoreItems.Sort((a, b) => a.Value - b.Value);

			Dictionary<Type,int> sortIndices = new Dictionary<Type,int>(scoreItems.Count);
			for (int sortIndex = 0; sortIndex < scoreItems.Count; sortIndex++)
			{
				sortIndices.Add(scoreItems[sortIndex].Key, sortIndex);
			}
			return sortIndices;
		}
	}
}
