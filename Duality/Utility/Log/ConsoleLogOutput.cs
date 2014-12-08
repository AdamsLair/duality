using System;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses the <see cref="System.Console"/> as message destination.
	/// </summary>
	public class ConsoleLogOutput : TextWriterLogOutput
	{
		private	ConsoleColor	bgColor;

		public ConsoleLogOutput(ConsoleColor bgColor = ConsoleColor.Black) : base(Console.Out)
		{
			this.bgColor = bgColor;
		}
		
		/// <summary>
		/// Writes a single message to the output.
		/// </summary>
		/// <param name="source">The <see cref="Log"/> from which the message originates.</param>
		/// <param name="type">The type of the log message.</param>
		/// <param name="msg">The message to write.</param>
		/// <param name="context">The context in which this log was written. Usually the primary object the log entry is associated with.</param>
		public override void Write(Log source, LogMessageType type, string msg, object context)
		{
			ConsoleColor clrBg = Console.BackgroundColor;
			ConsoleColor clrFg = Console.ForegroundColor;

			Console.BackgroundColor = this.bgColor;
			if (type == LogMessageType.Warning)		Console.ForegroundColor = ConsoleColor.Yellow;
			else if (type == LogMessageType.Error)	Console.ForegroundColor = ConsoleColor.Red;
			else									Console.ForegroundColor = ConsoleColor.Gray;

			base.Write(source, type, msg, context);

			Console.ForegroundColor = clrFg;
			Console.BackgroundColor = clrBg;
		}
	}
}
