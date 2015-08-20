using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend
{
	public interface IPluginLoader
	{
		IEnumerable<string> BaseDirectories { get; }
		IEnumerable<string> AvailableAssemblyPaths { get; }

		Assembly LoadAssembly(string assemblyPath, bool anonymous);
		int GetAssemblyHash(string assemblyPath);

		void Init(ResolveAssemblyCallback resolveCallback);
		void Terminate();
	}

	public delegate Assembly ResolveAssemblyCallback(ResolveAssemblyEventArgs args);

	public class ResolveAssemblyEventArgs : EventArgs
	{
		private string fullAssemblyName;
		private string assemblyName;

		public string FullAssemblyName
		{
			get { return this.fullAssemblyName; }
		}
		public string AssemblyName
		{
			get { return this.assemblyName; }
		}

		public ResolveAssemblyEventArgs(string fullAssemblyName)
		{
			this.fullAssemblyName = fullAssemblyName;
			this.assemblyName = ReflectionHelper.GetShortAssemblyName(fullAssemblyName);
		}
	}
}
