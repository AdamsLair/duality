using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Serialization;
using Duality.Cloning;
using Duality.Properties;
using Duality.Drawing;

namespace Duality.Resources
{
	/// <summary>
	/// A Scene encapsulates an organized set of <see cref="GameObject">GameObjects</see> and provides
	/// update-, rendering- and maintenance functionality. In Duality, there is always exactly one Scene
	/// <see cref="Scene.Current"/> which represents a level, gamestate or a combination of both, depending
	/// on you own design.
	/// </summary>
	[EditorHintCategory(CoreResNames.CategoryNone)]
	[EditorHintImage(CoreResNames.ImageScene)]
	public sealed class Scene : Resource
	{
		private static ContentRef<Scene>   current           = new Scene();
		private static bool                curAutoGen        = false;
		private static bool                isSwitching       = false;
		private static int                 switchLock        = 0;
		private static bool                switchToScheduled = false;
		private static ContentRef<Scene>   switchToTarget    = null;

		
		/// <summary>
		/// [GET / SET] The Scene that is currently active i.e. updated and rendered. This is never null.
		/// You may assign null in order to leave the current Scene and enter en empty dummy Scene.
		/// </summary>
		public static Scene Current
		{
			get
			{
				if (!curAutoGen && !current.IsAvailable)
				{
					curAutoGen = true;
					Current = new Scene();
					curAutoGen = false;
				}
				return current.Res;
			}
			private set
			{
				if (current.ResWeak != value)
				{
					if (!current.IsExplicitNull) 
						OnLeaving();

					current.Res = value ?? new Scene();

					OnEntered();
				}
				else
					current.Res = value ?? new Scene();
			}
		}
		/// <summary>
		/// [GET] The Resource file path of the current Scene.
		/// </summary>
		public static string CurrentPath
		{
			get { return current.Res != null ? current.Res.Path : current.Path; }
		}
		/// <summary>
		/// [GET] Returns whether <see cref="Scene.Current"/> is in a transition between two different states, i.e.
		/// whether the current Scene is being changed right now.
		/// </summary>
		public static bool IsSwitching
		{
			get { return isSwitching; }
		}


		/// <summary>
		/// Fired just before leaving the current Scene.
		/// </summary>
		public static event EventHandler Leaving;
		/// <summary>
		/// Fired right after entering the (now) current Scene.
		/// </summary>
		public static event EventHandler Entered;
		/// <summary>
		/// Fired when a <see cref="GameObject">GameObjects</see> parent object has been changed in the current Scene.
		/// </summary>
		public static event EventHandler<GameObjectParentChangedEventArgs> GameObjectParentChanged;
		/// <summary>
		/// Fired once every time a group of <see cref="GameObject"/> instances has been registered in the current Scene.
		/// </summary>
		public static event EventHandler<GameObjectGroupEventArgs> GameObjectsAdded;
		/// <summary>
		/// Fired once every time a group of <see cref="GameObject"/> instances has been unregistered from the current Scene.
		/// </summary>
		public static event EventHandler<GameObjectGroupEventArgs> GameObjectsRemoved;
		/// <summary>
		/// Fired when a <see cref="Component"/> has been added to a <see cref="GameObject"/> that is registered in the current Scene.
		/// </summary>
		public static event EventHandler<ComponentEventArgs> ComponentAdded;
		/// <summary>
		/// Fired when a <see cref="Component"/> has been removed from a <see cref="GameObject"/> that is registered in the current Scene.
		/// </summary>
		public static event EventHandler<ComponentEventArgs> ComponentRemoving;


		/// <summary>
		/// Switches to the specified <see cref="Scene"/>, which will become the new <see cref="Current">current one</see>.
		/// By default, this method does not guarantee to perform the Scene switch immediately, but may defer the switch
		/// to the end of the current update cycle.
		/// </summary>
		/// <param name="scene">The Scene to switch to.</param>
		/// <param name="forceImmediately">If true, an immediate switch is forced. Use only when necessary.</param>
		public static void SwitchTo(ContentRef<Scene> scene, bool forceImmediately = false)
		{
			// Check parameters
			if (!scene.IsExplicitNull && !scene.IsAvailable) 
				throw new ArgumentException("Can't switch to Scene '" + scene.Path + "' because it doesn't seem to exist.", "scene");

			if (switchLock == 0 || forceImmediately)
			{
				Scene.Current = scene.Res;
			}
			else
			{
				switchToTarget = scene;
				switchToScheduled = true;
			}
		}
		/// <summary>
		/// Reloads the <see cref="Current">current Scene</see> or schedules it for reload at the end of the
		/// frame, depending on whether it is considered safe to do so immediately. Similar to <see cref="SwitchTo"/> with
		/// regard to execution planning.
		/// </summary>
		public static void Reload()
		{
			ContentRef<Scene> target = Scene.Current;

			if (switchLock == 0)
				Scene.Current.Dispose();
			else
				Scene.Current.DisposeLater();

			Scene.SwitchTo(target);
		}

		/// <summary>
		/// Performs a <see cref="Scene"/> switch operation that was scheduled using
		/// <see cref="Scene.SwitchTo"/>.
		/// </summary>
		/// <returns></returns>
		private static bool PerformScheduledSwitch()
		{
			if (!switchToScheduled) return false;

			// Retrieve the target and reset the scheduled switch
			string oldName = Scene.Current.FullName;
			Scene target = switchToTarget.Res;
			switchToTarget = null;
			switchToScheduled = false;

			// Perform the scheduled switch
			Scene.Current = target;

			// If we now end up with another scheduled switch, we might be
			// caught up in a redirect loop, where a Scene, when activated,
			// will immediately switch to another Scene, which will do the same.
			if (switchToScheduled)
			{
				switchToTarget = null;
				switchToScheduled = false;
				Logs.Core.WriteWarning(
					"Potential Scene redirect loop detected: When performing previously " +
					"scheduled switch to Scene '{0}', a awitch to Scene '{1}' was immediately scheduled. " +
					"The second switch will not be performed to avoid entering a loop. Please " +
					"check when you call Scene.SwitchTo and avoid doing that during object activation.",
					oldName, Scene.Current.FullName);
			}

			return true;
		}

		private static void OnLeaving()
		{
			switchLock++;
			if (Leaving != null) Leaving(current, null);
			isSwitching = true;
			if (current.ResWeak != null)
			{
				// Deactivate GameObjects
				DualityApp.EditorGuard(() =>
				{
					// Create a list of components to deactivate
					List<ICmpInitializable> shutdownList = new List<ICmpInitializable>();
					foreach (Component component in current.ResWeak.FindComponents<ICmpInitializable>())
					{
						if (!component.Active) continue;
						shutdownList.Add(component as ICmpInitializable);
					}
					// Deactivate all the listed components. Note that they may create or destroy
					// objects, so it's important that we're iterating a copy of the scene objects
					// here, and not the real thing.
					for (int i = shutdownList.Count - 1; i >= 0; i--)
					{
						shutdownList[i].OnDeactivate();
					}
				});

				// Clear physics world as we're ending simulation
				current.ResWeak.Physics.Clear();
				current.ResWeak.Physics.ResetSimulation();
			}
			switchLock--;
		}
		private static void OnEntered()
		{
			switchLock++;
			if (current.ResWeak != null)
			{
				// Apply physical properties
				current.ResWeak.Physics.ResetSimulation();
				current.ResWeak.Physics.Gravity = PhysicsUnit.ForceToPhysical * current.ResWeak.GlobalGravity;

				// When in the editor, apply prefab links
				if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor)
					current.ResWeak.ApplyPrefabLinks();

				// When running the game, break prefab links
				if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
					current.ResWeak.BreakPrefabLinks();

				// Activate GameObjects
				DualityApp.EditorGuard(() =>
				{
					// Create a list of components to activate
					List<ICmpInitializable> initList = new List<ICmpInitializable>();
					foreach (Component component in current.ResWeak.FindComponents<ICmpInitializable>())
					{
						if (!component.Active) continue;
						initList.Add(component as ICmpInitializable);
					}
					// Activate all the listed components. Note that they may create or destroy
					// objects, so it's important that we're iterating a copy of the scene objects
					// here, and not the real thing.
					for (int i = 0; i < initList.Count; i++)
					{
						initList[i].OnActivate();
					}
				});

				// Update object visibility / culling info, so a scheduled switch at the
				// end of a frame will get up-to-date culling for rendering
				DualityApp.EditorGuard(() =>
				{
					current.ResWeak.VisibilityStrategy.Update();
				});
			}
			isSwitching = false;
			if (Entered != null) Entered(current, null);
			switchLock--;
		}
		private static void OnGameObjectParentChanged(GameObjectParentChangedEventArgs args)
		{
			if (GameObjectParentChanged != null) GameObjectParentChanged(current, args);
		}
		private static void OnGameObjectsAdded(GameObjectGroupEventArgs args)
		{
			// Gather a list of components to activate
			int objCount = 0;
			List<ICmpInitializable> initList = new List<ICmpInitializable>();
			foreach (GameObject obj in args.Objects)
			{
				if (!obj.ActiveSingle) continue;
				obj.GatherInitComponents(initList, false);
				objCount++;
			}

			// If we collected components from more than one object, sort by exec order.
			// Otherwise, we can safely assume that the list is already sorted.
			if (objCount > 1) Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), false);

			// Invoke the init event on all gathered components in the right order
			foreach (ICmpInitializable component in initList)
				component.OnActivate();

			// Fire a global event to indicate that the new objects are ready
			if (GameObjectsAdded != null)
				GameObjectsAdded(current, args);
		}
		private static void OnGameObjectsRemoved(GameObjectGroupEventArgs args)
		{
			// Fire a global event to indicate that the objects are going to be shut down
			if (GameObjectsRemoved != null)
				GameObjectsRemoved(current, args);

			// Gather a list of components to deactivate
			int objCount = 0;
			List<ICmpInitializable> initList = new List<ICmpInitializable>();
			foreach (GameObject obj in args.Objects)
			{
				if (!obj.ActiveSingle && !obj.Disposed) continue;
				obj.GatherInitComponents(initList, false);
				objCount++;
			}

			// If we collected components from more than one object, sort by exec order.
			// Otherwise, we can safely assume that the list is already sorted.
			if (objCount > 1)
				Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), true);
			else
				initList.Reverse();

			// Invoke the init event on all gathered components in the right order
			foreach (ICmpInitializable component in initList)
				component.OnDeactivate();
		}
		private static void OnComponentAdded(ComponentEventArgs args)
		{
			if (args.Component.Active)
			{
				ICmpInitializable cInit = args.Component as ICmpInitializable;
				if (cInit != null) cInit.OnActivate();
			}
			if (ComponentAdded != null) ComponentAdded(current, args);
		}
		private static void OnComponentRemoving(ComponentEventArgs args)
		{
			if (args.Component.Active)
			{
				ICmpInitializable cInit = args.Component as ICmpInitializable;
				if (cInit != null) cInit.OnDeactivate();
			}
			if (ComponentRemoving != null) ComponentRemoving(current, args);
		}


		private struct UpdateEntry
		{
			public TypeInfo Type;
			public int Count;
			public TimeCounter Profiler;
		}

		private Vector2                     globalGravity      = Vector2.UnitY * 33.0f;
		private IRendererVisibilityStrategy visibilityStrategy = new DefaultRendererVisibilityStrategy();
		private GameObject[]                serializeObj       = null;

		[DontSerialize]
		private PhysicsWorld physicsWorld = new PhysicsWorld();

		[DontSerialize]
		[CloneField(CloneFieldFlags.DontSkip)]
		[CloneBehavior(typeof(GameObject), CloneBehavior.ChildObject)]
		private	GameObjectManager objectManager = new GameObjectManager();

		[DontSerialize]
		[CloneField(CloneFieldFlags.DontSkip)]
		private Dictionary<TypeInfo,List<Component>> componentsByType = new Dictionary<TypeInfo,List<Component>>();

		// Temporary buffers used during scene updates, stored and re-used for efficiency
		[DontSerialize] private List<Type> updateTypeOrder = new List<Type>();
		[DontSerialize] private RawList<Component> updatableComponents = new RawList<Component>(256);
		[DontSerialize] private RawList<UpdateEntry> updateMap = new RawList<UpdateEntry>();

		
		/// <summary>
		/// [GET / SET] The strategy that is used to determine which <see cref="ICmpRenderer">renderers</see> are visible.
		/// </summary>
		public IRendererVisibilityStrategy VisibilityStrategy
		{
			get { return this.visibilityStrategy; }
			set { this.visibilityStrategy = value ?? new DefaultRendererVisibilityStrategy(); }
		}
		/// <summary>
		/// [GET / SET] Global gravity force that is applied to all objects that obey the laws of physics.
		/// </summary>
		public Vector2 GlobalGravity
		{
			get { return this.globalGravity; }
			set
			{
				this.globalGravity = value;
				this.physicsWorld.Gravity = PhysicsUnit.ForceToPhysical * value;
			}
		}
		/// <summary>
		/// [GET] Returns the current physics world.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public PhysicsWorld Physics
		{
			get { return this.physicsWorld; }
		}
		/// <summary>
		/// [GET] Enumerates all registered objects.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<GameObject> AllObjects
		{
			get { return this.objectManager.AllObjects; }
		}
		/// <summary>
		/// [GET] Enumerates all registered objects that are currently active.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<GameObject> ActiveObjects
		{
			get { return this.objectManager.ActiveObjects; }
		}
		/// <summary>
		/// [GET] Enumerates all root GameObjects, i.e. all GameObjects without a parent object.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<GameObject> RootObjects
		{
			get { return this.objectManager.RootObjects; }
		}
		/// <summary>
		/// [GET] Enumerates all <see cref="RootObjects"/> that are currently active.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public IEnumerable<GameObject> ActiveRootObjects
		{
			get { return this.objectManager.ActiveRootObjects; }
		}
		/// <summary>
		/// [GET] Returns whether this Scene is <see cref="Scene.Current"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsCurrent
		{
			get { return current.ResWeak == this; }
		}
		/// <summary>
		/// [GET] Returns whether this Scene is completely empty.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool IsEmpty
		{
			get { return !this.objectManager.AllObjects.Any(); }
		}


		/// <summary>
		/// Creates a new, empty scene which does not contain any <see cref="GameObject">GameObjects</see>.
		/// </summary>
		public Scene()
		{
			this.RegisterManagerEvents();
		}

		/// <summary>
		/// Renders the Scene
		/// </summary>
		/// <param name="target">
		/// The <see cref="RenderTarget"/> which will be used for all rendering output. 
		/// "null" means rendering directly to the output buffer of the game window / screen.
		/// </param>
		/// <param name="viewportRect">The viewport to which will be rendered.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		internal void Render(ContentRef<RenderTarget> target, Rect viewportRect, Vector2 imageSize)
		{
			if (!this.IsCurrent) throw new InvalidOperationException("Can't render non-current Scene!");
			switchLock++;
			
			// Retrieve the rendering setup that will be used for rendering the scene
			RenderSetup setup = 
				DualityApp.AppData.RenderingSetup.Res ?? 
				RenderSetup.Default.Res;

			// Render the scene
			setup.RenderScene(this, target, viewportRect, imageSize);

			switchLock--;
		}
		/// <summary>
		/// Updates the Scene
		/// </summary>
		internal void Update()
		{
			if (!this.IsCurrent) throw new InvalidOperationException("Can't update non-current Scene!");
			switchLock++;

			// Update physics
			this.physicsWorld.Simulate(Time.DeltaTime);

			// Update all GameObjects
			Profile.TimeUpdateScene.BeginMeasure();
			DualityApp.EditorGuard(() =>
			{
				this.UpdateComponents<ICmpUpdatable>(cmp => cmp.OnUpdate());
				this.visibilityStrategy.Update();
			});
			Profile.TimeUpdateScene.EndMeasure();

			// Perform a cleanup step to catch all DisposeLater calls from within the Scene update
			DualityApp.RunCleanup();
			
			// Perform a scheduled Scene switch
			PerformScheduledSwitch();

			switchLock--;
		}
		/// <summary>
		/// Updates the Scene in the editor.
		/// </summary>
		internal void EditorUpdate()
		{
			if (!this.IsCurrent) throw new InvalidOperationException("Can't update non-current Scene!");
			switchLock++;

			Profile.TimeUpdateScene.BeginMeasure();
			DualityApp.EditorGuard(() =>
			{
				this.UpdateComponents<ICmpEditorUpdatable>(cmp => cmp.OnUpdate());
			});
			Profile.TimeUpdateScene.EndMeasure();

			switchLock--;
		}
		private void UpdateComponents<T>(Action<T> updateAction) where T : class
		{
			Profile.TimeUpdateSceneComponents.BeginMeasure();

			// Create a sorted list of updatable component types
			this.updateTypeOrder.Clear();
			foreach (var pair in this.componentsByType)
			{
				// Skip Component types that aren't updatable anyway
				Component sampleComponent = pair.Value.Count > 0 ? pair.Value[0] : null;
				if (!(sampleComponent is T))
					continue;

				this.updateTypeOrder.Add(pair.Key.AsType());
			}
			Component.ExecOrder.SortTypes(this.updateTypeOrder, false);

			// Gather a list of updatable Components
			this.updatableComponents.Clear();
			this.updateMap.Clear();
			foreach (Type type in this.updateTypeOrder)
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				List<Component> components = this.componentsByType[typeInfo];
				int oldCount = this.updatableComponents.Count;

				// Collect Components
				this.updatableComponents.Reserve(this.updatableComponents.Count + components.Count);
				for (int i = 0; i < components.Count; i++)
				{
					this.updatableComponents.Add(components[i]);
				}

				// Keep in mind how many Components of each type we have in what order
				if (this.updatableComponents.Count - oldCount > 0)
				{
					this.updateMap.Add(new UpdateEntry
					{
						Type = typeInfo,
						Count = this.updatableComponents.Count - oldCount,
						Profiler = Profile.RequestCounter<TimeCounter>(Profile.TimeUpdateScene.FullName + @"\" + typeInfo.Name)
					});
				}
			}

			// Update all Components. They're still sorted by type.
			{
				int updateMapIndex = -1;
				int updateMapBegin = -1;
				TimeCounter activeProfiler = null;
				Component[] data = this.updatableComponents.Data;
				UpdateEntry[] updateData = this.updateMap.Data;

				for (int i = 0; i < data.Length; i++)
				{
					if (i >= this.updatableComponents.Count) break;

					// Manage profilers per Component type
					if (i == 0 || i - updateMapBegin >= updateData[updateMapIndex].Count)
					{
						// Note:
						// Since we're doing this based on index-count ranges, this needs to be
						// done before skipping inactive Components, so we don't run out of sync.

						updateMapIndex++;
						updateMapBegin = i;

						if (activeProfiler != null)
							activeProfiler.EndMeasure();
						activeProfiler = updateData[updateMapIndex].Profiler;
						activeProfiler.BeginMeasure();
					}
					
					// Skip inactive, disposed and detached Components
					if (!data[i].Active) continue;

					// Invoke the Component's update action
					updateAction(data[i] as T);
				}
				
				if (activeProfiler != null)
					activeProfiler.EndMeasure();
			}

			Profile.TimeUpdateSceneComponents.EndMeasure();
		}
		/// <summary>
		/// Cleanes up disposed Scene objects.
		/// </summary>
		new internal void RunCleanup()
		{
			this.objectManager.Flush();
			this.visibilityStrategy.CleanupRenderers();
			foreach (List<Component> cmpList in this.componentsByType.Values)
				cmpList.RemoveAll(i => i == null || i.Disposed);
		}

		/// <summary>
		/// Applies all <see cref="Duality.Resources.PrefabLink">PrefabLinks</see> contained withing this
		/// Scenes <see cref="GameObject">GameObjects</see>.
		/// </summary>
		public void ApplyPrefabLinks()
		{
			PrefabLink.ApplyAllLinks(this.objectManager.AllObjects);
		}
		/// <summary>
		/// Breaks all <see cref="Duality.Resources.PrefabLink">PrefabLinks</see> contained withing this
		/// Scenes <see cref="GameObject">GameObjects</see>.
		/// </summary>
		public void BreakPrefabLinks()
		{
			foreach (GameObject obj in this.objectManager.AllObjects)
				obj.BreakPrefabLink();
		}

		/// <summary>
		/// Clears the Scene, unregistering all GameObjects. This does not <see cref="GameObject.Dispose">dispose</see> them.
		/// </summary>
		public void Clear()
		{
			this.objectManager.Clear();
		}
		/// <summary>
		/// Appends a cloned version of the specified Scenes contents to this Scene.
		/// </summary>
		/// <param name="scene">The source Scene.</param>
		public void Append(ContentRef<Scene> scene)
		{
			if (!scene.IsAvailable) return;
			this.objectManager.AddObjects(scene.Res.RootObjects.Select(o => o.Clone()));
		}
		/// <summary>
		/// Appends the specified Scene's contents to this Scene and consumes the specified Scene.
		/// </summary>
		/// <param name="scene">The source Scene.</param>
		public void Consume(ContentRef<Scene> scene)
		{
			if (!scene.IsAvailable) return;
			Scene otherScene = scene.Res;
			var otherObj = otherScene.RootObjects.ToArray();
			otherScene.Clear();
			this.objectManager.AddObjects(otherObj);
			otherScene.Dispose();
		}

		/// <summary>
		/// Registers a GameObject and all of its children.
		/// </summary>
		/// <param name="obj"></param>
		public void AddObject(GameObject obj)
		{
			if (obj.Scene != null && obj.Scene != this) obj.Scene.RemoveObject(obj);
			this.objectManager.AddObject(obj);
		}
		/// <summary>
		/// Registers a set of GameObjects and all of their children.
		/// </summary>
		/// <param name="objEnum"></param>
		public void AddObjects(IEnumerable<GameObject> objEnum)
		{
			foreach (GameObject obj in objEnum)
			{
				if (obj.Scene == null || obj.Scene == this) continue;
				obj.Scene.RemoveObject(obj);
			}
			this.objectManager.AddObjects(objEnum);
		}
		/// <summary>
		/// Unregisters a GameObject and all of its children
		/// </summary>
		/// <param name="obj"></param>
		public void RemoveObject(GameObject obj)
		{
			if (obj.Scene != this) return;
			if (obj.Parent != null && obj.Parent.Scene == this)
			{
				obj.Parent = null;
			}
			this.objectManager.RemoveObject(obj);
		}
		/// <summary>
		/// Unregisters a set of GameObjects and all of their children.
		/// </summary>
		/// <param name="objEnum"></param>
		public void RemoveObjects(IEnumerable<GameObject> objEnum)
		{
			objEnum = objEnum.Where(o => o.Scene == this);
			foreach (GameObject obj in objEnum)
			{
				if (obj.Parent == null) continue;
				if (obj.Parent.Scene != this) continue;
				obj.Parent = null;
			}
			this.objectManager.RemoveObjects(objEnum);
		}

		/// <summary>
		/// Finds all GameObjects in the Scene that match the specified name or name path.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<GameObject> FindGameObjects(string name)
		{
			return this.AllObjects.ByName(name);
		}
		/// <summary>
		/// Finds all GameObjects in the Scene which have a Component of the specified type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<GameObject> FindGameObjects(Type hasComponentOfType)
		{
			return this.FindComponents(hasComponentOfType).GameObject();
		}
		/// <summary>
		/// Finds all GameObjects in the Scene which have a Component of the specified type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<GameObject> FindGameObjects<T>() where T : class
		{
			return this.FindComponents<T>().OfType<Component>().GameObject();
		}
		/// <summary>
		/// Finds all Components of the specified type in this Scene.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<T> FindComponents<T>() where T : class
		{
			return FindComponents(typeof(T)).OfType<T>();
		}
		/// <summary>
		/// Finds all Components of the specified type in this Scene.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IEnumerable<Component> FindComponents(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();

			// Determine which by-type lists to use
			bool multiple = false;
			List<Component> singleResult = null;
			List<List<Component>> query = null;
			foreach (var pair in this.componentsByType)
			{
				if (pair.Value.Count == 0) continue;
				if (typeInfo.IsAssignableFrom(pair.Key))
				{
					if (!multiple && singleResult == null)
					{
						// Select single result
						singleResult = pair.Value;
					}
					else
					{
						// Switch to multiselect mode
						if (!multiple)
						{
							query = new List<List<Component>>(this.componentsByType.Values.Count);
							if (singleResult != null) query.Add(singleResult);
						}
						query.Add(pair.Value);
						multiple = true;
					}
				}
			}

			// Found only one match? Return that one.
			IEnumerable<Component> result = null;
			if (!multiple)
			{
				result = singleResult as IEnumerable<Component> ?? new Component[0];
			}
			// Select from a multitude of results
			else
			{
				Component.ExecOrder.SortTypedItems(query, list => list[0].GetType(), false);
				result = query.SelectMany(cmpArr => cmpArr);
			}

			return result;
		}
		
		/// <summary>
		/// Finds a single GameObjects in the Scene that match the specified name or name path.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObject FindGameObject(string name, bool activeOnly = true)
		{
			return (activeOnly ? this.ActiveObjects : this.AllObjects).ByName(name).FirstOrDefault();
		}
		/// <summary>
		/// Finds a single GameObject in the Scene that has a Component of the specified type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObject FindGameObject(Type hasComponentOfType, bool activeOnly = true)
		{
			Component cmp = this.FindComponent(hasComponentOfType, activeOnly);
			return cmp != null ? cmp.GameObj : null;
		}
		/// <summary>
		/// Finds a single GameObject in the Scene that has a Component of the specified type.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObject FindGameObject<T>(bool activeOnly = true) where T : class
		{
			Component cmp = this.FindComponent<T>(activeOnly) as Component;
			return cmp != null ? cmp.GameObj : null;
		}
		/// <summary>
		/// Finds a single Component of the specified type in this Scene.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public T FindComponent<T>(bool activeOnly = true) where T : class
		{
			return FindComponent(typeof(T), activeOnly) as T;
		}
		/// <summary>
		/// Finds a single Component of the specified type in this Scene.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Component FindComponent(Type type, bool activeOnly = true)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			foreach (var pair in this.componentsByType)
			{
				if (typeInfo.IsAssignableFrom(pair.Key))
				{
					if (activeOnly)
					{
						foreach (Component cmp in pair.Value)
						{
							if (!cmp.Active) continue;
							return cmp;
						}
					}
					else if (pair.Value.Count > 0)
					{
						return pair.Value[0];
					}
				}
			}

			return null;
		}

		private void AddToManagers(GameObject obj)
		{
			foreach (Component cmp in obj.Components)
				this.AddToManagers(cmp);
		}
		private void AddToManagers(Component cmp)
		{
			// Per-Type lists
			TypeInfo cmpType = cmp.GetType().GetTypeInfo();
			List<Component> cmpList;
			if (!this.componentsByType.TryGetValue(cmpType, out cmpList))
			{
				cmpList = new List<Component>();
				this.componentsByType[cmpType] = cmpList;
			}
			cmpList.Add(cmp);

			// Specialized lists
			ICmpRenderer renderer = cmp as ICmpRenderer;
			if (renderer != null) this.visibilityStrategy.AddRenderer(renderer);
		}
		private void RemoveFromManagers(GameObject obj)
		{
			foreach (Component cmp in obj.Components)
				this.RemoveFromManagers(cmp);
		}
		private void RemoveFromManagers(Component cmp)
		{
			// Per-Type lists
			TypeInfo cmpType = cmp.GetType().GetTypeInfo();
			List<Component> cmpList;
			if (this.componentsByType.TryGetValue(cmpType, out cmpList))
				cmpList.Remove(cmp);

			// Specialized lists
			ICmpRenderer renderer = cmp as ICmpRenderer;
			if (renderer != null) this.visibilityStrategy.RemoveRenderer(renderer);
		}
		private void RegisterManagerEvents()
		{
			this.objectManager.GameObjectsAdded   += this.objectManager_GameObjectsAdded;
			this.objectManager.GameObjectsRemoved += this.objectManager_GameObjectsRemoved;
			this.objectManager.ParentChanged      += this.objectManager_ParentChanged;
			this.objectManager.ComponentAdded     += this.objectManager_ComponentAdded;
			this.objectManager.ComponentRemoving  += this.objectManager_ComponentRemoving;
		}
		private void UnregisterManagerEvents()
		{
			this.objectManager.GameObjectsAdded   -= this.objectManager_GameObjectsAdded;
			this.objectManager.GameObjectsRemoved -= this.objectManager_GameObjectsRemoved;
			this.objectManager.ParentChanged      -= this.objectManager_ParentChanged;
			this.objectManager.ComponentAdded     -= this.objectManager_ComponentAdded;
			this.objectManager.ComponentRemoving  -= this.objectManager_ComponentRemoving;
		}
		
		private void objectManager_GameObjectsAdded(object sender, GameObjectGroupEventArgs e)
		{
			foreach (GameObject obj in e.Objects)
			{
				this.AddToManagers(obj);
				obj.Scene = this;
			}
			if (this.IsCurrent) OnGameObjectsAdded(e);
		}
		private void objectManager_GameObjectsRemoved(object sender, GameObjectGroupEventArgs e)
		{
			foreach (GameObject obj in e.Objects)
			{
				this.RemoveFromManagers(obj);
				obj.Scene = null;
			}
			if (this.IsCurrent) OnGameObjectsRemoved(e);
		}
		private void objectManager_ParentChanged(object sender, GameObjectParentChangedEventArgs e)
		{
			if (this.IsCurrent) OnGameObjectParentChanged(e);
		}
		private void objectManager_ComponentAdded(object sender, ComponentEventArgs e)
		{
			this.AddToManagers(e.Component);
			if (this.IsCurrent) OnComponentAdded(e);
		}
		private void objectManager_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			this.RemoveFromManagers(e.Component);
			if (this.IsCurrent) OnComponentRemoving(e);
		}

		protected override void OnSaving(string saveAsPath)
		{
			base.OnSaving(saveAsPath);

			// Prepare all components for saving in reverse order, sorted by type
			List<ICmpSerializeListener> initList = this.FindComponents<ICmpSerializeListener>().ToList();
			for (int i = initList.Count - 1; i >= 0; i--)
				initList[i].OnSaving();

			this.serializeObj = this.objectManager.AllObjects.ToArray();
			this.serializeObj.StableSort(SerializeGameObjectComparison);
		}
		protected override void OnSaved(string saveAsPath)
		{
			if (this.serializeObj != null)
				this.serializeObj = null;

			base.OnSaved(saveAsPath);
			
			// Re-initialize all components after saving, sorted by type
			List<ICmpSerializeListener> initList = this.FindComponents<ICmpSerializeListener>().ToList();
			for (int i = 0; i < initList.Count; i++)
				initList[i].OnSaved();

			// If this Scene is the current one, but it wasn't saved before, update the current Scenes internal ContentRef
			if (this.IsCurrent && current.IsRuntimeResource)
				current = new ContentRef<Scene>(this, saveAsPath);
		}
		protected override void OnLoaded()
		{
			if (this.visibilityStrategy == null)
				this.visibilityStrategy = new DefaultRendererVisibilityStrategy();

			if (this.serializeObj != null)
			{
				this.UnregisterManagerEvents();
				foreach (GameObject obj in this.serializeObj)
				{
					obj.EnsureConsistentData();
					obj.EnsureComponentOrder();
				}
				foreach (GameObject obj in this.serializeObj)
				{
					obj.Scene = this;
					this.objectManager.AddObject(obj);
					this.AddToManagers(obj);
				}
				this.RegisterManagerEvents();
				this.serializeObj = null;
			}

			base.OnLoaded();

			this.ApplyPrefabLinks();
			
			// Initialize all loaded components, sorted by type
			List<ICmpSerializeListener> initList = this.FindComponents<ICmpSerializeListener>().ToList();
			for (int i = 0; i < initList.Count; i++)
				initList[i].OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);

			if (current.ResWeak == this) Current = null;

			GameObject[] obj = this.objectManager.AllObjects.ToArray();
			this.objectManager.Clear();
			foreach (GameObject g in obj) g.DisposeLater();
		}

		private static int SerializeGameObjectComparison(GameObject a, GameObject b)
		{
			int depthA = 0;
			int depthB = 0;
			while (a.Parent != null)
			{
				a = a.Parent;
				++depthA;
			}
			while (b.Parent != null)
			{
				b = b.Parent;
				++depthB;
			}
			return depthA - depthB;
		}
	}
}
