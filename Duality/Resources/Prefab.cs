using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Cloning;

namespace Duality.Resources
{
	/// <summary>
	/// Prefab is short for "prefabricated object" and encapsulates a single <see cref="GameObject"/> that can serve as a template.
	/// When creating a GameObject out of a Prefab, it maintains a connection to it using a <see cref="PrefabLink"/> object. This
	/// ensures that changes made to the Prefab propagate to all of its instances as well. It also keeps track of Properties that
	/// have been deliberately modified in the editor and restores them after re-applying the original Prefabs data.
	/// </summary>
	[Serializable]
	[ExplicitResourceReference()]
	public class Prefab : Resource
	{
		/// <summary>
		/// A Prefab resources file extension.
		/// </summary>
		public new const string FileExt = ".Prefab" + Resource.FileExt;

		private	GameObject	objTree	= null;

		/// <summary>
		/// [GET] Returns whether this Prefab contains any data.
		/// </summary>
		public bool ContainsData
		{
			get { return this.objTree != null; }
		}

		/// <summary>
		/// Creates a new, empty Prefab.
		/// </summary>
		public Prefab() : this(null) 
		{

		}
		/// <summary>
		/// Creates a new Prefab out of a GameObject.
		/// </summary>
		/// <param name="obj"></param>
		public Prefab(GameObject obj)
		{
			this.Inject(obj);
		}

		/// <summary>
		/// Discards previous data and injects the specified <see cref="GameObject"/> into the Prefab.
		/// The GameObject itsself will not be affected, instead a <see cref="GameObject.Clone"/> of it
		/// will be used for the Prefab.
		/// </summary>
		/// <param name="obj">The object to inject as Prefab root object.</param>
		public void Inject(GameObject obj)
		{
			// Dispose old content
			if (obj == null)
			{
				if (this.objTree != null)
				{
					this.objTree.Dispose();
					this.objTree = null;
				}
			}
			// Inject new content
			else
			{
				obj.OnSaving(true);
				if (this.objTree != null)
					obj.CopyTo(this.objTree);
				else
					this.objTree = obj.Clone();
				obj.OnSaved(true);

				this.objTree.Parent = null;
				this.objTree.BreakPrefabLink();

				// Prevent recursion
				foreach (GameObject child in this.objTree.ChildrenDeep)
				{
					if (child.PrefabLink != null && child.PrefabLink.Prefab == this)
					{
						child.BreakPrefabLink();
					}
				}
			}
		}
		/// <summary>
		/// Instantiates the Prefab.
		/// </summary>
		/// <returns>A new GameObject instance of this Prefab.</returns>
		public GameObject Instantiate()
		{
			if (this.objTree == null)
				return new GameObject();
			else
				return new GameObject(new ContentRef<Prefab>(this));
		}
		/// <summary>
		/// Copies this Prefabs data to a GameObject without linking itsself to it.
		/// </summary>
		/// <param name="obj">The GameObject to which the Prefabs data is copied.</param>
		public void CopyTo(GameObject obj)
		{
			if (this.objTree == null) return;
			this.objTree.CopyTo(obj);
		}
		/// <summary>
		/// Copies a subset of this Prefabs data to a specific Component.
		/// </summary>
		/// <param name="baseObjAddress">The GameObject IndexPath to locate the source Component</param>
		/// <param name="target">The Component to which the Prefabs data is copied.</param>
		public void CopyTo(IEnumerable<int> baseObjAddress, Component target)
		{
			if (this.objTree == null) return;

			GameObject baseObj = this.objTree.ChildAtIndexPath(baseObjAddress);
			if (baseObj == null) return;

			Component baseCmp = baseObj.GetComponent(target.GetType());
			if (baseCmp == null) return;

			baseCmp.CopyTo(target);
		}

		/// <summary>
		/// Returns whether this Prefab contains a <see cref="GameObject"/> with the specified <see cref="GameObject.IndexPathOfChild">index path</see>.
		/// It is based on this Prefabs root GameObject.
		/// </summary>
		/// <param name="indexPath">The <see cref="GameObject.IndexPathOfChild">index path</see> at which to search for a GameObject.</param>
		/// <returns>True, if such child GameObjects exists, false if not.</returns>
		public bool HasGameObject(IEnumerable<int> indexPath)
		{
			return this.objTree != null && this.objTree.ChildAtIndexPath(indexPath) != null;
		}
		/// <summary>
		/// Returns whether this Prefab contains a <see cref="Component"/> inside a GameObject with the specified <see cref="GameObject.IndexPathOfChild">index path</see>.
		/// It is based on this Prefabs root GameObject.
		/// </summary>
		/// <param name="gameObjIndexPath">The <see cref="GameObject.IndexPathOfChild">index path</see> at which to search for a GameObject.</param>
		/// <param name="cmpType">The Component type to search for inside the found GameObject.</param>
		/// <returns></returns>
		public bool HasComponent(IEnumerable<int> gameObjIndexPath, Type cmpType)
		{
			if (this.objTree == null) return false;

			GameObject child = this.objTree.ChildAtIndexPath(gameObjIndexPath);
			if (child == null) return false;
			return child.GetComponent(cmpType) != null;
		}

		protected override void OnLoaded()
		{
			base.OnLoaded();
			if (this.objTree != null)
			{
				this.objTree.PerformSanitaryCheck();
				this.objTree.OnLoaded(true);
			}
		}
		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			Prefab c = r as Prefab;
			c.objTree = provider.RequestObjectClone(this.objTree);
		}
	}

	/// <summary>
	/// Represents a <see cref="GameObject">GameObjects</see> connection to the <see cref="Prefab"/> it has been instanciated from.
	/// </summary>
	/// <seealso cref="Prefab"/>
	/// <seealso cref="GameObject"/>
	[Serializable]
	public sealed class PrefabLink : Duality.Cloning.ICloneable
	{
		[Serializable]
		private struct VarMod
		{
			public	PropertyInfo	prop;
			public	Type			componentType;
			public	List<int>		childIndex;
			public	object			val;
		}

		private static CloneProvider changeListValueCloneProvider = null;

		static PrefabLink()
		{
			changeListValueCloneProvider = new CloneProvider();
			changeListValueCloneProvider.SetExplicitUnwrap(typeof(System.Collections.ICollection));
		}


		private	ContentRef<Prefab>	prefab;
		private	GameObject			obj;
		private	List<VarMod>		changes;


		/// <summary>
		/// [GET] The GameObject this PrefabLink belongs to.
		/// </summary>
		public GameObject Obj
		{
			get { return this.obj; }
		}
		/// <summary>
		/// [GET] The Prefab to which the GameObject is connected to.
		/// </summary>
		public ContentRef<Prefab> Prefab
		{
			get { return this.prefab; }
		}
		/// <summary>
		/// [GET] If the connected GameObject is itsself contained within a hierarchy
		/// of GameObjects which is affected by a higher PrefabLink, this link will be
		/// returned.
		/// </summary>
		/// <seealso cref="GameObject.AffectedByPrefabLink"/>
		public PrefabLink ParentLink
		{
			get
			{
				return this.obj.Parent != null ? this.obj.Parent.AffectedByPrefabLink : null;
			}
		}


		private PrefabLink() : this(null, ContentRef<Prefab>.Null) {}
		/// <summary>
		/// Creates a new PrefabLink, connecting a GameObject to a Prefab.
		/// </summary>
		/// <param name="obj">The GameObject to link.</param>
		/// <param name="prefab">The Prefab to connect the GameObject with.</param>
		public PrefabLink(GameObject obj, ContentRef<Prefab> prefab)
		{
			this.obj = obj;
			this.prefab = prefab;
		}

		/// <summary>
		/// Relocates the internal change list from this PrefabLink to a different, hierarchially lower PrefabLink.
		/// </summary>
		/// <param name="other">
		/// The PrefabLink to which to relocate changes. It needs to be hierarchially lower than
		/// this one for the relocation to succeed.
		/// </param>
		/// <remarks>
		/// <para>
		/// In general, each PrefabLink is responsible for all hierarchially lower GameObjects. If one of them has
		/// a PrefabLink on its own, then the higher PrefabLinks responsibility ends there.
		/// </para>
		/// <para>
		/// Change relocation is done when linking an existing GameObject to a Prefab although it is already affected by a
		/// hierarchially higher PrefabLink. In order to prevent both PrefabLinks to interfere with each other, 
		/// all higher PrefabLink change list entries referring to that GameObject are relocated to the new, lower 
		/// PrefabLink that is specifically targetting it.
		/// </para>
		/// <para>
		/// This way, the above responsibility guideline remains applicable.
		/// </para>
		/// </remarks>
		public void RelocateChanges(PrefabLink other)
		{
			if (this.changes == null || this.changes.Count == 0) return;
			if (!other.obj.IsChildOf(this.obj)) return;
			List<int> childPath = this.obj.IndexPathOfChild(other.obj);

			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (this.changes[i].childIndex.Take(childPath.Count).SequenceEqual(childPath))
				{
					object target;

					GameObject targetObj = this.obj.ChildAtIndexPath(this.changes[i].childIndex);
					if (this.changes[i].componentType != null)
						target = targetObj.GetComponent(this.changes[i].componentType);
					else
						target = targetObj;

					other.PushChange(target, this.changes[i].prop, this.changes[i].val);
					this.changes.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Clones the PrefabLink, but targets a different GameObject and Prefab.
		/// </summary>
		/// <param name="newObj">The GameObject which the clone is connected to.</param>
		/// <param name="newPrefab">The Prefab which the clone will connect its GameObject to.</param>
		/// <returns>A cloned version of this PrefabLink</returns>
		public PrefabLink Clone(GameObject newObj, ContentRef<Prefab> newPrefab)
		{
			PrefabLink clone = this.Clone();
			clone.obj = newObj;
			clone.prefab = newPrefab;
			return clone;
		}
		/// <summary>
		/// Clones the PrefabLink, but targets a different GameObject.
		/// </summary>
		/// <param name="newObj">The GameObject which the clone is connected to.</param>
		/// <returns>A cloned version of this PrefabLink</returns>
		public PrefabLink Clone(GameObject newObj)
		{
			PrefabLink clone = this.Clone();
			clone.obj = newObj;
			return clone;
		}
		/// <summary>
		/// Clones the PrefabLink.
		/// </summary>
		/// <returns>A cloned version of this PrefabLink</returns>
		public PrefabLink Clone()
		{
			return Duality.Cloning.CloneProvider.DeepClone(this);
		}
		
		/// <summary>
		/// Applies both Prefab and change list to this PrefabLinks GameObject.
		/// </summary>
		public void Apply()
		{
			this.Apply(true);
		}
		private void Apply(bool deep)
		{
			this.ApplyPrefab();
			this.ApplyChanges();

			// Lower prefab links later
			if (deep)
			{
				foreach (GameObject child in this.obj.ChildrenDeep.ToArray())
				{
					if (child.PrefabLink != null && child.PrefabLink.ParentLink == this)
						child.PrefabLink.Apply(true);
				}
			}
		}
		/// <summary>
		/// Applies the Prefab to this PrefabLinks GameObject. This will overwrite
		/// all of its existing data and establish the state as defined in the Prefab.
		/// </summary>
		public void ApplyPrefab()
		{
			if (!this.prefab.IsAvailable) return;
			if (!this.prefab.Res.ContainsData) return;
			this.prefab.Res.CopyTo(this.obj);
		}
		/// <summary>
		/// Applies this PrefabLinks change list to its GameObject. This will restore
		/// all deliberate modifications (made in the editor) of the GameObjects Properties 
		/// after linking it to the Prefab.
		/// </summary>
		public void ApplyChanges()
		{
			if (this.changes == null || this.changes.Count == 0) return;

			for (int i = 0; i < this.changes.Count; i++)
			{
				GameObject targetObj = this.obj.ChildAtIndexPath(this.changes[i].childIndex);
				object target;
				if (this.changes[i].componentType != null)
					target = targetObj.GetComponent(this.changes[i].componentType);
				else
					target = targetObj;

				if (this.changes[i].prop != null) 
				{
					try
					{
						this.changes[i].prop.SetValue(target, this.changes[i].val, null);
					}
					catch (Exception e)
					{
						Log.Core.WriteError(
							"Error applying PrefabLink changes in {0}, property {1}:\n{2}", 
							this.obj.FullName,
							this.changes[i].prop.Name,
							Log.Exception(e));
					}
				}
			}
		}
		/// <summary>
		/// Updates all existing change list entries by the GameObjects current Property values.
		/// </summary>
		public void UpdateChanges()
		{
			if (this.changes == null || this.changes.Count == 0) return;

			for (int i = 0; i < this.changes.Count; i++)
			{
				GameObject targetObj = this.obj.ChildAtIndexPath(this.changes[i].childIndex);
				object target;
				if (this.changes[i].componentType != null)
					target = targetObj.GetComponent(this.changes[i].componentType);
				else
					target = targetObj;

				VarMod modTmp = this.changes[i];
				modTmp.val = this.changes[i].prop.GetValue(target, null);
				this.changes[i] = modTmp;
			}
		}

		/// <summary>
		/// Creates a new change list entry.
		/// </summary>
		/// <param name="target">The target object in which the change has been made. Must be a GameObject or Component.</param>
		/// <param name="prop">The target objects <see cref="System.Reflection.PropertyInfo">Property</see> that has been changed.</param>
		/// <param name="value">The value to which the specified Property has been changed to.</param>
		public void PushChange(object target, PropertyInfo prop, object value)
		{
			if (ReflectionHelper.MemberInfoEquals(prop, ReflectionInfo.Property_GameObject_Parent)) return; // Reject changing "Parent" as it would destroy the PrefabLink
			if (!prop.CanWrite) return;
			if (this.changes == null) this.changes = new List<VarMod>();

			GameObject targetObj = target as GameObject;
			Component targetComp = target as Component;
			if (targetObj == null && targetComp != null) targetObj = targetComp.gameobj;

			if (targetObj == null) 
				throw new ArgumentException("Target object is not a valid child of this PrefabLinks GameObject", "target");
			if (value == null && prop.PropertyType.IsValueType)
				throw new ArgumentException("Target field cannot be assigned from null value.", "value");
			if (value != null && !prop.PropertyType.IsInstanceOfType(value))
				throw new ArgumentException("Target field not assignable from Type " + value.GetType().Name + ".", "value");

			VarMod change;
			change.childIndex		= this.obj.IndexPathOfChild(targetObj);
			change.componentType	= (targetComp != null) ? targetComp.GetType() : null;
			change.prop				= prop;
			change.val				= value;

			this.PopChange(change.childIndex, prop);
			this.changes.Add(change);
		}
		/// <summary>
		/// Creates a new change list entry.
		/// </summary>
		/// <param name="target">The target object in which the change has been made. Must be a GameObject or Component.</param>
		/// <param name="prop">The target objects <see cref="System.Reflection.PropertyInfo">Property</see> that has been changed.</param>
		public void PushChange(object target, PropertyInfo prop)
		{
			if (!prop.CanWrite || !prop.CanRead) return;
			object changeVal = prop.GetValue(target, null);

			// Clone the changelist entry value
			changeListValueCloneProvider.ClearObjectMap();
			changeVal = changeListValueCloneProvider.RequestObjectClone(changeVal);

			this.PushChange(target, prop, changeVal);
		}
		/// <summary>
		/// Removes an existing change list entry.
		/// </summary>
		/// <param name="target">The target object in which the change has been made. Must be a GameObject or Component.</param>
		/// <param name="prop">The target objects <see cref="System.Reflection.PropertyInfo">Property</see> that has been changed.</param>
		public void PopChange(object target, PropertyInfo prop)
		{
			GameObject targetObj = target as GameObject;
			Component targetComp = target as Component;
			if (targetObj == null && targetComp != null) targetObj = targetComp.gameobj;

			if (targetObj == null) 
				throw new ArgumentException("Target object is not a valid child of this PrefabLinks GameObject", "target");

			this.PopChange(this.obj.IndexPathOfChild(targetObj), prop);
		}
		private void PopChange(IEnumerable<int> indexPath, PropertyInfo prop)
		{
			if (this.changes == null || this.changes.Count == 0) return;
			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (this.changes[i].prop == prop && this.changes[i].childIndex.SequenceEqual(indexPath))
				{
					this.changes.RemoveAt(i);
					break;
				}
			}
		}
		/// <summary>
		/// Returns whether there is a specific change list entry.
		/// </summary>
		/// <param name="target">The target object in which the change has been made. Must be a GameObject or Component.</param>
		/// <param name="prop">The target objects <see cref="System.Reflection.PropertyInfo">Property</see> that has been changed.</param>
		/// <returns>True, if such change list entry exists, false if not.</returns>
		public bool HasChange(object target, PropertyInfo prop)
		{
			if (this.changes == null || this.changes.Count == 0) return false;

			GameObject targetObj = target as GameObject;
			Component targetComp = target as Component;
			if (targetObj == null && targetComp != null) targetObj = targetComp.gameobj;

			if (targetObj == null) 
				throw new ArgumentException("Target object is not a valid child of this PrefabLinks GameObject", "target");

			List<int> indexPath = this.obj.IndexPathOfChild(targetObj);
			for (int i = 0; i < this.changes.Count; i++)
			{
				if (this.changes[i].childIndex.SequenceEqual(indexPath) && this.changes[i].prop == prop)
					return true;
			}

			return false;
		}
		/// <summary>
		/// Clears the change list.
		/// </summary>
		public void ClearChanges()
		{
			if (this.changes != null) this.changes.Clear();
		}
		/// <summary>
		/// Clears the change list for certain objects
		/// </summary>
		public void ClearChanges(GameObject targetObj, Type cmpType, PropertyInfo prop)
		{
			if (this.changes == null || this.changes.Count == 0) return;

			IEnumerable<int> indexPath = targetObj != null ? this.obj.IndexPathOfChild(targetObj) : null;
			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (indexPath != null && !this.changes[i].childIndex.SequenceEqual(indexPath)) continue;
				if (cmpType != null && !cmpType.IsAssignableFrom(this.changes[i].componentType)) continue;
				if (prop != null && prop != this.changes[i].prop) continue;
				this.changes.RemoveAt(i);
			}
		}

		/// <summary>
		/// Returns whether a specific object is affected by this PrefabLink.
		/// </summary>
		/// <param name="cmp"></param>
		/// <returns></returns>
		public bool AffectsObject(Component cmp)
		{
			return this.prefab.IsAvailable && this.prefab.Res.HasComponent(this.obj.IndexPathOfChild(cmp.GameObj), cmp.GetType());
		}
		/// <summary>
		/// Returns whether a specific object is affected by this PrefabLink.
		/// </summary>
		/// <param name="cmp"></param>
		/// <returns></returns>
		public bool AffectsObject(GameObject obj)
		{
			return this.prefab.IsAvailable && this.prefab.Res.HasGameObject(this.obj.IndexPathOfChild(obj));
		}

		/// <summary>
		/// Applies all PrefabLinks in a set of GameObjects. 
		/// </summary>
		/// <param name="objEnum">An enumeration of all GameObjects containing PrefabLinks that are to <see cref="Apply()">apply</see>.</param>
		/// <param name="predicate">An optional predicate. If set, only PrefabLinks meeting its requirements are applied.</param>
		/// <returns>A List of all PrefabLinks that have been applied.</returns>
		public static List<PrefabLink> ApplyAllLinks(IEnumerable<GameObject> objEnum, Predicate<PrefabLink> predicate = null)
		{
			if (predicate == null) predicate = p => true;
			List<PrefabLink> appliedLinks = new List<PrefabLink>();

			var sortedQuery = from obj in objEnum
							  where obj.PrefabLink != null && predicate(obj.PrefabLink)
							  group obj by obj.HierarchyLevel into g
							  orderby g.Key
							  select g;

			foreach (var group in sortedQuery)
				foreach (GameObject obj in group)
				{
					obj.PrefabLink.Apply();
					appliedLinks.Add(obj.PrefabLink);
				}

			return appliedLinks;
		}

		void Cloning.ICloneable.CopyDataTo(object targetObj, Cloning.CloneProvider provider)
		{
			PrefabLink castObj = targetObj as PrefabLink;

			castObj.prefab = this.prefab;
			castObj.obj = this.obj;
			castObj.changes = null;

			if (this.changes != null)
			{
				castObj.changes = new List<VarMod>(this.changes.Count);
				for (int i = 0; i < this.changes.Count; i++)
				{
					VarMod newVarMod = this.changes[i];
					newVarMod.childIndex = new List<int>(newVarMod.childIndex);
					castObj.changes.Add(newVarMod);
				}
			}
		}
	}
}
