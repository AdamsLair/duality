using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Resources
{
	/// <summary>
	/// A specialized data structure that performs a quick lookup in a set of available kerning pairs.
	/// </summary>
	public class FontKerningLookup
	{
		private FontKerningPair[] pairs;

		/// <summary>
		/// Creates a new kerning pair lookup. The specified array will be used directly,
		/// no copy will be made. Make sure to not re-use it afterwards.
		/// </summary>
		/// <param name="pairs"></param>
		public FontKerningLookup(FontKerningPair[] pairs)
		{
			this.pairs = pairs ?? new FontKerningPair[0];
			Array.Sort(this.pairs, (a, b) => 
			{
				if (a.FirstChar < b.FirstChar)
					return -1;
				else if (a.FirstChar > b.FirstChar)
					return 1;
				else if (a.SecondChar < b.SecondChar)
					return -1;
				else if (a.SecondChar > b.SecondChar)
					return 1;
				else
					return 0;
			});
		}

		/// <summary>
		/// Retrieves the <see cref="FontKerningPair.AdvanceOffset"/> value for the specified pair of characters.
		/// </summary>
		/// <param name="firstChar"></param>
		/// <param name="secondChar"></param>
		/// <returns></returns>
		public float GetAdvanceOffset(char firstChar, char secondChar)
		{
			// Find a matching range of entries for the first char
			int firstCharIndex = this.BinarySearchFirst(firstChar);
			if (firstCharIndex == -1) return 0.0f;

			// Determine the full range where the first char matches
			int rangeBegin = firstCharIndex;
			int rangeEnd = firstCharIndex;
			while (rangeBegin > 0)
			{
				if (this.pairs[rangeBegin - 1].FirstChar == firstChar)
					rangeBegin--;
				else
					break;
			}
			while (rangeEnd < this.pairs.Length - 1)
			{
				if (this.pairs[rangeEnd + 1].FirstChar == firstChar)
					rangeEnd++;
				else
					break;
			}

			// Find a matching item for the second char within the first char match range
			int matchIndex = this.BinarySearchSecond(secondChar, rangeBegin, rangeEnd);
			if (matchIndex == -1) return 0.0f;

			// Do we have a match? Return the advance offset
			return this.pairs[matchIndex].AdvanceOffset;
		}

		private int BinarySearchFirst(char target)
		{
			int first = 0;
			int last = this.pairs.Length - 1;
			int mid;

			while (first <= last)
			{
				mid = (first + last) / 2;

				if (target > this.pairs[mid].FirstChar)
					first = mid + 1;
				else if (target < this.pairs[mid].FirstChar)
					last = mid - 1;
				else
					return mid;
			}

			return -1;
		}
		private int BinarySearchSecond(char target, int first, int last)
		{
			int mid;

			while (first <= last)
			{
				mid = (first + last) / 2;

				if (target > this.pairs[mid].SecondChar)
					first = mid + 1;
				else if (target < this.pairs[mid].SecondChar)
					last = mid - 1;
				else
					return mid;
			}

			return -1;
		}
	}
}
