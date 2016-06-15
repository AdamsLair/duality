using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Duality.IO;
using Duality.Backend;

namespace Duality
{
	/// <summary>
	/// Manages loading, reloading, initialization and disposal of Duality core plugins.
	/// 
	/// Since all assemblies are owned by the .Net runtime that only exposes a very limited
	/// degree of control, this class should only be used statically: Disposing it would
	/// only get rid of management data, not of the actual plugin assemblies, which would
	/// then cause problems.
	/// 
	/// A static instance of this class is available through <see cref="DualityApp.PluginManager"/>.
	/// </summary>
	public class CorePluginManager
	{
		private IPluginLoader                   pluginLoader    = null;
		private Dictionary<string,CorePlugin>   loadedPlugins   = new Dictionary<string,CorePlugin>();
		private List<Assembly>                  lockedPlugins   = new List<Assembly>();
		private HashSet<Assembly>               disposedPlugins = new HashSet<Assembly>();
		private Dictionary<Type,List<TypeInfo>> availTypeDict   = new Dictionary<Type,List<TypeInfo>>();

		/// <summary>
		/// Called right before removing a plugin. This allows other systems to get rid of 
		/// data and content that still depends on those plugins. Note that it is possible
		/// for some plugin termination / disposal code to be run after this event.
		/// </summary>
		public event EventHandler<CorePluginEventArgs> PluginsRemoving = null;
		/// <summary>
		/// Called right after removing a plugin. This allows other system to clear their
		/// internal caches and clean up everything that might have been left by the removed
		/// plugin. No plugin code is run after this event has been called.
		/// </summary>
		public event EventHandler<CorePluginEventArgs> PluginsRemoved = null;
		/// <summary>
		/// Fired whenever a core plugin has been initialized. This is the case after loading or reloading one.
		/// </summary>
		public event EventHandler<CorePluginEventArgs> PluginsReady = null;
		
		
		/// <summary>
		/// [GET] The plugin loader which is used by the <see cref="CorePluginManager"/> to discover
		/// and load available plugin assemblies.
		/// </summary>
		public IPluginLoader PluginLoader
		{
			get { return this.pluginLoader; }
		}
		/// <summary>
		/// [GET] Enumerates all currently loaded plugins.
		/// </summary>
		public IEnumerable<CorePlugin> LoadedPlugins
		{
			get { return this.loadedPlugins.Values; }
		}
		/// <summary>
		/// [GET] Enumerates all plugin assemblies that have been loaded before, but have been discarded due to a runtime plugin reload operation.
		/// This is usually only the case when being executed from withing the editor or manually triggering a plugin reload. However,
		/// this is normally unnecessary.
		/// </summary>
		public IEnumerable<Assembly> DisposedPlugins
		{
			get { return this.disposedPlugins; }
		}


		/// <summary>
		/// <see cref="CorePluginManager"/> should usually not be instantiated by users due to 
		/// its forced singleton-like usage. Use <see cref="DualityApp.PluginManager"/> instead.
		/// </summary>
		internal CorePluginManager() { }

		/// <summary>
		/// Initializes the <see cref="CorePluginManager"/> with the specified <see cref="IPluginLoader"/>.
		/// This method needs to be called once after instantiation (or previous termination) before plugins 
		/// can be loaded.
		/// </summary>
		/// <param name="pluginLoader"></param>
		public void Init(IPluginLoader pluginLoader)
		{
			if (this.pluginLoader != null) throw new InvalidOperationException("Plugin manager is already initialized.");

			this.pluginLoader = pluginLoader;
			this.pluginLoader.AssemblyResolve += this.pluginLoader_AssemblyResolve;
			this.pluginLoader.Init();
		}
		/// <summary>
		/// Terminates the <see cref="CorePluginManager"/>. This will dispose all core plugins and plugin data.
		/// </summary>
		public void Terminate()
		{
			if (this.pluginLoader == null) throw new InvalidOperationException("Plugin manager is is not currently initialized.");

			this.ClearPlugins();
			this.pluginLoader.Terminate();
			this.pluginLoader.AssemblyResolve -= this.pluginLoader_AssemblyResolve;
			this.pluginLoader = null;
		}
		/// <summary>
		/// Requests the disposal of all content / data that is dependent on any of the currently active
		/// plugins. This functioanlity is invoked automatically as part of reloading or removing plugins.
		/// </summary>
		public void DiscardPluginData()
		{
			this.OnPluginsRemoving(this.LoadedPlugins);
		}
		
		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Assembly> GetCoreAssemblies()
		{
			yield return typeof(DualityApp).GetTypeInfo().Assembly;
			foreach (CorePlugin p in this.LoadedPlugins)
				yield return p.PluginAssembly;
		}
		/// <summary>
		/// Enumerates all available Duality <see cref="System.Type">Types</see> that are assignable
		/// to the specified Type. 
		/// </summary>
		/// <param name="baseType">The base type to use for matching the result types.</param>
		/// <returns>An enumeration of all Duality types deriving from the specified type.</returns>
		/// <example>
		/// The following code logs all available kinds of <see cref="Duality.Components.Renderer">Renderers</see>:
		/// <code>
		/// var rendererTypes = DualityApp.GetAvailDualityTypes(typeof(Duality.Components.Renderer));
		/// foreach (Type rt in rendererTypes)
		/// {
		/// 	Log.Core.Write("Renderer Type '{0}' from Assembly '{1}'", Log.Type(rt), rt.Assembly.FullName);
		/// }
		/// </code>
		/// </example>
		public IEnumerable<TypeInfo> GetCoreTypes(Type baseType)
		{
			List<TypeInfo> availTypes;
			if (this.availTypeDict.TryGetValue(baseType, out availTypes))
				return availTypes;

			availTypes = new List<TypeInfo>();
			IEnumerable<Assembly> asmQuery = this.GetCoreAssemblies();
			foreach (Assembly asm in asmQuery)
			{
				// Try to retrieve all Types from the current Assembly
				IEnumerable<TypeInfo> types;
				try { types = asm.ExportedTypes.Select(t => t.GetTypeInfo()); }
				catch (Exception) { continue; }

				// Add the matching subset of these types to the result
				availTypes.AddRange(
					from t in types
					where baseType.GetTypeInfo().IsAssignableFrom(t)
					orderby t.Name
					select t);
			}
			this.availTypeDict[baseType] = availTypes;

			return availTypes;
		}

		/// <summary>
		/// Loads all available core plugins, as well as auxilliary libraries.
		/// </summary>
		public void LoadPlugins()
		{
			Log.Core.Write("Scanning for core plugins...");
			Log.Core.PushIndent();

			List<string> auxilLibs = new List<string>();
			foreach (string dllPath in this.pluginLoader.AvailableAssemblyPaths)
			{
				if (!dllPath.EndsWith(".core.dll", StringComparison.OrdinalIgnoreCase))
				{ 
					if (!dllPath.EndsWith(".editor.dll", StringComparison.OrdinalIgnoreCase))
						auxilLibs.Add(dllPath);
					continue;
				}

				Log.Core.Write("{0}...", dllPath);
				Log.Core.PushIndent();
				this.LoadPlugin(dllPath);
				Log.Core.PopIndent();
			}

			Log.Core.PopIndent();

			// Make sure to have all plugin-related Assemblies available even before even
			// getting an AssemblyResolve event - we might need to resolve their Types due
			// to deserialization, which may happen before touching any related class in code.
			if (auxilLibs.Count > 0)
			{
				Log.Core.Write("Loading auxiliary libraries...");
				Log.Core.PushIndent();

				foreach (string dllPath in auxilLibs)
				{
					// Load the Assembly in a try-catch block, as we might accidentally stumble upon
					// unmanaged or incompatible Assemblies in the process.
					try
					{
						this.pluginLoader.LoadAssembly(dllPath, false);
					}
					catch (BadImageFormatException) { }
				}

				Log.Core.PopIndent();
			}
		}
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public void InitPlugins()
		{
			Log.Core.Write("Initializing core plugins...");
			Log.Core.PushIndent();
			CorePlugin[] initPlugins = this.loadedPlugins.Values.ToArray();
			foreach (CorePlugin plugin in initPlugins)
			{
				Log.Core.Write("{0}...", plugin.AssemblyName);
				Log.Core.PushIndent();
				this.InitPlugin(plugin);
				Log.Core.PopIndent();
			}
			Log.Core.PopIndent();
		}
		/// <summary>
		/// Disposes all loaded plugins and discards all related content / data.
		/// </summary>
		public void ClearPlugins()
		{
			CorePlugin[] oldPlugins = this.LoadedPlugins.ToArray();
			if (oldPlugins.Length == 0) return;

			foreach (CorePlugin plugin in oldPlugins)
			{
				this.disposedPlugins.Add(plugin.PluginAssembly);
			}
			this.OnPluginsRemoving(oldPlugins);
			foreach (CorePlugin plugin in oldPlugins)
			{
				try
				{
					plugin.Dispose();
				}
				catch (Exception e)
				{
					Log.Core.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				}
			}
			this.OnPluginsRemoved(oldPlugins);
			this.loadedPlugins.Clear();
		}
		
		/// <summary>
		/// Adds an already loaded plugin Assembly to the internal Duality CorePlugin registry.
		/// You shouldn't need to call this method in general, since Duality manages its plugins
		/// automatically. 
		/// </summary>
		/// <remarks>
		/// This method can be useful in certain cases when it is necessary to treat an Assembly as a
		/// Duality plugin, even though it isn't located in the Plugins folder, or is not available
		/// as a file at all. A typical case for this is Unit Testing where the testing Assembly may
		/// specify additional Duality types such as Components, Resources, etc.
		/// </remarks>
		/// <param name="pluginAssembly"></param>
		/// <param name="pluginFilePath"></param>
		/// <returns></returns>
		public CorePlugin LoadPlugin(Assembly pluginAssembly, string pluginFilePath)
		{
			this.disposedPlugins.Remove(pluginAssembly);

			string asmName = pluginAssembly.GetShortAssemblyName();
			CorePlugin plugin = this.loadedPlugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;
			
			try
			{
				TypeInfo pluginType = pluginAssembly.ExportedTypes
					.Select(t => t.GetTypeInfo())
					.FirstOrDefault(t => typeof(CorePlugin).GetTypeInfo().IsAssignableFrom(t));

				if (pluginType == null) 
					throw new Exception(string.Format(
						"Plugin does not contain a public {0} class.", 
						typeof(CorePlugin).Name));

				plugin = (CorePlugin)pluginType.CreateInstanceOf();

				if (plugin == null) 
					throw new Exception(string.Format(
						"Failed to instantiate {0} class.", 
						Log.Type(pluginType.GetType())));

				plugin.FilePath = pluginFilePath;
				plugin.FileHash = this.pluginLoader.GetAssemblyHash(pluginFilePath);

				this.loadedPlugins.Add(plugin.AssemblyName, plugin);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin: {0}", Log.Exception(e));
				this.disposedPlugins.Add(pluginAssembly);
				plugin = null;
			}

			return plugin;
		}
		/// <summary>
		/// Reloads the specified plugin. Does not initialize it.
		/// </summary>
		/// <param name="pluginFilePath"></param>
		public CorePlugin ReloadPlugin(string pluginFilePath)
		{
			if (!pluginFilePath.EndsWith(".core.dll", StringComparison.OrdinalIgnoreCase))
				return null;

			// If we're trying to reload an active backend plugin, stop
			foreach (var pair in this.loadedPlugins)
			{
				CorePlugin plugin = pair.Value;
				if (PathOp.ArePathsEqual(plugin.FilePath, pluginFilePath))
				{
					foreach (Assembly lockedAssembly in this.lockedPlugins)
					{
						if (plugin.PluginAssembly == lockedAssembly)
						{
							Log.Core.WriteError(
								"Can't reload plugin {0}, because it has been locked by the runtime. " + 
								"This usually happens for plugins that implement a currently active backend.",
								Log.Assembly(lockedAssembly));
							return null;
						}
					}
					break;
				}
			}
			
			// Load the updated plugin Assembly
			Assembly pluginAssembly = null;
			try
			{
				pluginAssembly = this.pluginLoader.LoadAssembly(pluginFilePath, true);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				return null;
			}

			// If we're overwriting an old plugin here, add the old version to the "disposed" blacklist
			string assemblyName = pluginAssembly.GetShortAssemblyName();
			CorePlugin oldPlugin;
			if (this.loadedPlugins.TryGetValue(assemblyName, out oldPlugin))
			{
				this.loadedPlugins.Remove(assemblyName);
				this.disposedPlugins.Add(oldPlugin.PluginAssembly);
				this.OnPluginsRemoving(new[] { oldPlugin });
				oldPlugin.Dispose();
			}

			// Load the new plugin from the updated Assembly
			CorePlugin updatedPlugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			
			// Discard temporary plugin-related data (cached Types, etc.)
			this.OnPluginsRemoved(new[] { oldPlugin });

			return updatedPlugin;
		}
		/// <summary>
		/// Initializes the specified plugin. This concludes a manual plugin load or reload operation
		/// using API like <see cref="LoadPlugin"/> and <see cref="ReloadPlugin"/>.
		/// </summary>
		/// <param name="plugin"></param>
		public void InitPlugin(CorePlugin plugin)
		{
			try
			{
				plugin.InitPlugin();
				this.OnPluginsReady(new[] { plugin });
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error initializing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				this.RemovePlugin(plugin);
			}
		}

		/// <summary>
		/// Locks the specified plugin assembly, so any reload attempts are blocked.
		/// </summary>
		/// <param name="pluginAssembly"></param>
		public void LockPlugin(Assembly pluginAssembly)
		{
			this.lockedPlugins.Add(pluginAssembly);
		}
		/// <summary>
		/// Unlocks the specified plugin assembly, so future attempts at reloading will
		/// no longer be blocked.
		/// </summary>
		/// <param name="pluginAssembly"></param>
		public void UnlockPlugin(Assembly pluginAssembly)
		{
			this.lockedPlugins.Remove(pluginAssembly);
		}

		private CorePlugin LoadPlugin(string pluginFilePath)
		{
			string asmName = PathOp.GetFileNameWithoutExtension(pluginFilePath);
			CorePlugin plugin = this.loadedPlugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;

			Assembly pluginAssembly = null;
			try
			{
				pluginAssembly = this.pluginLoader.LoadAssembly(pluginFilePath, true);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				plugin = null;
			}

			if (pluginAssembly != null)
			{
				plugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			}

			return plugin;
		}
		private void RemovePlugin(CorePlugin plugin)
		{
			// Dispose plugin and discard plugin related data
			this.disposedPlugins.Add(plugin.PluginAssembly);
			this.OnPluginsRemoving(new[] { plugin });
			this.loadedPlugins.Remove(plugin.AssemblyName);
			try
			{
				plugin.Dispose();
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
			}

			// Discard temporary plugin-related data (cached Types, etc.)
			this.OnPluginsRemoved(new[] { plugin });
		}

		private void OnPluginsReady(IEnumerable<CorePlugin> plugins)
		{
			if (this.PluginsReady != null)
				this.PluginsReady(null, new CorePluginEventArgs(plugins));
		}
		private void OnPluginsRemoving(IEnumerable<CorePlugin> oldPlugins)
		{
			if (this.PluginsRemoving != null)
				this.PluginsRemoving(null, new CorePluginEventArgs(oldPlugins));
		}
		private void OnPluginsRemoved(IEnumerable<CorePlugin> oldPlugins)
		{
			if (this.PluginsRemoved != null)
				this.PluginsRemoved(null, new CorePluginEventArgs(oldPlugins));

			// Clean cached type values
			this.availTypeDict.Clear();
		}

		private void pluginLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// First assume we are searching for a dynamically loaded plugin assembly
			CorePlugin plugin;
			if (this.loadedPlugins.TryGetValue(args.AssemblyName, out plugin))
			{
				args.Resolve(plugin.PluginAssembly);
				return;
			}
			// Not there? Search for other libraries in the Plugins folder
			else
			{
				// Search for core plugins that haven't been loaded yet, and load them first.
				// This is required to satisfy dependencies while loading plugins, since
				// we can't know which one requires which beforehand.
				foreach (string libFile in this.pluginLoader.AvailableAssemblyPaths)
				{
					if (!libFile.EndsWith(".core.dll", StringComparison.OrdinalIgnoreCase))
						continue;

					string libName = PathOp.GetFileNameWithoutExtension(libFile);
					if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
					{
						plugin = this.LoadPlugin(libFile);
						if (plugin != null)
						{
							args.Resolve(plugin.PluginAssembly);
							return;
						}
					}
				}

				// Search for other libraries that might be located inside the plugin directory
				foreach (string libFile in this.pluginLoader.AvailableAssemblyPaths)
				{
					string libName = PathOp.GetFileNameWithoutExtension(libFile);
					if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
					{
						Assembly assembly = this.pluginLoader.LoadAssembly(libFile, false);
						if (assembly != null)
						{
							args.Resolve(assembly);
							return;
						}
					}
				}
			}
		}
	}
}
