using System;

using Duality.Drawing;

namespace Duality
{
	public class CollectDrawcallEventArgs : EventArgs
	{
		private	IDrawDevice device;
		public IDrawDevice Device
		{
			get { return this.device; }
		}

		public CollectDrawcallEventArgs(IDrawDevice device)
		{
			this.device = device;
		}
	}
}
