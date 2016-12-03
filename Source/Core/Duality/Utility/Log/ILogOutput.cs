using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality
{
	/// <summary>
	/// Represents a single <see cref="Log"/> output and provides actual writing functionality for 
	/// </summary>
	public interface ILogOutput
	{
		/// <summary>
		/// Writes a single message to the output.
		/// </summary>
		/// <param name="entry">The new log entry that is to be written to the output.</param>
		/// <param name="context">The runtime context object of this log entry.</param>
		/// <param name="source">The <see cref="Log"/> instance that issued the log entry.</param>
		void Write(LogEntry entry, object context, Log source);
	}
}
