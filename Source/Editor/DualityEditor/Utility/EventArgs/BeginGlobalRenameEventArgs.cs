using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class BeginGlobalRenameEventArgs : ResourceRenamedEventArgs
	{
		private	bool	cancel	= false;
		public bool Cancel
		{
			get { return this.cancel; }
			set { this.cancel = value; }
		}
		public BeginGlobalRenameEventArgs(string path, string oldPath, bool isDirectory) : base(path, oldPath, isDirectory) {}
	}
}
