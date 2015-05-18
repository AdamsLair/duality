using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.Resources;
using Duality.Editor;
using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Implement this interface in C<see cref="Component">Components</see> that require per-frame updates in the editor.
	/// </summary>
	public interface ICmpEditorUpdatable
	{
		/// <summary>
		/// Called once per frame in order to update the Component in the editor.
		/// </summary>
		void OnUpdate();
	}
}
