using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public sealed class LocalPackage
	{
		private	string		id		= null;
		private	Version		version	= null;
		private	PackageInfo	info	= null;


		public string Id
		{
			get { return this.id; }
		}
		public Version Version
		{
			get { return this.version; }
		}
		public PackageInfo Info
		{
			get { return this.info; }
			internal set { this.info = value; }
		}
		public bool IsInstallationComplete
		{
			get { return this.info != null; }
		}


		internal LocalPackage(PackageInfo info)
		{
			this.id = info.Id;
			this.version = info.Version;
			this.info = info;
		}
		internal LocalPackage(string id, Version version)
		{
			this.id = id;
			this.version = version;
			this.info = null;
		}

		public override string ToString()
		{
			return string.Format("Local Package '{0}' {1}", this.id, this.version);
		}
	}
}
