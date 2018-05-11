using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
					? base.Data 
					: referenceMap[this.ID];
			}
			set
			{
				// Invalidate the ID in use. Could
				// occur if this SerializableReferenceWrapper
				// is being reused
				this.ID = -1;
				base.Data = value;
			}
		}

		public SerializableReferenceWrapper(object data)
			: base(data)
		{
		}
		private SerializableReferenceWrapper(SerializationInfo info, StreamingContext context)
		{
			object serializedObject = info.GetValue("data", typeof(long));
			if (serializedObject is long)
				this.ID = (long) serializedObject;
			else
				this.ID = -1;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// First time this object is being serialized. Give it an ID in the reference map
			// so we can look it up when deserializing the object and get the same reference
			if (this.ID < 0)
			{
				this.ID = nextID++;
				referenceMap[this.ID] = base.Data;
			}
			info.AddValue("data", this.ID);
		}
	}
}
