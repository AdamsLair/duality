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
		private Grid<bool> activeFillBuffer = new Grid<bool>();
		
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
			bool previewValid = this.GetFloodFillArea(env.ActiveTilemap, env.HoveredTile, true, env.ActiveArea, ref activeOrigin);
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
			this.GetFloodFillArea(env.ActiveTilemap, activeOrigin, false, env.ActiveArea, ref activeOrigin);
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

		/// <summary>
		/// Runs the flood fill algorithm on the specified position and writes the result into the specified variables.
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="pos"></param>
		/// <param name="preview">If true, the algorithm will cancel when taking too long for an interactive preview.</param>
		/// <param name="floodFillArea"></param>
		/// <param name="floodFillOrigin"></param>
		/// <returns>True, if the algorithm completed. False, if it was canceled.</returns>
		private bool GetFloodFillArea(Tilemap tilemap, Point2 pos, bool preview, Grid<bool> floodFillArea, ref Point2 floodFillOrigin)
		{
			Grid<Tile> tiles = tilemap.BeginUpdateTiles();
			Point2 fillTopLeft;
			Point2 fillSize;
			bool success = FloodFillTiles(ref this.activeFillBuffer, tiles, pos, preview ? (128 * 128) : 0, out fillTopLeft, out fillSize);
			tilemap.EndUpdateTiles(0, 0, 0, 0);

			// Find the filled areas boundaries and copy it to the active area
			if (success)
			{
				floodFillOrigin = fillTopLeft;
				floodFillArea.ResizeClear(fillSize.X, fillSize.Y);
				this.activeFillBuffer.CopyTo(floodFillArea, 0, 0, -1, -1, floodFillOrigin.X, floodFillOrigin.Y);
			}

			return success;
		}
		
		/// <summary>
		/// Performs a flood fill operation originating from the specified position. 
		/// <see cref="Tile"/> equality is checked in the <see cref="_FloodFill_TilesEqual"/> method.
		/// </summary>
		/// <param name="fillBuffer">A buffer that will be filled with the result of the flood fill operation.</param>
		/// <param name="tiles"></param>
		/// <param name="pos"></param>
		/// <param name="maxTileCount">The maximum number of tiles to fill. If the filled tile count exceeds this number, the algorithm is canceled.</param>
		/// <param name="fillAreaSize"></param>
		/// <param name="fillAreaTopLeft"></param>
		/// <returns>True when successful. False when aborted.</returns>
		private static bool FloodFillTiles(ref Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, int maxTileCount, out Point2 fillAreaTopLeft, out Point2 fillAreaSize)
		{
			// ## Note: ##
			// This flood fill algorithm is a modified version of "A More Efficient Flood Fill" by Adam Milazzo.
			// All credit for the original idea and sample implementation goes to him. Last seen on the web here:
			// http://adammil.net/blog/v126_A_More_Efficient_Flood_Fill.html
			// ###########

			// Initialize fill buffer
			if (fillBuffer == null)
				fillBuffer = new Grid<bool>(tiles.Width, tiles.Height);
			else if (fillBuffer.Width != tiles.Width || fillBuffer.Height != tiles.Height)
				fillBuffer.ResizeClear(tiles.Width, tiles.Height);
			else
				fillBuffer.Clear();

			// Get the base tile for comparison
			Tile baseTile = tiles[pos.X, pos.Y];

			// Find the topleft-most tile to start with
			fillAreaTopLeft = _FloodFillTiles_FindTopLeft(fillBuffer, tiles, pos, baseTile);
			fillAreaSize = new Point2(1 + pos.X - fillAreaTopLeft.X, 1 + pos.Y - fillAreaTopLeft.Y);
			pos = fillAreaTopLeft;

			// Run the main part of the algorithm
			if (maxTileCount <= 0) maxTileCount = int.MaxValue;
			bool success = _FloodFillTiles(fillBuffer, tiles, pos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize);

			return success;
		}
		private static bool _FloodFillTiles(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile, ref int maxTileCount, ref Point2 fillAreaTopLeft, ref Point2 fillAreaSize)
		{
			// Adjust the fill area to the position we'll be starting to fill
			fillAreaSize.X += Math.Max(fillAreaTopLeft.X - pos.X, 0);
			fillAreaSize.Y += Math.Max(fillAreaTopLeft.Y - pos.Y, 0);
			fillAreaTopLeft.X = Math.Min(fillAreaTopLeft.X, pos.X);
			fillAreaTopLeft.Y = Math.Min(fillAreaTopLeft.Y, pos.Y);

			// Since the top and left of the current tile are blocking the fill operation, proceed down and right
			int lastRowLength = 0;
			do
			{
				Point2 rowPos = pos;
				int rowLength = 0;
				bool firstRow = (lastRowLength == 0);

				// Narrow scan line width on the left when necessary
				if (!firstRow && !_FloodFill_IsCandidate(fillBuffer, tiles, pos, baseTile))
				{
					while (lastRowLength > 1)
					{
						pos.X++;
						lastRowLength--;

						if (_FloodFill_IsCandidate(fillBuffer, tiles, pos, baseTile))
							break;
					}

					rowPos.X = pos.X;
				}
				// Expand scan line width to the left when necessary
				else
				{
					for (; pos.X != 0 && _FloodFill_IsCandidate(fillBuffer, tiles, new Point2(pos.X - 1, pos.Y), baseTile); rowLength++, lastRowLength++)
					{
						pos.X--;
						fillBuffer[pos.X, pos.Y] = true;

						// Adjust the fill area
						fillAreaSize.X += Math.Max(fillAreaTopLeft.X - pos.X, 0);
						fillAreaTopLeft.X = Math.Min(fillAreaTopLeft.X, pos.X);

						// If something above the current scan line is free, handle it recursively
						if (pos.Y != 0 && _FloodFill_IsCandidate(fillBuffer, tiles, new Point2(pos.X, pos.Y - 1), baseTile))
						{
							// Find the topleft-most tile to start with
							Point2 targetPos = new Point2(pos.X, pos.Y - 1);
							targetPos = _FloodFillTiles_FindTopLeft(fillBuffer, tiles, targetPos, baseTile);
							if (!_FloodFillTiles(fillBuffer, tiles, targetPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
								return false;
						}
					}
				}
				
				// Fill the current row
				for (; rowPos.X < tiles.Width && _FloodFill_IsCandidate(fillBuffer, tiles, rowPos, baseTile); rowLength++, rowPos.X++)
				{
					fillBuffer[rowPos.X, rowPos.Y] = true;
				}
				maxTileCount -= rowLength;
				if (maxTileCount < 0) return false;

				// Adjust the fill area
				fillAreaSize.X = Math.Max(fillAreaSize.X, rowPos.X - fillAreaTopLeft.X);

				// If the current row is shorter than the previous, see if there are 
				// disconnected pixels below the (filled) previous row left to handle
				if (rowLength < lastRowLength)
				{
					for (int end = pos.X + lastRowLength; ++rowPos.X < end; )
					{
						// Recursively handle the disconnected below-bottom pixels of the last row
						if (_FloodFill_IsCandidate(fillBuffer, tiles, rowPos, baseTile))
						{
							if (!_FloodFillTiles(fillBuffer, tiles, rowPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
								return false;
						}
					}
				}
				// If the current row is longer than the previous, see if there are 
				// top pixels above this one that are disconnected from the last row
				else if (rowLength > lastRowLength && pos.Y != 0)
				{
					for (int prevRowX = pos.X + lastRowLength; ++prevRowX < rowPos.X; )
					{
						// Recursively handle the disconnected pixels of the last row
						if (_FloodFill_IsCandidate(fillBuffer, tiles, new Point2(prevRowX, pos.Y - 1), baseTile))
						{
							// Find the topleft-most tile to start with
							Point2 targetPos = new Point2(prevRowX, pos.Y - 1);
							targetPos = _FloodFillTiles_FindTopLeft(fillBuffer, tiles, targetPos, baseTile);
							if (!_FloodFillTiles(fillBuffer, tiles, targetPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
								return false;
						}
					}
				}

				lastRowLength = rowLength;
				pos.Y++;

				// Adjust the fill area
				fillAreaSize.Y = Math.Max(fillAreaSize.Y, pos.Y - fillAreaTopLeft.Y);
			}
			while (lastRowLength != 0 && pos.Y < tiles.Height);

			return true;
		}
		private static Point2 _FloodFillTiles_FindTopLeft(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile)
		{
			// Find the topleft-most connected matching tile
			while(true)
			{
				Point2 origin = pos;
				while (pos.Y != 0 && _FloodFill_IsCandidate(fillBuffer, tiles, new Point2(pos.X, pos.Y - 1), baseTile)) pos.Y--;
				while (pos.X != 0 && _FloodFill_IsCandidate(fillBuffer, tiles, new Point2(pos.X - 1, pos.Y), baseTile)) pos.X--;
				if (pos == origin) break;
			}
			return pos;
		}
		private static bool _FloodFill_TilesEqual(Tile baseTile, Tile otherTile)
		{
			return baseTile.Index == otherTile.Index;
		}
		private static bool _FloodFill_IsCandidate(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile)
		{
			return !fillBuffer[pos.X, pos.Y] && _FloodFill_TilesEqual(baseTile, tiles[pos.X, pos.Y]);
		}
	}
}
