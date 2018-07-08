using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class BeginGlobalRenameEventArgs : EventArgs
	{
		private string path = null;
		private string oldPath = null;
		private bool isDirectory = false;
		private bool cancel = false;

		public bool IsDirectory
		{
			get { return this.isDirectory; }
		}
		public string Path
		{
			get { return this.path; }
		}
		public string OldPath
		{
			get { return this.oldPath; }
		}
		public ContentRef<Resource> Content
		{
			get { return this.isDirectory ? null : new ContentRef<Resource>(null, this.path); }
		}
		public ContentRef<Resource> OldContent
		{
			get { return this.isDirectory ? null : new ContentRef<Resource>(null, this.oldPath); }
		}
		public bool IsCancelled
		{
			get { return this.cancel; }
		}

		public BeginGlobalRenameEventArgs(string path, string oldPath, bool isDirectory)
		{
			this.path = path;
			this.oldPath = oldPath;
			this.isDirectory = isDirectory;
		}

		public void Cancel()
		{
			this.cancel = true;
		}
	}
}
