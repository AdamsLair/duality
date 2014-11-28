using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO;
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
	}
}
