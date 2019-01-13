using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Versioning;

using NuGet;

using Duality.IO;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor.PackageManagement
{
	/// <summary>
	/// Allows to install, uninstall, update and query local and remote Duality packages.
	/// </summary>
	public class PackageManager
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
		private static readonly string[] DualityPackageNames = new string[] 
		{
			"AdamsLair.Duality",
			"AdamsLair.Duality.Editor",
			"AdamsLair.Duality.Launcher"
		};


		private PackageSetup              setup        = new PackageSetup();
		private PackageManagerEnvironment env          = null;
		private PackageCache              cache        = null;

		private Dictionary<PackageName,bool>  licenseAccepted      = new Dictionary<PackageName,bool>();
		private List<PackageDependencyWalker> dependencyWalkerPool = new List<PackageDependencyWalker>();
		private object                        dependencyWalkerLock = new object();

		private NuGetTargetedPackageManager manager    = null;
		private NuGet.IPackageRepository   repository = null;
		private PackageManagerLogger       logger     = null;

		public event EventHandler<PackageLicenseAgreementEventArgs> PackageLicenseAcceptRequired = null;
		public event EventHandler<PackageEventArgs>                 PackageInstalled             = null;
		public event EventHandler<PackageEventArgs>                 PackageUninstalled           = null;


		/// <summary>
		/// [GET] Whether the local <see cref="PackageSetup"/> and the actually installed packages
		/// are currently out of sync.
		/// </summary>
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
		/// <summary>
		/// [GET] The local file system environment in which this <see cref="PackageManager"/> operates.
		/// </summary>
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
						Logs.Editor.WriteError(
							"Failed to load PackageManager config file '{0}': {1}",
							configFilePath,
							LogFormat.Exception(e));
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
			this.repository = new AggregateRepository(repositories);
			this.manager = new NuGetTargetedPackageManager(
				new FrameworkName(".NETFramework", new Version(4, 5, 2)), 
				this.repository, 
				this.env.RepositoryPath);
			this.manager.PackageInstalling += this.manager_PackageInstalling;
			this.manager.PackageInstalled += this.manager_PackageInstalled;
			this.manager.PackageUninstalled += this.manager_PackageUninstalled;
			this.cache = new PackageCache(
				this.repository,
				repositories.OfType<LocalPackageRepository>().Any());

			this.logger = new PackageManagerLogger();
			this.manager.Logger = this.logger;

			// Retrieve information about local packages
			this.RetrieveLocalPackageInfo();
		}
		

		/// <summary>
		/// Installs the specified package if it wasn't installed yet and synchronizes
		/// the registered Duality package version number with the one that is present
		/// in the local cache.
		/// </summary>
		/// <param name="localPackage"></param>
		public void VerifyPackage(LocalPackage localPackage)
		{
			Version oldPackageVersion = localPackage.Version;

			// Determine the exact version that will be installed
			PackageInfo packageInfo = this.GetPackage(localPackage.Name);
			if (packageInfo == null)
			{
				throw new Exception(string.Format(
					"Can't resolve version of package '{0}'. There seems to be no compatible version available.",
					localPackage.Id));
			}

			// Prepare a listener to determine whether we actually installed something
			EventHandler<PackageOperationEventArgs> installListener = null;
			bool packageInstalled = false;
			installListener = delegate(object sender, PackageOperationEventArgs args)
			{
				if (args.Package.Id == localPackage.Id)
				{
					packageInstalled = true;
				}
			};

			// Install the package. Won't do anything if the package is already installed.
			this.manager.PackageInstalled += installListener;
			this.InstallPackage(localPackage.Name, true);
			this.manager.PackageInstalled -= installListener;

			// If we didn't install anything, that package was already in the local repo.
			// Update the local setup to match its specific version.
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
				this.UninstallPackage(
					new PackageName(package.Id, package.Version.Version), 
					true);
			}
		}

		/// <summary>
		/// Installs the specified package.
		/// </summary>
		/// <param name="packageName"></param>
		public void InstallPackage(PackageName packageName)
		{
			this.InstallPackage(packageName, false);
		}
		private void InstallPackage(PackageName packageName, bool skipLicense)
		{
			NuGet.IPackage installPackage = this.cache.GetNuGetPackage(packageName);
			if (installPackage == null)
			{
				throw new InvalidOperationException(string.Format(
					"Unable to install package '{0}', because no matching package could be found.",
					packageName));
			}

			// Check license terms
			if (!skipLicense && !this.CheckDeepLicenseAgreements(installPackage))
			{
				return;
			}

			Logs.Editor.Write("Installing package '{0}'...", packageName);
			Logs.Editor.PushIndent();
			try
			{
				// Request NuGet to install the package
				this.manager.InstallPackage(installPackage, false, false);
			}
			finally
			{
				Logs.Editor.PopIndent();
			}
		}
		/// <summary>
		/// Uninstalls the specified package.
		/// </summary>
		/// <param name="packageName"></param>
		public void UninstallPackage(PackageName packageName)
		{
			this.UninstallPackage(packageName, false);
		}
		private void UninstallPackage(PackageName packageName, bool force)
		{
			Logs.Editor.Write("Uninstalling package '{0}'...", packageName);
			Logs.Editor.PushIndent();
			try
			{
				// Find the local package that we'll uninstall
				NuGet.IPackage uninstallPackage = this.manager.LocalRepository
					.FindPackagesById(packageName.Id)
					.FirstOrDefault(p => p.Version.Version == packageName.Version);
				if (uninstallPackage == null) return;

				// Find all dependencies of the package we want to uninstall
				PackageName uninstallPackageName = new PackageName(uninstallPackage.Id, uninstallPackage.Version.Version);
				PackageInfo uninstallPackageInfo = this.GetPackage(uninstallPackageName);
				List<PackageInfo> uninstallDependencies = new List<PackageInfo>();
				if (uninstallPackageInfo != null)
				{
					PackageDependencyWalker dependencyWalker = this.RentDependencyWalker();
					dependencyWalker.IgnorePackage("NETStandard.Library"); // Avoid dependency explosion, treat as opaque
					dependencyWalker.WalkGraph(uninstallPackageInfo);
					uninstallDependencies.AddRange(dependencyWalker.VisitedPackages);
					uninstallDependencies.RemoveAll(package => package.Id == uninstallPackageInfo.Id);
					this.ReturnDependencyWalker(ref dependencyWalker);
				}

				// Filter out all dependencies that are used by other Duality packages
				foreach (LocalPackage otherPackage in this.LocalSetup.Packages)
				{
					if (otherPackage.Id == uninstallPackage.Id) continue;

					PackageInfo otherPackageInfo = otherPackage.Info ?? this.GetPackage(otherPackage.Name);
					if (otherPackageInfo == null) continue;
				
					PackageDependencyWalker dependencyWalker = this.RentDependencyWalker();
					dependencyWalker.IgnorePackage(uninstallPackage.Id);
					dependencyWalker.IgnorePackage("NETStandard.Library"); // Avoid dependency explosion, treat as opaque
					dependencyWalker.WalkGraph(otherPackageInfo);
					foreach (PackageInfo dependency in dependencyWalker.VisitedPackages)
					{
						// Don't check versions, as dependencies are usually not resolved
						// with an exact version match.
						uninstallDependencies.RemoveAll(item => item.Id == dependency.Id);
					}
					this.ReturnDependencyWalker(ref dependencyWalker);
				}

				// Uninstall the package itself
				this.manager.UninstallPackage(
					uninstallPackage,
					force);

				// Uninstall its dependencies that are no longer used, in reverse dependency order
				// to avoid stumbling over dependencies that they might have between each other.
				this.OrderByDependencies(uninstallDependencies);
				uninstallDependencies.Reverse();
				foreach (PackageInfo package in uninstallDependencies)
				{
					List<NuGet.IPackage> matchingNuGetPackages = this.manager.LocalRepository
						.FindPackagesById(package.Id)
						.ToList();

					foreach (NuGet.IPackage nugetPackage in matchingNuGetPackages)
					{
						this.manager.UninstallPackage(nugetPackage, force, false);
					}
				}
			}
			finally
			{
				Logs.Editor.PopIndent();
			}
		}
		/// <summary>
		/// Determines whether the specified package can be uninstalled. Returns
		/// false when other packages still depend on it.
		/// </summary>
		/// <param name="package"></param>
		/// <returns></returns>
		[DebuggerNonUserCode]
		public bool CanUninstallPackage(PackageName packageName)
		{
			NuGet.IPackage uninstallPackage = this.manager.LocalRepository
				.FindPackagesById(packageName.Id)
				.FirstOrDefault(p => p.Version.Version == packageName.Version);
			if (uninstallPackage == null) return false;

			bool allowed = true;
			this.manager.WhatIf = true;
			this.manager.Logger = null;
			try
			{
				this.manager.UninstallPackage(
					uninstallPackage,
					false, 
					true);
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
		/// Updates the specified package to the latest available version.
		/// </summary>
		/// <param name="packageName"></param>
		public void UpdatePackage(PackageName packageName)
		{
			if (!this.manager.LocalRepository.FindPackagesById(packageName.Id).Any())
			{
				throw new InvalidOperationException(string.Format(
					"Can't update package '{0}', because it is not installed.",
					packageName));
			}

			NuGet.IPackage latestPackage = this.cache.GetNuGetPackage(packageName.VersionInvariant);
			if (latestPackage == null)
			{
				throw new InvalidOperationException(string.Format(
					"Unable to update package '{0}', because no matching package could be found.",
					packageName));
			}

			// Check license terms
			if (!this.CheckDeepLicenseAgreements(latestPackage))
			{
				return;
			}
			
			Logs.Editor.Write("Updating package '{0}'...", packageName);
			Logs.Editor.PushIndent();
			try
			{
				this.manager.UpdatePackage(latestPackage, true, false);
			}
			finally
			{
				Logs.Editor.PopIndent();
			}
		}

		/// <summary>
		/// Starts applying any pending package updates and returns true when it is required
		/// to shut down the current editor instance in order to complete them.
		/// </summary>
		/// <param name="restartEditor">
		/// Whether the current editor instance should be restarted after the update has been applied.
		/// </param>
		/// <returns></returns>
		public bool ApplyUpdate(bool restartEditor = true)
		{
			if (!File.Exists(this.env.UpdateFilePath)) return false;

			Logs.Editor.Write("Applying package update...");
			Logs.Editor.PushIndent();
			try
			{
				// Manually perform update operations on the updater itself
				try
				{
					Logs.Editor.Write("Preparing updater...");
					PackageUpdateSchedule schedule = this.PrepareUpdateSchedule();
					schedule.ApplyUpdaterChanges(this.env.UpdaterExecFilePath);
					this.SaveUpdateSchedule(schedule);
				}
				catch (Exception e)
				{
					Logs.Editor.WriteError(
						"Can't update '{0}', because an error occurred: {1}", 
						this.env.UpdaterExecFilePath, 
						LogFormat.Exception(e));
					return false;
				}

				// Run the updater application
				Logs.Editor.Write("Running updater...");
				Process.Start(this.env.UpdaterExecFilePath, string.Format("\"{0}\" \"{1}\" \"{2}\"",
					this.env.UpdateFilePath,
					restartEditor ? typeof(DualityEditorApp).Assembly.Location : "",
					restartEditor ? Environment.CurrentDirectory : ""));
			}
			finally
			{
				Logs.Editor.PopIndent();
			}

			return true;
		}
		
		/// <summary>
		/// Clears the internal cache for remote repository packages, allowing
		/// to retrieve updated information about the latest available packages
		/// and package versions.
		/// </summary>
		public void ClearCache()
		{
			this.cache.Clear();
		}
		/// <summary>
		/// Enumerates the latest versions of all available Duality packages from
		/// the remote repository.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<PackageInfo> GetLatestDualityPackages()
		{
			return this.cache.GetLatestDualityPackages();
		}
		/// <summary>
		/// Retrieves detailed information about the specified package from the
		/// local or remote repository.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public PackageInfo GetPackage(PackageName name)
		{
			return this.cache.GetPackage(name);
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
				PackageInfo update = this.GetPackage(targetPackages[i].Name.VersionInvariant);
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
			PackageDependencyWalker dependencyWalker = this.RentDependencyWalker();
			dependencyWalker.IgnorePackage("NETStandard.Library"); // Avoid dependency explosion, doesn't matter for Duality compatibility anyway
			dependencyWalker.WalkGraph(target);
			List<PackageInfo> touchedPackages = dependencyWalker.VisitedPackages.ToList();
			this.ReturnDependencyWalker(ref dependencyWalker);

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
			PackageDependencyWalker dependencyWalker = this.RentDependencyWalker();
			dependencyWalker.IgnorePackage("NETStandard.Library"); // Avoid dependency explosion, treat as opaque
			dependencyWalker.WalkGraph(packages);

			// Sort packages according to their deep dependency counts
			packages.StableSort((a, b) =>
			{
				int countA = (a == null) ? 0 : dependencyWalker.GetDependencyCount(a.Name);
				int countB = (b == null) ? 0 : dependencyWalker.GetDependencyCount(b.Name);
				return countA - countB;
			});

			this.ReturnDependencyWalker(ref dependencyWalker);
		}
		/// <summary>
		/// Sorts the specified list of packages according to their dependencies, guaranteeing that no package
		/// is listed before its dependencies. Use this to determine the order of batch updates and installs
		/// to prevent conflicts from having different versions of the same packages.
		/// </summary>
		public void OrderByDependencies(IList<LocalPackage> packages)
		{
			// Map each list entry to its PackageInfo
			PackageInfo[] localInfo = packages
				.Select(p => p.Info ?? this.GetPackage(p.Name))
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

		private void RetrieveLocalPackageInfo()
		{
			foreach (LocalPackage setupItem in this.setup.Packages)
			{
				if (setupItem.Version == null) continue;
				if (string.IsNullOrEmpty(setupItem.Id)) continue;

				foreach (NuGet.IPackage localPackage in this.manager.LocalRepository.FindPackagesById(setupItem.Id))
				{
					if (localPackage.Version.Version != setupItem.Version) continue;

					setupItem.Info = new PackageInfo(localPackage);
					break;
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
					repositoryUrl = Path.GetFullPath(Path.Combine(this.env.RootPath, repositoryUrl));
			}
			catch (UriFormatException)
			{
				Logs.Editor.WriteError("NuGet repository URI '{0}' has an incorrect format and will be skipped.", repositoryUrl);
				return null;
			}

			return PackageRepositoryFactory.Default.CreateRepository(repositoryUrl);
		}

		/// <summary>
		/// Rents a pooled <see cref="PackageDependencyWalker"/> instance, or allocates a new one when required.
		/// Not returning a rented instance will harm efficiency, but not cause a leak.
		/// </summary>
		/// <returns></returns>
		private PackageDependencyWalker RentDependencyWalker()
		{
			lock (this.dependencyWalkerLock)
			{
				if (this.dependencyWalkerPool.Count == 0)
				{
					return new PackageDependencyWalker(this.GetPackage);
				}
				else
				{
					int lastWalkerIndex = this.dependencyWalkerPool.Count - 1;
					PackageDependencyWalker walker = this.dependencyWalkerPool[lastWalkerIndex];
					this.dependencyWalkerPool.RemoveAt(lastWalkerIndex);
					return walker;
				}
			}
		}
		/// <summary>
		/// Returns a previously rented <see cref="PackageDependencyWalker"/> instance, and nullifies the
		/// provided reference to it afterwards to avoid further use.
		/// </summary>
		/// <param name="walker"></param>
		private void ReturnDependencyWalker(ref PackageDependencyWalker walker)
		{
			walker.Clear();
			lock (this.dependencyWalkerLock)
			{
				this.dependencyWalkerPool.Add(walker);
				walker = null;
			}
		}

		private bool CheckDeepLicenseAgreements(NuGet.IPackage package)
		{
			PackageDependencyWalker dependencyWalker = this.RentDependencyWalker();
			dependencyWalker.IgnorePackage("NETStandard.Library"); // Avoid dependency explosion, doesn't require license acceptance anyway
			dependencyWalker.WalkGraph(new PackageName(package.Id, package.Version.Version));

			List<PackageInfo> packageGraph = dependencyWalker.VisitedPackages.ToList();
			List<NuGet.IPackage> installedPackages = this.manager.LocalRepository.GetPackages().ToList();

			this.ReturnDependencyWalker(ref dependencyWalker);

			foreach (PackageInfo visitedPackage in packageGraph)
			{
				// Skip the ones that are already installed
				if (installedPackages.Any(installed => 
					installed.Id == visitedPackage.Id && 
					installed.Version.Version >= visitedPackage.Version))
					continue;

				if (!this.CheckLicenseAgreement(visitedPackage))
					return false;
			}

			return true;
		}
		private bool CheckLicenseAgreement(PackageInfo package)
		{
			if (package.RequireLicenseAcceptance)
			{
				// On the very first install, do not display additional license agreement dialogs
				// because all the packages being installed are the default ones.
				if (this.setup.IsFirstInstall)
					return true;

				bool agreed;
				if (!this.licenseAccepted.TryGetValue(package.Name, out agreed) || !agreed)
				{
					PackageLicenseAgreementEventArgs args = new PackageLicenseAgreementEventArgs(
						package.Name,
						package.LicenseUrl,
						package.RequireLicenseAcceptance);

					if (this.PackageLicenseAcceptRequired != null)
						this.PackageLicenseAcceptRequired(this, args);
					else
						DisplayDefaultLicenseAcceptDialog(args);

					agreed = args.IsLicenseAccepted;
					this.licenseAccepted[package.Name] = agreed;
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
					Logs.Editor.WriteError("Error parsing existing package update schedule '{0}': {1}", 
						Path.GetFileName(updateFilePath), 
						LogFormat.Exception(exception));
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

			List<IPackageFile> applicableFiles = new List<IPackageFile>();
			try
			{
				// Separate package files in to framework-dependent lib files and framework-independent 
				// non-lib files. Those include content and source files, but also any other folder structure
				// that isn't specifically a lib binary.
				List<IPackageFile> files = package.GetFiles().ToList();
				List<IPackageFile> libFiles = new List<IPackageFile>();
				List<IPackageFile> nonLibFiles = new List<IPackageFile>();
				foreach (IPackageFile file in files)
				{
					string path = file.Path.Replace('/', '\\');
					bool isLibFile = path.StartsWith("lib\\");
					if (isLibFile)
						libFiles.Add(file);
					else
						nonLibFiles.Add(file);
				}
				List<IPackageFile> applicableLibFiles = new List<IPackageFile>();

				// Determine which frameworks are available in this package.
				// Note that due to the NuGet version we're using, .NETStandard target frameworks
				// are unknown and will be returned as "Unsupported, Version 0.0".
				List<FrameworkName> availableFrameworks = libFiles.Select(f => f.TargetFramework).Distinct().ToList();

				// Select the closest matching framework this package has and use all of its files,
				// and none of the others
				FrameworkName matchingFramework = SelectBestFrameworkMatch(availableFrameworks);
				applicableLibFiles.AddRange(libFiles.Where(f => f.TargetFramework == matchingFramework));

				// Check if we have files without a target framework that do not have an equivalent
				// in the files from the selected match. To support legacy packages, we'll use them
				// as a fallback. Packages that do this are for example AdamsLair.OpenTK, AdamsLair.WinForms
				// and others which have a set of explicit root files and a subset of implicit framework files.
				if (matchingFramework != null)
				{
					List<IPackageFile> rootFiles = libFiles.Where(f => f.TargetFramework == null).ToList();
					foreach (IPackageFile file in rootFiles)
					{
						string fileName = Path.GetFileName(file.Path);
						bool hasOverride = applicableLibFiles.Any(f => Path.GetFileName(f.Path) == fileName);
						if (!hasOverride)
						{
							applicableLibFiles.Add(file);
						}
					}
				}

				// Non-lib files (content, source) are treated differently and used as-is, since they are
				// not dependent on any target framework and just carried over.
				applicableFiles.AddRange(applicableLibFiles);
				applicableFiles.AddRange(nonLibFiles);
			}
			catch (DirectoryNotFoundException)
			{
				// We'll run into this exception when uninstalling a package without any files.
				return fileMapping;
			}

			foreach (IPackageFile f in applicableFiles)
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
		
		private void manager_PackageUninstalled(object sender, PackageOperationEventArgs e)
		{
			Logs.Editor.Write("Integrating uninstall of package '{0} {1}'...", e.Package.Id, e.Package.Version);

			// Determine all files that are referenced by a package, and the ones referenced by this one
			IEnumerable<string> localFiles = this.CreateFileMapping(e.Package).Select(p => p.Key);

			// Schedule files for removal
			PackageUpdateSchedule updateSchedule = this.PrepareUpdateSchedule();
			foreach (string packageFile in localFiles)
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
			PackageName packageName = new PackageName(e.Package.Id, e.Package.Version.Version);
			this.setup.Packages.RemoveAll(p => p.Name == packageName);
			this.setup.Save(this.env.ConfigFilePath);

			this.OnPackageUninstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
		}
		private void manager_PackageInstalling(object sender, PackageOperationEventArgs e)
		{
			// Duality does, by design, not support multiple versions of the same package to be
			// installed at the same time. Enforce this constraint by double-checking every install
			// for already installed versions of the same package.
			List<NuGet.IPackage> previousPackages = this.manager.LocalRepository
				.FindPackagesById(e.Package.Id)
				.ToList();
			foreach (NuGet.IPackage package in previousPackages)
			{
				// Installing a newer version? Make sure to uninstall any older versions of it.
				if (package.Version < e.Package.Version)
				{
					this.manager.UninstallPackage(
						package,
						true, 
						false);
				}
				// Installing an older version? Cancel the operation. We'll prefer the newer 
				// version and if a downgrade is really desired, it should be done by explicitly 
				// uninstalling the newer package first.
				else if (package.Version > e.Package.Version)
				{
					e.Cancel = true;
			}
		}
		}
		private void manager_PackageInstalled(object sender, PackageOperationEventArgs e)
		{
			Logs.Editor.Write("Integrating install of package '{0} {1}'...", e.Package.Id, e.Package.Version);
			
			// Update package entries from local config
			PackageInfo packageInfo = this.GetPackage(new PackageName(e.Package.Id, e.Package.Version.Version));
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
					if (localMapping.Any(p => string.Equals(p.Value, pair.Value, StringComparison.InvariantCultureIgnoreCase)))
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

		/// <summary>
		/// From the given list of target frameworks, this methods selects the one that best matches
		/// the one we want for installed packages.
		/// </summary>
		/// <param name="frameworks"></param>
		/// <returns></returns>
		internal static FrameworkName SelectBestFrameworkMatch(List<FrameworkName> frameworks)
		{
			int highestScore = -1;
			FrameworkName bestMatch = null;
			foreach (FrameworkName framework in frameworks)
			{
				int score = GetFrameworkMatchScore(framework);
				if (score > highestScore)
				{
					highestScore = score;
					bestMatch = framework;
				}
			}
			return bestMatch;
		}
		/// <summary>
		/// Determines a score value for the given target framework representing how well it
		/// matches the one we want for installed packages.
		/// </summary>
		/// <param name="framework"></param>
		/// <returns></returns>
		private static int GetFrameworkMatchScore(FrameworkName framework)
		{
			// A null framework is a valid value, as it represents NuGet package files
			// that are not associated with any specific target framework. For legacy
			// reasons, we score them slightly higher than defined, but completely unknown
			// frameworks.
			if (framework == null) return 1;

			// Since the context of our package selection is editor and desktop deployment
			// we'll prefer .NET Framework 4.5 binaries over others.
			switch (framework.Identifier)
			{
				case ".NETPortable":
					if (framework.Profile == "net45+win8+wpa81") // Profile 111
						return 50;
					else if (framework.Profile.Contains("net45"))
						return 49;
					else if (framework.Profile.Contains("net40"))
						return 48;
					else
						return 40;
				case ".NETFramework":
					if (framework.Version == new Version(4, 5))
						return 100;
					else if (framework.Version < new Version(4, 5))
						return 90;
					else
						return 80;
				default:
					return 0;
			}
		}
	}
}
