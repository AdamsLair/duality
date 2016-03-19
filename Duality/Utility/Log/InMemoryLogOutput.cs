using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that stores all log entries in memory.
	/// </summary>
	public class InMemoryLogOutput : ILogOutput
	{
		private RawList<LogEntry> data = new RawList<LogEntry>();

		/// <summary>
		/// [GET] Allows to access all log entries that have been received.
		/// </summary>
		public IReadOnlyList<LogEntry> Entries
		{
			get { return this.data; }
		}
		
		/// <inheritdoc />
		public void Write(LogEntry entry)
		{
			data.Add(entry);
		}
	}
}
