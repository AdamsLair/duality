using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Duality.IO
{
	/// <summary>
	/// A data structure that describes a file system event.
	/// </summary>
	public struct FileEvent : IEquatable<FileEvent>
	{
		/// <summary>
		/// The path of the file or folder that was affected by the event.
		/// </summary>
		public string Path;
		/// <summary>
		/// The old path of a renamed file or folder.
		/// </summary>
		public string OldPath;
		/// <summary>
		/// Whether the event refers to a file or folder.
		/// </summary>
		public bool IsDirectory;
		/// <summary>
		/// The type of event that took place.
		/// </summary>
		public FileEventType Type;


		public FileEvent(FileEventType type, string path, bool isDirectory)
		{
			this.Type = type;
			this.Path = path;
			this.OldPath = path;
			this.IsDirectory = isDirectory;
		}
		public FileEvent(FileEventType type, string oldPath, string path, bool isDirectory)
		{
			this.Type = type;
			this.Path = path;
			this.OldPath = oldPath;
			this.IsDirectory = isDirectory;
		}

		public bool Equals(FileEvent other)
		{
			return
				this.Path == other.Path &&
				this.OldPath == other.OldPath &&
				this.IsDirectory == other.IsDirectory &&
				this.Type == other.Type;
		}
		public override bool Equals(object obj)
		{
			if (obj is FileEvent)
				return this.Equals((FileEvent)obj);
			else
				return false;
		}
		public override int GetHashCode()
		{
			int hash = 17;
			MathF.CombineHashCode(ref hash, this.Path != null ? this.Path.GetHashCode() : 0);
			MathF.CombineHashCode(ref hash, this.OldPath != null ? this.OldPath.GetHashCode() : 0);
			MathF.CombineHashCode(ref hash, this.IsDirectory ? 23 : 0);
			MathF.CombineHashCode(ref hash, (int)this.Type);
			return hash;
		}
		public override string ToString()
		{
			if (this.Type == FileEventType.Renamed)
				return string.Format("{0} {1} '{2}' to '{3}'", this.Type, this.IsDirectory ? "Dir" : "File", this.OldPath, this.Path);
			else
				return string.Format("{0} {1} '{2}'", this.Type, this.IsDirectory ? "Dir" : "File", this.Path);
		}
	}
}
