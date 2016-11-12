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
		private Point2 lastHoveredTile = Point2.Zero;
		private Grid<bool> drawAreaBuffer = new Grid<bool>();

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
			this.lastHoveredTile = this.Environment.HoveredTile;
		}
		public override void UpdateAction()
		{
			base.UpdateAction();
			ITilemapToolEnvironment env = this.Environment;
			Grid<bool> drawArea;
			Point2 drawPos;
			
			// Since the cursor might move faster than one tile per update, we'll need
			// to expand the previewed actived area along its line of movement.
			if (Math.Abs(this.lastHoveredTile.X - env.HoveredTile.X) > 1 ||
				Math.Abs(this.lastHoveredTile.Y - env.HoveredTile.Y) > 1)
			{
				Grid<bool> activeBrush = env.ActiveArea;
				Point2 brushOffset = new Point2(
					env.ActiveOrigin.X - env.HoveredTile.X,
					env.ActiveOrigin.Y - env.HoveredTile.Y);

				// We'll be using a temporary draw buffer to accumulate a mask for our line-filled
				// drawing operation.
				this.drawAreaBuffer.ResizeClear(
					activeBrush.Width + MathF.Abs(env.HoveredTile.X - this.lastHoveredTile.X), 
					activeBrush.Height + MathF.Abs(env.HoveredTile.Y - this.lastHoveredTile.Y));
				drawArea = this.drawAreaBuffer;
				drawPos = new Point2(
					Math.Min(this.lastHoveredTile.X, env.HoveredTile.X) + brushOffset.X,
					Math.Min(this.lastHoveredTile.Y, env.HoveredTile.Y) + brushOffset.Y);
			
				// Determine the vector of movement
				Vector2 tileMoveDelta = (Vector2)env.HoveredTile - (Vector2)lastHoveredTile;
				Vector2 tileMoveDir = tileMoveDelta.Normalized;
				Vector2 tileMoveNormal = tileMoveDir.PerpendicularRight;

				// Project the shape onto the perpendicular axis to that vector, so
				// we can determine brush line width.
				float minPosInMoveShape = 0.0f;
				float maxPosInMoveShape = 0.0f;
				for (int y = 0; y < activeBrush.Height; y++)
				{
					for (int x = 0; x < activeBrush.Width; x++)
					{
						if (activeBrush[x, y])
						{
							Vector2 relativePos = new Vector2(x, y) + brushOffset;
							float projectedPos = Vector2.Dot(tileMoveNormal, relativePos);
							minPosInMoveShape = MathF.Min(minPosInMoveShape, projectedPos - 0.5f);
							maxPosInMoveShape = MathF.Max(maxPosInMoveShape, projectedPos + 0.5f);
						}
					}
				}

				// Fill all the tiles that are within the "movement line"
				for (int y = 0; y < this.drawAreaBuffer.Height; y++)
				{
					for (int x = 0; x < this.drawAreaBuffer.Width; x++)
					{
						Vector2 relativePos = new Vector2(x, y) + drawPos - this.lastHoveredTile;

						float projectedWidthPos = Vector2.Dot(tileMoveNormal, relativePos);
						if (projectedWidthPos < minPosInMoveShape) continue;
						if (projectedWidthPos > maxPosInMoveShape) continue;

						float projectedLengthPos = Vector2.Dot(tileMoveDir, relativePos);
						if (projectedLengthPos < 0.0f) continue;
						if (projectedLengthPos > tileMoveDelta.Length) continue;

						this.drawAreaBuffer[x, y] = true;
					}
				}

				// Insert the brush at the start of the line, i.e. the last position
				activeBrush.CopyTo(
					this.drawAreaBuffer,
					this.lastHoveredTile.X - drawPos.X + brushOffset.X,
					this.lastHoveredTile.Y - drawPos.Y + brushOffset.Y);

				// Insert the brush at the end of the line, i.e. the current position
				activeBrush.CopyTo(
					this.drawAreaBuffer,
					env.ActiveOrigin.X - drawPos.X,
					env.ActiveOrigin.Y - drawPos.Y);
			}
			// Otherwise, we can just draw exactly our movement brush
			else
			{
				drawArea = env.ActiveArea;
				drawPos = env.ActiveOrigin;
			}

			// Perform a tile editing operation using the drawin area and position we calculated
			env.PerformEditTiles(
				EditTilemapActionType.DrawTile, 
				env.ActiveTilemap, 
				drawPos, 
				drawArea, 
				env.TileDrawSource,
				new Point2(
					drawPos.X - env.ActionBeginTile.X,
					drawPos.Y - env.ActionBeginTile.Y));
			this.lastHoveredTile = env.HoveredTile;
		}
	}
}
