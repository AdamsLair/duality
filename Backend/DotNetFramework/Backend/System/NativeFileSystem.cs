using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality.IO;

namespace Duality.Backend.DotNetFramework
{
	[DontSerialize]
	public class NativeFileSystem : IFileSystem
	{
		string IFileSystem.GetFullPath(string path)
		{
			return Path.GetFullPath(path);
		}
		
		IEnumerable<string> IFileSystem.GetFiles(string path, bool recursive)
		{
			return Directory.EnumerateFiles(
				path, 
				"*", 
				recursive ? 
					SearchOption.AllDirectories : 
					SearchOption.TopDirectoryOnly);
		}
		IEnumerable<string> IFileSystem.GetDirectories(string path, bool recursive)
		{
			return Directory.EnumerateDirectories(
				path, 
				"*", 
				recursive ? 
					SearchOption.AllDirectories : 
					SearchOption.TopDirectoryOnly);
		}

		bool IFileSystem.FileExists(string path)
		{
			return File.Exists(path);
		}
		bool IFileSystem.DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}
		
		Stream IFileSystem.CreateFile(string path)
		{
			return File.Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
		}
		Stream IFileSystem.OpenFile(string path, FileAccessMode mode)
		{
			if (mode == FileAccessMode.None) throw new ArgumentException("Can't open a file stream without any access capabilities.");

			FileAccess access;
			switch (mode)
			{
				default:
				case FileAccessMode.Read:
					access = FileAccess.Read;
					break;
				case FileAccessMode.Write:
					access = FileAccess.Write;
					break;
				case FileAccessMode.ReadWrite:
					access = FileAccess.ReadWrite;
					break;
			}

			return File.Open(path, FileMode.Open, access, FileShare.ReadWrite);
		}
		void IFileSystem.DeleteFile(string path)
		{
			File.Delete(path);
		}

		void IFileSystem.CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}
		void IFileSystem.DeleteDirectory(string path)
		{
			Directory.Delete(path, true);
		}
	}
}
