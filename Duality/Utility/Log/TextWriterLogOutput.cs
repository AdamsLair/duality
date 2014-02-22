using System;
using System.IO;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses a <see cref="System.IO.TextWriter"/> as message destination.
	/// </summary>
	public class TextWriterLogOutput : ILogOutput
	{
		private	TextWriter		writer	= null;

		public TextWriterLogOutput(TextWriter writer)
		{
			this.writer = writer;
		}
		
		/// <summary>
		/// Writes a single message to the output.
		/// </summary>
		/// <param name="source">The <see cref="Log"/> from which the message originates.</param>
		/// <param name="type">The type of the log message.</param>
		/// <param name="msg">The message to write.</param>
		public virtual void Write(Log source, LogMessageType type, string msg)
		{
			int indent = source.Indent;
			string[] lines = msg.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++)
			{
				if (i == 0)
				{
					switch (type)
					{
						case LogMessageType.Message:
							lines[i] = source.Prefix + "Info:    " + new string(' ', indent * 4) + lines[i];
							break;
						case LogMessageType.Warning:
							lines[i] = source.Prefix + "Warning: " + new string(' ', indent * 4) + lines[i];
							break;
						case LogMessageType.Error:
							lines[i] = source.Prefix + "ERROR:   " + new string(' ', indent * 4) + lines[i];
							break;
					}
				}
				else
				{
					lines[i] = source.Prefix + "         " + new string(' ', indent * 4) + lines[i];
				}

				this.writer.WriteLine(lines[i]);
			}
		}
	}
}
