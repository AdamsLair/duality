﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.Plugins.LogView.Properties;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;


namespace Duality.Editor.Plugins.LogView
{
	public class LogViewPlugin : EditorPlugin
	{
		private	LogView	logView		= null;
		private	bool	isLoading	= false;


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
		protected override void SaveUserData(XElement node)
		{
			if (this.logView != null)
			{
				XElement logViewElem = new XElement("LogView");
				this.logView.SaveUserData(logViewElem);
				if (!logViewElem.IsEmpty)
					node.Add(logViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			if (this.logView != null)
			{
				foreach (XElement logViewElem in node.Elements("LogView"))
				{
					int i = logViewElem.GetAttributeValue("id", 0);
					if (i < 0 || i >= 1) continue;

					this.logView.LoadUserData(logViewElem);
				}
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);
			
			// Request menu
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_View);
			viewItem.AddItem(new MenuModelItem
			{
				Name = LogViewRes.MenuItemName_LogView,
				Icon = LogViewResCache.IconLogView.ToBitmap(),
				ActionHandler = this.menuItemLogView_Click
			});
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
