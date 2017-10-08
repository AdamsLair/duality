using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		private int               downloadCount  = 0;
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
		public int DownloadCount
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
		internal PackageInfo(NuGet.IPackage nuGetPackage)
		{
			// Retrieve package data
			this.name           = new PackageName(nuGetPackage.Id, nuGetPackage.Version.Version);
			this.title          = nuGetPackage.Title;
			this.summary        = nuGetPackage.Summary;
			this.description    = nuGetPackage.Description;
			this.releaseNotes   = nuGetPackage.ReleaseNotes;
			this.requireLicense = nuGetPackage.RequireLicenseAcceptance;
			this.projectUrl     = nuGetPackage.ProjectUrl;
			this.licenseUrl     = nuGetPackage.LicenseUrl;
			this.iconUrl        = nuGetPackage.IconUrl;
			this.downloadCount  = nuGetPackage.DownloadCount;
			this.publishDate    = nuGetPackage.Published.HasValue ? nuGetPackage.Published.Value.DateTime : DateTime.MinValue;
			
			if (nuGetPackage.Authors != null)
				this.authors.AddRange(nuGetPackage.Authors);
			if (nuGetPackage.Tags != null)
				this.tags.AddRange(nuGetPackage.Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

			// Retrieve the matching set of dependencies. For now, don't support different sets and just pick the first one.
			var matchingDependencySet = nuGetPackage.DependencySets.FirstOrDefault();
			if (matchingDependencySet != null)
			{
				foreach (NuGet.PackageDependency dependency in matchingDependencySet.Dependencies)
				{
					if (dependency.VersionSpec != null && dependency.VersionSpec.MinVersion != null)
						this.dependencies.Add(new PackageName(dependency.Id, dependency.VersionSpec.MinVersion.Version));
					else
						this.dependencies.Add(new PackageName(dependency.Id, null));
				}
			}
		}

		public override string ToString()
		{
			return string.Format("Package Info '{0}'", this.name);
		}
	}
}
