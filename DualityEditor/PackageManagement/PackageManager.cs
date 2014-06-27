using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;

using Duality;

using NuGet;

namespace Duality.Editor.PackageManagement
{
	public sealed class PackageManager
	{
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

		private NuGet.PackageManager		manager			= null;
		private	NuGet.IPackageRepository	repository		= null;


		public Uri RepositoryUrl
		{
			get { return this.repositoryUrl; }
		}
		public IEnumerable<LocalPackage> LocalPackages
		{
			get { return this.localPackages; }
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

			// Update local repository file mappings
			this.UpdateFileMappings();
		}

		public void InstallPackage(PackageInfo package)
		{
			// Update package entries from local config
			this.localPackages.RemoveAll(p => package.Id == p.Id);
			this.localPackages.Add(new LocalPackage(package.Id, package.Version));
			this.SaveConfig();

			// Request NuGet to install the package
			this.manager.InstallPackage(package.Id, new SemanticVersion(package.Version));
		}
		public void UninstallPackage(LocalPackage package)
		{
			this.manager.UninstallPackage(package.Id, new SemanticVersion(package.Version), false, true);
		}
		public void VerifyPackages()
		{
			Log.Editor.Write("Verifying packages...");
			Log.Editor.PushIndent();
			try
			{
				bool packageConfigModified = false;

				// Instruct NuGet to install all packages and see whether it does something
				foreach (LocalPackage package in this.localPackages)
				{
					// Determine the exact version that will be downloaded
					Version version = package.Version;
					bool explicitVersionRetrieved = false;
					if (version == null)
					{
						PackageInfo info = this.QueryPackageInfo(package.Id);
						if (info != null)
						{
							version = info.Version;
							explicitVersionRetrieved = (version != null);
						}
					}
					if (version == null)
					{
						Log.Editor.WriteError(
							"Can't resolve version of package '{0}'. There seems to be no compatible version available.",
							package.Id);
						continue;
					}

					// Install the package and prepare the update via event handlers
					this.manager.InstallPackage(package.Id, new SemanticVersion(version));

					// Update the package config version number, in case we've just retrieved an explicit version
					if (explicitVersionRetrieved)
					{
						package.Version = version;
						packageConfigModified = true;
					}
				}

				// If the package configuration was changed due to verification, make sure to update it
				if (packageConfigModified)
				{
					this.SaveConfig();
				}
			}
			catch (Exception e)
			{
				Log.Editor.WriteError(Log.Exception(e));
			}
			Log.Editor.PopIndent();
		}
		public bool ApplyUpdate(bool restartEditor = true)
		{
			if (!File.Exists(this.UpdateFilePath)) return false;
			
			Process.Start("DualityUpdater.exe", string.Format("{0} {1} {2}",
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
			    p.Tags.Contains("Duality") && 
			    p.Tags.Contains("Plugin"));

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
		public PackageInfo QueryPackageInfo(string packageId, Version packageVersion = null)
		{
			// Query all matching packages
			IEnumerable<NuGet.IPackage> query = this.repository.FindPackagesById(packageId);
			NuGet.IPackage[] data = query.ToArray();

			// Find a direct version match
			if (packageVersion != null)
			{
				foreach (NuGet.IPackage package in data)
				{
					if (package.Version.Version == packageVersion)
						return this.CreatePackageInfo(package);
				}
			}
			// Find the newest available version
			else
			{
				NuGet.IPackage newestPackage = data
					.Where(p => p.IsListed() && p.IsReleaseVersion() && p.IsLatestVersion)
					.OrderByDescending(p => p.Version.Version)
					.FirstOrDefault();

				if (newestPackage != null)
					return this.CreatePackageInfo(newestPackage);
			}

			// Nothing was found
			return null;
		}
		public IEnumerable<LocalPackage> QueryReferencedPackages(string filePath)
		{
			return this.localPackages.Where(p => p.Files.Any(path => PathHelper.ArePathsEqual(filePath, path)));
		}

		private void UpdateFileMappings()
		{
			foreach (LocalPackage package in this.localPackages)
			{
				if (package.Version == null) continue;

				NuGet.IPackage localNuGet = this.manager.LocalRepository.FindPackage(package.Id, new SemanticVersion(package.Version));
				if (localNuGet != null)
				{
					package.Files = this.CreateFileMapping(localNuGet).Select(p => p.Key );
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
						string versionString = packageElement.GetAttributeValue("version");
						Version packageVersion = (versionString != null ? Version.Parse(versionString) : null);

						// Create Package entry instance
						LocalPackage package = new LocalPackage(
							packageElement.GetAttributeValue("id"),
							packageVersion);

						this.localPackages.Add(package);
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
			updateDoc.Root.Add(new XElement("Update", 
				new XAttribute("source", copySource), 
				new XAttribute("target", copyTarget)));
		}
		private void AppendUpdateFileEntry(XDocument updateDoc, string deleteTarget)
		{
			updateDoc.Root.Add(new XElement("Remove", 
				new XAttribute("target", deleteTarget)));
		}

		private Dictionary<string,string> CreateFileMapping(NuGet.IPackage package)
		{
			Dictionary<string,string> fileMapping = new Dictionary<string,string>();

			bool isDualityPackage = 
				package.Tags != null &&
				package.Tags.Contains("Duality");
			bool isPluginPackage = 
				isDualityPackage && 
				package.Tags.Contains("Plugin");
			string binaryBaseDir = this.pluginTargetDir;
			string contentBaseDir = this.dataTargetDir;
			if (!isPluginPackage && isDualityPackage) binaryBaseDir = "";

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
			PackageInfo info = new PackageInfo(package.Id, package.Version.Version);

			info.Title			= package.Title;
			info.Summary		= package.Summary;
			info.Description	= package.Description;
			info.ProjectUrl		= package.ProjectUrl;
			info.IconUrl		= package.IconUrl;
			info.DownloadCount	= package.DownloadCount;
			info.PublishDate	= package.Published.HasValue ? package.Published.Value.DateTime : DateTime.MinValue;
			info.Authors		= package.Authors;
			info.Tags			= package.Tags != null ? package.Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : Enumerable.Empty<string>();

			return info;
		}

		private void manager_PackageUninstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Package removal scheduled: {0}, {1}", e.Package.Id, e.Package.Version);

			// Determine all files that are referenced by a package, and the ones referenced by this one
			LocalPackage localPackage = this.localPackages.FirstOrDefault(p => p.Id == e.Package.Id);
			string[] referencedFiles = this.localPackages
				.Where(p => p != localPackage)
				.SelectMany(p => p.Files)
				.Distinct()
				.ToArray();
			IEnumerable<string> localFiles = (localPackage != null) ? 
				localPackage.Files : 
				this.CreateFileMapping(e.Package).Select(p => p.Key);
			
			// If it's some unknown dependency, don't yet remove any files belonging to other local repositories with the same id
			if (localPackage == null && this.manager.LocalRepository.GetPackages().Any(p => p.Id == e.Package.Id))
				return;

			// Schedule files for removal
			XDocument updateDoc = this.PrepareUpdateFile();
			foreach (var packageFile in localFiles)
			{
				// Don't remove any file that is still referenced by a local package
				if (referencedFiles.Any(path => PathHelper.ArePathsEqual(packageFile, path)))
					continue;

				// Append the scheduled operation to the updater config file.
				this.AppendUpdateFileEntry(updateDoc, packageFile);
			}
			updateDoc.Save(this.UpdateFilePath);

			// Update local package configuration file
			this.localPackages.RemoveAll(p => p.Id == e.Package.Id);
			this.SaveConfig();
		}
		private void manager_PackageInstalled(object sender, PackageOperationEventArgs e)
		{
			Log.Editor.Write("Package downloaded: {0}, {1}", e.Package.Id, e.Package.Version);

			// Schedule files for updating / copying
			XDocument updateDoc = this.PrepareUpdateFile();
			Dictionary<string,string> fileMapping = this.CreateFileMapping(e.Package);
			foreach (var pair in fileMapping)
			{
				// Don't overwrite files from a newer version of this package with their old one (OpenTK, Duality, etc.)
				bool isOldVersion = false;
				LocalPackage[] referencedPackages = this.QueryReferencedPackages(pair.Key).ToArray();
				foreach (LocalPackage otherPackage in referencedPackages)
				{
					if (otherPackage.Id == e.Package.Id && otherPackage.Version > e.Package.Version.Version)
					{
						isOldVersion = true;
						break;
					}
				}
				if (isOldVersion) continue;

				// Append the scheduled operation to the updater config file.
				this.AppendUpdateFileEntry(updateDoc, Path.Combine(e.InstallPath, pair.Value), pair.Key);
			}
			updateDoc.Save(this.UpdateFilePath);
		}
	}
}
