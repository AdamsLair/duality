using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that require per-frame updates.
	/// </summary>
	public interface ICmpUpdatable
	{
		/// <summary>
		/// Called once per frame in order to update the Component.
		/// </summary>
		void OnUpdate();
	}
}
