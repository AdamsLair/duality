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

		/// <summary>
		/// The currently displayed sprite index.
		/// </summary>
		public int Current;
		/// <summary>
		/// The sprite index that is to be displayed next / gradually blended over to.
		/// </summary>
		public int Next;
		/// <summary>
		/// A blend value that represents the contribution of each sprite index to
		/// the displayed image. Ranging from zero to one, zero represents <see cref="Current"/>
		/// while one represents <see cref="Next"/> and 0.5 and equal blend between both.
		/// </summary>
		public float Blend;


		public SpriteIndexBlend(int index)
		{
			this.Current = index;
			this.Next = index;
			this.Blend = 0.0f;
			this.Clamp();
		}
		public SpriteIndexBlend(int currentIndex, int nextIndex, float blend)
		{
			this.Current = currentIndex;
			this.Next = nextIndex;
			this.Blend = blend;
			this.Clamp();
		}

		/// <summary>
		/// Clamps <see cref="Current"/>, <see cref="Next"/> and <see cref="Blend"/> into valid ranges.
		/// </summary>
		public void Clamp()
		{
			this.Current = Math.Max(this.Current, -1);
			this.Next = Math.Max(this.Next, -1);
			this.Blend = MathF.Clamp(this.Blend, 0.0f, 1.0f);
		}
		/// <summary>
		/// Removes any blending information and transforms the blend of sprite indices into
		/// one that represents only a single sprite index.
		/// </summary>
		public void RemoveBlend()
		{
			this.Next = this.Current;
			this.Blend = 0.0f;
		}

		public override string ToString()
		{
			if (this.Current != this.Next)
				return string.Format("Index {0} to {1} at {2}%", this.Current, this.Next, this.Blend);
			else
				return string.Format("Index {0}", this.Current);
		}
	}
}
