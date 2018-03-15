using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A helper class that provides a flood fill algorithm.
	/// </summary>
	public class TilemapFloodFill
	{
		private Grid<bool> activeFillBuffer = new Grid<bool>();

		/// <summary>
		/// Runs the flood fill algorithm on the specified position and writes the result into the specified variables.
		/// </summary>
		/// <param name="tilemap"></param>
		/// <param name="pos"></param>
		/// <param name="preview">If true, the algorithm will cancel when taking too long for an interactive preview.</param>
		/// <param name="floodFillArea"></param>
		/// <param name="floodFillOrigin"></param>
		/// <returns>True, if the algorithm completed. False, if it was canceled.</returns>
		public bool GetFillArea(Tilemap tilemap, Point2 pos, bool preview, Grid<bool> floodFillArea, ref Point2 floodFillOrigin)
		{
			Grid<Tile> tiles = tilemap.BeginUpdateTiles();
			Point2 fillTopLeft;
			Point2 fillSize;
			bool success = this.FillArea(ref this.activeFillBuffer, tiles, pos, preview ? (128 * 128) : 0, out fillTopLeft, out fillSize);
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
		/// <see cref="Tile"/> equality is checked in the <see cref="AreTilesEqual"/> method.
		/// </summary>
		/// <param name="fillBuffer">A buffer that will be filled with the result of the flood fill operation.</param>
		/// <param name="tiles"></param>
		/// <param name="pos"></param>
		/// <param name="maxTileCount">The maximum number of tiles to fill. If the filled tile count exceeds this number, the algorithm is canceled.</param>
		/// <param name="fillAreaSize"></param>
		/// <param name="fillAreaTopLeft"></param>
		/// <returns>True when successful. False when aborted.</returns>
		public bool FillArea(ref Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, int maxTileCount, out Point2 fillAreaTopLeft, out Point2 fillAreaSize)
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
			fillAreaTopLeft = FindTopLeft(fillBuffer, tiles, pos, baseTile);
			fillAreaSize = new Point2(1 + pos.X - fillAreaTopLeft.X, 1 + pos.Y - fillAreaTopLeft.Y);
			pos = fillAreaTopLeft;

			// Run the main part of the algorithm
			if (maxTileCount <= 0) maxTileCount = int.MaxValue;
			bool success = InternalFillArea(fillBuffer, tiles, pos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize);

			return success;
		}

		private bool InternalFillArea(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile, ref int maxTileCount, ref Point2 fillAreaTopLeft, ref Point2 fillAreaSize)
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
				if (!firstRow && !IsFillCandidate(fillBuffer, tiles, pos, baseTile))
				{
					while (lastRowLength > 1)
					{
						pos.X++;
						lastRowLength--;

						if (IsFillCandidate(fillBuffer, tiles, pos, baseTile))
							break;
					}

					rowPos.X = pos.X;
				}
				// Expand scan line width to the left when necessary
				else
				{
					for (; pos.X != 0 && IsFillCandidate(fillBuffer, tiles, new Point2(pos.X - 1, pos.Y), baseTile); rowLength++, lastRowLength++)
					{
						pos.X--;
						fillBuffer[pos.X, pos.Y] = true;

						// Adjust the fill area
						fillAreaSize.X += Math.Max(fillAreaTopLeft.X - pos.X, 0);
						fillAreaTopLeft.X = Math.Min(fillAreaTopLeft.X, pos.X);

						// If something above the current scan line is free, handle it recursively
						if (pos.Y != 0 && IsFillCandidate(fillBuffer, tiles, new Point2(pos.X, pos.Y - 1), baseTile))
						{
							// Find the topleft-most tile to start with
							Point2 targetPos = new Point2(pos.X, pos.Y - 1);
							targetPos = FindTopLeft(fillBuffer, tiles, targetPos, baseTile);
							if (!InternalFillArea(fillBuffer, tiles, targetPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
								return false;
						}
					}
				}

				// Fill the current row
				for (; rowPos.X < tiles.Width && IsFillCandidate(fillBuffer, tiles, rowPos, baseTile); rowLength++, rowPos.X++)
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
					for (int end = pos.X + lastRowLength; ++rowPos.X < end;)
					{
						// Recursively handle the disconnected below-bottom pixels of the last row
						if (IsFillCandidate(fillBuffer, tiles, rowPos, baseTile))
						{
							if (!InternalFillArea(fillBuffer, tiles, rowPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
								return false;
						}
					}
				}
				// If the current row is longer than the previous, see if there are
				// top pixels above this one that are disconnected from the last row
				else if (rowLength > lastRowLength && pos.Y != 0)
				{
					for (int prevRowX = pos.X + lastRowLength; ++prevRowX < rowPos.X;)
					{
						// Recursively handle the disconnected pixels of the last row
						if (IsFillCandidate(fillBuffer, tiles, new Point2(prevRowX, pos.Y - 1), baseTile))
						{
							// Find the topleft-most tile to start with
							Point2 targetPos = new Point2(prevRowX, pos.Y - 1);
							targetPos = FindTopLeft(fillBuffer, tiles, targetPos, baseTile);
							if (!InternalFillArea(fillBuffer, tiles, targetPos, baseTile, ref maxTileCount, ref fillAreaTopLeft, ref fillAreaSize))
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

		private static Point2 FindTopLeft(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile)
		{
			// Find the topleft-most connected matching tile
			while (true)
			{
				Point2 origin = pos;
				while (pos.Y != 0 && IsFillCandidate(fillBuffer, tiles, new Point2(pos.X, pos.Y - 1), baseTile)) pos.Y--;
				while (pos.X != 0 && IsFillCandidate(fillBuffer, tiles, new Point2(pos.X - 1, pos.Y), baseTile)) pos.X--;
				if (pos == origin) break;
			}
			return pos;
		}
		private static bool AreTilesEqual(Tile baseTile, Tile otherTile)
		{
			return baseTile.BaseIndex == otherTile.BaseIndex;
		}
		private static bool IsFillCandidate(Grid<bool> fillBuffer, Grid<Tile> tiles, Point2 pos, Tile baseTile)
		{
			return !fillBuffer[pos.X, pos.Y] && AreTilesEqual(baseTile, tiles[pos.X, pos.Y]);
		}
	}
}