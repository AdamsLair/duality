using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

using Duality;
using Duality.Serialization;
using Duality.Resources;

namespace Duality.Editor
{
	public class DesignTimeObjectData
	{
		[Serializable]
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

		private	static	DesignTimeObjectDataManager	manager	= new DesignTimeObjectDataManager();
		
		internal static void Init()
		{
			Load(DualityEditorApp.DesignTimeDataFile);
			Scene.Leaving += Scene_Leaving;
		}
		internal static void Terminate()
		{
			Scene.Leaving -= Scene_Leaving;
			Save(DualityEditorApp.DesignTimeDataFile);
		}

		public static DesignTimeObjectData Get(Guid objId)
		{
			return manager.RequestDesignTimeData(objId);
		}
		public static DesignTimeObjectData Get(GameObject obj)
		{
			return manager.RequestDesignTimeData(obj.Id);
		}

		private static void Save(string filePath)
		{
			Formatter.WriteObject(manager, filePath, FormattingMethod.Binary);
		}
		private static void Load(string filePath)
		{
			manager = Formatter.TryReadObject<DesignTimeObjectDataManager>(filePath) ?? new DesignTimeObjectDataManager();
		}
		private static void Scene_Leaving(object sender, EventArgs e)
		{
			manager.CleanupDesignTimeData();
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

	[Serializable]
	internal class DesignTimeObjectDataManager : ISerializeExplicit
	{
		private static readonly int GuidByteLength = Guid.Empty.ToByteArray().Length;
		private	const int Version_First	= 1;

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

		void ISerializeExplicit.WriteData(IDataWriter writer)
		{
			this.CleanupDesignTimeData();
			this.OptimizeDesignTimeData();

			Guid[] guidArray = dataStore.Keys.ToArray();
			byte[] data = new byte[guidArray.Length * GuidByteLength];
			for (int i = 0; i < guidArray.Length; i++)
			{
				Array.Copy(
					guidArray[i].ToByteArray(), 0, 
					data, i * GuidByteLength, GuidByteLength);
			}
			DesignTimeObjectData.DataContainer[] objData = dataStore.Values.Select(d => d.Data).ToArray();
			bool[] objDataDirty = dataStore.Values.Select(d => d.IsAttached).ToArray();

			writer.WriteValue("version", Version_First);
			writer.WriteValue("dataStoreKeys", data);
			writer.WriteValue("dataStoreValues", objData);
			writer.WriteValue("dataStoreDirtyFlag", objDataDirty);
		}
		void ISerializeExplicit.ReadData(IDataReader reader)
		{
			int version;
			reader.ReadValue("version", out version);
			
			if (this.dataStore == null)
				this.dataStore = new Dictionary<Guid,DesignTimeObjectData>();
			else
				this.dataStore.Clear();

			if (version == Version_First)
			{
				byte[] data;
				DesignTimeObjectData.DataContainer[] objData;
				bool[] objDataDirty;
				reader.ReadValue("dataStoreKeys", out data);
				reader.ReadValue("dataStoreValues", out objData);
				reader.ReadValue("dataStoreDirtyFlag", out objDataDirty);

				Guid[] guidArray = new Guid[data.Length / GuidByteLength];
				byte[] guidData = new byte[GuidByteLength];
				for (int i = 0; i < guidArray.Length; i++)
				{
					Array.Copy(
						data, i * GuidByteLength,
						guidData, 0, GuidByteLength);
					guidArray[i] = new Guid(guidData);
				}

				for (int i = 0; i < objData.Length; i++)
				{
					this.dataStore.Add(guidArray[i], new DesignTimeObjectData(guidArray[i], objData[i], objDataDirty[i]));
				}
			}
			else
			{
				// Unknown format
			}
		}
	}
}
