using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Duality.Serialization;
using Duality.Cloning;

namespace Duality.Editor
{
	/// <summary>
	/// A wrapper object that stores a non-<see cref="SerializableAttribute"/> object in a serializable way.
	/// </summary>
	[Serializable]
	public class SerializableWrapper : ISerializable
	{
		protected object[] data;

		public virtual IReadOnlyList<object> Data
		{
			get { return this.data; }
			set
			{
				// Clone the objects first to make sure they are isolated and don't
				// drag a whole Scene (or so) into the serialization graph.
				this.data = value.Select(o => o.DeepClone()).ToArray();
			}
		}

		public SerializableWrapper() : this(null) { }
		public SerializableWrapper(IEnumerable<object> data)
		{
			// Clone the objects first to make sure they are isolated and don't
			// drag a whole Scene (or so) into the serialization graph.
			this.data = data == null ? null : data.Select(o => o.DeepClone()).ToArray();
		}
		private SerializableWrapper(SerializationInfo info, StreamingContext context)
		{
			byte[] serializedData;
			try
			{
				serializedData = info.GetValue("data", typeof(byte[])) as byte[];
			}
			catch (Exception)
			{
				serializedData = null;
			}

			if (serializedData == null)
			{
				this.data = null;
				return;
			}

			using (MemoryStream stream = new MemoryStream(serializedData))
			{
				this.data = Serializer.TryReadObject<object[]>(stream);
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				// Now serialize the isolated object
				Serializer.WriteObject(this.data, stream, typeof(BinarySerializer));
				byte[] serializedData = stream.ToArray();
				info.AddValue("data", serializedData);
			}
		}
	}
}
