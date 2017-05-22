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
	public sealed class PackageManager
	{
		public const string DualityTag  = "Duality";
		public const string PluginTag   = "Plugin";
		public const string SampleTag   = "Sample";
		public const string CoreTag     = "Core";
		public const string EditorTag   = "Editor";
		public const string LauncherTag = "Launcher";

		/// <summary>
		/// A list of package names that are considered "core" Duality packages.
		/// If none of these show up anywhere in the deep dependency graph of a Duality package,
		/// it will be assumed that dependencies are simply not specified properly.
		/// </summary>
		private readonly string[] DualityPackageNames = new string[] 
		{
			"AdamsLair.Duality",
			"AdamsLair.Duality.Editor",
			"AdamsLair.Duality.Launcher"
		};


		private PackageSetup              setup          = new PackageSetup();
		private PackageManagerEnvironment env            = null;
		private bool                      hasLocalRepo   = false;
		private List<PackageName>         uninstallQueue = null;

		private object                              cacheLock              = new object();
		private Dictionary<string,NuGet.IPackage[]> repositoryPackageCache = new Dictionary<string,NuGet.IPackage[]>();
		private Dictionary<NuGet.IPackage,bool>     licenseAcceptedCache   = new Dictionary<NuGet.IPackage,bool>();

		private NuGet.PackageManager     manager    = null;
		private NuGet.IPackageRepository repository = null;
		private PackageManagerLogger     logger     = null;

		public event EventHandler<PackageLicenseAgreementEventArgs> PackageLicenseAcceptRequired = null;
		public event EventHandler<PackageEventArgs>                 PackageInstalled             = null;
		public event EventHandler<PackageEventArgs>                 PackageUninstalled           = null;


		public bool IsPackageSyncRequired
		{
			get
			{
				IPackage[] allInstalledPackages = this.manager
					.LocalRepository
					.GetPackages()
					.ToArray();

				// Do we have installed packages that are not registered, i.e. need to be uninstalled?
				foreach (IPackage package in allInstalledPackages)
				{
					if (!IsDualityPackage(package)) continue;
					if (this.setup.GetPackage(package.Id, package.Version.Version) == null)
						return true;
				}

				// Do we have registered packages that are not installed or don't have a specific version?
				// Also, are any of them not actually installed yet?
				foreach (LocalPackage package in this.setup.Packages)
				{
					if (package.Id == null) return true;
					if (package.Version == null) return true;
					if (!allInstalledPackages.Any(n => 
						n.Id == package.Id && 
						n.Version == new SemanticVersion(package.Version)))
						return true;
				}

				return false;
			}
		}
		public PackageManagerEnvironment LocalEnvironment
		{
			get { return this.env; }
		}
		/// <summary>
		/// [GET / SET] A data class representing the local package setup. Do not change these 
		/// values manually, but use <see cref="PackageManager"/> API instead.
		/// </summary>
		public PackageSetup LocalSetup
		{
			get { return this.setup; }
		}

		
		public PackageManager() : this(new PackageManagerEnvironment(null)) { }
		public PackageManager(string rootPath) : this(new PackageManagerEnvironment(rootPath)) { }
		public PackageManager(PackageManagerEnvironment workEnvironment) : this(workEnvironment, null) { }
		public PackageManager(PackageManagerEnvironment workEnvironment, PackageSetup packageSetup)
		{
			// If no external package setup is provided, load it from the config file
			if (packageSetup == null)
			{
				string configFilePath = workEnvironment.ConfigFilePath;
				if (!File.Exists(configFilePath))
				{
					packageSetup = new PackageSetup();
					packageSetup.Save(configFilePath);
				}
				else
				{
					try
					{
						packageSetup = PackageSetup.Load(configFilePath);
					}
					catch (Exception e)
					{
						Log.Editor.WriteError(
							"Failed to load PackageManager config file '{0}': {1}",
							configFilePath,
							Log.Exception(e));
					}
				}
			}

			// Assign work environment and package setup to work with
			this.env = workEnvironment;
			this.setup = packageSetup;

			// Create internal package management objects
			IPackageRepository[] repositories = this.setup.RepositoryUrls
				.Select(x => this.CreateRepository(x))
				.Where(x => x != null)
				.ToArray();
			this.hasLocalRepo = repositories.OfType<LocalPackageRepository>().Any();
			this.repository = new AggregateRepository(repositories);
			this.manager = new NuGet.PackageManager(this.repository, this.env.RepositoryPath);
			this.manager.PackageInstalled += this.manager_PackageInstalled;
			this.manager.PackageUninstalled += this.manager_PackageUninstalled;
			this.manager.PackageUninstalling += this.manager_PackageUninstalling;
			this.logger = new PackageManagerLogger();
			this.manager.Logger = this.logger;

			// Retrieve information about local packages
			this.RetrieveLocalPackageInfo();
		}

		public void InstallPackage(PackageInfo package)
		{
			this.InstallPackage(package, false);
		}
		private void InstallPackage(PackageInfo package, bool skipLicense)
		{
			NuGet.IPackage newPackage = this.FindPackageInfo(package.PackageName);

			// Check license terms
			if (!skipLicense && !this.CheckDeepLicenseAgreements(newPackage))
			{
				return;
			}

			Log.Editor.Write("Installing package '{0}'...", package.PackageName);
			Log.Editor.PushIndent();

			// Request NuGet to install the package
			this.manager.InstallPackage(newPackage, false, false);

			Log.Editor.PopIndent();
		}

		/// <summary>
		/// Installs the specified package if it wasn't installed yet and synchronizes
		/// the registered Duality package version number with the one that is present
		/// in the local cache.
		/// </summary>
		/// <param name="package"></param>
		public void VerifyPackage(LocalPackage package)
		{
			Log.Editor.Write("Verifying package '{0}'...", package.PackageName);
			Log.Editor.PushIndent();

			Version oldPackageVersion = package.Version;

			// Determine the exact version that will be downloaded
			PackageInfo packageInfo = this.QueryPackageInfo(package.PackageName);
			if (packageInfo == null)
			{
				throw new Exception(string.Format(
					"Can't resolve version of package '{0}'. There seems to be no compatible version available.",
					package.Id));
			}

			// Prepare a listener to determine whether we actually installed something
			EventHandler<PackageOperationEventArgs> installListener = null;
			bool packageInstalled = false;
			installListener = delegate(object sender, PackageOperationEventArgs args)
			{
				if (args.Package.Id == package.Id)
				{
					packageInstalled = true;
				}
			};

			// Install the package. Won't do anything if the package is already installed.
			this.manager.PackageInstalled += installListener;
			this.InstallPackage(packageInfo, true);
			this.manager.PackageInstalled -= installListener;

			// If we didn't install anything, that package was already present in the local cache, but not in the PackageConfig file
			if (!packageInstalled && oldPackageVersion == null)
			{
				// Add the explicit version to the PackageConfig file
				this.setup.Packages.RemoveAll(p => p.Id == packageInfo.Id);
				this.setup.Packages.Add(new LocalPackage(packageInfo));
			}

			// In case we've just retrieved an explicit version for the first time, save the config file.
			if (oldPackageVersion == null)
			{
				this.setup.Save(this.env.ConfigFilePath);
			}

			Log.Editor.PopIndent();
		}
		/// <summary>
		/// Uninstalls all Duality packages that are installed, but not registered. This
		/// usually happens when a user manually edits the package config file and either
		/// removes package entries explicitly, or changes their version numbers.
		/// 
		/// Due to the nature of this operation, it is possible that package dependencies
		/// are removed even though they might still be required by some installed packages.
		/// To make sure this state doesn't persist, it is recommended to perform a package
		/// verification step afterwards, which will re-install dependencies that might have
		/// been accidentally removed.
		/// </summary>
		public void UninstallNonRegisteredPackages()
		{
			IPackage[] allInstalledPackages = this.manager
				.LocalRepository
				.GetPackages()
				.ToArray();
			LocalPackage[] registeredPackages = this.setup.Packages
				.ToArray();

			foreach (IPackage package in allInstalledPackages)
			{
				// Skip non-Duality packages. They're not registered anyway and not expected to.
				if (!IsDualityPackage(package)) continue;

				// Skip packages that are registered in the exact same version
				bool isRegistered = registeredPackages.Any(p => 
					p.Id == package.Id && 
					p.Version == package.Version.Version);
				if (isRegistered) continue;

				// Uninstall all others with a force-uninstall. If we happen to remove
				// a package that is actually still needed, an additional package verification
				// step afterwards will fix this.
				this.UninstallPackage(package.Id, package.Version.Version, true);
			}
		}

		public void UninstallPackage(PackageInfo package)
		{
			this.UninstallPackage(package.Id, package.Version, false);
		}
		public void UninstallPackage(LocalPackage package)
		{
			this.UninstallPackage(package.Id, package.Version, false);
		}
		private void UninstallPackage(string id, Version version, bool force)
		{
			Log.Editor.Write("Uninstalling package '{0} {1}'...", id, version);
			Log.Editor.PushIndent();

			// NuGet dependency removal will remove all dependencies that are
			// not otherwise used. However, this will affect Duality packages
			// as well, so we might accidentally uninstall some of them, because
			// no package really depends on them.
			// 
			// To avoid this, we'll have an uninstall queue and cancel all Duality
			// package uninstalls that aren't registered there.
			this.uninstallQueue = new List<PackageName>();
			this.uninstallQueue.Add(new PackageName(id, version));

			this.manager.UninstallPackage(id, new SemanticVersion(version), force, true);

			this.uninstallQueue = null;

			Log.Editor.PopIndent();
		}
		public bool CanUninstallPackage(PackageInfo package)
		{
			return this.CanUninstallPackage(this.setup.GetPackage(package.Id));
		}
		[DebuggerNonUserCode]
		public bool CanUninstallPackage(LocalPackage package)
		{
			bool allowed = true;
			this.manager.WhatIf = true;
			this.manager.Logger = null;
			try
			{
				this.manager.UninstallPackage(package.Id, new SemanticVersion(package.Version), false, true);
			}
			catch (Exception)
			{
				allowed = false;
			}
			this.manager.Logger = this.logger;
			this.manager.WhatIf = false;
			return allowed;
		}

		public void UpdatePackage(PackageInfo package)
		{
			this.UpdatePackage(this.setup.GetPackage(package.Id));
		}
		public void UpdatePackage(LocalPackage package)
		{
			NuGet.IPackage newPackage = this.FindPackageInfo(new PackageName(package.Id));
			
			// Check license terms
			if (!this.CheckDeepLicenseAgreements(newPackage))
			{
				return;
			}
			
			Log.Editor.Write("Updating package '{0}'...", package.PackageName);
			Log.Editor.PushIndent();

			this.manager.UpdatePackage(newPackage, true, false);

			Log.Editor.PopIndent();
		}
		public bool CanUpdatePackage(PackageInfo package)
		{
			return this.CanUpdatePackage(this.setup.GetPackage(package.Id));
		}
		[DebuggerNonUserCode]
		public bool CanUpdatePackage(LocalPackage package)
		{
			bool allowed = true;
			this.manager.WhatIf = true;
			this.manager.Logger = null;
			try
			{
				Version version = this.QueryPackageInfo(package.PackageName.VersionInvariant).Version;
				this.manager.UpdatePackage(package.Id, new SemanticVersion(version), true, false);
			}
			catch (Exception)
			{
				allowed = false;
			}
			this.manager.Logger = this.logger;
			this.manager.WhatIf = false;
			return allowed;
		}

		/// <summary>
		/// Enumerates all target packages for local installs that could be updated.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<PackageInfo> GetUpdatablePackages()
		{
			List<PackageInfo> updatePackages = new List<PackageInfo>();
			LocalPackage[] targetPackages = this.setup.Packages.ToArray();
			for (int i = 0; i < targetPackages.Length; i++)
			{
				PackageInfo update = this.QueryPackageInfo(targetPackages[i].PackageName.VersionInvariant);
				if (update.Version <= targetPackages[i].Version) continue;
				updatePackages.Add(update);
			}
			return updatePackages;
		}

		/// <summary>
		/// Determines compatibility between the current package installs and the specified target package.
		/// Works for both updates and new installs.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public PackageCompatibility GetCompatibilityLevel(PackageInfo target)
		{
			// If the target package is already installed in the matching version, assume compatibility
			if (this.setup.GetPackage(target.Id, target.Version) != null)
				return PackageCompatibility.Definite;

			// Determine all packages that might be updated or installed
			PackageInfo[] touchedPackages = this.GetDeepDependencies(new[] { target }).ToArray();

			// Verify properly specified dependencies for Duality packages
			if (target.IsDualityPackage)
			{
				// If none of the targets deep dependencies is anyhow related to Duality, assume they're incomplete and potentially incompatible
				bool anyDualityDependency = false;
				foreach (PackageInfo package in touchedPackages)
				{
					if (DualityPackageNames.Any(name => string.Equals(name, package.Id)))
					{
						anyDualityDependency = true;
						break;
					}
				}
				if (!anyDualityDependency)
					return PackageCompatibility.None;
			}

			// Generate a mapping to already installed packages
			Dictionary<PackageInfo,LocalPackage> localMap = new Dictionary<PackageInfo,LocalPackage>();
			foreach (PackageInfo package in touchedPackages)
			{
				LocalPackage local = this.setup.GetPackage(package.Id);
				if (local == null) continue;

				localMap.Add(package, local);
			}

			// Determine the maximum version difference between target and installed
			PackageCompatibility compatibility = PackageCompatibility.Definite;
			foreach (var pair in localMap)
			{
				Version targetVersion = pair.Key.Version;
				Version localVersion = pair.Value.Version;
				
				if (localVersion.Major != targetVersion.Major)
					compatibility = compatibility.Combine(PackageCompatibility.Unlikely);
				else if (localVersion.Minor != targetVersion.Minor)
					compatibility = compatibility.Combine(PackageCompatibility.Likely);
			}

			return compatibility;
		}

		/// <summary>
		/// Sorts the specified list of packages according to their dependencies, guaranteeing that no package
		/// is listed before its dependencies. Use this to determine the order of batch updates and installs
		/// to prevent conflicts from having different versions of the same packages.
		/// </summary>
		public void OrderByDependencies(IList<PackageInfo> packages)
		{
			if (packages.Count < 2) return;

			// Determine the number of deep dependencies for each package
			Dictionary<PackageInfo,int> deepDependencyCount = this.GetDeepDependencyCount(packages);

			// Sort packages according to their deep dependency counts
			packages.StableSort((a, b) =>
			{
				int countA;
				int countB;
				if (a == null || !deepDependencyCount.TryGetValue(a, out countA))
					countA = 0;
				if (b == null || !deepDependencyCount.TryGetValue(b, out countB))
					countB = 0;
				return countA - countB;
			});
		}
		public void OrderByDependencies(IList<LocalPackage> packages)
		{
			// Map each list entry to its PackageInfo
			PackageInfo[] localInfo = packages
				.Select(p => p.Info ?? this.QueryPackageInfo(p.PackageName))
				.NotNull()
				.ToArray();

			// Sort the mapped list
			this.OrderByDependencies(localInfo);

			// Now sort the original list to match the sorted mapped list
			LocalPackage[] originalPackages = packages.ToArray();
			int nonSortedCount = 0;
			for (int i = 0; i < originalPackages.Length; i++)
			{
				LocalPackage localPackage = originalPackages[i];

				int newIndex = localInfo.IndexOfFirst(p => p.Id == localPackage.Id && p.Version == localPackage.Version);
				if (newIndex == -1)
					newIndex = localInfo.IndexOfFirst(p => p.Id == localPackage.Id);

				// Unresolved packages are always last in the sorted list
				if (newIndex == -1)
				{
					newIndex = packages.Count - nonSortedCount - 1;
					nonSortedCount++;
				}

				packages[newIndex] = localPackage;
			}
		}

		public bool ApplyUpdate(bool restartEditor = true)
		{
			if (!File.Exists(this.env.UpdateFilePath)) return false;

			Log.Editor.Write("Applying package update...");
			Log.Editor.PushIndent();
			
			// Manually perform update operations on the updater itself
			try
			{
				Log.Editor.Write("Preparing updater...");
				PackageUpdateSchedule schedule = this.PrepareUpdateSchedule();
				schedule.ApplyUpdaterChanges(this.env.UpdaterExecFilePath);
				this.SaveUpdateSchedule(schedule);
			}
			catch (Exception e)
			{
				Log.Editor.WriteError(
					"Can't update '{0}', because an error occurred: {1}", 
					this.env.UpdaterExecFilePath, 
					Log.Exception(e));
				return false;
			}

			// Run the updater application
			Log.Editor.Write("Running updater...");
			Process.Start(this.env.UpdaterExecFilePath, string.Format("\"{0}\" \"{1}\" \"{2}\"",
				this.env.UpdateFilePath,
				restartEditor ? typeof(DualityEditorApp).Assembly.Location : "",
				restartEditor ? Environment.CurrentDirectory : ""));

			Log.Editor.PopIndent();

			return true;
		}
		
		/// <summary>
		/// Enumerates the complete dependency tree of the specified packages.
		/// </summary>
		/// <param name="package"></param>
		/// <returns></returns>
		private IEnumerable<PackageInfo> GetDeepDependencies(IEnumerable<PackageInfo> packages)
		{
			Dictionary<PackageInfo,int> deepDependencyCount = this.GetDeepDependencyCount(packages);
			return deepDependencyCount.Keys;
		}
		/// <summary>
		/// Determines the number of deep dependencies for each package in the specified collection.
		/// </summary>
		/// <param name="packages"></param>
		/// <returns></returns>
		private Dictionary<PackageInfo,int> GetDeepDependencyCount(IEnumerable<PackageInfo> packages)
		{
			// Build a lookup for the packages we already know
			Dictionary<PackageName,PackageInfo> resolveCache = new Dictionary<PackageName,PackageInfo>();
			foreach (PackageInfo package in packages)
			{
				if (package == null) continue;
				resolveCache[package.PackageName] = package;
			}

			// Determine the dependency count of each package
			Dictionary<PackageInfo,int> result = new Dictionary<PackageInfo,int>();
			foreach (PackageInfo package in packages)
			{
				if (package == null) continue;
				GetDeepDependencyCount(package, result, resolveCache);
			}

			return result;
		}
		/// <summary>
		/// Determines the number of deep dependencies for the specified package.
		/// </summary>
		/// <param name="package"></param>
		/// <param name="deepCount"></param>
		/// <param name="resolver"></param>
		/// <returns></returns>
		private int GetDeepDependencyCount(PackageInfo package, Dictionary<PackageInfo,int> deepCount, Dictionary<PackageName,PackageInfo> resolveCache)
		{
			int count;
			if (!deepCount.TryGetValue(package, out count))
			{
				// Use the count of direct dependencies as starting value
				count = package.Dependencies.Count();

				// Prevent endless recursion on cyclic dependency graphs by registering early
				deepCount[package] = count;

				// Iterate over dependencies and count theirs as well
				foreach (PackageName dependencyName in package.Dependencies)
				{
					// Try to resolve the dependency name to get a hold on the actual info
					PackageInfo dependency;
					if (!resolveCache.TryGetValue(dependencyName, out dependency))
					{
						// Try the exact name locally and online
						dependency = this.QueryPackageInfo(dependencyName);

						// If nothing turns up, see if we have a similar package locally and try that as well
						if (dependency == null)
						{
							LocalPackage localDependency = this.setup.GetPackage(dependencyName.VersionInvariant);
							if (localDependency != null)
							{
								if (localDependency.Info != null)
									dependency = localDependency.Info;
								else
									dependency = this.QueryPackageInfo(localDependency.PackageName);
							}
						}

						// If we still have nothing, try a general search for the newest package with that Id
						if (dependency == null)
						{
							dependency = this.QueryPackageInfo(dependencyName.VersionInvariant);
						}

						// Cache the results for later
						resolveCache[dependencyName] = dependency;
					}
					if (dependency == null) continue;

					// Add secondary dependencies
					count += GetDeepDependencyCount(dependency, deepCount, resolveCache);
				}

				// Update the registered value
				deepCount[package] = count;
			}
			return count;
		}

		public IEnumerable<PackageInfo> QueryAvailablePackages()
		{
			// Query all NuGet packages
			IQueryable<NuGet.IPackage> query = this.repository.GetPackages();

			// Filter out old packages
			query = query.Where(p => p.IsLatestVersion);

			// Only look at NuGet packages tagged with "Duality" and "Plugin"
			query = query.Where(p => 
				p.Tags != null && 
				p.Tags.Contains(DualityTag));

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

			// Transform results into Duality package representation
			foreach (NuGet.IPackage package in query)
			{
				// Filter out all packages that shouldn't be presented to the user. This
				// includes pre-release and unlisted packages.
				if (!this.IsUserAvailable(package)) continue;

				// Create a Duality package representation for all query hits
				PackageInfo info = this.CreatePackageInfo(package);
				yield return info;
			}
		}
		public PackageInfo QueryPackageInfo(PackageName packageRef)
		{
			NuGet.IPackage package = this.FindPackageInfo(packageRef);
			return package != null ? this.CreatePackageInfo(package) : null;
		}
		
		private NuGet.IPackage FindPackageInfo(PackageName packageRef)
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
					foreach (NuGet.IPackage package in this.GetRepositoryPackages(packageRef.Id))
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
		private IEnumerable<NuGet.IPackage> GetRepositoryPackages(string id)
		{
			// Quickly check if we have this result in our cache already
			NuGet.IPackage[] result;
			lock (this.cacheLock)
			{
				if (this.repositoryPackageCache.TryGetValue(id, out result))
					return result;
			}
			
			// Otherwise, query the repository for all packages with this id
			result = this.repository
				.FindPackagesById(id)
				.ToArray();

			// Update the cache with our new results
			lock (this.cacheLock)
			{
				this.repositoryPackageCache[id] = result;
			}
			return result;
		}
		private bool IsUserAvailable(NuGet.IPackage package)
		{
			// Filter unlisted, non-release and special versions
			if (!package.IsReleaseVersion()) return false;
			if (!package.IsListed()) return false;
			if (package.Version != new SemanticVersion(package.Version.Version)) return false;
			return true;
		}

		private void RetrieveLocalPackageInfo()
		{
			foreach (LocalPackage localPackage in this.setup.Packages)
			{
				if (localPackage.Version != null && !string.IsNullOrEmpty(localPackage.Id))
				{
					foreach (NuGet.IPackage repoPackage in this.manager.LocalRepository.FindPackagesById(localPackage.Id))
					{
						if (repoPackage.Version.Version == localPackage.Version)
						{
							localPackage.Info = this.CreatePackageInfo(repoPackage);
							break;
						}
					}
				}
			}
		}
		private IPackageRepository CreateRepository(string repositoryUrl)
		{
			if (string.IsNullOrWhiteSpace(repositoryUrl))
				return null;

			try
			{
				string schemeName = null;
				if (repositoryUrl.Contains(Uri.SchemeDelimiter))
				{
					schemeName = repositoryUrl.Split(new[] { Uri.SchemeDelimiter }, StringSplitOptions.RemoveEmptyEntries)[0];
				}

				if (schemeName != null && Uri.CheckSchemeName(schemeName))
					repositoryUrl = new Uri(repositoryUrl).AbsoluteUri;
				else
					repositoryUrl = new Uri("file:///" + Path.GetFullPath(Path.Combine(this.env.RootPath, repositoryUrl))).AbsolutePath;
			}
			catch (UriFormatException)
			{
				Log.Editor.WriteError("NuGet repository URI '{0}' has an incorrect format and will be skipped.", repositoryUrl);
				return null;
			}

			return PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);
		}

		private bool CheckDeepLicenseAgreements(NuGet.IPackage package)
		{
			var deepDependencyCountDict = this.GetDeepDependencyCount(new[] { this.CreatePackageInfo(package) });
			NuGet.IPackage[] deepDependencies = deepDependencyCountDict.Keys
				.Select(i => this.FindPackageInfo(i.PackageName))
				.NotNull()
				.ToArray();
			NuGet.IPackage[] installedPackages = this.manager.LocalRepository.GetPackages().ToArray();

			foreach (NuGet.IPackage p in deepDependencies)
			{
				// Skip the ones that are already installed
				if (installedPackages.Any(l => l.Id == p.Id && l.Version >= p.Version))
					continue;

				if (!this.CheckLicenseAgreement(p))
					return false;
			}

			return true;
		}
		private bool CheckLicenseAgreement(NuGet.IPackage package)
		{
			if (package.RequireLicenseAcceptance)
			{
				// On the very first install, do not display additional license agreement dialogs
				// because all the packages being installed are the default ones.
				if (this.setup.IsFirstInstall)
					return true;

				bool agreed;
				if (!this.licenseAcceptedCache.TryGetValue(package, out agreed) || !agreed)
				{
					PackageLicenseAgreementEventArgs args = new PackageLicenseAgreementEventArgs(
						new PackageName(package.Id, package.Version.Version),
						package.LicenseUrl,
						package.RequireLicenseAcceptance);

					if (this.PackageLicenseAcceptRequired != null)
						this.PackageLicenseAcceptRequired(this, args);
					else
						DisplayDefaultLicenseAcceptDialog(args);

					agreed = args.IsLicenseAccepted;
					this.licenseAcceptedCache[package] = agreed;
				}

				if (!agreed)
				{ 
					return false;
				}
			}

			return true;
		}

		private PackageUpdateSchedule PrepareUpdateSchedule()
		{
			// Load existing update schedule in order to extend it
			string updateFilePath = this.env.UpdateFilePath;
			PackageUpdateSchedule updateSchedule = null;
			if (File.Exists(updateFilePath))
			{
				try
				{
					updateSchedule = PackageUpdateSchedule.Load(updateFilePath);
				}
				catch (Exception exception)
				{
					updateSchedule = null;
					Log.Editor.WriteError("Error parsing existing package update schedule '{0}': {1}", 
						Path.GetFileName(updateFilePath), 
						Log.Exception(exception));
				}
			}

			// If none existed yet, create a fresh update schedule
			if (updateSchedule == null)
				updateSchedule = new PackageUpdateSchedule();

			return updateSchedule;
		}
		private void SaveUpdateSchedule(PackageUpdateSchedule schedule)
		{
			string updateFilePath = this.env.UpdateFilePath;
			schedule.Save(updateFilePath);
		}

		private Dictionary<string,string> CreateFileMapping(NuGet.IPackage package)
		{
			Dictionary<string,string> fileMapping = new Dictionary<string,string>();

			bool isDualityPackage = IsDualityPackage(package);
			bool isPluginPackage = isDualityPackage && package.Tags.Contains(PluginTag);

			string folderFriendlyPackageName = package.Id;
			string binaryBaseDir = this.env.TargetPluginPath;
			string contentBaseDir = this.env.TargetDataPath;
			string sourceBaseDir = Path.Combine(this.env.TargetSourcePath, folderFriendlyPackageName);
			if (!isPluginPackage || !isDualityPackage)
			{
				binaryBaseDir = this.env.RootPath;
				contentBaseDir = this.env.RootPath;
			}

			IPackageFile[] packageFiles;
			try
			{
				packageFiles = package.GetFiles()
					.Where(f => f.TargetFramework == null || f.TargetFramework.Version < Environment.Version)
					.OrderByDescending(f => f.TargetFramework == null ? new Version() : f.TargetFramework.Version)
					.OrderByDescending(f => f.TargetFramework == null)
					.ToArray();
			}
			catch (DirectoryNotFoundException)
			{
				// We'll run into this exception when uninstalling a package without any files.
				return fileMapping;
			}

			foreach (IPackageFile f in packageFiles)
			{
				// Determine where the file needs to go
				string targetPath = f.EffectivePath;
				string baseDir = f.Path;
				while (baseDir.Contains(Path.DirectorySeparatorChar) || baseDir.Contains(Path.AltDirectorySeparatorChar))
				{
					baseDir = Path.GetDirectoryName(baseDir);
				}
				if (string.Equals(baseDir, "lib", StringComparison.InvariantCultureIgnoreCase))
					targetPath = Path.Combine(binaryBaseDir, targetPath);
				else if (string.Equals(baseDir, "content", StringComparison.InvariantCultureIgnoreCase))
					targetPath = Path.Combine(contentBaseDir, targetPath);
				else if (string.Equals(baseDir, "source", StringComparison.InvariantCultureIgnoreCase))
				{
					if (targetPath.StartsWith("source") && targetPath.Length > "source".Length)
						targetPath = targetPath.Remove(0, "source".Length + 1);
					targetPath = Path.Combine(sourceBaseDir, targetPath);
				}
				else
					continue;

				// Add a file mapping entry linking target path to package path
				if (fileMapping.ContainsKey(targetPath)) continue;
				fileMapping[targetPath] = f.Path;
			}

			return fileMapping;
		}
		private PackageInfo CreatePackageInfo(NuGet.IPackage package)
		{
			PackageInfo info = new PackageInfo(new PackageName(package.Id, package.Version.Version));

			// Retrieve package data
			info.Title			= package.Title;
			info.Summary		= package.Summary;
			info.Description	= package.Description;
			info.ReleaseNotes	= package.ReleaseNotes;
			info.RequireLicenseAcceptance = package.RequireLicenseAcceptance;
			info.ProjectUrl		= package.ProjectUrl;
			info.LicenseUrl		= package.LicenseUrl;
			info.IconUrl		= package.IconUrl;
			info.DownloadCount	= package.DownloadCount;
			info.PublishDate	= package.Published.HasValue ? package.Published.Value.DateTime : DateTime.MinValue;
			info.Authors		= package.Authors;
			info.Tags			= package.Tags != null ? package.Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();

			// Retrieve the matching set of dependencies. For now, don't support different sets and just pick the first one.
			var matchingDependencySet = package.DependencySets.FirstOrDefault();
			if (matchingDependencySet != null)
			{
				info.Dependencies = matchingDependencySet.Dependencies.Select(d => new PackageName(d.Id, (d.VersionSpec != null && d.VersionSpec.MinVersion != null) ? d.VersionSpec.MinVersion.Version : null));
			}

			return info;
		}

		private void OnPackageInstalled(PackageEventArgs args)
		{
			if (this.PackageInstalled != null)
				this.PackageInstalled(this, args);
		}
		private void OnPackageUninstalled(PackageEventArgs args)
		{
			if (this.PackageUninstalled != null)
				this.PackageUninstalled(this, args);
		}
		
		private void manager_PackageUninstalling(object sender, PackageOperationEventArgs e)
		{
			// Prevent NuGet from uninstalling Duality dependencies that aren't scheduled for uninstall
			PackageName packageName = new PackageName(e.Package.Id, e.Package.Version.Version);
			if (this.uninstallQueue != null && !this.uninstallQueue.Contains(packageName))
			{
				PackageInfo packageInfo = this.QueryPackageInfo(packageName);
				if (packageInfo.IsDualityPackage)
				{
					e.Cancel = true;
					Log.Editor.Write("Skip dependency uninstall of Duality package '{0}'.", packageName);
				}
			}
		}
		private void manager_PackageUninstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Integrating uninstall of package '{0} {1}'...", e.Package.Id, e.Package.Version);

			// Determine all files that are referenced by a package, and the ones referenced by this one
			IEnumerable<string> localFiles = this.CreateFileMapping(e.Package).Select(p => p.Key);

			// Schedule files for removal
			PackageUpdateSchedule updateSchedule = this.PrepareUpdateSchedule();
			foreach (var packageFile in localFiles)
			{
				// Don't remove any file that is still referenced by a local package
				bool stillInUse = false;
				foreach (NuGet.IPackage localNugetPackage in this.manager.LocalRepository.GetPackages())
				{
					Dictionary<string,string> localMapping = this.CreateFileMapping(localNugetPackage);
					if (localMapping.Any(p => PathHelper.Equals(p.Key, packageFile)))
					{
						stillInUse = true;
						break;
					}
				}
				if (stillInUse) continue;

				// Append the scheduled operation to the updater config file.
				updateSchedule.AppendDeleteFile(packageFile);
				if (Path.GetExtension(packageFile) == ".csproj")
				{
					updateSchedule.AppendSeparateProject(
						packageFile, 
						this.env.TargetSolutionPathRelative);
				}
			}
			this.SaveUpdateSchedule(updateSchedule);

			// Update local package configuration file
			this.setup.Packages.RemoveAll(p => p.Id == e.Package.Id);
			this.setup.Save(this.env.ConfigFilePath);

			this.OnPackageUninstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
		}
		private void manager_PackageInstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Integrating install of package '{0} {1}'...", e.Package.Id, e.Package.Version);
			
			// Update package entries from local config
			PackageInfo packageInfo = this.QueryPackageInfo(new PackageName(e.Package.Id, e.Package.Version.Version));
			if (packageInfo.IsDualityPackage)
			{
				this.setup.Packages.RemoveAll(p => p.Id == e.Package.Id);
				this.setup.Packages.Add(new LocalPackage(packageInfo));
				this.setup.Save(this.env.ConfigFilePath);
			}

			// Schedule files for updating / copying
			PackageUpdateSchedule updateSchedule = this.PrepareUpdateSchedule();
			Dictionary<string,string> fileMapping = this.CreateFileMapping(e.Package);
			foreach (var pair in fileMapping)
			{
				// Don't overwrite files from a newer version of this package with their old one (think of dependencies)
				bool isOldVersion = false;
				foreach (NuGet.IPackage localNugetPackage in this.manager.LocalRepository.GetPackages())
				{
					if (localNugetPackage.Id != e.Package.Id) continue;

					Dictionary<string,string> localMapping = this.CreateFileMapping(localNugetPackage);
					if (localMapping.Any(p => PathOp.ArePathsEqual(p.Key, pair.Key)))
					{
						if (localNugetPackage.Version > e.Package.Version)
						{
							isOldVersion = true;
							break;
						}
					}
				}
				if (isOldVersion) continue;

				// Append the scheduled operation to the updater config file.
				updateSchedule.AppendCopyFile(Path.Combine(e.InstallPath, pair.Value), pair.Key);
				if (Path.GetExtension(pair.Key) == ".csproj")
				{
					updateSchedule.AppendIntegrateProject(
						pair.Key, 
						this.env.TargetSolutionPathRelative, 
						this.env.TargetPluginPathRelative);
				}
			}
			this.SaveUpdateSchedule(updateSchedule);

			this.OnPackageInstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
		}
		
		private static bool IsDualityPackage(NuGet.IPackage package)
		{
			return
				package.Tags != null &&
				package.Tags.Contains(DualityTag);
		}
		private static void DisplayDefaultLicenseAcceptDialog(PackageLicenseAgreementEventArgs args)
		{
			LicenseAcceptDialog licenseDialog = new LicenseAcceptDialog
			{
				DescriptionText = string.Format(GeneralRes.LicenseAcceptDialog_PackageDesc, args.PackageName),
				LicenseUrl = args.LicenseUrl
			};

			Form invokeForm = DualityEditorApp.MainForm ?? Application.OpenForms.OfType<Form>().FirstOrDefault();
			DialogResult result = invokeForm.InvokeEx(main => licenseDialog.ShowDialog());
			if (result == DialogResult.OK)
			{
				args.AcceptLicense();
			}
		}
		public static string GetDisplayedVersion(Version version)
		{
			if (version == null)
				return string.Empty;
			else if (version.Build == 0)
				return string.Format("{0}.{1}", version.Major, version.Minor);
			else
				return string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
		}
	}
}
