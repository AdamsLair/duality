using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Editor.PackageManagement
{
	public struct PackageName : IEquatable<PackageName>
	{
		public static readonly PackageName None = new PackageName(null, null);

		public string  Id;
		public Version Version;

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

		public bool Equals(PackageName other)
		{
			return
				this.Id == other.Id &&
				this.Version == other.Version;
		}
		public override bool Equals(object obj)
		{
			if (obj is PackageName)
				return this.Equals((PackageName)obj);
			else
				return false;
		}
		public override int GetHashCode()
		{
			unchecked
			{
				return 
					92357 * this.Id.GetHashCode() + 
					this.Version.GetHashCode();
			}
		}
		public override string ToString()
		{
			return string.Format("{0} {1}", this.Id, this.Version);
		}
		
		public static bool operator == (PackageName a, PackageName b)
		{
			return a.Equals(b);
		}
		public static bool operator != (PackageName a, PackageName b)
		{
			return !a.Equals(b);
		}
	}
}
