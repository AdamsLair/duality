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
			// Build packages from the specs we're testing with
			foreach (MockPackageSpec package in testCase.Repository)
				package.CreatePackage(TestPackageBuildPath, TestRepositoryPath);

			// Set up a new package manager that we'll test our install with
			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);

			// Setup the required pre-installed packages for the test
			this.SetupPackagesForTest(packageManager, testCase.Setup);

			// Retrieve PackageInfo from the mock repository
			PackageInfo packageInfo = packageManager.QueryPackageInfo(testCase.Target.Name);
			Assert.IsNotNull(packageInfo);
			Assert.AreEqual(packageInfo.PackageName, testCase.Target.Name);

			// Install the package while listening for events
			List<PackageName> installEvents = new List<PackageName>();
			EventHandler<PackageEventArgs> installedHandler = (sender, args) =>
			{
				installEvents.Add(args.PackageName);
			};
			packageManager.PackageInstalled += installedHandler;
			packageManager.InstallPackage(packageInfo);
			packageManager.PackageInstalled -= installedHandler;

			// Assert that the expected install events were fired
			Assert.AreEqual(
				testCase.Installed.Count, 
				installEvents.Count);
			CollectionAssert.AreEquivalent(
				testCase.Installed.Select(p => p.Name), 
				installEvents);

			// Assert that the expected packages were correctly registered in the local setup
			Assert.AreEqual(
				testCase.DualityResults.Count, 
				packageManager.LocalSetup.Packages.Count);
			CollectionAssert.AreEquivalent(
				testCase.DualityResults.Select(p => p.Name), 
				packageManager.LocalSetup.Packages.Select(p => p.PackageName));
			Assert.IsTrue(
				packageManager.LocalSetup.Packages.All(p => p.IsInstallationComplete));

			// Expect the update schedule to reflect the expected installs
			bool anyUpdateExpected = testCase.Installed.Count > 0;
			Assert.AreEqual(anyUpdateExpected, File.Exists(this.workEnv.UpdateFilePath));
			if (anyUpdateExpected)
			{
				PackageUpdateSchedule applyScript = PackageUpdateSchedule.Load(this.workEnv.UpdateFilePath);
				List<XElement> updateItems = applyScript.Items.ToList();
				foreach (MockPackageSpec expectedPackage in testCase.Installed)
				{
					foreach (var pair in expectedPackage.LocalMapping)
					{
						this.AssertUpdateScheduleCopyItem(
							updateItems, 
							expectedPackage.Name, 
							pair.Key, 
							pair.Value);
					}
				}
			}
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
			MockPackageSpec dualityPluginA = new MockPackageSpec("AdamsLair.Duality.TestPluginA");
			dualityPluginA.Tags.Add(PackageManager.DualityTag);
			dualityPluginA.Tags.Add(PackageManager.PluginTag);
			dualityPluginA.Files.Add("TestPluginA.dll", "lib");
			dualityPluginA.Files.Add("Subfolder\\TestPluginA.Second.dll", "lib\\Subfolder");
			dualityPluginA.Files.Add("Data\\TestPluginA\\SomeRes.Pixmap.res", "content\\TestPluginA");
			dualityPluginA.Files.Add("Source\\Foo\\SomeCode.cs", "source\\Foo");
			dualityPluginA.LocalMapping.Add("lib\\TestPluginA.dll", "Plugins\\TestPluginA.dll");
			dualityPluginA.LocalMapping.Add("lib\\Subfolder\\TestPluginA.Second.dll", "Plugins\\TestPluginA.Second.dll");
			dualityPluginA.LocalMapping.Add("content\\TestPluginA\\SomeRes.Pixmap.res", "Data\\TestPluginA\\SomeRes.Pixmap.res");
			dualityPluginA.LocalMapping.Add("source\\Foo\\SomeCode.cs", "Source\\Code\\AdamsLair.Duality.TestPluginA\\Foo\\SomeCode.cs");

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, No Dependencies", 
				dualityPluginA, 
				new [] { dualityPluginA }));

			// Duality plugin depending on another Duality plugin
			MockPackageSpec dualityPluginB = new MockPackageSpec("AdamsLair.Duality.TestPluginB");
			dualityPluginB.Tags.Add(PackageManager.DualityTag);
			dualityPluginB.Tags.Add(PackageManager.PluginTag);
			dualityPluginB.Files.Add("TestPluginB.dll", "lib");
			dualityPluginB.LocalMapping.Add("lib\\TestPluginB.dll", "Plugins\\TestPluginB.dll");
			dualityPluginB.Dependencies.Add(dualityPluginA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Duality Dependencies", 
				dualityPluginB, 
				new [] { dualityPluginB, dualityPluginA }));

			// Duality plugin depending on a non-Duality NuGet package
			MockPackageSpec otherLibraryA = new MockPackageSpec("Some.Other.TestLibraryA");
			otherLibraryA.Files.Add("TestLibraryA.dll", "lib");
			otherLibraryA.Files.Add("Data\\TestLibraryA\\SomeFile.txt", "content\\TestLibraryA");
			otherLibraryA.LocalMapping.Add("lib\\TestLibraryA.dll", "TestLibraryA.dll");
			otherLibraryA.LocalMapping.Add("content\\TestLibraryA\\SomeFile.txt", "TestLibraryA\\SomeFile.txt");

			MockPackageSpec dualityPluginC = new MockPackageSpec("AdamsLair.Duality.TestPluginC");
			dualityPluginC.Tags.Add(PackageManager.DualityTag);
			dualityPluginC.Tags.Add(PackageManager.PluginTag);
			dualityPluginC.Files.Add("TestPluginC.dll", "lib");
			dualityPluginC.LocalMapping.Add("lib\\TestPluginC.dll", "Plugins\\TestPluginC.dll");
			dualityPluginC.Dependencies.Add(otherLibraryA.Name);

			cases.Add(new PackageOperationTestCase(
				"Duality Plugin, With Lib Dependencies", 
				dualityPluginC, 
				new [] { dualityPluginC, otherLibraryA }));
			
			// Duality package that is not a plugin
			MockPackageSpec dualityNonPluginA = new MockPackageSpec("AdamsLair.Duality.TestNonPluginA");
			dualityNonPluginA.Tags.Add(PackageManager.DualityTag);
			dualityNonPluginA.Files.Add("TestNonPluginA.dll", "lib");
			dualityNonPluginA.Files.Add("Data\\TestNonPluginA\\SomeFile.txt", "content\\TestNonPluginA");
			dualityNonPluginA.LocalMapping.Add("lib\\TestNonPluginA.dll", "TestNonPluginA.dll");
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

			return cases;
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
					LocalPackage localPackage = packageManager.LocalSetup.GetPackage(package.Name);
					if (localPackage == null || !localPackage.IsInstallationComplete)
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

		private void AssertUpdateScheduleCopyItem(IEnumerable<XElement> items, PackageName package, string source, string target)
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
	}
}
