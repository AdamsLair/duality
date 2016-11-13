using System;

namespace Duality
{
	public class ResourceEventArgs : EventArgs
	{
		private ContentRef<Resource>	content;
		private	bool					isDirectory;
		private	bool					isResource;

		public string Path
		{
			get { return this.content.Path; }
		}
		public bool IsDirectory
		{
			get { return this.isDirectory; }
		}
		public bool IsResource
		{
			get { return this.isResource; }
		}
		public bool IsDefaultContent
		{
			get { return this.content.IsDefaultContent; }
		}
		public Type ContentType
		{
			get 
			{
				if (isDirectory) return null;
				else return this.content.ResType;
			}
		}
		public ContentRef<Resource> Content
		{
			get { return this.isDirectory ? null : this.content; }
		}

		public ResourceEventArgs(string path, bool isDirectory) : this(new ContentRef<Resource>(null, path), isDirectory) {}
		public ResourceEventArgs(ContentRef<Resource> resRef, bool isDirectory = false)
		{
			this.content = resRef;
			this.isResource = !isDirectory;
			this.isDirectory = isDirectory;
		}
	}
}
