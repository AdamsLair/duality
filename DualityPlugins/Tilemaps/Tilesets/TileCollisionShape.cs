using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Defines the collision shape of a single tile by specifying
	/// whether each of its corners is blocked, in addition to a central
	/// "filled" flag.
	/// </summary>
	[Flags]
	public enum TileCollisionShape : byte
	{
		/// <summary>
		/// The tile is completely empty. No collision at all.
		/// </summary>
		Free           = 0x00,

		/// <summary>
		/// The tiles top edge is considered solid.
		/// </summary>
		Top            = 0x01,
		/// <summary>
		/// The tiles bottom edge is considered solid.
		/// </summary>
		Bottom         = 0x02,
		/// <summary>
		/// The tiles left edge is considered solid.
		/// </summary>
		Left           = 0x04,
		/// <summary>
		/// The tiles right edge is considered solid.
		/// </summary>
		Right          = 0x08,
		/// <summary>
		/// A solid diagonal edge from the tiles bottom left to its
		/// top right corner is assumed.
		/// </summary>
		DiagonalUp     = 0x10,
		/// <summary>
		/// A solid diagonal edge from the tiles top left to its
		/// bottom right corner is assumed.
		/// </summary>
		DiagonalDown   = 0x20,

		/// <summary>
		/// All collision bits are set. This is generally true for tiles that are
		/// completely impassable / solid.
		/// </summary>
		Solid          = Top | Bottom | Left | Right | DiagonalUp | DiagonalDown
	}
}
