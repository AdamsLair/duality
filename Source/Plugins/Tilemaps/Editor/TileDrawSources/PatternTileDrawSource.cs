using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class PatternTileDrawSource : ITileDrawSource
	{
		private Grid<bool> shape   = null;
		private Grid<Tile> pattern = null;


		public Tilemap SourceTilemap
		{
			get { return null; }
		}
		public Point2 SourceOrigin
		{
			get { return Point2.Zero; }
		}
		public IReadOnlyGrid<bool> SourceShape
		{
			get { return this.shape; }
		}


		public PatternTileDrawSource() : this(new Grid<bool>(), new Grid<Tile>()) { }
		public PatternTileDrawSource(Grid<bool> shape, Grid<Tile> pattern)
		{
			this.SetData(shape, pattern);
		}

		public void SetData(Grid<bool> shape, Grid<Tile> pattern)
		{
			this.pattern = new Grid<Tile>(pattern);
			this.shape = new Grid<bool>(shape);
		}
		public void FillTarget(Grid<Tile> target, Point2 offset)
		{
			Point2 patternSize = new Point2(this.pattern.Width, this.pattern.Height);

			// Adjust the offset to positive values only
			if (offset.X < 0) offset.X += (1 + (-offset.X / patternSize.X)) * patternSize.X;
			if (offset.Y < 0) offset.Y += (1 + (-offset.Y / patternSize.Y)) * patternSize.Y;

			// Apply the source tiles to the target area
			for (int y = 0; y < target.Height; y++)
			{
				for (int x = 0; x < target.Width; x++)
				{
					Point2 wrapped = new Point2(
						(x + offset.X) % patternSize.X, 
						(y + offset.Y) % patternSize.Y);

					if (this.shape[wrapped.X, wrapped.Y])
						target[x, y] = this.pattern[wrapped.X, wrapped.Y];
					else
						target[x, y] = new Tile();
				}
			}
		}
		public void BeginAction() { }
		public void EndAction() { }
	}
}
