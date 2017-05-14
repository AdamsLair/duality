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
	/// Describes the package setup that is required by a Duality project, including 
	/// repository URLs, package Ids and versions. It is a data representation of what's
	/// usually stored in the package config file.
	/// </summary>
	public class PackageSetup
	{
		private const string DefaultRepositoryUrl = @"https://packages.nuget.org/api/v2";

		private List<string>       repositoryUrls = new List<string>{ DefaultRepositoryUrl };
		private bool               firstInstall   = false;
		private List<LocalPackage> localPackages  = new List<LocalPackage>();
		

		/// <summary>
		/// [GET] A list of URLs to remote package repositories that will be used to pull packages from.
		/// For convenience reasons, it is allowed to specify regular file paths instead of URLs when referring
		/// to machine local repositories.
		/// </summary>
		public List<string> RepositoryUrls
		{
			get { return this.repositoryUrls; }
		}
		/// <summary>
		/// [GET / SET] Whether the next startup of the Duality editor will consider restoring packages
		/// to be the very first installation. This affects how license agreement requirements are handled.
		/// </summary>
		public bool IsFirstInstall
		{
			get { return this.firstInstall; }
			set { this.firstInstall = value; }
		}
		/// <summary>
		/// [GET] A list of packages that are required by this project. If a package does not specify a
		/// specific version, the latest available version will be installed.
		/// </summary>
		public List<LocalPackage> Packages
		{
			get { return this.localPackages; }
		}

		
		/// <summary>
		/// Retrieves a local project package by its Id, ignoring version numbers.
		/// Returns null if no such package is referenced.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public LocalPackage GetPackage(string id)
		{
			return this.GetPackage(new PackageName(id));
		}
		/// <summary>
		/// Retrieves a local project package by its Id and version.
		/// Returns null if no such package is referenced.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public LocalPackage GetPackage(string id, Version version)
		{
			return this.GetPackage(new PackageName(id, version));
		}
		/// <summary>
		/// Retrieves a local project package by its name, optionally requiring a specific version number.
		/// Returns null if no such package is referenced.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public LocalPackage GetPackage(PackageName name)
		{
			foreach (LocalPackage localPackage in this.localPackages)
			{
				if (localPackage.Id == name.Id)
				{
					 if (localPackage.Version == name.Version)
						 return localPackage;
					 else if (name.Version == null)
						 return localPackage;
				}
			}
			return null;
		}

		/// <summary>
		/// Updates an existing <see cref="PackageSetup"/> using information from the
		/// config file at the specified path. Settings that are not specified in the
		/// config file will remain as they were.
		/// </summary>
		/// <param name="configFilePath"></param>
		public void Populate(string configFilePath)
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
		/// <summary>
		/// Saves the <see cref="PackageSetup"/> to a config file at the specified path.
		/// </summary>
		/// <param name="configFilePath"></param>
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

		/// <summary>
		/// Loads an existing <see cref="PackageSetup"/> from the config file at the
		/// specified path.
		/// </summary>
		/// <param name="configFilePath"></param>
		/// <returns></returns>
		public static PackageSetup Load(string configFilePath)
		{
			PackageSetup setup = new PackageSetup();
			setup.Populate(configFilePath);
			return setup;
		}
	}
}
