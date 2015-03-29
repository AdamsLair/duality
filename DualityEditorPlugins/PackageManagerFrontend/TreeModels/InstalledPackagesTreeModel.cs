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
	public class InstalledPackagesTreeModel : PackageRepositoryTreeModel
	{
		protected enum ChangeOperation
		{
			Add,
			Remove
		}
		protected class ChangeEvent
		{
			private	BaseItem item;
			private	ChangeOperation operation;

			public BaseItem Item
			{
				get { return this.item; }
			}
			public ChangeOperation Operation
			{
				get { return this.operation; }
			}

			public ChangeEvent(BaseItem item, ChangeOperation op)
			{
				this.item = item;
				this.operation = op;
			}
		}

		private	ConcurrentQueue<ChangeEvent>	changeQueue	= new ConcurrentQueue<ChangeEvent>();

		public InstalledPackagesTreeModel(PackageManager manager) : base(manager)
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

		public override void Refresh()
		{
			base.Refresh();
			while (!this.changeQueue.IsEmpty)
			{
				ChangeEvent change;
				this.changeQueue.TryDequeue(out change);
			}
		}
		public override void ApplyChanges()
		{
			base.ApplyChanges();
			while (!this.changeQueue.IsEmpty)
			{
				ChangeEvent change;
				this.changeQueue.TryDequeue(out change);
				if (change.Operation == ChangeOperation.Add)
				{
					this.AddItem(change.Item);
				}
				else if (change.Operation == ChangeOperation.Remove)
				{
					this.RemoveItem(change.Item);
				}
			}
		}

		protected override IEnumerable<object> EnumeratePackages()
		{
			return this.packageManager.LocalPackages;
		}
		protected override BaseItem CreatePackageItem(object package, BaseItem parentItem)
		{
			return new LocalPackageItem(package as LocalPackage, parentItem);
		}
		
		private void packageManager_PackageInstalled(object sender, PackageEventArgs e)
		{
			// Will be called from a worker thread, because packages are installed in one.

			LocalPackage localPackage = this.packageManager.LocalPackages.FirstOrDefault(p => p.Id == e.Id && p.Version == e.Version);
			if (localPackage == null) return;

			BaseItem item = this.CreatePackageItem(localPackage, null);
			if (item == null) return;

			this.changeQueue.Enqueue(new ChangeEvent(item, ChangeOperation.Add));
		}
		private void packageManager_PackageUninstalled(object sender, PackageEventArgs e)
		{
			// Will be called from a worker thread, because packages are installed in one.

			BaseItem item = this.GetItem(e.Id, e.Version);
			if (item == null) return;

			this.changeQueue.Enqueue(new ChangeEvent(item, ChangeOperation.Remove));
		}
	}
}
