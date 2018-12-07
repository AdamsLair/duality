using System;
using System.Diagnostics;
using System.IO;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that uses <see cref="System.Diagnostics.Debug"/> as message destination.
	/// 
	/// This output will only be active if the core was compiled in DEBUG mode. Otherwise, all messages arriving here will be ignored.
	/// </summary>
	internal class DebugLogOutput : TextWriterLogOutput
	{
		public DebugLogOutput() : base(null) { }

#if DEBUG
		protected override void WriteLine(Log source, LogMessageType type, string formattedLine, object context)
		{
			Debug.WriteLine(formattedLine);
		}
#else
		// Since System.Diagnostics.Debug is ignored when not compiling with the DEBUG 
		// directive, we can early-out with a NOP implementation if DEBUG is not defined.
		public override void Write(LogEntry entry, object context, Log source) { }
#endif
	}
}
