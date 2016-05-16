using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Aggregates a single tile's multi-layer collision info.
	/// </summary>
	/// <seealso cref="TileCollisionLayer"/>
	/// <seealso cref="TileCollisionShape"/>
	/// <seealso cref="TileInput"/>
	public struct TileCollisionShapes
	{
		/// <summary>
		/// The tiles collision shape on the default layer.
		/// </summary>
		public TileCollisionShape Layer0;
		/// <summary>
		/// The tiles collision shape on first auxilliary layer.
		/// </summary>
		public TileCollisionShape Layer1;
		/// <summary>
		/// The tiles collision shape on second auxilliary layer.
		/// </summary>
		public TileCollisionShape Layer2;
		/// <summary>
		/// The tiles collision shape on third auxilliary layer.
		/// </summary>
		public TileCollisionShape Layer3;
	}
}
