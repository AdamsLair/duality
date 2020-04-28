using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Duality.IO
{
	/// <summary>
	/// Defines static methods for performing common operations on path strings, such as combining them or extracting file extensions.
	/// This class mirrors the functionality of <see cref="System.IO.Path"/> in a platform-agnostic way using Duality's path format.
	/// </summary>
	public static class PathOp
	{
		public static readonly char DirectorySeparatorChar;
		public static readonly char AltDirectorySeparatorChar;
		public static readonly char VolumeSeparatorChar;
		public static readonly char ExtensionSeparatorChar;

		private static readonly char[] DirectorySeparatorChars;
		private static readonly char[] InvalidPathChars;
		private static readonly char[] InvalidFileNameChars;

		static PathOp()
		{
			AltDirectorySeparatorChar = '/';
			DirectorySeparatorChar = '\\';
			VolumeSeparatorChar = ':';
			ExtensionSeparatorChar = '.';

			DirectorySeparatorChars = new char[] { DirectorySeparatorChar, AltDirectorySeparatorChar };

			char[] asciiBelow32 = new char[32];
			for (int i = 0; i < asciiBelow32.Length; i++)
			{
				asciiBelow32[i] = (char)i;
			}

			InvalidPathChars = new char[asciiBelow32.Length + 6];
			InvalidPathChars[0] = '"';
			InvalidPathChars[1] = '<';
			InvalidPathChars[2] = '>';
			InvalidPathChars[3] = '|';
			InvalidPathChars[4] = '*';
			InvalidPathChars[5] = '?';
			for (int i = 0; i < asciiBelow32.Length; i++)
			{
				InvalidPathChars[i + 6] = asciiBelow32[i];
			}
			
			InvalidFileNameChars = new char[InvalidPathChars.Length + 3];
			InvalidFileNameChars[0] = DirectorySeparatorChar;
			InvalidFileNameChars[1] = AltDirectorySeparatorChar;
			InvalidFileNameChars[2] = VolumeSeparatorChar;
			for (int i = 0; i < InvalidPathChars.Length; i++)
			{
				InvalidFileNameChars[i + 3] = InvalidPathChars[i];
			}
		}

		/// <summary>
		/// Determines whether the specified path begins with a file system root.
		/// </summary>
		/// <param name="path"></param>
		public static bool IsPathRooted(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return false;

			CheckInvalidPathChars(path);

			return 
				path[0] == DirectorySeparatorChar || 
				path[0] == AltDirectorySeparatorChar ||
				(path.Length > 1 && path[1] == VolumeSeparatorChar);
		}
		/// <summary>
		/// Returns a rooted version of the specified path, which uniquely identifies the referenced file system entity.
		/// Unlike most methods of <see cref="PathOp"/>, this method accesses the file system.
		/// </summary>
		/// <param name="path"></param>
		public static string GetFullPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return string.Empty;

			CheckInvalidPathChars(path);

			return DualityApp.SystemBackend.FileSystem.GetFullPath(path);
		}
		/// <summary>
		/// Returns whether two paths are referring to the same file system entity.
		/// Unlike most methods of <see cref="PathOp"/>, this method accesses the file system.
		/// </summary>
		/// <param name="firstPath"></param>
		/// <param name="secondPath"></param>
		public static bool ArePathsEqual(string firstPath, string secondPath)
		{
			// Early-out for null or empty cases
			if (string.IsNullOrEmpty(firstPath) && string.IsNullOrEmpty(secondPath)) return true;
			if (string.IsNullOrEmpty(firstPath) || string.IsNullOrEmpty(secondPath)) return false;

			// Prepare for early-out string equality check
			firstPath = firstPath.Trim();
			secondPath = secondPath.Trim();

			// Early-out for string equality, avoiding file system access
			if (string.Equals(firstPath, secondPath, StringComparison.OrdinalIgnoreCase)) return true;

			// Obtain absolute paths
			firstPath = GetFullPath(firstPath);
			secondPath = GetFullPath(secondPath);

			// Compare absolute paths
			return string.Equals(firstPath, secondPath, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// Returns whether one path is a sub-path of another.
		/// Unlike most methods of <see cref="PathOp"/>, this method accesses the file system.
		/// </summary>
		/// <param name="path">The supposed sub-path.</param>
		/// <param name="baseDir">The (directory) path in which the supposed sub-path might be located in.</param>
		/// <returns>True, if <c>path</c> is a sub-path of <c>baseDir</c>.</returns>
		/// <example>
		/// <c>PathHelper.IsPathLocatedIn(@"C:\SomeDir\SubDir", @"C:\SomeDir")</c> will return true.
		/// </example>
		public static bool IsPathLocatedIn(string path, string baseDir)
		{
			if (baseDir[baseDir.Length - 1] != DirectorySeparatorChar &&
				baseDir[baseDir.Length - 1] != AltDirectorySeparatorChar)
				baseDir += DirectorySeparatorChar;

			path = GetFullPath(path);
			baseDir = GetDirectoryName(GetFullPath(baseDir));
			do
			{
				path = GetDirectoryName(path);
				if (path == baseDir) return true;
				if (path.Length < baseDir.Length) return false;
			} while (!String.IsNullOrEmpty(path));

			return false;
		}
		
		/// <summary>
		/// Determines the directory name component of a path, i.e. everything except the rightmost path element name.
		/// </summary>
		/// <param name="path"></param>
		public static string GetDirectoryName(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int sepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			if (sepIndex == -1) return string.Empty;

			return path.Substring(0, sepIndex);
		}
		/// <summary>
		/// Determines the file name component of a path, i.e. the rightmost path element name.
		/// </summary>
		/// <param name="path"></param>
		public static string GetFileName(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int sepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			return (sepIndex > -1) ? 
				path.Substring(sepIndex + 1) : 
				path;
		}
		/// <summary>
		/// Similar to <see cref="GetFileName"/>, but also strips away the files extension.
		/// </summary>
		/// <param name="path"></param>
		public static string GetFileNameWithoutExtension(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int dirSepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			int extSepIndex = IndexOfExtension(path);
			int extLen = (extSepIndex != -1) ? path.Length - extSepIndex : 0;
			int dirLen = (dirSepIndex + 1);

			if (dirSepIndex == -1 && extSepIndex == -1)
				return path;
			else 
				return path.Substring(dirSepIndex + 1, path.Length - dirLen - extLen);
		}
		/// <summary>
		/// Determines the extension of a file path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns>
		/// The paths file extension, including the leading separator char. 
		/// Returns an empty string, if no extension was found.
		/// </returns>
		public static string GetExtension(string path)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int sepIndex = IndexOfExtension(path);
			return (sepIndex > -1) ? 
				path.Substring(sepIndex) : 
				string.Empty;
		}

		/// <summary>
		/// Concatenates two path strings.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		public static string Combine(string first, string second)
		{
			bool firstEmpty = string.IsNullOrWhiteSpace(first);
			bool secondEmpty = string.IsNullOrWhiteSpace(second);

			if (firstEmpty && secondEmpty) return string.Empty;
			if (firstEmpty) return second;
			if (secondEmpty) return first;

			CheckInvalidPathChars(first);
			CheckInvalidPathChars(second);
			first = first.Trim();
			second = second.Trim();

			if (IsPathRooted(second)) return second;
			
			char firstEnd = first[first.Length - 1];
			if (firstEnd != DirectorySeparatorChar && 
				firstEnd != AltDirectorySeparatorChar && 
				firstEnd != VolumeSeparatorChar)
				return first + DirectorySeparatorChar + second;

			return first + second;
		}
		/// <summary>
		/// Concatenates any number of path strings.
		/// </summary>
		/// <param name="paths"></param>
		public static string Combine(params string[] paths)
		{
			if (paths == null) throw new ArgumentNullException ("paths");
			if (paths.Length == 0) return string.Empty;

			StringBuilder builder = new StringBuilder();
			bool separatorRequired = false;
			for (int i = 0; i < paths.Length; i++)
			{
				string path = paths[i];

				if (string.IsNullOrWhiteSpace(path)) continue;

				CheckInvalidPathChars(path);

				if (separatorRequired)
				{
					separatorRequired = false;
					builder.Append(DirectorySeparatorChar);
				}

				if (IsPathRooted(path)) builder.Length = 0;
				
				builder.Append(path);

				char pathEnd = path[path.Length - 1];
				if (pathEnd != DirectorySeparatorChar && 
					pathEnd != AltDirectorySeparatorChar && 
					pathEnd != VolumeSeparatorChar)
					separatorRequired = true;
			}

			return builder.ToString();
		}

		/// <summary>
		/// Returns an array of characters which are invalid in path strings.
		/// </summary>
		public static char[] GetInvalidPathChars()
		{
			return InvalidPathChars.Clone() as char[];
		}
		/// <summary>
		/// Returns an array of characters which are invalid in file name strings.
		/// </summary>
		public static char[] GetInvalidFileNameChars()
		{
			return InvalidFileNameChars.Clone() as char[];
		}

		/// <summary>
		/// Returns a copy of the specified file name which has been
		/// cleared of all invalid path characters.
		/// </summary>
		/// <param name="fileName"></param>
		public static string GetValidFileName(string fileName)
		{
			string invalidChars = new string(InvalidFileNameChars);
			string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(invalidChars));
			return Regex.Replace(fileName, invalidReStr, "_");
		}
		
		private static int IndexOfExtension(string path)
		{
			if (path == null) return -1;

			int lastSepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			return path.LastIndexOf(ExtensionSeparatorChar, path.Length - 1, path.Length - (lastSepIndex + 1));
		}
		internal static void CheckInvalidPathChars(string path)
		{
			if (path.IndexOfAny(InvalidPathChars) != -1) throw new ArgumentException("Illegal characters in path.");
		}
	}
}
