using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Threading;

using Aga.Controls.Tree;

using Duality.Editor.PackageManagement;

namespace Duality.Editor.Plugins.PackageManagerFrontend.TreeModels
{
	public class OnlinePackagesTreeModel : PackageRepositoryTreeModel
	{
		public OnlinePackagesTreeModel(PackageManager manager) : base(manager)
		{
			this.packageManager.PackageInstalled	+= this.packageManager_PackageInstalled;
			this.packageManager.PackageUninstalled	+= this.packageManager_PackageUninstalled;
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.packageManager.PackageInstalled	-= this.packageManager_PackageInstalled;
			this.packageManager.PackageUninstalled	-= this.packageManager_PackageUninstalled;
		}

		protected override IEnumerable<object> EnumeratePackages()
		{
			return this.packageManager.GetLatestDualityPackages();
		}
		protected override BaseItem CreatePackageItem(object package, BaseItem parentItem)
		{
			return new OnlinePackageItem(package as PackageInfo, parentItem);
		}

		private void packageManager_PackageInstalled(object sender, PackageEventArgs e)
		{
			// Will be called from a worker thread, because packages are installed in one.
			
			BaseItem item = this.GetItem(e.Id, e.Version);
			if (item == null) return;

			this.ReadItemData(item);
		}
		private void packageManager_PackageUninstalled(object sender, PackageEventArgs e)
		{
			// Will be called from a worker thread, because packages are installed in one.

			BaseItem item = this.GetItem(e.Id, e.Version);
			if (item == null) return;

			this.ReadItemData(item);
		}
	}
}
