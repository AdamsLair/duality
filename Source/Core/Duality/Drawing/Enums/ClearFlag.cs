using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// A Bitmask describing which components of the current rendering buffer to clear.
	/// </summary>
	[Flags]
	public enum ClearFlag
	{
		/// <summary>
		/// Nothing.
		/// </summary>
		None	= 0x0,

		/// <summary>
		/// The buffers color components.
		/// </summary>
		Color	= 0x1,
		/// <summary>
		/// The buffers depth component.
		/// </summary>
		Depth	= 0x2,

		/// <summary>
		/// The default set of flags.
		/// </summary>
		Default	= Color | Depth,
		/// <summary>
		/// All flags set.
		/// </summary>
		All		= Color | Depth
	}
}
