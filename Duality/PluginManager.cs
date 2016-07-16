using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Duality.IO;
using Duality.Backend;

namespace Duality
{
	public abstract class PluginManager<T> where T : DualityPlugin
	{
		private static readonly Log dummyPluginLog = new Log("Dummy PluginManager Log");

		private Log                             pluginLog       = dummyPluginLog;
		private IPluginLoader                   pluginLoader    = null;
		private Dictionary<string,T>            pluginRegistry  = new Dictionary<string,T>();
		private List<Assembly>                  lockedPlugins   = new List<Assembly>();
		private HashSet<Assembly>               disposedPlugins = new HashSet<Assembly>();
		private Dictionary<Type,List<TypeInfo>> availTypeDict   = new Dictionary<Type,List<TypeInfo>>();

		/// <summary>
		/// Called right before removing a plugin. This allows other systems to get rid of 
		/// data and content that still depends on those plugins. Note that it is possible
		/// for some plugin termination / disposal code to be run after this event.
		/// </summary>
		public event EventHandler<DualityPluginEventArgs> PluginsRemoving = null;
		/// <summary>
		/// Called right after removing a plugin. This allows other system to clear their
		/// internal caches and clean up everything that might have been left by the removed
		/// plugin. No plugin code is run after this event has been called.
		/// </summary>
		public event EventHandler<DualityPluginEventArgs> PluginsRemoved = null;
		/// <summary>
		/// Fired whenever a Duality plugin has been initialized. This is the case after loading or reloading one.
		/// </summary>
		public event EventHandler<DualityPluginEventArgs> PluginsReady = null;
		
		
		/// <summary>
		/// [GET] The plugin loader which is used by the <see cref="PluginManager{T}"/> to discover
		/// and load available plugin assemblies.
		/// </summary>
		public IPluginLoader PluginLoader
		{
			get { return this.pluginLoader; }
		}
		/// <summary>
		/// [GET] Enumerates all currently loaded plugins.
		/// </summary>
		public IEnumerable<T> LoadedPlugins
		{
			get { return this.pluginRegistry.Values; }
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
		/// [GET] Maps assembly names to currently loaded plugins.
		/// </summary>
		protected IReadOnlyDictionary<string,T> PluginRegistry
		{
			get { return this.pluginRegistry; }
		}
		/// <summary>
		/// [GET / SET] An optional <see cref="Log"/> which is used for logging plugin
		/// loading states and issues.
		/// </summary>
		protected Log PluginLog
		{
			get { return this.pluginLog; }
			set { this.pluginLog = value ?? dummyPluginLog; }
		}


		internal PluginManager() { }

		/// <summary>
		/// Initializes the <see cref="PluginManager{T}"/> with the specified <see cref="IPluginLoader"/>.
		/// This method needs to be called once after instantiation (or previous termination) before plugins 
		/// can be loaded.
		/// </summary>
		/// <param name="pluginLoader"></param>
		public void Init(IPluginLoader pluginLoader)
		{
			if (this.pluginLoader != null) throw new InvalidOperationException("Plugin manager is already initialized.");

			this.pluginLoader = pluginLoader;
			this.pluginLoader.AssemblyResolve += this.pluginLoader_AssemblyResolve;
			this.OnInit();
		}
		/// <summary>
		/// Terminates the <see cref="PluginManager{T}"/>. This will dispose all Duality plugins and plugin data.
		/// </summary>
		public void Terminate()
		{
			if (this.pluginLoader == null) throw new InvalidOperationException("Plugin manager is is not currently initialized.");

			this.OnTerminate();
			this.ClearPlugins();
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
		/// Enumerates all currently loaded assemblies that are considered part of the 
		/// managed subset of this <see cref="PluginManager{T}"/>.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<Assembly> GetAssemblies()
		{
			return this.LoadedPlugins.Select(plugin => plugin.PluginAssembly);
		}
		/// <summary>
		/// Enumerates all locally available <see cref="System.Type">Types</see> that are assignable
		/// to the specified Type. 
		/// </summary>
		/// <param name="baseType">The base type to use for matching the result types.</param>
		/// <returns>An enumeration of all Duality types deriving from the specified type.</returns>
		public IEnumerable<TypeInfo> GetTypes(Type baseType)
		{
			List<TypeInfo> availTypes;
			if (this.availTypeDict.TryGetValue(baseType, out availTypes))
				return availTypes;

			availTypes = new List<TypeInfo>();
			IEnumerable<Assembly> asmQuery = this.GetAssemblies();
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
		/// Loads all available plugins, as well as their potentially required auxilliary libraries.
		/// </summary>
		public abstract void LoadPlugins();
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public abstract void InitPlugins();
		/// <summary>
		/// Disposes all loaded plugins and discards all related content / data.
		/// </summary>
		public void ClearPlugins()
		{
			T[] oldPlugins = this.LoadedPlugins.ToArray();
			if (oldPlugins.Length == 0) return;

			foreach (T plugin in oldPlugins)
			{
				this.disposedPlugins.Add(plugin.PluginAssembly);
			}
			this.OnPluginsRemoving(oldPlugins);
			foreach (T plugin in oldPlugins)
			{
				try
				{
					plugin.Dispose();
				}
				catch (Exception e)
				{
					this.pluginLog.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				}
			}
			this.OnPluginsRemoved(oldPlugins);
			this.pluginRegistry.Clear();
		}
		
		/// <summary>
		/// Adds an already loaded plugin Assembly to the internal Duality T registry.
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
		public T LoadPlugin(Assembly pluginAssembly, string pluginFilePath)
		{
			this.disposedPlugins.Remove(pluginAssembly);

			string asmName = pluginAssembly.GetShortAssemblyName();
			T plugin = this.pluginRegistry.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;
			
			try
			{
				TypeInfo pluginType = pluginAssembly.ExportedTypes
					.Select(t => t.GetTypeInfo())
					.FirstOrDefault(t => typeof(T).GetTypeInfo().IsAssignableFrom(t));

				if (pluginType == null) 
					throw new Exception(string.Format(
						"Plugin does not contain a public {0} class.", 
						typeof(T).Name));

				plugin = (T)pluginType.CreateInstanceOf();

				if (plugin == null) 
					throw new Exception(string.Format(
						"Failed to instantiate {0} class.", 
						Log.Type(pluginType.GetType())));

				plugin.FilePath = pluginFilePath;
				plugin.FileHash = this.pluginLoader.GetAssemblyHash(pluginFilePath);

				this.pluginRegistry.Add(plugin.AssemblyName, plugin);
			}
			catch (Exception e)
			{
				this.pluginLog.WriteError("Error loading plugin: {0}", Log.Exception(e));
				this.disposedPlugins.Add(pluginAssembly);
				plugin = null;
			}

			return plugin;
		}
		/// <summary>
		/// Reloads the specified plugin. Does not initialize it.
		/// </summary>
		/// <param name="pluginFilePath"></param>
		public T ReloadPlugin(string pluginFilePath)
		{
			// If we're trying to reload an active backend plugin, stop
			foreach (var pair in this.pluginRegistry)
			{
				T plugin = pair.Value;
				if (PathOp.ArePathsEqual(plugin.FilePath, pluginFilePath))
				{
					foreach (Assembly lockedAssembly in this.lockedPlugins)
					{
						if (plugin.PluginAssembly == lockedAssembly)
						{
							this.pluginLog.WriteError(
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
				this.pluginLog.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				return null;
			}

			// If we're overwriting an old plugin here, add the old version to the "disposed" blacklist
			string assemblyName = pluginAssembly.GetShortAssemblyName();
			T oldPlugin;
			if (this.pluginRegistry.TryGetValue(assemblyName, out oldPlugin))
			{
				this.pluginRegistry.Remove(assemblyName);
				this.disposedPlugins.Add(oldPlugin.PluginAssembly);
				this.OnPluginsRemoving(new[] { oldPlugin });
				oldPlugin.Dispose();
			}

			// Load the new plugin from the updated Assembly
			T updatedPlugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			
			// Discard temporary plugin-related data (cached Types, etc.)
			this.OnPluginsRemoved(new[] { oldPlugin });

			return updatedPlugin;
		}
		/// <summary>
		/// Initializes the specified plugin. This concludes a manual plugin load or reload operation
		/// using API like <see cref="LoadPlugin(Assembly, string)"/> and <see cref="ReloadPlugin"/>.
		/// </summary>
		/// <param name="plugin"></param>
		public void InitPlugin(T plugin)
		{
			try
			{
				this.OnInitPlugin(plugin);
				this.OnPluginsReady(new[] { plugin });
			}
			catch (Exception e)
			{
				this.pluginLog.WriteError("Error initializing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
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

		protected T LoadPlugin(string pluginFilePath)
		{
			// Check for already loaded plugins first
			string asmName = PathOp.GetFileNameWithoutExtension(pluginFilePath);
			T plugin = this.pluginRegistry.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;

			// Load the assembly from the specified path
			Assembly pluginAssembly = null;
			try
			{
				pluginAssembly = this.pluginLoader.LoadAssembly(pluginFilePath, true);
			}
			catch (Exception e)
			{
				this.pluginLog.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				plugin = null;
			}

			// If we succeeded, register the loaded assembly as a plugin
			if (pluginAssembly != null)
			{
				plugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			}

			return plugin;
		}
		protected void RemovePlugin(T plugin)
		{
			// Dispose plugin and discard plugin related data
			this.disposedPlugins.Add(plugin.PluginAssembly);
			this.OnPluginsRemoving(new[] { plugin });
			this.pluginRegistry.Remove(plugin.AssemblyName);
			try
			{
				plugin.Dispose();
			}
			catch (Exception e)
			{
				this.pluginLog.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
			}

			// Discard temporary plugin-related data (cached Types, etc.)
			this.OnPluginsRemoved(new[] { plugin });
		}

		protected virtual void OnInit() { }
		protected virtual void OnTerminate() { }
		protected abstract void OnInitPlugin(T plugin);

		private void OnPluginsReady(IEnumerable<T> plugins)
		{
			if (this.PluginsReady != null)
				this.PluginsReady(null, new DualityPluginEventArgs(plugins));
		}
		private void OnPluginsRemoving(IEnumerable<T> oldPlugins)
		{
			if (this.PluginsRemoving != null)
				this.PluginsRemoving(null, new DualityPluginEventArgs(oldPlugins));
		}
		private void OnPluginsRemoved(IEnumerable<T> oldPlugins)
		{
			if (this.PluginsRemoved != null)
				this.PluginsRemoved(null, new DualityPluginEventArgs(oldPlugins));

			// Clean cached type values
			this.availTypeDict.Clear();
		}

		private void pluginLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// Are we searching for an already loaded plugin?
			T plugin;
			if (this.pluginRegistry.TryGetValue(args.AssemblyName, out plugin))
			{
				args.Resolve(plugin.PluginAssembly);
				return;
			}
		}
	}
}
