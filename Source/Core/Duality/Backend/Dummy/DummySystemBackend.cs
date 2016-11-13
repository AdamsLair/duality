using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality.IO;

namespace Duality.Backend.Dummy
{
	public class DummySystemBackend : ISystemBackend
	{
		private DummyFileSystem fileSystem = new DummyFileSystem();

		string IDualityBackend.Id
		{
			get { return "DummySystemBackend"; }
		}
		string IDualityBackend.Name
		{
			get { return "No system backend"; }
		}
		int IDualityBackend.Priority
		{
			get { return int.MinValue; }
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
			return string.Empty;
		}
	}
}
