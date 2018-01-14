using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Linq;

namespace Duality.Drawing
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class UnicodeRangeAttribute : Attribute
	{
		public ulong CharStart { get; private set; }
		public ulong CharEnd { get; private set; }

		public UnicodeRangeAttribute(ulong charStart, ulong charEnd)
		{
			this.CharStart = charStart;
			this.CharEnd = charEnd;
		}
	}
}