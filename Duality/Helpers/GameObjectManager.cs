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
		public event EventHandler<GameObjectEventArgs>	Registered;
		/// <summary>
		/// Fired when a GameObject is unregistered
		/// </summary>
		public event EventHandler<GameObjectEventArgs>	Unregistered;
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
		public bool RegisterObj(GameObject obj)
		{
			return this.RegisterObjDeep(obj);
		}
		/// <summary>
		/// Registers a set of GameObjects
		/// </summary>
		/// <param name="objEnum"></param>
		public void RegisterObj(IEnumerable<GameObject> objEnum)
		{
			foreach (GameObject obj in objEnum.ToArray())
				this.RegisterObjDeep(obj);
		}
		/// <summary>
		/// Unregisters a GameObject and all of its children
		/// </summary>
		/// <param name="obj"></param>
		public bool UnregisterObj(GameObject obj)
		{
			bool removed = this.UnregisterObjDeep(obj);;
			this.Flush();
			return removed;
		}
		/// <summary>
		/// Unregisters a set of GameObjects
		/// </summary>
		/// <param name="objEnum"></param>
		public void UnregisterObj(IEnumerable<GameObject> objEnum)
		{
			foreach (GameObject obj in objEnum.ToArray())
				this.UnregisterObjDeep(obj);
			this.Flush();
		}
		/// <summary>
		/// Unregisters all GameObjects.
		/// </summary>
		public void Clear()
		{
			foreach (GameObject obj in this.allObj)
				this.OnUnregistered(obj);
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
				this.OnUnregistered(obj);
		}


		private bool RegisterObjDeep(GameObject obj)
		{
			bool added = false;
			if (!this.allObj.Contains(obj))
			{
				this.allObj.Add(obj);
				this.OnRegistered(obj);
				added = true;
			}
			foreach (GameObject child in obj.Children)
			{
				this.RegisterObjDeep(child);
			}
			return added;
		}
		private bool UnregisterObjDeep(GameObject obj)
		{
			foreach (GameObject child in obj.Children)
			{
				this.UnregisterObjDeep(child);
			}
			bool removed = this.allObj.Remove(obj);
			if (removed) this.OnUnregistered(obj);
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

		private void OnRegistered(GameObject obj)
		{
			this.RegisterEvents(obj);
			if (this.Registered != null)
				this.Registered(this, new GameObjectEventArgs(obj));
		}
		private void OnUnregistered(GameObject obj)
		{
			this.UnregisterEvents(obj);
			if (this.Unregistered != null)
				this.Unregistered(this, new GameObjectEventArgs(obj));
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
