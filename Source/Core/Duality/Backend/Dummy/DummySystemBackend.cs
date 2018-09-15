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
		void IDualityBackend.Init()
		{
			Logs.Core.WriteWarning("DummySystemBackend initialized. This is unusual and may cause problems when someone tries to access disk or system features.");
		}
		void IDualityBackend.Shutdown() { }

		string ISystemBackend.GetNamedPath(NamedDirectory dir)
		{
			return string.Empty;
		}
	}
}
