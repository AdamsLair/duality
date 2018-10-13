using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.IO;

namespace Duality.Editor
{
	/// <summary>
	/// <see cref="EventArgs"/> providing information about a series of file system changes
	/// regarding <see cref="Resource"/> files, or directories containing them.
	/// </summary>
	public class ResourceFilesChangedEventArgs : FileSystemChangedEventArgs
	{
		public ResourceFilesChangedEventArgs(IEnumerable<FileEvent> fileEvents) : base(fileEvents) { }
		public ResourceFilesChangedEventArgs(FileEventQueue fileEventQueue) : base(fileEventQueue) { }

		/// <summary>
		/// Returns whether a change was made on the specified <see cref="Resource"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		/// <seealso cref="FileSystemChangedEventArgs.Contains(FileEventType, string)"/>
		public bool Contains(FileEventType type, Resource content)
		{
			if (content == null) return false;
			return this.Contains(type, content.Path);
		}
		/// <summary>
		/// Returns whether a change was made on the specified <see cref="Resource"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		/// <seealso cref="FileSystemChangedEventArgs.Contains(FileEventType, string)"/>
		public bool Contains(FileEventType type, ContentRef<Resource> content)
		{
			return this.Contains(type, content.Path);
		}
		/// <summary>
		/// Returns whether a change was made on any of the specified <see cref="Resource"/> instances.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		/// <seealso cref="FileSystemChangedEventArgs.Contains(FileEventType, IEnumerable{string})"/>
		public bool Contains(FileEventType type, IEnumerable<Resource> content)
		{
			// Early-out to avoid iterating if we don't have anything of that type
			if (!this.Any(type)) return false;

			foreach (Resource item in content)
			{
				if (this.Contains(type, item))
					return true;
			}
			return false;
		}
		/// <summary>
		/// Returns whether a change was made on any of the specified <see cref="Resource"/> instances.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		/// <seealso cref="FileSystemChangedEventArgs.Contains(FileEventType, IEnumerable{string})"/>
		public bool Contains(FileEventType type, IEnumerable<ContentRef<Resource>> content)
		{
			// Early-out to avoid iterating if we don't have anything of that type
			if (!this.Any(type)) return false;

			foreach (ContentRef<Resource> item in content)
			{
				if (this.Contains(type, item))
					return true;
			}
			return false;
		}
		/// <summary>
		/// Returns whether a change was made on a <see cref="Resource"/> of the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		/// <seealso cref="FileSystemChangedEventArgs.Contains(FileEventType, string)"/>
		public bool Contains(FileEventType type, Type contentType)
		{
			// Early-out to avoid iterating if we don't have anything of that type
			if (!this.Any(type)) return false;
			if (contentType == null) return false;

			foreach (FileEvent item in this.FileEvents)
			{
				if ((item.Type & type) == FileEventType.None) continue;
				if (item.IsDirectory) continue;

				ContentRef<Resource> content = new ContentRef<Resource>(item.Path);
				if (contentType.IsAssignableFrom(content.ResType)) return true;

				if (item.Type == FileEventType.Renamed)
				{
					ContentRef<Resource> oldContent = new ContentRef<Resource>(item.OldPath);
					if (contentType.IsAssignableFrom(oldContent.ResType)) return true;
				}
			}
			return false;
		}
	}
}
