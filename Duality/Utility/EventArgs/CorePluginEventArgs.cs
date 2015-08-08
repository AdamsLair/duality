using System;

namespace Duality
{
	/// <summary>
	/// Provides event arguments related to <see cref="Duality.CorePlugin"/> instances.
	/// </summary>
	public class CorePluginEventArgs : EventArgs
	{
		private	CorePlugin	plugin;
		public CorePlugin Plugin
		{
			get { return this.plugin; }
		}
		public CorePluginEventArgs(CorePlugin plugin)
		{
			this.plugin = plugin;
		}
	}
}
