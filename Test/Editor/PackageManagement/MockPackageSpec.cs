using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Versioning;

using NuGet.Packaging;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace Duality.Editor.PackageManagement.Tests
{
	/// <summary>
	/// Specifies a NuGet package for mocking purposes during testing and provides
	/// functionality to build packages from those specs, as well as information on
	/// the expected outputs after an install via the <see cref="PackageManager"/>.
	/// </summary>
	public class MockPackageSpec
	{
		private PackageName name = new PackageName("Unknown", new Version(0, 0, 0, 0));
		private List<string> tags = new List<string>();
		private Dictionary<string, List<PackageName>> dependencySets = new Dictionary<string, List<PackageName>>();
		private List<KeyValuePair<string, string>> files = new List<KeyValuePair<string, string>>();
		private Dictionary<string, string> localMapping = new Dictionary<string, string>();


		/// <summary>
		/// [GET / SET] The packages ID and version.
		/// </summary>
		public PackageName Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		/// <summary>
		/// [GET] A list of keywords that this package will be tagged with.
		/// </summary>
		public List<string> Tags
		{
			get { return this.tags; }
		}
		/// <summary>
		/// [GET] A list of packages that this package depends on.
		/// </summary>
		public Dictionary<string, List<PackageName>> DependencySets
		{
			get { return this.dependencySets; }
		}
		/// <summary>
		/// [GET] A mapping of files from source files in the package build directory to
		/// their target inside the NuGet package. Note that the source files will be
		/// created automatically when building the mock package.
		/// </summary>
		public List<KeyValuePair<string, string>> Files
		{
			get { return this.files; }
		}
		/// <summary>
		/// [GET] The expected mapping of files from the local package repository path to the
		/// local root path. This information is used by tests for asserting correct install,
		/// update and uninstall operations and needs to be specified manually.
		/// </summary>
		public Dictionary<string, string> LocalMapping
		{
			get { return this.localMapping; }
		}


		public MockPackageSpec(string id) : this(id, new Version(1, 0, 0, 0)) { }
		public MockPackageSpec(string id, Version version)
		{
			this.name = new PackageName(id, version);
		}

		public void AddFile(string sourcePath, string packagePath)
		{
			this.files.Add(new KeyValuePair<string, string>(sourcePath, packagePath));
		}
		public List<PackageName> AddTarget(string targetFramework)
		{
			List<PackageName> dependencySet;
			if (!this.dependencySets.TryGetValue(targetFramework, out dependencySet))
			{
				dependencySet = new List<PackageName>();
				this.dependencySets.Add(targetFramework, dependencySet);
			}

			return dependencySet;
		}
		public void AddDependency(string targetFramework, PackageName package)
		{
			List<PackageName> dependencySet = this.AddTarget(targetFramework);
			dependencySet.Add(package);
		}
		public void AddDependency(PackageName package)
		{
			this.AddDependency("net45", package);
		}

		/// <summary>
		/// Creates a NuGet package from this mock package spec and copies it into the
		/// specified repository path.
		/// </summary>
		/// <param name="buildPath"></param>
		/// <param name="repositoryPath"></param>
		public void CreatePackage(string buildPath, string repositoryPath)
		{
			PackageBuilder builder = new PackageBuilder();
			ManifestMetadata metadata = new ManifestMetadata
			{
				Authors = new[] {"AdamsLair"},
				Version = new NuGetVersion(this.name.Version.ToString()),
				Id = this.name.Id,
				Description = string.Format("Mock Package: {0} {1}", this.name.Id, this.name.Version),
				Tags = string.Join(" ", this.tags),
				DependencyGroups = this.dependencySets.Select(pair => new PackageDependencyGroup(
					NuGetFramework.Parse(pair.Key),
					pair.Value.Select(item =>
							new PackageDependency(item.Id, new VersionRange(new NuGetVersion(item.Version.ToString()))))
						.ToList()))
			};

			// Set up file contents metadata for the package
			List<ManifestFile> fileMetadata = new List<ManifestFile>();
			foreach (var pair in this.files)
			{
				fileMetadata.Add(new ManifestFile { Source = pair.Key, Target = pair.Value });
				this.CreateFile(buildPath, pair.Key);
			}

			// If we don't have files or dependencies, at least at one mock file so we
			// can create a package at all. This is useful for test cases where we're
			// not actually interested in package contents at all.
			if (this.files.Count == 0 && !this.dependencySets.SelectMany(pair => pair.Value).Any())
			{
				fileMetadata.Add(new ManifestFile { Source = "Empty.dll", Target = "lib" });
				this.CreateFile(buildPath, "Empty.dll");
			}

			builder.PopulateFiles(buildPath, fileMetadata);
			builder.Populate(metadata);

			string packageFileName = Path.Combine(
				repositoryPath,
				string.Format("{0}.{1}.nupkg", this.name.Id, this.name.Version));
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

		public override string ToString()
		{
			return this.name.ToString();
		}

		/// <summary>
		/// Creates a package spec with a mock Assembly file that is not tagged as a Duality-related package.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		/// <param name="targetFramework"></param>
		/// <returns></returns>
		public static MockPackageSpec CreateLibrary(string id, Version version = null, string targetFramework = null)
		{
			MockPackageSpec package = new MockPackageSpec(id, version ?? new Version(1, 0, 0, 0));
			package.AddFile(
				string.Format("{0}.dll", id),
				targetFramework != null ?
					string.Format("lib\\{0}", targetFramework) :
					"lib");
			package.LocalMapping.Add(
				targetFramework != null ?
					string.Format("lib\\{0}\\{1}.dll", targetFramework, id) :
					string.Format("lib\\{0}.dll", id),
				string.Format("{0}.dll", id));
			return package;
		}
		/// <summary>
		/// Creates a package spec with a mock Assembly file, tagged as a Duality plugin.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		/// <param name="targetFrameworks"></param>
		/// <returns></returns>
		public static MockPackageSpec CreateDualityPlugin(string id, string[] targetFrameworks, Version version = null)
		{
			MockPackageSpec package = new MockPackageSpec(id, version ?? new Version(1, 0, 0, 0));
			package.Tags.Add(PackageManager.DualityTag);
			package.Tags.Add(PackageManager.PluginTag);
			foreach (string folder in targetFrameworks)
			{
				package.AddFile(
					string.Format("{0}.dll", id),
					folder != null ?
						string.Format("lib\\{0}", folder) :
						"lib");
				package.LocalMapping.Add(
					folder != null ?
						string.Format("lib\\{0}\\{1}.dll", folder, id) :
						string.Format("lib\\{0}.dll", id),
					string.Format("Plugins\\{0}.dll", id));
			}

			return package;
		}
		/// <summary>
		/// Creates a package spec with a mock Assembly file, tagged as a Duality plugin.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		/// <param name="targetFramework"></param>
		/// <returns></returns>
		public static MockPackageSpec CreateDualityPlugin(string id, Version version = null, string targetFramework = null)
		{
			MockPackageSpec package = new MockPackageSpec(id, version ?? new Version(1, 0, 0, 0));
			package.Tags.Add(PackageManager.DualityTag);
			package.Tags.Add(PackageManager.PluginTag);
			package.AddFile(
				string.Format("{0}.dll", id),
				targetFramework != null ?
					string.Format("lib\\{0}", targetFramework) :
					"lib");
			package.LocalMapping.Add(
				targetFramework != null ?
					string.Format("lib\\{0}\\{1}.dll", targetFramework, id) :
					string.Format("lib\\{0}.dll", id),
				string.Format("Plugins\\{0}.dll", id));
			return package;
		}
		/// <summary>
		/// Creates a package spec with a mock Assembly file, tagged as a Duality core part or non-plugin package.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		/// <param name="targetFramework"></param>
		/// <returns></returns>
		public static MockPackageSpec CreateDualityCorePart(string id, Version version = null, string targetFramework = null)
		{
			MockPackageSpec package = new MockPackageSpec(id, version ?? new Version(1, 0, 0, 0));
			package.Tags.Add(PackageManager.DualityTag);
			package.AddFile(
				string.Format("{0}.dll", id),
				targetFramework != null ?
					string.Format("lib\\{0}", targetFramework) :
					"lib");
			package.LocalMapping.Add(
				targetFramework != null ?
					string.Format("lib\\{0}\\{1}.dll", targetFramework, id) :
					string.Format("lib\\{0}.dll", id),
				string.Format("{0}.dll", id));
			return package;
		}
	}
}
