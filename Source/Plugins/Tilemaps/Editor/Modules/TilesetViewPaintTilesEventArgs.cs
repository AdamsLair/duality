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
	/// Event arguments that provide data about a set of painted tiles within a <see cref="TilesetView"/>.
	/// </summary>
	public class TilesetViewPaintTilesEventArgs : PaintEventArgs
	{
		private Tileset tileset;
		private Bitmap sourceImage;
		private RawList<TilesetViewPaintTileData> paintedTiles;

		/// <summary>
		/// [GET] The <see cref="Tileset"/> that is currently being painted.
		/// </summary>
		public Tileset Tileset
		{
			get { return this.tileset; }
		}
		/// <summary>
		/// [GET] A <see cref="Bitmap"/> that has been generated from the <see cref="Tileset"/>, which
		/// should be used as a(n atlas) source image for drawing individual tiles.
		/// </summary>
		public Bitmap SourceImage
		{
			get { return this.sourceImage; }
		}
		/// <summary>
		/// [GET] Data about the tiles that should be painted within the <see cref="TilesetView"/>.
		/// </summary>
		public IReadOnlyList<TilesetViewPaintTileData> PaintedTiles
		{
			get { return this.paintedTiles; }
		}

		public TilesetViewPaintTilesEventArgs(
			Graphics graphics, 
			Rectangle clipRect,
			Tileset tileset,
			Bitmap sourceImage, 
			RawList<TilesetViewPaintTileData> paintedTiles)
			: base(graphics, clipRect)
		{
			this.tileset = tileset;
			this.sourceImage = sourceImage;
			this.paintedTiles = paintedTiles;
		}
	}
}
