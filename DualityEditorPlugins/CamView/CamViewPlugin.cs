using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

using Duality;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.CamView.Properties;


namespace Duality.Editor.Plugins.CamView
{
	public class CamViewPlugin : EditorPlugin
	{
		private	List<CamView>			camViews		= new List<CamView>();
		private	bool					isLoading		= false;

		private	ToolStripMenuItem	menuItemCamView		= null;


		public override string Id
		{
			get { return "CamView"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(CamView))
				result = this.RequestCamView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(XElement node)
		{
			for (int i = 0; i < this.camViews.Count; i++)
			{
				XElement camViewElem = new XElement("CamView_" + i);
				node.Add(camViewElem);
				this.camViews[i].SaveUserData(camViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			for (int i = 0; i < this.camViews.Count; i++)
			{
				XElement camViewElem = node.Element("CamView_" + i);
				if (camViewElem == null) continue;
				this.camViews[i].LoadUserData(camViewElem);
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			this.menuItemCamView = main.RequestMenu(GeneralRes.MenuName_View, CamViewRes.MenuItemName_CamView);
			this.menuItemCamView.Image = CamViewResCache.IconEye.ToBitmap();
			this.menuItemCamView.Click += this.menuItemCamView_Click;

			Sandbox.Entering += this.Sandbox_Entering;
		}
		
		public CamView RequestCamView(string initStateTypeName = null)
		{
			CamView cam = new CamView(this.camViews.Count, initStateTypeName);
			this.camViews.Add(cam);
			cam.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.camViews.Remove(sender as CamView); };

			if (!this.isLoading)
			{
				cam.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (cam.Pane != null)
				{
					cam.Pane.Activate();
					if (cam.LocalGLControl != null)
						cam.LocalGLControl.Focus();
					else
						cam.Focus();
				}
			}
			return cam;
		}

		private void Sandbox_Entering(object sender, EventArgs e)
		{
			CamView gameView = null;
			if (this.camViews.Count == 0)
			{
				gameView = this.RequestCamView();
				gameView.SetCurrentState(typeof(CamViewStates.GameViewCamViewState));
			}
			else
			{
				gameView = this.camViews.FirstOrDefault(v => v.ActiveState.GetType() == typeof(CamViewStates.GameViewCamViewState));
				if (gameView != null && gameView.LocalGLControl != null) gameView.LocalGLControl.Focus();
			}
		}
		private void menuItemCamView_Click(object sender, EventArgs e)
		{
			this.RequestCamView();
		}
	}
}
