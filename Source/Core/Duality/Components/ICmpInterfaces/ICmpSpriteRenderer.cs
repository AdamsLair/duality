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
		/// [GET / SET] The sprite index that will be rendered.
		/// </summary>
		int SpriteIndex { get; set; }

		/// <summary>
		/// Applies the a new sprite index to the <see cref="ICmpSpriteRenderer"/> while providing
		/// additional information about the overall state of the animation.
		/// </summary>
		/// <param name="currentSpriteIndex">The current sprite index that should be displayed right now.</param>
		/// <param name="nextSpriteIndex">The next sprite index that will be displayed.</param>
		/// <param name="progressToNext">
		/// A factor from zero to one that indicates how far the sprite animation has advanced from the current sprite index to the next one.
		/// </param>
		void ApplySpriteAnimation(int currentSpriteIndex, int nextSpriteIndex, float progressToNext);
	}
}
