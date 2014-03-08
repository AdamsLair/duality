using System;
using System.Reflection;
using System.Xml.Linq;

using Duality;
using Duality.Editor.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor
{
	public abstract class EditorPlugin
	{
		private	Assembly	assembly	= null;
		private	string		asmName		= null;

		/// <summary>
		/// The Plugins ID. This should be unique.
		/// </summary>
		public abstract string Id { get; }
		public Assembly PluginAssembly
		{
			get { return this.assembly; }
		}
		public string AssemblyName
		{
			get { return this.asmName; }
		}
		
		protected EditorPlugin()
		{
			this.assembly = this.GetType().Assembly;
			this.asmName = this.assembly.GetShortAssemblyName();
		}
		/// <summary>
		/// This method is called as soon as the plugins assembly is loaded. Initializes the plugins internal data.
		/// </summary>
		internal protected virtual void LoadPlugin() {}
		/// <summary>
		/// This method is called when all plugins and the editors user data and layout are loaded. May initialize GUI.
		/// </summary>
		/// <param name="main"></param>
		internal protected virtual void InitPlugin(MainForm main) {}
		/// <summary>
		/// Saves the plugins user data to the provided Xml Node.
		/// </summary>
		/// <param name="node"></param>
		internal protected virtual void SaveUserData(XElement node) {}
		/// <summary>
		/// Loads the plugins user data from the provided Xml Node.
		/// </summary>
		/// <param name="node"></param>
		internal protected virtual void LoadUserData(XElement node) {}
		/// <summary>
		/// Called when initializing the editors layout and trying to set up one of this plugins DockContent.
		/// Returns an IDockContent instance of the specified dockContentType. May return already existing
		/// DockContent, a new an pre-setup instance or null as default.
		/// </summary>
		/// <param name="dockContentType"></param>
		/// <returns></returns>
		internal protected virtual IDockContent DeserializeDockContent(Type dockContentType) { return null; }
	}
}
