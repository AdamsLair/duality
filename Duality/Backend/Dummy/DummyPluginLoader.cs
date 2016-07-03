using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend.Dummy
{
	public class DummyPluginLoader : IPluginLoader
	{
		event EventHandler<AssemblyResolveEventArgs> IPluginLoader.AssemblyResolve
		{
			add { }
			remove { }
		}
		event EventHandler<AssemblyLoadedEventArgs> IPluginLoader.AssemblyLoaded
		{
			add { }
			remove { }
		}
		IEnumerable<string> IPluginLoader.BaseDirectories
		{
			get { return Enumerable.Empty<string>(); }
		}
		IEnumerable<string> IPluginLoader.AvailableAssemblyPaths
		{
			get { return Enumerable.Empty<string>(); }
		}
		IEnumerable<Assembly> IPluginLoader.LoadedAssemblies
		{
			get { return Enumerable.Empty<Assembly>(); }
		}
		Assembly IPluginLoader.LoadAssembly(string assemblyPath, bool anonymous)
		{
			return null;
		}
		int IPluginLoader.GetAssemblyHash(string assemblyPath)
		{
			return 0;
		}
		void IPluginLoader.Init() { }
		void IPluginLoader.Terminate() { }
	}
}
