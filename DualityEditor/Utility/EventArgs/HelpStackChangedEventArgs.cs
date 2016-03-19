using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class HelpStackChangedEventArgs : EventArgs
	{
		private	HelpInfo	last;
		private	HelpInfo	current;

		public HelpInfo LastHelp
		{
			get { return this.last; }
		}
		public HelpInfo CurrentHelp
		{
			get { return this.current; }
		}

		public HelpStackChangedEventArgs(HelpInfo last, HelpInfo current)
		{
			this.last = last;
			this.current = current;
		}
	}
}
