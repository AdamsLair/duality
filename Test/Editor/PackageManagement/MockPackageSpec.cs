using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Duality.IO;

using NUnit.Framework;

namespace Duality.Editor.PackageManagement.Tests
{
	public class MockPackageSpec
	{
		private PackageName name = new PackageName("Unknown", new Version(0, 0, 0, 0));
		private List<string> tags = new List<string>();
		private List<PackageName> dependencies = new List<PackageName>();

		public PackageName Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		public List<string> Tags
		{
			get { return this.tags; }
		}
		public List<PackageName> Dependencies
		{
			get { return this.dependencies; }
		}

		public MockPackageSpec(string id, Version version) : this(id, version, null, null) { }
		public MockPackageSpec(string id, Version version, IEnumerable<string> tags) : this(id, version, tags, null) { }
		public MockPackageSpec(string id, Version version, IEnumerable<string> tags, IEnumerable<PackageName> dependencies)
		{
			this.name = new PackageName(id, version);
			if (tags != null) this.tags = tags.ToList();
			if (dependencies != null) this.dependencies = dependencies.ToList();
		}

		public void CreatePackage(string buildPath, string repositoryPath)
		{
			NuGet.PackageBuilder builder = new NuGet.PackageBuilder();
			NuGet.ManifestMetadata metadata = new NuGet.ManifestMetadata
			{
				Authors = "AdamsLair",
				Version = this.name.Version.ToString(),
				Id = this.name.Id,
				Description = string.Format("Mock Package: {0} {1}", this.name.Id, this.name.Version),
				Tags = string.Join(" ", this.tags),
				DependencySets = new List<NuGet.ManifestDependencySet>
				{
					new NuGet.ManifestDependencySet
					{
						TargetFramework = "net45",
						Dependencies = this.dependencies
							.Select(item => new NuGet.ManifestDependency { Id = item.Id, Version = item.Version.ToString() })
							.ToList()
					}
				}
			};

			List<NuGet.ManifestFile> files = new List<NuGet.ManifestFile>();
			files.Add(new NuGet.ManifestFile { Source = "Foo.dll", Target = "lib" });
			files.Add(new NuGet.ManifestFile { Source = "Subfolder\\Bar.dll", Target = "lib\\Subfolder" });
			this.CreateFile(buildPath, "Foo.dll");
			this.CreateFile(buildPath, "Subfolder\\Bar.dll");

			builder.PopulateFiles(buildPath, files);
			builder.Populate(metadata);

			string packageFileName = Path.Combine(
				repositoryPath, 
				string.Format("{0}.{1}.nupkg", name.Id, name.Version));
			using (FileStream stream = File.Open(packageFileName, FileMode.Create))
			{
				builder.Save(stream);
			}
		}
		private void CreateFile(string buildPath, string pathOrName)
		{
			string filePath = Path.Combine(buildPath, pathOrName);
			string directory = Path.GetDirectoryName(filePath);
			Directory.CreateDirectory(directory);
			File.WriteAllText(filePath, string.Format(
				"{0} {1}: {2}",
				this.name.Id,
				this.name.Version,
				pathOrName));
		}
	}
}
