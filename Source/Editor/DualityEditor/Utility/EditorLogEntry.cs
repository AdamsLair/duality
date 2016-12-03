using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Editor
{
	/// <summary>
	/// A log entry, enriched with additional diagnostic information
	/// for in-editor usage.
	/// </summary>
	public struct EditorLogEntry
	{
		private LogEntry baseEntry;
		private Log source;
		private object context;

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

		public EditorLogEntry(LogEntry baseEntry, object context, Log source)
		{
			this.baseEntry = baseEntry;
			this.source = source;
			this.context = context;
		}

		public override string ToString()
		{
			return this.baseEntry.ToString();
		}
	}
}
