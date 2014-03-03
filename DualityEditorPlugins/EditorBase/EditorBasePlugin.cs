using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Diagnostics;
using Duality.Components.Physics;
using Duality.Resources;
using Duality.Properties;
using TextRenderer = Duality.Components.Renderers.TextRenderer;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Properties;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.Base.Properties;


namespace Duality.Editor.Plugins.Base
{
	public class EditorBasePlugin : EditorPlugin
	{
		private	static	EditorBasePlugin	instance	= null;
		internal static EditorBasePlugin Instance
		{
			get { return instance; }
		}


		private	SceneView				sceneView		= null;
		private	List<CamView>			camViews		= new List<CamView>();
		private	bool					isLoading		= false;

		private	ToolStripMenuItem	menuItemSceneView	= null;
		private	ToolStripMenuItem	menuItemCamView		= null;
		private	ToolStripMenuItem	menuItemAppData		= null;
		private	ToolStripMenuItem	menuItemUserData	= null;


		public override string Id
		{
			get { return "EditorBase"; }
		}
		public IEnumerable<CamView>	CamViews
		{
			get { return this.camViews; }
		}


		public EditorBasePlugin()
		{
			instance = this;
		}
		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(CamView))
				result = this.RequestCamView();
			else if (dockContentType == typeof(SceneView))
				result = this.RequestSceneView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(System.Xml.XmlElement node)
		{
			System.Xml.XmlDocument doc = node.OwnerDocument;
			for (int i = 0; i < this.camViews.Count; i++)
			{
				System.Xml.XmlElement camViewElem = doc.CreateElement("CamView_" + i);
				node.AppendChild(camViewElem);
				this.camViews[i].SaveUserData(camViewElem);
			}
			if (this.sceneView != null)
			{
				System.Xml.XmlElement sceneViewElem = doc.CreateElement("SceneView_0");
				node.AppendChild(sceneViewElem);
				this.sceneView.SaveUserData(sceneViewElem);
			}
		}
		protected override void LoadUserData(System.Xml.XmlElement node)
		{
			this.isLoading = true;
			for (int i = 0; i < this.camViews.Count; i++)
			{
				System.Xml.XmlNodeList camViewElemQuery = node.GetElementsByTagName("CamView_" + i);
				if (camViewElemQuery.Count == 0) continue;

				System.Xml.XmlElement camViewElem = camViewElemQuery[0] as System.Xml.XmlElement;
				this.camViews[i].LoadUserData(camViewElem);
			}
			if (this.sceneView != null)
			{
				System.Xml.XmlNodeList sceneViewElemQuery = node.GetElementsByTagName("SceneView_0");
				if (sceneViewElemQuery.Count > 0)
				{
					System.Xml.XmlElement sceneViewElem = sceneViewElemQuery[0] as System.Xml.XmlElement;
					this.sceneView.LoadUserData(sceneViewElem);
				}
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			this.menuItemSceneView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_SceneView);
			this.menuItemCamView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_CamView);
			this.menuItemAppData = main.RequestMenu(GeneralRes.MenuName_Settings, EditorBaseRes.MenuItemName_AppData);
			this.menuItemUserData = main.RequestMenu(GeneralRes.MenuName_Settings, EditorBaseRes.MenuItemName_UserData);

			// Configure menus
			this.menuItemSceneView.Image = EditorBaseResCache.IconSceneView.ToBitmap();
			this.menuItemCamView.Image = EditorBaseResCache.IconEye.ToBitmap();

			this.menuItemSceneView.Click += this.menuItemSceneView_Click;
			this.menuItemCamView.Click += this.menuItemCamView_Click;
			this.menuItemAppData.Click += this.menuItemAppData_Click;
			this.menuItemUserData.Click += this.menuItemUserData_Click;

			Sandbox.Entering += this.Sandbox_Entering;
			FileEventManager.ResourceModified += this.FileEventManager_ResourceChanged;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
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

		private void menuItemSceneView_Click(object sender, EventArgs e)
		{
			this.RequestSceneView();
		}
		private void menuItemCamView_Click(object sender, EventArgs e)
		{
			this.RequestCamView();
		}
		private void menuItemAppData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.AppData }));
		}
		private void menuItemUserData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.UserData }));
		}

		private void FileEventManager_ResourceChanged(object sender, ResourceEventArgs e)
		{
			if (e.IsResource) this.OnResourceModified(e.Content);
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.Objects.ResourceCount > 0)
			{
				foreach (var r in e.Objects.Resources)
					this.OnResourceModified(r);
			}
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
		private void OnResourceModified(ContentRef<Resource> resRef)
		{
			List<object> changedObj = null;

			// If a font has been modified, reload it and update all TextRenderers
			if (resRef.Is<Font>())
			{
				if (resRef.IsLoaded)
				{
					Font fnt = resRef.As<Font>().Res;
					if (fnt.NeedsReload)
						fnt.ReloadData();
				}

				foreach (Duality.Components.Renderers.TextRenderer r in Scene.Current.AllObjects.GetComponents<Duality.Components.Renderers.TextRenderer>())
				{
					r.Text.ApplySource();

					if (changedObj == null) changedObj = new List<object>();
					changedObj.Add(r);
				}
			}
			// If its a Pixmap, reload all associated Textures
			else if (resRef.Is<Pixmap>())
			{
				ContentRef<Pixmap> pixRef = resRef.As<Pixmap>();
				foreach (ContentRef<Texture> tex in ContentProvider.GetLoadedContent<Texture>())
				{
					if (!tex.IsAvailable) continue;
					if (tex.Res.BasePixmap == pixRef)
					{
						tex.Res.ReloadData();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(tex.Res);
					}
				}
			}
			// If its a Texture, update all associated RenderTargets
			else if (resRef.Is<Texture>())
			{
				if (resRef.IsLoaded)
				{
					Texture tex = resRef.As<Texture>().Res;
					if (tex.NeedsReload)
						tex.ReloadData();
				}

				ContentRef<Texture> texRef = resRef.As<Texture>();
				foreach (ContentRef<RenderTarget> rt in ContentProvider.GetLoadedContent<RenderTarget>())
				{
					if (!rt.IsAvailable) continue;
					if (rt.Res.Targets.Contains(texRef))
					{
						rt.Res.SetupOpenGLRes();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(rt.Res);
					}
				}
			}
			// If its some kind of shader, update all associated ShaderPrograms
			else if (resRef.Is<AbstractShader>())
			{
				ContentRef<FragmentShader> fragRef = resRef.As<FragmentShader>();
				ContentRef<VertexShader> vertRef = resRef.As<VertexShader>();
				foreach (ContentRef<ShaderProgram> sp in ContentProvider.GetLoadedContent<ShaderProgram>())
				{
					if (!sp.IsAvailable) continue;
					if (sp.Res.Fragment == fragRef ||
						sp.Res.Vertex == vertRef)
					{
						bool wasCompiled = sp.Res.Compiled;
						sp.Res.AttachShaders();
						if (wasCompiled) sp.Res.Compile();

						if (changedObj == null) changedObj = new List<object>();
						changedObj.Add(sp.Res);
					}
				}
			}

			// Notify a change that isn't critical regarding persistence (don't flag stuff unsaved)
			if (changedObj != null)
				DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(changedObj as IEnumerable<object>), false);
		}

		public static void InsertToolStripTypeItem(System.Collections.IList items, ToolStripItem newItem)
		{
			ToolStripItem item2 = newItem;
			ToolStripMenuItem menuItem2 = item2 as ToolStripMenuItem;
			for (int i = 0; i < items.Count; i++)
			{
				ToolStripItem item1 = items[i] as ToolStripItem;
				ToolStripMenuItem menuItem1 = item1 as ToolStripMenuItem;
				if (item1 == null)
					continue;

				bool item1IsType = item1.Tag is Type;
				bool item2IsType = item2.Tag is Type;
				System.Reflection.Assembly assembly1 = item1.Tag is Type ? (item1.Tag as Type).Assembly : item1.Tag as System.Reflection.Assembly;
				System.Reflection.Assembly assembly2 = item2.Tag is Type ? (item2.Tag as Type).Assembly : item2.Tag as System.Reflection.Assembly;
				int result = 
					(assembly2 == typeof(DualityApp).Assembly ? 1 : 0) - 
					(assembly1 == typeof(DualityApp).Assembly ? 1 : 0);
				if (result > 0)
				{
					items.Insert(i, newItem);
					return;
				}
				else if (result != 0) continue;

				result = 
					(item2IsType ? 1 : 0) - 
					(item1IsType ? 1 : 0);
				if (result > 0)
				{
					items.Insert(i, newItem);
					return;
				}
				else if (result != 0) continue;

				result = string.Compare(item1.Text, item2.Text);
				if (result > 0)
				{
					items.Insert(i, newItem);
					return;
				}
				else if (result != 0) continue;
			}

			items.Add(newItem);
		}
	}
}
