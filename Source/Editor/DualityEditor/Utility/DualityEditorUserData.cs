using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Duality.Resources;

namespace Duality.Editor
{
	public class DualityEditorUserData
	{
		private DesignTimeObjectDataManager perObjectData = new DesignTimeObjectDataManager();
		private XElement dockPanelState = XElement.Parse(Properties.GeneralRes.DefaultDockPanelData);
		private bool backups = false;
		private AutosaveFrequency autoSaves = AutosaveFrequency.ThirtyMinutes;
		private bool firstSession = false;
		private int activeDocumentIndex = 0;
		private bool startWithLastScene = false;
		private ContentRef<Scene> lastOpenScene = null;
		private PluginSettings pluginSettings = new PluginSettings();


		internal DesignTimeObjectDataManager PerObjectData
		{
			get { return this.perObjectData; }
			set { this.perObjectData = value; }
		}
		public XElement DockPanelState
		{
			get { return this.dockPanelState; }
			set { this.dockPanelState = value; }
		}
		public bool Backups
		{
			get { return this.backups; }
			set { this.backups = value; }
		}
		public AutosaveFrequency AutoSaves
		{
			get { return this.autoSaves; }
			set { this.autoSaves = value; }
		}
		public bool FirstSession
		{
			get { return this.firstSession; }
			set { this.firstSession = value; }
		}
		public int ActiveDocumentIndex
		{
			get { return this.activeDocumentIndex; }
			set { this.activeDocumentIndex = value; }
		}
		public bool StartWithLastScene
		{
			get { return this.startWithLastScene; }
			set { this.startWithLastScene = value; }
		}
		public ContentRef<Scene> LastOpenScene
		{
			get { return this.lastOpenScene; }
			set { this.lastOpenScene = value; }
		}
		public PluginSettings PluginSettings
		{
			get { return this.pluginSettings; }
			set { this.pluginSettings = value; }
		}
	}
}
