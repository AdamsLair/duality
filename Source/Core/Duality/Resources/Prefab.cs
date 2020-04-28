﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Components;
using Duality.Cloning;
using Duality.Properties;
using Duality.Editor;

namespace Duality.Resources
{
	/// <summary>
	/// Prefab is short for "prefabricated object" and encapsulates a single <see cref="GameObject"/> that can serve as a template.
	/// When creating a GameObject out of a Prefab, it maintains a connection to it using a <see cref="PrefabLink"/> object. This
	/// ensures that changes made to the Prefab propagate to all of its instances as well. It also keeps track of Properties that
	/// have been deliberately modified in the editor and restores them after re-applying the original Prefabs data.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryNone)]
	[EditorHintImage(CoreResNames.ImagePrefab)]
	public class Prefab : Resource
	{
		private static readonly ApplyPrefabContext PrefabContext = new ApplyPrefabContext();
		private static readonly CloneProvider SharedPrefabProvider = new CloneProvider(PrefabContext);

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
				// Compose a list of all initializable Components in the new 
				// content, and sort them by type
				ICmpSerializeListener[] initSchedule = obj.GetComponentsDeep<ICmpSerializeListener>().ToArray();
				Component.ExecOrder.SortTypedItems(initSchedule, item => item.GetType(), false);

				// Prepare components for saving
				for (int i = initSchedule.Length - 1; i >= 0; i--)
					initSchedule[i].OnSaving();

				// Copy the new content into the Prefabs internal object
				if (this.objTree != null)
					obj.CopyTo(this.objTree);
				else
					this.objTree = obj.Clone();

				// Execute re-init code after saving
				for (int i = 0; i < initSchedule.Length; i++)
					initSchedule[i].OnSaved();

				// Cleanup any leftover prefab links that might have been copied
				this.objTree.BreakPrefabLink();

				// Prevent recursion
				foreach (GameObject child in this.objTree.GetChildrenDeep())
				{
					if (child.PrefabLink != null && child.PrefabLink.Prefab == this)
					{
						child.BreakPrefabLink();
					}
				}
			}
		}
		/// <summary>
		/// Creates a new instance of the Prefab. You will need to add it to a Scene in most cases.
		/// </summary>
		public GameObject Instantiate()
		{
			if (this.objTree == null)
				return new GameObject();
			else
				return new GameObject(new ContentRef<Prefab>(this));
		}
		/// <summary>
		/// Creates a new instance of the Prefab with specified world space transform values. 
		/// This is a convenience method that calls <see cref="Instantiate()"/> and modifies the resulting
		/// object, in case it contains a Transform Component.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="angle"></param>
		/// <param name="scale"></param>
		public GameObject Instantiate(Vector3 position, float angle = 0.0f, float scale = 1.0f)
		{
			GameObject obj = this.Instantiate();
			Transform transform = obj.Transform;
			if (transform != null)
			{
				transform.Pos = position;
				transform.Angle = angle;
				transform.Scale = scale;
			}
			return obj;
		}
		/// <summary>
		/// Copies this Prefabs data to a GameObject without linking itsself to it.
		/// </summary>
		/// <param name="obj">The GameObject to which the Prefabs data is copied.</param>
		public void CopyTo(GameObject obj)
		{
			if (this.objTree == null) return;
			SharedPrefabProvider.CopyObject(this.objTree, obj);
		}
		/// <summary>
		/// Copies a subset of this Prefabs data to a specific Component.
		/// </summary>
		/// <param name="baseObjAddress">The GameObject IndexPath to locate the source Component</param>
		/// <param name="target">The Component to which the Prefabs data is copied.</param>
		public void CopyTo(IEnumerable<int> baseObjAddress, Component target)
		{
			if (this.objTree == null) return;

			GameObject baseObj = this.objTree.GetChildAtIndexPath(baseObjAddress);
			if (baseObj == null) return;

			Component baseCmp = baseObj.GetComponent(target.GetType());
			if (baseCmp == null) return;

			SharedPrefabProvider.CopyObject(baseCmp, target);
		}

		/// <summary>
		/// Returns whether this Prefab contains a <see cref="GameObject"/> with the specified <see cref="GameObject.GetIndexPathOfChild">index path</see>.
		/// It is based on this Prefabs root GameObject.
		/// </summary>
		/// <param name="indexPath">The <see cref="GameObject.GetIndexPathOfChild">index path</see> at which to search for a GameObject.</param>
		/// <returns>True, if such child GameObjects exists, false if not.</returns>
		public bool HasGameObject(IEnumerable<int> indexPath)
		{
			return this.objTree != null && this.objTree.GetChildAtIndexPath(indexPath) != null;
		}
		/// <summary>
		/// Returns whether this Prefab contains a <see cref="Component"/> inside a GameObject with the specified <see cref="GameObject.GetIndexPathOfChild">index path</see>.
		/// It is based on this Prefabs root GameObject.
		/// </summary>
		/// <param name="gameObjIndexPath">The <see cref="GameObject.GetIndexPathOfChild">index path</see> at which to search for a GameObject.</param>
		/// <param name="cmpType">The Component type to search for inside the found GameObject.</param>
		public bool HasComponent(IEnumerable<int> gameObjIndexPath, Type cmpType)
		{
			if (this.objTree == null) return false;

			GameObject child = this.objTree.GetChildAtIndexPath(gameObjIndexPath);
			if (child == null) return false;
			return child.GetComponent(cmpType) != null;
		}

		protected override void OnLoaded()
		{
			base.OnLoaded();
			if (this.objTree != null)
			{
				this.objTree.EnsureConsistentData();
				this.objTree.EnsureComponentOrder();

				// Compose a list of all initializable Components, sort them
				// by type and then execute their init code in order.
				ICmpSerializeListener[] initSchedule = this.objTree.GetComponentsDeep<ICmpSerializeListener>().ToArray();
				Component.ExecOrder.SortTypedItems(initSchedule, item => item.GetType(), false);
				for (int i = 0; i < initSchedule.Length; i++)
					initSchedule[i].OnLoaded();
			}
		}
	}

	/// <summary>
	/// Represents a <see cref="GameObject">GameObjects</see> connection to the <see cref="Prefab"/> it has been instanciated from.
	/// </summary>
	/// <seealso cref="Prefab"/>
	/// <seealso cref="GameObject"/>
	public sealed class PrefabLink
	{
		private struct VarMod
		{
			public	PropertyInfo	prop;
			public	Type			componentType;
			public	List<int>		childIndex;
			public	object			val;

			public override string ToString()
			{
				string childStr;
				string propStr;
				string valueStr;

				if (this.childIndex == null)
					childStr = "null";
				else 
					childStr = this.childIndex.Any() ? "(" + this.childIndex.ToString(",") + ")" : "this";

				if (this.componentType != null)
					childStr += "." + this.componentType.GetTypeCSCodeName(true);

				if (this.prop != null)
					propStr = this.prop.Name;
				else
					propStr = "null";

				if (this.val != null)
					valueStr = this.val.ToString();
				else
					valueStr = "null";

				return string.Format("VarMod: {0}.{1} = {2}", childStr, propStr, valueStr);
			}
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


		private PrefabLink() : this(null, null) {}
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
			List<int> childPath = this.obj.GetIndexPathOfChild(other.obj);

			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (this.changes[i].childIndex.Take(childPath.Count).SequenceEqual(childPath))
				{
					object target;

					GameObject targetObj = this.obj.GetChildAtIndexPath(this.changes[i].childIndex);
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
			return this.DeepClone();
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
				foreach (GameObject child in this.obj.GetChildrenDeep())
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
				GameObject targetObj = this.obj.GetChildAtIndexPath(this.changes[i].childIndex);
				object target;
				if (this.changes[i].componentType != null)
					target = targetObj.GetComponent(this.changes[i].componentType);
				else
					target = targetObj;

				if (this.changes[i].prop != null && target != null) 
				{
					object applyVal = null;
					try
					{
						CloneType cloneType = CloneProvider.GetCloneType(this.changes[i].prop.PropertyType);
						if (cloneType.Type.IsValueType || cloneType.DefaultCloneBehavior != CloneBehavior.ChildObject)
							applyVal = this.changes[i].val;
						else
							applyVal = this.changes[i].val.DeepClone();

						this.changes[i].prop.SetValue(target, applyVal, null);
					}
					catch (Exception e)
					{
						Logs.Core.WriteError(
							"Error applying PrefabLink changes in {0}, property {1}:\n{2}", 
							this.obj.FullName,
							this.changes[i].prop.Name,
							LogFormat.Exception(e));
					}
				}
				else
				{
					this.changes.RemoveAt(i);
					i--;
					continue;
				}
			}
		}
		/// <summary>
		/// Updates all existing change list entries by the GameObjects current Property values.
		/// </summary>
		public void UpdateChanges()
		{
			if (this.changes == null || this.changes.Count == 0) return;

			// Remove empty changelist entries
			this.ClearEmptyChanges();

			// Update changelist values from properties
			for (int i = 0; i < this.changes.Count; i++)
			{
				GameObject targetObj = this.obj.GetChildAtIndexPath(this.changes[i].childIndex);
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
			if (prop.IsEquivalent(ReflectionInfo.Property_GameObject_Parent)) return; // Reject changing "Parent" as it would destroy the PrefabLink
			if (!prop.CanWrite) return;
			if (this.changes == null) this.changes = new List<VarMod>();

			GameObject targetObj = target as GameObject;
			Component targetComp = target as Component;
			if (targetObj == null && targetComp != null) targetObj = targetComp.gameobj;

			if (targetObj == null) 
				throw new ArgumentException("Target object is not a valid child of this PrefabLinks GameObject", "target");
			if (value == null && prop.PropertyType.GetTypeInfo().IsValueType)
				throw new ArgumentException("Target field cannot be assigned from null value.", "value");
			if (value != null && !prop.PropertyType.GetTypeInfo().IsInstanceOfType(value))
				throw new ArgumentException("Target field not assignable from Type " + value.GetType().Name + ".", "value");

			VarMod change;
			change.childIndex		= this.obj.GetIndexPathOfChild(targetObj);
			change.componentType	= (targetComp != null) ? targetComp.GetType() : null;
			change.prop				= prop;
			change.val				= value;

			this.PopChange(change.childIndex, prop, change.componentType);
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

			// Clone the changelist entry value, if required
			if (changeVal != null)
			{
				CloneType cloneType = CloneProvider.GetCloneType(changeVal.GetType());
				if (!cloneType.Type.IsValueType && cloneType.DefaultCloneBehavior == CloneBehavior.ChildObject)
				{
					changeVal = changeVal.DeepClone();
				}
			}

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

			this.PopChange(this.obj.GetIndexPathOfChild(targetObj), prop);
		}
		private void PopChange(IEnumerable<int> indexPath, PropertyInfo prop, Type componentType = null)
		{
			if (this.changes == null || this.changes.Count == 0) return;
			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (this.changes[i].prop == prop && this.changes[i].childIndex.SequenceEqual(indexPath))
				{
					if (componentType != null && this.changes[i].componentType != componentType)
						continue;

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

			List<int> indexPath = this.obj.GetIndexPathOfChild(targetObj);
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
		public void ClearChanges(GameObject targetObj, TypeInfo cmpType, PropertyInfo prop)
		{
			if (this.changes == null || this.changes.Count == 0) return;

			IEnumerable<int> indexPath = targetObj != null ? this.obj.GetIndexPathOfChild(targetObj) : null;
			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (indexPath != null && !this.changes[i].childIndex.SequenceEqual(indexPath)) continue;
				if (cmpType != null && !cmpType.IsAssignableFrom(this.changes[i].componentType.GetTypeInfo())) continue;
				if (prop != null && prop != this.changes[i].prop) continue;
				this.changes.RemoveAt(i);
			}
		}
		private void ClearEmptyChanges()
		{
			for (int i = this.changes.Count - 1; i >= 0; i--)
			{
				if (this.changes[i].prop == null)
					this.changes.RemoveAt(i);
			}
		}

		/// <summary>
		/// Returns whether a specific object is affected by this PrefabLink.
		/// </summary>
		/// <param name="cmp"></param>
		public bool AffectsObject(Component cmp)
		{
			return this.prefab.IsAvailable && this.prefab.Res.HasComponent(this.obj.GetIndexPathOfChild(cmp.GameObj), cmp.GetType());
		}
		/// <summary>
		/// Returns whether a specific object is affected by this PrefabLink.
		/// </summary>
		public bool AffectsObject(GameObject obj)
		{
			return this.prefab.IsAvailable && this.prefab.Res.HasGameObject(this.obj.GetIndexPathOfChild(obj));
		}

		/// <summary>
		/// Applies all PrefabLinks in a set of GameObjects. 
		/// </summary>
		/// <param name="objEnum">An enumeration of all GameObjects containing PrefabLinks that are to <see cref="Apply()">apply</see>.</param>
		/// <param name="predicate">An optional predicate. If set, only PrefabLinks meeting its requirements are applied.</param>
		/// <returns>A List of all PrefabLinks that have been applied.</returns>
		public static HashSet<PrefabLink> ApplyAllLinks(IEnumerable<GameObject> objEnum, Predicate<PrefabLink> predicate = null)
		{
			if (predicate == null) predicate = p => true;
			HashSet<PrefabLink> appliedLinks = new HashSet<PrefabLink>();

			var sortedQuery = from obj in objEnum
							  where obj.PrefabLink != null && predicate(obj.PrefabLink)
							  group obj by GetObjectHierarchyLevel(obj) into g
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
		private static int GetObjectHierarchyLevel(GameObject obj)
		{
			if (obj.Parent == null)
				return 0;
			else
				return GetObjectHierarchyLevel(obj.Parent) + 1;
		}
	}

	public class ApplyPrefabContext : CloneProviderContext {}
}
