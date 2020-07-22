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
		internal EditorPluginManager()
		{
			this.PluginLog = Logs.Editor;
		}

		/// <summary>
		/// Enumerates all currently loaded editor assemblies that are part of Duality, i.e. 
		/// the editor Assembly itsself and all loaded plugins.
		/// </summary>
		public override IEnumerable<Assembly> GetAssemblies()
		{
			return this.editorAssemblies.Concat(base.GetAssemblies());
		}

		/// <summary>
		/// Loads all available editor plugins, as well as auxilliary libraries.
		/// </summary>
		public override void LoadPlugins()
		{
			this.PluginLog.Write("Scanning for editor plugins...");
			this.PluginLog.PushIndent();

			foreach (string dllPath in this.AssemblyLoader.AvailableAssemblyPaths)
			{
				if (!dllPath.EndsWith(".editor.dll", StringComparison.InvariantCultureIgnoreCase))
					continue;

				this.PluginLog.Write("{0}...", dllPath);
				this.PluginLog.PushIndent();
				LoadPlugin(dllPath);
				this.PluginLog.PopIndent();
			}

			this.PluginLog.PopIndent();
		}
		/// <summary>
		/// Initializes all previously loaded plugins.
		/// </summary>
		public override void InitPlugins()
		{
			this.PluginLog.Write("Initializing editor plugins...");
			this.PluginLog.PushIndent();
			EditorPlugin[] initPlugins = this.LoadedPlugins.ToArray();
			foreach (EditorPlugin plugin in initPlugins)
			{
				this.PluginLog.Write("{0}...", plugin.AssemblyName);
				this.PluginLog.PushIndent();
				this.InitPlugin(plugin);
				this.PluginLog.PopIndent();
			}
			this.PluginLog.PopIndent();
		}

		/// <summary>
		/// Saves all editor plugin user data into the provided settings object.
		/// </summary>
		/// <param name="settings"></param>
		private void SaveUserData(PluginSettings settings)
		{
			settings.Clear();
			foreach (EditorPlugin loadedPlugin in this.LoadedPlugins)
			{
				loadedPlugin.SaveUserData(settings);
			}

			// Legacy support
			XElement parentElement = settings.OldStyleSettings;
			foreach (EditorPlugin plugin in this.LoadedPlugins)
			{
				XElement pluginElement = new XElement("Plugin");
				pluginElement.SetAttributeValue("id", plugin.Id);

				plugin.SaveUserData(pluginElement);
				if (!pluginElement.IsEmpty)
					parentElement.Add(pluginElement);
			}

			settings.OldStyleSettings = parentElement;
		}
		/// <summary>
		/// Loads all editor plugin user data from the provided settings object.
		/// </summary>
		/// <param name="pluginSettings"></param>
		private void LoadUserData(PluginSettings pluginSettings)
		{
			foreach (EditorPlugin loadedPlugin in this.LoadedPlugins)
			{
				loadedPlugin.LoadUserData(pluginSettings);
			}

			// Legacy support
			XElement parentElement = pluginSettings.OldStyleSettings;
			XElement[] childs = parentElement.Elements("Plugin").ToArray();

			foreach (EditorPlugin loadedPlugin in this.LoadedPlugins)
			{
				XElement pluginSetting = childs.FirstOrDefault(x => x.GetAttributeValue("id") == loadedPlugin.Id);
				if (pluginSetting == null)
				{
					XElement defaultSettings = loadedPlugin.GetDefaultUserData();

					if (defaultSettings == null) continue;
					if (defaultSettings.Name != "Plugin") throw new InvalidOperationException("Expected a Plugin element as root");

					defaultSettings.SetAttributeValue("id", loadedPlugin.Id);
					pluginSetting = defaultSettings;
				}
				loadedPlugin.LoadUserData(pluginSetting);
			}
		}

		protected override void OnInit()
		{
			base.OnInit();
			this.AssemblyLoader.AssemblyResolve += this.assemblyLoader_AssemblyResolve;
			DualityEditorApp.UserData.Applying += this.EditorUserData_Applying;
			DualityEditorApp.UserData.Saving += this.EditorUserData_Saving;
		}
		protected override void OnTerminate()
		{
			base.OnTerminate();
			this.AssemblyLoader.AssemblyResolve -= this.assemblyLoader_AssemblyResolve;
			DualityEditorApp.UserData.Applying -= this.EditorUserData_Applying;
			DualityEditorApp.UserData.Saving -= this.EditorUserData_Saving;
		}
		protected override void OnInitPlugin(EditorPlugin plugin)
		{
			plugin.InitPlugin(DualityEditorApp.MainForm);
		}

		private void assemblyLoader_AssemblyResolve(object sender, AssemblyResolveEventArgs args)
		{
			// Early-out, if the Assembly has already been resolved
			if (args.IsResolved) return;

			// Search for editor plugins that haven't been loaded yet, and load them first.
			// This is required to satisfy dependencies while loading plugins, since
			// we can't know which one requires which beforehand.
			foreach (string libFile in this.AssemblyLoader.AvailableAssemblyPaths)
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
		private void EditorUserData_Applying(object sender, EventArgs e)
		{
			this.LoadUserData(DualityEditorApp.UserData.Instance.PluginSettings);
		}
		private void EditorUserData_Saving(object sender, EventArgs e)
		{
			this.SaveUserData(DualityEditorApp.UserData.Instance.PluginSettings);
		}
	}
}
