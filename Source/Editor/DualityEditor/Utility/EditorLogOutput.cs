using System;
using System.Collections.Generic;
using System.Reflection;

namespace Duality.Editor
{
	/// <summary>
	/// A <see cref="ILogOutput">Log output</see> that captures all log entries, as well
	/// as their context, sources and additional information for in-editor usage.
	/// </summary>
	public class EditorLogOutput : ILogOutput
	{
		private RawList<EditorLogEntry> data = new RawList<EditorLogEntry>();

		/// <summary>
		/// [GET] Allows to access all log entries that have been received.
		/// </summary>
		public IReadOnlyList<EditorLogEntry> Entries
		{
			get { return this.data; }
		}
		
		/// <inheritdoc />
		public void Write(LogEntry entry, object context, Log source)
		{
			data.Add(new EditorLogEntry(entry, context, source));
		}
	}
}
