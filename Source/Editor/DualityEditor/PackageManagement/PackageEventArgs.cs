using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public class PackageEventArgs : EventArgs
	{
		private	PackageName	packageName	= PackageName.None;
		
		public PackageName PackageName
		{
			get { return this.packageName; }
		}
		public string Id
		{
			get { return this.packageName.Id; }
		}
		public Version Version
		{
			get { return this.packageName.Version; }
		}

		public PackageEventArgs(PackageName package)
		{
			this.packageName = package;
		}
	}
}
