using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Duality;

namespace Duality.Editor
{
	/// <summary>
	/// Provides an interface for GUI elements that provide help
	/// information according to the current location of the mouse.
	/// </summary>
	public interface IHelpProvider
	{
		/// <summary>
		/// Determines the <see cref="HelpInfo"/> to provide for
		/// the given location withing the control.
		/// </summary>
		/// <param name="localPos">The location in the controls local coordinates of the mouse</param>
		/// <param name="captured">
		/// Should be set to true if this <see cref="IHelpProvider"/> has
		/// captured the input and ancestor <see cref="IHelpProvider"/>s should not be checked
		/// </param>
		/// <returns>
		/// The <see cref="HelpInfo"/> object describing the help information
		/// or null if no help information is available
		/// </returns>
		HelpInfo ProvideHoverHelp(Point localPos, ref bool captured);
	}
}
