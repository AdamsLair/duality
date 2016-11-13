using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;

namespace Duality.Editor
{
	/// <summary>
	/// Defines the reason why a selection change is occurring.
	/// </summary>
	public enum SelectionChangeReason
	{
		/// <summary>
		/// The exact reason for this selection change can't be determined
		/// or doesn't match any of the specified reasons.
		/// </summary>
		Unknown,

		/// <summary>
		/// The selection has changed to reflect previous or current user input.
		/// </summary>
		UserInput,
		/// <summary>
		/// The selection has changed because some or all of the selected objects
		/// have been or are being disposed and are no longer a valid selection target.
		/// </summary>
		ObjectDisposing
	}
}
