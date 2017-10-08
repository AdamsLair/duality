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
	public class PackageEventListener : IDisposable
	{
		private PackageManager packageManager = null;
		private List<PackageName> installEvents = new List<PackageName>();
		private List<PackageName> uninstallEvents = new List<PackageName>();


		public PackageEventListener(PackageManager packageManager)
		{
			this.packageManager = packageManager;
			this.packageManager.PackageInstalled += this.packageManager_PackageInstalled;
			this.packageManager.PackageUninstalled += this.packageManager_PackageUninstalled;
		}
		public void Dispose()
		{
			if (this.packageManager == null) return;
			this.packageManager.PackageInstalled -= this.packageManager_PackageInstalled;
			this.packageManager.PackageUninstalled -= this.packageManager_PackageUninstalled;
			this.packageManager = null;
		}
		
		public void AssertChanges(IEnumerable<MockPackageSpec> installed, IEnumerable<MockPackageSpec> uninstalled)
		{
			foreach (MockPackageSpec package in installed) this.AssertInstall(package.Name);
			foreach (MockPackageSpec package in uninstalled) this.AssertUninstall(package.Name);
			this.AssertEmpty();
		}
		public void AssertInstall(PackageName package)
		{
			Assert.IsTrue(
				this.installEvents.Remove(package), 
				"Expected install event for package {0}, but did not receive one.", 
				package);
		}
		public void AssertUninstall(PackageName package)
		{
			Assert.IsTrue(this.uninstallEvents.Remove(package), 
				"Expected uninstall event for package {0}, but did not receive one.", 
				package);
		}
		public void AssertEmpty()
		{
			Assert.IsEmpty(this.installEvents, "Install package events");
			Assert.IsEmpty(this.uninstallEvents, "Uninstall package events");
		}

		private void packageManager_PackageUninstalled(object sender, PackageEventArgs e)
		{
			this.uninstallEvents.Add(e.PackageName);
		}
		private void packageManager_PackageInstalled(object sender, PackageEventArgs e)
		{
			this.installEvents.Add(e.PackageName);
		}
	}
}
