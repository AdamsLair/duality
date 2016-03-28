using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Plugins.Tilemaps;

namespace Duality.Editor.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single painted tile in the paint event of a <see cref="TilesetView"/>.
	/// </summary>
	public struct TilesetViewPaintTileData
	{
		/// <summary>
		/// The index that identifies the painted tile within the <see cref="Tileset"/>.
		/// </summary>
		public int TileIndex;
		/// <summary>
		/// The rectangle that holds the tile within its <see cref="Tileset"/> source image.
		/// </summary>
		public Rectangle SourceRect;
		/// <summary>
		/// The rectangle within the <see cref="TilesetView"/> to which the tile is painted.
		/// </summary>
		public Rectangle ViewRect;
	}
}
