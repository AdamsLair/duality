using System;
using System.Collections.Generic;
using System.Linq;
using Duality;

namespace Duality.Editor
{
	public class ObjectSelection : IEquatable<ObjectSelection>, IEnumerable<object>
	{
		[Flags]
		public enum Category
		{
			None		= 0x0,

			Other		= 0x1,
			GameObjCmp	= 0x2,
			Resource	= 0x4,

			All			= Other | GameObjCmp | Resource
		}
		public static Category GetObjCategory(object obj)
		{
			if (obj is GameObject) return Category.GameObjCmp;
			else if (obj is Component) return Category.GameObjCmp;
			else if (obj is Resource) return Category.Resource;
			else return Category.Other;
		}

		public static readonly ObjectSelection Null	= new ObjectSelection();

		private	List<object>	obj	= null;
		private Category		cat	= Category.None;

		public Category Categories
		{
			get { return this.cat; }
		}
		public bool Empty
		{
			get { return this.obj.Count == 0; }
		}

		public object MainObject
		{
			get { return this.obj.FirstOrDefault(); }
		}
		public GameObject MainGameObject
		{
			get { return this.GameObjects.FirstOrDefault(); }
		}
		public Component MainComponent
		{
			get { return this.Components.FirstOrDefault(); }
		}
		public Resource MainResource
		{
			get { return this.Resources.FirstOrDefault(); }
		}
		public object MainOtherObject
		{
			get { return this.OtherObjects.FirstOrDefault(); }
		}

		public IEnumerable<object> Objects
		{
			get { return this.obj; }
		}
		public IEnumerable<GameObject> GameObjects
		{
			get
			{
				return from o in this.obj
					   where o is GameObject
					   select o as GameObject;
			}
		}
		public IEnumerable<Component> Components
		{
			get
			{
				return from o in this.obj
					   where o is Component
					   select o as Component;
			}
		}
		public IEnumerable<Resource> Resources
		{
			get
			{
				return from o in this.obj
					   where o is Resource
					   select o as Resource;
			}
		}
		public IEnumerable<object> OtherObjects
		{
			get { return this.obj.Where(o => !(o is GameObject) && !(o is Component) && !(o is Resource)); }
		}

		public int ObjectCount
		{
			get { return this.obj.Count; }
		}
		public int GameObjectCount
		{
			get { return this.obj.Count(o => o is GameObject); }
		}
		public int ComponentCount
		{
			get { return this.obj.Count(o => o is Component); }
		}
		public int ResourceCount
		{
			get { return this.obj.Count(o => o is Resource); }
		}
		public int OtherObjectCount
		{
			get { return this.OtherObjects.Count(); }
		}

		public ObjectSelection()
		{
			this.obj = new List<object>();
		}
		public ObjectSelection(ObjectSelection other)
		{
			this.obj = new List<object>(other.obj);
			this.cat = other.cat;
		}
		public ObjectSelection(IEnumerable<object> obj)
		{
			this.obj = new List<object>(obj.NotNull());
			this.UpdateCategories();
		}
		public ObjectSelection(params GameObject[] obj) : this(obj as IEnumerable<object>) {}
		public ObjectSelection(params Component[] obj) : this(obj as IEnumerable<object>) {}
		public ObjectSelection(params Resource[] obj) : this(obj as IEnumerable<object>) {}
		
		protected void LocalExclusive(Category singleCat)
		{
			this.LocalClear(Category.All & ~singleCat);
		}
		protected void LocalClear()
		{
			this.obj.Clear();
			this.UpdateCategories();
		}
		protected void LocalClear(Category clearCat)
		{
			this.obj.RemoveAll(o => (GetObjCategory(o) & clearCat) != Category.None);	
			this.UpdateCategories();
		}
		protected void LocalClear(Predicate<object> clearPred)
		{
			this.obj.RemoveAll(clearPred);	
			this.UpdateCategories();
		}
		protected void LocalTransform(ObjectSelection target)
		{
			// Group source objects by their selection category
			var typedQuerySrc = 
				from o in this.obj
				group o by GetObjCategory(o) into g
				select new { ObjType = g.Key, Obj = g };

			// Iterate through currently existent categories
			Category clearCat = Category.None;
			foreach (var categoryGroup in typedQuerySrc)
			{
				// If any object of a currently available category is present in the target: Deselect current category
				if (target.obj.Any(o => GetObjCategory(o) == categoryGroup.ObjType))
					clearCat |= categoryGroup.ObjType;				
			}
			this.LocalClear(clearCat);

			// Append new selection
			this.LocalAppend(target);
		}
		protected void LocalAppend(ObjectSelection other)
		{
			this.obj = new List<object>(this.obj.Union(other.obj));
			this.UpdateCategories();
		}
		protected void LocalRemove(ObjectSelection other)
		{
			this.obj = new List<object>(this.obj.Except(other.obj));
			this.UpdateCategories();
		}
		protected void LocalToggle(ObjectSelection other)
		{
			var common = this.obj.Intersect(other.obj);
			var added = other.obj.Except(this.obj);
			this.obj = new List<object>(this.obj.Except(common).Union(added));
			this.UpdateCategories();
		}
		protected void LocalClearDisposed()
		{
			this.LocalClear(o => 
			{
				if (o == null) return true;

				IManageableObject manObj = o as IManageableObject;
				if (manObj != null && manObj.Disposed) return true;

				Resource resObj = o as Resource;
				if (resObj != null && resObj.Disposed) return true;

				return false;
			});
		}
		protected void LocalHierarchyExpand()
		{
			var gameobjQuery = this.GameObjects.Concat(this.GameObjects.ChildrenDeep());
			var componentQuery = this.GameObjects.GetComponentsDeep<Component>();
			var gameObjComponentQuery = gameobjQuery.AsEnumerable<object>().Concat(componentQuery.AsEnumerable<object>()).Distinct();
			this.obj = new List<object>(gameObjComponentQuery.Concat(this.Resources).Concat(this.OtherObjects));
			this.UpdateCategories();
		}

		public ObjectSelection Exclusive(Category singleCat)
		{
			return this.Clear(Category.All & ~singleCat);
		}
		public ObjectSelection Clear(Category clearCat)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalClear(clearCat);
			return result;
		}
		public ObjectSelection Clear(Predicate<object> clearPred)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalClear(clearPred);
			return result;
		}
		public ObjectSelection Transform(ObjectSelection target)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalTransform(target);
			return result;
		}
		public ObjectSelection Append(ObjectSelection other)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalAppend(other);
			return result;
		}
		public ObjectSelection Remove(ObjectSelection other)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalRemove(other);
			return result;
		}
		public ObjectSelection Toggle(ObjectSelection other)
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalToggle(other);
			return result;
		}
		public ObjectSelection ClearDisposed()
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalClearDisposed();
			return result;
		}
		public ObjectSelection HierarchyExpand()
		{
			ObjectSelection result = new ObjectSelection(this);
			result.LocalHierarchyExpand();
			return result;
		}
		public Type GetSharedType()
		{
			var query = 
				from o in this.obj
				select new { Obj = o, Type = o.GetType() };
			var entry = query.OrderBy(e => e.Type.GetTypeHierarchyLevel()).LastOrDefault();
			if (entry == null) return null;

			Type t = entry.Type;
			while (t != typeof(object) && !this.obj.All(o => t.IsInstanceOfType(o)))
				t = t.BaseType;

			return t;
		}
		
		protected void UpdateCategories()
		{
			Category catAvail = Category.None;
			for (int i = 0, catId = 0; (catId = (1 << i)) < (int)Category.All; i++)
			{
				Category curCat = (Category)catId;
				if (this.obj.Any(o => GetObjCategory(o) == curCat))
				{
					catAvail |= curCat;
				}
			}
			this.cat = catAvail;
		}

		public override int GetHashCode()
		{
			int h = 0;
			for (int i = 0; i < this.obj.Count; i++) h = h ^ (this.obj[i] != null ? this.obj[i].GetHashCode() : 0);
			return h;
		}
		public override bool Equals(object obj)
		{
			if (obj is ObjectSelection) return this == (obj as ObjectSelection);

			return base.Equals(obj);
		}
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.cat, this.obj.Count);
		}
		public bool Equals(ObjectSelection other)
		{
			return this == other;
		}

		public static int GetTypeShareLevel(ObjectSelection first, ObjectSelection second)
		{
			if (first == null || second == null) return -1;
			Type firstType = first.GetSharedType();
			Type secondType = second.GetSharedType();
			if (firstType == null || secondType == null) return -1;
			if (firstType.GetTypeHierarchyLevel() < secondType.GetTypeHierarchyLevel()) MathF.Swap(ref firstType, ref secondType);

			int level = 0;
			while (firstType != secondType && !firstType.IsAssignableFrom(secondType))
			{
				level++;
				firstType = firstType.BaseType;
			}
			return firstType == typeof(object) ? int.MaxValue : level;
		}
		public static Category GetAffectedCategories(ObjectSelection first, ObjectSelection second)
		{
			if (first == null && second == null) return Category.None;
			if (first == null) return second.Categories;
			if (second == null) return first.Categories;

			Category catDiff = Category.None;
			for (int i = 0, catId = 0; (catId = (1 << i)) < (int)Category.All; i++)
			{
				Category curCat = (Category)catId;
				var firstCatQuery = first.obj.Where(o => GetObjCategory(o) == curCat);
				var secondCatQuery = second.obj.Where(o => GetObjCategory(o) == curCat);

				int firstCount = firstCatQuery.Count();
				int secondCount = secondCatQuery.Count();
				if (firstCount != secondCount)
				{
					catDiff |= curCat;
					continue;
				}

				var unionQuery = firstCatQuery.Union(secondCatQuery);
				if (unionQuery.Count() != firstCount) catDiff |= curCat;
			}
			return catDiff;
		}
		public static IEnumerable<Category> EnumerateCategories(Category cat)
		{
			for (int i = 0, catId = 0; (catId = (1 << i)) < (int)Category.All; i++)
			{
				Category curCat = (Category)catId;
				if (cat.HasFlag(curCat)) yield return curCat;
			}
		}

		public static bool operator ==(ObjectSelection first, ObjectSelection second)
		{
			if (object.ReferenceEquals(first, second)) return true;
			if (object.ReferenceEquals(first, null) || object.ReferenceEquals(second, null)) return false;

			if (first.cat != second.cat) return false;
			return first.obj.SetEqual(second.obj);
		}
		public static bool operator !=(ObjectSelection first, ObjectSelection second)
		{
			return !(first == second);
		}

		public static ObjectSelection operator -(ObjectSelection first, ObjectSelection second)
		{
			return new ObjectSelection(first.obj.Except(second.obj));
		}
		public static ObjectSelection operator +(ObjectSelection first, ObjectSelection second)
		{
			return new ObjectSelection(first.obj.Concat(second.obj).Distinct());
		}

		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			return this.obj.GetEnumerator();
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.obj.GetEnumerator();
		}
	}
}
