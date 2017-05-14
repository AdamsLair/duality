using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

namespace Duality.Editor.PackageManagement.Tests
{
	[TestFixture]
	public class PackageManagerTest
	{
		private const string TestBasePath = "PackageManagerTest";
		private const string TestClientPath = TestBasePath + "\\Client";
		private const string TestRepositoryPath = TestBasePath + "\\Repository";
		private const string TestPackageBuildPath = TestBasePath + "\\PackageBuild";

		private PackageManagerEnvironment workEnv;
		private PackageSetup setup;


		[SetUp] public void Init()
		{
			Directory.CreateDirectory(TestClientPath);
			Directory.CreateDirectory(TestRepositoryPath);

			this.workEnv = new PackageManagerEnvironment(TestClientPath);

			this.setup = new PackageSetup();
			this.setup.RepositoryUrls.Clear();
			this.setup.RepositoryUrls.Add(Path.GetFullPath(TestRepositoryPath));
		}
		[TearDown] public void Cleanup()
		{
			this.workEnv = null;

			Directory.Delete(TestBasePath, true);
		}


		[Test] public void EmptySetup()
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			Assert.IsFalse(packageManager.IsPackageSyncRequired);
			Assert.IsFalse(packageManager.LocalSetup.IsFirstInstall);
			Assert.IsEmpty(packageManager.LocalSetup.Packages);
			Assert.AreEqual(1, packageManager.LocalSetup.RepositoryUrls.Count);
		}
		[Test] public void NonExistentConfigFile()
		{
			PackageManager packageManager = new PackageManager(this.workEnv);

			Assert.IsFalse(packageManager.IsPackageSyncRequired);
			Assert.IsFalse(packageManager.LocalSetup.IsFirstInstall);
			Assert.IsEmpty(packageManager.LocalSetup.Packages);
			Assert.AreEqual(1, packageManager.LocalSetup.RepositoryUrls.Count);
		}
		[Test] public void QueryPackageInfo()
		{
			PackageName name = new PackageName("AdamsLair.Duality.Test", new Version(1, 2, 3, 4));
			this.CreateRemoteMockPackage(name);

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			PackageInfo info = packageManager.QueryPackageInfo(name);

			Assert.IsNotNull(info);
			Assert.AreEqual(name.Id, info.Id);
			Assert.AreEqual(name.Version, info.Version);
			Assert.AreEqual(name, info.PackageName);
		}
		[Test] public void QueryAvailablePackages()
		{
			PackageName packageNonDuality = new PackageName("Some.Other.Package", new Version(1, 0, 0, 0));
			PackageName packagePlugin = new PackageName("AdamsLair.Duality.TestPlugin", new Version(1, 0, 0, 0));
			PackageName packagePluginLatest = new PackageName("AdamsLair.Duality.TestPlugin", new Version(1, 1, 0, 0));
			PackageName packageSample = new PackageName("AdamsLair.Duality.TestSample", new Version(1, 1, 0, 0));

			this.CreateRemoteMockPackage(packageNonDuality);
			this.CreateRemoteMockPackage(packagePlugin, new[] { PackageManager.DualityTag, PackageManager.PluginTag });
			this.CreateRemoteMockPackage(packagePluginLatest, new[] { PackageManager.DualityTag, PackageManager.PluginTag });
			this.CreateRemoteMockPackage(packageSample, new[] { PackageManager.DualityTag, PackageManager.SampleTag });

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			List<PackageInfo> packages = packageManager.QueryAvailablePackages().ToList();

			// We expect that only Duality packages are reported, and only the latest version of each.
			Assert.IsNotNull(packages);
			Assert.AreEqual(2, packages.Count);

			PackageInfo packagePluginInfo = packages.FirstOrDefault(item => item.Id == packagePlugin.Id);
			PackageInfo packageSampleInfo = packages.FirstOrDefault(item => item.Id == packageSample.Id);

			Assert.IsNotNull(packagePluginInfo);
			Assert.IsNotNull(packageSampleInfo);
			Assert.AreEqual(packagePluginLatest.Version, packagePluginInfo.Version);
		}

		private void CreateRemoteMockPackage(PackageName name, IEnumerable<string> tags = null, IEnumerable<PackageName> dependencies = null)
		{
			dependencies = dependencies ?? Enumerable.Empty<PackageName>();
			tags = tags ?? Enumerable.Empty<string>();

			NuGet.PackageBuilder builder = new NuGet.PackageBuilder();
			NuGet.ManifestMetadata metadata = new NuGet.ManifestMetadata
			{
				Authors = "AdamsLair",
				Version = name.Version.ToString(),
				Id = name.Id,
				Description = string.Format("Mock Package: {0} {1}", name.Id, name.Version),
				Tags = string.Join(" ", tags),
				DependencySets = new List<NuGet.ManifestDependencySet>
				{
					new NuGet.ManifestDependencySet
					{
						TargetFramework = "net45",
						Dependencies = dependencies
							.Select(item => new NuGet.ManifestDependency { Id = item.Id, Version = item.Version.ToString() })
							.ToList()
					}
				}
			};

			List<NuGet.ManifestFile> files = new List<NuGet.ManifestFile>();
			files.Add(new NuGet.ManifestFile { Source = "Foo.dll", Target = "lib" });
			files.Add(new NuGet.ManifestFile { Source = "Subfolder/Bar.dll", Target = "lib/Subfolder" });
			this.CreateMockPackageFile("Foo.dll");
			this.CreateMockPackageFile("Subfolder/Bar.dll");

			builder.PopulateFiles(TestPackageBuildPath, files);
			builder.Populate(metadata);

			string packageFileName = Path.Combine(
				TestRepositoryPath, 
				string.Format("{0}.{1}.nupkg", name.Id, name.Version));
			using (FileStream stream = File.Open(packageFileName, FileMode.Create))
			{
				builder.Save(stream);
			}
		}
		private void CreateMockPackageFile(string pathOrName)
		{
			string filePath = Path.Combine(TestPackageBuildPath, pathOrName);
			string directory = Path.GetDirectoryName(filePath);
			Directory.CreateDirectory(directory);
			File.WriteAllText(filePath, "Mock Content");
		}
	}
}
