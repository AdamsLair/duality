using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies the style of a text.
	/// </summary>
	[Flags]
	public enum FontStyle
	{
		/// <summary>
		/// Regular text.
		/// </summary>
		Regular	= 0x0,
		/// <summary>
		/// Bold text.
		/// </summary>
		Bold	= 0x1,
		/// <summary>
		/// Italic text.
		/// </summary>
		Italic	= 0x2
	}
}
