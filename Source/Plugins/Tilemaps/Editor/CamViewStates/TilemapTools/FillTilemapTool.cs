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
	/// Flood-fills the selected tile patch with the source pattern.
	/// </summary>
	public class FillTilemapTool : TilemapTool
	{
		private readonly TilemapFloodFill floodFill = new TilemapFloodFill();


		public override string Name
		{
			get { return TilemapsRes.ItemName_TileFill; }
		}
		public override Image Icon
		{
			get { return TilemapsResCache.IconTileFill; }
		}
		public override Cursor ActionCursor
		{
			get { return TilemapsResCache.CursorTileFill; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.T; }
		}
		public override bool FadeInPreviews
		{
			get { return true; }
		}
		public override int SortOrder
		{
			get { return 4; }
		}


		public override void UpdatePreview()
		{
			ITilemapToolEnvironment env = this.Environment;

			// Don't update flood fill when hovering a tile that's out of range of the tilemap
			if (env.HoveredTile.X < 0 ||
				env.HoveredTile.Y < 0 ||
				env.HoveredTile.X >= env.ActiveTilemap.Size.X ||
				env.HoveredTile.Y >= env.ActiveTilemap.Size.Y)
				return;

			// Don't update flood fill when still hovering the same tile
			if (env.ActiveOrigin == env.HoveredTile)
				return;

			// Don't update flood fill when still inside the previous flood fill area
			Point2 activeLocalHover = new Point2(
				env.HoveredTile.X - env.ActiveOrigin.X,
				env.HoveredTile.Y - env.ActiveOrigin.Y);
			bool hoverInsideActiveRect = (
				activeLocalHover.X >= 0 &&
				activeLocalHover.Y >= 0 &&
				activeLocalHover.X < env.ActiveArea.Width &&
				activeLocalHover.Y < env.ActiveArea.Height);
			bool hoverInsideActiveArea = (hoverInsideActiveRect && env.ActiveArea[activeLocalHover.X, activeLocalHover.Y]);
			if (hoverInsideActiveArea)
				return;

			// Run the flood fill algorithm
			Point2 activeOrigin = env.ActiveOrigin;
			bool previewValid = this.floodFill.GetFillArea(env.ActiveTilemap, env.HoveredTile, true, env.ActiveArea, ref activeOrigin);
			env.ActiveOrigin = activeOrigin;
			if (!previewValid)
			{
				env.ActiveOrigin = env.HoveredTile;
				env.ActiveArea.ResizeClear(1, 1);
				env.ActiveArea[0, 0] = true;
			}
			env.ActiveAreaOutlines.Clear();
			env.SubmitActiveAreaChanges(previewValid);
		}

		public override void UpdateActiveArea()
		{
			base.UpdateActiveArea();
			ITilemapToolEnvironment env = this.Environment;

			Point2 activeOrigin = env.ActiveOrigin;
			this.floodFill.GetFillArea(env.ActiveTilemap, activeOrigin, false, env.ActiveArea, ref activeOrigin);
			env.ActiveOrigin = activeOrigin;
			env.ActiveAreaOutlines.Clear();
			env.SubmitActiveAreaChanges(true);
		}

		public override void BeginAction()
		{
			base.BeginAction();

			// Fill the determined area
			this.Environment.PerformEditTiles(
				EditTilemapActionType.FloodFill,
				this.Environment.ActiveTilemap,
				this.Environment.ActiveOrigin,
				this.Environment.ActiveArea,
				this.Environment.TileDrawSource,
				Point2.Zero);

			// Clear our buffered fill tool state / invalidate the preview
			this.Environment.ActiveOrigin = new Point2(-1, -1);
			this.Environment.ActiveArea.Clear();
			this.Environment.ActiveAreaOutlines.Clear();
			this.Environment.SubmitActiveAreaChanges(true);
		}
	}
}