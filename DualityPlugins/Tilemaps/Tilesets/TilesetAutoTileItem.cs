using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Extends the description of a single input tile by defining its role in an AutoTile.
	/// </summary>
	public struct TilesetAutoTileItem
	{
		/// <summary>
		/// Whether or not this tile is considered part of the AutoTile mapping.
		/// </summary>
		public bool IsAutoTile;
		/// <summary>
		/// Describes which neighbourhood tiles the tile needs to connect to in order to
		/// match this AutoTile mapping.
		/// </summary>
		public TileConnection Neighbours;

		public override int GetHashCode()
		{
			int hash = 17;
			MathF.CombineHashCode(ref hash, this.IsAutoTile ? 1 : 0);
			MathF.CombineHashCode(ref hash, (int)this.Neighbours);
			return hash;
		}
	}
}
