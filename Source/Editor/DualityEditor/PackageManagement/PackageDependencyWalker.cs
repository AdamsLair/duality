using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using NuGet;

using Duality.IO;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor.PackageManagement
{
	/// <summary>
	/// Implements an algorithm to walk a package dependency graph and determine
	/// a packages transitive dependencies.
	/// </summary>
	public class PackageDependencyWalker
	{
		private Func<PackageName,PackageInfo>       packageResolver  = null;
		private HashSet<string>                     skipPackageIds   = new HashSet<string>();
		private Dictionary<PackageName,PackageInfo> resolveCache     = new Dictionary<PackageName,PackageInfo>();
		private HashSet<PackageName>                resolveCacheTemp = new HashSet<PackageName>();
		private Dictionary<PackageName,int>         dependencyCount  = new Dictionary<PackageName,int>();
		private HashSet<PackageInfo>                visitedPackages  = new HashSet<PackageInfo>();


		/// <summary>
		/// [GET] Enumerates all packages that were visited in all walk operations 
		/// since the last <see cref="Clear"/>.
		/// </summary>
		public IEnumerable<PackageInfo> VisitedPackages
		{
			get { return this.visitedPackages; }
		}


		public PackageDependencyWalker(Func<PackageName,PackageInfo> packageResolver)
		{
			this.packageResolver = packageResolver;
		}

		/// <summary>
		/// Resets the algorithm and clears all previous results, as well as the 
		/// internal <see cref="IgnorePackage"/> blacklist.
		/// </summary>
		public void Clear()
		{
			this.skipPackageIds.Clear();
			this.dependencyCount.Clear();
			this.visitedPackages.Clear();

			// Clear temporary items from the resolve cache
			foreach (PackageName name in this.resolveCacheTemp)
			{
				this.resolveCache.Remove(name);
			}
			this.resolveCacheTemp.Clear();
		}

		/// <summary>
		/// Configures the algorithm to not visit packages with the specified ID while
		/// exploring the graph.
		/// </summary>
		/// <param name="id"></param>
		public void IgnorePackage(string id)
		{
			if (this.skipPackageIds.Add(id))
			{
				this.dependencyCount.Clear();
				this.visitedPackages.Clear();
			}
		}

		/// <summary>
		/// Walks the specified packages dependency graph and adds 
		/// new findings to the result list.
		/// </summary>
		/// <param name="package"></param>
		public void WalkGraph(PackageInfo package)
		{
			this.WalkGraph(new [] { package });
		}
		/// <summary>
		/// Walks the specified packages dependency graph and adds 
		/// new findings to the result list.
		/// </summary>
		/// <param name="packageName"></param>
		public void WalkGraph(PackageName packageName)
		{
			this.WalkGraph(new [] { packageName });
		}
		/// <summary>
		/// Walks the specified packages dependency graphs and adds 
		/// new findings to the result list.
		/// </summary>
		/// <param name="packages"></param>
		public void WalkGraph(IEnumerable<PackageInfo> packages)
		{
			foreach (PackageInfo package in packages)
			{
				if (package == null) continue;
				this.resolveCache[package.Name] = package;
			}

			this.WalkGraph(packages.Select(p => p.Name));
		}
		/// <summary>
		/// Walks the specified packages dependency graphs and adds 
		/// new findings to the result list.
		/// </summary>
		/// <param name="packageNames"></param>
		public void WalkGraph(IEnumerable<PackageName> packageNames)
		{
			// If there are no packages in the arguments that we didn't already
			// walk before, do nothing. We already have the result.
			bool anyNewInput = false;
			foreach (PackageName packageName in packageNames)
			{
				if (this.dependencyCount.ContainsKey(packageName)) continue;
				anyNewInput = true;
				break;
			}
			if (!anyNewInput) return;

			// Determine the dependency graph of each package
			foreach (PackageName packageName in packageNames)
			{
				PackageInfo package = this.ResolvePackage(packageName);
				if (package == null) continue;

				VisitPackage(package);
			}
		}

		/// <summary>
		/// Retrieves the transitive dependency count of the specified package.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetDependencyCount(PackageName name)
		{
			int count;
			if (this.dependencyCount.TryGetValue(name, out count))
				return count;
			else
				return 0;
		}

		private PackageInfo ResolvePackage(PackageName name)
		{
			PackageInfo package;
			if (!this.resolveCache.TryGetValue(name, out package))
			{
				// Resolve the dependency using the exact name first, then 
				// the newest version if that fails.
				package = this.packageResolver(name);
				if (package == null)
					package = this.packageResolver(name.VersionInvariant);

				// Cache the results for later
				this.resolveCache[name] = package;

				// If we resolved to a different version, remove that item
				// from the cache on the next Clear to allow it to re-resolve
				// differently in the future
				if (package.Version != name.Version)
					this.resolveCacheTemp.Add(name);
			}
			return package;
		}
		private void VisitPackage(PackageInfo package)
		{
			if (!this.visitedPackages.Add(package)) return;

			// Improve cyclic dependency handling by counting direct dependencies early
			int count = package.Dependencies.Count;
			this.dependencyCount[package.Name] = count;

			// Iterate over dependencies and count theirs as well
			foreach (PackageName dependencyName in package.Dependencies)
			{
				// If this dependency is part of the set of skipped packages, skip it
				if (this.skipPackageIds.Contains(dependencyName.Id)) continue;

				// Resolve the dependency name to get a hold on the actual info
				PackageInfo dependency = this.ResolvePackage(dependencyName);
				if (dependency == null) continue;

				// Add secondary dependencies
				VisitPackage(dependency);
				count += this.dependencyCount[dependency.Name];
			}

			// Update the previous count value
			this.dependencyCount[package.Name] = count;
		}
	}
}
