using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Describes an interface for a <see cref="Component"/> that renders a sprite.
	/// </summary>
	public interface ICmpSpriteRenderer : ICmpRenderer
	{
		/// <summary>
		/// [GET / SET] A color by which the rendered sprite is tinted.
		/// </summary>
		ColorRgba ColorTint { get; set; }
		/// <summary>
		/// [GET / SET] The sprite index or blend of sprite indices that
		/// will be rendered.
		/// </summary>
		SpriteIndexBlend SpriteIndex { get; set; }
	}
}
