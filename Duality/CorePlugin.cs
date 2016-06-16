using System;
using System.Linq;
using System.Reflection;

namespace Duality
{
	public abstract class CorePlugin : DualityPlugin
	{
		/// <summary>
		/// Called when initializing the plugin. It is guaranteed that all plugins have been loaded at this point, so
		/// this is the ideal place to establish communication with other plugins or load Resources that may rely on them.
		/// It is NOT defined whether or not other plugins have been initialized yet.
		/// </summary>
		internal protected virtual void InitPlugin() {}
		/// <summary>
		/// Called before Duality updates the game scene
		/// </summary>
		internal protected virtual void OnBeforeUpdate() {}
		/// <summary>
		/// Called after Duality updates the game scene
		/// </summary>
		internal protected virtual void OnAfterUpdate() {}
		/// <summary>
		/// Called when Dualitys <see cref="DualityApp.ExecutionContext"/> changes.
		/// </summary>
		internal protected virtual void OnExecContextChanged(DualityApp.ExecutionContext previousContext) {}
	}
}
