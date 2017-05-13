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
	public class PackageSetup
	{
		private const string DefaultRepositoryUrl = @"https://packages.nuget.org/api/v2";

		private List<string>       repositoryUrls = new List<string>{ DefaultRepositoryUrl };
		private bool               firstInstall   = false;
		private List<LocalPackage> localPackages  = new List<LocalPackage>();
		

		public List<string> RepositoryUrls
		{
			get { return this.repositoryUrls; }
		}
		public bool IsFirstInstall
		{
			get { return this.firstInstall; }
			set { this.firstInstall = value; }
		}
		public List<LocalPackage> LocalPackages
		{
			get { return this.localPackages; }
		}


		public bool HasPackage(PackageName package)
		{
			foreach (LocalPackage localPackage in this.localPackages)
			{
				if (localPackage.Id == package.Id)
				{
					 if (localPackage.Version == package.Version)
						 return true;
					 else if (package.Version == null)
						 return true;
				}
			}
			return false;
		}

		public void Load(string configFilePath)
		{
			try
			{
				XDocument doc = XDocument.Load(configFilePath);

				// Parse primitive values
				doc.Root.TryGetElementValue("FirstDualityInstall", ref this.firstInstall);

				// Parse repository URLs
				IEnumerable<XElement> repoUrlElements = doc.Root.Elements("RepositoryUrl");
				if (repoUrlElements.Any())
				{
					this.repositoryUrls.Clear();
					this.repositoryUrls.AddRange(repoUrlElements.Select(x => x.Value));
					if (this.repositoryUrls.Count == 0)
					{
						this.repositoryUrls.Add(DefaultRepositoryUrl);
					}
				}

				// Parse package list
				XElement packagesElement = doc.Root.Element("Packages");
				if (packagesElement != null)
				{
					this.localPackages.Clear();
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
		public void Save(string configFilePath)
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
			doc.Save(configFilePath);
		}
	}
}
