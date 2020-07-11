using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Resources;

namespace Duality.Editor
{
	public class DesignTimeObjectData
	{
		internal class DataContainer : IEquatable<DataContainer>
		{
			public	bool	hidden	= false;
			public	bool	locked	= false;
			public	Dictionary<Type,object>	custom	= null;

			public DataContainer() {}
			public DataContainer(DataContainer baseData)
			{
				this.hidden = baseData.hidden;
				this.locked = baseData.locked;
				this.custom = baseData.custom != null ? new Dictionary<Type,object>(baseData.custom) : null;
			}

			public static bool operator ==(DataContainer a, DataContainer b)
			{
				if (object.ReferenceEquals(a, b)) return true;
				if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) return false;

				if (a.hidden != b.hidden) return false;
				if (a.locked != b.locked) return false;

				if (a.custom != b.custom)
				{
					if (a.custom == null || b.custom == null) return false;
					if (a.custom.Count != b.custom.Count) return false;
					foreach (var pair in a.custom)
					{
						object valB;
						if (!b.custom.TryGetValue(pair.Key, out valB)) return false;
						if (!object.Equals(pair.Value, valB)) return false;
					}
				}

				return true;
			}
			public static bool operator !=(DataContainer a, DataContainer b)
			{
				return !(a == b);
			}
			public override bool Equals(object obj)
			{
				if (obj is DataContainer)
					return this.Equals(obj as DataContainer);
				else
					return base.Equals(obj);
			}
			public override int GetHashCode()
			{
				int hash = 17;
				MathF.CombineHashCode(hash, this.hidden.GetHashCode());
				MathF.CombineHashCode(hash, this.locked.GetHashCode());
				if (this.custom != null)
				{
					foreach (var pair in this.custom)
						MathF.CombineHashCode(hash, pair.Value.GetHashCode());
				}
				return hash;
			}
			public bool Equals(DataContainer other)
			{
				return this == other;
			}
		}

		internal static void Init()
		{
			Scene.Leaving += Scene_Leaving;
		}
		internal static void Terminate()
		{
			Scene.Leaving -= Scene_Leaving;
		}

		private static void Scene_Leaving(object sender, EventArgs e)
		{
			DualityEditorApp.DualityEditorUserData.Instance?.DesignTimeObjectDataManager.CleanupDesignTimeData();
		}

		public static readonly DesignTimeObjectData Default = new DesignTimeObjectData();

		private	Guid			objId		= Guid.Empty;
		private DataContainer	data		= null;
		private	bool			attached	= false;

		internal DataContainer Data
		{
			get { return this.data; }
		}
		internal bool IsAttached
		{
			get { return this.attached; }
		}
		public Guid ParentObjectId
		{
			get { return this.objId; }
		}
		public bool IsHidden
		{
			get { return this.data.hidden; }
			set
			{
				if (this.data.hidden != value)
				{
					this.Detach();
					this.data.hidden = value;
				}
			}
		}
		public bool IsLocked
		{
			get { return this.data.locked; }
			set
			{
				if (this.data.locked != value)
				{
					this.Detach();
					this.data.locked = value;
				}
			}
		}
		public bool IsDefault
		{
			get
			{
				if (object.ReferenceEquals(this, Default)) return true;
				return this.data == Default.data;
			}
		}


		private DesignTimeObjectData() : this(Guid.Empty) {}
		internal DesignTimeObjectData(Guid parentId, DataContainer data, bool dirty)
		{
			this.objId = parentId;
			this.data = data;
			this.attached = dirty;
		}
		public DesignTimeObjectData(Guid parentId)
		{
			this.objId = parentId;
			this.data = new DataContainer();
		}
		public DesignTimeObjectData(Guid parentId, DesignTimeObjectData baseData)
		{
			this.objId = parentId;
			this.data = baseData.data;
			this.attached = true;
		}

		public static DesignTimeObjectData Get(GameObject obj)
		{
			return DualityEditorApp.DualityEditorUserData.Instance.DesignTimeObjectDataManager.RequestDesignTimeData(obj.Id);
		}

		public T RequestCustomData<T>() where T : new()
		{
			this.Detach();

			if (this.data.custom == null) this.data.custom = new Dictionary<Type,object>();

			object val;
			if (!this.data.custom.TryGetValue(typeof(T), out val))
			{
				T newVal = new T();
				this.data.custom[typeof(T)] = newVal;
				return newVal;
			}
			else
			{
				return (T)val;
			}
		}
		public void RemoveCustomData<T>()
		{
			this.Detach();

			if (this.data.custom == null) return;
			this.data.custom.Remove(typeof(T));
		}

		internal bool TryShareData(DesignTimeObjectData other)
		{
			if (object.ReferenceEquals(this.data, other.data)) return true;
			if (this.data == other.data)
			{
				other.data = this.data;
				this.attached = true;
				other.attached = true;
				return true;
			}
			return false;
		}
		private void Detach()
		{
			if (!this.attached) return;
			if (this.data == null)	this.data = new DataContainer();
			else					this.data = new DataContainer(this.data);
			this.attached = false;
		}
	}

	public class DesignTimeObjectDataManager
	{
		private	Dictionary<Guid,DesignTimeObjectData>	dataStore	= new Dictionary<Guid,DesignTimeObjectData>();

		public DesignTimeObjectData RequestDesignTimeData(Guid objId)
		{
			DesignTimeObjectData data;
			if (!this.dataStore.TryGetValue(objId, out data))
			{
				data = new DesignTimeObjectData(objId, DesignTimeObjectData.Default);
				this.dataStore[objId] = data;
			}
			return data;
		}
		public void CleanupDesignTimeData()
		{
			// Remove trivial / default data
			var removeQuery = 
				from p in this.dataStore
				where p.Value.IsDefault
				select p.Value.ParentObjectId;
			foreach (Guid objId in removeQuery.ToArray())
				this.dataStore.Remove(objId);
		}
		public void OptimizeDesignTimeData()
		{
			// Optimize data by sharing
			var shareValues = this.dataStore.Values.ToList();
			while (shareValues.Count > 0)
			{
				DesignTimeObjectData data = shareValues[shareValues.Count - 1];
				foreach (DesignTimeObjectData other in shareValues)
				{
					data.TryShareData(other);
				}
				shareValues.RemoveAt(shareValues.Count - 1);
			}
		}
	}
}
