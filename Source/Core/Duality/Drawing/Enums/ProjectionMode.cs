using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies the type of projection that is applied when rendering the world.
	/// </summary>
	public enum ProjectionMode
	{
		/// <summary>
		/// Vertices are rendered in world coordinates, but objects appear at the same size
		/// regardless of their distance.
		/// </summary>
		Orthographic,
		/// <summary>
		/// Vertices are rendered in world coordinates and appear bigger or smaller depending
		/// on how far away they are.
		/// </summary>
		Perspective,
		/// <summary>
		/// Vertices are rendered in screen / pixel coordinates where (0, 0) is in the screens upper left. 
		/// Z values are used for sorting drawcalls, but no per-pixel depth testing is done.
		/// </summary>
		Screen
	}
}
