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
	/// Describes a single tile in a compiled <see cref="Tileset"/>.
	/// The <see cref="TileInfo"/> data block is generated during <see cref="Tileset"/> compilation
	/// based on <see cref="TileInput"/> data and <see cref="Tileset.RenderConfig"/> settings.
	/// </summary>
	public struct TileInfo
	{
		/// <summary>
		/// The tile's <see cref="Texture"/> coordinates in the first <see cref="Texture"/> of the <see cref="Tileset"/>.
		/// They are stored here as a measure of performance optimization for quick access - for UV information on
		/// other <see cref="Texture"/> instances of the <see cref="Tileset"/>, use <see cref="Tileset.LookupTileAtlas"/>.
		/// </summary>
		public Rect TexCoord0;
		/// <summary>
		/// The tile's depth offset, measured in tiles.
		/// </summary>
		public int DepthOffset;
		/// <summary>
		/// Specifies whether the tile is standing upright / vertical, as opposed to being flat on its <see cref="Tilemap"/> surface.
		/// </summary>
		public bool IsVertical;
		/// <summary>
		/// Specifies whether the tile can be considered visually empty, e.g. by being completely transparent.
		/// </summary>
		public bool IsVisuallyEmpty;
		/// <summary>
		/// Specifies the per-layer collision shape of this tile.
		/// </summary>
		public TileCollisionShapes Collision;
	}
}
