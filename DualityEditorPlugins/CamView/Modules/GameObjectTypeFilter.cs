using System;
using System.Collections.Generic;
using System.Linq;

using Duality;

namespace Duality.Editor.Plugins.CamView
{
	/// <summary>
	/// Filters <see cref="GameObject"/> instances based on the <see cref="Component"/> types they have.
	/// </summary>
	public class GameObjectTypeFilter
	{
		private HashSet<string> typeIds   = new HashSet<string>();
		private HashSet<Type>   typeCache = new HashSet<Type>();

		/// <summary>
		/// [GET] Enumerates all <see cref="Component"/> types that will match with this filter.
		/// </summary>
		public IEnumerable<Type> MatchingTypes
		{
			get
			{
				this.UpdateTypeCache();
				return this.typeCache;
			}
		}

		/// <summary>
		/// Returns true if the specified <see cref="GameObject"/> has any of the matching
		/// <see cref="Component"/> types, or if the filter is empty.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Matches(GameObject obj)
		{
			if (this.typeIds.Count == 0) return true;
			
			this.UpdateTypeCache();
			foreach (Type type in this.typeCache)
			{
				if (obj.GetComponent(type) != null)
					return true;
			}

			return false;
		}
		/// <summary>
		/// Returns true, if the specified <see cref="Type"/> is considered a match.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Matches(Type type)
		{
			this.UpdateTypeCache();
			return this.typeCache.Contains(type);
		}
		
		/// <summary>
		/// Replaces the set of matching <see cref="Component"/> types with the
		/// enumerates types.
		/// </summary>
		/// <param name="visibleObjectTypes"></param>
		public void SetMatchingTypes(IEnumerable<Type> visibleObjectTypes)
		{
			this.typeIds.Clear();
			this.typeCache.Clear();
			foreach (Type type in visibleObjectTypes)
			{
				this.typeIds.Add(type.GetTypeId());
				this.typeCache.Add(type);
			}
		}
		/// <summary>
		/// Changes whether the specified <see cref="Component"/> type is considered
		/// a match.
		/// </summary>
		/// <param name="objectType"></param>
		/// <param name="isMatch"></param>
		/// <returns></returns>
		public bool SetTypeMatches(Type objectType, bool isMatch)
		{
			string objectTypeId = objectType.GetTypeId();
			if (isMatch && this.typeIds.Add(objectTypeId))
			{
				this.typeCache.Clear();
				return true;
			}
			else if (!isMatch && this.typeIds.Remove(objectTypeId))
			{
				this.typeCache.Clear();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Clears all type-specific information in this filter. This should be done
		/// when a core plugin reload took place, so all internal <see cref="Type"/>
		/// references can be refreshed.
		/// </summary>
		public void ClearTypeCache()
		{
			this.typeCache.Clear();
		}
		private void UpdateTypeCache()
		{
			if (this.typeCache.Count > 0) return;
			if (this.typeIds.Count == 0) return;

			foreach (string typeId in this.typeIds)
			{
				Type type = ReflectionHelper.ResolveType(typeId);
				if (type != null) this.typeCache.Add(type);
			}
		}
	}
}
