using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Manages a set of <see cref="GameObject">GameObject</see> and exposes suitable object enumerations as well as un/registeration events.
	/// If a registered object has been disposed, it will be automatically unregistered.
	/// </summary>
	public class GameObjectManager
	{
		private	List<GameObject>	allObj	= new List<GameObject>();


		/// <summary>
		/// [GET] The number of registered objects.
		/// </summary>
		public int Count
		{
			get { return this.allObj.Count; }
		}
		/// <summary>
		/// [GET] Enumerates all registered GameObjects.
		/// </summary>
		public IEnumerable<GameObject> AllObjects
		{
			get 
			{
				return this.allObj.Where(o => !o.Disposed);
			}
		}
		/// <summary>
		/// [GET] Enumerates all registered GameObjects that are currently active.
		/// </summary>
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
		public IEnumerable<GameObject> ActiveRootObjects
		{
			get
			{
				return this.allObj.Where(o => o.Parent == null && o.Active);
			}
		}
		

		/// <summary>
		/// Fired when a GameObject is registered
		/// </summary>
		public event EventHandler<GameObjectEventArgs>	GameObjectAdded;
		/// <summary>
		/// Fired when a GameObject is unregistered
		/// </summary>
		public event EventHandler<GameObjectEventArgs>	GameObjectRemoved;
		/// <summary>
		/// Fired when a registered GameObjects parent has changed
		/// </summary>
		public event EventHandler<GameObjectParentChangedEventArgs>	ParentChanged;
		/// <summary>
		/// Fired when a <see cref="Duality.Component"/> is added to an already registered GameObject.
		/// </summary>
		public event EventHandler<ComponentEventArgs> ComponentAdded;
		/// <summary>
		/// Fired when a <see cref="Duality.Component"/> is removed from an already registered GameObject.
		/// </summary>
		public event EventHandler<ComponentEventArgs> ComponentRemoving;
		
		

		/// <summary>
		/// Registers a GameObject and all of its children.
		/// </summary>
		/// <param name="obj"></param>
		public bool AddObject(GameObject obj)
		{
			return this.AddObjectDeep(obj);
		}
		/// <summary>
		/// Registers a set of GameObjects
		/// </summary>
		/// <param name="objEnum"></param>
		public void AddObject(IEnumerable<GameObject> objEnum)
		{
			foreach (GameObject obj in objEnum.ToArray())
				this.AddObjectDeep(obj);
		}
		/// <summary>
		/// Unregisters a GameObject and all of its children
		/// </summary>
		/// <param name="obj"></param>
		public bool RemoveObject(GameObject obj)
		{
			bool removed = this.RemoveObjectDeep(obj);;
			this.Flush();
			return removed;
		}
		/// <summary>
		/// Unregisters a set of GameObjects
		/// </summary>
		/// <param name="objEnum"></param>
		public void RemoveObject(IEnumerable<GameObject> objEnum)
		{
			foreach (GameObject obj in objEnum.ToArray())
				this.RemoveObjectDeep(obj);
			this.Flush();
		}
		/// <summary>
		/// Unregisters all GameObjects.
		/// </summary>
		public void Clear()
		{
			foreach (GameObject obj in this.allObj)
				this.OnObjectRemoved(obj);
			this.allObj.Clear();
		}
		/// <summary>
		/// Unregisters all dead / disposed GameObjects
		/// </summary>
		public void Flush()
		{
			List<GameObject> removed;
			this.allObj.FlushDisposedObj(out removed);
			foreach (GameObject obj in removed)
				this.OnObjectRemoved(obj);
		}


		private bool AddObjectDeep(GameObject obj)
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
		private bool RemoveObjectDeep(GameObject obj)
		{
			foreach (GameObject child in obj.Children)
			{
				this.RemoveObjectDeep(child);
			}
			bool removed = this.allObj.Remove(obj);
			if (removed) this.OnObjectRemoved(obj);
			return removed;
		}

		private void RegisterEvents(GameObject obj)
		{
			obj.EventParentChanged		+= this.OnParentChanged;
			obj.EventComponentAdded		+= this.OnComponentAdded;
			obj.EventComponentRemoving	+= this.OnComponentRemoving;
		}
		private void UnregisterEvents(GameObject obj)
		{
			obj.EventParentChanged		-= this.OnParentChanged;
			obj.EventComponentAdded		-= this.OnComponentAdded;
			obj.EventComponentRemoving	-= this.OnComponentRemoving;
		}

		private void OnObjectAdded(GameObject obj)
		{
			this.RegisterEvents(obj);
			if (this.GameObjectAdded != null)
				this.GameObjectAdded(this, new GameObjectEventArgs(obj));
		}
		private void OnObjectRemoved(GameObject obj)
		{
			this.UnregisterEvents(obj);
			if (this.GameObjectRemoved != null)
				this.GameObjectRemoved(this, new GameObjectEventArgs(obj));
		}
		private void OnParentChanged(object sender, GameObjectParentChangedEventArgs e)
		{
			if (this.ParentChanged != null)
				this.ParentChanged(sender, e);
		}
		private void OnComponentAdded(object sender, ComponentEventArgs e)
		{
			if (this.ComponentAdded != null)
				this.ComponentAdded(sender, e);
		}
		private void OnComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this.ComponentRemoving != null)
				this.ComponentRemoving(sender, e);
		}
	}
}
