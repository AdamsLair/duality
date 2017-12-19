using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Distinguishes between screen (overlay) and world rendering.
	/// </summary>
	public enum RenderMode
	{
		/// <summary>
		/// Rendering things that are in the world, from the point of view of an observer.
		/// This mode is for assembling how the world looks like through the lens of the camera.
		/// </summary>
		World,
		/// <summary>
		/// Rendering a screen space (overlay), things are displayed in screen coordinates and without
		/// any kind of perspective projection or view-dependent transformation.
		/// This mode is like drawing directly on the camera lens.
		/// </summary>
		Screen
	}
}
