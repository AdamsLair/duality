using System;

namespace Duality.Plugins.Tilemaps
{
	/// <summary>
	/// A bitmask that specifies which collision layers are addressed in a <see cref="Tileset"/>.
	/// </summary>
	[Flags]
	public enum TileCollisionLayer : byte
	{
		/// <summary>
		/// No collision layers at all.
		/// </summary>
		None   = 0x0,

		/// <summary>
		/// The default collision layer.
		/// </summary>
		Layer0 = 0x1,
		/// <summary>
		/// The first auxilliary collision layer.
		/// </summary>
		Layer1 = 0x2,
		/// <summary>
		/// The second auxilliary collision layer.
		/// </summary>
		Layer2 = 0x4,
		/// <summary>
		/// The third auxilliary collision layer.
		/// </summary>
		Layer3 = 0x8,

		/// <summary>
		/// All collision layers.
		/// </summary>
		All    = 0xF
	}
}
