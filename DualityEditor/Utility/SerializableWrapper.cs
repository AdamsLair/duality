using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Duality.Serialization;

namespace Duality.Editor
{
	/// <summary>
	/// A wrapper object that stores a non-<see cref="SerializableAttribute"/> object in a serializable way.
	/// </summary>
	[Serializable]
	public class SerializableWrapper : ISerializable
	{
		private object data;

		public object Data
		{
			get { return this.data; }
			set { this.data = value; }
		}

		public SerializableWrapper() : this(null) { }
		public SerializableWrapper(object data)
		{
			this.data = data;
		}
		private SerializableWrapper(SerializationInfo info, StreamingContext context)
		{
			byte[] serializedData = info.GetValue("data", typeof(byte[])) as byte[];
			using (MemoryStream stream = new MemoryStream(serializedData ?? new byte[0]))
			{
				this.data = Serializer.TryReadObject<object>(stream);
			}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				Serializer.WriteObject(this.data, stream, typeof(BinarySerializer));
				byte[] serializedData = stream.ToArray();
				info.AddValue("data", serializedData);
			}
		}
	}
}
