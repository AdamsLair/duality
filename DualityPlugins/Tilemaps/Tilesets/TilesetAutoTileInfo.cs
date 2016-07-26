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
		private int[] tiles = new int[(int)TileConnection.All + 1];
		
		/// <summary>
		/// [GET / SET] The tile index inside the <see cref="Tileset"/> that is considered to be the base
		/// tile representing the entire set of connection-dependend tiles inside this auto-tile.
		/// </summary>
		public int BaseTileIndex
		{
			get { return this.baseTile; }
			set { this.baseTile = value; }
		}
		/// <summary>
		/// [GET] An array where the item at a <see cref="TileConnection"/> index represents the tile index
		/// of the border tile to use in this connectivity setup.
		/// </summary>
		public int[] BorderTileIndices
		{
			get { return this.tiles; }
		}
	}
}
