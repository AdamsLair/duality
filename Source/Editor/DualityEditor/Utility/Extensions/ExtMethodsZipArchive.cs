using System;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace Duality.Editor
{
	public static class ExtMethodsZipArchive
	{
		public static void ExtractAll(this ZipArchive archive, string targetDir, bool overwrite)
		{
			DirectoryInfo dirInfo = Directory.CreateDirectory(targetDir);
			targetDir = dirInfo.FullName;

			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				string targetFilePath = Path.GetFullPath(Path.Combine(targetDir, entry.FullName));
				string targetFileDir = Path.GetDirectoryName(targetFilePath);

				// Ensure the items target directory exists
				Directory.CreateDirectory(targetFileDir);

				// If the item is a directory itself, just create a directory
				if (string.IsNullOrEmpty(Path.GetFileName(targetFilePath)))
				{
					Directory.CreateDirectory(targetFilePath);
				}
				// Otherwise, extract it as a file
				else
				{
					entry.Extract(targetFilePath, overwrite);
				}
			}
		}
		public static void Extract(this ZipArchiveEntry entry, string targetPath, bool overwrite)
		{
			if (!overwrite && File.Exists(targetPath)) return;
			using (Stream stream = File.Open(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				entry.Extract(stream);
			}
		}
		public static void Extract(this ZipArchiveEntry entry, Stream targetStream)
		{
			using (Stream sourceStream = entry.Open())
			{
				sourceStream.CopyTo(targetStream);
			}
		}

		public static void AddDirectory(this ZipArchive archive, string sourceDir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
			sourceDir = directoryInfo.FullName;

			foreach (FileSystemInfo entry in directoryInfo.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
			{
				string relativePath = entry.FullName.Substring(sourceDir.Length, entry.FullName.Length - sourceDir.Length);
				relativePath = relativePath.TrimStart(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});

				if (entry is FileInfo)
				{
					archive.AddFile(entry.FullName, relativePath);
				}
				else
				{
					DirectoryInfo subDirInfo = entry as DirectoryInfo;
					if (subDirInfo != null && !subDirInfo.EnumerateFileSystemInfos().Any())
					{
						archive.CreateEntry(relativePath + Path.DirectorySeparatorChar);
					}
				}
			}
		}
		public static ZipArchiveEntry AddFile(this ZipArchive archive, string sourceFile)
		{
			return archive.AddFile(sourceFile, Path.GetFileName(sourceFile));
		}
		private static ZipArchiveEntry AddFile(this ZipArchive archive, string sourceFile, string targetRelativePath)
		{
			ZipArchiveEntry entry;
			using (Stream sourceStream = File.OpenRead(sourceFile))
			{
				entry = archive.CreateEntry(targetRelativePath);
				using (Stream targetStream = entry.Open())
				{
					sourceStream.CopyTo(targetStream);
				}
			}
			return entry;
		}
	}
}
