using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Specifies the perspective effect that is applied when rendering the world.
	/// </summary>
	public enum PerspectiveMode
	{
		/// <summary>
		/// No perspective effect is applied. Z points into the screen and is only used for object sorting.
		/// </summary>
		Flat,
		/// <summary>
		/// Objects that are far away appear smaller. Z points into the screen and is used for scaling and sorting.
		/// </summary>
		Parallax
	}
}
