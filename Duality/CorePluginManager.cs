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

		/// <summary>
		/// <see cref="CorePluginManager"/> should usually not be instantiated by users due to 
		/// its forced singleton-like usage. Use <see cref="DualityApp.PluginManager"/> instead.
		/// </summary>
		internal CorePluginManager() { }
		
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
			Log.Core.Write("Scanning for core plugins...");
			Log.Core.PushIndent();

			List<string> auxilLibs = new List<string>();
			foreach (string dllPath in this.PluginLoader.AvailableAssemblyPaths)
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
						this.PluginLoader.LoadAssembly(dllPath, false);
					}
					catch (BadImageFormatException) { }
				}

				Log.Core.PopIndent();
			}
		}
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public override void InitPlugins()
		{
			Log.Core.Write("Initializing core plugins...");
			Log.Core.PushIndent();
			CorePlugin[] initPlugins = this.LoadedPlugins.ToArray();
			foreach (CorePlugin plugin in initPlugins)
			{
				Log.Core.Write("{0}...", plugin.AssemblyName);
				Log.Core.PushIndent();
				this.InitPlugin(plugin);
				Log.Core.PopIndent();
			}
			Log.Core.PopIndent();
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
					Assembly assembly = this.PluginLoader.LoadAssembly(libFile, false);
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
