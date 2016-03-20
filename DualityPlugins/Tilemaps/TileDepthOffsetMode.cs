using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Specifies the way in which depth offsets are generated per-tile.
	/// </summary>
	public enum TileDepthOffsetMode
	{
		/// <summary>
		/// All tiles share the same depth offset.
		/// </summary>
		Flat,
		/// <summary>
		/// A tile's depth offset is derived from its local position in the tilemap.
		/// </summary>
		Local,
		/// <summary>
		/// A tile's depth offset is derived from its world-space position.
		/// </summary>
		World
	}
}
