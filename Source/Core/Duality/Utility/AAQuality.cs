using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	/// <summary>
	/// Specifies the quality level of the anti-aliasing used for rendering.
	/// </summary>
	public enum AAQuality
	{
		/// <summary>
		/// Highest possible quality. Sacrifices performance for smooth edges. Can be a problem on older machines.
		/// </summary>
		High,
		/// <summary>
		/// Medium quality. A tradeoff between looks and Profile.
		/// </summary>
		Medium,
		/// <summary>
		/// Low quality. Favors Profile.
		/// </summary>
		Low,
		/// <summary>
		/// No hardware anti-aliasing is used at all.
		/// </summary>
		Off
	}
}
