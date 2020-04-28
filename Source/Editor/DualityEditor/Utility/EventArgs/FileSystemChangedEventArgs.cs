using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	/// <summary>
	/// <see cref="EventArgs"/> providing information about a series of file system changes.
	/// </summary>
	public class FileSystemChangedEventArgs : EventArgs
	{
		private List<FileEvent> fileEvents = null;
		private FileEventType fileEventTypes = FileEventType.None;
		private FileEventType directoryEventTypes = FileEventType.None;

		/// <summary>
		/// A read-only list of file system events that took place.
		/// </summary>
		public IReadOnlyList<FileEvent> FileEvents
		{
			get { return this.fileEvents; }
		}


		public FileSystemChangedEventArgs(IEnumerable<FileEvent> fileEvents)
		{
			this.fileEvents = new List<FileEvent>(fileEvents);
			foreach (FileEvent item in this.fileEvents)
			{
				if (item.IsDirectory)
					this.directoryEventTypes |= item.Type;
				else
					this.fileEventTypes |= item.Type;
			}
		}
		public FileSystemChangedEventArgs(FileEventQueue fileEventQueue) : this(fileEventQueue.Items) { }

		/// <summary>
		/// Returns whether any changes of the specified type were made.
		/// 
		/// Note that <paramref name="type"/> can be used as a bitmask including
		/// multiple types, in which case any detected match will yield a "true" result.
		/// </summary>
		/// <param name="type"></param>
		public bool Any(FileEventType type)
		{
			return 
				(this.fileEventTypes & type) != FileEventType.None ||
				(this.directoryEventTypes & type) != FileEventType.None;
		}
		/// <summary>
		/// Returns whether any file changes of the specified type were made.
		/// 
		/// Note that <paramref name="type"/> can be used as a bitmask including
		/// multiple types, in which case any detected match will yield a "true" result.
		/// </summary>
		/// <param name="type"></param>
		public bool AnyFiles(FileEventType type)
		{
			return (this.fileEventTypes & type) != FileEventType.None;
		}
		/// <summary>
		/// Returns whether any directory changes of the specified type were made.
		/// 
		/// Note that <paramref name="type"/> can be used as a bitmask including
		/// multiple types, in which case any detected match will yield a "true" result.
		/// </summary>
		/// <param name="type"></param>
		public bool AnyDirectories(FileEventType type)
		{
			return (this.directoryEventTypes & type) != FileEventType.None;
		}

		/// <summary>
		/// Returns whether a change was made on the specified path.
		/// 
		/// Note that <paramref name="type"/> can be used as a bitmask including
		/// multiple types, in which case any detected match will yield a "true" result.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="path"></param>
		public bool Contains(FileEventType type, string path)
		{
			// Early-out to avoid iterating if we don't have anything of that type
			if (!this.Any(type)) return false;
			if (path == null) return false;

			foreach (FileEvent item in this.fileEvents)
			{
				if ((item.Type & type) == FileEventType.None) continue;
				if (item.Path == path || item.OldPath == path)
					return true;
			}
			return false;
		}
		/// <summary>
		/// Returns whether a change was made on any of the specified paths.
		/// 
		/// Note that <paramref name="type"/> can be used as a bitmask including
		/// multiple types, in which case any detected match will yield a "true" result.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="paths"></param>
		public bool Contains(FileEventType type, IEnumerable<string> paths)
		{
			// Early-out to avoid iterating if we don't have anything of that type
			if (!this.Any(type)) return false;

			foreach (string path in paths)
			{
				if (this.Contains(type, path))
					return true;
			}
			return false;
		}
	}
}
