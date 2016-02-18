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
		public const string DualityTag	= "Duality";
		public const string PluginTag	= "Plugin";
		public const string SampleTag	= "Sample";
		public const string CoreTag		= "Core";
		public const string EditorTag	= "Editor";
		public const string LauncherTag	= "Launcher";

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

		private const string UpdateConfigFile		= "ApplyUpdate.xml";
		private const string PackageConfigFile		= "PackageConfig.xml";
		private const string LocalPackageDir		= EditorHelper.SourceDirectory + @"\Packages";
		private const string DefaultRepositoryUrl	= @"https://packages.nuget.org/api/v2";


		private List<string>		repositoryUrls	= new List<string>{ DefaultRepositoryUrl };
		private	bool				firstInstall	= false;
		private	bool				hasLocalRepo	= false;
		private	string				dataTargetDir	= null;
		private	string				sourceTargetDir	= null;
		private	string				pluginTargetDir	= null;
		private	string				rootPath		= null;
		private	List<LocalPackage>	localPackages	= new List<LocalPackage>();
		private	List<LocalPackage>	uninstallQueue	= new List<LocalPackage>();

		private	object cacheLock = new object();
		private	Dictionary<string,NuGet.IPackage[]> repositoryPackageCache = new Dictionary<string,NuGet.IPackage[]>();
		private	Dictionary<NuGet.IPackage,bool> licenseAcceptedCache = new Dictionary<NuGet.IPackage,bool>();

		private NuGet.PackageManager		manager		= null;
		private	NuGet.IPackageRepository	repository	= null;

		public event EventHandler<PackageLicenseAgreementEventArgs> PackageLicenseAcceptRequired = null;
		public event EventHandler<PackageEventArgs> PackageInstalled = null;
		public event EventHandler<PackageEventArgs> PackageUninstalled = null;


		public IEnumerable<LocalPackage> LocalPackages
		{
			get { return this.localPackages; }
		}
		public bool IsFirstInstall
		{
			get { return this.firstInstall; }
		}
		public bool IsPackageUpdateRequired
		{
			get
			{
				return this.localPackages.Any(p => 
					p.Version == null || 
					p.Info == null || 
					!this.manager.LocalRepository.GetPackages().Any(n => 
						n.Id == p.Id && 
						n.Version == new SemanticVersion(p.Version)));
			}
		}
		/// <summary>
		/// [GET] The local directory where packages are installed and stored prior to applying the update.
		/// </summary>
		public string LocalPackageStoreDirectory
		{
			get { return LocalPackageDir; }
		}
		private string PackageFilePath
		{
			get { return Path.Combine(this.rootPath, PackageConfigFile); }
		}
		private string UpdateFilePath
		{
			get { return Path.Combine(this.rootPath, UpdateConfigFile); }
		}


		internal PackageManager(string rootPath = null, string dataTargetDir = null, string sourceTargetDir = null, string pluginTargetDir = null)
		{
			// Setup base parameters
			this.rootPath			= rootPath			?? Environment.CurrentDirectory;
			this.dataTargetDir		= dataTargetDir		?? DualityApp.DataDirectory;
			this.sourceTargetDir	= sourceTargetDir	?? EditorHelper.SourceCodeDirectory;
			this.pluginTargetDir	= pluginTargetDir	?? DualityApp.PluginDirectory;

			// Load additional config parameters
			this.LoadConfig();

			// Create internal package management objects
			IPackageRepository[] repositories = this.repositoryUrls.Select(x => this.CreateRepository(x)).Where(x => x != null).ToArray();
			this.hasLocalRepo = repositories.OfType<LocalPackageRepository>().Any();
			this.repository = new AggregateRepository(repositories);
			this.manager = new NuGet.PackageManager(this.repository, LocalPackageDir);
			this.manager.PackageInstalled += this.manager_PackageInstalled;
			this.manager.PackageUninstalled += this.manager_PackageUninstalled;
			this.manager.PackageUninstalling += this.manager_PackageUninstalling;

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

			// Request NuGet to install the package
			this.manager.InstallPackage(newPackage, false, false);
		}

		public void VerifyPackage(LocalPackage package)
		{
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
				this.localPackages.RemoveAll(p => p.Id == packageInfo.Id);
				this.localPackages.Add(new LocalPackage(packageInfo));
			}

			// In case we've just retrieved an explicit version for the first time, save the config file.
			if (oldPackageVersion == null)
			{
				this.SaveConfig();
			}
		}

		public void UninstallPackage(PackageInfo package)
		{
			this.UninstallPackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id));
		}
		public void UninstallPackage(LocalPackage package)
		{
			this.uninstallQueue.Add(package);
			this.manager.UninstallPackage(package.Id, new SemanticVersion(package.Version), false, true);
			this.uninstallQueue.Clear();
		}
		public bool CanUninstallPackage(PackageInfo package)
		{
			return this.CanUninstallPackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id));
		}
		[DebuggerNonUserCode]
		public bool CanUninstallPackage(LocalPackage package)
		{
			bool allowed = true;
			this.manager.WhatIf = true;
			try
			{
				this.manager.UninstallPackage(package.Id, new SemanticVersion(package.Version), false, true);
			}
			catch (Exception)
			{
				allowed = false;
			}
			this.manager.WhatIf = false;
			return allowed;
		}

		public void UpdatePackage(PackageInfo package)
		{
			this.UpdatePackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id));
		}
		public void UpdatePackage(LocalPackage package)
		{
			NuGet.IPackage newPackage = this.FindPackageInfo(new PackageName(package.Id));
			
			// Check license terms
			if (!this.CheckDeepLicenseAgreements(newPackage))
			{
				return;
			}

			this.uninstallQueue = null;
			this.manager.UpdatePackage(newPackage, true, false);
			this.uninstallQueue = new List<LocalPackage>();
		}
		public bool CanUpdatePackage(PackageInfo package)
		{
			return this.CanUpdatePackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id));
		}
		[DebuggerNonUserCode]
		public bool CanUpdatePackage(LocalPackage package)
		{
			bool allowed = true;
			this.manager.WhatIf = true;
			try
			{
				Version version = this.QueryPackageInfo(package.PackageName.VersionInvariant).Version;
				this.manager.UpdatePackage(package.Id, new SemanticVersion(version), true, false);
			}
			catch (Exception)
			{
				allowed = false;
			}
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
			LocalPackage[] targetPackages = this.localPackages.ToArray();
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
			if (this.localPackages.Any(local => local.Id == target.Id && local.Version == target.Version))
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
				LocalPackage local = this.localPackages.FirstOrDefault(p => p.Id == package.Id);
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
				deepDependencyCount.TryGetValue(a, out countA);
				deepDependencyCount.TryGetValue(b, out countB);
				return countA - countB;
			});
		}
		public void OrderByDependencies(IList<LocalPackage> packages)
		{
			// Map each list entry to its PackageInfo
			PackageInfo[] localInfo = packages.Select(p => p.Info ?? this.QueryPackageInfo(p.PackageName)).ToArray();

			// Sort the mapped list
			this.OrderByDependencies(localInfo);

			// Now sort the original list to match the sorted mapped list
			LocalPackage[] originalPackages = packages.ToArray();
			for (int i = 0; i < originalPackages.Length; i++)
			{
				LocalPackage localPackage = originalPackages[i];

				int newIndex = localInfo.IndexOfFirst(p => p.Id == localPackage.Id && p.Version == localPackage.Version);
				if (newIndex == -1)
					newIndex = localInfo.IndexOfFirst(p => p.Id == localPackage.Id);

				packages[newIndex] = localPackage;
			}
		}

		public bool ApplyUpdate(bool restartEditor = true)
		{
			const string UpdaterFileName = "DualityUpdater.exe";
			if (!File.Exists(this.UpdateFilePath)) return false;
			
			// Manually perform update operations on the updater itself
			try
			{
				XDocument updateDoc = this.PrepareUpdateFile();
				foreach (XElement elem in updateDoc.Root.Elements())
				{
					XAttribute attribTarget = elem.Attribute("target");
					XAttribute attribSource = elem.Attribute("source");
					string target = (attribTarget != null) ? attribTarget.Value : null;
					string source = (attribSource != null) ? attribSource.Value : null;

					// Only Updater-updates
					if (string.Equals(Path.GetFileName(target), UpdaterFileName, StringComparison.InvariantCultureIgnoreCase))
					{
						if (string.Equals(elem.Name.LocalName, "Remove", StringComparison.InvariantCultureIgnoreCase))
							File.Delete(target);
						else if (string.Equals(elem.Name.LocalName, "Update", StringComparison.InvariantCultureIgnoreCase))
							File.Copy(source, target, true);
					}
				}
			}
			catch (Exception e)
			{
				Log.Editor.WriteError("Can't update {0}, because an error occurred: {1}", UpdaterFileName, Log.Exception(e));
				return false;
			}

			// Run the updater application
			Process.Start(UpdaterFileName, string.Format("\"{0}\" \"{1}\" \"{2}\"",
				UpdateConfigFile,
				restartEditor ? typeof(DualityEditorApp).Assembly.Location : "",
				restartEditor ? Environment.CurrentDirectory : ""));

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
				resolveCache[package.PackageName] = package;
			}

			// Determine the dependency count of each package
			Dictionary<PackageInfo,int> result = new Dictionary<PackageInfo,int>();
			foreach (PackageInfo package in packages)
			{
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
							LocalPackage localDependency = this.localPackages.FirstOrDefault(p => p.Id == dependencyName.Id);
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
				// Do some additional checks that can't fit into the query itself
				if (!package.IsListed()) continue;
				if (!package.IsReleaseVersion()) continue;
				if (package.Version != new SemanticVersion(package.Version.Version)) continue;

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
			// Find a direct version match
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
					foreach (NuGet.IPackage package in this.GetRepositoryPackages(packageRef.Id))
					{
						if (package.Version.Version == packageRef.Version)
							return package;
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
			// Find the newest available version online
			else
			{
				NuGet.IPackage[] data = null;
				try
				{
					IEnumerable<NuGet.IPackage> query = this.GetRepositoryPackages(packageRef.Id);
					data = query.ToArray();
				}
				catch (Exception)
				{
					return null;
				}

				var packageQuery = data.Where(p => p.IsListed() && p.IsReleaseVersion());

				NuGet.IPackage newestPackage = packageQuery
					.OrderByDescending(p => p.Version.Version)
					.FirstOrDefault();

				if (newestPackage != null)
					return newestPackage;
			}

			// Nothing was found
			return null;
		}
		private IEnumerable<NuGet.IPackage> GetRepositoryPackages(string id)
		{
			NuGet.IPackage[] result;
			lock (this.cacheLock)
			{
				if (!this.repositoryPackageCache.TryGetValue(id, out result))
				{
					result = this.repository
						.FindPackagesById(id)
						.Where(p => p.IsReleaseVersion())
						.ToArray();
					this.repositoryPackageCache[id] = result;
				}
			}
			return result;
		}

		private void RetrieveLocalPackageInfo()
		{
			foreach (LocalPackage localPackage in this.localPackages)
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
					repositoryUrl = new Uri("file:///" + Path.GetFullPath(Path.Combine(this.rootPath, repositoryUrl))).AbsolutePath;
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
				if (installedPackages.Any(l => l.Id == p.Id && l.Version == p.Version))
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
				if (this.firstInstall)
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
		private void LoadConfig()
		{
			// Reset to default data
			this.localPackages.Clear();

			// Check whethere there is a config file to load
			string configFilePath = this.PackageFilePath;
			if (!File.Exists(configFilePath))
			{
				this.SaveConfig();
				return;
			}

			// If there is, load data from the config file
			try
			{
				XDocument doc = XDocument.Load(configFilePath);

				string firstInstallString = doc.Root.GetElementValue("FirstDualityInstall");
				bool firstInstallValue;
				if (!string.IsNullOrWhiteSpace(firstInstallString) && bool.TryParse(firstInstallString, out firstInstallValue))
					this.firstInstall = firstInstallValue;

				this.repositoryUrls.Clear();
				this.repositoryUrls.AddRange(doc.Root.Elements("RepositoryUrl").Select(x => x.Value));
				if (this.repositoryUrls.Count == 0)
				{
					this.repositoryUrls.Add(DefaultRepositoryUrl);
				}

				XElement packagesElement = doc.Root.Element("Packages");
				if (packagesElement != null)
				{
					foreach (XElement packageElement in packagesElement.Elements("Package"))
					{
						PackageName package = PackageName.None;
						package.Id = packageElement.GetAttributeValue("id");
						string versionString = packageElement.GetAttributeValue("version");
						if (versionString != null) Version.TryParse(versionString, out package.Version);

						// Skip invalid package references
						if (string.IsNullOrWhiteSpace(package.Id)) continue;

						// Create local package entry
						this.localPackages.Add(new LocalPackage(package));
					}
				}
			}
			catch (Exception e)
			{
				Log.Editor.WriteError(
					"Failed to load PackageManager config file '{0}': {1}",
					configFilePath,
					Log.Exception(e));
			}
		}
		private void SaveConfig()
		{
			XDocument doc = new XDocument(
				new XElement("PackageConfig",
					this.repositoryUrls.Select(x => new XElement("RepositoryUrl", x)),
					new XElement("Packages", this.localPackages.Select(p =>
						new XElement("Package",
							new XAttribute("id", p.Id),
							p.Version != null ? new XAttribute("version", p.Version) : null
						)
					))
				));
			doc.Save(this.PackageFilePath);
		}

		private XDocument PrepareUpdateFile()
		{
			// Load existing update file in order to update it
			string updateFilePath = this.UpdateFilePath;
			XDocument updateDoc = null;
			if (File.Exists(updateFilePath))
			{
				try
				{
					updateDoc = XDocument.Load(updateFilePath);
				}
				catch (Exception exception)
				{
					updateDoc = null;
					Log.Editor.WriteError("Can't update existing '{0}' file: {1}", 
						Path.GetFileName(updateFilePath), 
						Log.Exception(exception));
				}
			}

			// If none existed yet, create an update file skeleton
			if (updateDoc == null)
			{
				updateDoc = new XDocument(new XElement("UpdateConfig"));
			}

			return updateDoc;
		}
		private void AppendCopyUpdateFileEntry(XDocument updateDoc, string copySource, string copyTarget)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveUpdateFileEntries(updateDoc, "Remove", copyTarget);

			// Append the copy entry
			updateDoc.Root.Add(new XElement("Update", 
				new XAttribute("source", copySource), 
				new XAttribute("target", copyTarget)));
		}
		private void AppendDeleteUpdateFileEntry(XDocument updateDoc, string deleteTarget)
		{
			// Remove previous elements referring to the yet-to-delete file
			this.RemoveUpdateFileEntries(updateDoc, "Update", deleteTarget);
			this.RemoveUpdateFileEntries(updateDoc, "IntegrateProject", deleteTarget);

			// Append the delete entry
			updateDoc.Root.Add(new XElement("Remove", 
				new XAttribute("target", deleteTarget)));
		}
		private void AppendIntegrateProjectUpdateFileEntry(XDocument updateDoc, string projectFile, string solutionFile)
		{
			// Remove previous deletion schedules referring to the copy target
			this.RemoveUpdateFileEntries(updateDoc, "Remove", projectFile);
			this.RemoveUpdateFileEntries(updateDoc, "Remove", solutionFile);
			this.RemoveUpdateFileEntries(updateDoc, "SeparateProject", solutionFile);

			// Append the integrate entry
			updateDoc.Root.Add(new XElement("IntegrateProject", 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile), 
				new XAttribute("pluginDirectory", DualityApp.PluginDirectory)));
		}
		private void AppendSeparateProjectUpdateFileEntry(XDocument updateDoc, string projectFile, string solutionFile)
		{
			this.RemoveUpdateFileEntries(updateDoc, "IntegrateProject", projectFile);

			// Append the integrate entry
			updateDoc.Root.Add(new XElement("SeparateProject", 
				new XAttribute("project", projectFile), 
				new XAttribute("solution", solutionFile)));
		}
		private void RemoveUpdateFileEntries(XDocument updateDoc, string elementName, string referringTo)
		{
			var query = string.IsNullOrEmpty(elementName) ? updateDoc.Root.Elements() : updateDoc.Root.Elements(elementName);
			foreach (XElement element in query.ToArray())
			{
				bool anyReference = false;
				foreach (XAttribute attribute in element.Attributes())
				{
					try
					{
						if (PathOp.ArePathsEqual(attribute.Value, referringTo))
						{
							anyReference = true;
							break;
						}
					}
					catch (Exception) {}
				}
				if (anyReference)
				{
					element.Remove();
				}
			}
		}

		private Dictionary<string,string> CreateFileMapping(NuGet.IPackage package)
		{
			Dictionary<string,string> fileMapping = new Dictionary<string,string>();

			bool isDualityPackage = 
				package.Tags != null &&
				package.Tags.Contains(DualityTag);
			bool isPluginPackage = 
				isDualityPackage && 
				package.Tags.Contains(PluginTag);

			string folderFriendlyPackageName = package.Id;
			string binaryBaseDir = this.pluginTargetDir;
			string contentBaseDir = this.dataTargetDir;
			string sourceBaseDir = Path.Combine(this.sourceTargetDir, folderFriendlyPackageName);
			if (!isPluginPackage || !isDualityPackage) binaryBaseDir = "";

			foreach (var f in package.GetFiles()
				.Where(f => f.TargetFramework == null || f.TargetFramework.Version < Environment.Version)
				.OrderByDescending(f => f.TargetFramework == null ? new Version() : f.TargetFramework.Version)
				.OrderByDescending(f => f.TargetFramework == null))
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
			if (this.uninstallQueue != null)
			{
				PackageInfo packageInfo = this.QueryPackageInfo(new PackageName(e.Package.Id, e.Package.Version.Version));
				if (packageInfo.IsDualityPackage)
				{
					if (!this.uninstallQueue.Any(p => p.Id == e.Package.Id && p.Version == e.Package.Version.Version))
						e.Cancel = true;
				}
			}
		}
		private void manager_PackageUninstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Package removal scheduled: {0}, {1}", e.Package.Id, e.Package.Version);

			// Determine all files that are referenced by a package, and the ones referenced by this one
			IEnumerable<string> localFiles = this.CreateFileMapping(e.Package).Select(p => p.Key);

			// Schedule files for removal
			XDocument updateDoc = this.PrepareUpdateFile();
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
				this.AppendDeleteUpdateFileEntry(updateDoc, packageFile);
				if (Path.GetExtension(packageFile) == ".csproj")
					this.AppendSeparateProjectUpdateFileEntry(updateDoc, packageFile, EditorHelper.SourceCodeSolutionFile);
			}
			updateDoc.Save(this.UpdateFilePath);

			// Update local package configuration file
			this.localPackages.RemoveAll(p => p.Id == e.Package.Id);
			this.SaveConfig();

			this.OnPackageUninstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
		}
		private void manager_PackageInstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Package downloaded: {0}, {1}", e.Package.Id, e.Package.Version);
			
			// Update package entries from local config
			PackageInfo packageInfo = this.QueryPackageInfo(new PackageName(e.Package.Id, e.Package.Version.Version));
			if (packageInfo.IsDualityPackage)
			{
				this.localPackages.RemoveAll(p => p.Id == e.Package.Id);
				this.localPackages.Add(new LocalPackage(packageInfo));
				this.SaveConfig();
			}

			// Schedule files for updating / copying
			XDocument updateDoc = this.PrepareUpdateFile();
			Dictionary<string,string> fileMapping = this.CreateFileMapping(e.Package);
			foreach (var pair in fileMapping)
			{
				// Don't overwrite files from a newer version of this package with their old one (think of dependencies)
				bool isOldVersion = false;
				foreach (NuGet.IPackage localNugetPackage in this.manager.LocalRepository.GetPackages())
				{
					if (localNugetPackage.Id != e.Package.Id) continue;

					Dictionary<string,string> localMapping = this.CreateFileMapping(localNugetPackage);
					if (localMapping.Any(p => PathHelper.Equals(p.Key, pair.Key)))
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
				this.AppendCopyUpdateFileEntry(updateDoc, Path.Combine(e.InstallPath, pair.Value), pair.Key);
				if (Path.GetExtension(pair.Key) == ".csproj")
					this.AppendIntegrateProjectUpdateFileEntry(updateDoc, pair.Key, EditorHelper.SourceCodeSolutionFile);
			}
			updateDoc.Save(this.UpdateFilePath);

			this.OnPackageInstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
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
