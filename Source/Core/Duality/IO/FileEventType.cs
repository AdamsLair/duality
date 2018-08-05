using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Duality.IO
{
	/// <summary>
	/// Describes the type of a <see cref="FileEvent"/>. 
	/// 
	/// Note that this is a <see cref="FlagsAttribute"/> enum so it can also be used to describe event masks.
	/// </summary>
	[Flags]
	public enum FileEventType
	{
		/// <summary>
		/// Mask item representing no events at all.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// A new file or folder has been created.
		/// </summary>
		Created = 0x1,
		/// <summary>
		/// An existing file or folder has been deleted.
		/// </summary>
		Deleted = 0x2,
		/// <summary>
		/// An existing file or folder has been modified.
		/// </summary>
		Changed = 0x4,
		/// <summary>
		/// An existing file or folder has been renamed or moved.
		/// </summary>
		Renamed = 0x8,

		/// <summary>
		/// Mask item representing all events.
		/// </summary>
		All = Created | Deleted | Changed | Renamed
	}
}
