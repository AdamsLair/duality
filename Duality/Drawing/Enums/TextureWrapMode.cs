using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines how Texture coordinates outside the regular [0 - 1] range will be handled.
	/// </summary>
	public enum TextureWrapMode
	{
		Clamp,
		Repeat,
		MirroredRepeat,
	}
}
