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
			if (Directory.Exists(TestBasePath))
				Directory.Delete(TestBasePath, true);

			Directory.CreateDirectory(TestClientPath);
			Directory.CreateDirectory(TestRepositoryPath);
			
			string relativeRepositoryPath = PathHelper.MakeDirectoryPathRelative(
				TestRepositoryPath, 
				TestClientPath);

			this.workEnv = new PackageManagerEnvironment(TestClientPath);
			this.setup = new PackageSetup();
			this.setup.RepositoryUrls.Clear();
			this.setup.RepositoryUrls.Add(relativeRepositoryPath);
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
			MockPackageSpec packageSpec = new MockPackageSpec("AdamsLair.Duality.Test", new Version(1, 2, 3, 4));
			packageSpec.CreatePackage(TestPackageBuildPath, TestRepositoryPath);

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			PackageInfo info = packageManager.QueryPackageInfo(packageSpec.Name);

			Assert.IsNotNull(info);
			Assert.AreEqual(packageSpec.Name.Id, info.Id);
			Assert.AreEqual(packageSpec.Name.Version, info.Version);
			Assert.AreEqual(packageSpec.Name, info.PackageName);
		}
		[Test] public void QueryAvailablePackages()
		{
			MockPackageSpec packageSpecNonDuality = new MockPackageSpec("Some.Other.Package");
			MockPackageSpec packageSpecPlugin = new MockPackageSpec("AdamsLair.Duality.TestPlugin", new Version(1, 0, 0, 0));
			MockPackageSpec packageSpecPluginLatest = new MockPackageSpec("AdamsLair.Duality.TestPlugin", new Version(1, 1, 0, 0));
			MockPackageSpec packageSpecSample = new MockPackageSpec("AdamsLair.Duality.TestSample");

			packageSpecPlugin.Tags.Add(PackageManager.DualityTag);
			packageSpecPlugin.Tags.Add(PackageManager.PluginTag);
			packageSpecPluginLatest.Tags.Add(PackageManager.DualityTag);
			packageSpecPluginLatest.Tags.Add(PackageManager.PluginTag);
			packageSpecSample.Tags.Add(PackageManager.DualityTag);
			packageSpecSample.Tags.Add(PackageManager.SampleTag);

			packageSpecNonDuality.CreatePackage(TestPackageBuildPath, TestRepositoryPath);
			packageSpecPlugin.CreatePackage(TestPackageBuildPath, TestRepositoryPath);
			packageSpecPluginLatest.CreatePackage(TestPackageBuildPath, TestRepositoryPath);
			packageSpecSample.CreatePackage(TestPackageBuildPath, TestRepositoryPath);

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			List<PackageInfo> packages = packageManager.QueryAvailablePackages().ToList();

			// We expect that only Duality packages are reported, and only the latest version of each.
			Assert.IsNotNull(packages);
			Assert.AreEqual(2, packages.Count);

			PackageInfo packagePluginInfo = packages.FirstOrDefault(item => item.Id == packageSpecPlugin.Name.Id);
			PackageInfo packageSampleInfo = packages.FirstOrDefault(item => item.Id == packageSpecSample.Name.Id);

			Assert.IsNotNull(packagePluginInfo);
			Assert.IsNotNull(packageSampleInfo);
			Assert.AreEqual(packageSpecPluginLatest.Name.Version, packagePluginInfo.Version);
		}
		

		[Test, TestCaseSource("InstallPackageTestCases")]
		public void InstallPackage(PackageOperationTestCase testCase)
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(testCase.Repository);
			this.SetupPackagesForTest(packageManager, testCase.Setup);

			using (PackageEventListener listener = new PackageEventListener(packageManager))
			{
				// Find and install the package to test
				PackageInfo packageInfo = packageManager.QueryPackageInfo(testCase.Target.Name);
				packageManager.InstallPackage(packageInfo);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed, 
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}
		[Test, TestCaseSource("UninstallPackageTestCases")]
		public void UninstallPackage(PackageOperationTestCase testCase)
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(testCase.Repository);
			this.SetupPackagesForTest(packageManager, testCase.Setup);

			using (PackageEventListener listener = new PackageEventListener(packageManager))
			{
				// Find and install the package to test
				PackageInfo packageInfo = packageManager.QueryPackageInfo(testCase.Target.Name);
				packageManager.UninstallPackage(packageInfo);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed, 
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}
		[Test, TestCaseSource("UpdatePackageTestCases")]
		public void UpdatePackage(PackageOperationTestCase testCase)
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(testCase.Repository);
			this.SetupPackagesForTest(packageManager, testCase.Setup);

			using (PackageEventListener listener = new PackageEventListener(packageManager))
			{
				// Find and install the package to test
				PackageInfo packageInfo = packageManager.QueryPackageInfo(testCase.Target.Name);
				packageManager.UpdatePackage(packageInfo);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed, 
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}
		[Test, TestCaseSource("PackageRestoreTestCases")] 
		public void PackageRestore(PackageRestoreTestCase testCase)
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			this.SetupReporistoryForTest(testCase.Repository);
			this.SetupPackagesForTest(packageManager, testCase.PreSetup);

			packageManager.LocalSetup.Packages.Clear();
			packageManager.LocalSetup.Packages.AddRange(testCase.DesiredSetup.Select(name => new LocalPackage(name)));

			using (PackageEventListener listener = new PackageEventListener(packageManager))
			{
				// Uninstall packages that are installed but no longer present in the local setup config
				packageManager.UninstallNonRegisteredPackages();

				// Install, update or check packages that are in the local setup config
				List<LocalPackage> packagesToVerify = packageManager.LocalSetup.Packages.ToList();
				packageManager.OrderByDependencies(packagesToVerify);
				foreach (LocalPackage package in packagesToVerify)
				{
					packageManager.VerifyPackage(package);
				}

				listener.AssertChanges(
					testCase.Installed, 
					testCase.Uninstalled);
			}

			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}

		private IEnumerable<PackageOperationTestCase> InstallPackageTestCases()
		{
			// Note that NuGet by default does not recognize lib subfolders during installation.
			// The files will be packaged with subfolders, but their EffectivePath won't include
			// them - which is why, unless Duality addresses this at some point in the future, 
			// all plugins will end up in the Plugins root folder, regardless of their previous
			// hierarchy.

			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Duality plugin without any dependencies
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			dualityPluginA.Files.Add("Subfolder\\TestPluginA.Second.dll", "lib\\Subfolder");
			dualityPluginA.Files.Add("Data\\TestPluginA\\SomeRes.Pixmap.res", "content\\TestPluginA");
			dualityPluginA.Files.Add("Source\\Foo\\SomeCode.cs", "source\\Foo");
			dualityPluginA.LocalMapping.Add("lib\\Subfolder\\TestPluginA.Second.dll", "Plugins\\TestPluginA.Second.dll");
			dualityPluginA.LocalMapping.Add("content\\TestPluginA\\SomeRes.Pixmap.res", "Data\\TestPluginA\\SomeRes.Pixmap.res");
			dualityPluginA.LocalMapping.Add("source\\Foo\\SomeCode.cs", "Source\\Code\\AdamsLair.Duality.TestPluginA\\Foo\\SomeCode.cs");

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies", 
				dualityPluginA, 
				new [] { dualityPluginA }));

			// Duality plugin depending on another Duality plugin
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			dualityPluginB.Dependencies.Add(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies", 
				dualityPluginB, 
				new [] { dualityPluginB, dualityPluginA }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA");
			otherLibraryA.Files.Add("Data\\TestLibraryA\\SomeFile.txt", "content\\TestLibraryA");
			otherLibraryA.LocalMapping.Add("content\\TestLibraryA\\SomeFile.txt", "TestLibraryA\\SomeFile.txt");

			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC");
			dualityPluginC.Dependencies.Add(otherLibraryA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies", 
				dualityPluginC, 
				new [] { dualityPluginC, otherLibraryA }));
			
			// Duality package that is not a plugin
			MockPackageSpec dualityNonPluginA = MockPackageSpec.CreateDualityCorePart("AdamsLair.Duality.TestNonPluginA");
			dualityNonPluginA.Files.Add("Data\\TestNonPluginA\\SomeFile.txt", "content\\TestNonPluginA");
			dualityNonPluginA.LocalMapping.Add("content\\TestNonPluginA\\SomeFile.txt", "TestNonPluginA\\SomeFile.txt");

			cases.Add(new PackageOperationTestCase(
				"Duality Non-Plugin Package", 
				dualityNonPluginA, 
				new [] { dualityNonPluginA }));

			// Installing a package that was already installed
			cases.Add(new PackageOperationTestCase(
				"Package Already Installed", 
				new[] { dualityPluginA },
				dualityPluginA, 
				new[] { dualityPluginA }));

			// Installing a package where one of its dependencies was already installed
			cases.Add(new PackageOperationTestCase(
				"Package Dependency Already Installed", 
				new[] { dualityPluginA },
				dualityPluginB, 
				new[] { dualityPluginA, dualityPluginB }));

			// Installing a package where an old version of one of its dependencies is already installed
			MockPackageSpec dualityPluginA_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(0, 9, 0, 0));

			cases.Add(new PackageOperationTestCase(
				"Older Package Dependency Installed", 
				new[] { dualityPluginA_Old },
				dualityPluginB, 
				new[] { dualityPluginA, dualityPluginB }));

			// Installing a package where a newer version of one of its dependencies is already installed
			MockPackageSpec dualityPluginA_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(1, 1, 0, 0));

			cases.Add(new PackageOperationTestCase(
				"Newer Package Dependency Installed", 
				new[] { dualityPluginA_New },
				dualityPluginB, 
				new[] { dualityPluginA_New, dualityPluginB }));

			return cases;
		}
		private IEnumerable<PackageOperationTestCase> UninstallPackageTestCases()
		{
			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Duality plugin without any dependencies
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies", 
				new [] { dualityPluginA },
				dualityPluginA, 
				new MockPackageSpec[0]));

			// Duality plugin with Duality plugin dependencies
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			dualityPluginB.Dependencies.Add(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies", 
				new [] { dualityPluginA, dualityPluginB },
				dualityPluginB, 
				new [] { dualityPluginA }));
			
			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA");
			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC");
			dualityPluginC.Dependencies.Add(otherLibraryA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies", 
				new [] { dualityPluginC, otherLibraryA },
				dualityPluginC, 
				new MockPackageSpec[0]));

			// Duality plugin that has a dependency that other plugins still need
			MockPackageSpec dualityPluginD = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginD");
			dualityPluginD.Dependencies.Add(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Shared Dependencies", 
				new [] { dualityPluginA, dualityPluginB, dualityPluginD },
				dualityPluginD, 
				new [] { dualityPluginA, dualityPluginB }));

			return cases;
		}
		private IEnumerable<PackageOperationTestCase> UpdatePackageTestCases()
		{
			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Duality plugin without any dependencies
			MockPackageSpec dualityPluginA_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginA_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(1, 1, 0, 0));

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies", 
				new [] { dualityPluginA_Old },
				dualityPluginA_Old, 
				new [] { dualityPluginA_New }));

			// Duality plugin with Duality plugin dependencies
			MockPackageSpec dualityPluginB_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginB_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 1, 0, 0));
			dualityPluginB_Old.Dependencies.Add(dualityPluginA_Old.Name);
			dualityPluginB_New.Dependencies.Add(dualityPluginA_New.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies", 
				new [] { dualityPluginA_Old, dualityPluginB_Old },
				dualityPluginB_Old, 
				new [] { dualityPluginA_New, dualityPluginB_New }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA_Old = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA", new Version(1, 0, 0, 0));
			MockPackageSpec otherLibraryA_New = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA", new Version(1, 1, 0, 0));
			MockPackageSpec dualityPluginC_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginC_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 1, 0, 0));
			dualityPluginC_Old.Dependencies.Add(otherLibraryA_Old.Name);
			dualityPluginC_New.Dependencies.Add(otherLibraryA_New.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies", 
				new [] { otherLibraryA_Old, dualityPluginC_Old },
				dualityPluginC_Old, 
				new [] { otherLibraryA_New, dualityPluginC_New }));

			return cases;
		}
		private IEnumerable<PackageRestoreTestCase> PackageRestoreTestCases()
		{
			// Create a shared repository for all test cases to save some space and redundancy
			MockPackageSpec libraryA1 = MockPackageSpec.CreateLibrary("Some.Other.LibraryA", new Version(5, 0, 1, 0));
			MockPackageSpec libraryA2 = MockPackageSpec.CreateLibrary("Some.Other.LibraryA", new Version(6, 1, 0, 0));
			MockPackageSpec pluginA1 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(1, 0, 1, 0));
			MockPackageSpec pluginA2 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(2, 1, 0, 0));
			MockPackageSpec pluginB1 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 0, 2, 0));
			MockPackageSpec pluginB2 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(2, 2, 0, 0));
			MockPackageSpec pluginC1 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 1, 2, 0));
			MockPackageSpec pluginC2 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(2, 2, 1, 0));
			MockPackageSpec pluginD1 = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginD", new Version(1, 1, 3, 0));
			pluginA1.Dependencies.Add(libraryA1.Name);
			pluginA2.Dependencies.Add(libraryA2.Name);
			pluginB1.Dependencies.Add(pluginA1.Name);
			pluginB2.Dependencies.Add(pluginA2.Name);
			pluginD1.Dependencies.Add(pluginA1.Name);

			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(libraryA1);
			repository.Add(libraryA2);
			repository.Add(pluginA1);
			repository.Add(pluginA2);
			repository.Add(pluginB1);
			repository.Add(pluginB2);
			repository.Add(pluginC1);
			repository.Add(pluginC2);
			repository.Add(pluginD1);

			List<PackageRestoreTestCase> cases = new List<PackageRestoreTestCase>();

			// Full restore of all packages to the newest available versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name.VersionInvariant);
				configSetup.Add(pluginB1.Name.VersionInvariant);

				cases.Add(new PackageRestoreTestCase(
					"Full Restore, Newest",
 					repository,
					new MockPackageSpec[0],
					configSetup, 
					new [] { libraryA2, pluginA2, pluginB2 }));
			}

			// Full restore of all packages to specific package versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginB1.Name);

				cases.Add(new PackageRestoreTestCase(
					"Full Restore, Specific",
 					repository,
					new MockPackageSpec[0],
					configSetup, 
					new [] { libraryA1, pluginA1, pluginB1 }));
			}

			// Partial restore of packages to newest available versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginC1.Name.VersionInvariant);

				cases.Add(new PackageRestoreTestCase(
					"Partial Restore, Newest",
 					repository,
					new [] { libraryA1, pluginA1 },
					configSetup, 
					new [] { libraryA1, pluginA1, pluginC2 }));
			}

			// Partial restore of packages to specific package versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginB1.Name);

				cases.Add(new PackageRestoreTestCase(
					"Partial Restore, Specific",
 					repository,
					new [] { libraryA1, pluginA1 },
					configSetup, 
					new [] { libraryA1, pluginA1, pluginB1 }));
			}

			// Partial restore of packages to newest available versions implicitly
			// leading to an update of already installed packages
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginB1.Name.VersionInvariant);

				cases.Add(new PackageRestoreTestCase(
					"Partial Restore, Implicit Update",
 					repository,
					new [] { libraryA1, pluginA1 },
					configSetup, 
					new [] { libraryA2, pluginA2, pluginB2 }));
			}

			// Full uninstall of all packages because they've been removed from the config
			{
				List<PackageName> configSetup = new List<PackageName>();

				cases.Add(new PackageRestoreTestCase(
					"Full Uninstall",
 					repository,
					new [] { libraryA2, pluginA2, pluginB2 },
					configSetup, 
					new MockPackageSpec[0]));
			}

			// Uninstall of a leaf package
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA2.Name);

				cases.Add(new PackageRestoreTestCase(
					"Partial Uninstall, Leaf",
 					repository,
					new [] { libraryA2, pluginA2, pluginB2 },
					configSetup, 
					new [] { libraryA2, pluginA2 }));
			}

			// Uninstall of a package that others depend on
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginB2.Name);

				// Duality will first uninstall pluginA2, but then re-install it
				// while verifying pluginB2, so we expect both an install and an
				// uninstall for that package.
				cases.Add(new PackageRestoreTestCase(
					"Dependency Uninstall, Reinstall",
 					repository,
					new [] { libraryA2, pluginA2, pluginB2 },
					configSetup, 
					new [] { libraryA2, pluginA2, pluginB2 },
					new [] { libraryA2, pluginA2 },
					new [] { libraryA2, pluginA2 }));
			}

			// Uninstall of a package which has an older version that others depend on
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginD1.Name);

				// Duality will first uninstall pluginA2, but then re-install an
				// older version of it because pluginD1 requires that as a dependency.
				cases.Add(new PackageRestoreTestCase(
					"Dependency Uninstall, Revert to Old",
 					repository,
					new [] { libraryA2, pluginA2, pluginD1 },
					configSetup, 
					new [] { libraryA1, pluginA1, pluginD1 }));
			}

			return cases;
		}


		private void SetupReporistoryForTest(IEnumerable<MockPackageSpec> repository)
		{
			foreach (MockPackageSpec package in repository)
			{
				package.CreatePackage(TestPackageBuildPath, TestRepositoryPath);
			}
		}
		private void SetupPackagesForTest(PackageManager packageManager, IEnumerable<MockPackageSpec> setup)
		{
			try
			{
				// Install all required packages
				foreach (MockPackageSpec package in setup)
				{
					PackageInfo packageInfo = packageManager.QueryPackageInfo(package.Name);
					if (packageInfo == null || packageInfo.PackageName != package.Name)
					{
						Assert.Inconclusive(
							"Failed to create the required package setup for the test. Unable to retrieve package '{0}'", 
							package.Name);
					}

					packageManager.InstallPackage(packageInfo);
				}

				// Make sure all required packages are really there
				foreach (MockPackageSpec package in setup)
				{
					// Skip checking non-Duality packages, as they do not show up in
					// the local package setup and thus would always fail this check.
					bool isDualityPackage = package.Tags.Contains(PackageManager.DualityTag);
					if (!isDualityPackage) continue;

					LocalPackage localPackage = packageManager.LocalSetup.GetPackage(package.Name);
					if (localPackage == null)
					{
						Assert.Inconclusive(
							"Failed to create the required package setup for the test. Install failed for package '{0}'", 
							package.Name);
					}
				}

				// Apply all scheduled copy and delete operations immediately
				if (File.Exists(this.workEnv.UpdateFilePath))
				{
					PackageUpdateSchedule applyScript = PackageUpdateSchedule.Load(this.workEnv.UpdateFilePath);
					applyScript.ApplyChanges(applyScript.Items);

					// Get rid of the other scheduled updates
					File.Delete(this.workEnv.UpdateFilePath);
				}
			}
			catch (Exception e)
			{
				Assert.Inconclusive(
					"Failed to create the required package setup for the test because an exception occurred: {0}", 
					e);
			}
		}

		private void AssertLocalSetup(PackageSetup actualSetup, IEnumerable<MockPackageSpec> expectedSetup)
		{
			Assert.AreEqual(
				expectedSetup.Count(), 
				actualSetup.Packages.Count,
				"Number of registered Duality packages in local setup");
			CollectionAssert.AreEquivalent(
				expectedSetup.Select(p => p.Name), 
				actualSetup.Packages.Select(p => p.PackageName),
				"Registered Duality packages in local setup");
		}
		private void AssertUpdateSchedule(IEnumerable<MockPackageSpec> installed, IEnumerable<MockPackageSpec> uninstalled)
		{
			bool anyUpdateExpected = installed.Any() || uninstalled.Any();
			bool updateScheduleExists = File.Exists(this.workEnv.UpdateFilePath);
			
			// Assert that the existence of an update file reflects whether we expect an update
			if (!anyUpdateExpected)
			{
				Assert.IsFalse(updateScheduleExists);
				return;
			}
			else
			{
				Assert.IsTrue(updateScheduleExists);
			}

			// Load the update schedule to check its contents
			PackageUpdateSchedule applyScript = PackageUpdateSchedule.Load(this.workEnv.UpdateFilePath);
			List<XElement> updateItems = applyScript.Items.ToList();

			// Assert that every install has a matching copy for each of its files.
			HashSet<string> writtenFiles = new HashSet<string>();
			foreach (MockPackageSpec package in installed)
			{
				foreach (var pair in package.LocalMapping)
				{
					this.AssertUpdateScheduleCopy(
						updateItems, 
						package.Name, 
						pair.Key, 
						pair.Value);

					// Note that an install can supersede a previous uninstall by copying a 
					// file into the same location. Keep track of all written files to check this.
					bool uniqueCopy = writtenFiles.Add(pair.Value);

					// Assert that we don't copy multiple files to the same target location.
					// The package manager should take care of resolving this up front.
					Assert.IsTrue(uniqueCopy);
				}
			}

			// Assert that every uninstall has a matching delete for each of its files.
			foreach (MockPackageSpec package in uninstalled)
			{
				foreach (var pair in package.LocalMapping)
				{
					// If the file we expect to see deleted was overwritten instead, skip
					// the assert to account for update situations where an uninstall is
					// immediately followed by an install.
					if (writtenFiles.Contains(pair.Value))
						continue;

					this.AssertUpdateScheduleDelete(
						updateItems, 
						package.Name, 
						pair.Value);
				}
			}
		}
		private void AssertUpdateScheduleCopy(IEnumerable<XElement> items, PackageName package, string source, string target)
		{
			string sourceAbs = Path.Combine(this.workEnv.RepositoryPath, package.Id + "." + package.Version, source);
			string targetAbs = Path.Combine(this.workEnv.RootPath, target);

			foreach (XElement item in items)
			{
				if (item.Name != PackageUpdateSchedule.CopyItem) continue;
				if (!PathOp.ArePathsEqual(item.GetAttributeValue("source"), sourceAbs)) continue;

				string itemTarget = item.GetAttributeValue("target");
				Assert.IsTrue(
					PathOp.ArePathsEqual(itemTarget, targetAbs), 
					"Found copy instruction with the expected source, but the target differs." + Environment.NewLine +
					"  Matching source: '{0}'" + Environment.NewLine +
					"  Expected target: '{1}'" + Environment.NewLine +
					"  Actual target:   '{2}'",
					sourceAbs,
					targetAbs,
					itemTarget);
				return;
			}

			Assert.Fail(
				"Expected copy instruction, but found no matching item." + Environment.NewLine +
				"  {0} update schedule items" + Environment.NewLine +
				"  Expected source: '{1}'" + Environment.NewLine +
				"  Expected target: '{2}'",
				items.Count(),
				sourceAbs,
				targetAbs);
		}
		private void AssertUpdateScheduleDelete(IEnumerable<XElement> items, PackageName package, string target)
		{
			string targetAbs = Path.Combine(this.workEnv.RootPath, target);

			foreach (XElement item in items)
			{
				if (item.Name != PackageUpdateSchedule.DeleteItem) continue;
				if (PathOp.ArePathsEqual(item.GetAttributeValue("target"), targetAbs))
				{
					return;
				}
			}

			Assert.Fail(
				"Expected delete instruction, but found no matching item." + Environment.NewLine +
				"  {0} update schedule items" + Environment.NewLine +
				"  Expected target: '{1}'",
				items.Count(),
				targetAbs);
		}
	}
}
