using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend.Dummy
{
	public class DummyPluginLoader : IPluginLoader
	{
		IEnumerable<string> IPluginLoader.BaseDirectories
		{
			get { return Enumerable.Empty<string>(); }
		}
		IEnumerable<string> IPluginLoader.AvailableAssemblyPaths
		{
			get { return Enumerable.Empty<string>(); }
		}
		Assembly IPluginLoader.LoadAssembly(string assemblyPath, bool anonymous)
		{
			return null;
		}
		int IPluginLoader.GetAssemblyHash(string assemblyPath)
		{
			return 0;
		}
		void IPluginLoader.Init(ResolveAssemblyCallback resolveCallback) { }
		void IPluginLoader.Terminate() { }
	}
}
