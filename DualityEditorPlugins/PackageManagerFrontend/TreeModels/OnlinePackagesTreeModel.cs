using System;
using System.Collections;
using System.Collections.Generic;
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
		public OnlinePackagesTreeModel(PackageManager manager) : base(manager) {}

		protected override IEnumerable<object> EnumeratePackages()
		{
			return this.packageManager.QueryAvailablePackages();
		}
		protected override BaseItem CreatePackageItem(object package, BaseItem parentItem)
		{
			return new OnlinePackageItem(package as PackageInfo, parentItem);
		}
	}
}
