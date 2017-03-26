using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Resources;
using Duality.Cloning;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Properties;
using Duality.Serialization;

namespace Duality
{
	/// <summary>
	/// GameObjects are what every <see cref="Duality.Resources.Scene"/> consists of. They represent nodes in the hierarchial scene graph and
	/// can maintain a <see cref="Duality.Resources.PrefabLink"/> connection. A GameObject's main duty is to group several <see cref="Component"/>s
	/// to form one logical instance of an actual object as part of the game, such as "Car" or "PlayerCharacter". However,
	/// the GameObjects itsself does not contain any game-related logic and, by default, doesn't even occupy a position in space.
	/// This is the job of its Components.
	/// </summary>
	/// <seealso cref="Component"/>
	/// <seealso cref="Duality.Resources.Scene"/>
	/// <seealso cref="Duality.Resources.PrefabLink"/>
	[CloneBehavior(CloneBehavior.Reference)]
	[EditorHintCategory(CoreResNames.CategoryNone)]
	[EditorHintImage(CoreResNames.ImageGameObject)]
	public sealed class GameObject : IManageableObject, IUniqueIdentifyable, ICloneExplicit
	{
		private static readonly GameObject[] EmptyChildren = new GameObject[0];

		[DontSerialize] 
		private		Scene						scene		= null;
		private		GameObject					parent		= null;
		private		PrefabLink					prefabLink	= null;
		private		Guid						identifier	= Guid.NewGuid();
		private		List<GameObject>			children	= null;
		private		List<Component>				compList	= new List<Component>();
		private		Dictionary<Type,Component>	compMap		= new Dictionary<Type,Component>();
		private		string						name		= string.Empty;
		private		bool						active		= true;
		private		InitState					initState	= InitState.Initialized;

		// Built-in heavily used component lookup
		private		Transform					compTransform	= null;
		
		[DontSerialize]
		private EventHandler<GameObjectParentChangedEventArgs>	eventParentChanged		= null;
		[DontSerialize]
		private EventHandler<ComponentEventArgs>				eventComponentAdded		= null;
		[DontSerialize]
		private EventHandler<ComponentEventArgs>				eventComponentRemoving	= null;


		/// <summary>
		/// [GET / SET] This GameObject's parent object in the scene graph.
		/// A GameObject usually depends on its parent in some way, such as being
		/// positioned relative to it when occupying a position in space.
		/// </summary>
		public GameObject Parent
		{
			get { return this.parent; }
			set
			{
				if (this.parent != value)
				{
					// Consistency checks. Do not allow closed parent-child loops.
					if (value != null)
					{
						if (this == value) throw new ArgumentException("Can't parent a GameObject to itself.");
						if (value.IsChildOf(this)) throw new ArgumentException("Can't parent a GameObject to one of its children.");
					}

					GameObject oldParent = this.parent;
					Scene newScene = (value != null) ? value.scene : this.scene;

					if (this.parent != null) this.parent.children.Remove(this);
					if (newScene != this.scene)
					{
						if (this.scene != null) this.scene.RemoveObject(this);
						if (newScene != null) newScene.AddObject(this);
					}
					this.parent = value;
					if (this.parent != null)
					{
						if (this.parent.children == null) this.parent.children = new List<GameObject>();
						this.parent.children.Add(this);
					}

					this.OnParentChanged(oldParent, this.parent);
				}
			}
		}
		/// <summary>
		/// [GET] The GameObjects parent <see cref="Duality.Resources.Scene"/>. Each GameObject can belong to
		/// exactly one Scene, or no Scene at all. To add or remove GameObjects to / from a Scene, use the <see cref="Duality.Resources.Scene.AddObject(Duality.GameObject)"/> and
		/// <see cref="Duality.Resources.Scene.RemoveObject(Duality.GameObject)"/> methods.
		/// </summary>
		public Scene ParentScene
		{
			get { return this.scene; }
			internal set { this.scene = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the GameObject is currently active. To return true,
		/// both the GameObject itsself and all of its parent GameObjects need to be active.
		/// </summary>
		/// <seealso cref="ActiveSingle"/>
		public bool Active
		{
			get { return this.ActiveSingle && (this.parent == null || this.parent.Active); }
			set { this.ActiveSingle = value; }
		}
		/// <summary>
		/// [GET / SET] Whether or not the GameObject is currently active. Unlike <see cref="Active"/>,
		/// this property ignores parent activation states and depends only on this single GameObject.
		/// The scene graph and other Duality instances usually check <see cref="Active"/>, not ActiveSingle.
		/// </summary>
		/// <seealso cref="Active"/>
		public bool ActiveSingle
		{
			get { return this.active && this.initState.IsActive(); }
			set 
			{ 
				if (this.active != value)
				{
					if (this.scene != null && this.scene.IsCurrent)
					{
						List<ICmpInitializable> initList = new List<ICmpInitializable>();
						bool hasChildren = this.children != null && this.children.Count > 0;
						this.GatherInitComponents(initList, hasChildren);
						if (value)
						{
							if (hasChildren)
								Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), false);
							foreach (ICmpInitializable component in initList)
								component.OnInit(Component.InitContext.Activate);
						}
						else
						{
							if (hasChildren)
								Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), true);
							else
								initList.Reverse();
							foreach (ICmpInitializable component in initList)
								component.OnShutdown(Component.ShutdownContext.Deactivate);
						}
					}

					this.active = value;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The name of this GameObject.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set { this.name = string.IsNullOrWhiteSpace(value) ? "null" : value; }
		}
		/// <summary>
		/// [GET] The path-like hierarchial name of this GameObject.
		/// </summary>
		/// <example>For an object called <c>Wheel</c> inside an object called <c>Car</c>, this would return <c>Car/Wheel</c>.</example>
		public string FullName
		{
			get { return (this.parent != null) ? this.parent.FullName + '/' + this.name : this.name; }
		}
		/// <summary>
		/// [GET] The GameObjects persistent globally unique identifier.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Guid Id
		{
			get { return this.identifier; }
			internal set { this.identifier = value; }
		}
		/// <summary>
		/// [GET] Enumerates this objects child GameObjects.
		/// </summary>
		public IReadOnlyList<GameObject> Children
		{
			get { return this.children ?? EmptyChildren as IReadOnlyList<GameObject>; }
		}
		/// <summary>
		/// [GET] The <see cref="Duality.Resources.PrefabLink"/> that connects this object to a <see cref="Duality.Resources.Prefab"/>.
		/// </summary>
		/// <seealso cref="Duality.Resources.PrefabLink"/>
		/// <seealso cref="Duality.Resources.Prefab"/>
		public PrefabLink PrefabLink
		{
			get { return this.prefabLink; }
			internal set { this.prefabLink = value; }
		}
		/// <summary>
		/// [GET] The <see cref="Duality.Resources.PrefabLink"/> that connects this object or one or its parent GameObjects to a <see cref="Duality.Resources.Prefab"/>.
		/// </summary>
		/// <remarks>
		/// This does not necessarily mean that this GameObject will be affected by the PrefabLink, since it might not be part of
		/// the linked Prefab. It simply indicates the returned PrefabLink's potential to adjust this GameObject when being applied.
		/// </remarks>
		/// <seealso cref="Duality.Resources.PrefabLink"/>
		/// <seealso cref="Duality.Resources.Prefab"/>
		public PrefabLink AffectedByPrefabLink
		{
			get
			{
				if (this.prefabLink != null) return this.prefabLink;
				else if (this.parent != null) return this.parent.AffectedByPrefabLink;
				else return null;
			}
		}
		/// <summary>
		/// [GET] Returns whether this GameObject has been disposed. Disposed GameObjects are not to be used and should
		/// be treated specifically or as null references by your code.
		/// </summary>
		public bool Disposed
		{
			get { return this.initState == InitState.Disposed; }
		}

		/// <summary>
		/// [GET] The GameObject's <see cref="Duality.Components.Transform"/> Component, if existing.
		/// This is a cached / faster shortcut-version of <see cref="GetComponent{T}"/>.
		/// </summary>
		/// <seealso cref="Duality.Components.Transform"/>
		public Transform Transform
		{
			get { return this.compTransform; }
		}

		uint IUniqueIdentifyable.PreferredId
		{
			get { unchecked { return (uint)this.identifier.GetHashCode(); } }
		}
		
		
		/// <summary>
		/// Fired when this GameObjects parent has changed
		/// </summary>
		public event EventHandler<GameObjectParentChangedEventArgs>	EventParentChanged
		{
			add { this.eventParentChanged += value; }
			remove { this.eventParentChanged -= value; }
		}
		/// <summary>
		/// Fired when a Component has been added to the GameObject
		/// </summary>
		public event EventHandler<ComponentEventArgs>				EventComponentAdded
		{
			add { this.eventComponentAdded += value; }
			remove { this.eventComponentAdded -= value; }
		}
		/// <summary>
		/// Fired when a Component is about to be removed from the GameObject
		/// </summary>
		public event EventHandler<ComponentEventArgs>				EventComponentRemoving
		{
			add { this.eventComponentRemoving += value; }
			remove { this.eventComponentRemoving -= value; }
		}


		/// <summary>
		/// Creates a new, empty GameObject.
		/// </summary>
		public GameObject() {}
		/// <summary>
		/// Creates a new, empty GameObject with a specific name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		public GameObject(string name, GameObject parent = null)
		{
			this.Name = name;
			this.Parent = parent;
		}
		/// <summary>
		/// Creates a GameObject based on a specific <see cref="Duality.Resources.Prefab"/>.
		/// </summary>
		/// <param name="prefab">The Prefab that will be applied to this GameObject.</param>
		/// <seealso cref="Duality.Resources.Prefab"/>
		public GameObject(ContentRef<Prefab> prefab)
		{
			if (!prefab.IsAvailable) return;
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				prefab.Res.CopyTo(this);
			}
			else
			{
				this.LinkToPrefab(prefab);
				this.PrefabLink.Apply();
			}
		}

		/// <summary>
		/// Sets or alters this GameObject's <see cref="Duality.Resources.PrefabLink"/> to reference the specified <see cref="Prefab"/>.
		/// </summary>
		/// <param name="prefab">The Prefab that will be linked to.</param>
		/// <seealso cref="Duality.Resources.PrefabLink"/>
		/// <seealso cref="Duality.Resources.Prefab"/>
		public void LinkToPrefab(ContentRef<Prefab> prefab)
		{
			if (this.prefabLink == null)
			{
				// Not affected by another (higher) PrefabLink
				if (this.AffectedByPrefabLink == null)
				{
					this.prefabLink = new PrefabLink(this, prefab);
					// If a nested object is already PrefabLink'ed, add it to the changelist
					foreach (GameObject child in this.GetChildrenDeep())
					{
						if (child.PrefabLink != null && child.PrefabLink.ParentLink == this.prefabLink)
						{
							this.prefabLink.PushChange(child, ReflectionInfo.Property_GameObject_PrefabLink, child.PrefabLink.Clone());
						}
					}
				}
				// Already affected by another (higher) PrefabLink
				else
				{
					this.prefabLink = new PrefabLink(this, prefab);
					this.prefabLink.ParentLink.RelocateChanges(this.prefabLink);
				}
			}
			else
				this.prefabLink = this.prefabLink.Clone(this, prefab);
		}
		/// <summary>
		/// Breaks this GameObject's <see cref="Duality.Resources.PrefabLink"/>
		/// </summary>
		public void BreakPrefabLink()
		{
			this.prefabLink = null;
		}
		
		/// <summary>
		/// Enumerates all GameObjects that are directly or indirectly parented to this object, i.e. its
		/// children, grandchildren, etc.
		/// </summary>
		public IEnumerable<GameObject> GetChildrenDeep()
		{
			if (this.children == null) return EmptyChildren;

			int startCapacity = Math.Max(this.children.Count * 2, 8);
			List<GameObject> result = new List<GameObject>(startCapacity);
			this.GetChildrenDeep(result);
			return result;
		}
		/// <summary>
		/// Gathers all GameObjects that are directly or indirectly parented to this object, i.e. its
		/// children, grandchildren, etc.
		/// </summary>
		public void GetChildrenDeep(List<GameObject> resultList)
		{
			if (this.children == null) return;
			resultList.AddRange(this.children);
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildrenDeep(resultList);
			}
		}

		/// <summary>
		/// Returns the first child GameObject with the specified name. You may also specify a full name to access children's children.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObject GetChildByName(string name)
		{
			if (this.children == null || string.IsNullOrEmpty(name)) return null;
			return this.children.FirstByName(name);
		}
		/// <summary>
		/// Executes a series of child indexing operations, beginning at this GameObject and 
		/// each on the last retrieved child object.
		/// </summary>
		/// <param name="indexPath">An enumeration of child indices.</param>
		/// <returns>The last retrieved GameObject after executing all indexing steps.</returns>
		/// <example>
		/// Calling <c>ChildAtIndexPath(new[] { 0, 0 })</c> will return the first child of the first child.
		/// </example>
		public GameObject GetChildAtIndexPath(IEnumerable<int> indexPath)
		{
			GameObject curObj = this;
			foreach (int i in indexPath)
			{
				if (i < 0) return null;
				if (i >= curObj.children.Count) return null;
				curObj = curObj.children[i];
			}
			return curObj;
		}
		/// <summary>
		/// Determines the index path from this GameObject to the specified child (or grandchild, etc.) of it.
		/// </summary>
		/// <param name="child">The child GameObject to lead to.</param>
		/// <returns>A <see cref="List{T}"/> of indices that lead from this GameObject to the specified child GameObject.</returns>
		/// <seealso cref="GetChildAtIndexPath"/>
		public List<int> GetIndexPathOfChild(GameObject child)
		{
			List<int> path = new List<int>();
			while (child.parent != null && child != this)
			{
				path.Add(child.parent.children.IndexOf(child));
				child = child.parent;
			}
			path.Reverse();
			return path;
		}
		/// <summary>
		/// Returns whether this GameObject is a child, grandchild or similar of the specified GameObject.
		/// </summary>
		/// <param name="parent">The GameObject to check whether or not it is a parent of this one.</param>
		/// <returns>True, if it is, false if not.</returns>
		public bool IsChildOf(GameObject parent)
		{
			if (this.parent == parent)
				return true;
			else if (this.parent != null) 
				return this.parent.IsChildOf(parent);
			else
				return false;
		}
		
		/// <summary>
		/// Returns a single <see cref="Component"/> that matches the specified <see cref="System.Type"/>.
		/// </summary>
		/// <typeparam name="T">The Type to match the Components with.</typeparam>
		/// <returns>A single Component that matches the specified Type. Null, if none was found.</returns>
		public T GetComponent<T>() where T : class
		{
			Component result;
			if (!this.compMap.TryGetValue(typeof(T), out result))
				return this.GetComponents<T>().FirstOrDefault();
			else
				return result as T;
		}
		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this GameObject that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <typeparam name="T">The base Type to match when iterating through the Components.</typeparam>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponents(System.Type)"/>
		public IEnumerable<T> GetComponents<T>() where T : class
		{
			return this.compList.OfType<T>();
		}
		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this object's child GameObjects that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <typeparam name="T">The base Type to match when iterating through the Components.</typeparam>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponentsInChildren(System.Type)"/>
		public IEnumerable<T> GetComponentsInChildren<T>() where T : class
		{
			if (this.children == null) return new T[0];
			return this.children.SelectMany(o => o.GetComponentsDeep<T>());
		}
		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this GameObject or any child GameObject that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <typeparam name="T">The base Type to match when iterating through the Components.</typeparam>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponentsDeep(System.Type)"/>
		public IEnumerable<T> GetComponentsDeep<T>() where T : class
		{
			return this.GetComponents<T>().Concat(this.GetComponentsInChildren<T>());
		}

		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this GameObject that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <param name="t">The base Type to match when iterating through the Components.</param>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponents{T}()"/>
		public IEnumerable<Component> GetComponents(Type t)
		{
			TypeInfo typeInfo = t.GetTypeInfo();
			return this.compList.Where(c => typeInfo.IsInstanceOfType(c));
		}
		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this object's child GameObjects that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <param name="t">The base Type to match when iterating through the Components.</param>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponentsInChildren{T}()"/>
		public IEnumerable<Component> GetComponentsInChildren(Type t)
		{
			if (this.children == null) return new Component[0];
			return this.children.SelectMany(o => o.GetComponentsDeep(t));
		}
		/// <summary>
		/// Enumerates all <see cref="Component"/>s of this GameObject or any child GameObject that match the specified <see cref="Type"/> or subclass it.
		/// </summary>
		/// <param name="t">The base Type to match when iterating through the Components.</param>
		/// <returns>An enumeration of all Components that match the specified conditions.</returns>
		/// <seealso cref="GetComponentsDeep{T}()"/>
		public IEnumerable<Component> GetComponentsDeep(Type t)
		{
			return this.GetComponents(t).Concat(this.GetComponentsInChildren(t));
		}
		/// <summary>
		/// Returns a single <see cref="Component"/> that matches the specified <see cref="System.Type"/>.
		/// </summary>
		/// <param name="t">The Type to match the Components with.</param>
		/// <returns>A single Component that matches the specified Type. Null, if none was found.</returns>
		/// <seealso cref="GetComponent{T}()"/>
		public Component GetComponent(Type t)
		{
			Component result;
			if (!this.compMap.TryGetValue(t, out result))
				return this.GetComponents(t).FirstOrDefault();
			else
				return result;
		}

		/// <summary>
		/// Adds a <see cref="Component"/> of the specified <see cref="System.Type"/> to this GameObject, if not existing yet.
		/// Simply uses the existing Component otherwise.
		/// </summary>
		/// <typeparam name="T">The Type of which to request a Component instance.</typeparam>
		/// <returns>A reference to a Component of the specified Type.</returns>
		/// <seealso cref="AddComponent(System.Type)"/>
		public T AddComponent<T>() where T : Component, new()
		{
			if (this.compMap.ContainsKey(typeof(T))) return this.compMap[typeof(T)] as T;
			T newComp = new T();
			return this.AddComponent<T>(newComp);
		}
		/// <summary>
		/// Adds a <see cref="Component"/> of the specified <see cref="System.Type"/> to this GameObject, if not existing yet.
		/// Simply uses the existing Component otherwise.
		/// </summary>
		/// <param name="type">The Type of which to request a Component instance.</param>
		/// <returns>A reference to a Component of the specified Type.</returns>
		/// <seealso cref="AddComponent{T}()"/>
		public Component AddComponent(Type type)
		{
			Component existing;
			if (this.compMap.TryGetValue(type, out existing))
			{
				return existing;
			}

			Component newComp = type.GetTypeInfo().CreateInstanceOf() as Component;
			this.AddComponent(newComp, type);

			return newComp;
		}
		/// <summary>
		/// Adds the specified <see cref="Component"/> to this GameObject, if no Component of that Type is already part of this GameObject.
		/// Simply uses the already added Component otherwise.
		/// </summary>
		/// <typeparam name="T">The Components Type.</typeparam>
		/// <param name="newComp">The Component instance to add to this GameObject.</param>
		/// <returns>A reference to a Component of the specified Type</returns>
		/// <exception cref="System.ArgumentException">Thrown if the specified Component is already attached to a GameObject</exception>
		public T AddComponent<T>(T newComp) where T : Component
		{
			if (newComp.gameobj != null) throw new ArgumentException(String.Format(
				"Specified Component '{0}' is already part of another GameObject '{1}'",
				LogFormat.Type(newComp.GetType()),
				newComp.gameobj.FullName));
			
			Type type = newComp.GetType();
			Component existing;
			if (this.compMap.TryGetValue(type, out existing))
			{
				return existing as T;
			}

			this.AddComponent(newComp, type);

			return newComp;
		}
		private void AddComponent(Component newComp, Type type)
		{
			newComp.gameobj = this;
			this.compMap.Add(type, newComp);
			
			bool added = false;
			int newSortIndex = Component.ExecOrder.GetSortIndex(type);
			for (int i = 0; i < this.compList.Count; i++)
			{
				Type itemType = this.compList[i].GetType();
				int itemSortIndex = Component.ExecOrder.GetSortIndex(itemType);
				if (itemSortIndex > newSortIndex)
				{
					this.compList.Insert(i, newComp);
					added = true;
					break;
				}
			}
			if (!added)
				this.compList.Add(newComp);

			if (newComp is Transform) this.compTransform = newComp as Transform;

			this.OnComponentAdded(newComp);
		}
		/// <summary>
		/// Removes a <see cref="Component"/> of the specified <see cref="System.Type"/> from this GameObject, if existing.
		/// </summary>
		/// <typeparam name="T">The Type of which to remove a Component instance.</typeparam>
		/// <returns>A reference to the removed Component. Null otherwise.</returns>
		/// <seealso cref="RemoveComponent(Type)"/>
		/// <seealso cref="RemoveComponent(Component)"/>
		public T RemoveComponent<T>() where T : Component
		{
			return this.RemoveComponent(typeof(T)) as T;
		}
		/// <summary>
		/// Removes a <see cref="Component"/> of the specified <see cref="System.Type"/> from this GameObject, if existing.
		/// </summary>
		/// <param name="t">The Type of which to remove a Component instance.</param>
		/// <returns>A reference to the removed Component. Null otherwise.</returns>
		/// <seealso cref="RemoveComponent{T}()"/>
		/// <seealso cref="RemoveComponent(Component)"/>
		public Component RemoveComponent(Type t)
		{
			Component cmp = this.GetComponent(t);
			if (cmp != null) this.RemoveComponent(cmp);
			return cmp;
		}
		/// <summary>
		/// Removes a specific <see cref="Component"/> from this GameObject.
		/// </summary>
		/// <param name="cmp">The Component to remove from this GameObject</param>
		/// <exception cref="System.ArgumentNullException">Thrown when the specified Component is a null reference.</exception>
		/// <exception cref="System.ArgumentException">Thrown when the specified Component does not belong to this GameObject</exception>
		/// <seealso cref="RemoveComponent(Type)"/>
		/// <seealso cref="RemoveComponent{T}()"/>
		public void RemoveComponent(Component cmp)
		{
			if (cmp == null) throw new ArgumentNullException("cmp", "Can't remove a null reference Component");
			if (cmp.gameobj != this) throw new ArgumentException("The specified Component does not belong to this GameObject", "cmp");

			this.OnComponentRemoving(cmp);

			this.compMap.Remove(cmp.GetType());
			this.compList.Remove(cmp);

			if (cmp is Components.Transform) this.compTransform = null;

			cmp.gameobj = null;
		}
		/// <summary>
		/// Removes all <see cref="Component">Components</see> from this GameObject.
		/// </summary>
		public void ClearComponents()
		{
			foreach (Component c in this.compList)
			{
				this.OnComponentRemoving(c);
				c.gameobj = null;
			}
			this.compList.Clear();
			this.compMap.Clear();
			this.compTransform = null;
		}

		/// <summary>
		/// Iterates over all Components that are instances of Type T. Unlike iterating manually over <see cref="GetComponents{T}"/>,
		/// this method allows the underlying collection to change while iterating, making it a good candidate for ICmp notify operations.
		/// </summary>
		/// <typeparam name="T">The base Type of Components that are iterated. May be an ICmp interface or similar.</typeparam>
		/// <param name="forEach">The operation that is performed on each Component.</param>
		/// <param name="where">An optional predicate that needs to return true in order to perform the operation.</param>
		public void IterateComponents<T>(Action<T> forEach, Predicate<T> where = null) where T : class
		{
			Component[] iterateList = this.compList.ToArray();
			for (int i = 0; i < iterateList.Length; i++)
			{
				// Perform operation on elements matching predicate and Type
				T cmp = iterateList[i] as T;
				if (cmp != null && (where == null || where(cmp)))
					forEach(cmp);
			}
		}
		/// <summary>
		/// Iterates over all child GameObjects. Unlike iterating manually over <see cref="Children"/>,
		/// this method allows the underlying collection to change while iterating, making it a good candidate for notify operations
		/// that may execute code which adds or removed children.
		/// </summary>
		/// <param name="forEach">The operation that is performed on each child object.</param>
		/// <param name="where">An optional predicate that needs to return true in order to perform the operation.</param>
		public void IterateChildren(Action<GameObject> forEach, Predicate<GameObject> where = null)
		{
			if (this.children == null) return;

			GameObject[] iterateList = this.children.ToArray();
			for (int i = 0; i < iterateList.Length; i++)
			{
				// Perform operation on elements matching the predicate
				GameObject obj = iterateList[i];
				if (where == null || where(obj))
					forEach(obj);
			}
		}

		/// <summary>
		/// Disposes this GameObject as well as all of its child GameObjects and <see cref="Component">Components</see>.
		/// You usually don't need this - use <see cref="ExtMethodsIManageableObject.DisposeLater"/> instead.
		/// </summary>
		/// <seealso cref="ExtMethodsIManageableObject.DisposeLater"/>
		public void Dispose()
		{
			if (this.initState == InitState.Initialized)
			{
				this.initState = InitState.Disposing;

				// Don't bother disposing Components - they inherit the Disposed state 
				// from their GameObject and receive a Deactivate event due to the objects
				// removel from Scene anyway.
				/*
				for (int i = this.compList.Count - 1; i >= 0; i--)
				{
					this.compList[i].Dispose(false);
				}
				*/

				// Dispose child objects
				if (this.children != null)
				{
					for (int i = this.children.Count - 1; i >= 0; i--)
					{
						this.children[i].Dispose();
					}
				}

				// Remove from parent, if that's still alive
				if (this.parent != null && this.parent.initState != InitState.Disposing && this.parent.initState != InitState.Disposed)
				{
					this.Parent = null;
				}

				this.initState = InitState.Disposed;
			}
		}
		/// <summary>
		/// Creates a deep copy of this GameObject.
		/// </summary>
		/// <returns>A reference to a newly created deep copy of this GameObject.</returns>
		public GameObject Clone()
		{
			return this.DeepClone();
		}
		/// <summary>
		/// Deep-copies this GameObject's data to the specified target GameObject.
		/// </summary>
		/// <param name="target">The target GameObject to copy to.</param>
		public void CopyTo(GameObject target)
		{
			this.DeepCopyTo(target);
		}
		void ICloneExplicit.SetupCloneTargets(object targetObj, ICloneTargetSetup setup)
		{
			GameObject target = targetObj as GameObject;
			bool isPrefabApply = setup.Context is ApplyPrefabContext;

			if (!isPrefabApply)
			{
				// Destroy additional Components in the target GameObject
				if (target.compMap.Count > 0)
				{
					List<Type> removeComponentTypes = null;
					foreach (var pair in target.compMap)
					{
						if (!this.compMap.ContainsKey(pair.Key))
						{
							if (removeComponentTypes == null) removeComponentTypes = new List<Type>();
							removeComponentTypes.Add(pair.Key);
						}
					}
					if (removeComponentTypes != null)
					{
						foreach (Type type in removeComponentTypes)
						{
							target.RemoveComponent(type);
						}
					}
				}
				// Destroy additional child objects in the target GameObject
				if (target.children != null)
				{
					int thisChildCount = this.children != null ? this.children.Count : 0;
					for (int i = target.children.Count - 1; i >= thisChildCount; i--)
					{
						target.children[i].Dispose();
					}
				}
			}

			// Create missing Components in the target GameObject
			foreach (var pair in this.compMap)
			{
				Component targetComponent = target.AddComponent(pair.Key);
				setup.HandleObject(pair.Value, targetComponent, CloneBehavior.ChildObject);
			}
			// Create missing child objects in the target GameObject
			if (this.children != null)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					GameObject targetChild;
					if (target.children != null && target.children.Count > i)
						targetChild = target.children[i];
					else
						targetChild = new GameObject(string.Empty, target);

					setup.HandleObject(this.children[i], targetChild, CloneBehavior.ChildObject);
				}
			}

			// Handle referenced and child objects
			setup.HandleObject(this.scene, target.scene, CloneBehavior.WeakReference);
			setup.HandleObject(this.prefabLink, target.prefabLink);
		}
		void ICloneExplicit.CopyDataTo(object targetObj, ICloneOperation operation)
		{
			GameObject target = targetObj as GameObject;

			// Copy plain old data
			target.name = this.name;
			target.active = this.active;
			target.initState = this.initState;
			if (!operation.Context.PreserveIdentity)
				target.identifier = this.identifier;

			// Copy Components from source to target
			for (int i = 0; i < this.compList.Count; i++)
			{
				operation.HandleObject(this.compList[i]);
			}

			// Copy child objects from source to target
			if (this.children != null)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					operation.HandleObject(this.children[i]);
				}
			}

			// Copy referenced and child objects
			operation.HandleObject(this.scene, ref target.scene, true);
			operation.HandleObject(this.prefabLink, ref target.prefabLink, true);
		}

		/// <summary>
		/// Gathers a list of components that would be affected if this <see cref="GameObject"/>
		/// changed its activation state. This excludes components and child objects that
		/// are inactive in their own right.
		/// </summary>
		/// <param name="initList"></param>
		/// <param name="deep"></param>
		internal void GatherInitComponents(List<ICmpInitializable> initList, bool deep)
		{
			for (int i = 0; i < this.compList.Count; i++)
			{
				Component component = this.compList[i];
				ICmpInitializable init = component as ICmpInitializable;

				if (init == null) continue;
				if (!component.ActiveSingle) continue;

				initList.Add(init);
			}

			if (deep && this.children != null)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					GameObject child = this.children[i];
					if (!child.ActiveSingle && !child.Disposed) continue;
					child.GatherInitComponents(initList, deep);
				}
			}
		}

		/// <summary>
		/// Checks all internal data for consistency and fixes problems where possible.
		/// This helps mitigate serialization problems that arise from changing data
		/// structures during dev time.
		/// </summary>
		internal void EnsureConsistentData()
		{
			// Check for null or disposed child objects
			if (this.children != null)
			{
				for (int i = this.children.Count - 1;  i >= 0; i--)
				{
					if (this.children[i] == null || this.children[i].Disposed)
					{
						this.children.RemoveAt(i);
						Logs.Core.WriteWarning(
							"Missing or Disposed Child in GameObject '{0}'. Check for serialization problems. Did you recently rename or remove classes?", 
							this);
					}
				}
			}

			// Check Component List for null or disposed entries
			if (this.compList != null)
			{
				for (int i = this.compList.Count - 1;  i >= 0; i--)
				{
					if (this.compList[i] == null || this.compList[i].Disposed)
					{
						this.compList.RemoveAt(i);
						Logs.Core.WriteWarning(
							"Missing or Disposed Component in GameObject '{0}'. Check for serialization problems. Did you recently rename or remove classes?", 
							this);
					}
				}
			}
			else
			{
				this.compList = new List<Component>();
				Logs.Core.WriteWarning(
					"GameObject '{0}' didn't have a Component list. Check for serialization problems. Did you recently rename or remove classes?", 
					this);
			}

			// Check Component Map for null or disposed entries
			if (this.compMap != null)
			{
				foreach (Type key in this.compMap.Keys.ToArray())
				{
					if (this.compMap[key] == null || this.compMap[key].Disposed)
					{
						this.compMap.Remove(key);
						Logs.Core.WriteWarning(
							"Missing or Disposed Component '{0}' in GameObject '{1}'. Check for serialization problems. Did you recently rename or remove classes?", 
							key,
							this);
					}
				}
			}
			else
			{
				this.compMap = new Dictionary<Type,Component>();
				Logs.Core.WriteWarning(
					"GameObject '{0}' didn't have a Component map. Check for serialization problems. Did you recently rename or remove classes?", 
					this);
			}
		}
		/// <summary>
		/// Checks the objects internal <see cref="Component"/> containers for the correct
		/// execution order and sorts them where necessary.
		/// </summary>
		internal void EnsureComponentOrder()
		{
			// Using insertion sort here, because it achieves best performance for already 
			// sorted lists, and nearly sorted lists, as well as small lists.
			ComponentExecutionOrder execOrder = Component.ExecOrder;
			for (int k = 1; k < this.compList.Count; k++)
			{
				Component swapComponent = this.compList[k];
				int swapSortIndex = execOrder.GetSortIndex(swapComponent.GetType());
				int index = k - 1;
				while (index >= 0)
				{
					int sortIndex = execOrder.GetSortIndex(this.compList[index].GetType());
					if (sortIndex > swapSortIndex)
					{
						this.compList[index + 1] = this.compList[index];
						index--;
						continue;
					}
					break;
				}
				this.compList[index + 1] = swapComponent;
			}
		}

		private void OnParentChanged(GameObject oldParent, GameObject newParent)
		{
			// Public event
			if (this.eventParentChanged != null)
				this.eventParentChanged(this, new GameObjectParentChangedEventArgs(this, oldParent, newParent));
		}
		private void OnComponentAdded(Component cmp)
		{
			// Notify Components
			ICmpInitializable cmpInit = cmp as ICmpInitializable;
			if (cmpInit != null) cmpInit.OnInit(Component.InitContext.AddToGameObject);

			// Public event
			if (this.eventComponentAdded != null)
				this.eventComponentAdded(this, new ComponentEventArgs(cmp));
		}
		private void OnComponentRemoving(Component cmp)
		{
			// Notify Components
			ICmpInitializable cmpInit = cmp as ICmpInitializable;
			if (cmpInit != null) cmpInit.OnShutdown(Component.ShutdownContext.RemovingFromGameObject);

			// Public event
			if (this.eventComponentRemoving != null)
				this.eventComponentRemoving(this, new ComponentEventArgs(cmp));
		}

		public override string ToString()
		{
			return string.Format("GameObject \"{0}\"", this.FullName);
		}
	}
}
