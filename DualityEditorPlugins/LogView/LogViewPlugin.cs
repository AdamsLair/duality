using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.Plugins.LogView.Properties;

using WeifenLuo.WinFormsUI.Docking;


namespace Duality.Editor.Plugins.LogView
{
	public class LogViewPlugin : EditorPlugin
	{
		private	LogView	logView		= null;
		private	bool	isLoading	= false;

		private	ToolStripMenuItem	menuItemLogView		= null;


		public override string Id
		{
			get { return "LogView"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(LogView))
				result = this.RequestLogView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(XmlElement node)
		{
			XmlDocument doc = node.OwnerDocument;
			if (this.logView != null)
			{
				XmlElement logViewElem = doc.CreateElement("LogView_0");
				node.AppendChild(logViewElem);
				this.logView.SaveUserData(logViewElem);
			}
		}
		protected override void LoadUserData(XmlElement node)
		{
			this.isLoading = true;
			if (this.logView != null)
			{
				XmlNodeList logViewElemQuery = node.GetElementsByTagName("LogView_0");
				if (logViewElemQuery.Count > 0)
				{
					XmlElement logViewElem = logViewElemQuery[0] as System.Xml.XmlElement;
					this.logView.LoadUserData(logViewElem);
				}
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			this.menuItemLogView = main.RequestMenu(GeneralRes.MenuName_View, LogViewRes.MenuItemName_LogView);
			this.menuItemLogView.Image = LogViewResCache.IconLogView.ToBitmap();
			this.menuItemLogView.Click += this.menuItemLogView_Click;
		}
		
		public LogView RequestLogView()
		{
			if (this.logView == null || this.logView.IsDisposed)
			{
				this.logView = new LogView();
				this.logView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.logView = null; };
			}

			if (!this.isLoading)
			{
				this.logView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.logView.Pane != null)
				{
					this.logView.Pane.Activate();
					this.logView.Focus();
				}
			}

			return this.logView;
		}

		private void menuItemLogView_Click(object sender, EventArgs e)
		{
			this.RequestLogView();
		}
	}
}
