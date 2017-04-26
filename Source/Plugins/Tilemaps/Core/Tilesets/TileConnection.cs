using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// Specifies the 8-neighbour connectivity of a tile to its neighbouring tiles.
	/// </summary>
	[Flags]
	public enum TileConnection : byte
	{
		None		= 0x00,

		Top		 = 0x01,
		Right	   = 0x02,
		Bottom	  = 0x04,
		Left		= 0x08,
		TopLeft	 = 0x10,
		TopRight	= 0x20,
		BottomRight = 0x40,
		BottomLeft  = 0x80,

		All		 = 0xFF
	}
}
