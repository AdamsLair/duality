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

			env.ActiveAreaOutlines.Clear();
			env.ActiveOrigin = env.HoveredTile;
			env.ActiveArea.ResizeClear(1, 1);
			env.ActiveArea[0, 0] = true;
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
				new Tile { Index = 1 });
		}
		public override void UpdateAction()
		{
			base.UpdateAction();
			this.Environment.PerformEditTiles(
				EditTilemapActionType.DrawTile, 
				this.Environment.ActiveTilemap, 
				this.Environment.ActiveOrigin, 
				this.Environment.ActiveArea, 
				new Tile { Index = 1 });
		}
	}
}
