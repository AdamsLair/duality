using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

using Duality.IO;
using Duality.Backend;

using WeifenLuo.WinFormsUI.Docking;

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
	public class EditorPluginManager : PluginManager<EditorPlugin>
	{
		private Assembly[] editorAssemblies = new Assembly[] { typeof(DualityEditorApp).GetTypeInfo().Assembly };

		/// <summary>
		/// <see cref="EditorPluginManager"/> should usually not be instantiated by users due to 
		/// its forced singleton-like usage. Use <see cref="DualityApp.PluginManager"/> instead.
		/// </summary>
		internal EditorPluginManager() { }

		/// <summary>
		/// Enumerates all currently loaded editor assemblies that are part of Duality, i.e. 
		/// the editor Assembly itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public override IEnumerable<Assembly> GetAssemblies()
		{
			return this.editorAssemblies.Concat(base.GetAssemblies());
		}

		/// <summary>
		/// Loads all available editor plugins, as well as auxilliary libraries.
		/// </summary>
		public override void LoadPlugins()
		{
			Log.Editor.Write("Scanning for editor plugins...");
			Log.Editor.PushIndent();

			foreach (string dllPath in this.PluginLoader.AvailableAssemblyPaths)
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
		public override void InitPlugins()
		{
			Log.Core.Write("Initializing editor plugins...");
			Log.Core.PushIndent();
			EditorPlugin[] initPlugins = this.LoadedPlugins.ToArray();
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
		/// <summary>
		/// As part of the docking suite layout deserialization, this method resolves
		/// a persistent type name to an instance of the desired type.
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public IDockContent DeserializeDockContent(string typeName)
		{
			Type dockContentType = null;
			Assembly dockContentAssembly = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblies)
			{
				if ((dockContentType = a.GetType(typeName)) != null)
				{
					dockContentAssembly = a;
					break;
				}
			}
			
			if (dockContentType == null) 
				return null;
			else
			{
				// First ask plugins from the dock contents assembly for existing instances
				IDockContent deserializeDockContent = null;
				foreach (EditorPlugin plugin in this.LoadedPlugins)
				{
					if (plugin.GetType().Assembly == dockContentAssembly)
					{
						deserializeDockContent = plugin.DeserializeDockContent(dockContentType);
						if (deserializeDockContent != null) break;
					}
				}

				// If none exists, create one
				return deserializeDockContent ?? (dockContentType.GetTypeInfo().CreateInstanceOf() as IDockContent);
			}
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
		protected override void OnInitPlugin(EditorPlugin plugin)
		{
			plugin.InitPlugin(DualityEditorApp.MainForm);
		}

		private void pluginLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// Search for editor plugins that haven't been loaded yet, and load them first.
			// This is required to satisfy dependencies while loading plugins, since
			// we can't know which one requires which beforehand.
			foreach (string libFile in this.PluginLoader.AvailableAssemblyPaths)
			{
				if (!libFile.EndsWith(".editor.dll", StringComparison.OrdinalIgnoreCase))
					continue;

				string libName = PathOp.GetFileNameWithoutExtension(libFile);
				if (libName.Equals(args.AssemblyName, StringComparison.OrdinalIgnoreCase))
				{
					EditorPlugin plugin = this.LoadPlugin(libFile);
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
