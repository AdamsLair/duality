using System;
using System.Collections.Generic;
using System.Threading;

namespace Duality.Editor
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that captures all log entries, as well
	/// as their context, sources and additional information for in-editor usage.
	/// </summary>
	public class EditorLogOutput : ILogOutput
	{
		private int indent = 0;
		private int messageCount = 0;
		private int warningCount = 0;
		private int errorCount = 0;
		private RawList<EditorLogEntry> entries = new RawList<EditorLogEntry>();
		private object syncObj = new object();

		/// <summary>
		/// [GET] The total number of log entries that have been received.
		/// </summary>
		public int EntryCount
		{
			get { return this.entries.Count; }
		}
		/// <summary>
		/// [GET] The number of received log messages. This will not account for
		/// warnings or errors. For the total number of entries, see <see cref="EntryCount"/>.
		/// </summary>
		public int MessageCount
		{
			get { return this.messageCount; }
		}
		/// <summary>
		/// [GET] The number of received log warnings.
		/// </summary>
		public int WarningCount
		{
			get { return this.warningCount; }
		}
		/// <summary>
		/// [GET] The number of received log errors.
		/// </summary>
		public int ErrorCount
		{
			get { return this.errorCount; }
		}

		/// <summary>
		/// Retrieves the specified subset of log entries.
		/// </summary>
		/// <param name="target">An array that will store the retrieved log entries.</param>
		/// <param name="targetIndex">The starting index within the target array that entries will be written to.</param>
		/// <param name="index">The first log index that will be retrieved.</param>
		/// <param name="count">The number of log entries to be retrieved.</param>
		public void ReadEntries(EditorLogEntry[] target, int targetIndex, int index, int count)
		{
			lock (this.syncObj)
			{
				Array.Copy(
					this.entries.Data, index, 
					target, targetIndex, count);
			}
		}

		void ILogOutput.Write(LogEntry entry, object context, Log source)
		{
			EditorLogEntry extendedEntry = new EditorLogEntry(entry, context, source, this.indent);
			lock (this.syncObj)
			{
				this.entries.Add(extendedEntry);
				switch (entry.Type)
				{
					case LogMessageType.Error: this.errorCount++; break;
					case LogMessageType.Warning: this.warningCount++; break;
					default: this.messageCount++; break;
				}
			}
		}
		void ILogOutput.PushIndent()
		{
			Interlocked.Increment(ref this.indent);
		}
		void ILogOutput.PopIndent()
		{
			Interlocked.Decrement(ref this.indent);
		}
	}
}
