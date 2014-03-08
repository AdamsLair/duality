using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.SceneView.Properties;

using WeifenLuo.WinFormsUI.Docking;


namespace Duality.Editor.Plugins.SceneView
{
	public class SceneViewPlugin : EditorPlugin
	{
		private	SceneView	sceneView	= null;
		private	bool		isLoading	= false;

		private	ToolStripMenuItem	menuItemSceneView	= null;


		public override string Id
		{
			get { return "SceneView"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(SceneView))
				result = this.RequestSceneView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(XElement node)
		{
			if (this.sceneView != null)
			{
				XElement sceneViewElem = new XElement("SceneView_0");
				node.Add(sceneViewElem);
				this.sceneView.SaveUserData(sceneViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			if (this.sceneView != null)
			{
				XElement sceneViewElem = node.Element("SceneView_0");
				if (sceneViewElem != null)
				{
					this.sceneView.LoadUserData(sceneViewElem);
				}
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			this.menuItemSceneView = main.RequestMenu(GeneralRes.MenuName_View, SceneViewRes.MenuItemName_SceneView);
			this.menuItemSceneView.Image = SceneViewResCache.IconSceneView.ToBitmap();
			this.menuItemSceneView.Click += this.menuItemSceneView_Click;
		}
		
		public SceneView RequestSceneView()
		{
			if (this.sceneView == null || this.sceneView.IsDisposed)
			{
				this.sceneView = new SceneView();
				this.sceneView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.sceneView = null; };
			}

			if (!this.isLoading)
			{
				this.sceneView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.sceneView.Pane != null)
				{
					this.sceneView.Pane.Activate();
					this.sceneView.Focus();
				}
			}

			return this.sceneView;
		}

		private void menuItemSceneView_Click(object sender, EventArgs e)
		{
			this.RequestSceneView();
		}
	}
}
