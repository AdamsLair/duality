using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies a rendering matrix setup.
	/// </summary>
	public enum RenderMatrix
	{
		/// <summary>
		/// Rendering in world space, things are displayed from the point of view of an observer.
		/// </summary>
		WorldSpace,
		/// <summary>
		/// Rendering in screen space, things are displayed in screen coordinates and without depth.
		/// </summary>
		ScreenSpace
	}
}
