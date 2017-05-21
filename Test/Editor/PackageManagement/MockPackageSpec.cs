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
		private Dictionary<string,string> files = new Dictionary<string,string>();


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
		public Dictionary<string,string> Files
		{
			get { return this.files; }
		}


		public MockPackageSpec(string id) : this(id, new Version(1, 0, 0, 0)) { }
		public MockPackageSpec(string id, Version version)
		{
			this.name = new PackageName(id, version);
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

			List<NuGet.ManifestFile> fileMetadata = new List<NuGet.ManifestFile>();
			foreach (var pair in this.files)
			{
				fileMetadata.Add(new NuGet.ManifestFile { Source = pair.Key, Target = pair.Value });
				this.CreateFile(buildPath, pair.Key);
			}

			// If we don't have files or dependencies, at least at one mock file so we
			// can create a package at all.
			if (this.files.Count == 0 && this.dependencies.Count == 0)
			{
				fileMetadata.Add(new NuGet.ManifestFile { Source = "Empty.dll", Target = "lib" });
				this.CreateFile(buildPath, "Empty.dll");
			}

			builder.PopulateFiles(buildPath, fileMetadata);
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
