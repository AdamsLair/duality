using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Duality.IO;
using Duality.Backend;
using Duality.Serialization;
using Duality.Cloning;
using Duality.Resources;
using Duality.Drawing;
using Duality.Input;

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
		/// Called when Duality needs to discard plugin data such as cached Types and values.
		/// </summary>
		public event EventHandler DiscardPluginDataRequested = null;
		/// <summary>
		/// Fired whenever a core plugin has been initialized. This is the case after loading or reloading one.
		/// </summary>
		public event EventHandler<CorePluginEventArgs> PluginReady = null;
		
		
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
		/// Should not be instantiated by users due to its forced singleton-like
		/// usage. Use <see cref="DualityApp.PluginManager"/> instead.
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
			this.pluginLoader.Init(this.pluginLoader_ResolveAssembly);
		}
		/// <summary>
		/// Terminates the <see cref="CorePluginManager"/>. This will dispose all core plugins and plugin data.
		/// </summary>
		public void Terminate()
		{
			if (this.pluginLoader == null) throw new InvalidOperationException("Plugin manager is is not currently initialized.");

			this.ClearPlugins();
			this.pluginLoader.Terminate();
			this.pluginLoader = null;
		}
		/// <summary>
		/// Requests the disposal of all content / data that is dependent on any of the currently active
		/// plugins. This functioanlity is invoked automatically as part of reloading or removing plugins.
		/// </summary>
		public void DiscardPluginData()
		{
			this.OnDiscardPluginData();
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
			if (this.availTypeDict.TryGetValue(baseType, out availTypes)) return availTypes;

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
			if (this.loadedPlugins.Count > 0) throw new InvalidOperationException("Can't load plugins more than once.");

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
			foreach (CorePlugin plugin in this.loadedPlugins.Values)
			{
				this.disposedPlugins.Add(plugin.PluginAssembly);
			}
			this.DiscardPluginData();
			foreach (CorePlugin plugin in this.loadedPlugins.Values)
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
			this.CleanupAfterPlugins(loadedPlugins.Values);
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
				this.DiscardPluginData();
				oldPlugin.Dispose();
			}

			// Load the new plugin from the updated Assembly
			CorePlugin updatedPlugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			
			// Discard temporary plugin-related data (cached Types, etc.)
			this.CleanupAfterPlugins(new[] { oldPlugin });

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
				this.OnPluginReady(plugin);
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
			this.DiscardPluginData();
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
			this.CleanupAfterPlugins(new[] { plugin });
		}

		private void OnPluginReady(CorePlugin plugin)
		{
			if (this.PluginReady != null)
				this.PluginReady(null, new CorePluginEventArgs(plugin));
		}
		private void OnDiscardPluginData()
		{
			if (this.DiscardPluginDataRequested != null)
				this.DiscardPluginDataRequested(null, EventArgs.Empty);

			// Dispose any existing Resources that could reference plugin data
			VisualLog.ClearAll();
			if (!Scene.Current.IsEmpty)
				Scene.Current.Dispose();
			foreach (Resource r in ContentProvider.EnumeratePluginContent().ToArray())
				ContentProvider.RemoveContent(r.Path);
		}
		private void CleanupAfterPlugins(IEnumerable<CorePlugin> oldPlugins)
		{
			oldPlugins = oldPlugins.NotNull().Distinct();
			if (!oldPlugins.Any()) oldPlugins = null;

			// Clean globally cached type values
			this.availTypeDict.Clear();
			ImageCodec.ClearTypeCache();
			ObjectCreator.ClearTypeCache();
			ReflectionHelper.ClearTypeCache();
			Component.ClearTypeCache();
			Serializer.ClearTypeCache();
			CloneProvider.ClearTypeCache();
			
			// Clean input sources that a disposed Assembly forgot to unregister.
			if (oldPlugins != null)
			{
				foreach (CorePlugin plugin in oldPlugins)
					this.CleanInputSources(plugin.PluginAssembly);
			}
			// Clean event bindings that are still linked to the disposed Assembly.
			if (oldPlugins != null)
			{
				foreach (CorePlugin plugin in oldPlugins)
					this.CleanEventBindings(plugin.PluginAssembly);
			}
		}
		private void CleanEventBindings(Assembly invalidAssembly)
		{
			// Note that this method is only a countermeasure against common mistakes. It doesn't guarantee
			// full error safety in all cases. Event bindings inbetween different plugins aren't checked,
			// for example.

			string warningText = string.Format(
				"Found leaked event bindings to invalid Assembly '{0}' from {1}. " +
				"This is a common problem when registering global events from within a CorePlugin " +
				"without properly unregistering them later. Please make sure that all events are " +
				"unregistered in CorePlugin::OnDisposePlugin().",
				invalidAssembly.GetShortAssemblyName(),
				"{0}");

			if (ReflectionHelper.CleanEventBindings(typeof(DualityApp),      invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)));
			if (ReflectionHelper.CleanEventBindings(typeof(Scene),           invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(Scene)));
			if (ReflectionHelper.CleanEventBindings(typeof(Resource),        invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(Resource)));
			if (ReflectionHelper.CleanEventBindings(typeof(ContentProvider), invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(ContentProvider)));
			if (ReflectionHelper.CleanEventBindings(DualityApp.Keyboard,     invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Keyboard");
			if (ReflectionHelper.CleanEventBindings(DualityApp.Mouse,        invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Mouse");
			foreach (JoystickInput joystick in DualityApp.Joysticks)
				if (ReflectionHelper.CleanEventBindings(joystick,            invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Joysticks");
			foreach (GamepadInput gamepad in DualityApp.Gamepads)
				if (ReflectionHelper.CleanEventBindings(gamepad,             invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Gamepads");
		}
		private void CleanInputSources(Assembly invalidAssembly)
		{
			string warningText = string.Format(
				"Found leaked input source '{1}' defined in invalid Assembly '{0}'. " +
				"This is a common problem when registering input sources from within a CorePlugin " +
				"without properly unregistering them later. Please make sure that all sources are " +
				"unregistered in CorePlugin::OnDisposePlugin() or sooner.",
				invalidAssembly.GetShortAssemblyName(),
				"{0}");

			if (DualityApp.Mouse.Source != null && DualityApp.Mouse.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(DualityApp.Mouse.Source.GetType()));
				DualityApp.Mouse.Source = null;
			}
			if (DualityApp.Keyboard.Source != null && DualityApp.Keyboard.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(DualityApp.Keyboard.Source.GetType()));
				DualityApp.Keyboard.Source = null;
			}
			foreach (JoystickInput joystick in DualityApp.Joysticks.ToArray())
			{
				if (joystick.Source != null && joystick.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(joystick.Source.GetType()));
					DualityApp.Joysticks.RemoveSource(joystick.Source);
				}
			}
			foreach (GamepadInput gamepad in DualityApp.Gamepads.ToArray())
			{
				if (gamepad.Source != null && gamepad.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(gamepad.Source.GetType()));
					DualityApp.Gamepads.RemoveSource(gamepad.Source);
				}
			}
		}

		private Assembly pluginLoader_ResolveAssembly(ResolveAssemblyEventArgs args)
		{
			// First assume we are searching for a dynamically loaded plugin assembly
			CorePlugin plugin;
			if (this.loadedPlugins.TryGetValue(args.AssemblyName, out plugin))
			{
				return plugin.PluginAssembly;
			}
			// Not there? Search for other libraries in the Plugins folder
			else
			{
				//  Search for plugins that haven't been loaded yet, and load them first
				foreach (string libFile in this.pluginLoader.AvailableAssemblyPaths)
				{
					string libFileEnding = ".core.dll";
					if (!libFile.EndsWith(libFileEnding, StringComparison.OrdinalIgnoreCase))
						continue;

					string libName = libFile.Remove(libFile.Length - libFileEnding.Length, libFileEnding.Length);
					if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
					{
						plugin = this.LoadPlugin(libFile);
						if (plugin != null) return plugin.PluginAssembly;
					}
				}

				// Search for other libraries that might be located inside the plugin directory
				foreach (string libFile in this.pluginLoader.AvailableAssemblyPaths)
				{
					string libName = PathOp.GetFileNameWithoutExtension(libFile);
					if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
					{
						return this.pluginLoader.LoadAssembly(libFile, false);
					}
				}
			}

			// Admit that we didn't find anything.
			Log.Core.WriteWarning(
				"Can't resolve Assembly '{0}': None of the available assembly paths matches the requested name.",
				args.AssemblyName);
			return null;
		}
	}
}
