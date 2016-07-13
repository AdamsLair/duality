using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Plugins.Tilemaps;


namespace Duality.Editor.Plugins.Tilemaps
{
	public class DummyTileDrawSource : ITileDrawSource
	{
		private static readonly Grid<bool> EmptyShape = new Grid<bool>();

		public Tilemap SourceTilemap
		{
			get { return null; }
		}
		public Point2 SourceOrigin
		{
			get { return new Point2(); }
		}
		public IReadOnlyGrid<bool> SourceShape
		{
			get { return EmptyShape; }
		}

		public void FillTarget(Grid<Tile> target, Point2 offset)
		{
			target.Fill(new Tile(), 0, 0, target.Width, target.Height);
		}
		public void BeginAction() { }
		public void EndAction() { }
	}
}
