using System;

namespace Duality.Resources
{
	/// <summary>
	/// Provides information about the shape and size of a <see cref="Font"/>.
	/// This information is not specific to a certain text or glyph.
	/// </summary>
	public class FontMetrics
	{
		private float size;
		private int height;
		private int ascent;
		private int bodyAscent;
		private int descent;
		private int baseLine;
		private bool monospace;

		/// <summary>
		/// [GET] The size of the <see cref="Font"/>.
		/// </summary>
		public float Size
		{
			get { return this.size; }
		}
		/// <summary>
		/// [GET] The height of the <see cref="Font"/>, in pixels.
		/// </summary>
		public int Height
		{
			get { return this.height; }
		}
		/// <summary>
		/// [GET] The ascender height of the <see cref="Font"/>, in pixels.
		/// </summary>
		public int Ascent
		{
			get { return this.ascent; }
		}
		/// <summary>
		/// [GET] The median / mean line / x glyph height of the <see cref="Font"/>, in pixels.
		/// </summary>
		public int BodyAscent
		{
			get { return this.bodyAscent; }
		}
		/// <summary>
		/// [GET] The descender height of the <see cref="Font"/>, in pixels.
		/// </summary>
		public int Descent
		{
			get { return this.descent; }
		}
		/// <summary>
		/// [GET] The baseline height of the <see cref="Font"/>, in pixels.
		/// </summary>
		public int BaseLine
		{
			get { return this.baseLine; }
		}
		/// <summary>
		/// [GET] Whether the described <see cref="Font"/> is considered to be a monospace <see cref="Font"/>,
		/// i.e. whether all characters occupy the same horizontal space.
		/// </summary>
		public bool Monospace
		{
			get { return this.monospace; }
		}

		private FontMetrics() { }
		public FontMetrics(float size, int height, int ascent, int bodyAscent, int descent, int baseLine, bool monospace)
		{
			this.size = size;
			this.height = height;
			this.ascent = ascent;
			this.bodyAscent = bodyAscent;
			this.descent = descent;
			this.baseLine = baseLine;
			this.monospace = monospace;
		}
	}
}
