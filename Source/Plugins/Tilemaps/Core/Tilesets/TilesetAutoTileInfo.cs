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
		private TilesetAutoTileItem[] tileInfo = null;
		
		/// <summary>
		/// [GET] The tile index inside the <see cref="Tileset"/> that is considered to be the base
		/// tile representing the entire set of connection-dependend tiles inside this auto-tile.
		/// </summary>
		public int BaseTileIndex
		{
			get { return this.baseTile; }
		}
		/// <summary>
		/// [GET] An readonly list where the item at a <see cref="TileConnection"/> index represents the tile index
		/// of the border tile to use in this connectivity setup.
		/// </summary>
		public IReadOnlyList<int> StateToTile
		{
			get { return this.stateToTile; }
		}
		/// <summary>
		/// [GET] A readonly list where each item extends the regular <see cref="Tileset.TileData"/> description by
		/// AutoTile-specific information, such as its matching connectivity state, or whether it is considered to
		/// connect to this AutoTile.
		/// </summary>
		public IReadOnlyList<TilesetAutoTileItem> TileInfo
		{
			get { return this.tileInfo; }
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
		public TilesetAutoTileInfo(int baseTile, int[] stateToTile, TilesetAutoTileItem[] tileInfo)
		{
			if (stateToTile == null) throw new ArgumentNullException("stateToTile");
			if (tileInfo == null) throw new ArgumentNullException("tileInfo");
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
			this.tileInfo = tileInfo;
		}
	}
}
