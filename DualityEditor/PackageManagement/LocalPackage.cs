using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public sealed class LocalPackage
	{
		private	PackageName	packageName	= PackageName.None;
		private	PackageInfo	info		= null;

		
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
			this.packageName = info.PackageName;
			this.info = info;
		}
		internal LocalPackage(PackageName package)
		{
			this.packageName = package;
			this.info = null;
		}

		public override string ToString()
		{
			return string.Format("Local Package '{0}'", this.packageName);
		}
	}
}
