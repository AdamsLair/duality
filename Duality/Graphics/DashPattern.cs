using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.VertexFormat;
using Duality.ColorFormat;
using Duality.Resources;
using Duality.Cloning;
using ICloneable = Duality.Cloning.ICloneable;

namespace Duality
{
	/// <summary>
	/// Describes a pattern for dashed lines.
	/// </summary>
	public enum DashPattern : uint
	{
		/// <summary>
		/// There is no line at all.
		/// </summary>
		Empty		= 0x0U,

		/// <summary>
		/// A dotted line with a a lot of dots.
		/// Pattern: #_#_#_#_#_#_#_#_#_#_#_#_#_#_#_#_
		/// </summary>
		DotMore		= 0xAAAAAAAAU,
		/// <summary>
		/// A dotted line.
		/// Pattern: #___#___#___#___#___#___#___#___
		/// </summary>
		Dot			= 0x88888888U,
		/// <summary>
		/// A dotted line with less dots.
		/// Pattern: #_______#_______#_______#_______
		/// </summary>
		DotLess		= 0x80808080U,

		/// <summary>
		/// A dashed line with short dashes.
		/// Pattern: ##__##__##__##__##__##__##__##__
		/// </summary>
		DashShort	= 0xCCCCCCCCU,
		/// <summary>
		/// A dashed line.
		/// Pattern: ####____####____####____####____
		/// </summary>
		Dash		= 0xF0F0F0F0U,
		/// <summary>
		/// A dashed line with long dashes.
		/// Pattern: ########________########________
		/// </summary>
		DashLong	= 0xFF00FF00U,

		/// <summary>
		/// A line with alternating dashes and dots.
		/// Pattern: ###__#__###__#__###__#__###__#__
		/// </summary>
		DashDot		= 0xE4E4E4E4U,
		/// <summary>
		/// An alternating line with more dots than dashes.
		/// Pattern: #####___#___#___#####___#___#___
		/// </summary>
		DashDotDot	= 0xF888F888U,
		/// <summary>
		/// An alternating line with more dashes than dots.
		/// Pattern: ####____####____####____#___#___
		/// </summary>
		DashDashDot	= 0xF0F0F088U,

		/// <summary>
		/// The line isn't dashed.
		/// </summary>
		Full		= 0xFFFFFFFFU
	}
}