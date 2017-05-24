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
	/// Caches information about local and remote package repositories.
	/// </summary>
	internal class PackageCache
	{
		private NuGet.PackageManager     manager      = null;
		private NuGet.IPackageRepository repository   = null;
		private bool                     hasLocalRepo = false;

		private object                                  cacheLock          = new object();
		private Dictionary<PackageName,PackageInfo>     nameToPackage      = new Dictionary<PackageName,PackageInfo>();
		private Dictionary<PackageName,NuGet.IPackage>  nameToNuGetPackage = new Dictionary<PackageName,NuGet.IPackage>();
		private Dictionary<PackageName,PackageInfo>     uniquePackageWrap  = new Dictionary<PackageName,PackageInfo>();
		private List<PackageInfo>                       latestDuality      = null;
		private Dictionary<string,List<NuGet.IPackage>> repositoryPackages = new Dictionary<string,List<NuGet.IPackage>>();


		public PackageCache(NuGet.PackageManager local, NuGet.IPackageRepository remote, bool remoteHasLocal)
		{
			this.manager = local;
			this.repository = remote;
			this.hasLocalRepo = remoteHasLocal;
		}

		/// <summary>
		/// Clears the cache to allow querying updated information from the repository.
		/// 
		/// Does not clear the internal mapping from fully qualified package names to
		/// actual packages, as the repository is still the same and NuGet packages 
		/// are read-only after publish.
		/// </summary>
		public void Clear()
		{
			lock (this.cacheLock)
			{
				this.latestDuality = null;
				this.nameToPackage.Clear();
				this.nameToNuGetPackage.Clear();
				this.repositoryPackages.Clear();

				// Don't clear the unique wrap cache, see XML docs comment about
				// read-only NuGet packages after publish.
			}
		}

		/// <summary>
		/// Enumerates the latest available Duality packages in the remote repository.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<PackageInfo> GetLatestDualityPackages()
		{
			lock (this.cacheLock)
			{
				if (this.latestDuality != null)
					return this.latestDuality;
			}

			return this.QueryLatestDualityPackages();
		}
		/// <summary>
		/// Retrieves information about the specified package.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public PackageInfo GetPackage(PackageName name)
		{
			PackageInfo result;
			lock (this.cacheLock)
			{
				if (this.nameToPackage.TryGetValue(name, out result))
					return result;

				NuGet.IPackage nugetPackage = this.GetNuGetPackage(name);
				if (nugetPackage == null)
					result = null;
				else
					result = this.WrapPackage(nugetPackage);
			
				this.nameToPackage[name] = result;
			}
			return result;
		}
		/// <summary>
		/// Retrieves a NuGet package based on the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public NuGet.IPackage GetNuGetPackage(PackageName name)
		{
			NuGet.IPackage result;
			lock (this.cacheLock)
			{
				if (this.nameToNuGetPackage.TryGetValue(name, out result))
					return result;

				result = this.FindPackage(name);

				this.nameToNuGetPackage[name] = result;
			}
			return result;
		}

		
		private IEnumerable<PackageInfo> QueryLatestDualityPackages()
		{
			// Query all NuGet packages
			IQueryable<NuGet.IPackage> query = this.repository.GetPackages();

			// Filter out old packages
			query = query.Where(p => p.IsLatestVersion);

			// Only look at NuGet packages tagged with "Duality" and "Plugin"
			query = query.Where(p => 
				p.Tags != null && 
				p.Tags.Contains(PackageManager.DualityTag));

			// If there is a local package repository, IsLatest isn't set. Need to emulate this
			if (this.hasLocalRepo)
			{
				List<IPackage> packages = query.ToList();
				for (int i = packages.Count - 1; i >= 0; i--)
				{
					IPackage current = packages[i];
					bool isLatest = !packages.Any(p => p.Id == current.Id && p.Version > current.Version);
					if (!isLatest)
					{
						packages.RemoveAt(i);
					}
				}
				query = packages.AsQueryable<IPackage>();
			}

			// Cache every result item as it arrives, validate the cache only
			// after the qurey has finished
			List<PackageInfo> results = new List<PackageInfo>();

			// Transform results into Duality package representation
			foreach (NuGet.IPackage package in query)
			{
				// Filter out all packages that shouldn't be presented to the user. This
				// includes pre-release and unlisted packages.
				if (!this.IsUserAvailable(package)) continue;

				// Create a Duality package representation for all query hits
				PackageInfo info = this.WrapPackage(package);
				results.Add(info);
				yield return info;
			}

			lock (this.cacheLock)
			{
				this.latestDuality = results;
			}
		}
		private NuGet.IPackage FindPackage(PackageName packageRef)
		{
			// Find a specific version. We're not looking for listed packages only.
			if (packageRef.Version != null)
			{
				// Query locally first, since we're looking for a specific version number anyway.
				foreach (NuGet.IPackage package in this.manager.LocalRepository.FindPackagesById(packageRef.Id))
				{
					if (package.Version.Version == packageRef.Version)
						return package;
				}

				// Nothing found? Query online then.
				try
				{
					// First try an indexed lookup. This may fail for freshly released packages.
					foreach (NuGet.IPackage package in this.GetRemotePackages(packageRef.Id))
					{
						if (!package.IsReleaseVersion()) continue;
						if (package.Version.Version != packageRef.Version) continue;
						return package;
					}

					// If that fails, enumerate all packages and select the one we need.
					//
					// Note: Make sure to include OrderByDescending. Without it, non-indexed
					// packages will not be returned from the query.
					IQueryable<NuGet.IPackage> query = 
						this.repository.GetPackages()
						.Where(p => p.Id == packageRef.Id)
						.OrderByDescending(p => p.Version);
					foreach (NuGet.IPackage package in query)
					{
						if (!package.IsReleaseVersion()) continue;
						if (package.Version.Version != packageRef.Version) continue;
						return package;
					}
				}
				catch (Exception e)
				{
					Log.Editor.WriteWarning("Error querying NuGet package repository: {0}", Log.Exception(e));
					return null;
				}

				return null;
			}
			// Find the newest available, listed version online.
			else
			{
				try
				{
					// Enumerate all package versions - do not rely on an indexed
					// lookup to get the latest, as the index might not be up-to-date.
					//
					// Note: Make sure to include OrderByDescending. Without it, non-indexed
					// packages will not be returned from the query.
					IQueryable<NuGet.IPackage> query = 
						this.repository.GetPackages()
						.Where(p => p.Id == packageRef.Id)
						.OrderByDescending(p => p.Version);

					// Note: IQueryable LINQ expressions will actually be transformed into
					// queries that are executed server-side. Unfortunately for us, the server
					// will order versions as if they were strings, meaning that 1.0.10 < 1.0.9.
					// To fix this, we'll have to iterate over them all and find the highest one
					// manually. We'll still include the OrderByDescending, so we avoid running
					// into a supposed caching mechanism that doesn't return non-indexed packages
					// and appears to be active when just filtering by ID.
					Version latestVersion = new Version(0, 0);
					NuGet.IPackage latestPackage = null;
					foreach (NuGet.IPackage package in query)
					{
						if (!this.IsUserAvailable(package)) continue;
						if (package.Version.Version > latestVersion)
						{
							latestVersion = package.Version.Version;
							latestPackage = package;
						}
					}
					return latestPackage;
				}
				catch (Exception e)
				{
					Log.Editor.WriteWarning("Error querying NuGet package repository: {0}", Log.Exception(e));
					return null;
				}
			}
		}
		private List<NuGet.IPackage> GetRemotePackages(string id)
		{
			// Quickly check if we have this result in our cache already
			List<NuGet.IPackage> result;
			lock (this.cacheLock)
			{
				if (this.repositoryPackages.TryGetValue(id, out result))
					return result;
			}
			
			// Otherwise, query the repository for all packages with this id
			result = this.repository
				.FindPackagesById(id)
				.ToList();

			// Update the cache with our new results
			lock (this.cacheLock)
			{
				this.repositoryPackages[id] = result;
			}
			return result;
		}
		
		private PackageInfo WrapPackage(NuGet.IPackage package)
		{
			PackageName name = new PackageName(package.Id, package.Version.Version);
			lock (this.cacheLock)
			{
				PackageInfo info;
				if (this.uniquePackageWrap.TryGetValue(name, out info))
					return info;

				info = new PackageInfo(package);

				this.uniquePackageWrap.Add(name, info);
				return info;
			}
		}
		private bool IsUserAvailable(NuGet.IPackage package)
		{
			// Filter unlisted, non-release and special versions
			if (!package.IsReleaseVersion()) return false;
			if (!package.IsListed()) return false;
			if (package.Version != new SemanticVersion(package.Version.Version)) return false;
			return true;
		}
	}
}
