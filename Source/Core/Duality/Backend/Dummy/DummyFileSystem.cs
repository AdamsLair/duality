using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality.IO;

namespace Duality.Backend.Dummy
{
	public class DummyFileSystem : IFileSystem
	{
		string IFileSystem.GetFullPath(string path)
		{
			return path;
		}
		
		IEnumerable<string> IFileSystem.GetFiles(string path, bool recursive)
		{
			return Enumerable.Empty<string>();
		}
		IEnumerable<string> IFileSystem.GetDirectories(string path, bool recursive)
		{
			return Enumerable.Empty<string>();
		}

		bool IFileSystem.FileExists(string path)
		{
			return false;
		}
		bool IFileSystem.DirectoryExists(string path)
		{
			return false;
		}
		
		Stream IFileSystem.CreateFile(string path)
		{
			throw new BackendException(string.Format("Can't create file '{0}'. Write access denied.", path));
		}
		Stream IFileSystem.OpenFile(string path, FileAccessMode mode)
		{
			throw new BackendException(string.Format("Can't open non-existent file '{0}'.", path));
		}
		void IFileSystem.DeleteFile(string path) { }

		void IFileSystem.CreateDirectory(string path) { }
		void IFileSystem.DeleteDirectory(string path) { }
	}
}
