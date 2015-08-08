using System;

namespace Duality
{
	public class ResourceResolveEventArgs : EventArgs
	{
		private	string		requestedContent;
		private	Resource	result;

		public string RequestedContent
		{
			get { return this.requestedContent; }
		}
		public bool Handled
		{
			get { return this.result != null && !this.result.Disposed; }
		}
		public Resource Result
		{
			get { return this.result; }
		}

		public ResourceResolveEventArgs(string path)
		{
			this.requestedContent = path;
		}

		public void Resolve(Resource result)
		{
			if (result == null) throw new ArgumentNullException("result");
			if (result.Disposed) throw new ObjectDisposedException("result");
			this.result = result;
		}
	}
}
