﻿using System;
using System.IO;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses a <see cref="System.IO.TextWriter"/> as message destination.
	/// </summary>
	public class TextWriterLogOutput : ILogOutput
	{
		private	TextWriter target = null;

		public TextWriter Target
		{
			get { return this.target; }
		}

		public TextWriterLogOutput(TextWriter target)
		{
			this.target = target;
		}
		
		/// <inheritdoc />
		public virtual void Write(LogEntry entry)
		{
			int indent = entry.Source.Indent;
			string prefix = entry.Source.Prefix ?? "";
			string[] lines = entry.Message.Split(new[] { '\n', '\r', '\0' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++)
			{
				if (i == 0)
				{
					switch (entry.Type)
					{
						case LogMessageType.Message:
							lines[i] = prefix + "Msg: " + new string(' ', indent * 2) + lines[i];
							break;
						case LogMessageType.Warning:
							lines[i] = prefix + "Wrn: " + new string(' ', indent * 2) + lines[i];
							break;
						case LogMessageType.Error:
							lines[i] = prefix + "ERR: " + new string(' ', indent * 2) + lines[i];
							break;
					}
				}
				else
				{
					lines[i] = new string(' ', prefix.Length + 5 + indent * 2) + lines[i];
				}

				this.WriteLine(entry.Source, entry.Type, lines[i], entry.Context);
			}
		}
		/// <summary>
		/// Writes a single line of the final, formatted text message to the output.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="type"></param>
		/// <param name="formattedLine"></param>
		/// <param name="context"></param>
		protected virtual void WriteLine(Log source, LogMessageType type, string formattedLine, object context)
		{
			this.target.WriteLine(formattedLine);
		}
	}
}
