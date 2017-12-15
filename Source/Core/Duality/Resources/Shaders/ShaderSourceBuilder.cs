using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Duality.Resources
{
	/// <summary>
	/// A utility class for preprocessing and merging chunks of shader source code.
	/// </summary>
	public class ShaderSourceBuilder
	{
		/// <summary>
		/// Describes a range via index and length.
		/// </summary>
		private struct IndexRange
		{
			public int Index;
			public int Length;

			public IndexRange(int index, int length)
			{
				this.Index = index;
				this.Length = length;
			}

			public bool Overlaps(IndexRange other)
			{
				if (other.Index >= this.Index && other.Index < this.Index + this.Length) return true;
				if (this.Index >= other.Index && this.Index < other.Index + other.Length) return true;
				return false;
			}

			public override string ToString()
			{
				return string.Format("[{0} - {1}]", this.Index, this.Index + this.Length);
			}
		}

		private static readonly Regex RegexBlockComment = new Regex(@"/\*(.*?)\*/", RegexOptions.Singleline);
		private static readonly Regex RegexLineComment = new Regex(@"//(.*?)\r?\n", RegexOptions.Singleline);
		private static readonly Regex RegexVariableDeclaration = new Regex(@"(uniform|varying|attribute|in|out)\s+(\w+)\s+(\w+)\s*;", RegexOptions.Singleline);
		private static readonly Regex RegexVersionDirective = new Regex(@"#version\s+(\d+)\s+");


		private string mainChunk = string.Empty;
		private List<string> sharedChunk = new List<string>();
		private StringBuilder textBuilder = new StringBuilder();


		/// <summary>
		/// Clears this <see cref="ShaderSourceBuilder"/> instance of all content.
		/// </summary>
		public void Clear()
		{
			this.mainChunk = string.Empty;
			this.sharedChunk.Clear();
		}
		/// <summary>
		/// Specifies the main chunk of source code to use. The main chunk is the one
		/// where line numbers remain unchanged due to preprocessing.
		/// </summary>
		/// <param name="sourceCode"></param>
		public void SetMainChunk(string sourceCode)
		{
			this.mainChunk = sourceCode ?? string.Empty;
		}
		/// <summary>
		/// Adds a shared chunk of source code to use. Shared chunks do not retain their
		/// line numbers during preprocessing, but will be re-assigned to individual (virtual)
		/// ranges in the 10000s.
		/// </summary>
		/// <param name="sourceCode"></param>
		public void AddSharedChunk(string sourceCode)
		{
			this.sharedChunk.Add(sourceCode);
		}

		/// <summary>
		/// Builds the merged, preprocessed source code.
		/// </summary>
		/// <returns></returns>
		public string Build()
		{
			this.textBuilder.Clear();

			// Append shared chunks at the top
			for (int i = 0; i < this.sharedChunk.Count; i++)
			{
				this.textBuilder.AppendFormat("#line {0}", (i + 1) * 10000);
				this.textBuilder.AppendLine();
				this.textBuilder.Append(this.sharedChunk[i]);
				this.textBuilder.AppendLine();
			}

			// Append main chunk below
			this.textBuilder.AppendLine("#line 1");
			this.textBuilder.Append(this.mainChunk);

			// Generate a first, raw version of the merged source code
			string rawMerge = this.textBuilder.ToString();

			// Identify all commented regions, so we can exclude them from further processing
			List<IndexRange> ignoreRegions = new List<IndexRange>();
			foreach (Match match in RegexBlockComment.Matches(rawMerge))
			{
				ignoreRegions.Add(new IndexRange(
					match.Groups[1].Index, 
					match.Groups[1].Length));
			}
			foreach (Match match in RegexLineComment.Matches(rawMerge))
			{
				ignoreRegions.Add(new IndexRange(
					match.Groups[1].Index,
					match.Groups[1].Length));
			}

			// Identify (shared) variable declarations
			HashSet<string> variableDeclarations = new HashSet<string>();
			List<IndexRange> removeLines = new List<IndexRange>();
			foreach (Match match in RegexVariableDeclaration.Matches(rawMerge))
			{
				IndexRange range = new IndexRange(match.Index, match.Length);
				if (this.AnyRangeOverlap(range, ignoreRegions)) continue;

				// Normalize declaration, so we can compare it to others and match regardless of spacing,
				// declaring keyword or optional qualifiers.
				string normalizedDecl = (match.Groups[2].Value + match.Groups[3].Value);

				// If we previously saw that declaration, schedule it for removal
				if (!variableDeclarations.Add(normalizedDecl))
				{
					IndexRange lineRange = this.ExpandToLine(rawMerge, range);
					if (lineRange.Length > 0)
						removeLines.Add(lineRange);
				}
			}

			// Identify version directives
			int lastVersion = 0;
			foreach (Match match in RegexVersionDirective.Matches(rawMerge))
			{
				IndexRange range = new IndexRange(match.Index, match.Length);
				if (this.AnyRangeOverlap(range, ignoreRegions)) continue;

				int version;
				if (int.TryParse(match.Groups[1].Value, out version))
					lastVersion = version;

				IndexRange lineRange = this.ExpandToLine(rawMerge, range);
				if (lineRange.Length > 0)
					removeLines.Add(lineRange);
			}

			// Comment out lines that we scheduled for removal
			this.CommentOutLines(this.textBuilder, removeLines);

			// If we encountered any version directive, add it back at the top
			if (lastVersion > 0)
			{
				this.textBuilder.Insert(0, "\r\n");
				this.textBuilder.Insert(0, lastVersion);
				this.textBuilder.Insert(0, "#version ");
			}

			rawMerge = this.textBuilder.ToString();
			return rawMerge;
		}

		/// <summary>
		/// Given an index, length, and text they're referring to, this method will expand
		/// the specified range to include all overlapped full lines from start to end.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		private IndexRange ExpandToLine(string text, IndexRange range)
		{
			int lineBeginIndex = text.LastIndexOfAny(new char[] { '\n', '\r' }, range.Index) + 1;
			int lineEndIndex = text.IndexOfAny(new char[] { '\n', '\r' }, range.Index + range.Length);
			return new IndexRange(lineBeginIndex, lineEndIndex - lineBeginIndex);
		}
		/// <summary>
		/// Determines whether the specified range overlaps with any of the comparison ranges.
		/// </summary>
		/// <param name="range"></param>
		/// <param name="compareToRanges"></param>
		/// <returns></returns>
		private bool AnyRangeOverlap(IndexRange range, List<IndexRange> compareToRanges)
		{
			foreach (IndexRange ignoreRange in compareToRanges)
			{
				if (range.Overlaps(ignoreRange))
					return true;
			}
			return false;
		}
		/// <summary>
		/// Turns the specified lines into commented-out lines.
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="lines"></param>
		private void CommentOutLines(StringBuilder builder, List<IndexRange> lines)
		{
			for (int i = lines.Count - 1; i >= 0; i--)
			{
				IndexRange range = lines[i];
				this.textBuilder.Insert(range.Index, "// ");
			}
		}
	}
}
