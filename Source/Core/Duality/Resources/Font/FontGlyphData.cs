using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality.Drawing;
using Duality.Editor;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Resources
{
	/// <summary>
	/// Contains data about a single glyph.
	/// </summary>
	public struct FontGlyphData
	{
		/// <summary>
		/// The glyph that is encoded.
		/// </summary>
		public char Glyph;
		/// <summary>
		/// The displayed size of the glyph.
		/// </summary>
		public Vector2 Size;
		/// <summary>
		/// The offset at which the glyph image is displayed relative to its base position.
		/// </summary>
		public Vector2 Offset;
		/// <summary>
		/// The glyphs kerning samples to the left.
		/// </summary>
		public int[] KerningSamplesLeft;
		/// <summary>
		/// The glyphs kerning samples to the right.
		/// </summary>
		public int[] KerningSamplesRight;

		public override string ToString()
		{
			return string.Format("Glyph '{0}', Size {1}, Offset {2}", 
				this.Glyph, 
				this.Size, 
				this.Offset);
		}
	}
}
