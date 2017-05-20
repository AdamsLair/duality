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
			MockPackageSpec packageSpecNonDuality = new MockPackageSpec("Some.Other.Package", new Version(1, 0, 0, 0));
			MockPackageSpec packageSpecPlugin = new MockPackageSpec("AdamsLair.Duality.TestPlugin", new Version(1, 0, 0, 0));
			MockPackageSpec packageSpecPluginLatest = new MockPackageSpec("AdamsLair.Duality.TestPlugin", new Version(1, 1, 0, 0));
			MockPackageSpec packageSpecSample = new MockPackageSpec("AdamsLair.Duality.TestSample", new Version(1, 1, 0, 0));

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
		[Test] public void InstallPackage()
		{
			MockPackageSpec packageSpec = new MockPackageSpec("AdamsLair.Duality.TestPlugin", new Version(1, 0, 0, 0));
			packageSpec.CreatePackage(TestPackageBuildPath, TestRepositoryPath);

			PackageManager packageManager = new PackageManager(this.workEnv, this.setup);
			
			// Precondition: No packages installed, no apply or sync required
			Assert.IsFalse(packageManager.IsPackageSyncRequired);
			Assert.IsEmpty(packageManager.LocalSetup.Packages);
			Assert.IsFalse(File.Exists(this.workEnv.UpdateFilePath));

			// Retrieve PackageInfo from the mock repository
			PackageInfo packageInfo = packageManager.QueryPackageInfo(packageSpec.Name);
			Assert.IsNotNull(packageInfo);
			Assert.AreEqual(packageInfo.PackageName, packageSpec.Name);

			// Install the package while listening for events
			List<PackageName> installEvents = new List<PackageName>();
			EventHandler<PackageEventArgs> installedHandler = (sender, args) =>
			{
				installEvents.Add(args.PackageName);
			};
			packageManager.PackageInstalled += installedHandler;
			packageManager.InstallPackage(packageInfo);
			packageManager.PackageInstalled -= installedHandler;

			// Assert that the package was installed properly
			Assert.AreEqual(1, installEvents.Count);
			Assert.AreEqual(packageSpec.Name, installEvents[0]);
			Assert.AreEqual(1, packageManager.LocalSetup.Packages.Count);
			Assert.AreEqual(packageSpec.Name, packageManager.LocalSetup.Packages[0].PackageName);
			Assert.IsTrue(packageManager.LocalSetup.Packages[0].IsInstallationComplete);

			// Assert that there is an apply script now
			Assert.IsTrue(File.Exists(this.workEnv.UpdateFilePath));

			// Load the apply script and assert its contents match the expected
			PackageUpdateSchedule applyScript = PackageUpdateSchedule.Load(this.workEnv.UpdateFilePath);
			List<XElement> updateItems = applyScript.Items.ToList();
			Assert.AreEqual(2, updateItems.Count);

			// Note that NuGet by default does not recognize lib subfolders during installation.
			// The files will be packaged with subfolders, but their EffectivePath won't include
			// them - which is why, unless Duality addresses this at some point in the future, 
			// all plugins will end up in the Plugins root folder, regardless of their previous
			// hierarchy.
			this.AssertUpdateScheduleCopyItem(updateItems, packageSpec.Name, "lib\\Foo.dll", "Plugins\\Foo.dll");
			this.AssertUpdateScheduleCopyItem(updateItems, packageSpec.Name, "lib\\Subfolder\\Bar.dll", "Plugins\\Bar.dll");
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
