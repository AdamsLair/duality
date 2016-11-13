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
	public class CorePluginManager : PluginManager<CorePlugin>
	{
		private Assembly[] coreAssemblies = new Assembly[] { typeof(DualityApp).GetTypeInfo().Assembly };
		private Dictionary<string,Assembly> auxilRegistry = new Dictionary<string,Assembly>();

		/// <summary>
		/// <see cref="CorePluginManager"/> should usually not be instantiated by users due to 
		/// its forced singleton-like usage. Use <see cref="DualityApp.PluginManager"/> instead.
		/// </summary>
		internal CorePluginManager()
		{
			this.PluginLog = Log.Core;
		}
		
		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Assembly> GetAssemblies()
		{
			return this.coreAssemblies.Concat(base.GetAssemblies());
		}

		/// <summary>
		/// Loads all available core plugins, as well as auxilliary libraries.
		/// </summary>
		public override void LoadPlugins()
		{
			this.PluginLog.Write("Scanning for core plugins...");
			this.PluginLog.PushIndent();

			List<string> auxilLibs = new List<string>();
			foreach (string dllPath in this.PluginLoader.AvailableAssemblyPaths)
			{
				if (!dllPath.EndsWith(".core.dll", StringComparison.OrdinalIgnoreCase))
				{ 
					if (!dllPath.EndsWith(".editor.dll", StringComparison.OrdinalIgnoreCase))
						auxilLibs.Add(dllPath);
					continue;
				}

				this.PluginLog.Write("{0}...", dllPath);
				this.PluginLog.PushIndent();
				this.LoadPlugin(dllPath);
				this.PluginLog.PopIndent();
			}

			this.PluginLog.PopIndent();

			// Make sure to have all plugin-related Assemblies available even before even
			// getting an AssemblyResolve event - we might need to resolve their Types due
			// to deserialization, which may happen before touching any related class in code.
			if (auxilLibs.Count > 0)
			{
				this.PluginLog.Write("Loading auxiliary libraries...");
				this.PluginLog.PushIndent();

				foreach (string dllPath in auxilLibs)
				{
					// Load the Assembly in try-only mode, as we might accidentally stumble upon
					// unmanaged or incompatible Assemblies in the process.
					this.LoadAuxilliaryLibrary(dllPath, true);
				}

				this.PluginLog.PopIndent();
			}
		}
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public override void InitPlugins()
		{
			this.PluginLog.Write("Initializing core plugins...");
			this.PluginLog.PushIndent();
			CorePlugin[] initPlugins = this.LoadedPlugins.ToArray();
			foreach (CorePlugin plugin in initPlugins)
			{
				this.PluginLog.Write("{0}...", plugin.AssemblyName);
				this.PluginLog.PushIndent();
				this.InitPlugin(plugin);
				this.PluginLog.PopIndent();
			}
			this.PluginLog.PopIndent();
		}

		/// <summary>
		/// Invokes each plugin's <see cref="CorePlugin.OnBeforeUpdate"/> event handler.
		/// </summary>
		public void InvokeBeforeUpdate()
		{
			foreach (CorePlugin plugin in this.LoadedPlugins)
				plugin.OnBeforeUpdate();
		}
		/// <summary>
		/// Invokes each plugin's <see cref="CorePlugin.OnAfterUpdate"/> event handler.
		/// </summary>
		public void InvokeAfterUpdate()
		{
			foreach (CorePlugin plugin in this.LoadedPlugins)
				plugin.OnAfterUpdate();
		}
		/// <summary>
		/// Invokes each plugin's <see cref="CorePlugin.OnExecContextChanged"/> event handler.
		/// </summary>
		public void InvokeExecContextChanged(DualityApp.ExecutionContext previousContext)
		{
			foreach (CorePlugin plugin in this.LoadedPlugins)
				plugin.OnExecContextChanged(previousContext);
		}
		
		protected override void OnInit()
		{
			base.OnInit();
			this.PluginLoader.AssemblyResolve += this.pluginLoader_AssemblyResolve;
		}
		protected override void OnTerminate()
		{
			base.OnTerminate();
			this.PluginLoader.AssemblyResolve -= this.pluginLoader_AssemblyResolve;
		}
		protected override void OnInitPlugin(CorePlugin plugin)
		{
			plugin.InitPlugin();
		}

		/// <summary>
		/// Loads a managed non-plugin <see cref="Assembly"/> and returns it. Each
		/// <see cref="Assembly"/> is only loaded once, all subsequent calls will return
		/// the cached instance.
		/// </summary>
		/// <param name="dllPath">The path to load the <see cref="Assembly"/> file from.</param>
		/// <param name="tryAndFailSilently">
		/// If true, any exceptions caused by attempting to load the library itself
		/// (such as <see cref="BadImageFormatException"/>) are catched and ignored
		/// without reporting an error.
		/// </param>
		/// <returns></returns>
		private Assembly LoadAuxilliaryLibrary(string dllPath, bool tryAndFailSilently)
		{
			// Check for already loaded assemblies first
			string asmName = PathOp.GetFileNameWithoutExtension(dllPath);
			Assembly auxilAssembly;
			if (this.auxilRegistry.TryGetValue(asmName, out auxilAssembly))
				return auxilAssembly;

			// Load the assembly from the specified path
			try
			{
				auxilAssembly = this.PluginLoader.LoadAssembly(dllPath);
			}
			catch (Exception)
			{
				if (!tryAndFailSilently)
					throw;
			}

			// If we succeeded, register the loaded assembly for re-use
			if (auxilAssembly != null)
			{
				this.auxilRegistry.Add(
					auxilAssembly.GetShortAssemblyName(), 
					auxilAssembly);
			}

			return auxilAssembly;
		}

		private void pluginLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// Search for core plugins that haven't been loaded yet, and load them first.
			// This is required to satisfy dependencies while loading plugins, since
			// we can't know which one requires which beforehand.
			foreach (string libFile in this.PluginLoader.AvailableAssemblyPaths)
			{
				if (!libFile.EndsWith(".core.dll", StringComparison.OrdinalIgnoreCase))
					continue;

				string libName = PathOp.GetFileNameWithoutExtension(libFile);
				if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					CorePlugin plugin = this.LoadPlugin(libFile);
					if (plugin != null)
					{
						args.Resolve(plugin.PluginAssembly);
						return;
					}
				}
			}

			// Search for other libraries that might be located inside the plugin directory
			foreach (string libFile in this.PluginLoader.AvailableAssemblyPaths)
			{
				// Don't load editor (or any other) plugins here, only auxilliary libs allowed
				if (libFile.EndsWith(".editor.dll", StringComparison.OrdinalIgnoreCase))
					continue;

				string libName = PathOp.GetFileNameWithoutExtension(libFile);
				if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					Assembly assembly = this.LoadAuxilliaryLibrary(libFile, false);
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
