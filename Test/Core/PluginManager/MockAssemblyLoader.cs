﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using NUnit.Framework;

using Duality.IO;
using Duality.Backend;

namespace Duality.Tests.PluginManager
{
	public class MockAssemblyLoader : IAssemblyLoader, IDisposable
	{
		private List<string>                       baseDirs         = new List<string>();
		private Dictionary<string,Assembly>        assemblyMap      = new Dictionary<string,Assembly>();
		private HashSet<string>                    incompatibleDlls = new HashSet<string>();
		private List<string>                       loadedAssemblies = new List<string>();
		private Dictionary<DualityPlugin,Assembly> pluginMap        = new Dictionary<DualityPlugin,Assembly>();
		private bool                               initialized      = false;
		private bool                               disposed         = false;
		

		public event EventHandler<AssemblyResolveEventArgs> AssemblyResolve;
		public event EventHandler<AssemblyLoadedEventArgs> AssemblyLoaded;


		public bool Initialized
		{
			get { return this.initialized; }
		}
		public bool Disposed
		{
			get { return this.disposed; }
		}
		public IReadOnlyDictionary<string,Assembly> Plugins
		{
			get { return this.assemblyMap; }
		}
		public IEnumerable<string> LoadedAssemblyPaths
		{
			get { return this.loadedAssemblies; }
		}
		public IEnumerable<string> BaseDirectories
		{
			get { return this.baseDirs; }
		}
		public IEnumerable<string> AvailableAssemblyPaths
		{
			get { return this.assemblyMap.Keys.Concat(this.incompatibleDlls); }
		}
		public IEnumerable<Assembly> LoadedAssemblies
		{
			get { return this.assemblyMap.Values; }
		}
		

		public MockAssemblyLoader()
		{
			MockCorePlugin.MapToAssemblyCallback += this.MockCorePlugin_MapToAssemblyCallback;
			MockPlugin.MapToAssemblyCallback += this.MockPlugin_MapToAssemblyCallback;
		}
		public void Dispose()
		{
			MockCorePlugin.MapToAssemblyCallback -= this.MockCorePlugin_MapToAssemblyCallback;
			MockPlugin.MapToAssemblyCallback -= this.MockPlugin_MapToAssemblyCallback;
		}

		public Assembly InvokeResolveAssembly(string fullAssemblyName)
		{
			AssemblyResolveEventArgs resolveArgs = new AssemblyResolveEventArgs(fullAssemblyName);
			this.AssemblyResolve(this, resolveArgs);
			return resolveArgs.ResolvedAssembly;
		}
		public void AddPlugin(Assembly assembly)
		{
			this.assemblyMap.Add(assembly.Location, assembly);
		}
		public void AddIncompatibleDll(string path)
		{
			this.incompatibleDlls.Add(path);
		}
		public void AddBaseDir(string baseDir)
		{ 
			this.baseDirs.Add(baseDir);
		}
		public void ReplacePlugin(Assembly assembly, Assembly newAssembly)
		{
			this.assemblyMap.Remove(assembly.Location);
			this.AddPlugin(newAssembly);
		}

		public Assembly LoadAssembly(string assemblyPath)
		{
			string assemblyName = PathOp.GetFileNameWithoutExtension(assemblyPath);
			bool wasLoaded = this.loadedAssemblies.Contains(assemblyPath);
			this.loadedAssemblies.Add(assemblyPath);

			// Don't create a specific exception, so we force the system to be prepared for any.
			if (this.incompatibleDlls.Contains(assemblyPath))
				throw new Exception("This path has been mocked to be an incompatible dll file.");

			Assembly assembly = 
				this.LookupAssembly(assemblyPath) ?? 
				this.InvokeResolveAssembly(assemblyName);

			if (!wasLoaded && assembly != null && this.AssemblyLoaded != null)
				this.AssemblyLoaded(this, new AssemblyLoadedEventArgs(assembly));

			return assembly;
		}
		public int GetAssemblyHash(string assemblyPath)
		{
			Assembly assembly = this.LookupAssembly(assemblyPath);
			return (assembly != null) ? assembly.GetHashCode() : 0;
		}

		public void Init()
		{
			this.initialized = true;
		}
		public void Terminate()
		{
			this.disposed = true;
		}

		private Assembly LookupAssembly(string assemblyPath)
		{
			Assembly assembly;

			// Try a lookup for a direct match, otherwise check for path equality
			if (!this.assemblyMap.TryGetValue(assemblyPath, out assembly))
			{
				assembly = this.assemblyMap
					.FirstOrDefault(pair => PathOp.ArePathsEqual(pair.Key, assemblyPath))
					.Value;
			}

			return assembly;
		}
		private Assembly MockCorePlugin_MapToAssemblyCallback(MockCorePlugin mockPlugin)
		{
			// We'll just map the newly instantiated core plugin to any of the assemblies
			// we're mock-providing, except the ones that are already mapped
			Assembly assembly;
			if (!this.pluginMap.TryGetValue(mockPlugin, out assembly))
			{
				assembly = this.assemblyMap
					.Where(pair => !this.pluginMap.ContainsValue(pair.Value))
					.Where(pair => pair.Key.EndsWith(".core.dll", StringComparison.InvariantCultureIgnoreCase))
					.Select(pair => pair.Value)
					.FirstOrDefault();
				this.pluginMap.Add(mockPlugin, assembly);
			}
			return assembly;
		}
		private Assembly MockPlugin_MapToAssemblyCallback(MockPlugin mockPlugin)
		{
			// We'll just map the newly instantiated plugin to any of the assemblies
			// we're mock-providing, except the ones that are already mapped
			Assembly assembly;
			if (!this.pluginMap.TryGetValue(mockPlugin, out assembly))
			{
				assembly = this.assemblyMap
					.Where(pair => !this.pluginMap.ContainsValue(pair.Value))
					.Select(pair => pair.Value)
					.FirstOrDefault();
				this.pluginMap.Add(mockPlugin, assembly);
			}
			return assembly;
		}
	}
}
