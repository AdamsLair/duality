using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;
using Duality.Editor.Plugins.Tilemaps.Properties;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Fills all tiles in the selected rectangular area with the source pattern.
	/// </summary>
	public class RectTilemapTool : TilemapTool
	{
		public override string Name
		{
			get { return TilemapsRes.ItemName_TileRect; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTileRect; }
		}
		public override Cursor ActionCursor
		{
			get { return TilemapsResCache.CursorTileRect; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.E; }
		}
		public override int SortOrder
		{
			get { return 2; }
		}

		public override void UpdatePreview()
		{
			ITilemapToolEnvironment env = this.Environment;
			Point2 topLeft = new Point2(
				Math.Min(env.ActionBeginTile.X, env.HoveredTile.X),
				Math.Min(env.ActionBeginTile.Y, env.HoveredTile.Y));
			Point2 size = new Point2(
				1 + Math.Abs(env.ActionBeginTile.X - env.HoveredTile.X),
				1 + Math.Abs(env.ActionBeginTile.Y - env.HoveredTile.Y));

			// Don't update the rect when still using the same boundary size
			bool hoverInsideActiveArea = (env.ActiveOrigin == topLeft && env.ActiveArea.Width == size.X && env.ActiveArea.Height == size.Y);
			if (hoverInsideActiveArea)
				return;

			env.ActiveOrigin = topLeft;
			env.ActiveArea.ResizeClear(size.X, size.Y);
			env.ActiveArea.Fill(true, 0, 0, size.X, size.Y);

			// Manually define outlines in the trivial rect case
			Tileset tileset = env.ActiveTilemap != null ? env.ActiveTilemap.Tileset.Res : null;
			Vector2 tileSize = tileset != null ? tileset.TileSize : Tileset.DefaultTileSize;
			env.ActiveAreaOutlines.Clear();
			env.ActiveAreaOutlines.Add(new Vector2[]
			{
				new Vector2(0, 0),
				new Vector2(tileSize.X * size.X, 0),
				new Vector2(tileSize.X * size.X, tileSize.Y * size.Y),
				new Vector2(0, tileSize.Y * size.Y),
				new Vector2(0, 0) // Close the loop
			});

			env.SubmitActiveAreaChanges(true);
		}
		public override void EndAction()
		{
			base.EndAction();
			this.Environment.PerformEditTiles(
				EditTilemapActionType.FillRect, 
				this.Environment.ActiveTilemap, 
				this.Environment.ActiveOrigin, 
				this.Environment.ActiveArea, 
				this.Environment.TileDrawSource,
				Point2.Zero);
		}
	}
}
