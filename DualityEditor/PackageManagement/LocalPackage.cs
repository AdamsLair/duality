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
		private	string[]	files	= null;


		public string Id
		{
			get { return this.id; }
		}
		public Version Version
		{
			get { return this.version; }
			internal set { this.version = value; }
		}
		public IEnumerable<string> Files
		{
			get { return this.files ?? Enumerable.Empty<string>(); }
			internal set { this.files = (value != null ? value.ToArray() : null); }
		}
		public bool IsInstallationComplete
		{
			get { return this.files != null; }
		}


		internal LocalPackage(string id, Version version)
		{
			this.id = id;
			this.version = version;
		}
	}
}
