using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require specific init and shutdown logic.
	/// </summary>
	public interface ICmpInitializable
	{
		/// <summary>
		/// Called in order to initialize the Component in a specific way.
		/// </summary>
		/// <param name="context">The kind of initialization that is intended.</param>
		void OnInit(Component.InitContext context);
		/// <summary>
		/// Called in order to shutdown the Component in a specific way.
		/// </summary>
		/// <param name="context">The kind of shutdown that is intended.</param>
		void OnShutdown(Component.ShutdownContext context);
	}
}
