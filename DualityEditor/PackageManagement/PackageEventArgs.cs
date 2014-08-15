using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public class PackageEventArgs : EventArgs
	{
		private	string		id		= null;
		private	Version		version	= null;

		public string Id
		{
			get { return this.id; }
		}
		public Version Version
		{
			get { return this.version; }
		}

		public PackageEventArgs(string id, Version version)
		{
			this.id = id;
			this.version = version;
		}
	}
}
