using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Duality.Editor.Controls.TreeModels.FileSystem
{
	public abstract class BaseItem
	{
		private string path = "";
		private DateTime lastWriteDate = DateTime.MinValue;
		private DateTime creationDate = DateTime.MinValue;
		private Image icon = null;
		private	bool readOnly = false;

		public string ItemPath
		{
			get { return path; }
			set { path = value; }
		}
		public DateTime LastWriteDate
		{
			get { return lastWriteDate; }
			set { lastWriteDate = value; }
		}
		public DateTime CreationDate
		{
			get { return creationDate; }
			set { creationDate = value; }
		}
		public bool ReadOnly
		{
			get { return this.readOnly; }
			set { this.readOnly = value; }
		}
		public Image Icon
		{
			get { return icon; }
			set { icon = value; }
		}

		public abstract string Name
		{
			get;
			set;
		}

		private BaseItem _parent;
		public BaseItem Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}
	}

	public class LogicalDriveItem : BaseItem
	{
		public LogicalDriveItem(string name)
		{
			ItemPath = name;
		}

		public override string Name
		{
			get
			{
				return ItemPath;
			}
			set {}
		}
	}

	public class FolderItem : BaseItem
	{
		public override string Name
		{
			get
			{
				return Path.GetFileName(ItemPath);
			}
			set
			{
				string dir = Path.GetDirectoryName(ItemPath);
				string destination = Path.Combine(dir, value);
				Directory.Move(ItemPath, destination);
				ItemPath = destination;
			}
		}

		public FolderItem(string name, BaseItem parent)
		{
			ItemPath = name;
			Parent = parent;
		}
	}

	public class FileItem : BaseItem
	{
		private long size = 0;

		public long Size
		{
			get { return size; }
			set { size = value; }
		}
		public override string Name
		{
			get
			{
				return Path.GetFileName(ItemPath);
			}
			set
			{
				string dir = Path.GetDirectoryName(ItemPath);
				string destination = Path.Combine(dir, value);
				File.Move(ItemPath, destination);
				ItemPath = destination;
			}
		}

		public FileItem(string name, BaseItem parent)
		{
			ItemPath = name;
			Parent = parent;
		}
	}
}
