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
	/// Draws the source pattern directly onto the tilemap.
	/// </summary>
	public class BrushTilemapTool : TilemapTool
	{
		public override string Name
		{
			get { return TilemapsRes.ItemName_TileBrush; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTileBrush; }
		}
		public override Cursor ActionCursor
		{
			get { return TilemapsResCache.CursorTileBrush; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.W; }
		}
		public override int SortOrder
		{
			get { return 1; }
		}

		public override void UpdatePreview()
		{
			ITilemapToolEnvironment env = this.Environment;
			IReadOnlyGrid<bool> sourceShape = env.TileDrawSource.SourceShape;

			env.ActiveAreaOutlines.Clear();
			env.ActiveOrigin = new Point2(
				env.HoveredTile.X - sourceShape.Width / 2,
				env.HoveredTile.Y - sourceShape.Height / 2);
			if (sourceShape.Width > 0 && sourceShape.Height > 0)
			{
				env.ActiveArea.ResizeClear(sourceShape.Width, sourceShape.Height);
				for (int y = 0; y < sourceShape.Height; y++)
				{
					for (int x = 0; x < sourceShape.Width; x++)
					{
						env.ActiveArea[x, y] = sourceShape[x, y];
					}
				}
			}
			else
			{
				env.ActiveArea.ResizeClear(1, 1);
				env.ActiveArea[0, 0] = true;
			}
			env.SubmitActiveAreaChanges(true);
		}
		public override void BeginAction()
		{
			base.BeginAction();
			this.Environment.PerformEditTiles(
				EditTilemapActionType.DrawTile, 
				this.Environment.ActiveTilemap, 
				this.Environment.ActiveOrigin, 
				this.Environment.ActiveArea, 
				this.Environment.TileDrawSource,
				new Point2(
					this.Environment.ActiveOrigin.X - this.Environment.ActionBeginTile.X,
					this.Environment.ActiveOrigin.Y - this.Environment.ActionBeginTile.Y));
		}
		public override void UpdateAction()
		{
			base.UpdateAction();
			this.Environment.PerformEditTiles(
				EditTilemapActionType.DrawTile, 
				this.Environment.ActiveTilemap, 
				this.Environment.ActiveOrigin, 
				this.Environment.ActiveArea, 
				this.Environment.TileDrawSource,
				new Point2(
					this.Environment.ActiveOrigin.X - this.Environment.ActionBeginTile.X,
					this.Environment.ActiveOrigin.Y - this.Environment.ActionBeginTile.Y));
		}
	}
}
