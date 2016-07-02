using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.IO;

namespace Duality.Backend.DotNetFramework
{
	[DontSerialize]
	public class SystemBackend : ISystemBackend
	{
		private NativeFileSystem fileSystem = new NativeFileSystem();

		string IDualityBackend.Id
		{
			get { return "DotNetFrameworkSystemBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return ".Net Framework"; }
		}
		int IDualityBackend.Priority
		{
			get { return 0; }
		}

		IFileSystem ISystemBackend.FileSystem
		{
			get { return this.fileSystem; }
		}

		bool IDualityBackend.CheckAvailable()
		{
			return true;
		}
		void IDualityBackend.Init() { }
		void IDualityBackend.Shutdown() { }

		string ISystemBackend.GetNamedPath(NamedDirectory dir)
		{
			string path;
			switch (dir)
			{
				default:                             path = null; break;
				case NamedDirectory.Current:         path = System.IO.Directory.GetCurrentDirectory(); break;
				case NamedDirectory.ApplicationData: path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); break;
				case NamedDirectory.MyDocuments:     path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); break;
				case NamedDirectory.MyMusic:         path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); break;
				case NamedDirectory.MyPictures:      path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); break;
			}
			return this.fileSystem.GetDualityPathFormat(path);
		}
	}
}
