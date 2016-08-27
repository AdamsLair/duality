using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Plugins.Tilemaps.Properties;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A single <see cref="Tile"/> in a <see cref="Tilemap"/>.
	/// </summary>
	public struct Tile
	{
		/// <summary>
		/// The final, all-things-considered index in the <see cref="Tileset"/> to which this <see cref="Tile"/> refers.
		/// 
		/// You usually wouldn't change this value directly, but change <see cref="BaseIndex"/> instead, as <see cref="Index"/>
		/// will be updated based on <see cref="BaseIndex"/>, but not the other way around. When you change this value, make sure
		/// to invoke <see cref="ResolveIndex"/> afterwards.
		/// 
		/// This value isn't serialized, but generated afterwards, since it may change when the <see cref="Tileset"/>
		/// configuration changes. Since some tiles might be auto-generated, it is not guaranteed that this index is
		/// also valid in non-compiled <see cref="Tileset"/> source data.
		/// </summary>
		/// <seealso cref="BaseIndex"/>
		public int Index;

		/// <summary>
		/// The base / source tile index in the <see cref="Tileset"/> to which this <see cref="Tile"/> refers.
		/// The <see cref="BaseIndex"/> is a valid index in the <see cref="Tileset"/> source data, even if the 
		/// actually displayed and used <see cref="Index"/> was generated during compilation.
		/// </summary>
		/// <seealso cref="Index"/>
		public int BaseIndex;
		/// <summary>
		/// The depth offset of the <see cref="Tile"/>, measured in tiles.
		/// </summary>
		public short DepthOffset;
		/// <summary>
		/// The connection state of this <see cref="Tile"/> with regard to AutoTiling rules.
		/// </summary>
		public TileConnection AutoTileCon;


		public Tile(int baseIndex)
		{
			// As long as not resolved otherwise, use the base index directly.
			this.Index = baseIndex;
			this.BaseIndex = baseIndex;
			this.DepthOffset = 0;
			this.AutoTileCon = TileConnection.None;
		}
		
		/// <summary>
		/// Resolves the <see cref="Index"/> of the <see cref="Tile"/> based on 
		/// its <see cref="BaseIndex"/> and <see cref="AutoTileCon"/>.
		/// </summary>
		/// <param name="tileset"></param>
		public void ResolveIndex(ContentRef<Tileset> tileset)
		{
			if (tileset.Res == null) throw new ArgumentNullException("tileset");
			if (!tileset.Res.Compiled) throw new InvalidOperationException("The specified Tileset needs to be compiled first.");

			Tileset tilesetRes = tileset.Res;
			int autoTileIndex = tilesetRes.TileData[this.BaseIndex].AutoTileLayer - 1;
			TilesetAutoTileInfo autoTile = autoTileIndex >= 0 ? tilesetRes.AutoTileData[autoTileIndex] : null;

			this.ResolveIndex(autoTile);
		}
		/// <summary>
		/// Resolves the <see cref="Index"/> of the <see cref="Tile"/> based on 
		/// its <see cref="BaseIndex"/> and <see cref="AutoTileCon"/>.
		/// </summary>
		/// <param name="autoTile"></param>
		private void ResolveIndex(TilesetAutoTileInfo autoTile)
		{
			// Non-AutoTiles always use their base index directly.
			if (autoTile == null)
			{
				this.Index = this.BaseIndex;
			}
			// AutoTiles require a dynamic lookup with their connectivity state, because
			// they might use generated tiles that do not have a consistent index across
			// different Tileset configs.
			else
			{
				int targetIndex = autoTile.StateToTile[(int)this.AutoTileCon];

				// If the AutoTile connectivity state already matches the one we'd get with the default
				// resolved tile index, use the current one directly and don't change it.
				// This will allow scenarios where users specify multiple tiles for a certain connectivity
				// state, without forcing them back to a single one during resolve.
				if (autoTile.TileInfo[this.BaseIndex].Neighbours == autoTile.TileInfo[targetIndex].Neighbours)
				{
					this.Index = this.BaseIndex;
				}
				// Otherwise, lookup the expected tile using base index and connectivity. This
				// will retrieve the proper generated tile, which has an index that may change
				// between multiple compilations.
				else
				{ 
					this.Index = targetIndex;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("Tile: {0}, base {1} [c{2}]", this.Index, this.BaseIndex, (byte)this.AutoTileCon);
		}

		
		/// <summary>
		/// Resolves the <see cref="Index"/> values of the specified tile array segment.
		/// </summary>
		/// <param name="tiles"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="tileset"></param>
		public static void ResolveIndices(Tile[] tiles, int index, int count, ContentRef<Tileset> tileset)
		{
			ResolveIndices(tiles, 0, index, 0, count, 1, tileset);
		}
		/// <summary>
		/// Resolves the <see cref="Index"/> values of the specified <see cref="Tile"/> grid area.
		/// </summary>
		/// <param name="tileGrid"></param>
		/// <param name="beginX"></param>
		/// <param name="beginY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="tileset"></param>
		public static void ResolveIndices(Grid<Tile> tileGrid, int beginX, int beginY, int width, int height, ContentRef<Tileset> tileset)
		{
			Tile[] rawData = tileGrid.RawData;
			int stride = tileGrid.Width;
			ResolveIndices(rawData, stride, beginX, beginY, width, height, tileset);
		}
		/// <summary>
		/// Resolves the <see cref="Index"/> values of the specified <see cref="Tile"/> grid area, given the grid's raw data block.
		/// </summary>
		/// <param name="tileGridData"></param>
		/// <param name="beginX"></param>
		/// <param name="beginY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="stride"></param>
		/// <param name="tilesetRes"></param>
		public static void ResolveIndices(Tile[] tileGridData, int stride, int beginX, int beginY, int width, int height, ContentRef<Tileset> tileset)
		{
			if (tileset.Res == null) throw new ArgumentNullException("tileset");
			if (!tileset.Res.Compiled) throw new InvalidOperationException("The specified Tileset needs to be compiled first.");

			Tileset tilesetRes = tileset.Res;
			TileInfo[] tileData = tilesetRes.TileData.Data;
			for (int y = beginY; y < beginY + height; y++)
			{
				for (int x = beginX; x < beginX + width; x++)
				{
					int i = y * stride + x;
					int autoTileIndex = tileData[tileGridData[i].BaseIndex].AutoTileLayer - 1;
					TilesetAutoTileInfo autoTile = autoTileIndex > -1 ? tilesetRes.AutoTileData[autoTileIndex] : null;

					tileGridData[i].ResolveIndex(autoTile);
				}
			}
		}

		/// <summary>
		/// Updates the <see cref="AutoTileCon"/> state of a rectangular region on the specified tile grid
		/// based on its connectivity state with neighbouring tiles.
		/// </summary>
		/// <param name="tileGrid"></param>
		/// <param name="beginX"></param>
		/// <param name="beginY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="tileset"></param>
		public static void UpdateAutoTileCon(Grid<Tile> tileGrid, int beginX, int beginY, int width, int height, ContentRef<Tileset> tileset)
		{
			UpdateAutoTileCon(tileGrid, null, beginX, beginY, width, height, tileset);
		}
		/// <summary>
		/// Updates the <see cref="AutoTileCon"/> state of an arbitrary region on the specified tile grid
		/// based on its connectivity state with neighbouring tiles.
		/// </summary>
		/// <param name="tileGrid"></param>
		/// <param name="updateMask"></param>
		/// <param name="beginX"></param>
		/// <param name="beginY"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="tilesetRes"></param>
		public static void UpdateAutoTileCon(Grid<Tile> tileGrid, Grid<bool> updateMask, int beginX, int beginY, int width, int height, ContentRef<Tileset> tileset)
		{
			if (tileset.Res == null) throw new ArgumentNullException("tileset");
			if (tileGrid == null) throw new ArgumentNullException("tileGrid");

			Tileset tilesetRes = tileset.Res;
			TileInfo[] tileData = tilesetRes.TileData.Data;
			Tile[] tiles = tileGrid.RawData;
			bool[] maskData = updateMask != null ? updateMask.RawData : null;
			int tileStride = tileGrid.Width;
			int maskStride = updateMask.Width;
			int maxTileX = tileGrid.Width - 1;
			int maxTileY = tileGrid.Height - 1;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					// Skip tiles that have been masked away
					int m = x + maskStride * y;
					if (maskData != null && !maskData[m]) continue;

					// Determine tilemap coordinates and index
					int tileX = x + beginX;
					int tileY = y + beginY;
					int i = tileX + tileStride * tileY;

					// Skip non-AutoTiles
					int autoTileIndex = tileData[tiles[i].BaseIndex].AutoTileLayer - 1;
					if (autoTileIndex == -1) continue;

					// Lookup AutoTile data
					TilesetAutoTileInfo autoTile = tilesetRes.AutoTileData[autoTileIndex];
					IReadOnlyList<TilesetAutoTileItem> autoTileInfo = autoTile.TileInfo;

					// Check neighbour connectivity
					bool topLeft     = (tileX <= 0        || tileY <= 0       ) || autoTileInfo[tiles[i - 1 - tileStride].Index].ConnectsToAutoTile;
					bool top         = (                     tileY <= 0       ) || autoTileInfo[tiles[i     - tileStride].Index].ConnectsToAutoTile;
					bool topRight    = (tileX >= maxTileX || tileY <= 0       ) || autoTileInfo[tiles[i + 1 - tileStride].Index].ConnectsToAutoTile;
					bool left        = (tileX <= 0                            ) || autoTileInfo[tiles[i - 1             ].Index].ConnectsToAutoTile;
					bool right       = (tileX >= maxTileX                     ) || autoTileInfo[tiles[i + 1             ].Index].ConnectsToAutoTile;
					bool bottomLeft  = (tileX <= 0        || tileY >= maxTileY) || autoTileInfo[tiles[i - 1 + tileStride].Index].ConnectsToAutoTile;
					bool bottom      = (                     tileY >= maxTileY) || autoTileInfo[tiles[i     + tileStride].Index].ConnectsToAutoTile;
					bool bottomRight = (tileX >= maxTileX || tileY >= maxTileY) || autoTileInfo[tiles[i + 1 + tileStride].Index].ConnectsToAutoTile;

					// Create connectivity bitmask
					TileConnection autoTileCon = TileConnection.None;
					if (topLeft)     autoTileCon |= TileConnection.TopLeft;
					if (top)         autoTileCon |= TileConnection.Top;
					if (topRight)    autoTileCon |= TileConnection.TopRight;
					if (left)        autoTileCon |= TileConnection.Left;
					if (right)       autoTileCon |= TileConnection.Right;
					if (bottomLeft)  autoTileCon |= TileConnection.BottomLeft;
					if (bottom)      autoTileCon |= TileConnection.Bottom;
					if (bottomRight) autoTileCon |= TileConnection.BottomRight;

					// Update connectivity and re-resolve index
					tiles[i].AutoTileCon = autoTileCon;
					tiles[i].ResolveIndex(autoTile);
				}
			}
		}
	}
}
