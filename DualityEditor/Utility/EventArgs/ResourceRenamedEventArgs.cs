using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	public class ResourceRenamedEventArgs : ResourceEventArgs
	{
		private	string	oldPath;

		public string OldPath
		{
			get { return this.oldPath; }
		}
		public ContentRef<Resource> OldContent
		{
			get { return this.IsDirectory ? null : new ContentRef<Resource>(null, this.oldPath); }
		}

		public ResourceRenamedEventArgs(string path, string oldPath, bool isDirectory) : base(path, isDirectory)
		{
			this.oldPath = oldPath;
		}
	}
}
