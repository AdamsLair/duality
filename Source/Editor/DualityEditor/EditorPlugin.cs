using System;
using System.Reflection;
using System.Xml.Linq;

using Duality;
using Duality.Editor.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor
{
	public abstract class EditorPlugin : DualityPlugin
	{
		/// <summary>
		/// The Plugins ID. This should be unique.
		/// </summary>
		public abstract string Id { get; }
		
		/// <summary>
		/// This method is called when all plugins and the editors user data and layout are loaded. May initialize GUI.
		/// </summary>
		/// <param name="main"></param>
		internal protected virtual void InitPlugin(MainForm main) {}
		/// <summary>
		/// Saves the plugins user data to the provided Xml Node.
		/// </summary>
		/// <param name="node"></param>
		[Obsolete("Use the model based api")]
		internal protected virtual void SaveUserData(XElement node) {}

		[Obsolete("Use the model based api")]
		internal protected virtual XElement GetDefaultUserData()
		{
			return null;
		}
		/// <summary>
		/// Loads the plugins user data from the provided Xml Node.
		/// </summary>
		/// <param name="node"></param>
		[Obsolete("Use the model based api")]
		internal protected virtual void LoadUserData(XElement node) {}

		/// <summary>
		/// Saves the plugins user data by populating the provided settings object.
		/// </summary>
		internal protected virtual void SaveUserData(PluginSettings settings) { }
		/// <summary>
		/// Loads the plugins user data from the provided settings object.
		/// </summary>
		/// <param name="settings"></param>
		internal protected virtual void LoadUserData(PluginSettings settings) { }
		/// <summary>
		/// Called when initializing the editors layout and trying to set up one of this plugins DockContent.
		/// Returns an IDockContent instance of the specified dockContentType. May return already existing
		/// DockContent, a new an pre-setup instance or null as default.
		/// </summary>
		/// <param name="dockContentType"></param>
		internal protected virtual IDockContent DeserializeDockContent(Type dockContentType) { return null; }
	}
}
