using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Describes how a rectangular object is resized to fit a target size.
	/// </summary>
	public enum TargetResize
	{
		/// <summary>
		/// No resize takes place.
		/// </summary>
		None,
		/// <summary>
		/// The resize will match the object's width and height exactly with the target size.
		/// </summary>
		Stretch,
		/// <summary>
		/// The resize will scale the object so it fits inside the target rect, while keeping its aspect ratio.
		/// </summary>
		Fit,
		/// <summary>
		/// The resize will scale the object so the entire target rect fits inside, while keeping its aspect ratio.
		/// </summary>
		Fill
	}
}
