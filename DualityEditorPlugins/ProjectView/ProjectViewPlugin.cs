using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.ProjectView.Properties;


namespace Duality.Editor.Plugins.ProjectView
{
	public class ProjectViewPlugin : EditorPlugin
	{
		private	ProjectFolderView	projectView		= null;
		private	bool				isLoading		= false;

		private	ToolStripMenuItem	menuItemProjectView	= null;


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
			this.menuItemProjectView = main.RequestMenu(GeneralRes.MenuName_View, ProjectViewRes.MenuItemName_ProjectView);
			this.menuItemProjectView.Image = EditorBaseResCache.IconProjectView.ToBitmap();
			this.menuItemProjectView.Click += this.menuItemProjectView_Click;
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
