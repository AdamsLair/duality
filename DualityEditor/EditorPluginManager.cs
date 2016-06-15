using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Duality.IO;
using Duality.Backend;

namespace Duality.Editor
{
	/// <summary>
	/// Manages loading, initialization and life cycle of Duality editor plugins.
	/// 
	/// Since all assemblies are owned by the .Net runtime that only exposes a very limited
	/// degree of control, this class should only be used statically: Disposing it would
	/// only get rid of management data, not of the actual plugin assemblies, which would
	/// then cause problems.
	/// 
	/// A static instance of this class is available through <see cref="DualityEditorApp.PluginManager"/>.
	/// </summary>
	public class EditorPluginManager
	{
		private IPluginLoader                   pluginLoader    = null;
		private Dictionary<string,EditorPlugin> loadedPlugins   = new Dictionary<string,EditorPlugin>();
		private Dictionary<Type,List<TypeInfo>> availTypeDict   = new Dictionary<Type,List<TypeInfo>>();
		
		
		/// <summary>
		/// [GET] The plugin loader which is used by the <see cref="EditorPluginManager"/> to discover
		/// and load available plugin assemblies.
		/// </summary>
		public IPluginLoader PluginLoader
		{
			get { return this.pluginLoader; }
		}
		/// <summary>
		/// [GET] Enumerates all currently loaded plugins.
		/// </summary>
		public IEnumerable<EditorPlugin> LoadedPlugins
		{
			get { return this.loadedPlugins.Values; }
		}


		/// <summary>
		/// <see cref="EditorPluginManager"/> should usually not be instantiated by users due to 
		/// its forced singleton-like usage. Use <see cref="DualityApp.PluginManager"/> instead.
		/// </summary>
		internal EditorPluginManager() { }

		/// <summary>
		/// Initializes the <see cref="EditorPluginManager"/> with the specified <see cref="IPluginLoader"/>.
		/// This method needs to be called once after instantiation (or previous termination) before plugins 
		/// can be loaded.
		/// </summary>
		/// <param name="pluginLoader"></param>
		public void Init(IPluginLoader pluginLoader)
		{
			if (this.pluginLoader != null) throw new InvalidOperationException("Plugin manager is already initialized.");

			this.pluginLoader = pluginLoader;
			this.pluginLoader.AssemblyResolve += this.pluginLoader_AssemblyResolve;
		}
		/// <summary>
		/// Terminates the <see cref="EditorPluginManager"/>. This will dispose all editor plugins and plugin data.
		/// </summary>
		public void Terminate()
		{
			if (this.pluginLoader == null) throw new InvalidOperationException("Plugin manager is is not currently initialized.");

			this.pluginLoader.AssemblyResolve -= this.pluginLoader_AssemblyResolve;
			this.pluginLoader = null;
		}
		
		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Assembly> GetEditorAssemblies()
		{
			yield return typeof(DualityApp).GetTypeInfo().Assembly;
			foreach (EditorPlugin p in this.LoadedPlugins)
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
		public IEnumerable<TypeInfo> GetEditorTypes(Type baseType)
		{
			List<TypeInfo> availTypes;
			if (this.availTypeDict.TryGetValue(baseType, out availTypes))
				return availTypes;

			availTypes = new List<TypeInfo>();
			IEnumerable<Assembly> asmQuery = this.GetEditorAssemblies();
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
		/// Loads all available editor plugins, as well as auxilliary libraries.
		/// </summary>
		public void LoadPlugins()
		{
			Log.Editor.Write("Scanning for editor plugins...");
			Log.Editor.PushIndent();

			foreach (string dllPath in this.pluginLoader.AvailableAssemblyPaths)
			{
				if (!dllPath.EndsWith(".editor.dll", StringComparison.InvariantCultureIgnoreCase))
					continue;

				Log.Core.Write("{0}...", dllPath);
				Log.Editor.PushIndent();
				LoadPlugin(dllPath);
				Log.Editor.PopIndent();
			}

			Log.Editor.PopIndent();
		}
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public void InitPlugins()
		{
			Log.Core.Write("Initializing editor plugins...");
			Log.Core.PushIndent();
			EditorPlugin[] initPlugins = this.loadedPlugins.Values.ToArray();
			foreach (EditorPlugin plugin in initPlugins)
			{
				Log.Core.Write("{0}...", plugin.AssemblyName);
				Log.Core.PushIndent();
				this.InitPlugin(plugin);
				Log.Core.PopIndent();
			}
			Log.Core.PopIndent();
		}

		/// <summary>
		/// Saves all editor plugin user data into the specified parent <see cref="XElement"/>.
		/// </summary>
		/// <param name="parentElement"></param>
		public void SaveUserData(XElement parentElement)
		{
			foreach (EditorPlugin plugin in this.LoadedPlugins)
			{
				XElement pluginElement = new XElement("Plugin");
				pluginElement.SetAttributeValue("id", plugin.Id);
				plugin.SaveUserData(pluginElement);
				if (!pluginElement.IsEmpty)
					parentElement.Add(pluginElement);
			}
		}
		/// <summary>
		/// Loads all editor plugin user data from the specified parent <see cref="XElement"/>.
		/// </summary>
		/// <param name="parentElement"></param>
		public void LoadUserData(XElement parentElement)
		{
			foreach (XElement child in parentElement.Elements("Plugin"))
			{
				string id = child.GetAttributeValue("id");
				if (id == null) continue;

				foreach (EditorPlugin plugin in this.LoadedPlugins)
				{
					if (plugin.Id != id) continue;

					plugin.LoadUserData(child);
					break;
				}
			}
		}

		private EditorPlugin LoadPlugin(string pluginFilePath)
		{
			// Check for already loaded plugins first
			string asmName = PathOp.GetFileNameWithoutExtension(pluginFilePath);
			EditorPlugin plugin = this.loadedPlugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;

			// Load the assembly from the specified path
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

			// If we succeeded, register the loaded assembly as a plugin
			if (pluginAssembly != null)
			{
				plugin = this.LoadPlugin(pluginAssembly, pluginFilePath);
			}

			return plugin;
		}
		private EditorPlugin LoadPlugin(Assembly pluginAssembly, string pluginFilePath)
		{
			string asmName = pluginAssembly.GetShortAssemblyName();
			EditorPlugin plugin = this.loadedPlugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;
			
			try
			{
				TypeInfo pluginType = pluginAssembly.ExportedTypes
					.Select(t => t.GetTypeInfo())
					.FirstOrDefault(t => typeof(EditorPlugin).GetTypeInfo().IsAssignableFrom(t));

				if (pluginType == null) 
					throw new Exception(string.Format(
						"Plugin does not contain a public {0} class.", 
						typeof(EditorPlugin).Name));

				plugin = (EditorPlugin)pluginType.CreateInstanceOf();

				if (plugin == null) 
					throw new Exception(string.Format(
						"Failed to instantiate {0} class.", 
						Log.Type(pluginType.GetType())));

				this.loadedPlugins.Add(plugin.AssemblyName, plugin);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin: {0}", Log.Exception(e));
				plugin = null;
			}

			return plugin;
		}
		private void InitPlugin(EditorPlugin plugin)
		{
			try
			{
				plugin.InitPlugin(DualityEditorApp.MainForm);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error initializing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				this.loadedPlugins.Remove(plugin.AssemblyName);
			}
		}

		private void pluginLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// First assume we are searching for a dynamically loaded plugin assembly
			EditorPlugin plugin;
			if (this.loadedPlugins.TryGetValue(args.AssemblyName, out plugin))
			{
				args.Resolve(plugin.PluginAssembly);
				return;
			}
			// Not there? Search for other libraries in the Plugins folder
			else
			{
				// Search for editor plugins that haven't been loaded yet, and load them first.
				// This is required to satisfy dependencies while loading plugins, since
				// we can't know which one requires which beforehand.
				foreach (string libFile in this.pluginLoader.AvailableAssemblyPaths)
				{
					if (!libFile.EndsWith(".editor.dll", StringComparison.OrdinalIgnoreCase))
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
			}
		}
	}
}
