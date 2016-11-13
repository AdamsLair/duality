using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public struct PackageName
	{
		public static readonly PackageName None = new PackageName(null, null);

		public string	Id;
		public Version	Version;

		public PackageName VersionInvariant
		{
			get { return new PackageName(this.Id); }
		}

		public PackageName(string id)
		{
			this.Id = id;
			this.Version = null;
		}
		public PackageName(string id, Version version)
		{
			this.Id = id;
			this.Version = version;
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", this.Id, this.Version);
		}
	}
}
