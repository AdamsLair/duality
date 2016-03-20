using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;
using Duality.Resources;
using Duality.Editor;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single tile input in a <see cref="Tileset"/>.
	/// When compiling a <see cref="Tileset"/>, these inputs are transformed into
	/// a <see cref="TileInfo"/> data block which can then be used by <see cref="Tilemap"/>
	/// Components and others.
	/// </summary>
	public struct TileInput
	{
		/// <summary>
		/// The tile's depth offset, measured in tiles.
		/// </summary>
		public int DepthOffset;
		/// <summary>
		/// Whether the tile is standing upright / vertical, as opposed to being flat on its <see cref="Tilemap"/> surface.
		/// </summary>
		public bool IsVertical;
	}
}
