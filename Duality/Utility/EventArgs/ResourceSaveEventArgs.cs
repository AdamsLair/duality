using System;

namespace Duality
{
	public class ResourceSaveEventArgs : ResourceEventArgs
	{
		private	string	saveAsPath;

		public string SaveAsPath
		{
			get { return this.saveAsPath; }
		}
		
		public ResourceSaveEventArgs(string path, string saveAsPath) : this(new ContentRef<Resource>(null, path), saveAsPath) {}
		public ResourceSaveEventArgs(IContentRef resRef, string saveAsPath) : this(new ContentRef<Resource>(resRef.ResWeak, resRef.Path), saveAsPath) {}
		public ResourceSaveEventArgs(ContentRef<Resource> resRef, string saveAsPath) : base(resRef)
		{
			this.saveAsPath = saveAsPath;
		}
	}
}
