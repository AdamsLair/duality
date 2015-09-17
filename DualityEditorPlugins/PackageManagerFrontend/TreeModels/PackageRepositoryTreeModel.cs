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
	public abstract class PackageRepositoryTreeModel : ITreeModel, IDisposable
	{
		private class NewItemRetrievedReport
		{
			public BaseItem Item { get; set; }
			public int InsertAtIndex { get; set; }
		}
		
		protected	PackageManager					packageManager		= null;
		private		bool							disposed			= false;
		private		List<BaseItem>					items				= new List<BaseItem>();
		private		BackgroundWorker				itemRetriever		= null;
		private		BackgroundWorker				itemInfoLoader		= null;
		private		BackgroundWorker				itemIconLoader		= null;
		private		ConcurrentQueue<BaseItem>		itemsToRead			= new ConcurrentQueue<BaseItem>();
		private		ConcurrentQueue<BaseItem>		itemsToGetIcon		= new ConcurrentQueue<BaseItem>();
		private		object							workerLock			= new object();
		private		object							itemLock			= new object();
		private		bool							requireFullSort		= false;
		private		IComparer<BaseItem>				sortComparer		= null;
		private		Predicate<BaseItem>				itemFilter			= null;

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
		public Predicate<BaseItem> ItemFilter
		{
			get { return this.itemFilter; }
			set
			{
				this.itemFilter = value;
				if (this.StructureChanged != null)
					this.StructureChanged(this, new TreePathEventArgs());
			}
		}
		public bool IsBusy
		{
			get { return this.itemRetriever.IsBusy || this.itemInfoLoader.IsBusy || this.itemIconLoader.IsBusy; }
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

			this.itemIconLoader = new BackgroundWorker();
			this.itemIconLoader.WorkerReportsProgress = true;
			this.itemIconLoader.DoWork += this.Worker_ReadItemIcon;
			this.itemIconLoader.ProgressChanged += this.Worker_IconChanged;
			
			this.itemRetriever = new BackgroundWorker();
			this.itemRetriever.WorkerReportsProgress = true;
			this.itemRetriever.DoWork += this.Worker_RetrieveItems;
			this.itemRetriever.ProgressChanged += this.Worker_ItemsRetrieved;
		}
		~PackageRepositoryTreeModel()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool manually)
		{
			if (!this.disposed)
			{
				this.OnDisposing(manually);
				this.disposed = true;
			}
		}
		protected virtual void OnDisposing(bool manually)
		{

		}
		
		/// <summary>
		/// Rebuilds the entire model.
		/// </summary>
		public virtual void Refresh()
		{
			while (!this.itemsToRead.IsEmpty)
			{
				BaseItem item;
				this.itemsToRead.TryDequeue(out item);
			}
			while (!this.itemsToGetIcon.IsEmpty)
			{
				BaseItem item;
				this.itemsToGetIcon.TryDequeue(out item);
			}
			lock (this.itemLock)
			{
				this.items.Clear();
			}
			if (this.StructureChanged != null)
				this.StructureChanged(this, new TreePathEventArgs());
		}
		/// <summary>
		/// Performs a main-thread update on the item model to process
		/// update operations that were scheduled from worker threads.
		/// </summary>
		public virtual void ApplyChanges()
		{
			this.RunLoadInfoWorkers();
		}

		public IEnumerable GetChildren(TreePath treePath)
		{
			BaseItem parentItem = treePath.LastNode as BaseItem;

			if (parentItem == null)
			{
				// Start retrieving all the items from out source asynchronously
				if (this.items.Count == 0)
					this.RunWorker(this.itemRetriever);

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
						if (itemFilter != null && !itemFilter(item)) continue;
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
		
		protected void ReadItemData(BaseItem item)
		{
			this.itemsToRead.Enqueue(item);
			this.itemsToGetIcon.Enqueue(item);
		}
		/// <summary>
		/// Wakes info and icon loaders to read online item data.
		/// </summary>
		protected void RunLoadInfoWorkers()
		{
			if (!this.itemsToRead.IsEmpty)
				this.RunWorker(this.itemInfoLoader);
			if (!this.itemsToGetIcon.IsEmpty)
				this.RunWorker(this.itemIconLoader);
		}
		protected BaseItem GetItem(string packageId, Version packageVersion)
		{
			lock (this.itemLock)
			{
				return this.items.OfType<PackageItem>().FirstOrDefault(i => i.Id == packageId && i.Version == packageVersion);
			}
		}
		protected void AddItem(BaseItem item)
		{
			this.SubmitItem(item);
			this.ReadItemData(item);

			if (this.NodesInserted != null)
			{
				int index = GetItemIndex(item);
				if (index != -1)
				{
					this.NodesInserted(this, new TreeModelEventArgs(this.GetPath(item.Parent), new int[] { index }, new[] { item }));
				}
			}

			// Wake info and icon loaders to read online item data
			this.RunLoadInfoWorkers();
		}
		protected void RemoveItem(BaseItem item)
		{
			int index = GetItemIndex(item);
			lock (this.itemLock)
			{
				this.items.Remove(item);
			}
			if (this.NodesRemoved != null && index != -1)
			{
				this.NodesRemoved(this, new TreeModelEventArgs(this.GetPath(item.Parent), new int[] { index }, new[] { item }));
			}
		}

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
		private int GetItemIndex(BaseItem item)
		{
			if (this.itemFilter == null)
			{
				int index;
				lock (this.itemLock)
				{
					index = this.items.IndexOf(item);
				}
				return index;
			}
			else if (this.itemFilter(item))
			{
				int index = 0;
				lock (this.itemLock)
				{
					foreach (BaseItem i in this.items)
					{
						if (!this.itemFilter(i)) continue;
						if (i == item) break;
						++index;
					}
				}
				return index;
			}
			else
			{
				return -1;
			}
		}

		private void RunWorker(BackgroundWorker worker)
		{
			if (!worker.IsBusy)
			{
				lock (this.workerLock)
				{
					if (!worker.IsBusy)
						worker.RunWorkerAsync();
				}
			}
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
				this.ReadItemData(item);
				this.itemRetriever.ReportProgress(0, new NewItemRetrievedReport
				{
					Item = item,
					InsertAtIndex = this.GetItemIndex(item)
				});
			}
		}
		private void Worker_ItemsRetrieved(object sender, ProgressChangedEventArgs e)
		{
			NewItemRetrievedReport report = e.UserState as NewItemRetrievedReport;

			// Notify the model that we've got some new items
			if (this.NodesInserted != null)
			{
				int newIndex = this.GetItemIndex(report.Item);
				if (newIndex != -1)
				{
					this.NodesInserted(this, new TreeModelEventArgs(this.GetPath(report.Item.Parent), new int[] { report.InsertAtIndex }, new[] { report.Item }));
				}
			}

			// Wake info and icon loaders to read online item data
			this.RunLoadInfoWorkers();
		}
		private void Worker_ReadItemData(object sender, DoWorkEventArgs e)
		{
			while (!this.itemsToRead.IsEmpty)
			{
				BaseItem item;
				if (!this.itemsToRead.TryDequeue(out item))
					continue;

				// Read additional item data and report back
				item.RetrieveAsyncData(this.packageManager);
				this.SubmitItem(item);
				this.itemInfoLoader.ReportProgress(0, item);
			}
		}
		private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			BaseItem item = e.UserState as BaseItem;

			// Notify the model that we've changed some items
			if (this.NodesChanged != null)
			{
				int index = this.GetItemIndex(item);
				if (index != -1)
				{
					this.NodesChanged(this, new TreeModelEventArgs(this.GetPath(item.Parent), new[] { item }));
				}
			}
		}
		private void Worker_ReadItemIcon(object sender, DoWorkEventArgs e)
		{
			while (!this.itemsToGetIcon.IsEmpty)
			{
				BaseItem item;
				if (!this.itemsToGetIcon.TryDequeue(out item))
					continue;

				// Read the item icon and update the view. Doesn't affect sorting, so no Submit here
				item.RetrieveIcon();
				this.itemIconLoader.ReportProgress(0, item);
			}
		}
		private void Worker_IconChanged(object sender, ProgressChangedEventArgs e)
		{
			BaseItem item = e.UserState as BaseItem;

			// Notify the model that we've changed some items
			if (this.NodesChanged != null)
			{
				int index = this.GetItemIndex(item);
				if (index != -1)
				{
					this.NodesChanged(this, new TreeModelEventArgs(this.GetPath(item.Parent), new[] { item }));
				}
			}
		}
	}
}
