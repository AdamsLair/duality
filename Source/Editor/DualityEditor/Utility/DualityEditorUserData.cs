using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Duality.Resources;

namespace Duality.Editor
{
	public class DualityEditorUserData
	{
		private DesignTimeObjectDataManager designTimeObjectDataManager = new DesignTimeObjectDataManager();
		internal DesignTimeObjectDataManager DesignTimeObjectDataManager
		{
			get { return this.designTimeObjectDataManager; }
			set { this.designTimeObjectDataManager = value; }
		}

		private XElement dockPanelState = XElement.Parse(Properties.GeneralRes.DefaultDockPanelData);
		public XElement DockPanelState
		{
			get { return this.dockPanelState; }
			set { this.dockPanelState = value; }
		}

		private bool backups = false;
		public bool Backups
		{
			get { return this.backups; }
			set { this.backups = value; }
		}

		private AutosaveFrequency autoSaves = AutosaveFrequency.ThirtyMinutes;
		public AutosaveFrequency AutoSaves
		{
			get { return this.autoSaves; }
			set { this.autoSaves = value; }
		}

		private bool firstSession = false;
		public bool FirstSession
		{
			get { return this.firstSession; }
			set { this.firstSession = value; }
		}

		private int activeDocumentIndex = 0;
		public int ActiveDocumentIndex
		{
			get { return this.activeDocumentIndex; }
			set { this.activeDocumentIndex = value; }
		}

		private bool startWithLastScene = false;
		public bool StartWithLastScene
		{
			get { return this.startWithLastScene; }
			set { this.startWithLastScene = value; }
		}

		private ContentRef<Scene> lastOpenScene = null;
		public ContentRef<Scene> LastOpenScene
		{
			get { return this.lastOpenScene; }
			set { this.lastOpenScene = value; }
		}

		private PluginSettings pluginSettings = new PluginSettings();
		public PluginSettings PluginSettings
		{
			get { return this.pluginSettings; }
			set { this.pluginSettings = value; }
		}
	}

	public class PluginSettings
	{
		private XElement oldStyleSettings = XElement.Parse("<Plugins></Plugins>");

		[Obsolete("Use the model based api")]
		public XElement OldStyleSettings
		{
			get { return this.oldStyleSettings; }
			set { this.oldStyleSettings = value; }
		}

		private List<object> plugins = new List<object>();

		internal void Clear()
		{
			this.plugins.Clear();
			this.oldStyleSettings = XElement.Parse("<Plugins></Plugins>");
		}

		public T GetSettings<T>()
		where T : class, new()
		{
			T setting = this.plugins.OfType<T>().FirstOrDefault();
			if (setting == null)
			{
				setting = new T();
				this.plugins.Add(setting);
			}

			return setting;
		}
	}
}
