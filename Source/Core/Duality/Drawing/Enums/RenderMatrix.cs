using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Distinguishes between screen space / overlay and world space rendering.
	/// </summary>
	public enum RenderMatrix
	{
		/// <summary>
		/// Rendering things that are in the world, from the point of view of an observer.
		/// </summary>
		WorldSpace,
		/// <summary>
		/// Rendering a screen space overlay, things are displayed in screen coordinates and without depth.
		/// This mode is like drawing directly on the camera lens.
		/// </summary>
		ScreenSpace
	}
}
