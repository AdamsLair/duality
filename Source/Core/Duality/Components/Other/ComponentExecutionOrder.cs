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
				this.componentTypes.Add(componentType);
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

		private void InitSortIndex()
		{
			// Gather a list of all available component types to minimize rebuild of the sort index
			foreach (TypeInfo typeInfo in DualityApp.GetAvailDualityTypes(typeof(Component)))
				this.componentTypes.Add(typeInfo.AsType());

			// Re-generate sort indices for all relevant component types
			this.sortIndexCache.Clear();
			HashSet<int> takenSortIndices = new HashSet<int>();
			foreach (Type type in this.componentTypes)
			{
				// ToDo: Add an implementation that takes into account 
				// ExecutionOrder and RequiredComponent attributes
				int nameHash = 13;
				unchecked
				{
					// Make sure the hash is calculated in a way that lexical ordering
					// does not affect hash ordering.
					string fullName = type.FullName;
					for (int i = 0; i < fullName.Length; i++)
						nameHash = nameHash * 23 + (((int)fullName[i] * 449) % 991);
				}
				int sortIndex = nameHash % 10000;

				// No sorting index can be used twice to ensure stable sort
				// regardless of sorting algorithms used.
				while (!takenSortIndices.Add(sortIndex))
					sortIndex++;

				this.sortIndexCache.Add(type, sortIndex);
			}
		}
	}
}
