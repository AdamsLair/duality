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
	public abstract class PackageRepositoryTreeModel : ITreeModel
	{
		protected	PackageManager				packageManager		= null;
		private		List<BaseItem>				items				= new List<BaseItem>();
		private		BackgroundWorker			itemRetriever		= null;
		private		BackgroundWorker			itemInfoLoader		= null;
		private		ConcurrentQueue<BaseItem>	itemsToRead			= new ConcurrentQueue<BaseItem>();
		private		object						itemLock			= new object();
		private		bool						requireFullSort		= false;
		private		IComparer<BaseItem>			sortComparer		= null;

		public IComparer<BaseItem> SortComparer
		{
			get { return this.sortComparer; }
			set
			{
				this.sortComparer = value;
				this.requireFullSort = true;
				if (this.StructureChanged != null)
					this.StructureChanged(this, new TreePathEventArgs());
			}
		}
		public bool IsBusy
		{
			get { return this.itemRetriever.IsBusy || this.itemInfoLoader.IsBusy; }
		}

		#pragma warning disable 67  // Event never used
		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;

		public PackageRepositoryTreeModel(PackageManager manager)
		{
			this.packageManager = manager;
			
			this.itemInfoLoader = new BackgroundWorker();
			this.itemInfoLoader.WorkerReportsProgress = true;
			this.itemInfoLoader.DoWork += this.Worker_ReadItemData;
			this.itemInfoLoader.ProgressChanged += this.Worker_ProgressChanged;
			
			this.itemRetriever = new BackgroundWorker();
			this.itemRetriever.WorkerReportsProgress = true;
			this.itemRetriever.DoWork += this.Worker_RetrieveItems;
			this.itemRetriever.ProgressChanged += this.Worker_ItemsRetrieved;
		}

		public IEnumerable GetChildren(TreePath treePath)
		{
			BaseItem parentItem = treePath.LastNode as BaseItem;

			if (parentItem == null)
			{
				// Start retrieving all the items from out source asynchronously
				if (this.items.Count == 0 && !this.itemRetriever.IsBusy)
				{
					this.itemRetriever.RunWorkerAsync();
				}

				// Determine which items to display and return them
				List<BaseItem> displayedItems = new List<BaseItem>();
				lock (this.itemLock)
				{
					if (this.requireFullSort)
					{
						this.items.Sort(this.sortComparer);
					}
					foreach (BaseItem item in this.items)
					{
						displayedItems.Add(item);
					}
				}
				return displayedItems;
			}

			return Enumerable.Empty<BaseItem>();
		}
		public bool IsLeaf(TreePath treePath)
		{
			return true;
		}

		protected abstract IEnumerable<object> EnumeratePackages();
		protected abstract BaseItem CreatePackageItem(object package, BaseItem parentItem);
		
		private void SubmitItem(BaseItem item)
		{
			lock (this.itemLock)
			{
				// Remove the item, if already part of the list
				int oldIndex = this.items.IndexOf(item);
				if (oldIndex != -1)
					this.items.RemoveAt(oldIndex);

				// Determine where to insert the item
				int newIndex = oldIndex;
				if (this.sortComparer != null)
				{
					for (int i = 0; i < this.items.Count; i++)
					{
						if (this.sortComparer.Compare(this.items[i], item) > 0)
						{
							newIndex = i;
							break;
						}
					}
				}

				// Insert the item
				if (newIndex != -1)
					this.items.Insert(newIndex, item);
				else
					this.items.Add(item);
			}
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

		private void Worker_RetrieveItems(object sender, DoWorkEventArgs e)
		{
			// Create items within the model
			foreach (object package in this.EnumeratePackages())
			{
				// Create item and report back
				BaseItem item = null;
				item = this.CreatePackageItem(package, null);
				this.SubmitItem(item);
				this.itemsToRead.Enqueue(item);
				this.itemRetriever.ReportProgress(0, item);
			}
		}
		private void Worker_ItemsRetrieved(object sender, ProgressChangedEventArgs e)
		{
			BaseItem item = e.UserState as BaseItem;

			// Notify the model that we've got some new items
			if (this.NodesInserted != null)
			{
				int index;
				lock (this.items)
				{
					index = this.items.IndexOf(item);
				}
				this.NodesInserted(this, new TreeModelEventArgs(this.GetPath(item.Parent), new int[] { index }, new[] { item }));
			}

			// Wake info loader to read online item data
			if (!this.itemInfoLoader.IsBusy)
			{
				this.itemInfoLoader.RunWorkerAsync();
			}
		}
		private void Worker_ReadItemData(object sender, DoWorkEventArgs e)
		{
			while (!this.itemsToRead.IsEmpty)
			{
				BaseItem item;
				if (!this.itemsToRead.TryDequeue(out item))
					continue;

				// Read additional item data and report back
				item.RetrieveOnlineData(this.packageManager);
				this.SubmitItem(item);
				this.itemInfoLoader.ReportProgress(0, item);
			}
		}
		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			BaseItem item = e.UserState as BaseItem;

			// Notify the model that we'll need to re-retrieve the whole item list
			if (this.sortComparer != null)
			{
				if (this.StructureChanged != null)
					this.StructureChanged(this, new TreePathEventArgs());
			}
		}
	}
}
