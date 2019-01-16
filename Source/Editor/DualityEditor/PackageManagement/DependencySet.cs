using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;

namespace Duality.Editor.PackageManagement
{
	public class DependencySet
	{
		public static readonly FrameworkName Net452Target = new FrameworkName(".NETFramework", new Version(4, 5, 2));

		public readonly FrameworkName TargetFramework;
		public readonly List<PackageName> Dependencies;

		public DependencySet(FrameworkName targetFramework, IEnumerable<PackageName> dependencies = null)
		{
			this.TargetFramework = targetFramework;
			this.Dependencies = dependencies != null ? dependencies.ToList() : new List<PackageName>();
		}
	}
}