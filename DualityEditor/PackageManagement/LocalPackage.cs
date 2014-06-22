using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public sealed class LocalPackage
	{
		private	string			id			= null;
		private	Version			version		= null;


		public string Id
		{
			get { return this.id; }
		}
		public Version Version
		{
			get { return this.version; }
			internal set { this.version = value; }
		}


		internal LocalPackage(string id, Version version)
		{
			this.id = id;
			this.version = version;
		}
	}
}
