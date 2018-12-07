using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend.Dummy
{
	public class DummyAssemblyLoader : IAssemblyLoader
	{
		event EventHandler<AssemblyResolveEventArgs> IAssemblyLoader.AssemblyResolve
		{
			add { }
			remove { }
		}
		event EventHandler<AssemblyLoadedEventArgs> IAssemblyLoader.AssemblyLoaded
		{
			add { }
			remove { }
		}
		IEnumerable<string> IAssemblyLoader.BaseDirectories
		{
			get { return Enumerable.Empty<string>(); }
		}
		IEnumerable<string> IAssemblyLoader.AvailableAssemblyPaths
		{
			get { return Enumerable.Empty<string>(); }
		}
		IEnumerable<Assembly> IAssemblyLoader.LoadedAssemblies
		{
			get { return Enumerable.Empty<Assembly>(); }
		}
		Assembly IAssemblyLoader.LoadAssembly(string assemblyPath)
		{
			return null;
		}
		int IAssemblyLoader.GetAssemblyHash(string assemblyPath)
		{
			return 0;
		}
		void IAssemblyLoader.Init() { }
		void IAssemblyLoader.Terminate() { }
	}
}
