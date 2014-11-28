using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Duality.Updater
{
	public static class IOHelper
	{
		public static void RemoveEmptyDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path)) return;
			if (File.Exists(path)) path = Path.GetDirectoryName(path);
			if (!Directory.Exists(path)) return;

			if (Directory.EnumerateFiles(path).Any()) return;
			if (Directory.EnumerateDirectories(path).Any()) return;
			Directory.Delete(path);

			string parent;
			try
			{
				parent = Path.GetDirectoryName(path);
			}
			catch (Exception)
			{
				parent = null;
			}

			if (parent != null)
			{
				RemoveEmptyDirectory(parent);
			}
		}
		public static bool IsFileLocked(string filePath)
		{
			return IsFileLocked(new FileInfo(filePath));
		}
		public static bool IsFileLocked(FileInfo file)
		{
			if (!file.Exists) return false;

			FileStream stream = null;
			try
			{
				stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			return false;
		}
		public static void WaitForLockRelease(params string[] files)
		{
			bool anyFileLocked;
			int iterations = 0;
			do
			{
				anyFileLocked = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (IsFileLocked(files[i]))
					{
						anyFileLocked = true;
						break;
					}
				}
				if (anyFileLocked)
				{
					iterations++;
					if (iterations > 0 && (iterations % 30) == 0)
					{
						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.WriteLine("(waiting for file access) ");
						Console.ResetColor();
					}
					Thread.Sleep(100);
				}
			} while (anyFileLocked);
		}
		public static void WaitForUserRead(int timeoutSeconds = 0)
		{
			if (timeoutSeconds > 0)
			{
				for (int i = 0; i < timeoutSeconds; i++)
				{
					Thread.Sleep(1000);
					Console.Write(".");
				}
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("Press any key to continue");
				Console.WriteLine();
				Console.ReadKey(true);
			}
		}
		public static string MakePathRelative(string path, string relativeToDir = ".")
		{
			string dir		= Path.GetFullPath(path);
			string dirRel	= Path.GetFullPath(relativeToDir);

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
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < numBackDir; i++)
				{
					builder.Append("..");
					builder.Append(Path.DirectorySeparatorChar);
				}
				resultDir = builder.ToString() + resultDir;
			}

			// ... and then go to the desired path from there
			for (int i = sameDirIndex + 1; i < dirToken.Length; i++)
			{
				resultDir = Path.Combine(resultDir, dirToken[i]);
			}

			return resultDir;
		}
	}
}
