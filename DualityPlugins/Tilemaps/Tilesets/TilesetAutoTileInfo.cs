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
		private int[] stateToTile = null;
		private bool[] connectivity = null;
		
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
		public IReadOnlyList<int> StateToTile
		{
			get { return this.stateToTile; }
		}
		/// <summary>
		/// [GET] An array that specifies for each tile index whether that tile is considered to be connecting
		/// to this AutoTile. This also involves all the tiles that belong directly to it. Note that the
		/// returned array may be smaller than the overall amount of valid tile indices.
		/// </summary>
		public IReadOnlyList<bool> Connectivity
		{
			get { return this.connectivity; }
		}

		/// <summary>
		/// Creates a new <see cref="TilesetAutoTileInfo"/> based on prepared data.
		/// </summary>
		/// <param name="baseTile">The tile index of the base tile for this AutoTile.</param>
		/// <param name="stateToTile">
		/// An array where the item at a <see cref="TileConnection"/> index represents the tile index
		/// of the border tile to use in this connectivity setup. This array is not copied. If you plan
		/// to re-use it, pass a copy as a parameter.
		/// </param>
		public TilesetAutoTileInfo(int baseTile, int[] stateToTile, bool[] connectivity)
		{
			if (stateToTile == null) throw new ArgumentNullException("stateToTile");
			if (connectivity == null) throw new ArgumentNullException("connectivity");
			if (stateToTile.Length != (int)TileConnection.All + 1) 
			{
				throw new ArgumentException(
					string.Format(
						"Invalid number of border tile mappings. It always has to equal {0}.", 
						(int)TileConnection.All + 1), 
					"borderTiles");
			}

			this.baseTile = baseTile;
			this.stateToTile = stateToTile;
			this.connectivity = connectivity;
		}

		/// <summary>
		/// Determines whether this AutoTile connects to tiles of the specified tile index.
		/// </summary>
		/// <param name="tileIndex"></param>
		/// <returns></returns>
		public bool ConnectsToTile(int tileIndex)
		{
			if (tileIndex < 0) return false;
			if (tileIndex >= this.connectivity.Length) return false;
			return this.connectivity[tileIndex];
		}
	}
}
