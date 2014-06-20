using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Duality;

using NuGet;

namespace Duality.Editor.PackageManagement
{
	public sealed class PackageManager
	{
		private	const	string	PackageConfigFile		= "PackageConfig.xml";
		private	const	string	DefaultRepositoryUrl	= @"https://packages.nuget.org/api/v2";

		private	Uri						repositoryUrl	= null;
		private	string					dataTargetDir	= null;
		private	string					pluginTargetDir	= null;
		private	string					rootPath		= null;
		private	List<LocalPackage>		localPackages	= new List<LocalPackage>();

		private NuGet.PackageManager		manager		= null;
		private	NuGet.IPackageRepository	repository	= null;


		public Uri RepositoryUrl
		{
			get { return this.repositoryUrl; }
		}
		public IEnumerable<LocalPackage> LocalPackages
		{
			get { return this.localPackages; }
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
			this.manager = new NuGet.PackageManager(this.repository, "Packages");
			this.manager.PackageInstalled += this.manager_PackageInstalled;
			this.manager.PackageUninstalled += this.manager_PackageUninstalled;
		}

		public void InstallPackage(PackageInfo package)
		{
			this.manager.InstallPackage(package.Id, new SemanticVersion(package.Version));
		}
		public void UninstallPackage(LocalPackage package)
		{
			this.manager.UninstallPackage(package.Id, new SemanticVersion(package.Version), false, true);
		}
		public void VerifyPackages()
		{
			// Instruct NuGet to install all packages and see whether it does something
			foreach (LocalPackage package in this.localPackages)
			{
				this.manager.InstallPackage(package.Id, new SemanticVersion(package.Version));
			}

			// Apply all the updates that may have been downloaded
			this.ApplyUpdate();
		}
		public void ApplyUpdate()
		{
			// ToDo: Call on each editor startup
			// ToDo: Early-out, if no update is scheduled
			// ToDo: Shutdown Dualitor, launch an updater process, copy files, etc.
			// ToDo: Implement an updater process
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

		private void LoadConfig()
		{
			// Reset to default data
			this.repositoryUrl = new Uri(DefaultRepositoryUrl);
			this.localPackages.Clear();

			// Check whethere there is a config file to load
			string configFilePath = Path.Combine(this.rootPath, PackageConfigFile);
			if (!File.Exists(configFilePath)) return;

			// If there is, load data from the config file
			try
			{
				XDocument doc = XDocument.Load(configFilePath);

				this.repositoryUrl = new Uri(doc.Root.GetElementValue("RepositoryUrl") ?? DefaultRepositoryUrl);

				XElement packagesElement = doc.Root.Element("Packages");
				if (packagesElement != null)
				{
					foreach (XElement packageElement in packagesElement.Elements("Package"))
					{
						string versionString = packageElement.GetAttributeValue("version");
						LocalPackage package = new LocalPackage(
							packageElement.GetAttributeValue("id"),
							versionString != null ? Version.Parse(versionString) : null);

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
					new XElement("RepositoryUrl", this.repositoryUrl),
					new XElement("Packages", this.localPackages.Select(p => 
						new XElement("Package",
							new XAttribute("id", p.Id),
							new XAttribute("version", p.Version)
						)
					))
				));
			doc.Save(PackageConfigFile);
		}

		
		private void manager_PackageUninstalled(object sender, PackageOperationEventArgs e)
		{
			// ToDo: Collect (and directly save) update data
		}
		private void manager_PackageInstalled(object sender, PackageOperationEventArgs e)
		{
			// ToDo: Collect (and directly save) update data
		}
	}
}
