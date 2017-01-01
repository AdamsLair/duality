using System;
using System.IO;
using System.Text;
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
		private int prefixLength = 4;
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
			StringBuilder builder = new StringBuilder();

			string prefix = source.Id;
			string[] lines = entry.Message.Split(new[] { '\n', '\r', '\0' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < lines.Length; i++)
			{
				builder.Clear();
				if (i == 0)
				{
					builder.Append('[');
					builder.Append(prefix, 0, Math.Min(prefix.Length, this.prefixLength));
					builder.Append("] ");
					switch (entry.Type)
					{
						case LogMessageType.Message: builder.Append("Msg: "); break;
						case LogMessageType.Warning: builder.Append("Wrn: "); break;
						case LogMessageType.Error:   builder.Append("ERR: "); break;
					}
					builder.Append(' ', this.indent * 2);
					builder.Append(lines[i]);
				}
				else
				{
					builder.Append(' ', this.prefixLength + 3 + 5 + this.indent * 2);
					builder.Append(lines[i]);
				}
				lines[i] = builder.ToString();
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
