using System;
using System.IO;
using System.Threading;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses a <see cref="System.IO.TextWriter"/> as message destination.
	/// </summary>
	public class TextWriterLogOutput : ILogOutput
	{
		private	TextWriter target = null;
		private int indent = 0;
		private object writerLock = new object();

		public TextWriter Target
		{
			get { return this.target; }
		}
		public int Indent
		{
			get { return this.indent; }
		}

		public TextWriterLogOutput(TextWriter target)
		{
			this.target = target;
		}
		
		/// <inheritdoc />
		public virtual void Write(LogEntry entry, object context, Log source)
		{
			string prefix = source.Prefix ?? "";
			string[] lines = entry.Message.Split(new[] { '\n', '\r', '\0' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++)
			{
				if (i == 0)
				{
					switch (entry.Type)
					{
						case LogMessageType.Message:
							lines[i] = prefix + "Msg: " + new string(' ', this.indent * 2) + lines[i];
							break;
						case LogMessageType.Warning:
							lines[i] = prefix + "Wrn: " + new string(' ', this.indent * 2) + lines[i];
							break;
						case LogMessageType.Error:
							lines[i] = prefix + "ERR: " + new string(' ', this.indent * 2) + lines[i];
							break;
					}
				}
				else
				{
					lines[i] = new string(' ', prefix.Length + 5 + this.indent * 2) + lines[i];
				}
			}

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
	}
}
