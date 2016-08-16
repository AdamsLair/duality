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
		/// will be updated based on <see cref="BaseIndex"/>, but not the other way around.
		/// 
		/// This value isn't serialized, but generated afterwards, since it may change when the <see cref="Tileset"/>
		/// configuration changes. Since some tiles might be auto-generated, it is not guaranteed that this index is
		/// also valid in non-compiled <see cref="Tileset"/> source data.
		/// </summary>
		/// <seealso cref="BaseIndex"/>
		public int Index;

		/// <summary>
		/// The base / source tile index in the <see cref="Tileset"/> to which this <see cref="Tile"/> refers.
		/// This is a valid index in the <see cref="Tileset"/> source data, even if the tile it refers to was
		/// generated during compilation.
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

		public override string ToString()
		{
			return string.Format("Tile: {0}, base {1} [c{2}]", this.Index, this.BaseIndex, (byte)this.AutoTileCon);
		}
	}
}
