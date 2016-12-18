using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Describes a blend between two sprite indices.
	/// </summary>
	public struct SpriteIndexBlend
	{
		/// <summary>
		/// A value that indicates that no specific sprite indices are used at all. This
		/// can be used to represent a state where a <see cref="Duality.Resources.Texture"/>
		/// is used as a whole, without doing a UV rect lookup.
		/// </summary>
		public static readonly SpriteIndexBlend None = new SpriteIndexBlend(-1);
		/// <summary>
		/// A value that represents the first available sprite.
		/// </summary>
		public static readonly SpriteIndexBlend First = new SpriteIndexBlend(0);

		private int current;
		private int next;
		private float blend;


		/// <summary>
		/// The currently displayed sprite index.
		/// </summary>
		public int Current
		{
			get { return this.current; }
			set { this.current = Math.Max(value, -1); }
		}
		/// <summary>
		/// The sprite index that is to be displayed next / gradually blended over to.
		/// </summary>
		public int Next
		{
			get { return this.next; }
			set { this.next = Math.Max(value, -1); }
		}
		/// <summary>
		/// A blend value that represents the contribution of each sprite index to
		/// the displayed image. Ranging from zero to one, zero represents <see cref="Current"/>
		/// while one represents <see cref="Next"/> and 0.5 and equal blend between both.
		/// </summary>
		public float Blend
		{
			get { return this.blend; }
			set { this.blend = MathF.Clamp(value, 0.0f, 1.0f); }
		}


		public SpriteIndexBlend(int index)
		{
			this.current = Math.Max(index, -1);
			this.next = this.current;
			this.blend = 0.0f;
		}
		public SpriteIndexBlend(int currentIndex, int nextIndex, float blend)
		{
			this.current = Math.Max(currentIndex, -1);
			this.next = Math.Max(nextIndex, -1);
			this.blend = blend;
		}

		/// <summary>
		/// Removes any blending information and transforms the blend of sprite indices into
		/// one that represents only a single sprite index.
		/// </summary>
		public void RemoveBlend()
		{
			this.next = this.current;
			this.blend = 0.0f;
		}
	}
}
