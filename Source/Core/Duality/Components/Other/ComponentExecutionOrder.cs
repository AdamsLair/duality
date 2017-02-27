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
		private struct IndexedType
		{
			public Type Type;
			public int Index;
		}

		private Dictionary<Type,int> sortIndexCache = new Dictionary<Type,int>();
		private IndexedType[] sortedComponentTypes = new IndexedType[0];
		private HashSet<Type> componentTypes = new HashSet<Type>();


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
			this.sortedComponentTypes = new IndexedType[0];
			this.componentTypes.Clear();
		}

		private void InitSortIndex()
		{
			// Re-generate sort indices for all relevant component types
			this.sortIndexCache.Clear();
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
				this.sortIndexCache.Add(type, sortIndex);
			}

			// Create a sorted list of component types
			this.sortedComponentTypes = new IndexedType[this.componentTypes.Count];
			int arrayIndex = 0;
			foreach (var pair in this.sortIndexCache)
			{
				this.sortedComponentTypes[arrayIndex].Type = pair.Key;
				this.sortedComponentTypes[arrayIndex].Index = pair.Value;
				arrayIndex++;
			}
			Array.Sort(this.sortedComponentTypes, (a, b) => a.Index - b.Index);
		}
	}
}
