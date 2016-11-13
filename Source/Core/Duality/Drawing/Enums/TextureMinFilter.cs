using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines which filtering algorithm will be used when displaying the Texture smaller than it is.
	/// </summary>
	public enum TextureMinFilter
	{
		/// <summary>
		/// Point filtering with sharp edges.
		/// </summary>
		Nearest,
		/// <summary>
		/// Linear interpolation.
		/// </summary>
		Linear,
		/// <summary>
		/// Point filtering with sharp edges. Mipmaps will be used, but switch from one to the next instantly.
		/// </summary>
		NearestMipmapNearest,
		/// <summary>
		/// Linear interpolation. Mipmaps will be used, but switch from one to the next instantly.
		/// </summary>
		LinearMipmapNearest,
		/// <summary>
		/// Point filtering with sharp edges. Mipmaps will be used and smoothly blend over from one to the next.
		/// </summary>
		NearestMipmapLinear,
		/// <summary>
		/// Linear interpolation. Mipmaps will be used and smoothly blend over from one to the next.
		/// </summary>
		LinearMipmapLinear,
	}
}
