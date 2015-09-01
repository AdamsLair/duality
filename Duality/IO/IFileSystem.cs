using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Duality.IO
{
	/// <summary>
	/// A file system allows to perform read / write operations on a virtual or actual storage device.
	/// All paths are expected to match <see cref="Duality.IO.PathOp">Duality's path format</see>.
	/// </summary>
	public interface IFileSystem
	{
		/// <summary>
		/// Returns a rooted version of the specified path, which uniquely identifies the referenced file system entity.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetFullPath(string path);

		/// <summary>
		/// Enumerates all files that are located within the specified path.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recursive">If true, the specified path will be searched recursively and yield all descendant file paths.</param>
		/// <returns></returns>
		IEnumerable<string> GetFiles(string path, bool recursive = false);
		/// <summary>
		/// Enumerates all directories that are located within the specified path.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recursive">If true, the specified path will be searched recursively and yield all descendant directory paths.</param>
		/// <returns></returns>
		IEnumerable<string> GetDirectories(string path, bool recursive = false);

		/// <summary>
		/// Returns whether the specified path refers to an existing file.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool FileExists(string path);
		/// <summary>
		/// Returns whether the specified path refers to an existing directory.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool DirectoryExists(string path);

		/// <summary>
		/// Creates or overwrites a file at the specified path and returns a <see cref="System.IO.Stream"/> to it.
		/// The returned stream has implicit read / write access.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		Stream CreateFile(string path);
		/// <summary>
		/// Opens an existing file at the specified path and returns a <see cref="System.IO.Stream"/> to it.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		Stream OpenFile(string path, FileAccessMode mode);
		/// <summary>
		/// Deletes the file that is referred to by the specified path.
		/// </summary>
		/// <param name="path"></param>
		void DeleteFile(string path);

		/// <summary>
		/// Creates a directory tree matching the specified directory path.
		/// </summary>
		/// <param name="path"></param>
		void CreateDirectory(string path);
		/// <summary>
		/// Deletes the directory that is referred to by the specified path.
		/// </summary>
		/// <param name="path"></param>
		void DeleteDirectory(string path);
	}
}
