using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Utility
{
    public interface IGameObjectManager
    {
		/// <summary>
		/// [GET] The number of registered objects.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// [GET] Enumerates all registered GameObjects.
		/// </summary>
		IEnumerable<GameObject> AllObjects { get; }

		/// <summary>
		/// [GET] Enumerates all registered GameObjects that are currently active.
		/// </summary>
		IEnumerable<GameObject> ActiveObjects { get; }

		/// <summary>
		/// [GET] Enumerates all root GameObjects, i.e. all GameObjects without a parent object.
		/// </summary>
		IEnumerable<GameObject> RootObjects { get; }

		/// <summary>
		/// [GET] Enumerates all <see cref="RootObjects"/> that are currently active.
		/// </summary>
		IEnumerable<GameObject> ActiveRootObjects { get; }
		

		/// <summary>
		/// Fired when a GameObject is registered
		/// </summary>
		event EventHandler<GameObjectEventArgs>	ObjectManagerGameObjectAdded;
		/// <summary>
		/// Fired when a GameObject is unregistered
		/// </summary>
        event EventHandler<GameObjectEventArgs> ObjectManagerGameObjectRemoved;
		/// <summary>
		/// Fired when a registered GameObjects parent has changed
		/// </summary>
        event EventHandler<GameObjectParentChangedEventArgs> ObjectManagerParentChanged;
		/// <summary>
		/// Fired when a <see cref="Duality.Component"/> is added to an already registered GameObject.
		/// </summary>
        event EventHandler<ComponentEventArgs> ObjectManagerComponentAdded;
		/// <summary>
		/// Fired when a <see cref="Duality.Component"/> is removed from an already registered GameObject.
		/// </summary>
        event EventHandler<ComponentEventArgs> ObjectManagerComponentRemoving;

        /// <summary>
        /// Registers a GameObject and all of its children.
        /// </summary>
        /// <param name="obj"></param>
        bool AddObject(GameObject obj);
		/// <summary>
		/// Registers a set of GameObjects
		/// </summary>
		/// <param name="objEnum"></param>
		void AddObject(IEnumerable<GameObject> objEnum);
		
        /// <summary>
        /// Unregisters a GameObject and all of its children
        /// </summary>
        /// <param name="obj"></param>
        bool RemoveObject(GameObject obj);

        /// <summary>
        /// Unregisters a set of GameObjects
        /// </summary>
        /// <param name="objEnum"></param>
        void RemoveObject(IEnumerable<GameObject> objEnum);

        /// <summary>
        /// Unregisters all GameObjects.
        /// </summary>
        void Clear();

        /// <summary>
        /// Unregisters all dead / disposed GameObjects
        /// </summary>
        void Flush();
        
        bool AddObjectDeep(GameObject obj);

        bool RemoveObjectDeep(GameObject obj);

        void RegisterEvents(GameObject obj);

        void UnregisterEvents(GameObject obj);

        void OnObjectAdded(GameObject obj);

        void OnObjectRemoved(GameObject obj);

        void OnParentChanged(object sender, GameObjectParentChangedEventArgs e);

        void OnComponentAdded(object sender, ComponentEventArgs e);

        void OnComponentRemoving(object sender, ComponentEventArgs e);
    }

}

