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
		private LogMessageType type;
		private string         message;
		private DateTime       timeStamp;
		private int            frameStamp;

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
		/// [GET] The messages timestamp in UTC.
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

		public LogEntry(LogMessageType type, string msg)
		{
			this.type = type;
			this.message = msg;
			this.timeStamp = DateTime.UtcNow;
			this.frameStamp = Time.FrameCount;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", this.type, this.message);
		}
	}
}
