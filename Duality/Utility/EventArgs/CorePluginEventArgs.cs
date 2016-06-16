using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	[Obsolete("Use DualityPluginEventArgs instead.")]
	public class CorePluginEventArgs : EventArgs
	{
		private CorePlugin[] plugins;
		public IReadOnlyList<CorePlugin> Plugins
		{
			get { return this.plugins; }
		}
		public CorePluginEventArgs(IEnumerable<CorePlugin> plugins)
		{
			this.plugins = 
				(plugins ?? Enumerable.Empty<CorePlugin>())
				.NotNull()
				.Distinct()
				.ToArray();
		}
	}
}
