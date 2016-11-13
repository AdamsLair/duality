using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.IO;

namespace Duality.Backend
{
	public interface ISystemBackend : IDualityBackend
	{
		/// <summary>
		/// [GET] An interface that provides file system access on the current platform.
		/// </summary>
		IFileSystem FileSystem { get; }

		/// <summary>
		/// Retrieves the path of a named / special directory.
		/// </summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		string GetNamedPath(NamedDirectory dir);
	}
}
