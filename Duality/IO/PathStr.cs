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
	public static class PathStr
	{
		public static readonly char DirectorySeparatorChar;
		public static readonly char AltDirectorySeparatorChar;
		public static readonly char VolumeSeparatorChar;
		public static readonly char ExtensionSeparatorChar;

		private static readonly char[] DirectorySeparatorChars;
		private static readonly char[] AsciiBelow32;
		private static readonly char[] InvalidPathChars;
		private static readonly char[] InvalidFileNameChars;

		static PathStr()
		{
			AltDirectorySeparatorChar = '/';
			DirectorySeparatorChar = '\\';
			VolumeSeparatorChar = ':';
			ExtensionSeparatorChar = '.';

			DirectorySeparatorChars = new char[] { DirectorySeparatorChar, AltDirectorySeparatorChar };

			AsciiBelow32 = new char[32];
			for (int i = 0; i < AsciiBelow32.Length; i++)
			{
				AsciiBelow32[i] = (char)i;
			}

			InvalidPathChars = new char[AsciiBelow32.Length + 6];
			InvalidPathChars[0] = '"';
			InvalidPathChars[1] = '<';
			InvalidPathChars[2] = '>';
			InvalidPathChars[3] = '|';
			InvalidPathChars[4] = '*';
			InvalidPathChars[5] = '?';
			for (int i = 0; i < AsciiBelow32.Length; i++)
			{
				InvalidPathChars[i + 6] = AsciiBelow32[i];
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
		/// <returns></returns>
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
		/// Determines the directory name component of a path, i.e. everything except the rightmost path element name.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
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
		/// <returns></returns>
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
		/// <returns></returns>
		public static string GetFileNameWithoutExtension(string path, bool multiExt = false)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int dirSepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			int extSepIndex = IndexOfExtension(path, multiExt);
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
		/// <param name="multiExt">
		/// If true, multi-extensions such as ".Texture.res" will be considered a 
		/// single extension, not two consecutive ones.
		/// </param>
		/// <returns>
		/// The paths file extension, including the leading separator char. 
		/// Returns an empty string, if no extension was found.
		/// </returns>
		public static string GetExtension(string path, bool multiExt = false)
		{
			if (string.IsNullOrEmpty(path)) return string.Empty;

			CheckInvalidPathChars(path);
			path = path.Trim();

			int sepIndex = IndexOfExtension(path, multiExt);
			return (sepIndex > -1) ? 
				path.Substring(sepIndex) : 
				string.Empty;
		}

		/// <summary>
		/// Concatenates two path strings.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
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
		/// <returns></returns>
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
		/// <returns></returns>
		public static char[] GetInvalidPathChars()
		{
			return InvalidPathChars.Clone() as char[];
		}
		/// <summary>
		/// Returns an array of characters which are invalid in file name strings.
		/// </summary>
		/// <returns></returns>
		public static char[] GetInvalidFileNameChars()
		{
			return InvalidFileNameChars.Clone() as char[];
		}

		/// <summary>
		/// Returns a copy of the specified file name which has been
		/// cleared of all invalid path characters.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string GetValidFileName(string fileName)
		{
			string invalidChars = new string(InvalidFileNameChars);
			string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(invalidChars));
			return Regex.Replace(fileName, invalidReStr, "_");
		}
		
		private static int IndexOfExtension(string path, bool firstOfMultiExt)
		{
			if (path == null) return -1;

			int lastSepIndex = path.LastIndexOfAny(DirectorySeparatorChars);
			if (!firstOfMultiExt)
				return path.LastIndexOf(ExtensionSeparatorChar, path.Length - 1, path.Length - (lastSepIndex + 1));
			else
				return path.IndexOf(ExtensionSeparatorChar, lastSepIndex + 1);
		}
		private static void CheckInvalidPathChars(string path)
		{
			if (path.IndexOfAny(InvalidPathChars) != -1) throw new ArgumentException("Illegal characters in path.");
		}
	}
}
