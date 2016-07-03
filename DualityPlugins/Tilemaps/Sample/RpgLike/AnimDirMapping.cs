using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Editor;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Maps a names animation direction to a sprite sheet index that represents
	/// the first frame of the animation in that direction.
	/// </summary>
	public struct AnimDirMapping
	{
		/// <summary>
		/// The name of the direction that is mapped.
		/// </summary>
		public string Direction;
		/// <summary>
		/// The reference angle of the direction, in degrees.
		/// </summary>
		public float Angle;
		/// <summary>
		/// The sprite sheet index that will be used as the first frame.
		/// </summary>
		public int SpriteSheetIndex;

		public override string ToString()
		{
			return string.Format(
				"{0} ({1}°) at index {2}", 
				this.Direction, 
				MathF.RoundToInt(this.Angle), 
				this.SpriteSheetIndex);
		}
	}
}
