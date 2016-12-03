using System;
using System.Collections.Generic;
using System.Reflection;

namespace Duality.Editor
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that captures all log entries, as well
	/// as their context, sources and additional information for in-editor usage.
	/// </summary>
	/// <remarks>
	/// This class synchronizes incoming logs from different threads, but is otherwise not
	/// thread-safe. Only read its data in a single thread at a time.
	/// </remarks>
	public class EditorLogOutput : ILogOutput
	{
		private int indent = 0;
		private RawList<EditorLogEntry> entries = new RawList<EditorLogEntry>();
		private RawList<EditorLogEntry> schedule = new RawList<EditorLogEntry>();
		private object syncObj = new object();

		/// <summary>
		/// [GET] A list of all log entries that have been received by this output.
		/// Each item is enriched with diagnostic information for in-editor usage.
		/// </summary>
		public IReadOnlyList<EditorLogEntry> Entries
		{
			get
			{ 
				this.Synchronize();
				return this.entries;
			}
		}
		
		private void Synchronize()
		{
			if (this.schedule.Count == 0) return;
			lock (this.syncObj)
			{
				int oldCount = this.entries.Count;
				this.entries.Count += this.schedule.Count;
				Array.Copy(
					this.schedule.Data, 0, 
					this.entries.Data, oldCount, this.schedule.Count);
				this.schedule.Clear();
			}
		}

		void ILogOutput.Write(LogEntry entry, object context, Log source)
		{
			EditorLogEntry extendedEntry = new EditorLogEntry(entry, context, source, this.indent);
			lock (this.syncObj)
			{
				this.schedule.Add(extendedEntry);
			}
		}
		void ILogOutput.PushIndent()
		{
			this.indent++;
		}
		void ILogOutput.PopIndent()
		{
			this.indent--;
		}
	}
}
