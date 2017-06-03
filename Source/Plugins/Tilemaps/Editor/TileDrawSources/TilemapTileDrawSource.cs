using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class TilemapTileDrawSource : ITileDrawSource
	{
		private Tilemap    tilemap = null;
		private Point2     origin  = Point2.Zero;
		private Grid<bool> area    = null;
		private Grid<Tile> source  = new Grid<Tile>();


		public Tilemap SourceTilemap
		{
			get { return this.tilemap; }
		}
		public Point2 SourceOrigin
		{
			get { return this.origin; }
		}
		public IReadOnlyGrid<bool> SourceShape
		{
			get { return this.area; }
		}


		public TilemapTileDrawSource(Tilemap tilemap, Point2 origin, Grid<bool> area)
		{
			if (tilemap == null) throw new ArgumentNullException("tilemap");
			this.tilemap = tilemap;
			this.origin = origin;
			this.area = new Grid<bool>(area);
		}

		public void FillTarget(Grid<Tile> target, Point2 offset)
		{
			Point2 sourceSize = new Point2(this.source.Width, this.source.Height);

			// Adjust the offset to positive values only
			if (offset.X < 0) offset.X += (1 + (-offset.X / sourceSize.X)) * sourceSize.X;
			if (offset.Y < 0) offset.Y += (1 + (-offset.Y / sourceSize.Y)) * sourceSize.Y;

			// Apply the source tiles to the target area
			for (int y = 0; y < target.Height; y++)
			{
				for (int x = 0; x < target.Width; x++)
				{
					Point2 wrapped = new Point2(
						(x + offset.X) % sourceSize.X, 
						(y + offset.Y) % sourceSize.Y);

					if (this.area[wrapped.X, wrapped.Y])
						target[x, y] = this.source[wrapped.X, wrapped.Y];
					else
						target[x, y] = new Tile();
				}
			}
		}
		public void BeginAction()
		{
			// Acquire a copy of the source tile rectangle
			this.TakeSourceSnapshot();
		}
		public void EndAction() { }

		private void TakeSourceSnapshot()
		{
			this.source.ResizeClear(this.area.Width, this.area.Height);

			Grid<Tile> tiles = this.tilemap.BeginUpdateTiles();
			tiles.CopyTo(this.source, 0, 0, -1, -1, this.origin.X, this.origin.Y);
			this.tilemap.EndUpdateTiles(0, 0, 0, 0);
		}
	}
}
