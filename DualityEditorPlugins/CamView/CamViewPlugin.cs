using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;

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


		public override string Id
		{
			get { return "CamView"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(CamView))
				result = this.CreateCamView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(XElement node)
		{
			for (int i = 0; i < this.camViews.Count; i++)
			{
				XElement camViewElem = new XElement("CamView");
				camViewElem.SetAttributeValue("id", i);
				node.Add(camViewElem);
				this.camViews[i].SaveUserData(camViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			foreach (XElement camViewElem in node.Elements("CamView"))
			{
				int i = camViewElem.GetAttributeValue("id", 0);
				if (i < 0 || i >= this.camViews.Count) continue;

				this.camViews[i].LoadUserData(camViewElem);
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
				Name = CamViewRes.MenuItemName_CamView,
				Icon = CamViewResCache.IconEye.ToBitmap(),
				ActionHandler = this.menuItemCamView_Click
			});

			Sandbox.Entering += this.Sandbox_Entering;
		}
		
		/// <summary>
		/// Switches an existing <see cref="CamView"/> to the specified <see cref="CamViewState"/> type id,
		/// or creates a new one, should none exist that is allowed to switch.
		/// </summary>
		/// <param name="camViewState"></param>
		/// <returns></returns>
		public CamView CreateOrSwitchCamView(string stateTypeId)
		{
			Type stateType = ReflectionHelper.ResolveType(stateTypeId) ?? typeof(CamViewStates.SceneEditorCamViewState);
			return CreateOrSwitchCamView(stateType);
		}
		/// <summary>
		/// Switches an existing <see cref="CamView"/> to the specified <see cref="CamViewState"/> type,
		/// or creates a new one, should none exist that is allowed to switch.
		/// </summary>
		/// <param name="camViewState"></param>
		/// <returns></returns>
		public CamView CreateOrSwitchCamView(Type camViewState)
		{
			// Search for existing cam views already using the requested state
			foreach (CamView view in this.camViews)
			{
				if (view.ActiveState == null) continue;
				if (!camViewState.IsInstanceOfType(view.ActiveState)) continue;
				return view;
			}

			// Switch one of the existing cam views to the desired state
			foreach (CamView view in this.camViews)
			{
				view.SetCurrentState(camViewState);
				return view;
			}

			// No cam view available? Create one
			return CreateCamView(camViewState.GetTypeId());
		}
		/// <summary>
		/// Creates a new <see cref="CamView"/> and initializes it with the specified <see cref="CamViewState"/>.
		/// </summary>
		/// <param name="initStateTypeId"></param>
		/// <returns></returns>
		public CamView CreateCamView(string initStateTypeId = null)
		{
			CamView cam = new CamView(this.camViews.Count, initStateTypeId);
			this.camViews.Add(cam);
			cam.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.camViews.Remove(sender as CamView); };

			if (!this.isLoading)
			{
				cam.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (cam.Pane != null)
				{
					cam.Pane.Activate();
					if (cam.RenderableControl != null)
						cam.RenderableControl.Focus();
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
				gameView = this.CreateCamView();
				gameView.SetCurrentState(typeof(CamViewStates.GameViewCamViewState));
			}
			else
			{
				gameView = this.camViews.FirstOrDefault(v => v.ActiveState.GetType() == typeof(CamViewStates.GameViewCamViewState));
				if (gameView != null && gameView.RenderableControl != null) gameView.RenderableControl.Focus();
			}
		}
		private void menuItemCamView_Click(object sender, EventArgs e)
		{
			this.CreateCamView();
		}
	}
}
