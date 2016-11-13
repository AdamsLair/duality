using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a filtering method for rescaling images.
	/// </summary>
	public enum ImageScaleFilter
	{
		/// <summary>
		/// Nearest neighbor filterting / No interpolation.
		/// </summary>
		Nearest,
		/// <summary>
		/// Linear interpolation.
		/// </summary>
		Linear
	}
}
