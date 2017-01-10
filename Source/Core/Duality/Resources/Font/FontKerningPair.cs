using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Resources
{
	/// <summary>
	/// Represents a pair of text characters that deviate from their default advance
	/// values when occurring next to each other.
	/// </summary>
	public struct FontKerningPair
	{
		/// <summary>
		/// The first character of this kerning pair to occur in the text.
		/// </summary>
		public char FirstChar;
		/// <summary>
		/// The character that follows the <see cref="FirstChar"/> in the text.
		/// </summary>
		public char SecondChar;
		/// <summary>
		/// An offset to the <see cref="GlyphData.Advance"/> value that will be applied
		/// right after the first character, affecting the placement of the second one.
		/// </summary>
		public float AdvanceOffset;

		public FontKerningPair(char first, char second, float offset)
		{
			this.FirstChar = first;
			this.SecondChar = second;
			this.AdvanceOffset = offset;
		}

		public override string ToString()
		{
			return string.Format("['{0}', '{1}']: {2:F}", 
				this.FirstChar, 
				this.SecondChar, 
				this.AdvanceOffset);
		}
	}
}
