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
	public class InstalledPackagesTreeModel : ITreeModel
	{
		private		PackageManager		packageManager	= null;
		private		BackgroundWorker	itemLoader		= null;
		private		List<BaseItem>		itemsToRead		= null;

		#pragma warning disable 67  // Event never used
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;

		public InstalledPackagesTreeModel(PackageManager manager)
		{
			this.packageManager = manager;
			this.itemsToRead = new List<BaseItem>();
			
			this.itemLoader = new BackgroundWorker();
			this.itemLoader.WorkerReportsProgress = true;
			this.itemLoader.DoWork += this.Worker_ReadItemData;
			this.itemLoader.ProgressChanged += this.Worker_ProgressChanged;
		}

		public IEnumerable GetChildren(TreePath treePath)
		{
			BaseItem parentItem = treePath.LastNode as BaseItem;
			if (parentItem == null)
			{
				foreach (LocalPackage package in this.packageManager.LocalPackages)
				{
					LocalPackageItem item = new LocalPackageItem(package, parentItem);
					this.itemsToRead.Add(item);
					yield return item;
				}
				if (!this.itemLoader.IsBusy) this.itemLoader.RunWorkerAsync();
			}
			yield break;
		}
		public bool IsLeaf(TreePath treePath)
		{
			return true;
		}
		
		private TreePath GetPath(BaseItem item)
		{
			Stack<object> stack = new Stack<object>();
			while (item != null)
			{
				stack.Push(item);
				item = item.Parent;
			}
			return new TreePath(stack.ToArray());
		}
		private void Worker_ReadItemData(object sender, DoWorkEventArgs e)
		{
			while (this.itemsToRead.Count > 0)
			{
				BaseItem item = this.itemsToRead[0];
				this.itemsToRead.RemoveAt(0);
				item.RetrieveOnlineData(this.packageManager);
				this.itemLoader.ReportProgress(0, item);
			}
		}
		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			BaseItem item = e.UserState as BaseItem;
			if (this.NodesChanged != null)
			{
				TreePath path = GetPath(item.Parent);
				this.NodesChanged(this, new TreeModelEventArgs(path, new object[] { item } ));
			}
		}
	}
}
