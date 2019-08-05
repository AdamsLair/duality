using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;

namespace Duality.Editor.PackageManagement
{
	public class PackageInfo
	{
		private PackageName       name           = PackageName.None;
		private string            title          = null;
		private string            summary        = null;
		private string            description    = null;
		private string            releaseNotes   = null;
		private bool              requireLicense = false;
		private Uri               projectUrl     = null;
		private Uri               licenseUrl     = null;
		private Uri               iconUrl        = null;
		private long               downloadCount  = 0;
		private DateTime          publishDate    = DateTime.MinValue;
		private List<string>      authors        = new List<string>();
		private List<string>      tags           = new List<string>();
		private List<PackageName> dependencies   = new List<PackageName>();


		public bool IsDualityPackage
		{
			get { return this.tags.Contains(PackageManager.DualityTag); }
		}
		public bool IsSamplePackage
		{
			get { return this.tags.Contains(PackageManager.SampleTag); }
		}
		public bool IsEditorPackage
		{
			get { return this.tags.Contains(PackageManager.EditorTag); }
		}
		public bool IsCorePackage
		{
			get { return this.tags.Contains(PackageManager.CoreTag); }
		}
		public PackageName Name
		{
			get { return this.name; }
		}
		public string Id
		{
			get { return this.name.Id; }
		}
		public Version Version
		{
			get { return this.name.Version; }
		}
		public string Title
		{
			get { return this.title; }
			internal set { this.title = value; }
		}
		public string Summary
		{
			get { return this.summary; }
			internal set { this.summary = value; }
		}
		public string Description
		{
			get { return this.description; }
			internal set { this.description = value; }
		}
		public string ReleaseNotes
		{
			get { return this.releaseNotes; }
			internal set { this.releaseNotes = value; }
		}
		public bool RequireLicenseAcceptance
		{
			get { return this.requireLicense; }
			internal set { this.requireLicense = value; }
		}
		public Uri ProjectUrl
		{
			get { return this.projectUrl; }
			internal set { this.projectUrl = value; }
		}
		public Uri LicenseUrl
		{
			get { return this.licenseUrl; }
			internal set { this.licenseUrl = value; }
		}
		public Uri IconUrl
		{
			get { return this.iconUrl; }
			internal set { this.iconUrl = value; }
		}
		public long DownloadCount
		{
			get { return this.downloadCount; }
			internal set { this.downloadCount = value; }
		}
		public DateTime PublishDate
		{
			get { return this.publishDate; }
			internal set { this.publishDate = value; }
		}
		public IReadOnlyList<string> Authors
		{
			get { return this.authors; }
			internal set
			{
				this.authors.Clear();
				if (value != null) this.authors.AddRange(value);
			}
		}
		public IReadOnlyList<string> Tags
		{
			get { return this.tags; }
			internal set
			{
				this.tags.Clear();
				if (value != null) this.tags.AddRange(value);
			}
		}
		public IReadOnlyList<PackageName> Dependencies
		{
			get { return this.dependencies; }
			internal set
			{
				this.dependencies.Clear();
				if (value != null) this.dependencies.AddRange(value);
			}
		}


		internal PackageInfo(PackageName package)
		{
			this.name = package;
		}
		internal PackageInfo(IPackageSearchMetadata nuGetPackage)
		{
			// Retrieve package data
			this.name           = new PackageName(nuGetPackage.Identity.Id, nuGetPackage.Identity.Version.Version);
			this.title          = nuGetPackage.Title;
			this.summary        = nuGetPackage.Summary;
			this.description    = nuGetPackage.Description;
			this.releaseNotes   = string.Empty;//nuGetPackage.ReleaseNotes;
			this.requireLicense = nuGetPackage.RequireLicenseAcceptance;
			this.projectUrl     = nuGetPackage.ProjectUrl;
			this.licenseUrl     = nuGetPackage.LicenseUrl;
			this.iconUrl        = nuGetPackage.IconUrl;
			this.downloadCount  = nuGetPackage.DownloadCount ?? 0;
			this.publishDate    = nuGetPackage.Published.HasValue ? nuGetPackage.Published.Value.DateTime : DateTime.MinValue;
			
			if (nuGetPackage.Authors != null)
				this.authors.Add(nuGetPackage.Authors);
			if (nuGetPackage.Tags != null)
				this.tags.AddRange(nuGetPackage.Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

			// Retrieve the matching set of dependencies.
			IEnumerable<PackageDependencyGroup> dependencySets = nuGetPackage.DependencySets;
			if (dependencySets != null)
			{
				var availableTargetFrameworks = dependencySets.Select(item => item.TargetFramework).ToList();
				var matchingTargetFramework = PackageManager.SelectBestFrameworkMatch(availableTargetFrameworks);

				var matchingDependencySet = dependencySets.FirstOrDefault(item => item.TargetFramework == matchingTargetFramework);
				if (matchingDependencySet != null)
				{
					foreach (var dependency in matchingDependencySet.Packages)
					{
						if (dependency.VersionRange != null && dependency.VersionRange.MinVersion != null)
							this.dependencies.Add(new PackageName(dependency.Id, dependency.VersionRange.MinVersion.Version));
						else
							this.dependencies.Add(new PackageName(dependency.Id, null));
					}
				}
			}
		}

		public override string ToString()
		{
			return string.Format("Package Info '{0}'", this.name);
		}
	}
}
