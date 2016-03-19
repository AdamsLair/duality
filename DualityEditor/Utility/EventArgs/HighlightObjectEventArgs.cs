using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class HighlightObjectEventArgs : EventArgs
	{
		private	ObjectSelection	target;
		private	HighlightMode	mode;

		public ObjectSelection Target
		{
			get { return this.target; }
		}
		public HighlightMode Mode
		{
			get { return this.mode; }
		}

		public HighlightObjectEventArgs(ObjectSelection target, HighlightMode mode)
		{
			this.target = target;
			this.mode = mode;
		}
	}
}
