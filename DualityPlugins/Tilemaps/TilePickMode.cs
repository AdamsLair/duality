using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Specifies the desired behavior when picking a tile outside the rendered area.
	/// </summary>
	public enum TilePickMode
	{
		/// <summary>
		/// Negative and out-of-bounds coordinates are returned.
		/// </summary>
		Free,
		/// <summary>
		/// The returned tile coordinates are clamped to the available rendered area.
		/// </summary>
		Clamp,
		/// <summary>
		/// Coordinates outside the rendered area are rejected.
		/// </summary>
		Reject
	}
}
