using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

using Duality;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;
using Duality.Resources;
using TextRenderer = Duality.Components.Renderers.TextRenderer;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.EditorRes;
using DualityEditor.CorePluginInterface;
using DualityEditor.UndoRedoActions;

using EditorBase.PluginRes;


namespace EditorBase
{
	public class EditorBasePlugin : EditorPlugin
	{
		private	static	EditorBasePlugin	instance	= null;
		internal static EditorBasePlugin Instance
		{
			get { return instance; }
		}


		private	ProjectFolderView		projectView		= null;
		private	SceneView				sceneView		= null;
		private	LogView					logView			= null;
		private	List<ObjectInspector>	objViews		= new List<ObjectInspector>();
		private	List<CamView>			camViews		= new List<CamView>();
		private	bool					isLoading		= false;

		private	ToolStripMenuItem	menuItemProjectView	= null;
		private	ToolStripMenuItem	menuItemSceneView	= null;
		private	ToolStripMenuItem	menuItemObjView		= null;
		private	ToolStripMenuItem	menuItemCamView		= null;
		private	ToolStripMenuItem	menuItemLogView		= null;
		private	ToolStripMenuItem	menuItemAppData		= null;
		private	ToolStripMenuItem	menuItemUserData	= null;


		public override string Id
		{
			get { return "EditorBase"; }
		}
		public IEnumerable<ObjectInspector>	ObjViews
		{
			get { return this.objViews; }
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
			else if (dockContentType == typeof(ProjectFolderView))
				result = this.RequestProjectView();
			else if (dockContentType == typeof(SceneView))
				result = this.RequestSceneView();
			else if (dockContentType == typeof(ObjectInspector))
				result = this.RequestObjView();
			else if (dockContentType == typeof(LogView))
				result = this.RequestLogView();
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
			for (int i = 0; i < this.objViews.Count; i++)
			{
				System.Xml.XmlElement objViewElem = doc.CreateElement("ObjInspector_" + i);
				node.AppendChild(objViewElem);
				this.objViews[i].SaveUserData(objViewElem);
			}
			if (this.logView != null)
			{
				System.Xml.XmlElement logViewElem = doc.CreateElement("LogView_0");
				node.AppendChild(logViewElem);
				this.logView.SaveUserData(logViewElem);
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
			for (int i = 0; i < this.objViews.Count; i++)
			{
				System.Xml.XmlNodeList objViewElemQuery = node.GetElementsByTagName("ObjInspector_" + i);
				if (objViewElemQuery.Count == 0) continue;

				System.Xml.XmlElement objViewElem = objViewElemQuery[0] as System.Xml.XmlElement;
				this.objViews[i].LoadUserData(objViewElem);
			}
			if (this.logView != null)
			{
				System.Xml.XmlNodeList logViewElemQuery = node.GetElementsByTagName("LogView_0");
				if (logViewElemQuery.Count > 0)
				{
					System.Xml.XmlElement logViewElem = logViewElemQuery[0] as System.Xml.XmlElement;
					this.logView.LoadUserData(logViewElem);
				}
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

		protected override void LoadPlugin()
		{
			base.LoadPlugin();

			// Register core resource lookups
			CorePluginRegistry.RegisterTypeImage(typeof(DrawTechnique),			EditorBaseResCache.IconResDrawTechnique);
			CorePluginRegistry.RegisterTypeImage(typeof(FragmentShader),		EditorBaseResCache.IconResFragmentShader);
			CorePluginRegistry.RegisterTypeImage(typeof(Material),				EditorBaseResCache.IconResMaterial);
			CorePluginRegistry.RegisterTypeImage(typeof(Pixmap),				EditorBaseResCache.IconResPixmap);
			CorePluginRegistry.RegisterTypeImage(typeof(Prefab),				EditorBaseResCache.IconResPrefabFull, CorePluginRegistry.ImageContext_Icon + "_Full");
			CorePluginRegistry.RegisterTypeImage(typeof(Prefab),				EditorBaseResCache.IconResPrefabEmpty);
			CorePluginRegistry.RegisterTypeImage(typeof(RenderTarget),			EditorBaseResCache.IconResRenderTarget);
			CorePluginRegistry.RegisterTypeImage(typeof(ShaderProgram),			EditorBaseResCache.IconResShaderProgram);
			CorePluginRegistry.RegisterTypeImage(typeof(Texture),				EditorBaseResCache.IconResTexture);
			CorePluginRegistry.RegisterTypeImage(typeof(VertexShader),			EditorBaseResCache.IconResVertexShader);
			CorePluginRegistry.RegisterTypeImage(typeof(Scene),					EditorBaseResCache.IconResScene);
			CorePluginRegistry.RegisterTypeImage(typeof(AudioData),				EditorBaseResCache.IconResAudioData);
			CorePluginRegistry.RegisterTypeImage(typeof(Sound),					EditorBaseResCache.IconResSound);
			CorePluginRegistry.RegisterTypeImage(typeof(Font),					EditorBaseResCache.IconResFont);

			CorePluginRegistry.RegisterTypeImage(typeof(GameObject),			EditorBaseResCache.IconGameObj);
			CorePluginRegistry.RegisterTypeImage(typeof(Component),				EditorBaseResCache.IconCmpUnknown);
			CorePluginRegistry.RegisterTypeImage(typeof(SpriteRenderer),		EditorBaseResCache.IconCmpSpriteRenderer);
			CorePluginRegistry.RegisterTypeImage(typeof(AnimSpriteRenderer),	EditorBaseResCache.IconCmpSpriteRenderer);
			CorePluginRegistry.RegisterTypeImage(typeof(TextRenderer),			EditorBaseResCache.IconResFont);
			CorePluginRegistry.RegisterTypeImage(typeof(Transform),				EditorBaseResCache.IconCmpTransform);
			CorePluginRegistry.RegisterTypeImage(typeof(Camera),				EditorBaseResCache.IconCmpCamera);
			CorePluginRegistry.RegisterTypeImage(typeof(SoundEmitter),			EditorBaseResCache.IconResSound);
			CorePluginRegistry.RegisterTypeImage(typeof(SoundListener),			EditorBaseResCache.IconCmpSoundListener);
			CorePluginRegistry.RegisterTypeImage(typeof(RigidBody),				EditorBaseResCache.IconCmpRectCollider);
			CorePluginRegistry.RegisterTypeImage(typeof(ProfileRenderer),		EditorBaseResCache.IconCmpProfileRenderer);

			CorePluginRegistry.RegisterTypeCategory(typeof(Transform),			"");
			CorePluginRegistry.RegisterTypeCategory(typeof(SpriteRenderer),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(AnimSpriteRenderer), GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(TextRenderer),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(Camera),				GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(SoundEmitter),		GeneralRes.Category_Sound);
			CorePluginRegistry.RegisterTypeCategory(typeof(SoundListener),		GeneralRes.Category_Sound);
			CorePluginRegistry.RegisterTypeCategory(typeof(RigidBody),			GeneralRes.Category_Physics);
			CorePluginRegistry.RegisterTypeCategory(typeof(ProfileRenderer),	GeneralRes.Category_Diagnostics);

			CorePluginRegistry.RegisterTypeCategory(typeof(Scene),				"");
			CorePluginRegistry.RegisterTypeCategory(typeof(Prefab),				"");
			CorePluginRegistry.RegisterTypeCategory(typeof(Pixmap),				GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(Texture),			GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(Material),			GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(Font),				GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(RenderTarget),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(DrawTechnique),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(ShaderProgram),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(VertexShader),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(FragmentShader),		GeneralRes.Category_Graphics);
			CorePluginRegistry.RegisterTypeCategory(typeof(AudioData),			GeneralRes.Category_Sound);
			CorePluginRegistry.RegisterTypeCategory(typeof(Sound),				GeneralRes.Category_Sound);

			// Register conversion actions
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Pixmap>				(EditorBaseRes.ActionName_CreateTexture,		EditorBaseRes.IconResTexture,		this.ActionPixmapCreateTexture,		EditorBaseRes.ActionDesc_CreateTexture),		CorePluginRegistry.ActionContext_ContextMenu);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Texture>				(EditorBaseRes.ActionName_CreateMaterial,		EditorBaseRes.IconResMaterial,		this.ActionTextureCreateMaterial,	EditorBaseRes.ActionDesc_CreateMaterial),		CorePluginRegistry.ActionContext_ContextMenu);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<AudioData>				(EditorBaseRes.ActionName_CreateSound,			EditorBaseRes.IconResSound,			this.ActionAudioDataCreateSound,	EditorBaseRes.ActionDesc_CreateSound),			CorePluginRegistry.ActionContext_ContextMenu);
			CorePluginRegistry.RegisterEditorAction(new EditorGroupAction<AbstractShader>	(EditorBaseRes.ActionName_CreateShaderProgram,	EditorBaseRes.IconResShaderProgram, this.ActionShaderCreateProgram,		EditorBaseRes.ActionDesc_CreateShaderProgram),	CorePluginRegistry.ActionContext_ContextMenu);

			// Register open actions
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Pixmap>			(null, null, this.ActionPixmapOpenRes,			EditorBaseRes.ActionDesc_OpenResourceExternal), CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<AudioData>			(null, null, this.ActionAudioDataOpenRes,		EditorBaseRes.ActionDesc_OpenResourceExternal), CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<AbstractShader>	(null, null, this.ActionAbstractShaderOpenRes,	EditorBaseRes.ActionDesc_OpenResourceExternal), CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Prefab>			(null, null, this.ActionPrefabOpenRes,			EditorBaseRes.ActionDesc_InstantiatePrefab),	CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Scene>				(null, null, this.ActionSceneOpenRes,			EditorBaseRes.ActionDesc_OpenScene),			CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<GameObject>		(null, null, this.ActionGameObjectOpenRes,		EditorBaseRes.ActionDesc_FocusGameObject,		g => g.Transform != null),			CorePluginRegistry.ActionContext_OpenRes);
			CorePluginRegistry.RegisterEditorAction(new EditorAction<Component>			(null, null, this.ActionComponentOpenRes,		EditorBaseRes.ActionDesc_FocusGameObject,		c => c.GameObj.Transform != null),	CorePluginRegistry.ActionContext_OpenRes);

			// Register data converters
			CorePluginRegistry.RegisterDataConverter<GameObject>(new DataConverters.GameObjFromPrefab());
			CorePluginRegistry.RegisterDataConverter<GameObject>(new DataConverters.GameObjFromComponents());
			CorePluginRegistry.RegisterDataConverter<Component>(new DataConverters.ComponentFromSound());
			CorePluginRegistry.RegisterDataConverter<Component>(new DataConverters.ComponentFromMaterial());
			CorePluginRegistry.RegisterDataConverter<Component>(new DataConverters.ComponentFromFont());
			CorePluginRegistry.RegisterDataConverter<BatchInfo>(new DataConverters.BatchInfoFromMaterial());
			CorePluginRegistry.RegisterDataConverter<Material>(new DataConverters.MaterialFromBatchInfo());
			CorePluginRegistry.RegisterDataConverter<Material>(new DataConverters.MaterialFromTexture());
			CorePluginRegistry.RegisterDataConverter<Texture>(new DataConverters.TextureFromMaterial());
			CorePluginRegistry.RegisterDataConverter<Texture>(new DataConverters.TextureFromPixmap());
			CorePluginRegistry.RegisterDataConverter<Pixmap>(new DataConverters.PixmapFromTexture());
			CorePluginRegistry.RegisterDataConverter<Sound>(new DataConverters.SoundFromAudioData());
			CorePluginRegistry.RegisterDataConverter<AudioData>(new DataConverters.AudioDataFromSound());
			CorePluginRegistry.RegisterDataConverter<Prefab>(new DataConverters.PrefabFromGameObject());

			// Register preview generators
			CorePluginRegistry.RegisterPreviewGenerator(new PreviewGenerators.PixmapPreviewGenerator());
			CorePluginRegistry.RegisterPreviewGenerator(new PreviewGenerators.AudioDataPreviewGenerator());
			CorePluginRegistry.RegisterPreviewGenerator(new PreviewGenerators.SoundPreviewGenerator());
			CorePluginRegistry.RegisterPreviewGenerator(new PreviewGenerators.FontPreviewGenerator());

			// Register file importers
			CorePluginRegistry.RegisterFileImporter(new PixmapFileImporter());
			CorePluginRegistry.RegisterFileImporter(new AudioDataFileImporter());
			CorePluginRegistry.RegisterFileImporter(new ShaderFileImporter());
			CorePluginRegistry.RegisterFileImporter(new FontFileImporter());

			// Register PropertyEditor provider
			CorePluginRegistry.RegisterPropertyEditorProvider(new PropertyEditors.PropertyEditorProvider());
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			this.menuItemProjectView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_ProjectView);
			this.menuItemSceneView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_SceneView);
			this.menuItemObjView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_ObjView);
			this.menuItemCamView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_CamView);
			this.menuItemLogView = main.RequestMenu(GeneralRes.MenuName_View, EditorBaseRes.MenuItemName_LogView);
			this.menuItemAppData = main.RequestMenu(GeneralRes.MenuName_Settings, EditorBaseRes.MenuItemName_AppData);
			this.menuItemUserData = main.RequestMenu(GeneralRes.MenuName_Settings, EditorBaseRes.MenuItemName_UserData);

			// Configure menus
			this.menuItemProjectView.Image = EditorBaseResCache.IconProjectView.ToBitmap();
			this.menuItemSceneView.Image = EditorBaseResCache.IconSceneView.ToBitmap();
			this.menuItemObjView.Image = EditorBaseResCache.IconObjView.ToBitmap();
			this.menuItemCamView.Image = EditorBaseResCache.IconEye.ToBitmap();
			this.menuItemLogView.Image = EditorBaseResCache.IconLogView.ToBitmap();

			this.menuItemProjectView.Click += this.menuItemProjectView_Click;
			this.menuItemSceneView.Click += this.menuItemSceneView_Click;
			this.menuItemObjView.Click += this.menuItemObjView_Click;
			this.menuItemCamView.Click += this.menuItemCamView_Click;
			this.menuItemLogView.Click += this.menuItemLogView_Click;
			this.menuItemAppData.Click += this.menuItemAppData_Click;
			this.menuItemUserData.Click += this.menuItemUserData_Click;

			Sandbox.Entering += this.Sandbox_Entering;
			FileEventManager.ResourceModified += this.FileEventManager_ResourceChanged;
		//	FileEventManager.ResourceCreated += this.FileEventManager_ResourceChanged;
		//	FileEventManager.ResourceDeleted += this.FileEventManager_ResourceChanged;
		//	FileEventManager.ResourceRenamed += this.FileEventManager_ResourceChanged;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;
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
		public ObjectInspector RequestObjView(bool dontShow = false)
		{
			ObjectInspector objView = new ObjectInspector(this.objViews.Count);
			this.objViews.Add(objView);
			objView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.objViews.Remove(sender as ObjectInspector); };

			if (!this.isLoading && !dontShow)
			{
				objView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (objView.Pane != null)
				{
					objView.Pane.Activate();
					objView.Focus();
				}
			}
			return objView;
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

		private void menuItemProjectView_Click(object sender, EventArgs e)
		{
			this.RequestProjectView();
		}
		private void menuItemSceneView_Click(object sender, EventArgs e)
		{
			this.RequestSceneView();
		}
		private void menuItemObjView_Click(object sender, EventArgs e)
		{
			ObjectInspector objView = this.RequestObjView();
		}
		private void menuItemCamView_Click(object sender, EventArgs e)
		{
			this.RequestCamView();
		}
		private void menuItemLogView_Click(object sender, EventArgs e)
		{
			this.RequestLogView();
		}
		private void menuItemAppData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.AppData }));
		}
		private void menuItemUserData_Click(object sender, EventArgs e)
		{
			DualityEditorApp.Select(this, new ObjectSelection(new [] { DualityApp.UserData }));
		}

		private void ActionPixmapCreateTexture(Pixmap pixmap)
		{
			Texture.CreateFromPixmap(pixmap);
		}
		private void ActionTextureCreateMaterial(Texture tex)
		{
			Material.CreateFromTexture(tex);
		}
		private void ActionAudioDataCreateSound(AudioData audio)
		{
			Sound.CreateFromAudioData(audio);
		}
		private void ActionShaderCreateProgram(IEnumerable<AbstractShader> shaderEnum)
		{
			List<VertexShader> vertexShaders = shaderEnum.OfType<VertexShader>().ToList();
			List<FragmentShader> fragmentShaders = shaderEnum.OfType<FragmentShader>().ToList();

			if (vertexShaders.Count == 1 && fragmentShaders.Count >= 1)
				foreach (FragmentShader frag in fragmentShaders) this.ActionShaderCreateProgram_Create(frag, vertexShaders[0]);
			else if (fragmentShaders.Count == 1 && vertexShaders.Count >= 1)
				foreach (VertexShader vert in vertexShaders) this.ActionShaderCreateProgram_Create(fragmentShaders[0], vert);
			else
			{
				for (int i = 0; i < MathF.Max(vertexShaders.Count, fragmentShaders.Count); i++)
				{
					this.ActionShaderCreateProgram_Create(
						i < fragmentShaders.Count ? fragmentShaders[i] : null, 
						i < vertexShaders.Count ? vertexShaders[i] : null);
				}
			}
		}
		private void ActionShaderCreateProgram_Create(FragmentShader frag, VertexShader vert)
		{
			AbstractShader refShader = (vert != null) ? (AbstractShader)vert : (AbstractShader)frag;

			string nameTemp = refShader.Name;
			string dirTemp = Path.GetDirectoryName(refShader.Path);
			if (nameTemp.Contains("Shader"))
				nameTemp = nameTemp.Replace("Shader", "Program");
			else if (nameTemp.Contains("Shader"))
				nameTemp = nameTemp.Replace("shader", "program");
			else
				nameTemp += "Program";

			string programPath = PathHelper.GetFreePath(Path.Combine(dirTemp, nameTemp), ShaderProgram.FileExt);
			ShaderProgram program = new ShaderProgram(vert, frag);
			program.Save(programPath);
		}

		private void ActionPixmapOpenRes(Pixmap pixmap)
		{
			if (pixmap == null) return;
			FileImportProvider.OpenSourceFile(pixmap, ".png", pixmap.SavePixelData);
		}
		private void ActionAudioDataOpenRes(AudioData audio)
		{
			if (audio == null) return;
			FileImportProvider.OpenSourceFile(audio, ".ogg", audio.SaveOggVorbisData);
		}
		private void ActionAbstractShaderOpenRes(AbstractShader shader)
		{
			if (shader == null) return;
			FileImportProvider.OpenSourceFile(shader, shader is FragmentShader ? ".frag" : ".vert", shader.SaveSource);
		}
		private void ActionPrefabOpenRes(Prefab prefab)
		{
			try
			{
				CreateGameObjectAction undoRedoAction = new CreateGameObjectAction(null, prefab.Instantiate());
				UndoRedoManager.Do(undoRedoAction);
				DualityEditorApp.Select(this, new ObjectSelection(undoRedoAction.Result));
			}
			catch (Exception exception)
			{
				Log.Editor.WriteError("An error occurred instanciating Prefab {1}: {0}", 
					Log.Exception(exception),
					prefab != null ? prefab.Path : "null");
			}
		}
		private void ActionSceneOpenRes(Scene scene)
		{
			string lastPath = Scene.CurrentPath;
			try
			{
				Scene.Current = scene;
			}
			catch (Exception exception)
			{
				Log.Editor.WriteError("An error occurred while switching from Scene {1} to Scene {2}: {0}", 
					Log.Exception(exception),
					lastPath,
					scene != null ? scene.Path : "null");
			}
		}
		private void ActionGameObjectOpenRes(GameObject obj)
		{
			if (obj.Transform == null) return;
			foreach (CamView view in this.camViews)
				view.FocusOnObject(obj);
		}
		private void ActionComponentOpenRes(Component cmp)
		{
			GameObject obj = cmp.GameObj;
			if (obj == null) return;
			this.ActionGameObjectOpenRes(obj);
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
