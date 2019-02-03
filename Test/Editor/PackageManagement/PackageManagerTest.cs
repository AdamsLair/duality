using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Xml;
using System.Xml.Linq;

using Duality.IO;

using NuGet;

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
		[Test] public void GetPackage()
		{
			MockPackageSpec packageSpec = new MockPackageSpec("AdamsLair.Duality.Test", new Version(1, 2, 3, 4));
			packageSpec.CreatePackage(TestPackageBuildPath, TestRepositoryPath);

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			PackageInfo info = packageManager.GetPackage(packageSpec.Name);

			Assert.IsNotNull(info);
			Assert.AreEqual(packageSpec.Name.Id, info.Id);
			Assert.AreEqual(packageSpec.Name.Version, info.Version);
			Assert.AreEqual(packageSpec.Name, info.Name);
		}
		[Test] public void GetLatestDualityPackages()
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
			List<PackageInfo> packages = packageManager.GetLatestDualityPackages().ToList();

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
				packageManager.InstallPackage(testCase.Target.Name);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed,
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync");
			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}
		[Test, TestCaseSource("InstallPackageWithFrameworkFolderTestCases")]
		public void InstallPackageWithFrameworkFolder(PackageOperationTestCase testCase)
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(testCase.Repository);
			this.SetupPackagesForTest(packageManager, testCase.Setup);

			using (PackageEventListener listener = new PackageEventListener(packageManager))
			{
				// Find and install the package to test
				packageManager.InstallPackage(testCase.Target.Name);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed,
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync");
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
				// Uninstall the package to test
				packageManager.UninstallPackage(testCase.Target.Name);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed,
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync");
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
				// Update the package to test
				packageManager.UpdatePackage(testCase.Target.Name);

				// Assert that the expected events were fired
				listener.AssertChanges(
					testCase.Installed,
					testCase.Uninstalled);
			}

			// Assert client state / setup after the install was done
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync");
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

			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync");
			this.AssertLocalSetup(packageManager.LocalSetup, testCase.DualityResults);
			this.AssertUpdateSchedule(testCase.Installed, testCase.Uninstalled);
		}

		private IEnumerable<PackageOperationTestCase> InstallPackageTestCases()
		{
			// Note that NuGet treats the very first lib subfolder as a target framework specifier,
			// so we can't naively use a folder structure in the lib package directory. Otherwise,
			// it will be treated as an unsupported / unknown target framework.

			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Duality plugin without any dependencies
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			dualityPluginA.AddFile("Subfolder\\TestPluginA.Second.dll", "lib");
			dualityPluginA.AddFile("Data\\TestPluginA\\SomeRes.Pixmap.res", "content\\TestPluginA");
			dualityPluginA.AddFile("Source\\Foo\\SomeCode.cs", "source\\Foo");
			dualityPluginA.LocalMapping.Add("lib\\TestPluginA.Second.dll", "Plugins\\TestPluginA.Second.dll");
			dualityPluginA.LocalMapping.Add("content\\TestPluginA\\SomeRes.Pixmap.res", "Data\\TestPluginA\\SomeRes.Pixmap.res");
			dualityPluginA.LocalMapping.Add("source\\Foo\\SomeCode.cs", "Source\\Code\\AdamsLair.Duality.TestPluginA\\Foo\\SomeCode.cs");

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies",
				dualityPluginA,
				new[] { dualityPluginA }));

			// Duality plugin depending on another Duality plugin
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			dualityPluginB.AddDependency(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies",
				dualityPluginB,
				new[] { dualityPluginB, dualityPluginA }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA");
			otherLibraryA.AddFile("Data\\TestLibraryA\\SomeFile.txt", "content\\TestLibraryA");
			otherLibraryA.LocalMapping.Add("content\\TestLibraryA\\SomeFile.txt", "TestLibraryA\\SomeFile.txt");

			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC");
			dualityPluginC.AddDependency(otherLibraryA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies",
				dualityPluginC,
				new[] { dualityPluginC, otherLibraryA }));

			// Duality package that is not a plugin
			MockPackageSpec dualityNonPluginA = MockPackageSpec.CreateDualityCorePart("AdamsLair.Duality.TestNonPluginA");
			dualityNonPluginA.AddFile("Data\\TestNonPluginA\\SomeFile.txt", "content\\TestNonPluginA");
			dualityNonPluginA.LocalMapping.Add("content\\TestNonPluginA\\SomeFile.txt", "TestNonPluginA\\SomeFile.txt");

			cases.Add(new PackageOperationTestCase(
				"Duality Non-Plugin Package",
				dualityNonPluginA,
				new[] { dualityNonPluginA }));

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
		private IEnumerable<PackageOperationTestCase> InstallPackageWithFrameworkFolderTestCases()
		{
			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Files are located in lib root, without any framework folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					null);
				cases.Add(new PackageOperationTestCase(
					"No Framework",
					spec,
					new[] { spec }));
			}

			// Files are located in a Portable Profile 111 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"portable-net45+win8+wpa81");
				cases.Add(new PackageOperationTestCase(
					"Portable Profile 111",
					spec,
					new[] { spec }));
			}

			// Files are located in a Portable Profile 328 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"portable-net40+sl5+win8+wp8+wpa81");
				cases.Add(new PackageOperationTestCase(
					"Portable Profile 328",
					spec,
					new[] { spec }));
			}

			// Files are located in a .NET Framework 4.5 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"net45");
				cases.Add(new PackageOperationTestCase(
					".NET Framework 4.5",
					spec,
					new[] { spec }));
			}

			// Files are located in a .NET Framework 2.0 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"net20");
				cases.Add(new PackageOperationTestCase(
					".NET Framework 2.0",
					spec,
					new[] { spec }));
			}

			// Files are located in a .NET Standard 1.1 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"netstandard1.1");
				cases.Add(new PackageOperationTestCase(
					".NET Standard 1.1",
					spec,
					new[] { spec }));
			}

			// Files are located in a .NET Standard 2.0 framework lib folder
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin(
					"AdamsLair.Duality.TestPlugin",
					new Version(1, 0, 0, 0),
					"netstandard2.0");
				cases.Add(new PackageOperationTestCase(
					".NET Standard 2.0",
					spec,
					new[] { spec }));
			}

			// Different versions of the binaries are located in various framework folders, as well as the root folder
			{
				MockPackageSpec spec = new MockPackageSpec("AdamsLair.Duality.TestPlugin");
				spec.Tags.Add(PackageManager.DualityTag);
				spec.Tags.Add(PackageManager.PluginTag);

				// See here for a big list: https://docs.microsoft.com/en-us/nuget/reference/target-frameworks
				spec.AddFile("TestPlugin-Root.dll", "lib");
				spec.AddFile("TestPlugin-Portable111.dll", "lib\\portable-net45+win8+wpa81");
				spec.AddFile("TestPlugin-Portable328.dll", "lib\\portable-net40+sl5+win8+wp8+wpa81");
				spec.AddFile("TestPlugin-NetFramework47.dll", "lib\\net47");
				spec.AddFile("TestPlugin-NetFramework46.dll", "lib\\net46");
				spec.AddFile("TestPlugin-NetFramework45.dll", "lib\\net45");
				spec.AddFile("TestPlugin-NetFramework40.dll", "lib\\net40");
				spec.AddFile("TestPlugin-NetFramework30.dll", "lib\\net30");
				spec.AddFile("TestPlugin-NetFramework20.dll", "lib\\net20");
				spec.AddFile("TestPlugin-NetCore.dll", "lib\\netcore");
				spec.AddFile("TestPlugin-NetCore451.dll", "lib\\netcore451");
				spec.AddFile("TestPlugin-NetStandard10.dll", "lib\\netstandard1.0");
				spec.AddFile("TestPlugin-NetStandard11.dll", "lib\\netstandard1.1");
				spec.AddFile("TestPlugin-NetStandard13.dll", "lib\\netstandard1.3");
				spec.AddFile("TestPlugin-NetStandard20.dll", "lib\\netstandard2.0");
				spec.AddFile("TestPlugin-NetStandard21.dll", "lib\\netstandard2.1");
				spec.AddFile("TestPlugin-NetStandard30.dll", "lib\\netstandard3.0");
				spec.AddFile("TestPlugin-FooFramework.dll", "lib\\fooframework");

				// We expect the package manager to make a choice to find the closest matching framework.
				// Since the context of the package manager is editor / desktop development for now, we
				// expect to prefer the closest we can get to .NET Framework 4.5, as that's what the editor
				// is compiled with.
				spec.LocalMapping.Add(
					"lib\\net45\\TestPlugin-NetFramework45.dll",
					"Plugins\\TestPlugin-NetFramework45.dll");

				cases.Add(new PackageOperationTestCase(
					"Primary Preference .NET Framework 4.5",
					spec,
					new[] { spec }));
			}

			// Different versions of binaries again. Checking for secondary portable / profile 111 preference this time
			{
				MockPackageSpec spec = new MockPackageSpec("AdamsLair.Duality.TestPlugin");
				spec.Tags.Add(PackageManager.DualityTag);
				spec.Tags.Add(PackageManager.PluginTag);

				// See here for a big list: https://docs.microsoft.com/en-us/nuget/reference/target-frameworks
				spec.AddFile("TestPlugin-Root.dll", "lib");
				spec.AddFile("TestPlugin-Portable111.dll", "lib\\portable-net45+win8+wpa81");
				spec.AddFile("TestPlugin-Portable328.dll", "lib\\portable-net40+sl5+win8+wp8+wpa81");
				spec.AddFile("TestPlugin-NetCore.dll", "lib\\netcore");
				spec.AddFile("TestPlugin-NetCore451.dll", "lib\\netcore451");
				spec.AddFile("TestPlugin-NetStandard10.dll", "lib\\netstandard1.0");
				spec.AddFile("TestPlugin-NetStandard11.dll", "lib\\netstandard1.1");
				spec.AddFile("TestPlugin-NetStandard13.dll", "lib\\netstandard1.3");
				spec.AddFile("TestPlugin-NetStandard20.dll", "lib\\netstandard2.0");
				spec.AddFile("TestPlugin-NetStandard21.dll", "lib\\netstandard2.1");
				spec.AddFile("TestPlugin-NetStandard30.dll", "lib\\netstandard3.0");
				spec.AddFile("TestPlugin-FooFramework.dll", "lib\\fooframework");

				// We expect the package manager to make a choice to find the closest matching framework.
				// Since the context of the package manager is editor / desktop development for now, we
				// expect to prefer the closest we can get to .NET Framework 4.5, as that's what the editor
				// is compiled with.
				spec.LocalMapping.Add(
					"lib\\portable-net45+win8+wpa81\\TestPlugin-Portable111.dll",
					"Plugins\\TestPlugin-Portable111.dll");

				cases.Add(new PackageOperationTestCase(
					"Secondary Preference Portable Profile 111",
					spec,
					new[] { spec }));
			}

			// Legacy support: Files from the root folder are used as a fallback, if no framework-specific
			// "override" is defined for any of them.
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPlugin");
				spec.AddFile("AdamsLair.Duality.TestPlugin.dll", "lib");
				spec.AddFile("AdamsLair.Duality.TestPlugin.xml", "lib");
				spec.LocalMapping.Add("lib\\AdamsLair.Duality.TestPlugin.xml", "Plugins\\AdamsLair.Duality.TestPlugin.xml");
				cases.Add(new PackageOperationTestCase(
					"Root Folder Fallback",
					spec,
					new[] { spec }));
			}

			// Ensure framework selection does not affect content and source files
			{
				MockPackageSpec spec = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPlugin");
				spec.AddFile("Subfolder\\UnusedBinary.dll", "lib\\net20");
				spec.AddFile("Subfolder\\UnusedBinary2.dll", "lib\\netstandard1.1");
				spec.AddFile("Subfolder\\SecondBinary.dll", "lib");
				spec.AddFile("Data\\TestPlugin\\SomeRes.Pixmap.res", "content\\TestPlugin");
				spec.AddFile("Source\\Foo\\SomeCode.cs", "source\\Foo");
				spec.LocalMapping.Add("lib\\SecondBinary.dll", "Plugins\\SecondBinary.dll");
				spec.LocalMapping.Add("content\\TestPlugin\\SomeRes.Pixmap.res", "Data\\TestPlugin\\SomeRes.Pixmap.res");
				spec.LocalMapping.Add("source\\Foo\\SomeCode.cs", "Source\\Code\\AdamsLair.Duality.TestPlugin\\Foo\\SomeCode.cs");
				cases.Add(new PackageOperationTestCase(
					"Content Remains Unaffected",
					spec,
					new[] { spec }));
			}

			return cases;
		}
		private IEnumerable<PackageOperationTestCase> UninstallPackageTestCases()
		{
			List<PackageOperationTestCase> cases = new List<PackageOperationTestCase>();

			// Duality plugin without any dependencies
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies",
				new[] { dualityPluginA },
				dualityPluginA,
				new MockPackageSpec[0]));

			// Duality plugin with Duality plugin dependencies
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			dualityPluginB.AddDependency(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies",
				new[] { dualityPluginA, dualityPluginB },
				dualityPluginB,
				new[] { dualityPluginA }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA");
			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC");
			dualityPluginC.AddDependency(otherLibraryA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies",
				new[] { dualityPluginC, otherLibraryA },
				dualityPluginC,
				new MockPackageSpec[0]));

			// Duality plugin that has a dependency that other plugins still need
			MockPackageSpec dualityPluginD = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginD");
			dualityPluginD.AddDependency(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Shared Dependencies",
				new[] { dualityPluginA, dualityPluginB, dualityPluginD },
				dualityPluginD,
				new[] { dualityPluginA, dualityPluginB }));

			// Duality plugin that has multiple non-Duality dependencies that depend on each other
			MockPackageSpec otherLibraryB = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryB");
			MockPackageSpec otherLibraryC = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryC");
			MockPackageSpec otherLibraryD = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryD");
			MockPackageSpec otherLibraryE = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryE");
			MockPackageSpec dualityPluginE = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginE");
			otherLibraryB.AddDependency(otherLibraryD.Name);
			otherLibraryB.AddDependency(otherLibraryE.Name);
			otherLibraryD.AddDependency(otherLibraryC.Name);
			// Order is important - biggest dependency set comes centered to trigger most problems
			dualityPluginE.AddDependency(otherLibraryC.Name);
			dualityPluginE.AddDependency(otherLibraryB.Name);
			dualityPluginE.AddDependency(otherLibraryE.Name);

			cases.Add(new PackageOperationTestCase(
				"Interconnected Dependencies",
				new[] { otherLibraryD, otherLibraryC, otherLibraryE, otherLibraryB, dualityPluginE },
				dualityPluginE,
				new MockPackageSpec[0]));

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
				new[] { dualityPluginA_Old },
				dualityPluginA_Old,
				new[] { dualityPluginA_New }));

			// Duality plugin with Duality plugin dependencies
			MockPackageSpec dualityPluginB_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginB_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 1, 0, 0));
			dualityPluginB_Old.AddDependency(dualityPluginA_Old.Name);
			dualityPluginB_New.AddDependency(dualityPluginA_New.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies",
				new[] { dualityPluginA_Old, dualityPluginB_Old },
				dualityPluginB_Old,
				new[] { dualityPluginA_New, dualityPluginB_New }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA_Old = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA", new Version(1, 0, 0, 0));
			MockPackageSpec otherLibraryA_New = MockPackageSpec.CreateLibrary("Some.Other.TestLibraryA", new Version(1, 1, 0, 0));
			MockPackageSpec dualityPluginC_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginC_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 1, 0, 0));
			dualityPluginC_Old.AddDependency(otherLibraryA_Old.Name);
			dualityPluginC_New.AddDependency(otherLibraryA_New.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies",
				new[] { otherLibraryA_Old, dualityPluginC_Old },
				dualityPluginC_Old,
				new[] { otherLibraryA_New, dualityPluginC_New }));

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
			pluginA1.AddDependency(libraryA1.Name);
			pluginA2.AddDependency(libraryA2.Name);
			pluginB1.AddDependency(pluginA1.Name);
			pluginB2.AddDependency(pluginA2.Name);
			pluginD1.AddDependency(pluginA1.Name);

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
					new[] { libraryA2, pluginA2, pluginB2 }));
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
					new[] { libraryA1, pluginA1, pluginB1 }));
			}

			// Partial restore of packages to newest available versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginC1.Name.VersionInvariant);

				cases.Add(new PackageRestoreTestCase(
					"Partial Restore, Newest",
 					repository,
					new[] { libraryA1, pluginA1 },
					configSetup,
					new[] { libraryA1, pluginA1, pluginC2 }));
			}

			// Partial restore of packages to specific package versions
			{
				List<PackageName> configSetup = new List<PackageName>();
				configSetup.Add(pluginA1.Name);
				configSetup.Add(pluginB1.Name);

				cases.Add(new PackageRestoreTestCase(
					"Partial Restore, Specific",
 					repository,
					new[] { libraryA1, pluginA1 },
					configSetup,
					new[] { libraryA1, pluginA1, pluginB1 }));
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
					new[] { libraryA1, pluginA1 },
					configSetup,
					new[] { libraryA2, pluginA2, pluginB2 }));
			}

			// Full uninstall of all packages because they've been removed from the config
			{
				List<PackageName> configSetup = new List<PackageName>();

				cases.Add(new PackageRestoreTestCase(
					"Full Uninstall",
 					repository,
					new[] { libraryA2, pluginA2, pluginB2 },
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
					new[] { libraryA2, pluginA2, pluginB2 },
					configSetup,
					new[] { libraryA2, pluginA2 }));
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
					new[] { libraryA2, pluginA2, pluginB2 },
					configSetup,
					new[] { libraryA2, pluginA2, pluginB2 },
					new[] { libraryA2, pluginA2 },
					new[] { libraryA2, pluginA2 }));
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
					new[] { libraryA2, pluginA2, pluginD1 },
					configSetup,
					new[] { libraryA1, pluginA1, pluginD1 }));
			}

			return cases;
		}

		[Test] public void MultiFrameworkPackage()
		{
			// Let's see how the package manager handles a package with multiple frameworks
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			dualityPluginB.AddDependency("net45", dualityPluginA.Name);
			dualityPluginB.AddDependency("netstandard1.1", dualityPluginA.Name);

			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			repository.Add(dualityPluginB);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(repository);

			// Install pluginB and check if it correctly installs
			packageManager.InstallPackage(dualityPluginB.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA, dualityPluginB });
		}
		[Test] public void MultiFrameworkPackageWithDifferentDependencies()
		{
			// Let's see how the package manager handles a package with multiple frameworks but the dependencies also differ per framework
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC");
			dualityPluginA.AddDependency("netstandard1.1", dualityPluginB.Name);
			dualityPluginA.AddDependency("net45", dualityPluginC.Name);

			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			repository.Add(dualityPluginB);
			repository.Add(dualityPluginC);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(repository);

			// Install plugin A which should bring along its .NET Framework 4.5 dependency, but not
			// the .NET Standard 1.1 one.
			packageManager.InstallPackage(dualityPluginA.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA, dualityPluginC });
		}
		[Test] public void MultiFrameworkPackageWithUnsupportedFramework()
		{
			// Let's see how the package manager handles a package with multiple frameworks but one of the targetframeworks is unsupported!
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");

			dualityPluginB.AddDependency("net45", dualityPluginA.Name);
			dualityPluginB.AddDependency("foo", new PackageName("SomeNonexistentPackage", new Version(1, 0, 0)));

			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			repository.Add(dualityPluginB);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(repository);

			// Install pluginB and check if it correctly installs
			packageManager.InstallPackage(dualityPluginB.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA, dualityPluginB });
		}

		[Test] public void DuplicatePackage()
		{
			// Let's try to trick the package manager into having a duplicate version installed!
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			MockPackageSpec dualityPluginA_Old = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(0, 9, 0, 0));
			MockPackageSpec dualityPluginA_New = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB", new Version(1, 0, 0, 0));
			MockPackageSpec dualityPluginC = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginC", new Version(1, 0, 0, 0));
			dualityPluginB.AddDependency(dualityPluginA_New.Name);
			dualityPluginC.AddDependency(dualityPluginA_Old.Name);

			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA_Old);
			repository.Add(dualityPluginA_New);
			repository.Add(dualityPluginB);
			repository.Add(dualityPluginC);

			// Prepare the test by setting up remote repository and pre-installed local packages
			this.SetupReporistoryForTest(repository);

			// Install the old version first. Nothing special happens.
			packageManager.InstallPackage(dualityPluginA_Old.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_Old });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Install a newer version without uninstalling the old one.
			// Expect the newer version to replace the old.
			packageManager.InstallPackage(dualityPluginA_New.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_New });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Install an older version without uninstalling the newer one.
			// Expect the newer version to persist with no old version being installed.
			packageManager.InstallPackage(dualityPluginA_Old.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_New });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Downgrade from new to old explicitly
			packageManager.UninstallPackage(dualityPluginA_New.Name);
			packageManager.InstallPackage(dualityPluginA_Old.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_Old });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Install a package that depends on the newer version of the package.
			// Expect an update, but not a duplicate.
			packageManager.InstallPackage(dualityPluginB.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_New, dualityPluginB });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Install a package that depends on the older version of the package.
			// Expect the newer version to be used because it was already there.
			packageManager.InstallPackage(dualityPluginC.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA_New, dualityPluginB, dualityPluginC });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");
		}
		[Test] public void InstallNonExistent()
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			this.SetupReporistoryForTest(repository);

			// Install a non-existent package, invariant version
			Assert.Throws<InvalidOperationException>(() =>
			{
				packageManager.InstallPackage(new PackageName("Unknown.Doesnt.Exist"));
			});

			// Install a non-existent package, specific version
			Assert.Throws<InvalidOperationException>(() =>
			{
				packageManager.InstallPackage(new PackageName("Unknown.Doesnt.Exist", new Version(1, 0, 0, 0)));
			});

			// Install an existing package in a non-existent version
			Assert.Throws<InvalidOperationException>(() =>
			{
				packageManager.InstallPackage(new PackageName(dualityPluginA.Name.Id, new Version(9, 8, 7, 6)));
			});

			// Install a regular, existing package
			packageManager.InstallPackage(dualityPluginA.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");
		}
		[Test] public void UninstallNonExistent()
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			this.SetupReporistoryForTest(repository);

			// Uninstall a package that is not installed
			packageManager.UninstallPackage(dualityPluginA.Name);

			// Uninstall a package that does not exist at all
			packageManager.UninstallPackage(new PackageName("Unknown.Doesnt.Exist"));
			packageManager.UninstallPackage(new PackageName("Unknown.Doesnt.Exist", new Version(9, 8, 7, 6)));
			packageManager.UninstallPackage(new PackageName(dualityPluginA.Name.Id, new Version(9, 8, 7, 6)));
		}
		[Test] public void UpdateNonExistent()
		{
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Prepare the test by setting up remote repository and pre-installed local packages
			MockPackageSpec dualityPluginA = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginA");
			MockPackageSpec dualityPluginB = MockPackageSpec.CreateDualityPlugin("AdamsLair.Duality.TestPluginB");
			List<MockPackageSpec> repository = new List<MockPackageSpec>();
			repository.Add(dualityPluginA);
			repository.Add(dualityPluginB);
			this.SetupReporistoryForTest(repository);

			// Install a regular, existing package
			packageManager.InstallPackage(dualityPluginA.Name);
			this.AssertLocalSetup(packageManager.LocalSetup, new[] { dualityPluginA });
			Assert.IsFalse(packageManager.IsPackageSyncRequired, "Package setup out of sync.");

			// Update the existing package
			packageManager.UpdatePackage(dualityPluginA.Name);

			// Update a package that does not exist at all
			Assert.Throws<InvalidOperationException>(() =>
			{
				packageManager.UpdatePackage(new PackageName("Unknown.Doesnt.Exist"));
			});

			// Update a package that is not installed
			Assert.Throws<InvalidOperationException>(() =>
			{
				packageManager.UpdatePackage(dualityPluginB.Name);
			});
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
					PackageInfo packageInfo = packageManager.GetPackage(package.Name);
					if (packageInfo == null || packageInfo.Name != package.Name)
					{
						Assert.Inconclusive(
							"Failed to create the required package setup for the test. Unable to retrieve package '{0}'",
							package.Name);
					}

					packageManager.InstallPackage(packageInfo.Name);
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

				// Make sure that the install didn't leave the setup out of sync with the install
				if (packageManager.IsPackageSyncRequired)
				{
					Assert.Inconclusive(
						"Failed to create the required package setup for the test. " +
						"Local setup out of sync with installs.");
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
				actualSetup.Packages.Select(p => p.Name),
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
				Assert.IsTrue(
					File.Exists(sourceAbs),
					"Found copy instruction with the expected source, but the source file does not exist." + Environment.NewLine +
					"  Matching source: '{0}'",
					sourceAbs);
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
