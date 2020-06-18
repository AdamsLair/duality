using System;
using Duality;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses the <see cref="System.Console"/> as message destination.
	/// </summary>
	public class ConsoleLogOutput : TextWriterLogOutput
	{
		private string[] lastLogLines = new string[3];
		private int lastLogLineIndex = 0;

		public ConsoleLogOutput() : base(Console.Out) { }
		
		protected override void WriteLine(Log source, LogMessageType type, string formattedLine, object context)
		{
			ConsoleColor clrBg = Console.BackgroundColor;
			ConsoleColor clrFg = Console.ForegroundColor;

			bool highlight = this.IsHighlightLine(source, formattedLine);

			// If we're writing the same kind of text again, "grey out" the repeating parts
			int beginGreyLength = 0;
			int endGreyLength   = 0;
			if (!highlight)
			{
				for (int i = 0; i < this.lastLogLines.Length; i++)
				{
					string lastLogLine = this.lastLogLines[i] ?? string.Empty;
					beginGreyLength = Math.Max(beginGreyLength, this.GetEqualBeginChars(lastLogLine, formattedLine));
					endGreyLength   = Math.Max(endGreyLength  , this.GetEqualEndChars  (lastLogLine, formattedLine));
				}
				if (beginGreyLength == formattedLine.Length)
					endGreyLength = 0;
				if (beginGreyLength + endGreyLength >= formattedLine.Length)
					endGreyLength = 0;
			}

			// Dark beginning
			if (beginGreyLength != 0)
			{
				this.SetDarkConsoleColor(type);
				this.Target.Write(formattedLine.Substring(0, beginGreyLength));
			}

			// Bright main part
			this.SetBrightConsoleColor(type, highlight);
			this.Target.Write(formattedLine.Substring(beginGreyLength, formattedLine.Length - beginGreyLength - endGreyLength));

			// Dark ending
			if (endGreyLength != 0)
			{
				this.SetDarkConsoleColor(type);
				this.Target.Write(formattedLine.Substring(formattedLine.Length - endGreyLength, endGreyLength));
			}

			// End the current line
			this.Target.WriteLine();

			this.lastLogLines[this.lastLogLineIndex] = formattedLine;
			this.lastLogLineIndex = (this.lastLogLineIndex + 1) % this.lastLogLines.Length;
			Console.ForegroundColor = clrFg;
			Console.BackgroundColor = clrBg;
		}

		private void SetConsoleBackColor(Log source)
		{
			if      (source == Logs.Core)   Console.BackgroundColor = ConsoleColor.DarkBlue;
			else if (source == Logs.Game)   Console.BackgroundColor = ConsoleColor.DarkCyan;
			else if (source == Logs.Editor) Console.BackgroundColor = ConsoleColor.DarkMagenta;
			else                           Console.BackgroundColor = ConsoleColor.Black;
		}
		private void SetDarkConsoleColor(LogMessageType type)
		{
			switch (type)
			{
				default:
				case LogMessageType.Message: Console.ForegroundColor = ConsoleColor.DarkGray;   break;
				case LogMessageType.Warning: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
				case LogMessageType.Error:   Console.ForegroundColor = ConsoleColor.DarkRed;    break;
			}
		}
		private void SetBrightConsoleColor(LogMessageType type, bool highlight)
		{
			switch (type)
			{
				default:
				case LogMessageType.Message: Console.ForegroundColor = highlight ? 
					                                                   ConsoleColor.White   : 
					                                                   ConsoleColor.Gray;   break;
				case LogMessageType.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
				case LogMessageType.Error:   Console.ForegroundColor = ConsoleColor.Red;    break;
			}
		}

		private bool IsHighlightLine(Log source, string line)
		{
			// If it's an indented line, don't highlight it
			if (this.Indent != 0) return false;

			// If the line ends with three dots, assume that it's the header of a series of actions
			if (line.EndsWith("...")) return true;

			return false;
		}

		private int GetEqualBeginChars(string a, string b)
		{
			int minLen = Math.Min(a.Length, b.Length);
			int lastBreakCount = 0;
			int i = 0;
			int j = 0;
			while (i < a.Length && j < b.Length)
			{
				// Skip whitespace / indentation
				if (a[i] == ' ') { ++i; continue; }
				if (b[j] == ' ') { ++j; lastBreakCount = j; continue; }

				if (a[i] != b[j])
					return lastBreakCount;

				if (!char.IsLetterOrDigit(b[j]))
					lastBreakCount = j + 1;

				++i;
				++j;
			}
			return minLen;
		}
		private int GetEqualEndChars(string a, string b)
		{
			int minLen = Math.Min(a.Length, b.Length);
			int lastBreakCount = 0;
			for (int i = 0; i < minLen; i++)
			{
				if (a[a.Length - 1 - i] != b[b.Length - 1 - i])
					return lastBreakCount;
				if (!char.IsLetterOrDigit(a[a.Length - 1 - i]))
					lastBreakCount = i + 1;
			}
			return minLen;
		}
	}
}
