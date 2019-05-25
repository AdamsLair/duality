using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses a <see cref="System.IO.TextWriter"/> as message destination.
	/// </summary>
	public class TextWriterLogOutput : ILogOutput
	{
		private static readonly char[] LineEndingChars = new[] { '\n', '\r', '\0' };

		private	TextWriter target = null;
		private int indent = 0;
		private int prefixLength = 4;

		private StringBuilder builder = new StringBuilder();
		private object builderLock = new object();
		private object writerLock = new object();


		public TextWriter Target
		{
			get { return this.target; }
		}
		public int Indent
		{
			get { return this.indent; }
		}
		public int PrefixLength
		{
			get { return this.prefixLength; }
			set { this.prefixLength = value; }
		}


		public TextWriterLogOutput(TextWriter target)
		{
			this.target = target;
		}
		
		/// <inheritdoc />
		public virtual void Write(LogEntry entry, object context, Log source)
		{
			string[] lines = this.BuildLines(ref entry, source);

			lock (this.writerLock)
			{
				for (int i = 0; i < lines.Length; i++)
					this.WriteLine(source, entry.Type, lines[i], context);
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

		/// <inheritdoc />
		public void PushIndent()
		{
			Interlocked.Increment(ref this.indent);
		}
		/// <inheritdoc />
		public void PopIndent()
		{
			Interlocked.Decrement(ref this.indent);
		}

		private string[] BuildLines(ref LogEntry entry, Log source)
		{
			string prefix = source.Id;
			string[] lines = entry.Message.Split(LineEndingChars, StringSplitOptions.RemoveEmptyEntries);
			lock (this.builderLock)
			{
				int headerLength = 0;
				for (int i = 0; i < lines.Length; i++)
				{
					this.builder.Clear();
					if (i == 0)
					{
						// Channel / source prefix
						this.builder.Append('[');
						this.builder.Append(prefix, 0, Math.Min(prefix.Length, this.prefixLength));
						this.builder.Append(']');

						// Spacing
						this.builder.Append(' ', 1 + Math.Max(0, this.prefixLength - prefix.Length));

						// Message type
						switch (entry.Type)
						{
							case LogMessageType.Message: this.builder.Append("Msg: "); break;
							case LogMessageType.Warning: this.builder.Append("Wrn: "); break;
							case LogMessageType.Error: this.builder.Append("ERR: "); break;
						}

						// Indentation
						this.builder.Append(' ', this.indent * 2);

						headerLength = this.builder.Length;
					}
					else
					{
						this.builder.Append(' ', headerLength);
					}

					// Message line
					this.builder.Append(lines[i]);

					lines[i] = this.builder.ToString();
				}
			}
			return lines;
		}
	}
}
