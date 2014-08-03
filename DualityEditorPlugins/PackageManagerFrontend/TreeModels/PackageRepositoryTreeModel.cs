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
	public abstract class PackageRepositoryTreeModel : ITreeModel
	{
		protected	PackageManager		packageManager	= null;
		private		BackgroundWorker	itemLoader		= null;
		private		List<BaseItem>		itemsToRead		= null;
		private		object				itemLock		= new object();

		#pragma warning disable 67  // Event never used
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;

		public PackageRepositoryTreeModel(PackageManager manager)
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
				// Create items within the model
				foreach (object package in this.EnumeratePackages())
				{
					// Create item
					BaseItem item = null;
					lock (this.itemLock)
					{
						item = this.CreatePackageItem(package, parentItem);
						this.itemsToRead.Add(item);

						// Return the item so it can be displayed while still loading more
						yield return item;
					}

					// Wake worker to read online item data
					if (!this.itemLoader.IsBusy)
					{
						this.itemLoader.RunWorkerAsync();
					}
				}
			}

			yield break;
		}
		public bool IsLeaf(TreePath treePath)
		{
			return true;
		}

		protected abstract IEnumerable<object> EnumeratePackages();
		protected abstract BaseItem CreatePackageItem(object package, BaseItem parentItem);
		
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
				BaseItem item = null;
				lock (this.itemLock)
				{
					item = this.itemsToRead[0];
					this.itemsToRead.RemoveAt(0);
				}
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
				lock (this.itemLock)
				{
					this.NodesChanged(this, new TreeModelEventArgs(path, new[] { item }));
				}
			}
		}
	}
}
