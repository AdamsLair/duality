using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single auto-tile as generated during a <see cref="Tileset"/> compilation process.
	/// </summary>
	public class TilesetAutoTileInfo
	{
		private int baseTile = 0;
		private int[] tiles = null;
		
		/// <summary>
		/// [GET] The tile index inside the <see cref="Tileset"/> that is considered to be the base
		/// tile representing the entire set of connection-dependend tiles inside this auto-tile.
		/// </summary>
		public int BaseTileIndex
		{
			get { return this.baseTile; }
		}
		/// <summary>
		/// [GET] An array where the item at a <see cref="TileConnection"/> index represents the tile index
		/// of the border tile to use in this connectivity setup.
		/// </summary>
		public IReadOnlyList<int> BorderTileIndices
		{
			get { return this.tiles; }
		}

		/// <summary>
		/// Creates a new <see cref="TilesetAutoTileInfo"/> based on prepared data.
		/// </summary>
		/// <param name="baseTile">The tile index of the base tile for this AutoTile.</param>
		/// <param name="borderTiles">
		/// An array where the item at a <see cref="TileConnection"/> index represents the tile index
		/// of the border tile to use in this connectivity setup. This array is not copied. If you plan
		/// to re-use it, pass a copy as a parameter.
		/// </param>
		public TilesetAutoTileInfo(int baseTile, int[] borderTiles)
		{
			if (borderTiles == null) throw new ArgumentNullException("borderTiles");
			if (borderTiles.Length != (int)TileConnection.All + 1) 
			{
				throw new ArgumentException(
					string.Format(
						"Invalid number of border tile mappings. It always has to equal {0}.", 
						(int)TileConnection.All + 1), 
					"borderTiles");
			}

			this.baseTile = baseTile;
			this.tiles = borderTiles;
		}
	}
}
