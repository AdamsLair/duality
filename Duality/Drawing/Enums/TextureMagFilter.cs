using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines which filtering algorithm will be used when displaying the Texture larger than it is.
	/// </summary>
	public enum TextureMagFilter
	{
		/// <summary>
		/// Point filtering with sharp edges.
		/// </summary>
		Nearest,
		/// <summary>
		/// Linear interpolation.
		/// </summary>
		Linear
	}
}
