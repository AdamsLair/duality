using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Resources
{
	/// <summary>
	/// Represents a character set that defines which glyphs are available in a <see cref="Font"/> Resource.
	/// </summary>
	public class FontCharSet
	{
		private string chars             = string.Empty;
		private string charBaseLineRef   = string.Empty;
		private string charDescentRef    = string.Empty;
		private string charBodyAscentRef = string.Empty;

		/// <summary>
		/// [GET / SET] All characters that will be available in the rendered character set.
		/// </summary>
		public string Chars
		{
			get { return this.chars; }
			set { this.chars = value ?? string.Empty; }
		}
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font.BaseLine"/> parameter.
		/// </summary>
		public string CharBaseLineRef
		{
			get { return this.charBaseLineRef; }
			set { this.charBaseLineRef = value ?? string.Empty; }
		}
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font.Descent"/> parameter.
		/// </summary>
		public string CharDescentRef
		{
			get { return this.charDescentRef; }
			set { this.charDescentRef = value ?? string.Empty; }
		}
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font.Ascent"/> parameter.
		/// </summary>
		public string CharBodyAscentRef
		{
			get { return this.charBodyAscentRef; }
			set { this.charBodyAscentRef = value ?? string.Empty; }
		}

		/// <summary>
		/// Merges two character sets to form a new one that contains both of their characters without duplicates.
		/// </summary>
		/// <param name="second"></param>
		/// <returns></returns>
		public FontCharSet MergedWith(FontCharSet second)
		{
			return new FontCharSet
			{
				Chars             = MergeCharList(this.Chars,             (second != null) ? second.Chars             : null),
				CharBaseLineRef   = MergeCharList(this.CharBaseLineRef,   (second != null) ? second.CharBaseLineRef   : null),
				CharDescentRef    = MergeCharList(this.CharDescentRef,    (second != null) ? second.CharDescentRef    : null),
				CharBodyAscentRef = MergeCharList(this.CharBodyAscentRef, (second != null) ? second.CharBodyAscentRef : null),
			};
		}

		private static string MergeCharList(string first, string second)
		{
			if (string.IsNullOrEmpty(first)) return second ?? string.Empty;
			if (string.IsNullOrEmpty(second)) return first ?? string.Empty;

			StringBuilder builder = new StringBuilder(first, first.Length + second.Length);
			for (int i = 0; i < second.Length; i++)
			{
				if (first.IndexOf(second[i]) == -1) continue;
				builder.Append(second[i]);
			}
			return builder.ToString();
		}
	}
}
