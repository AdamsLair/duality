using System;
using System.Diagnostics;
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
		private static readonly Dictionary<long, object> referenceMap = new Dictionary<long, object>();
		private static readonly long contextID = unchecked(DateTime.Now.Ticks * (long)Process.GetCurrentProcess().Id);

		private long id = -1;

		public sealed override object Data
		{
			get
			{
				return base.Data;
			}
			set
			{
				// Invalidate the ID in use. Could
				// occur if this SerializableReferenceWrapper
				// is being reused
				this.id = -1;
				base.Data = value;
			}
		}

		public SerializableReferenceWrapper(object data) : base(data) { }
		private SerializableReferenceWrapper(SerializationInfo info, StreamingContext context)
		{
			try
			{
				long referenceID = info.GetInt64("data");
				long referenceContext = info.GetInt64("context");

				// Retrieve reference, but safeguard against IDs from a different
				// application instance, or invalid / unavailable IDs.
				object reference;
				if (referenceContext != contextID || !referenceMap.TryGetValue(referenceID, out reference))
				{
					reference = null;
					referenceID = -1;
				}

				this.data = reference;
				this.id = referenceID;
			}
			catch (Exception)
			{
				this.data = null;
				this.id = -1;
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// First time this object is being serialized. Give it an ID in the reference map
			// so we can look it up when deserializing the object and get the same reference
			if (this.id < 0)
			{
				this.id = nextID++;
				referenceMap[this.id] = this.data;
			}

			info.AddValue("data", this.id);
			info.AddValue("context", contextID);
		}
	}
}
