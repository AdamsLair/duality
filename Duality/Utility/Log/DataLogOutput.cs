using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that stores all log entries in memory.
	/// </summary>
	public class DataLogOutput : ILogOutput
	{
		/// <summary>
		/// EventArgs for <see cref="LogEntry">LogEntries</see>.
		/// </summary>
		public class LogEntryEventArgs : EventArgs
		{
			private LogEntry	entry;
			public LogEntry Entry
			{
				get { return this.entry; }
			}
			public LogEntryEventArgs(LogEntry entry)
			{
				this.entry = entry;
			}
		}

		/// <summary>
		/// A log entry.
		/// </summary>
		public class LogEntry
		{
			private	Log				source;
			private	LogMessageType	type;
			private	string			msg;
			private	int				indent;
			private	DateTime		timestamp;
			private	int				frameIndex;

			/// <summary>
			/// [GET] The <see cref="Log"/> from which this entry originates.
			/// </summary>
			public Log Source
			{
				get { return this.source; }
			}
			/// <summary>
			/// [GET] The message's type.
			/// </summary>
			public LogMessageType Type
			{
				get { return this.type; }
			}
			/// <summary>
			/// [GET] The log entry's message.
			/// </summary>
			public string Message
			{
				get { return this.msg; }
			}
			/// <summary>
			/// [GET] The message's indent value.
			/// </summary>
			public int Indent
			{
				get { return this.indent; }
			}
			/// <summary>
			/// [GET] The messages timestamp.
			/// </summary>
			public DateTime Timestamp
			{
				get { return this.timestamp; }
			}
			/// <summary>
			/// [GET] The value of <see cref="Time.FrameCount"/> when the message was logged.
			/// </summary>
			public int FrameIndex
			{
				get { return this.frameIndex; }
			}

			public LogEntry(Log source, LogMessageType type, string msg)
			{
				this.source = source;
				this.type = type;
				this.msg = msg;
				this.indent = source.Indent;
				this.timestamp = DateTime.Now;
				this.frameIndex = Time.FrameCount;
			}
		}

		private	List<LogEntry>	data	= new List<LogEntry>();

		public event EventHandler<LogEntryEventArgs> NewEntry = null;

		/// <summary>
		/// [GET] Enumerates all log entries that have been made.
		/// </summary>
		public IEnumerable<LogEntry> Data
		{
			get { return this.data; }
		}
		
		/// <summary>
		/// Writes a single message to the output.
		/// </summary>
		/// <param name="source">The <see cref="Log"/> from which the message originates.</param>
		/// <param name="type">The type of the log message.</param>
		/// <param name="msg">The message to write.</param>
		public void Write(Log source, LogMessageType type, string msg)
		{
			LogEntry entry;
			entry = new LogEntry(source, type, msg);
			data.Add(entry);
			this.OnNewEntry(entry);
		}
		private void OnNewEntry(LogEntry entry)
		{
			if (this.NewEntry != null)
				this.NewEntry(this, new LogEntryEventArgs(entry));
		}
	}
}
