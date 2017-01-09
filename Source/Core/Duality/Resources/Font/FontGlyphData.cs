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
		/// The width of the glyph.
		/// </summary>
		public int Width;
		/// <summary>
		/// The height of the glyph.
		/// </summary>
		public int Height;
		/// <summary>
		/// The glyphs X offset when rendering it.
		/// </summary>
		public int OffsetX;
		/// <summary>
		/// The glyphs Y offset when rendering it.
		/// </summary>
		public int OffsetY;
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
			return string.Format("Glyph '{0}', {1}x{2}, OffsetX {3}, OffsetY {4}", 
				this.Glyph, 
				this.Width, 
				this.Height, 
				this.OffsetX,
				this.OffsetY);
		}
	}
}
