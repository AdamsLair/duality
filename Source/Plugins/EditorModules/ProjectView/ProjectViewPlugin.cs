using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.ProjectView.Properties;
using System.Xml.Linq;


namespace Duality.Editor.Plugins.ProjectView
{
	public class ProjectViewPlugin : EditorPlugin
	{
		private	ProjectFolderView	projectView		= null;
		private	bool				isLoading		= false;


		public override string Id
		{
			get { return "ProjectView"; }
		}

		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(ProjectFolderView))
				result = this.RequestProjectView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_View);
			viewItem.AddItem(new MenuModelItem
			{
				Name = ProjectViewRes.MenuItemName_ProjectView,
				Icon = ProjectViewResCache.IconProjectView.ToBitmap(),
				ActionHandler = this.menuItemProjectView_Click
			});
		}
		protected override void LoadUserData(PluginSettings settings)
		{
			ProjectViewSettings projectViewSettings = settings.Get<ProjectViewSettings>();
			this.projectView.LoadUserData(projectViewSettings);
		}
		protected override void SaveUserData(PluginSettings settings)
		{
			ProjectViewSettings projectViewSettings = settings.Get<ProjectViewSettings>();
			this.projectView.SaveUserData(projectViewSettings);
		}
		public ProjectFolderView RequestProjectView()
		{
			if (this.projectView == null || this.projectView.IsDisposed)
			{
				this.projectView = new ProjectFolderView();
				this.projectView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.projectView = null; };
			}

			if (!this.isLoading)
			{
				this.projectView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.projectView.Pane != null)
				{
					this.projectView.Pane.Activate();
					this.projectView.Focus();
				}
			}

			return this.projectView;
		}

		private void menuItemProjectView_Click(object sender, EventArgs e)
		{
			this.RequestProjectView();
		}
	}
}
