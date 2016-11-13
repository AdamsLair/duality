using System;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Serialization.Surrogates
{
	/// <summary>
	/// De/Serializes a <see cref="System.Guid"/>.
	/// </summary>
	public class GuidSurrogate : SerializeSurrogate<Guid>
	{
		public override void WriteConstructorData(IDataWriter writer)
		{
			writer.WriteValue("data", this.RealObject.ToByteArray());
		}
		public override void WriteData(IDataWriter writer) {}
		public override object ConstructObject(IDataReader reader, TypeInfo objType)
		{
			byte[] data;
			reader.ReadValue("data", out data);
			return new Guid(data);
		}
		public override void ReadData(IDataReader reader) {}
	}
	/// <summary>
	/// De/Serializes an array of <see cref="System.Guid"/> objects.
	/// </summary>
	public class GuidArraySurrogate : SerializeSurrogate<Guid[]>
	{
		private static readonly int GuidByteLength = Guid.Empty.ToByteArray().Length;

		public override void WriteConstructorData(IDataWriter writer)
		{
			Guid[] guidArray = this.RealObject;
			byte[] data = new byte[guidArray.Length * GuidByteLength];
			for (int i = 0; i < guidArray.Length; i++)
			{
				Array.Copy(
					guidArray[i].ToByteArray(), 0, 
					data, i * GuidByteLength, GuidByteLength);
			}
			writer.WriteValue("data", data);
		}
		public override void WriteData(IDataWriter writer) {}
		public override object ConstructObject(IDataReader reader, TypeInfo objType)
		{
			byte[] data;
			reader.ReadValue("data", out data);
			Guid[] guidArray = new Guid[data.Length / GuidByteLength];
			byte[] guidData = new byte[GuidByteLength];
			for (int i = 0; i < guidArray.Length; i++)
			{
				Array.Copy(
					data, i * GuidByteLength,
					guidData, 0, GuidByteLength);
				guidArray[i] = new Guid(guidData);
			}
			return guidArray;
		}
		public override void ReadData(IDataReader reader) {}
	}
}
