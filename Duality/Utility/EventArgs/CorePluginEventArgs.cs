using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Provides event arguments related to <see cref="Duality.CorePlugin"/> instances.
	/// </summary>
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
