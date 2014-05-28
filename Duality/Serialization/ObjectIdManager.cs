using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Duality.Serialization
{
	/// <summary>
	/// Manages object IDs during de/serialization.
	/// </summary>
	public class ObjectIdManager
	{
		/// <summary>
		/// Compares two objects for equality strictly by reference. This is needed to build
		/// the object id mapping, since some objects may expose some unfortunate equality behavior,
		/// and we really want to distinguish different objects by reference, and not by "content" here.
		/// </summary>
		private class ReferenceEqualityComparer : IEqualityComparer<object>
		{
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return obj != null ? obj.GetHashCode() : 0;
			}
		}

		private const int BaseId = 129723834;

		private	int							idLevel			= 0;
		private	List<uint>					idGenSeed		= new List<uint> { 0 };
		private	List<uint>					idStack			= new List<uint> { 0 };
		private	int							idStackHash		= 0;
		private	Dictionary<object,uint>		objRefIdMap		= new Dictionary<object,uint>(new ReferenceEqualityComparer());
		private	Dictionary<uint,object>		idObjRefMap		= new Dictionary<uint,object>();
		private	Dictionary<Type,uint>		typeHashCache	= new Dictionary<Type,uint>();

		/// <summary>
		/// Clears all object id mappings.
		/// </summary>
		public void Clear()
		{
			this.typeHashCache.Clear();
			this.objRefIdMap.Clear();
			this.idObjRefMap.Clear();
			this.idGenSeed.Clear();
			this.idGenSeed.Add(0);
			this.idStack.Clear();
			this.idStack.Add(0);
			this.idLevel = 0;
		}
		/// <summary>
		/// Returns the id that is assigned to the specified object. Assigns one, if
		/// there is none yet.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="isNewId"></param>
		/// <returns></returns>
		public uint Request(object obj, out bool isNewId)
		{
			uint id;
			if (this.objRefIdMap.TryGetValue(obj, out id))
			{
				isNewId = false;
				return id;
			}

			// Choose a base value for the object id
			bool isGeneratedId = false;
			if (obj is IUniqueIdentifyable)
			{
				id = (obj as IUniqueIdentifyable).PreferredId;
				isGeneratedId = true;
			}
			else
			{
				id = this.idGenSeed[this.idLevel];
			}

			// Don't allow zero-ids
			if (id == 0) id = BaseId;

			// Make sure it doesn't collide
			unchecked
			{
				const uint p = 16777619;
				uint idLevelHash = (uint)this.idStackHash;

				while (this.idObjRefMap.ContainsKey(id))
				{
					id = (id ^ idLevelHash) * p;
				}
			}

			// When using the id generator, keep the current seed in mind.
			if (!isGeneratedId)
				this.idGenSeed[this.idLevel] = id;
			this.idStack[this.idLevel] = id;

			this.objRefIdMap[obj] = id;
			this.idObjRefMap[id] = obj;

			isNewId = true;
			return id;
		}
		/// <summary>
		/// Assigns an id to a specific object.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="id">The id to assign. Zero ids are rejected.</param>
		public void Inject(object obj, uint id)
		{
			if (id == 0) return;

			if (obj != null) this.objRefIdMap[obj] = id;
			this.idObjRefMap[id] = obj;
		}
		/// <summary>
		/// Tries to lookup an object based on its id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Lookup(uint id, out object obj)
		{
			return this.idObjRefMap.TryGetValue(id, out obj);
		}

		/// <summary>
		/// Increases the reference hierarchy level of the object id generator. Each level of id generation uses its own algorithm, so different levels of ids are unlikely to affect each other.
		/// </summary>
		public void PushIdLevel()
		{
			this.idGenSeed.Add(0);
			this.idStack.Add(0);
			this.idLevel++;
			this.idStackHash = this.idStack.GetCombinedHashCode(0, this.idLevel);
		}
		/// <summary>
		/// Decreases the reference hierarchy level of the object id generator. Each level of id generation uses its own algorithm, so different levels of ids are unlikely to affect each other.
		/// </summary>
		public void PopIdLevel()
		{
			if (this.idLevel == 0) throw new InvalidOperationException("Can't pop persistent id level, because it is already zero / root");
			this.idGenSeed.RemoveAt(this.idLevel);
			this.idStack.RemoveAt(this.idLevel);
			this.idLevel--;
			this.idStackHash = this.idStack.GetCombinedHashCode(0, this.idLevel);
		}
	}
}
