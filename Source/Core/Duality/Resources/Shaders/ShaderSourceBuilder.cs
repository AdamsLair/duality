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
			public void Trim(IndexRange exclude)
			{
				// Trim end where it overlaps
				if (exclude.Index >= this.Index && 
					exclude.Index < this.Index + this.Length && 
					exclude.Index + exclude.Length >= this.Index + this.Length)
				{
					int overlapSize = (this.Index + this.Length) - exclude.Index;
					this.Length -= overlapSize;
					if (this.Length < 0) this.Length = 0;
				}
				// Trim start where it overlaps
				if (this.Index >= exclude.Index && 
					this.Index < exclude.Index + exclude.Length &&
					this.Index + this.Length >= exclude.Index + exclude.Length)
				{
					int overlapSize = (exclude.Index + exclude.Length) - this.Index;
					this.Index += overlapSize;
					this.Length -= overlapSize;
					if (this.Length < 0) this.Length = 0;
				}
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
		private static readonly Regex RegexMetadataDirective = new Regex(@"#pragma\s+duality\s+(.+)");


		private string mainChunk = string.Empty;
		private List<string> sharedChunk = new List<string>();
		private List<string> conditionalSymbols = new List<string>();
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
		/// Sets or removes a shared conditional compilation symbol to be included as a 
		/// preprocessor define in the resulting shader.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="isActive"></param>
		public void SetConditional(string name, bool isActive)
		{
			bool hasSymbol = this.conditionalSymbols.Contains(name);
			if (isActive && !hasSymbol)
				this.conditionalSymbols.Add(name);
			else if (!isActive && hasSymbol)
				this.conditionalSymbols.Remove(name);
		}

		/// <summary>
		/// Builds the merged, preprocessed source code.
		/// </summary>
		/// <returns></returns>
		public string Build()
		{
			this.textBuilder.Clear();

			// Append preprocessor defines at the top
			if (this.conditionalSymbols.Count > 0)
			{
				this.textBuilder.AppendFormat("#line {0}", (this.sharedChunk.Count + 1) * 10000);
				this.textBuilder.AppendLine();
				for (int i = 0; i < this.conditionalSymbols.Count; i++)
				{
					this.textBuilder.AppendFormat("#define {0}", this.conditionalSymbols[i]);
					this.textBuilder.AppendLine();
				}
				this.textBuilder.AppendLine();
			}

			// Append shared chunks before any other code
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
					match.Index, 
					match.Length));
			}
			foreach (Match match in RegexLineComment.Matches(rawMerge))
			{
				ignoreRegions.Add(new IndexRange(
					match.Index,
					match.Length));
			}

			// Identify all metadata directives
			Dictionary<IndexRange, List<IndexRange>> metadataBlocks = this.IdentifyMetadataBlocks(
				rawMerge, 
				ignoreRegions);

			// Identify (shared) variable declarations
			List<IndexRange> removeLines = new List<IndexRange>();
			this.RemoveDoubleVariableDeclarations(
				rawMerge, 
				ignoreRegions, 
				removeLines, 
				metadataBlocks);

			// Identify version directives
			int lastVersion = this.RemoveVersionDirectives(
				rawMerge, 
				ignoreRegions, 
				removeLines);

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
		/// Identify continuous blocks of variable metadata directives, mapped to the range of source code
		/// they're located in.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="ignoreRegions"></param>
		/// <returns></returns>
		private Dictionary<IndexRange,List<IndexRange>> IdentifyMetadataBlocks(string source, List<IndexRange> ignoreRegions)
		{
			Dictionary<IndexRange, List<IndexRange>> metadataBlocks = new Dictionary<IndexRange, List<IndexRange>>();

			using (StringReader reader = new StringReader(source))
			{
				List<IndexRange> currentBlock = new List<IndexRange>();
				IndexRange currentBlockRange = new IndexRange(0, 0);
				int lineIndex = 0;
				while (true)
				{
					string line = reader.ReadLine();
					if (line == null) break;

					// Trim the current line, so ignored ranges are removed
					IndexRange lineRange = new IndexRange(lineIndex, line.Length);
					IndexRange trimmedLineRange = lineRange;
					foreach (IndexRange ignoreRange in ignoreRegions)
					{
						trimmedLineRange.Trim(ignoreRange);
						if (trimmedLineRange.Length == 0) break;
					}
					string trimmedLine =
						(trimmedLineRange.Length == 0) ?
						string.Empty :
						source.Substring(trimmedLineRange.Index, trimmedLineRange.Length);

					// Process the current line
					bool isLineEmpty = string.IsNullOrWhiteSpace(trimmedLine);
					if (!isLineEmpty)
					{
						Match match = RegexMetadataDirective.Match(trimmedLine);
						if (match != null && match.Length > 0)
						{
							// Extend the current metadata block to include the detected directive
							IndexRange metadataRange = new IndexRange(trimmedLineRange.Index + match.Index, match.Length);
							metadataRange = this.ExpandToLine(source, metadataRange);
							currentBlock.Add(metadataRange);
							currentBlockRange.Length = (metadataRange.Index + metadataRange.Length) - currentBlockRange.Index;
						}
						else
						{
							// Close the current metadata block
							if (currentBlock.Count > 0)
							{
								metadataBlocks.Add(currentBlockRange, new List<IndexRange>(currentBlock));
							}

							// Start a new metadata block
							currentBlock.Clear();
							currentBlockRange.Index = lineRange.Index + lineRange.Length + Environment.NewLine.Length;
							currentBlockRange.Length = 0;
						}
					}
					else
					{
						// If we have a current block, incorporate comment and empty lines into it until it ends
						if (currentBlock.Count > 0)
						{
							currentBlockRange.Length = (lineRange.Index + lineRange.Length) - currentBlockRange.Index;
						}
						// Otherwise, move the start of the current block forward until we actually find a metadata line
						else
						{
							currentBlockRange.Index = lineRange.Index + lineRange.Length + Environment.NewLine.Length;
							currentBlockRange.Length = 0;
						}
					}

					lineIndex += line.Length;
					lineIndex += Environment.NewLine.Length;
				}
			}

			return metadataBlocks;
		}
		/// <summary>
		/// Detects double variable declarations and schedules all but the first one for removal.
		/// Will also schedule additional lines for removal that are considered to be part of the 
		/// variable declaration, such as metadata directives.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="ignoreRegions"></param>
		/// <param name="removeSchedule"></param>
		/// <param name="metadataBlocks"></param>
		private void RemoveDoubleVariableDeclarations(string source, List<IndexRange> ignoreRegions, List<IndexRange> removeSchedule, Dictionary<IndexRange, List<IndexRange>> metadataBlocks)
		{
			HashSet<string> variableDeclarations = new HashSet<string>();
			foreach (Match match in RegexVariableDeclaration.Matches(source))
			{
				IndexRange range = new IndexRange(match.Index, match.Length);
				if (this.AnyRangeOverlap(range, ignoreRegions)) continue;

				// Normalize declaration, so we can compare it to others and match regardless of spacing,
				// declaring keyword or optional qualifiers.
				string normalizedDecl = (match.Groups[2].Value + match.Groups[3].Value);

				// If we previously saw that declaration, schedule it for removal
				if (!variableDeclarations.Add(normalizedDecl))
				{
					IndexRange lineRange = this.ExpandToLine(source, range);
					if (lineRange.Length > 0) removeSchedule.Add(lineRange);

					// Schedule all matching metadata declarations for removal as well
					foreach (var block in metadataBlocks)
					{
						if (block.Key.Index >= lineRange.Index) continue;
						if (block.Key.Index + block.Key.Length + Environment.NewLine.Length < lineRange.Index) continue;

						foreach (IndexRange metadataLine in block.Value)
						{
							removeSchedule.Add(metadataLine);
						}
						break;
					}
				}
			}
		}
		/// <summary>
		/// Detects double version directives and schedules them for removal.
		/// Returns the last encountered version.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="ignoreRegions"></param>
		/// <param name="removeSchedule"></param>
		private int RemoveVersionDirectives(string source, List<IndexRange> ignoreRegions, List<IndexRange> removeSchedule)
		{
			int lastVersion = 0;
			foreach (Match match in RegexVersionDirective.Matches(source))
			{
				IndexRange range = new IndexRange(match.Index, match.Length);
				if (this.AnyRangeOverlap(range, ignoreRegions)) continue;

				int version;
				if (int.TryParse(match.Groups[1].Value, out version))
					lastVersion = version;

				IndexRange lineRange = this.ExpandToLine(source, range);
				if (lineRange.Length > 0)
					removeSchedule.Add(lineRange);
			}
			return lastVersion;
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
			lines.Sort((a, b) => a.Index.CompareTo(b.Index));
			for (int i = lines.Count - 1; i >= 0; i--)
			{
				IndexRange range = lines[i];
				this.textBuilder.Insert(range.Index, "// ");
			}
		}
	}
}
