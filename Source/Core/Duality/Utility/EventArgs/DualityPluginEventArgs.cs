using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Provides event arguments related to <see cref="Duality.CorePlugin"/> instances.
	/// </summary>
	public class DualityPluginEventArgs : EventArgs
	{
		private DualityPlugin[] plugins;
		public IReadOnlyList<DualityPlugin> Plugins
		{
			get { return this.plugins; }
		}
		public DualityPluginEventArgs(IEnumerable<DualityPlugin> plugins)
		{
			this.plugins = 
				(plugins ?? Enumerable.Empty<DualityPlugin>())
				.NotNull()
				.Distinct()
				.ToArray();
		}
	}
}
