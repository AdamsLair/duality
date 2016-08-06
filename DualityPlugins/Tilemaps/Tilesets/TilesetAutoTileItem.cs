using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Describes a single AutoTile item by mapping neighbourhood connectivity to the index of the matching tile index.
	/// </summary>
	public struct TilesetAutoTileItem
	{
		/// <summary>
		/// Describes which neighbourhood tiles a tile needs to connect to in order to
		/// match this AutoTile mapping.
		/// </summary>
		public TileConnection Neighbours;
		/// <summary>
		/// The tile index that should be used when the described neighbourhood connectivity
		/// condition is met.
		/// </summary>
		public int TileIndex;
	}
}
