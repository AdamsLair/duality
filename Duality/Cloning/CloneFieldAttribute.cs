using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Cloning
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class CloneFieldAttribute : Attribute
	{
		private CloneFieldFlags	flags;
		public CloneFieldFlags Flags
		{
			get { return this.flags; }
		}
		public CloneFieldAttribute(CloneFieldFlags flags)
		{
			this.flags = flags;
		}
	}
}
