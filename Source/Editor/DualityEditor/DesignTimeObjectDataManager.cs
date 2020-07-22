using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Serialization;
using Duality.Resources;

namespace Duality.Editor
{
	internal class DesignTimeObjectDataManager : ISerializeExplicit
	{
		private static readonly int GuidByteLength = Guid.Empty.ToByteArray().Length;
		private const int Version_First = 1;

		private	Dictionary<Guid,DesignTimeObjectData>	dataStore	= new Dictionary<Guid,DesignTimeObjectData>();

		public DesignTimeObjectData Request(Guid objId)
		{
			DesignTimeObjectData data;
			if (!this.dataStore.TryGetValue(objId, out data))
			{
				data = new DesignTimeObjectData(objId, DesignTimeObjectData.Default);
				this.dataStore[objId] = data;
			}
			return data;
		}
		public void Cleanup()
		{
			// Remove trivial / default data
			var removeQuery =
				from p in this.dataStore
				where p.Value.IsDefault
				select p.Value.ParentObjectId;
			foreach (Guid objId in removeQuery.ToArray())
				this.dataStore.Remove(objId);
		}
		public void Optimize()
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
			this.Cleanup();
			this.Optimize();

			Guid[] guidArray = this.dataStore.Keys.ToArray();
			byte[] data = new byte[guidArray.Length * GuidByteLength];
			for (int i = 0; i < guidArray.Length; i++)
			{
				Array.Copy(
					guidArray[i].ToByteArray(), 0,
					data, i * GuidByteLength, GuidByteLength);
			}
			DesignTimeObjectData.DataContainer[] objData = this.dataStore.Values.Select(d => d.Data).ToArray();
			bool[] objDataDirty = this.dataStore.Values.Select(d => d.IsAttached).ToArray();

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
				this.dataStore = new Dictionary<Guid, DesignTimeObjectData>();
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
