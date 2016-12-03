using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Editor
{
	/// <summary>
	/// Represents a log entry that was received by an <see cref="EditorLogOutput"/> and
	/// enriched with additional diagnostic information.
	/// </summary>
	public struct EditorLogEntry
	{
		private LogEntry baseEntry;
		private Log source;
		private object context;
		private int indent;

		/// <summary>
		/// [GET] The core log entry that contains message, timestamps and other basic information.
		/// </summary>
		public LogEntry Content
		{
			get { return this.baseEntry; }
		}
		/// <summary>
		/// [GET] The <see cref="Log"/> instance that issued the log entry.
		/// </summary>
		public Log Source
		{
			get { return this.source; }
		}
		/// <summary>
		/// [GET] A reference to the context object of this log entry, if one was provided.
		/// </summary>
		public object Context
		{
			get { return this.context; }
		}
		/// <summary>
		/// [GET] The indentation level of the receiving editor log output at the time of
		/// this log entry being added.
		/// </summary>
		public int Indent
		{
			get { return this.indent; }
		}

		public EditorLogEntry(LogEntry baseEntry, object context, Log source, int indent)
		{
			this.baseEntry = baseEntry;
			this.source = source;
			this.context = context;
			this.indent = indent;
		}

		public override string ToString()
		{
			return this.baseEntry.ToString();
		}
	}
}
