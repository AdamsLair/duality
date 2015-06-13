using System;

namespace Duality.Launcher
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses the <see cref="System.Console"/> as message destination.
	/// </summary>
	public class ConsoleLogOutput : TextWriterLogOutput
	{
		public ConsoleLogOutput() : base(Console.Out) { }
		
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

			if (source == Log.Game)					Console.BackgroundColor = ConsoleColor.DarkGray;
			else if (source == Log.Core)			Console.BackgroundColor = ConsoleColor.DarkBlue;
			else if (source == Log.Editor)			Console.BackgroundColor = ConsoleColor.DarkMagenta;
			else									Console.BackgroundColor = ConsoleColor.Black;

			if (type == LogMessageType.Warning)		Console.ForegroundColor = ConsoleColor.Yellow;
			else if (type == LogMessageType.Error)	Console.ForegroundColor = ConsoleColor.Red;
			else									Console.ForegroundColor = ConsoleColor.Gray;

			base.Write(source, type, msg, context);

			Console.ForegroundColor = clrFg;
			Console.BackgroundColor = clrBg;
		}
	}
}
