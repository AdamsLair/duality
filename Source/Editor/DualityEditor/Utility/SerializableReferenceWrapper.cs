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

		private long[] ids = null;

		public sealed override object[] Data
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
				this.ids = null;
				base.Data = value;
			}
		}

		public SerializableReferenceWrapper(object[] data) : base(data) { }
		private SerializableReferenceWrapper(SerializationInfo info, StreamingContext context)
		{
			try
			{
				long[] referenceIDs = info.GetValue("data", typeof(long[])) as long[];
				long referenceContext = info.GetInt64("context");

				List<object> retrievedReferences = new List<object>();
				List<long> retrievedIDs = new List<long>();

				// Retrieve references, but safeguard against IDs from a different
				// application instance, or invalid / unavailable IDs.
				if (referenceContext == contextID)
				{
					foreach (long id in referenceIDs)
					{
						object reference;
						if (referenceMap.TryGetValue(id, out reference))
						{
							retrievedReferences.Add(reference);
							retrievedIDs.Add(id);
						}
					}
				}

				this.data = retrievedReferences.ToArray();
				this.ids = retrievedIDs.ToArray();
			}
			catch (Exception)
			{
				this.data = null;
				this.ids = null;
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// First time these objects are being serialized. Give them an ID in the reference map
			// so we can look it up when deserializing the objects and get the same reference
			if (this.ids == null)
			{
				this.ids = new long[this.data.Length];
				for (int i = 0; i < this.data.Length; i++)
				{
					this.ids[i] = nextID++;
					referenceMap[this.ids[i]] = this.data[i];
				}
			}

			info.AddValue("data", this.ids);
			info.AddValue("context", contextID);
		}
	}
}
