﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duality.Utility;
using OpenTK;
using FarseerPhysics.Dynamics;

using Duality.Editor;
using Duality.Components;
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
    [Serializable]
    [EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryNone)]
    [EditorHintImage(typeof(CoreRes), CoreResNames.ImageScene)]
    public sealed class Scene : Resource, IGameObjectManager
    {
        /// <summary>
        /// A Scene resources file extension.
        /// </summary>
        public new const string FileExt = ResourceFileExtension.SceneFileExtension;
        private const float PhysicsAccStart = Time.MsPFMult;
        private List<GameObject> allObj = new List<GameObject>();


        private static World physicsWorld = new World(Vector2.Zero);
        private static float physicsAcc = 0.0f;
        private static bool physicsLowFps = false;
        private static ContentRef<Scene> current = new Scene();
        private static bool curAutoGen = false;
        private static bool isSwitching = false;
        private static int switchLock = 0;
        private static bool switchToScheduled = false;
        private static ContentRef<Scene> switchToTarget = null;


        /// <summary>
        /// [GET] When using fixed-timestep physics, the alpha value [0.0 - 1.0] indicates how
        /// complete the next step is. This is used for linear interpolation inbetween fixed physics steps.
        /// </summary>
        public static float PhysicsAlpha
        {
            get { return physicsAcc / Time.MsPFMult; }
        }
        /// <summary>
        /// [GET] Is fixed-timestep physics calculation currently active?
        /// </summary>
        public static bool PhysicsFixedTime
        {
            get { return DualityApp.AppData.PhysicsFixedTime && !physicsLowFps; }
        }
        /// <summary>
        /// [GET] Returns the current physics world.
        /// </summary>
        public static World PhysicsWorld
        {
            get { return physicsWorld; }
        }
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
        /// Fired when a <see cref="GameObject"/> has been registered in the current Scene.
        /// </summary>
        public static event EventHandler<GameObjectEventArgs> GameObjectAdded;
        /// <summary>
        /// Fired when a <see cref="GameObject"/> has been unregistered from the current Scene.
        /// </summary>
        public static event EventHandler<GameObjectEventArgs> GameObjectRemoved;
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

        private static void OnLeaving()
        {
            switchLock++;
            if (Leaving != null) Leaving(current, null);
            isSwitching = true;
            if (current.ResWeak != null)
            {
                foreach (GameObject o in current.ResWeak.ActiveObjects.ToArray()) o.OnDeactivate();
                physicsWorld.Clear();
                ResetPhysics();
            }
            switchLock--;
        }
        private static void OnEntered()
        {
            switchLock++;
            if (current.ResWeak != null)
            {
                // Apply physical properties
                ResetPhysics();
                physicsWorld.Gravity = PhysicsConvert.ToPhysicalUnit(current.ResWeak.GlobalGravity / Time.SPFMult);

                // When in the editor, apply prefab links
                if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor)
                    current.ResWeak.ApplyPrefabLinks();

                // When running the game, break prefab links
                if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
                    current.ResWeak.BreakPrefabLinks();

                // Activate GameObjects
                foreach (GameObject o in current.ResWeak.ActiveObjects.ToArray())
                    o.OnActivate();
            }
            isSwitching = false;
            if (Entered != null) Entered(current, null);
            switchLock--;
        }
        private static void OnGameObjectParentChanged(GameObjectParentChangedEventArgs args)
        {
            if (GameObjectParentChanged != null) GameObjectParentChanged(current, args);
        }
        private static void OnGameObjectAdded(GameObjectEventArgs args)
        {
            args.Object.OnActivate();
            if (GameObjectAdded != null) GameObjectAdded(current, args);
        }
        private static void OnGameObjectRemoved(GameObjectEventArgs args)
        {
            if (GameObjectRemoved != null) GameObjectRemoved(current, args);
            args.Object.OnDeactivate();
        }
        private static void OnComponentAdded(ComponentEventArgs args)
        {
            if (args.Component.Active)
            {
                ICmpInitializable cInit = args.Component as ICmpInitializable;
                if (cInit != null) cInit.OnInit(Component.InitContext.Activate);
            }
            if (ComponentAdded != null) ComponentAdded(current, args);
        }
        private static void OnComponentRemoving(ComponentEventArgs args)
        {
            if (args.Component.Active)
            {
                ICmpInitializable cInit = args.Component as ICmpInitializable;
                if (cInit != null) cInit.OnShutdown(Component.ShutdownContext.Deactivate);
            }
            if (ComponentRemoving != null) ComponentRemoving(current, args);
        }


        private Vector2 globalGravity = Vector2.UnitY * 33.0f;
        private GameObject[] serializeObj = null;
        //[NonSerialized]
        //[CloneField(CloneFieldFlags.DontSkip)]
        //[CloneBehavior(typeof(GameObject), CloneBehavior.ChildObject)]
        //private GameObjectManager objectManager = new GameObjectManager();
        [NonSerialized]
        [CloneField(CloneFieldFlags.DontSkip)]
        private List<Component> renderers = new List<Component>();
        [NonSerialized]
        [CloneField(CloneFieldFlags.DontSkip)]
        private Dictionary<Type, List<Component>> componentyByType = new Dictionary<Type, List<Component>>();


        /// <summary>
        /// [GET] Enumerates all registered objects.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public IEnumerable<GameObject> AllObjects
        {
            get
            {
                return this.allObj.Where(o => !o.Disposed);
            }
        }
        /// <summary>
        /// [GET] Enumerates all registered objects that are currently active.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public IEnumerable<GameObject> ActiveObjects
        {
            get
            {
                return this.allObj.Where(o => o.Active);
            }
        }
        /// <summary>
        /// [GET] Enumerates all root GameObjects, i.e. all GameObjects without a parent object.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public IEnumerable<GameObject> RootObjects
        {
            get
            {
                return this.allObj.Where(o => !o.Disposed && o.Parent == null);
            }
        }
        /// <summary>
        /// [GET] Enumerates all <see cref="RootObjects"/> that are currently active.
        /// </summary>
        [EditorHintFlags(MemberFlags.Invisible)]
        public IEnumerable<GameObject> ActiveRootObjects
        {
            get
            {
                return this.allObj.Where(o => o.Parent == null && o.Active);
            }
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
                if (this.IsCurrent)
                {
                    physicsWorld.Gravity = PhysicsConvert.ToPhysicalUnit(value / Time.SPFMult);
                    foreach (Body b in physicsWorld.BodyList)
                    {
                        if (b.IgnoreGravity || b.BodyType != BodyType.Dynamic) continue;
                        b.Awake = true;
                    }
                }
            }
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
            get { return !this.AllObjects.Any(); }
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
        /// <param name="viewportRect">The viewport to which will be rendered.</param>
        /// <param name="camPredicate">Optional predicate to select which Cameras may be rendered and which not.</param>
        internal void Render(Rect viewportRect, Predicate<Camera> camPredicate = null)
        {
            if (!this.IsCurrent) throw new InvalidOperationException("Can't render non-current Scene!");
            switchLock++;

            Camera[] activeCams = this.FindComponents<Camera>().Where(c => c.Active && (camPredicate == null || camPredicate(c))).ToArray();
            // Maybe sort / process list first later on.
            foreach (Camera c in activeCams)
                c.Render(viewportRect);

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
            bool physUpdate = false;
            double physBegin = Time.MainTimer.TotalMilliseconds;
            if (Scene.PhysicsFixedTime)
            {
                physicsAcc += Time.MsPFMult * Time.TimeMult;
                int iterations = 0;
                if (physicsAcc >= Time.MsPFMult)
                {
                    Profile.TimeUpdatePhysics.BeginMeasure();
                    double timeUpdateBegin = Time.MainTimer.TotalMilliseconds;
                    while (physicsAcc >= Time.MsPFMult)
                    {
                        // Catch up on updating progress
                        FarseerPhysics.Settings.VelocityThreshold = PhysicsConvert.ToPhysicalUnit(DualityApp.AppData.PhysicsVelocityThreshold / Time.SPFMult);
                        physicsWorld.Step(Time.SPFMult);
                        physicsAcc -= Time.MsPFMult;
                        iterations++;

                        double timeSpent = Time.MainTimer.TotalMilliseconds - timeUpdateBegin;
                        if (timeSpent >= Time.MsPFMult * 10.0f) break; // Emergency exit
                    }
                    physUpdate = true;
                    Profile.TimeUpdatePhysics.EndMeasure();
                }
            }
            else
            {
                Profile.TimeUpdatePhysics.BeginMeasure();
                FarseerPhysics.Settings.VelocityThreshold = PhysicsConvert.ToPhysicalUnit(Time.TimeMult * DualityApp.AppData.PhysicsVelocityThreshold / Time.SPFMult);
                physicsWorld.Step(Time.TimeMult * Time.SPFMult);
                if (Time.TimeMult == 0.0f) physicsWorld.ClearForces(); // Complete freeze? Clear forces, so they don't accumulate.
                physicsAcc = PhysicsAccStart;
                physUpdate = true;
                Profile.TimeUpdatePhysics.EndMeasure();
            }
            double physTime = Time.MainTimer.TotalMilliseconds - physBegin;

            // Apply Farseers internal measurements to Duality
            if (physUpdate)
            {
                Profile.TimeUpdatePhysicsAddRemove.Set(1000.0f * physicsWorld.AddRemoveTime / System.Diagnostics.Stopwatch.Frequency);
                Profile.TimeUpdatePhysicsContacts.Set(1000.0f * physicsWorld.ContactsUpdateTime / System.Diagnostics.Stopwatch.Frequency);
                Profile.TimeUpdatePhysicsContinous.Set(1000.0f * physicsWorld.ContinuousPhysicsTime / System.Diagnostics.Stopwatch.Frequency);
                Profile.TimeUpdatePhysicsController.Set(1000.0f * physicsWorld.ControllersUpdateTime / System.Diagnostics.Stopwatch.Frequency);
                Profile.TimeUpdatePhysicsSolve.Set(1000.0f * physicsWorld.SolveUpdateTime / System.Diagnostics.Stopwatch.Frequency);
            }

            // Update low fps physics state
            if (!physicsLowFps)
                physicsLowFps = Time.LastDelta > Time.MsPFMult && physTime > Time.LastDelta * 0.85f;
            else
                physicsLowFps = !(Time.LastDelta < Time.MsPFMult * 0.9f || physTime < Time.LastDelta * 0.6f);

            Profile.TimeUpdateScene.BeginMeasure();
            {
                // Update all GameObjects
                GameObject[] activeObj = this.ActiveObjects.ToArray();
                foreach (GameObject obj in activeObj)
                    obj.Update();
            }
            Profile.TimeUpdateScene.EndMeasure();

            // Perform a scheduled Scene switch
            if (switchToScheduled)
            {
                Scene.Current = switchToTarget.Res;
                switchToTarget = null;
                switchToScheduled = false;
            }

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
            {
                // Update all GameObjects
                GameObject[] activeObj = this.ActiveObjects.ToArray();
                foreach (GameObject obj in activeObj)
                    obj.EditorUpdate();
            }
            Profile.TimeUpdateScene.EndMeasure();

            switchLock--;
        }
        /// <summary>
        /// Cleanes up disposed Scene objects.
        /// </summary>
        new internal void RunCleanup()
        {
            this.Flush();
            this.renderers.FlushDisposedObj();
            foreach (var cmpList in this.componentyByType.Values)
                cmpList.FlushDisposedObj();
        }

        /// <summary>
        /// Applies all <see cref="Duality.Resources.PrefabLink">PrefabLinks</see> contained withing this
        /// Scenes <see cref="GameObject">GameObjects</see>.
        /// </summary>
        public void ApplyPrefabLinks()
        {
            PrefabLink.ApplyAllLinks(this.AllObjects);
        }
        /// <summary>
        /// Breaks all <see cref="Duality.Resources.PrefabLink">PrefabLinks</see> contained withing this
        /// Scenes <see cref="GameObject">GameObjects</see>.
        /// </summary>
        public void BreakPrefabLinks()
        {
            foreach (GameObject obj in this.AllObjects)
                obj.BreakPrefabLink();
        }

        /// <summary>
        /// Clears the Scene, unregistering all GameObjects. This does not <see cref="GameObject.Dispose">dispose</see> them.
        /// </summary>
        public void Clear()
        {
            foreach (GameObject obj in this.allObj)
                this.OnObjectRemoved(obj);
            this.allObj.Clear();
        }
        /// <summary>
        /// Appends a cloned version of the specified Scenes contents to this Scene.
        /// </summary>
        /// <param name="scene">The source Scene.</param>
        public void Append(ContentRef<Scene> scene)
        {
            if (!scene.IsAvailable) return;
            this.AddObject(scene.Res.RootObjects.Select(o => o.Clone()));
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
            this.AddObject(otherObj);
            otherScene.Dispose();
        }

        /// <summary>
        /// Registers a GameObject and all of its children.
        /// </summary>
        /// <param name="obj"></param>
        public void AddObject(GameObject obj)
        {
            if (obj.ParentScene != null && obj.ParentScene != this) obj.ParentScene.RemoveObject(obj);
            this.ObjectManagerAddObject(obj);
        }
        /// <summary>
        /// Registers a set of GameObjects and all of their children.
        /// </summary>
        /// <param name="objEnum"></param>
        public void AddObject(IEnumerable<GameObject> objEnum)
        {
            foreach (GameObject obj in objEnum)
            {
                if (obj.ParentScene == null || obj.ParentScene == this) continue;
                obj.ParentScene.RemoveObject(obj);
            }
            this.ObjectManagerAddObject(objEnum);
        }

        private void ObjectManagerAddObject(IEnumerable<GameObject> objEnum)
        {
            foreach (GameObject obj in objEnum.ToArray())
                this.AddObjectDeep(obj);
        }
        /// <summary>
        /// Unregisters a GameObject and all of its children
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(GameObject obj)
        {
            if (obj.ParentScene != this) return;
            if (obj.Parent != null && obj.Parent.ParentScene == this)
            {
                obj.Parent = null;
            }
            this.ObjectManagerRemoveObject(obj);
        }
        /// <summary>
        /// Unregisters a set of GameObjects and all of their children.
        /// </summary>
        /// <param name="objEnum"></param>
        public void RemoveObject(IEnumerable<GameObject> objEnum)
        {
            objEnum = objEnum.Where(o => o.ParentScene == this);
            foreach (GameObject obj in objEnum)
            {
                if (obj.Parent == null) continue;
                if (obj.Parent.ParentScene != this) continue;
                obj.Parent = null;
            }
            this.ObjectManagerRemoveObject(objEnum);
        }

        private void ObjectManagerRemoveObject(IEnumerable<GameObject> objEnum)
        {
            foreach (GameObject obj in objEnum.ToArray())
            {
                this.RemoveObjectDeep(obj);
            }
            this.Flush();
        }

        /// <summary>
        /// Enumerates all <see cref="Duality.Components.Renderer">Renderers</see> that are visible to
        /// the specified <see cref="IDrawDevice"/>.
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public IEnumerable<ICmpRenderer> QueryVisibleRenderers(IDrawDevice device)
        {
            return this.renderers.Where(r => r.Active && (r as ICmpRenderer).IsVisible(device)).OfType<ICmpRenderer>();
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
            // Determine which by-type lists to use
            bool multiple = false;
            List<Component> singleResult = null;
            List<List<Component>> query = null;
            foreach (var pair in this.componentyByType)
            {
                if (type.IsAssignableFrom(pair.Key))
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
                            query = new List<List<Component>>(this.componentyByType.Values.Count);
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
            foreach (var pair in this.componentyByType)
            {
                if (type.IsAssignableFrom(pair.Key))
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
            foreach (Component cmp in obj.GetComponents<Component>())
                this.AddToManagers(cmp);
        }
        private void AddToManagers(Component cmp)
        {
            // Per-Type lists
            Type cmpType = cmp.GetType();
            List<Component> cmpList;
            if (!this.componentyByType.TryGetValue(cmpType, out cmpList))
            {
                cmpList = new List<Component>();
                this.componentyByType[cmpType] = cmpList;
            }
            cmpList.Add(cmp);

            // Specialized lists
            if (cmp is ICmpRenderer) this.renderers.Add(cmp);
        }
        private void RemoveFromManagers(GameObject obj)
        {
            foreach (Component cmp in obj.GetComponents<Component>())
                this.RemoveFromManagers(cmp);
        }
        private void RemoveFromManagers(Component cmp)
        {
            // Per-Type lists
            Type cmpType = cmp.GetType();
            List<Component> cmpList;
            if (this.componentyByType.TryGetValue(cmpType, out cmpList))
                cmpList.Remove(cmp);

            // Specialized lists
            if (cmp is ICmpRenderer) this.renderers.Remove(cmp);
        }
        private void RegisterManagerEvents()
        {
            this.ObjectManagerGameObjectAdded += this.objectManager_GameObjectAdded;
            this.ObjectManagerGameObjectRemoved += this.objectManager_GameObjectRemoved;
            this.ObjectManagerParentChanged += this.objectManager_ParentChanged;
            this.ObjectManagerComponentAdded += this.objectManager_ComponentAdded;
            this.ObjectManagerComponentRemoving += this.objectManager_ComponentRemoving;
        }
        private void UnregisterManagerEvents()
        {
            this.ObjectManagerGameObjectAdded -= this.objectManager_GameObjectAdded;
            this.ObjectManagerGameObjectRemoved -= this.objectManager_GameObjectRemoved;
            this.ObjectManagerParentChanged -= this.objectManager_ParentChanged;
            this.ObjectManagerComponentAdded -= this.objectManager_ComponentAdded;
            this.ObjectManagerComponentRemoving -= this.objectManager_ComponentRemoving;
        }

        private static void ResetPhysics()
        {
            physicsLowFps = false;
            physicsAcc = PhysicsAccStart;
        }
        /// <summary>
        /// Awakes all currently existing physical objects.
        /// </summary>
        public static void AwakePhysics()
        {
            foreach (Body b in physicsWorld.BodyList)
                b.Awake = true;
        }

        private void objectManager_GameObjectAdded(object sender, GameObjectEventArgs e)
        {
            this.AddToManagers(e.Object);
            e.Object.ParentScene = this;
            if (this.IsCurrent) OnGameObjectAdded(e);
        }
        private void objectManager_GameObjectRemoved(object sender, GameObjectEventArgs e)
        {
            this.RemoveFromManagers(e.Object);
            e.Object.ParentScene = null;
            if (this.IsCurrent) OnGameObjectRemoved(e);
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
            foreach (GameObject obj in this.AllObjects)
                obj.OnSaving();

            this.serializeObj = this.AllObjects.ToArray();
            this.serializeObj.StableSort(SerializeGameObjectComparison);
        }
        protected override void OnSaved(string saveAsPath)
        {
            if (this.serializeObj != null)
                this.serializeObj = null;

            base.OnSaved(saveAsPath);
            foreach (GameObject obj in this.AllObjects)
                obj.OnSaved();

            // If this Scene is the current one, but it wasn't saved before, update the current Scenes internal ContentRef
            if (this.IsCurrent && current.IsRuntimeResource)
                current = new ContentRef<Scene>(this, saveAsPath);
        }
        protected override void OnLoaded()
        {
            if (this.serializeObj != null)
            {
                this.UnregisterManagerEvents();
                foreach (GameObject obj in this.serializeObj) obj.PerformSanitaryCheck();
                foreach (GameObject obj in this.serializeObj)
                {
                    obj.ParentScene = this;
                    this.AddObject(obj);
                    this.AddToManagers(obj);
                }
                this.RegisterManagerEvents();
                this.serializeObj = null;
            }

            base.OnLoaded();

            this.ApplyPrefabLinks();

            foreach (GameObject obj in this.AllObjects)
                obj.OnLoaded();
        }
        protected override void OnDisposing(bool manually)
        {
            base.OnDisposing(manually);

            if (current.ResWeak == this) Current = null;

            GameObject[] obj = this.AllObjects.ToArray();
            this.Clear();
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

        public int Count
        {
            get { return this.allObj.Count; }
        }

        public event EventHandler<GameObjectEventArgs> ObjectManagerGameObjectAdded;

        public event EventHandler<GameObjectEventArgs> ObjectManagerGameObjectRemoved;

        public event EventHandler<GameObjectParentChangedEventArgs> ObjectManagerParentChanged;

        public event EventHandler<ComponentEventArgs> ObjectManagerComponentAdded;

        public event EventHandler<ComponentEventArgs> ObjectManagerComponentRemoving;

        bool ObjectManagerAddObject(GameObject obj)
        {
            return this.AddObjectDeep(obj);
        }

        bool ObjectManagerRemoveObject(GameObject obj)
        {
            bool removed = this.RemoveObjectDeep(obj);
            this.Flush();
            return removed;
        }

        public void Flush()
        {
            List<GameObject> removed;
            this.allObj.FlushDisposedObj(out removed);
            foreach (GameObject obj in removed)
                this.OnObjectRemoved(obj);
        }

        public bool AddObjectDeep(GameObject obj)
        {
            bool added = false;
            if (!this.allObj.Contains(obj))
            {
                this.allObj.Add(obj);
                this.OnObjectAdded(obj);
                added = true;
            }
            foreach (GameObject child in obj.Children)
            {
                this.AddObjectDeep(child);
            }
            return added;
        }

        public bool RemoveObjectDeep(GameObject obj)
        {
            foreach (GameObject child in obj.Children)
            {
                this.RemoveObjectDeep(child);
            }
            bool removed = this.allObj.Remove(obj);
            if (removed) this.OnObjectRemoved(obj);
            return removed;
        }

        public void RegisterEvents(GameObject obj)
        {
            obj.EventParentChanged += this.OnParentChanged;
            obj.EventComponentAdded += this.OnComponentAdded;
            obj.EventComponentRemoving += this.OnComponentRemoving;
        }

        public void UnregisterEvents(GameObject obj)
        {
            obj.EventParentChanged -= this.OnParentChanged;
            obj.EventComponentAdded -= this.OnComponentAdded;
            obj.EventComponentRemoving -= this.OnComponentRemoving;
        }

        public void OnObjectAdded(GameObject obj)
        {
            this.RegisterEvents(obj);
            if (this.ObjectManagerGameObjectAdded != null)
                this.ObjectManagerGameObjectAdded(this, new GameObjectEventArgs(obj));
        }

        public void OnObjectRemoved(GameObject obj)
        {
            this.UnregisterEvents(obj);
            if (this.ObjectManagerGameObjectRemoved != null)
                this.ObjectManagerGameObjectRemoved(this, new GameObjectEventArgs(obj));
        }

        public void OnParentChanged(object sender, GameObjectParentChangedEventArgs e)
        {
            if (this.ObjectManagerParentChanged != null)
                this.ObjectManagerParentChanged(sender, e);
        }

        public void OnComponentAdded(object sender, ComponentEventArgs e)
        {
            if (this.ObjectManagerComponentAdded != null)
                this.ObjectManagerComponentAdded(sender, e);
        }

        public void OnComponentRemoving(object sender, ComponentEventArgs e)
        {
            if (this.ObjectManagerComponentRemoving != null)
                this.ObjectManagerComponentRemoving(sender, e);
        }
    }
}
