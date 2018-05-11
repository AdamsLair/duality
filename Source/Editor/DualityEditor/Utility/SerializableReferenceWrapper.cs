using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Duality.Serialization;

namespace Duality.Editor
{
	/// <summary>
	/// A wrapper object that stores a reference to a non-<see cref="SerializableAttribute"/> object.
	/// </summary>
	[Serializable]
	public class SerializableReferenceWrapper : SerializableWrapper
	{
		private static long nextID = 0;
		private static readonly Dictionary<long, object> referenceMap 
			= new Dictionary<long, object>();

		private long ID = -1;

		public sealed override object Data
		{
			get
			{
				return this.ID < 0 
					? null 
					: referenceMap[this.ID];
			}
			set
			{
				if (this.ID < 0)
					this.ID = nextID++;

				referenceMap[this.ID] = value;
			}
		}

		public SerializableReferenceWrapper(object data)
		{
			this.Data = data;
		}
		private SerializableReferenceWrapper(SerializationInfo info, StreamingContext context)
		{
			byte[] serializedData = info.GetValue("data", typeof(byte[])) as byte[];
			using (MemoryStream stream = new MemoryStream(serializedData ?? new byte[0]))
			{
				this.ID = Serializer.TryReadObject<long>(stream);
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				Serializer.WriteObject(this.ID, stream, typeof(BinarySerializer));
				byte[] serializedData = stream.ToArray();
				info.AddValue("data", serializedData);
			}
		}
	}
}
