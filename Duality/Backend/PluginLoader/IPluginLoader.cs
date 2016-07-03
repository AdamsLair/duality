using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend
{
	/// <summary>
	/// Specifies an API for enumerating and loading plugin Assemblies.
	/// </summary>
	public interface IPluginLoader
	{
		/// <summary>
		/// Fired when the runtime attempts to resolve a non-trivial <see cref="Assembly"/>
		/// dependency, which may or may not be a plugin. Handling this event allows to
		/// specify which <see cref="Assembly"/> to use.
		/// </summary>
		event EventHandler<AssemblyResolveEventArgs> AssemblyResolve;
		/// <summary>
		/// Fired when an <see cref="Assembly"/> is loaded by the runtime.
		/// </summary>
		event EventHandler<AssemblyLoadedEventArgs> AssemblyLoaded;

		/// <summary>
		/// [GET] Enumerates all base directories that will be searched for plugin
		/// Assemblies.
		/// </summary>
		IEnumerable<string> BaseDirectories { get; }
		/// <summary>
		/// [GET] Enumerates all plugin Assemblies that are available for loading.
		/// </summary>
		IEnumerable<string> AvailableAssemblyPaths { get; }
		/// <summary>
		/// [GET] Enumerates all Assemblies that are currently loaded in the context of this application.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Assembly> LoadedAssemblies { get; }

		/// <summary>
		/// Loads a plugin Assembly from the specified path. For reliable cross-platform
		/// usage, that path should be one of the <see cref="AvailableAssemblyPaths"/>.
		/// </summary>
		/// <param name="assemblyPath">The path from which the Assembly will be loaded.</param>
		/// <param name="anonymous">
		/// When true, the Assembly is requested to be loaded without being tied to its
		/// identity path or location. This will avoid both locking the Assembly file and
		/// caching the Assembly internally, thus allowing to reload the Assembly plugin
		/// at runtime.
		/// </param>
		/// <returns></returns>
		Assembly LoadAssembly(string assemblyPath, bool anonymous);
		/// <summary>
		/// Determines the hash code of the specified Assembly. This may be used for
		/// verification or comparison purposes, such as determining whether two Assemblies
		/// are equal.
		/// </summary>
		/// <param name="assemblyPath"></param>
		/// <returns></returns>
		int GetAssemblyHash(string assemblyPath);

		/// <summary>
		/// Initializes the plugin loader.
		/// </summary>
		void Init();
		/// <summary>
		/// Terminates the plugin loader and provides the opportunity for its implementation
		/// to shut down properly.
		/// </summary>
		void Terminate();
	}
}
