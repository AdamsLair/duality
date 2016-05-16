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
		Free        = 0x0,

		/// <summary>
		/// Specifies that the top left corner of the tile is blocked.
		/// </summary>
		TopLeft     = 0x1,
		/// <summary>
		/// Specifies that the top right corner of the tile is blocked.
		/// </summary>
		TopRight    = 0x2,
		/// <summary>
		/// Specifies that the bottom left corner of the tile is blocked.
		/// </summary>
		BottomLeft  = 0x4,
		/// <summary>
		/// Specifies that the bottom right corner of the tile is blocked.
		/// </summary>
		BottomRight = 0x8,
		/// <summary>
		/// Whether or not the fill bit is set can make the difference between
		/// a diagonal slope or a two-sided corner, as well as the difference
		/// between a completely solid tile and one that is fenced on four sides.
		/// </summary>
		Fill        = 0x10,

		/// <summary>
		/// All collision bits are set. This is generally true for tiles that are
		/// completely impassable / solid.
		/// </summary>
		Solid       = TopLeft | TopRight | BottomLeft | BottomRight | Fill
	}
}
