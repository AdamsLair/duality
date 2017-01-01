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
		public static readonly FontCharSet Default = new FontCharSet(
			chars:             "? abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,;.:-_<>|#'+*~@^°!\"§$%&/()=`²³{[]}\\´öäüÖÄÜß",
			charBaseLineRef:   "acemnorsuvwxz",
			charDescentRef:    "pqgjyQ|",
			charBodyAscentRef: "acemnorsuvwxz"
		);

		private string chars             = string.Empty;
		private string charBaseLineRef   = string.Empty;
		private string charDescentRef    = string.Empty;
		private string charBodyAscentRef = string.Empty;

		/// <summary>
		/// [GET] All characters that are part of this <see cref="FontCharSet"/>.
		/// </summary>
		public string Chars
		{
			get { return this.chars; }
		}
		/// <summary>
		/// [GET] Characters which will contribute to calculating the <see cref="FontMetrics.BaseLine"/> parameter.
		/// </summary>
		public string CharBaseLineRef
		{
			get { return this.charBaseLineRef; }
		}
		/// <summary>
		/// [GET] Characters which will contribute to calculating the <see cref="FontMetrics.Descent"/> parameter.
		/// </summary>
		public string CharDescentRef
		{
			get { return this.charDescentRef; }
		}
		/// <summary>
		/// [GET] Characters which will contribute to calculating the <see cref="FontMetrics.Ascent"/> parameter.
		/// </summary>
		public string CharBodyAscentRef
		{
			get { return this.charBodyAscentRef; }
		}

		private FontCharSet() { }
		public FontCharSet(string chars) : this(chars, string.Empty, string.Empty, string.Empty) { }
		public FontCharSet(string chars, string charBaseLineRef, string charDescentRef, string charBodyAscentRef)
		{
			this.chars = chars;
			this.charBaseLineRef = charBaseLineRef;
			this.charDescentRef = charDescentRef;
			this.charBodyAscentRef = charBodyAscentRef;
		}

		/// <summary>
		/// Merges two character sets to form a new one that contains both of their characters without duplicates.
		/// </summary>
		/// <param name="second"></param>
		/// <returns></returns>
		public FontCharSet MergedWith(FontCharSet second)
		{
			return new FontCharSet(
				chars:             MergeCharList(this.chars,             (second != null) ? second.chars             : null),
				charBaseLineRef:   MergeCharList(this.charBaseLineRef,   (second != null) ? second.charBaseLineRef   : null),
				charDescentRef:    MergeCharList(this.charDescentRef,    (second != null) ? second.charDescentRef    : null),
				charBodyAscentRef: MergeCharList(this.charBodyAscentRef, (second != null) ? second.charBodyAscentRef : null)
			);
		}

		private static string MergeCharList(string first, string second)
		{
			if (string.IsNullOrEmpty(first)) return second ?? string.Empty;
			if (string.IsNullOrEmpty(second)) return first ?? string.Empty;

			StringBuilder builder = new StringBuilder(first, first.Length + second.Length);
			for (int i = 0; i < second.Length; i++)
			{
				if (first.IndexOf(second[i]) != -1) continue;
				builder.Append(second[i]);
			}
			return builder.ToString();
		}
	}
}
