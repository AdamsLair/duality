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

		public virtual object[] Data
		{
			get { return this.data; }
			set { this.data = value; }
		}

		public SerializableWrapper() : this(null) { }
		public SerializableWrapper(object[] data)
		{
			this.data = data;
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
				// Clone the object first to make sure it's isolated and doesn't 
				// drag a whole Scene (or so) into the serialization graph.
				object[] isolatedObjs = this.data.Select(obj => obj.DeepClone()).ToArray();

				// Now serialize the isolated object
				Serializer.WriteObject(isolatedObjs, stream, typeof(BinarySerializer));
				byte[] serializedData = stream.ToArray();
				info.AddValue("data", serializedData);
			}
		}
	}
}
