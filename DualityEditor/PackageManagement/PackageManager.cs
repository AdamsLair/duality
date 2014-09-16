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

namespace Duality.Editor.PackageManagement
{
	public sealed class PackageManager
	{
		internal const string DualityTag = "Duality";
		internal const string PluginTag = "Plugin";

		private	const	string	UpdateConfigFile			= "ApplyUpdate.xml";
		private	const	string	PackageConfigFile			= "PackageConfig.xml";
		private const	string	LocalPackageDir				= EditorHelper.SourceDirectory + @"\Packages";
		private	const	string	DefaultRepositoryUrl		= @"https://packages.nuget.org/api/v2";

		private	string					repositoryOriginal	= null;
		private	Uri						repositoryUrl		= null;
		private	string					dataTargetDir		= null;
		private	string					pluginTargetDir		= null;
		private	string					rootPath			= null;
		private	List<LocalPackage>		localPackages		= new List<LocalPackage>();
		private	List<LocalPackage>		uninstallQueue		= new List<LocalPackage>();

		private	object cacheLock = new object();
		private	Dictionary<PackageNamePair,PackageCompatibility> forwardCompatibilityCache = new Dictionary<PackageNamePair,PackageCompatibility>();
		private	Dictionary<PackageName,int> dependencyScoreCache = new Dictionary<PackageName,int>();
		private	Dictionary<string,NuGet.IPackage[]> repositoryPackageCache = new Dictionary<string,NuGet.IPackage[]>();

		private NuGet.PackageManager		manager			= null;
		private	NuGet.IPackageRepository	repository		= null;

		public event EventHandler<PackageEventArgs> PackageInstalled = null;
		public event EventHandler<PackageEventArgs> PackageUninstalled = null;


		public Uri RepositoryUrl
		{
			get { return this.repositoryUrl; }
		}
		public IEnumerable<LocalPackage> LocalPackages
		{
			get { return this.localPackages; }
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
		private string PackageFilePath
		{
			get { return Path.Combine(this.rootPath, PackageConfigFile); }
		}
		private string UpdateFilePath
		{
			get { return Path.Combine(this.rootPath, UpdateConfigFile); }
		}


		internal PackageManager(string rootPath = null, string dataTargetDir = null, string pluginTargetDir = null)
		{
			// Setup base parameters
			this.rootPath			= rootPath			?? Environment.CurrentDirectory;
			this.dataTargetDir		= dataTargetDir		?? DualityApp.DataDirectory;
			this.pluginTargetDir	= pluginTargetDir	?? DualityApp.PluginDirectory;

			// Load additional config parameters
			this.LoadConfig();

			// Create internal package management objects
			this.repository = NuGet.PackageRepositoryFactory.Default.CreateRepository(this.repositoryUrl.AbsoluteUri);
			this.manager = new NuGet.PackageManager(this.repository, LocalPackageDir);
			this.manager.PackageInstalled += this.manager_PackageInstalled;
			this.manager.PackageUninstalled += this.manager_PackageUninstalled;
			this.manager.PackageUninstalling += this.manager_PackageUninstalling;

			// Retrieve information about local packages
			this.RetrieveLocalPackageInfo();
		}

		public void InstallPackage(PackageInfo package)
		{
			// Request NuGet to install the package
			NuGet.IPackage newPackage = this.FindPackageInfo(package.PackageName, false);
			this.manager.InstallPackage(newPackage, false, false);
		}
		public void VerifyPackage(LocalPackage package)
		{
			Version oldPackageVersion = package.Version;

			// Determine the exact version that will be downloaded
			PackageInfo packageInfo = this.QueryPackageInfo(package.PackageName);
			if (packageInfo == null)
			{
				throw new ApplicationException(string.Format(
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
			this.InstallPackage(packageInfo);
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
		[System.Diagnostics.DebuggerNonUserCode]
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

		public void UpdatePackage(PackageInfo package, Version specificVersion = null)
		{
			this.UpdatePackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id), specificVersion);
		}
		public void UpdatePackage(LocalPackage package, Version specificVersion = null)
		{
			// Due to a bug in NuGet 2.8.2, specific-version downgrades are limited to the package itself,
			// without updating its dependencies. Otherwise, some of them might be uninstalled without
			// being reinstalled properly.

			this.uninstallQueue = null;
			bool isDowngrade = specificVersion != null && specificVersion < package.Version;
			NuGet.IPackage newPackage = this.FindPackageInfo(new PackageName(package.Id, specificVersion), false);
			this.manager.UpdatePackage(newPackage, !isDowngrade, false);
			this.uninstallQueue = new List<LocalPackage>();
		}
		public bool CanUpdatePackage(PackageInfo package, Version specificVersion = null)
		{
			return this.CanUpdatePackage(this.localPackages.FirstOrDefault(p => p.Id == package.Id), specificVersion);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public bool CanUpdatePackage(LocalPackage package, Version specificVersion = null)
		{
			bool allowed = true;
			this.manager.WhatIf = true;
			try
			{
				bool isSpecific = specificVersion != null;
				if (specificVersion == null) specificVersion = this.QueryPackageInfo(package.PackageName.VersionInvariant).Version;
				this.manager.UpdatePackage(package.Id, new SemanticVersion(specificVersion), !isSpecific, false);
			}
			catch (Exception)
			{
				allowed = false;
			}
			this.manager.WhatIf = false;
			return allowed;
		}

		/// <summary>
		/// Determines whether all installed pacakges are forward compatible to <paramref name="target"/>.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public PackageCompatibility GetForwardCompatibility(PackageInfo target)
		{
			// ToDo: Implement this
			return PackageCompatibility.Definite;
		}
		/// <summary>
		/// Given the specified set of packages, this method returns a new set of the same packages where each version is the newest one
		/// that can be safely updated to, given the specified minimum forward compatibility level. If a package cannot be updated at all,
		/// it will be omitted in the resulting list.
		/// </summary>
		/// <param name="packages"></param>
		/// <param name="minCompatibility"></param>
		/// <returns></returns>
		public IEnumerable<PackageInfo> GetSafeUpdateConfig(IEnumerable<PackageInfo> packages, PackageCompatibility minCompatibility)
		{
			List<PackageInfo> safeUpdateList = new List<PackageInfo>();
			PackageInfo[] targetPackages = packages.OrderByDescending(p => this.GetDependencyScore(p.PackageName)).ToArray();

			for (int i = 0; i < targetPackages.Length; i++)
			{
				PackageInfo package = targetPackages[i];
				Version maxVersion = null;

				PackageInfo update = this.QueryPackageInfo(new PackageName(package.Id, maxVersion), true);
				if (update.Version <= package.Version) continue;

				// ToDo: Implement this

				safeUpdateList.Add(update);
			}

			return safeUpdateList;
		}
		/// <summary>
		/// Given the specified set of packages, this method returns a new set of the same packages where each version is the newest one
		/// that can be safely updated to, given the specified minimum forward compatibility level. If a package cannot be updated at all,
		/// it will be omitted in the resulting list.
		/// </summary>
		/// <param name="packages"></param>
		/// <param name="minCompatibility"></param>
		/// <returns></returns>
		public IEnumerable<PackageInfo> GetSafeUpdateConfig(IEnumerable<LocalPackage> packages, PackageCompatibility minCompatibility = PackageCompatibility.Likely)
		{
			var localInfo = packages.Select(p => p.Info ?? this.QueryPackageInfo(p.PackageName)).ToArray();
			return this.GetSafeUpdateConfig(localInfo, minCompatibility);
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
			return this.QueryPackageInfo(packageRef, false);
		}
		private PackageInfo QueryPackageInfo(PackageName packageRef, bool findMaxVersionBelow)
		{
			NuGet.IPackage package = this.FindPackageInfo(packageRef, findMaxVersionBelow);
			return package != null ? this.CreatePackageInfo(package) : null;
		}
		
		private PackageCompatibility GetIndividualForwardCompatibility(PackageInfo package, PackageInfo target)
		{
			if (package.Id == target.Id) return PackageCompatibility.Definite;

			PackageNamePair key = new PackageNamePair { First = package.PackageName, Second = target.PackageName };
			PackageCompatibility compatibility;
			lock (this.cacheLock)
			{
				if (this.forwardCompatibilityCache.TryGetValue(key, out compatibility))
					return compatibility;
			}

			compatibility = PackageCompatibility.Definite;

			// Evaluate how the package depends on the target.
			PackageName dependecy = package.Dependencies.FirstOrDefault(d => d.Id == target.Id);
			if (!string.IsNullOrWhiteSpace(dependecy.Id))
			{
				PackageInfo dependencyInfo = this.QueryPackageInfo(dependecy);
				if (dependencyInfo == null)
				{
					// If no information is available on this dependency, don't assume compatibility
					compatibility = PackageCompatibility.None;
				}
				else if (!dependencyInfo.IsDualityPackage)
				{
					// If it is not a Duality package, ignore this
					compatibility = PackageCompatibility.Definite;
				}
				else if (dependecy.Version >= target.Version)
				{
					// If the package depends on a newer or similar version, it's okay
					compatibility = PackageCompatibility.Definite;
				}
				else if (dependecy.Version.Major == target.Version.Major && dependecy.Version.Minor == target.Version.Minor)
				{
					// If the target is equal in major and minor, it should be okay
					compatibility = PackageCompatibility.Likely;
				}
				else if (dependencyInfo.PublishDate > target.PublishDate)
				{
					// If the package depends on something that was released after the target, it should be okay
					compatibility = PackageCompatibility.Likely;
				}
				else if (dependecy.Version.Major == target.Version.Major)
				{
					// If the target is equal in major, it could be okay but doesn't have to be
					compatibility = PackageCompatibility.Unlikely;
				}
				else 
				{
					// Otherwise, no compatibility can be assumed
					compatibility = PackageCompatibility.None;
				}
			}
			
			lock (this.cacheLock)
			{
				this.forwardCompatibilityCache[key] = compatibility;
			}
			return compatibility;
		}
		private int GetDependencyScore(PackageName packageName)
		{
			int depth;
			lock (this.cacheLock)
			{
				if (!this.dependencyScoreCache.TryGetValue(packageName, out depth))
				{
					this.dependencyScoreCache[packageName] = 0;

					depth = 1;
					PackageInfo package = this.QueryPackageInfo(packageName);
					if (package != null)
					{
						if (package.IsDualityPackage)
						{
							foreach (PackageName dependency in package.Dependencies)
							{
								depth += this.GetDependencyScore(dependency);
							}
						}
						else
						{
							depth = 0;
						}
					}

					this.dependencyScoreCache[packageName] = depth;
				}
			}
			return depth;
		}
		private NuGet.IPackage FindPackageInfo(PackageName packageRef, bool findMaxVersionBelow)
		{
			// Find a direct version match
			if (packageRef.Version != null && !findMaxVersionBelow)
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
				if (findMaxVersionBelow && packageRef.Version != null)
					packageQuery = packageQuery.Where(p => p.Version.Version < packageRef.Version);

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
					result = this.repository.FindPackagesById(id).ToArray();
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
		private void LoadConfig()
		{
			// Reset to default data
			this.repositoryUrl = new Uri(DefaultRepositoryUrl);
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

				string repoUrlString = doc.Root.GetElementValue("RepositoryUrl") ?? DefaultRepositoryUrl;
				if (repoUrlString.Contains(Uri.SchemeDelimiter) && Uri.CheckSchemeName(repoUrlString.Split(new string[] { Uri.SchemeDelimiter }, StringSplitOptions.RemoveEmptyEntries)[0]))
				{
					this.repositoryUrl = new Uri(repoUrlString);
					this.repositoryOriginal = null;
				}
				else
				{
					this.repositoryUrl = new Uri("file:///" + Path.GetFullPath(Path.Combine(this.rootPath, repoUrlString)));
					this.repositoryOriginal = repoUrlString;
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
					new XElement("RepositoryUrl", string.IsNullOrEmpty(this.repositoryOriginal) ? this.repositoryUrl.ToString() : this.repositoryOriginal),
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
		private void AppendUpdateFileEntry(XDocument updateDoc, string copySource, string copyTarget)
		{
			foreach (XElement element in updateDoc.Root.Elements("Remove").ToArray())
			{
				if (element.Attribute("target") != null && PathHelper.ArePathsEqual(element.Attribute("target").Value, copyTarget))
				{
					element.Remove();
				}
			}
			updateDoc.Root.Add(new XElement("Update", 
				new XAttribute("source", copySource), 
				new XAttribute("target", copyTarget)));
		}
		private void AppendUpdateFileEntry(XDocument updateDoc, string deleteTarget)
		{
			foreach (XElement element in updateDoc.Root.Elements("Update").ToArray())
			{
				if (element.Attribute("target") != null && PathHelper.ArePathsEqual(element.Attribute("target").Value, deleteTarget))
				{
					element.Remove();
				}
			}
			updateDoc.Root.Add(new XElement("Remove", 
				new XAttribute("target", deleteTarget)));
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
			string binaryBaseDir = this.pluginTargetDir;
			string contentBaseDir = this.dataTargetDir;
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
			info.ProjectUrl		= package.ProjectUrl;
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
				this.AppendUpdateFileEntry(updateDoc, packageFile);
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
				this.AppendUpdateFileEntry(updateDoc, Path.Combine(e.InstallPath, pair.Value), pair.Key);
			}
			updateDoc.Save(this.UpdateFilePath);

			this.OnPackageInstalled(new PackageEventArgs(new PackageName(e.Package.Id, e.Package.Version.Version)));
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

		private struct PackageNamePair
		{
			public PackageName First;
			public PackageName Second;

			public override string ToString()
			{
				return string.Format("{0} => {1}", this.First, this.Second);
			}
		}
	}
}
