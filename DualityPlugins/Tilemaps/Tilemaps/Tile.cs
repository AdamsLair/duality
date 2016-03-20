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
		/// The index in the <see cref="Tileset"/> to which this <see cref="Tile"/> refers.
		/// </summary>
		public int Index;
		/// <summary>
		/// The depth offset of the <see cref="Tile"/>, measured in tiles.
		/// </summary>
		public short DepthOffset;

		public override string ToString()
		{
			return string.Format("Tile: {0}", this.Index);
		}
	}
}
