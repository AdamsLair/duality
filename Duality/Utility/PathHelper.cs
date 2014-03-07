using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Duality
{
	/// <summary>
	/// Provides helper methods for handling <see cref="System.IO.Path">Paths</see>.
	/// </summary>
	public static class PathHelper
	{
		private static string executingBinDir;
		/// <summary>
		/// Returns the directory
		/// </summary>
		public static string ExecutingAssemblyDir
		{
			get
			{
				if (string.IsNullOrEmpty(executingBinDir))
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly() ?? typeof(DualityApp).Assembly;
					executingBinDir = Path.GetFullPath(Path.GetDirectoryName(entryAssembly.Location));
				}
				return executingBinDir;
			}
		}

		/// <summary>
		/// Returns a path that isn't taken yet.
		/// </summary>
		/// <param name="pathBase">The path to use as base for finding a available path.</param>
		/// <param name="pathExt">The (file) extension to add to the new path.</param>
		/// <returns>A path that doesn't relate to any existing file or directory.</returns>
		/// <example>
		/// Assuming the directory <c>C:\SomeDir\</c> contains the file <c>File.txt</c>,
		/// the code <c>PathHelper.GetFreePath(@"C:\SomeDir\File", ".txt");</c>
		/// will return <c>C:\SomeDir\File (2).txt</c>.
		/// </example>
		public static string GetFreePath(string pathBase, string pathExt)
		{
			int nameNum = 1;
			string path = pathBase + pathExt;
			while (Directory.Exists(path) || File.Exists(path))
			{
				nameNum++;
				path = pathBase + " (" + nameNum + ")" + pathExt;
			}
			return path;
		}

		/// <summary>
		/// Returns whether one path is a sub-path of another.
		/// </summary>
		/// <param name="path">The supposed sub-path.</param>
		/// <param name="baseDir">The (directory) path in which the supposed sub-path might be located in.</param>
		/// <returns>True, if <c>path</c> is a sub-path of <c>baseDir</c>.</returns>
		/// <example>
		/// <c>PathHelper.IsPathLocatedIn(@"C:\SomeDir\SubDir", @"C:\SomeDir")</c> will return true.
		/// </example>
		public static bool IsPathLocatedIn(string path, string baseDir)
		{
			bool baseDirIsFile = File.Exists(baseDir);
			if (baseDirIsFile) return false;

			if (baseDir[baseDir.Length - 1] != Path.DirectorySeparatorChar &&
				baseDir[baseDir.Length - 1] != Path.AltDirectorySeparatorChar)
				baseDir += Path.DirectorySeparatorChar;

			path = Path.GetFullPath(path);
			baseDir = Path.GetDirectoryName(Path.GetFullPath(baseDir));
			do
			{
				path = Path.GetDirectoryName(path);
				if (path == baseDir) return true;
				if (path.Length < baseDir.Length) return false;
			} while (!String.IsNullOrEmpty(path));
			return false;
		}
		/// <summary>
		/// Returns the relative path from one path to another.
		/// </summary>
		/// <param name="filePath">The path to make relative.</param>
		/// <param name="relativeToDir">The path to make it relative to.</param>
		/// <returns>A path that, if <see cref="System.IO.Path.Combine(string,string)">combined</see> with <c>relativeTo</c>, equals the original path.</returns>
		/// <example>
		/// <c>PathHelper.MakePathRelative(@"C:\SomeDir\SubDir\File.txt", @"C:\SomeDir")</c> will return <c>SubDir\File.txt</c>.
		/// </example>
		public static string MakeFilePathRelative(string filePath, string relativeToDir = ".")
		{
			string dir		= Path.GetFullPath(filePath);
			string dirRel	= Path.GetFullPath(relativeToDir);
			string fileName;

			fileName = Path.GetFileName(dir);
			dir = Path.GetDirectoryName(dir);

			// Different disk drive: Cannot generate relative path.
			if (Directory.GetDirectoryRoot(dir) != Directory.GetDirectoryRoot(dirRel))	return null;

			string		resultDir	= "";
			string[]	dirToken	= dir.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			string[]	dirRelToken	= dirRel.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			int numBackDir = dirRelToken.Length - dirToken.Length;
			int sameDirIndex = int.MaxValue;
			for (int i = 0; i < Math.Min(dirToken.Length, dirRelToken.Length); i++)
			{
				if (dirToken[i] != dirRelToken[i])
				{
					numBackDir = dirRelToken.Length - i;
					break;
				}
				else
				{
					sameDirIndex = i;
				}
			}

			// Go back until we've reached the smallest mutual directory
			if (numBackDir > 0)
			{
				resultDir = 
					(".." + Path.DirectorySeparatorChar).Multiply(numBackDir) + 
					resultDir;
			}

			// ... and then go to the desired path from there
			for (int i = sameDirIndex + 1; i < dirToken.Length; i++)
			{
				resultDir = Path.Combine(resultDir, dirToken[i]);
			}

			return Path.Combine(resultDir, fileName);
		}
		/// <summary>
		/// Returns a mutual base path of two different paths.
		/// </summary>
		/// <param name="path">The first path.</param>
		/// <param name="path2">The second path.</param>
		/// <returns>The mutual base path of both.</returns>
		/// <example>
		/// <c>PathHelper.GetMutualDirectory(@"C:\SomeDir\SubDir\File.txt", @"C:\SomeDir\SubDir2\File.txt")</c> will return <c>C:\SomeDir</c>.
		/// </example>
		public static string GetMutualDirectory(string path, string path2)
		{
			string dir		= Path.GetFullPath(path);
			string dirRel	= Path.GetFullPath(path2);

			// Different disk drive: No mutual directory
			if (Directory.GetDirectoryRoot(dir) != Directory.GetDirectoryRoot(dirRel))	return null;

			string		resultDir	= "";
			string[]	dirToken	= dir.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			string[]	dirRelToken	= dirRel.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

			int numBackDir = dirRelToken.Length - dirToken.Length;
			for (int i = 1; i < Math.Min(dirToken.Length, dirRelToken.Length); i++)
			{
				if (dirToken[i] != dirRelToken[i])
				{
					numBackDir = dirRelToken.Length - i;
					break;
				}
			}

			// Go back until we've reached the smallest mutual directory
			if (numBackDir > 0)
			{
				resultDir = 
					Path.DirectorySeparatorChar + 
					(".." + Path.DirectorySeparatorChar).Multiply(numBackDir) + 
					resultDir;
			}

			return resultDir;
		}

		/// <summary>
		/// Takes a string that is supposed to be a file name and converts it into an
		/// actually valid file name, replacing invalid characters by undercores.
		/// </summary>
		/// <param name="fileName">A string that is supposed to be a file name.</param>
		/// <returns>A valid file name.</returns>
		public static string GetValidFileName(string fileName)
		{
			string invalidChars = new string(Path.GetInvalidFileNameChars());
			string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(invalidChars));
			return Regex.Replace(fileName, invalidReStr, "_");
		}

		/// <summary>
		/// Returns whether the specified file or directory is visible, i.e. not hidden.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool IsPathVisible(string path)
		{
			if (Directory.Exists(path))
			{
				DirectoryInfo dirInfo = new DirectoryInfo(path);
				return (dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden;
			}
			else if (File.Exists(path))
			{
				FileInfo fileInfo = new FileInfo(path);
				return (fileInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden;
			}
			else return false;
		}
		/// <summary>
		/// Returns whether the specified path is considered a valid file or folder path.
		/// Does not check whether the file or folder actually exists, only if the path can
		/// be validly used to address one.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		[System.Diagnostics.DebuggerStepThrough]
		public static bool IsPathValid(string path)
		{
			bool valid = false;
			try { new FileInfo(path); valid = true; } catch (Exception) {}
			return valid;
		}

		/// <summary>
		/// Deep-Copies the source directory to the target path.
		/// </summary>
		/// <param name="sourcePath"></param>
		/// <param name="targetPath"></param>
		/// <param name="overwrite"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static bool CopyDirectory(string sourcePath, string targetPath, bool overwrite = false, Predicate<string> filter = null)
		{
			if (!Directory.Exists(sourcePath)) return false;
			if (!overwrite && Directory.Exists(targetPath)) return false;

			if (!Directory.Exists(targetPath)) 
				Directory.CreateDirectory(targetPath);

			foreach (string file in Directory.GetFiles(sourcePath))
			{
				if (filter != null && !filter(file)) continue;
				File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), overwrite);
			}
			foreach (string subDir in Directory.GetDirectories(sourcePath))
			{
				if (filter != null && !filter(subDir)) continue;
				CopyDirectory(subDir, Path.Combine(targetPath, Path.GetFileName(subDir)), overwrite, filter);
			}

			return true;
		}

		/// <summary>
		/// Determines whether the contents of two files are completely equal.
		/// </summary>
		/// <param name="filePathA"></param>
		/// <param name="filePathB"></param>
		/// <returns></returns>
		public static bool FilesEqual(string filePathA, string filePathB)
		{
			filePathA = Path.GetFullPath(filePathA);
			filePathB = Path.GetFullPath(filePathB);
			if (filePathA == filePathB) return true;

			if (!File.Exists(filePathA)) return false;
			if (!File.Exists(filePathB)) return false;

			using (FileStream streamA = File.OpenRead(filePathA))
			using (FileStream streamB = File.OpenRead(filePathB))
			{
				if (streamA.Length != streamB.Length) return false;
				while (streamA.Position < streamA.Length)
				{
					if (streamA.ReadByte() != streamB.ReadByte()) return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Calculates a hash value from the full (byte by byte) content of a file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static int GetFileHash(string filePath)
		{
			if (!File.Exists(filePath)) return 0;
			int hash = 17;
			using (FileStream stream = File.OpenRead(filePath))
			{
				unchecked
				{
					while (stream.Position < stream.Length)
					{
						hash = hash * 23 + stream.ReadByte();
					}
				}
			}
			return hash;
		}
	}
}
