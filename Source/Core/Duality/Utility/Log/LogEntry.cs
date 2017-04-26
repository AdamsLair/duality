using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Duality
{
	/// <summary>
	/// A log entry.
	/// </summary>
	public struct LogEntry
	{
		private Log			source;
		private LogMessageType type;
		private string		 message;
		private object		 context;
		private DateTime	   timeStamp;
		private int			frameStamp;
		private int			indent;

		/// <summary>
		/// [GET] The <see cref="Log"/> from which this entry originates.
		/// </summary>
		public Log Source
		{
			get { return this.source; }
		}
		/// <summary>
		/// [GET] The messages type.
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
			get { return this.message; }
		}
		/// <summary>
		/// [GET] The context in which this log was written. Usually the primary object the log entry is associated with.
		/// </summary>
		public object Context
		{
			get { return this.context; }
		}
		/// <summary>
		/// [GET] The messages timestamp.
		/// </summary>
		public DateTime TimeStamp
		{
			get { return this.timeStamp; }
		}
		/// <summary>
		/// [GET] The value of <see cref="Time.FrameCount"/> when the message was logged.
		/// </summary>
		public int FrameStamp
		{
			get { return this.frameStamp; }
		}
		/// <summary>
		/// [GET] The desired indentation level for this log message when displaying it.
		/// </summary>
		public int Indent
		{
			get { return this.indent; }
		}

		public LogEntry(Log source, LogMessageType type, string msg, object context)
		{
			this.source = source;
			this.type = type;
			this.message = msg;
			this.context = context;
			this.indent = source.Indent;
			this.timeStamp = DateTime.Now;
			this.frameStamp = Time.FrameCount;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", this.type, this.message);
		}
	}
}
