using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Duality;
using Duality.Plugins.Tilemaps;
using Duality.Editor.Plugins.Tilemaps.Properties;
using Duality.Editor.Plugins.Tilemaps.UndoRedoActions;


namespace Duality.Editor.Plugins.Tilemaps.CamViewStates
{
	/// <summary>
	/// Fills all tiles in the selected oval area with the source pattern.
	/// </summary>
	public class OvalTilemapTool : TilemapTool
	{
		public override string Name
		{
			get { return TilemapsRes.ItemName_TileOval; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTileOval; }
		}
		public override Cursor ActionCursor
		{
			get { return TilemapsResCache.CursorTileOval; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.R; }
		}
		public override int SortOrder
		{
			get { return 3; }
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

			Vector2 radius = (Vector2)size * 0.5f;
			Vector2 offset = new Vector2(0.5f, 0.5f) - radius;

			// Adjust to receive nicer low-res shapes
			radius.X -= 0.1f;
			radius.Y -= 0.1f;
			
			env.ActiveOrigin = topLeft;
			env.ActiveArea.ResizeClear(size.X, size.Y);
			for (int y = 0; y < size.Y; y++)
			{
				for (int x = 0; x < size.X; x++)
				{
					Vector2 relative = new Vector2(x, y) + offset;
					env.ActiveArea[x, y] = 
						((relative.X * relative.X) / (radius.X * radius.X)) + 
						((relative.Y * relative.Y) / (radius.Y * radius.Y)) <= 1.0f;
				}
			}

			env.ActiveAreaOutlines.Clear();
			env.SubmitActiveAreaChanges(true);
		}
		public override void EndAction()
		{
			base.EndAction();
			this.Environment.PerformEditTiles(
				EditTilemapActionType.FillOval, 
				this.Environment.ActiveTilemap, 
				this.Environment.ActiveOrigin, 
				this.Environment.ActiveArea, 
				this.Environment.TileDrawSource,
				Point2.Zero);
		}
	}
}
