using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines the format that is used to store the Textures pixel data.
	/// </summary>
	public enum TexturePixelFormat
	{
		Single,
		Dual,
		Rgb,
		Rgba,

		FloatSingle,
		FloatDual,
		FloatRgb,
		FloatRgba,

		CompressedSingle,
		CompressedDual,
		CompressedRgb,
		CompressedRgba
	}
}
