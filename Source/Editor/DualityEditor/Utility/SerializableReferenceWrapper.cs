using System;
using System.Collections.Generic;

namespace Duality.Editor.Utility
{
	[Serializable]
	public class SerializableReferenceWrapper : SerializableWrapper
	{
		private static long NextID = 0;
		private static readonly Dictionary<long, object> ReferenceMap 
			= new Dictionary<long, object>();

		private long ID = -1;

		public sealed override object Data
		{
			get
			{
				return this.ID < 0 
					? null 
					: ReferenceMap[this.ID];
			}
			set
			{
				if (this.ID < 0)
					this.ID = NextID++;

				ReferenceMap[this.ID] = value;
				base.Data = this.ID;
			}
		}

		public SerializableReferenceWrapper(object data)
		{
			this.Data = data;
		}
	}
}
