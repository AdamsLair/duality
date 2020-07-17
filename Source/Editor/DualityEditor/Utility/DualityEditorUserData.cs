using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Duality.Resources;

namespace Duality.Editor
{
	/// <summary>
	/// Contains user settings for the Duality editor application, such UI layout or preferences.
	/// </summary>
	public class DualityEditorUserData
	{
		private DesignTimeObjectDataManager perObjectData = new DesignTimeObjectDataManager();
		private XElement dockPanelState = XElement.Parse(Properties.GeneralRes.DefaultDockPanelData);
		private bool backups = true;
		private AutosaveFrequency autoSaves = AutosaveFrequency.ThirtyMinutes;
		private bool firstSession = false;
		private int activeDocumentIndex = 0;
		private bool startWithLastScene = true;
		private ContentRef<Scene> lastOpenScene = null;
		private PluginSettings pluginSettings = new PluginSettings();


		/// <summary>
		/// Per-object user settings, such as global object visibility or lock states.
		/// </summary>
		internal DesignTimeObjectDataManager PerObjectData
		{
			get { return this.perObjectData; }
			set { this.perObjectData = value; }
		}
		/// <summary>
		/// A serialized XML blob representing the UI dock panel layout to use when starting the editor.
		/// </summary>
		public XElement DockPanelState
		{
			get { return this.dockPanelState; }
			set { this.dockPanelState = value; }
		}
		/// <summary>
		/// Specifies whether the editor will copy previous versions of saved resources to a local Backups folder. 
		/// </summary>
		public bool Backups
		{
			get { return this.backups; }
			set { this.backups = value; }
		}
		/// <summary>
		/// Specifies auto-save behavior in the editor, e.g. the frequency at which unsaved Resources are saved.
		/// </summary>
		public AutosaveFrequency AutoSaves
		{
			get { return this.autoSaves; }
			set { this.autoSaves = value; }
		}
		/// <summary>
		/// Whether this is the first time the editor has been started in this project.
		/// </summary>
		public bool FirstSession
		{
			get { return this.firstSession; }
			set { this.firstSession = value; }
		}
		/// <summary>
		/// The index of the dock panel main area's active document, to be applied when starting the editor.
		/// </summary>
		public int ActiveDocumentIndex
		{
			get { return this.activeDocumentIndex; }
			set { this.activeDocumentIndex = value; }
		}
		/// <summary>
		/// Whether the editor should open the <see cref="LastOpenScene"/> on startup.
		/// </summary>
		public bool StartWithLastScene
		{
			get { return this.startWithLastScene; }
			set { this.startWithLastScene = value; }
		}
		/// <summary>
		/// A reference to the most recently opened scene. Used in combination with <see cref="StartWithLastScene"/>.
		/// </summary>
		public ContentRef<Scene> LastOpenScene
		{
			get { return this.lastOpenScene; }
			set { this.lastOpenScene = value; }
		}
		/// <summary>
		/// Container object for each editor plugin's specific user settings.
		/// </summary>
		public PluginSettings PluginSettings
		{
			get { return this.pluginSettings; }
			set { this.pluginSettings = value; }
		}
	}
}
