using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Resources;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines how a Texture should handle pixel data without power-of-two dimensions.
	/// </summary>
	public enum TextureSizeMode
	{
		/// <summary>
		/// Enlarges the images dimensions without scaling the image, leaving
		/// the new space empty. Texture coordinates are automatically adjusted in
		/// order to display the image correctly. This preserves the images full
		/// quality but prevents tiling, if not power-of-two anyway.
		/// </summary>
		Enlarge,
		/// <summary>
		/// Stretches the image to fit power-of-two dimensions and downscales it
		/// again when displaying. This might blur the image slightly but allows
		/// tiling it.
		/// </summary>
		Stretch,
		/// <summary>
		/// The images dimensions are not affected, as OpenGL uses an actual 
		/// non-power-of-two texture. However, this might be unsupported on older hardware.
		/// </summary>
		NonPowerOfTwo,

		/// <summary>
		/// The default behaviour. Equals <see cref="Enlarge"/>.
		/// </summary>
		Default = Enlarge
	}
}
