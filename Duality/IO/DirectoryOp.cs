using System;
using System.Linq;
using System.Collections.Generic;

namespace Duality.IO
{
	/// <summary>
	/// Defines static methods for performing common file system operations on directories.
	/// </summary>
	public static class DirectoryOp
	{
		/// <summary>
		/// Returns whether the specified path refers to an existing directory.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool Exists(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return false;
			PathOp.CheckInvalidPathChars(path);
			return DualityApp.SystemBackend.FileSystem.DirectoryExists(path);
		}
		/// <summary>
		/// Creates a directory tree matching the specified directory path.
		/// </summary>
		/// <param name="path"></param>
		public static void Create(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("The specified path is null or whitespace-only.");
			PathOp.CheckInvalidPathChars(path);
			DualityApp.SystemBackend.FileSystem.CreateDirectory(path);
		}
		/// <summary>
		/// Deletes the directory that is referred to by the specified path.
		/// </summary>
		/// <param name="path"></param>
		public static void Delete(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return;
			PathOp.CheckInvalidPathChars(path);
			DualityApp.SystemBackend.FileSystem.DeleteDirectory(path);
		}
		/// <summary>
		/// Enumerates all files that are located within the specified path.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recursive">If true, the specified path will be searched recursively and yield all descendant file paths.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetFiles(string path, bool recursive = false)
		{
			if (string.IsNullOrWhiteSpace(path)) return Enumerable.Empty<string>();
			PathOp.CheckInvalidPathChars(path);
			return DualityApp.SystemBackend.FileSystem.GetFiles(path, recursive);
		}
		/// <summary>
		/// Enumerates all directories that are located within the specified path.
		/// </summary>
		/// <param name="path"></param>
		/// <param name="recursive">If true, the specified path will be searched recursively and yield all descendant directory paths.</param>
		/// <returns></returns>
		public static IEnumerable<string> GetDirectories(string path, bool recursive = false)
		{
			if (string.IsNullOrWhiteSpace(path)) return Enumerable.Empty<string>();
			PathOp.CheckInvalidPathChars(path);
			return DualityApp.SystemBackend.FileSystem.GetDirectories(path, recursive);
		}
	}
}
