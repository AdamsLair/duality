using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Aga.Controls.Tree;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace Duality.Editor.Controls.TreeModels.FileSystem
{
	public class FolderBrowserTreeModel : ITreeModel
	{
		private		Predicate<string>	filter		= null;
		private		string				basePath	= null;
		private		BackgroundWorker	itemLoader	= null;
		private		List<BaseItem>		itemsToRead	= null;

		public Predicate<string> Filter
		{
			get { return this.filter; }
			set { this.filter = value; }
		}
		public string BasePath
		{
			get { return this.basePath; }
			set { this.basePath = value; }
		}

		public FolderBrowserTreeModel(string basePath = null)
		{
			this.basePath = basePath;
			this.itemsToRead = new List<BaseItem>();
			
			this.itemLoader = new BackgroundWorker();
			this.itemLoader.WorkerReportsProgress = true;
			this.itemLoader.DoWork += this.Worker_ReadFilesProperties;
			this.itemLoader.ProgressChanged += this.Worker_ProgressChanged;
		}

		private void Worker_ReadFilesProperties(object sender, DoWorkEventArgs e)
		{
			while (this.itemsToRead.Count > 0)
			{
				BaseItem item = this.itemsToRead[0];
				FolderItem folderItem = item as FolderItem;
				FileItem fileItem = item as FileItem;
				this.itemsToRead.RemoveAt(0);

				if (folderItem != null)
				{
					DirectoryInfo info = new DirectoryInfo(item.ItemPath);
					folderItem.LastWriteDate = info.CreationTime;
					folderItem.CreationDate = info.CreationTime;
				}
				else if (fileItem != null)
				{
					FileInfo info = new FileInfo(item.ItemPath);
					fileItem.Size = info.Length;
					fileItem.LastWriteDate = info.LastWriteTime;
					fileItem.CreationDate = info.CreationTime;
				}
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
		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			if (string.IsNullOrEmpty(this.basePath) && treePath.IsEmpty())
			{
				foreach (string str in Environment.GetLogicalDrives())
				{
					LogicalDriveItem item = new LogicalDriveItem(str);
					yield return item;
				}
			}
			else
			{
				List<BaseItem> items = new List<BaseItem>();
				BaseItem parent = treePath.LastNode as BaseItem;
				string path = parent != null ? parent.ItemPath : this.basePath;
				if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
				{
					try
					{
						IEnumerable<string> dirQuery = Directory.GetDirectories(path);
						IEnumerable<string> fileQuery = Directory.GetFiles(path);
						if (this.filter != null)
						{
							dirQuery = dirQuery.Where(s => this.filter(s));
							fileQuery = fileQuery.Where(s => this.filter(s));
						}
						foreach (string str in dirQuery) items.Add(new FolderItem(str, parent));
						foreach (string str in fileQuery) items.Add(new FileItem(str, parent));
					}
					catch (UnauthorizedAccessException) {}

					this.itemsToRead.AddRange(items);
					if (!this.itemLoader.IsBusy) this.itemLoader.RunWorkerAsync();

					foreach (BaseItem item in items)
						yield return item;
				}
				else
					yield break;
			}
		}
		public bool IsLeaf(TreePath treePath)
		{
			return treePath.LastNode is FileItem;
		}

		public event EventHandler<TreeModelEventArgs> NodesChanged;
		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
	}
}
