using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using Duality;
using Duality.IO;
using Duality.Components;
using Duality.Serialization;
using Duality.Resources;
using Duality.Drawing;
using Duality.Backend;
using Duality.Editor.Backend;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.PackageManagement;
using Duality.Editor.AssetManagement;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor
{
	public static class DualityEditorApp
	{
		public	const	string	EditorLogfilePath		= "logfile_editor.txt";
		public	const	string	EditorPrevLogfileName	= "logfile_editor_{0}.txt";
		public	const	string	EditorPrevLogfileDir	= "Temp";
		public	const	string	DesignTimeDataFile		= "DesignTimeData.dat";
		public	const	string	UserDataFile			= "EditorUserData.xml";
		private	const	string	UserDataDockSeparator	= "<!-- DockPanel Data -->";

		public	const	string	ActionContextMenu					= "ContextMenu";
		public	const	string	ActionContextOpenRes				= "OpenRes";
		public	const	string	ActionContextFirstSession			= "FirstSession";
		public	const	string	ActionContextSetupObjectForEditing	= "SetupObjectForEditing";

		private	static EditorPluginManager			pluginManager		= new EditorPluginManager();
		private	static MainForm						mainForm			= null;
		private	static IEditorGraphicsBackend		graphicsBack		= null;
		private	static INativeEditorGraphicsContext	mainGraphicsContext	= null;
		private	static List<IEditorAction>			editorActions		= new List<IEditorAction>();
		private	static ReloadCorePluginDialog		corePluginReloader	= null;
		private	static bool							needsRecovery		= false;
		private	static GameObjectManager			editorObjects		= new GameObjectManager();
		private	static HashSet<GameObject>			updateObjects		= new HashSet<GameObject>();
		private	static bool							dualityAppSuspended	= true;
		private	static List<Resource>				unsavedResources	= new List<Resource>();
		private	static ObjectSelection				selectionCurrent	= ObjectSelection.Null;
		private	static ObjectSelection				selectionPrevious	= ObjectSelection.Null;
		private	static ObjectSelection.Category		selectionActiveCat	= ObjectSelection.Category.None;
		private	static bool							selectionChanging	= false;
		private	static Dictionary<Guid,Type>		selectionTempScene	= null;	// GameObjCmp sel inbetween scene switches
		private	static bool							firstEditorSession	= false;
		private	static bool							backupsEnabled		= true;
		private	static AutosaveFrequency			autosaveFrequency	= AutosaveFrequency.ThirtyMinutes;
		private	static DateTime						autosaveLast		= DateTime.Now;
		private	static string						launcherApp			= null;
		private	static ContentRef<Scene>			lastOpenScene		= null;
		private	static bool							startWithLastScene	= true;
		private	static PackageManager				packageManager		= null;
		private	static EditorLogOutput			memoryLogOutput		= null;


		public	static	event	EventHandler	Terminating			= null;
		public	static	event	EventHandler	EventLoopIdling		= null;
		public	static	event	EventHandler	EditorIdling		= null;
		public	static	event	EventHandler	UpdatingEngine		= null;
		public	static	event	EventHandler	SaveAllTriggered	= null;
		public	static	event	EventHandler<HighlightObjectEventArgs>			HighlightObject			= null;
		public	static	event	EventHandler<SelectionChangedEventArgs>			SelectionChanged		= null;
		/// <summary>
		/// Fired whenever an objects property changes within the editor.
		/// Generally used as a means of synchronizing with changes to an
		/// objects data from other elements of the GUI. To fire this event
		/// from a custom plugin
		/// see <seealso cref="NotifyObjPropChanged"/>
		/// and <seealso cref="NotifyObjPrefabApplied"/>.
		/// </summary>
		public static	event	EventHandler<ObjectPropertyChangedEventArgs>	ObjectPropertyChanged	= null;
		
		
		public static EditorPluginManager PluginManager
		{
			get { return pluginManager; }
		}
		public static EditorLogOutput GlobalLogData
		{
			get { return memoryLogOutput; }
		}
		public static PackageManager PackageManager
		{
			get { return packageManager; }
		}
		public static MainForm MainForm
		{
			get { return mainForm; }
		}
		public static GameObjectManager EditorObjects
		{
			get { return editorObjects; }
		}
		public static ObjectSelection Selection
		{
			get { return selectionCurrent; }
		}
		public static ObjectSelection.Category SelectionActiveCategory
		{
			get { return selectionActiveCat; }
		}
		public static bool IsSelectionChanging
		{
			get { return selectionChanging; }
		}
		public static bool IsReloadingPlugins
		{
			get 
			{
				return 
					corePluginReloader.State == ReloadCorePluginDialog.ReloaderState.ReloadPlugins ||
					corePluginReloader.State == ReloadCorePluginDialog.ReloaderState.RecoverFromRestart;
			}
		}
		public static IEnumerable<Resource> UnsavedResources
		{
			get { return unsavedResources.Where(r => !r.Disposed && !r.IsDefaultContent && !r.IsRuntimeResource && (r != Scene.Current || !Sandbox.IsActive)); }
		}
		public static bool IsFirstEditorSession
		{
			get { return firstEditorSession; }
		}
		public static bool BackupsEnabled
		{
			get { return backupsEnabled; }
			set { backupsEnabled = value; }
		}
		public static AutosaveFrequency Autosaves
		{
			get { return autosaveFrequency; }
			set { autosaveFrequency = value; }
		}
		public static string LauncherAppPath
		{
			get
			{
				string launcherPath = string.IsNullOrWhiteSpace(launcherApp) ? EditorHelper.DualityLauncherExecFile : launcherApp;
				if (File.Exists(launcherPath)) return launcherPath;

				if (!Path.IsPathRooted(launcherPath))
				{
					string appDirLauncherApp = Path.Combine(PathHelper.ExecutingAssemblyDir, launcherPath);
					if (File.Exists(appDirLauncherApp)) return appDirLauncherApp;
				}

				return EditorHelper.DualityLauncherExecFile;
			}
			set
			{
				if (Path.GetFullPath(value) == Path.GetFullPath(EditorHelper.DualityLauncherExecFile)) value = null;
				if (value != launcherApp)
				{
					launcherApp = value;
					UpdatePluginSourceCode();
				}
			}
		}
		private static bool AppStillIdle
		{
			 get
			{
				NativeMethods.Message msg;
				return !NativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
			 }
		}
		
		
		public static void Init(MainForm mainForm, bool recover)
		{
			DualityEditorApp.needsRecovery = recover;
			DualityEditorApp.mainForm = mainForm;

			// Set up an in-memory data log so plugins can access the log history when needed
			memoryLogOutput = new EditorLogOutput();
			Logs.AddGlobalOutput(memoryLogOutput);

			// Create working directories, if not existing yet.
			if (!Directory.Exists(DualityApp.DataDirectory))
			{
				Directory.CreateDirectory(DualityApp.DataDirectory);
				using (FileStream s = File.OpenWrite(Path.Combine(DualityApp.DataDirectory, "WorkingFolderIcon.ico")))
				{
					Properties.GeneralResCache.IconWorkingFolder.Save(s);
				}
				using (StreamWriter w = new StreamWriter(Path.Combine(DualityApp.DataDirectory, "desktop.ini")))
				{
					w.WriteLine("[.ShellClassInfo]");
					w.WriteLine("ConfirmFileOp=0");
					w.WriteLine("NoSharing=0");
					w.WriteLine("IconFile=WorkingFolderIcon.ico");
					w.WriteLine("IconIndex=0");
					w.WriteLine("InfoTip=This is Dualitors working folder");
				}

				DirectoryInfo dirInfo = new DirectoryInfo(DualityApp.DataDirectory);
				dirInfo.Attributes |= FileAttributes.System;

				FileInfo fileInfoDesktop = new FileInfo(Path.Combine(DualityApp.DataDirectory, "desktop.ini"));
				fileInfoDesktop.Attributes |= FileAttributes.Hidden;

				FileInfo fileInfoIcon = new FileInfo(Path.Combine(DualityApp.DataDirectory, "WorkingFolderIcon.ico"));
				fileInfoIcon.Attributes |= FileAttributes.Hidden;
			}
			if (!Directory.Exists(DualityApp.PluginDirectory)) Directory.CreateDirectory(DualityApp.PluginDirectory);
			if (!Directory.Exists(EditorHelper.SourceDirectory)) Directory.CreateDirectory(EditorHelper.SourceDirectory);
			if (!Directory.Exists(EditorHelper.SourceMediaDirectory)) Directory.CreateDirectory(EditorHelper.SourceMediaDirectory);
			if (!Directory.Exists(EditorHelper.SourceCodeDirectory)) Directory.CreateDirectory(EditorHelper.SourceCodeDirectory);

			// Initialize Package Management system
			packageManager = new PackageManager();

			// Initialize Duality
			EditorHintImageAttribute.ImageResolvers += EditorHintImageResolver;
			DualityApp.PluginManager.PluginsReady += DualityApp_PluginsReady;
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Editor, 
				DualityApp.ExecutionContext.Editor, 
				new DefaultAssemblyLoader(), 
				null);

			// Initialize the plugin manager for the editor. We'll use the same loader as the core.
			pluginManager.Init(DualityApp.PluginManager.AssemblyLoader);
			
			// Need to load editor plugins before initializing the graphics context, so the backend is available
			pluginManager.LoadPlugins();

			// Need to initialize graphics context and default content before instantiating anything that could require any of them
			InitMainGraphicsContext();
			DualityApp.InitPostWindow();

			LoadUserData();
			pluginManager.InitPlugins();


			// Set up core plugin reloader
			corePluginReloader = new ReloadCorePluginDialog(mainForm);
			
			// Register events
			mainForm.Activated += mainForm_Activated;
			mainForm.Deactivate += mainForm_Deactivate;
			Scene.Leaving += Scene_Leaving;
			Scene.Entered += Scene_Entered;
			Application.Idle += Application_Idle;
			Resource.ResourceDisposing += Resource_ResourceDisposing;
			Resource.ResourceSaved += Resource_ResourceSaved;
			Resource.ResourceSaving += Resource_ResourceSaving;
			FileEventManager.PluginChanged += FileEventManager_PluginChanged;
			editorObjects.GameObjectsAdded += editorObjects_GameObjectsAdded;
			editorObjects.GameObjectsRemoved += editorObjects_GameObjectsRemoved;
			editorObjects.ComponentAdded += editorObjects_ComponentAdded;
			editorObjects.ComponentRemoving += editorObjects_ComponentRemoved;

			// Initialize secondary editor components
			DesignTimeObjectData.Init();
			AssetManager.Init();
			ConvertOperation.Init();
			PreviewProvider.Init();
			Sandbox.Init();
			HelpSystem.Init();
			FileEventManager.Init();
			UndoRedoManager.Init();

			// Initialize editor actions
			foreach (TypeInfo actionType in GetAvailDualityEditorTypes(typeof(IEditorAction)))
			{
				if (actionType.IsAbstract) continue;
				IEditorAction action = actionType.CreateInstanceOf() as IEditorAction;
				if (action != null) editorActions.Add(action);
			}
			editorActions.StableSort((a, b) => b.Priority.CompareTo(a.Priority));
			
			if (startWithLastScene && lastOpenScene.IsAvailable)
			{				
				Scene.SwitchTo(lastOpenScene, true);
			}
			else
			{
				// Enter a new, empty Scene, which will trigger the usual updates
				Scene.SwitchTo(null, true);

				// If there are no Scenes in the current project, init the first one with some default objects.
				if (!Directory.EnumerateFiles(DualityApp.DataDirectory, "*" + Resource.GetFileExtByType<Scene>(), SearchOption.AllDirectories).Any())
				{
					GameObject mainCam = new GameObject("MainCamera");
					mainCam.AddComponent<Transform>().Pos = new Vector3(0, 0, -DrawDevice.DefaultFocusDist);
					mainCam.AddComponent<VelocityTracker>();
					mainCam.AddComponent<Camera>();
					mainCam.AddComponent<SoundListener>();
					Scene.Current.AddObject(mainCam);
				}
			}

			// Allow the engine to run
			dualityAppSuspended = false;
		}
		public static bool Terminate(bool byUser)
		{
			bool cancel = false;

			// Display safety message boxes if the close operation is triggered by the user.
			if (byUser)
			{
				var unsavedResTemp = DualityEditorApp.UnsavedResources.ToArray();
				if (unsavedResTemp.Any())
				{
					string unsavedResText = unsavedResTemp.Take(5).ToString(r => r.GetType().GetTypeCSCodeName(true) + ":\t" + r.FullName, "\n");
					if (unsavedResTemp.Count() > 5) 
						unsavedResText += "\n" + string.Format(Properties.GeneralRes.Msg_ConfirmQuitUnsaved_Desc_More, unsavedResTemp.Count() - 5);
					DialogResult result = MessageBox.Show(
						string.Format(Properties.GeneralRes.Msg_ConfirmQuitUnsaved_Desc, "\n\n" + unsavedResText + "\n\n"), 
						Properties.GeneralRes.Msg_ConfirmQuitUnsaved_Caption, 
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
					if (result == DialogResult.Yes)
					{
						Sandbox.Stop();
						DualityEditorApp.SaveAllProjectData();
					}
					else if (result == DialogResult.Cancel)
						cancel = true;
				}
			}

			// Did we cancel it? Return false.
			if (cancel)
				return false;

			// Otherwise, actually start terminating.
			// From this point on, there's no return - need to re-init the editor afterwards.
			if (Terminating != null)
				Terminating(null, EventArgs.Empty);

			// Unregister events
			EditorHintImageAttribute.ImageResolvers -= EditorHintImageResolver;
			DualityApp.PluginManager.PluginsReady -= DualityApp_PluginsReady;
			mainForm.Activated -= mainForm_Activated;
			mainForm.Deactivate -= mainForm_Deactivate;
			Scene.Leaving -= Scene_Leaving;
			Scene.Entered -= Scene_Entered;
			Application.Idle -= Application_Idle;
			Resource.ResourceSaved -= Resource_ResourceSaved;
			Resource.ResourceSaving -= Resource_ResourceSaving;
			Resource.ResourceDisposing -= Resource_ResourceDisposing;
			FileEventManager.PluginChanged -= FileEventManager_PluginChanged;
			editorObjects.GameObjectsAdded -= editorObjects_GameObjectsAdded;
			editorObjects.GameObjectsRemoved -= editorObjects_GameObjectsRemoved;
			editorObjects.ComponentAdded -= editorObjects_ComponentAdded;
			editorObjects.ComponentRemoving -= editorObjects_ComponentRemoved;

			// Terminate editor actions
			editorActions.Clear();

			// Terminate secondary editor components
			UndoRedoManager.Terminate();
			FileEventManager.Terminate();
			HelpSystem.Terminate();
			Sandbox.Terminate();
			PreviewProvider.Terminate();
			ConvertOperation.Terminate();
			AssetManager.Terminate();
			DesignTimeObjectData.Terminate();

			// Shut down the editor backend
			DualityApp.ShutdownBackend(ref graphicsBack);

			// Shut down the plugin manager 
			pluginManager.Terminate();

			// Terminate Duality
			DualityApp.Terminate();

			// Remove the global in-memory log
			if (memoryLogOutput != null)
			{
				Logs.RemoveGlobalOutput(memoryLogOutput);
				memoryLogOutput = null; 
			}

			return true;
		}

		public static IEnumerable<Assembly> GetDualityEditorAssemblies()
		{
			return pluginManager.GetAssemblies();
		}
		public static IEnumerable<TypeInfo> GetAvailDualityEditorTypes(Type baseType)
		{
			return pluginManager.GetTypes(baseType);
		}
		/// <summary>
		/// Enumerates editor user actions that can be applied to objects of the specified type.
		/// A typical usage example for this are context menus that are populated dynamically
		/// based on the selected object and the available editor plugin capabilities.
		/// </summary>
		/// <param name="subjectType">The type ob the object the action operates on.</param>
		/// <param name="objects">
		/// The set of objects the action will be applied on, which is used to
		/// determine whether or not a given action can operate on the specific set of objects. If this is null, 
		/// no such check is performed and all editor actions that match the other criteria are returned.
		/// </param>
		/// <param name="context">The context in which this action is performed.</param>
		/// <returns></returns>
		public static IEnumerable<IEditorAction> GetEditorActions(Type subjectType, IEnumerable<object> objects, string context = ActionContextMenu)
		{
			if (objects != null)
			{
				return editorActions.Where(a => 
					a.SubjectType.IsAssignableFrom(subjectType) && 
					a.MatchesContext(context) && 
					a.CanPerformOn(objects));
			}
			else
			{
				return editorActions.Where(a => 
					a.SubjectType.IsAssignableFrom(subjectType) && 
					a.MatchesContext(context));
			}
		}

		public static void SaveUserData()
		{
			Logs.Editor.Write("Saving user data...");
			Logs.Editor.PushIndent();

			using (FileStream str = File.Create(UserDataFile))
			{
				Encoding encoding = Encoding.Default;
				using (StreamWriter writer = new StreamWriter(str.NonClosing()))
				{
					encoding = writer.Encoding;

					XDocument xmlDoc = new XDocument();
					XElement rootElement = new XElement("UserData");
					{
						XElement editorAppElement = new XElement("EditorApp");
						{
							editorAppElement.SetElementValue("Backups", backupsEnabled);
							editorAppElement.SetElementValue("Autosaves", autosaveFrequency);
							editorAppElement.SetElementValue("LauncherPath", launcherApp);
							editorAppElement.SetElementValue("FirstSession", false);
							editorAppElement.SetElementValue("ActiveDocumentIndex", mainForm.ActiveDocumentIndex);
							editorAppElement.SetElementValue("LastOpenScene", lastOpenScene.Path);
							editorAppElement.SetElementValue("StartWithLastScene", startWithLastScene);							
						}
						if (!editorAppElement.IsEmpty)
							rootElement.Add(editorAppElement);

						XElement pluginsElement = new XElement("Plugins");
						pluginManager.SaveUserData(pluginsElement);
						if (!pluginsElement.IsEmpty)
							rootElement.Add(pluginsElement);
					}
					xmlDoc.Add(rootElement);
					xmlDoc.Save(writer.BaseStream);

					writer.WriteLine();
					writer.WriteLine(UserDataDockSeparator);
					writer.Flush();
				}
				mainForm.MainDockPanel.SaveAsXml(str, encoding);
			}

			Logs.Editor.PopIndent();
		}
		private static void LoadUserData()
		{
			if (!File.Exists(UserDataFile))
			{
				File.WriteAllText(UserDataFile, Properties.GeneralRes.DefaultEditorUserData);
				if (!File.Exists(UserDataFile)) return;
			}

			Logs.Editor.Write("Loading user data...");
			Logs.Editor.PushIndent();

			Encoding encoding = Encoding.Default;
			StringBuilder editorData = new StringBuilder();
			StringBuilder dockPanelData = new StringBuilder();
			using (StreamReader reader = new StreamReader(UserDataFile))
			{
				encoding = reader.CurrentEncoding;
				string line;

				// Retrieve pre-DockPanel section
				while ((line = reader.ReadLine()) != null && line.Trim() != UserDataDockSeparator) 
					editorData.AppendLine(line);

				// Retrieve DockPanel section
				while ((line = reader.ReadLine()) != null) 
					dockPanelData.AppendLine(line);
			}

			// Load DockPanel Data
			{
				Logs.Editor.Write("Loading DockPanel data...");
				Logs.Editor.PushIndent();
				MemoryStream dockPanelDataStream = new MemoryStream(encoding.GetBytes(dockPanelData.ToString()));
				try
				{
					mainForm.MainDockPanel.LoadFromXml(dockPanelDataStream, DeserializeDockContent);
				}
				catch (Exception e)
				{
					Logs.Editor.WriteError("Cannot load DockPanel data due to malformed or non-existent Xml: {0}", LogFormat.Exception(e));
				}
				Logs.Editor.PopIndent();
			}

			// Load editor userdata
			{
				Logs.Editor.Write("Loading editor user data...");
				Logs.Editor.PushIndent();
				try
				{
					int activeDocumentIndex = 0;

					// Load main editor data
					XDocument xmlDoc = XDocument.Parse(editorData.ToString());
					XElement rootElement = xmlDoc.Root;
					XElement editorAppElement = rootElement.Elements("EditorApp").FirstOrDefault();
					if (editorAppElement != null)
					{
						editorAppElement.TryGetElementValue("Backups", ref backupsEnabled);
						editorAppElement.TryGetElementValue("Autosaves", ref autosaveFrequency);
						editorAppElement.TryGetElementValue("LauncherPath", ref launcherApp);
						editorAppElement.TryGetElementValue("FirstSession", ref firstEditorSession);
						editorAppElement.TryGetElementValue("ActiveDocumentIndex", ref activeDocumentIndex);

						string scenePath;
						editorAppElement.GetElementValue("LastOpenScene", out scenePath);
						if (scenePath != null) lastOpenScene = new ContentRef<Scene>(null, scenePath);

						editorAppElement.TryGetElementValue("StartWithLastScene", ref startWithLastScene);
					}

					// Load plugin editor data
					XElement pluginsElement = rootElement.Elements("Plugins").FirstOrDefault();
					if (pluginsElement != null)
						pluginManager.LoadUserData(pluginsElement);

					// Set the active document as loaded from user data
					mainForm.ActiveDocumentIndex = activeDocumentIndex;
				}
				catch (Exception e)
				{
					Logs.Editor.WriteError("Error loading editor user data: {0}", LogFormat.Exception(e));
				}
				Logs.Editor.PopIndent();
			}

			Logs.Editor.PopIndent();
			return;
		}
		private static IDockContent DeserializeDockContent(string persistName)
		{
			Logs.Editor.Write("Deserializing layout: '" + persistName + "'");
			return pluginManager.DeserializeDockContent(persistName);
		}

		private static void InitMainGraphicsContext()
		{
			if (mainGraphicsContext != null) return;

			if (graphicsBack == null)
				DualityApp.InitBackend(out graphicsBack, GetAvailDualityEditorTypes);

			Logs.Editor.Write("Creating editor graphics context...");
			Logs.Editor.PushIndent();
			try
			{
				// Currently bound to game-specific settings. Should be decoupled
				// from them at some point, so the editor can use independent settings.
				mainGraphicsContext = graphicsBack.CreateContext(
					DualityApp.AppData.MultisampleBackBuffer ?
					DualityApp.UserData.AntialiasingQuality :
					AAQuality.Off);
			}
			catch (Exception e)
			{
				mainGraphicsContext = null;
				Logs.Editor.WriteError("Can't create editor graphics context, because an error occurred: {0}", LogFormat.Exception(e));
			}
			Logs.Editor.PopIndent();
		}
		public static void PerformBufferSwap()
		{
			if (mainGraphicsContext == null) return;
			mainGraphicsContext.PerformBufferSwap();
		}
		public static INativeRenderableSite CreateRenderableSite()
		{
			if (mainGraphicsContext == null) return null;
			return mainGraphicsContext.CreateRenderableSite();
		}

		public static void UpdateGameObject(GameObject obj)
		{
			updateObjects.Add(obj);
		}

		/// <summary>
		/// Triggers a highlight event in the editor, to which the appropriate modules will
		/// be able to react. This usually means flashing a certain tree view entry or similar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="obj"></param>
		/// <param name="mode"></param>
		public static void Highlight(object sender, ObjectSelection obj, HighlightMode mode = HighlightMode.Conceptual)
		{
			OnHightlightObject(sender, obj, mode);
		}
		public static void Select(object sender, ObjectSelection sel, SelectMode mode = SelectMode.Set)
		{
			selectionPrevious = selectionCurrent;
			if (mode == SelectMode.Set)
				selectionCurrent = selectionCurrent.Transform(sel);
			else if (mode == SelectMode.Append)
				selectionCurrent = selectionCurrent.Append(sel);
			else if (mode == SelectMode.Toggle)
				selectionCurrent = selectionCurrent.Toggle(sel);
			OnSelectionChanged(sender, sel.Categories, SelectionChangeReason.Unknown);
		}
		public static void Deselect(object sender, ObjectSelection sel)
		{
			Deselect(sender, sel, SelectionChangeReason.Unknown);
		}
		public static void Deselect(object sender, ObjectSelection.Category category)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Clear(category);
			OnSelectionChanged(sender, ObjectSelection.Category.None, SelectionChangeReason.Unknown);
		}
		public static void Deselect(object sender, Predicate<object> predicate)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Clear(predicate);
			OnSelectionChanged(sender, ObjectSelection.Category.None, SelectionChangeReason.Unknown);
		}
		private static void Deselect(object sender, ObjectSelection sel, SelectionChangeReason reason)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Remove(sel);
			OnSelectionChanged(sender, ObjectSelection.Category.None, reason);
		}

		public static string SaveCurrentScene(bool skipYetUnsaved = true)
		{
			if (!Scene.Current.IsRuntimeResource)
			{
				if (IsResourceUnsaved(Scene.Current))
				{
					Scene.Current.Save();
					DualityApp.AppData.Version++;
				}
			}
			else if (!skipYetUnsaved)
			{
				string basePath = Path.Combine(DualityApp.DataDirectory, "Scene");
				string path = PathHelper.GetFreePath(basePath, Resource.GetFileExtByType<Scene>());
				Scene.Current.Save(path);
				DualityApp.AppData.Version++;
				
				// If there is no start scene defined, use this one.
				if (DualityApp.AppData.StartScene == null)
				{
					DualityApp.AppData.StartScene = Scene.Current;
					DualityApp.SaveAppData();
				}
			}
			return Scene.Current.Path;
		}
		public static void SaveResources()
		{
			bool anySaved = false;
			Resource[] resToSave = UnsavedResources.ToArray(); // The Property does some safety checks
			foreach (Resource res in resToSave)
			{
				res.Save();
				anySaved = true;
			}
			unsavedResources.Clear();
			if (anySaved) DualityApp.AppData.Version++;
		}
		public static void FlagResourceUnsaved(IEnumerable<Resource> res)
		{
			foreach (Resource r in res)
				FlagResourceUnsaved(r);
		}
		public static void FlagResourceUnsaved(Resource res)
		{
			if (unsavedResources.Contains(res)) return;
			unsavedResources.Add(res);
		}
		public static void FlagResourceSaved(IEnumerable<Resource> res)
		{
			foreach (Resource r in res)
				FlagResourceSaved(r);
		}
		public static void FlagResourceSaved(Resource res)
		{
			unsavedResources.Remove(res);
		}
		public static bool IsResourceUnsaved(Resource res)
		{
			return UnsavedResources.Contains(res);
		}
		public static bool IsResourceUnsaved(IContentRef res)
		{
			if (res.IsDefaultContent) return false;
			return res.ResWeak != null ? IsResourceUnsaved(res.ResWeak) : IsResourceUnsaved(res.Path);
		}
		public static bool IsResourceUnsaved(string resPath)
		{
			return UnsavedResources.Any(r => Path.GetFullPath(r.Path) == Path.GetFullPath(resPath));
		}
		public static void SaveAllProjectData()
		{
			if (!IsResourceUnsaved(Scene.Current) && !Sandbox.IsActive) SaveCurrentScene();
			SaveResources();

			if (SaveAllTriggered != null)
				SaveAllTriggered(null, EventArgs.Empty);

			autosaveLast = DateTime.Now;
		}

		public static void BackupResource(string path)
		{
			if (Resource.IsDefaultContentPath(path)) return;
			if (!File.Exists(path)) return;
			if (!PathOp.IsPathLocatedIn(path, DualityApp.DataDirectory)) return;

			// We don't want to screw anything up by trying to backup stuff, so just catch and log everything.
			try
			{
				string fileName = Path.GetFileName(path);
				string resourceName = Resource.GetNameFromPath(path);
				string pathCompleteExt = fileName.Remove(0, resourceName.Length);
				string fileBackupDir = Path.Combine(EditorHelper.BackupDirectory, PathHelper.MakeFilePathRelative(path, DualityApp.DataDirectory));
				string fileBackupName = DateTime.Now.ToString("yyyy-MM-dd T HH-mm", System.Globalization.CultureInfo.InvariantCulture) + pathCompleteExt;

				// Copy the file to the backup directory
				if (!Directory.Exists(fileBackupDir)) Directory.CreateDirectory(fileBackupDir);
				File.Copy(path, Path.Combine(fileBackupDir, fileBackupName), true);
			}
			catch (Exception e)
			{
				Logs.Editor.WriteError("Backup of file '{0}' failed: {1}", path, LogFormat.Exception(e));
			}
		}
		
		public static void UpdatePluginSourceCode()
		{
			string sourceCodeSolutionFile = EditorHelper.SourceCodeSolutionFilePath;
			// Initially generate source code, if not existing yet
			if (!File.Exists(sourceCodeSolutionFile))
			{
				InitPluginSourceCode();
				sourceCodeSolutionFile = EditorHelper.DefaultSourceCodeSolutionFile;
			}

			// Replace exec path in user files, since VS doesn't support relative paths there..
			{
				XDocument projectDoc;
				string coreProjectFile = EditorHelper.SourceCodeProjectCorePluginFile;
				string editorProjectFile = EditorHelper.SourceCodeProjectEditorPluginFile;
				string solutionDir = Path.GetFullPath(Path.GetDirectoryName(sourceCodeSolutionFile));

				// Adjust the core plugin startup parameters, if a core plugin exists under the expected name
				if (File.Exists(coreProjectFile))
				{
					bool anythingChanged = false;

					string startProgram = Path.GetFullPath(DualityEditorApp.LauncherAppPath);
					string startProgramRelative = PathHelper.MakeFilePathRelative(startProgram, solutionDir);
					if (startProgramRelative != null)
						startProgramRelative = "$(SolutionDir)" + startProgramRelative;
					else
						startProgramRelative = startProgram;

					string startWorkingDir = Path.GetFullPath(".");
					string startWorkingDirRelative = PathHelper.MakeDirectoryPathRelative(startWorkingDir, solutionDir);
					if (startWorkingDirRelative != null)
						startWorkingDirRelative = "$(SolutionDir)" + startWorkingDirRelative;
					else
						startWorkingDirRelative = startWorkingDir;

					projectDoc = XDocument.Load(coreProjectFile);
					foreach (XElement element in projectDoc.Descendants("StartProgram", true))
					{
						if (!string.Equals(element.Value, startProgramRelative))
						{
							element.Value = startProgramRelative;
							anythingChanged = true;
						}
					}
					foreach (XElement element in projectDoc.Descendants("StartWorkingDirectory", true))
					{
						if (!string.Equals(element.Value, startWorkingDirRelative))
						{
							element.Value = startWorkingDirRelative;
							anythingChanged = true;
						}
					}
					if (anythingChanged)
					{
						projectDoc.Save(coreProjectFile);
					}
				}
				
				// Adjust the editor plugin startup parameters, if an editor plugin exists under the expected name
				if (File.Exists(editorProjectFile))
				{
					bool anythingChanged = false;

					string startProgram = Path.GetFullPath("DualityEditor.exe");
					string startProgramRelative = PathHelper.MakeFilePathRelative(startProgram, solutionDir);
					if (startProgramRelative != null)
						startProgramRelative = "$(SolutionDir)" + startProgramRelative;
					else
						startProgramRelative = startProgram;

					string startWorkingDir = Path.GetFullPath(".");
					string startWorkingDirRelative = PathHelper.MakeDirectoryPathRelative(startWorkingDir, solutionDir);
					if (startWorkingDirRelative != null)
						startWorkingDirRelative = "$(SolutionDir)" + startWorkingDirRelative;
					else
						startWorkingDirRelative = startWorkingDir;

					projectDoc = XDocument.Load(editorProjectFile);
					foreach (XElement element in projectDoc.Descendants("StartProgram", true))
					{
						if (!string.Equals(element.Value, startProgramRelative))
						{
							element.Value = startProgramRelative;
							anythingChanged = true;
						}
					}
					foreach (XElement element in projectDoc.Descendants("StartWorkingDirectory", true))
					{
						if (!string.Equals(element.Value, startWorkingDirRelative))
						{
							element.Value = startWorkingDirRelative;
							anythingChanged = true;
						}
					}
					if (anythingChanged)
					{
						projectDoc.Save(editorProjectFile);
					}
				}
			}
		}
		public static void ReadPluginSourceCodeContentData(out string rootNamespace, out string desiredRootNamespace)
		{
			rootNamespace = null;
			desiredRootNamespace = EditorHelper.GenerateClassNameFromPath(EditorHelper.CurrentProjectName);

			// Read root namespaces
			if (File.Exists(EditorHelper.SourceCodeProjectCorePluginFile))
			{
				XDocument projXml = XDocument.Load(EditorHelper.SourceCodeProjectCorePluginFile);
				foreach (XElement element in projXml.Descendants("RootNamespace", true))
				{
					if (rootNamespace == null) rootNamespace = element.Value;
				}
			}
		}
		public static void InitPluginSourceCode()
		{
			// Check if the solution file has to be created		
			if (!File.Exists(EditorHelper.SourceCodeSolutionFilePath))
			{
				// Create solution file
				using (MemoryStream gamePluginStream = new MemoryStream(Properties.GeneralRes.GamePluginTemplate))
				using (ZipArchive gamePluginZip = new ZipArchive(gamePluginStream))
				{
					gamePluginZip.ExtractAll(EditorHelper.SourceCodeDirectory, false);
				}
			}

			string projectClassName = EditorHelper.GenerateClassNameFromPath(EditorHelper.CurrentProjectName);
			string newRootNamespaceCore = projectClassName;
			string newRootNamespaceEditor = newRootNamespaceCore + ".Editor";
			string pluginNameCore = projectClassName + "CorePlugin";
			string pluginNameEditor = projectClassName + "EditorPlugin";
			string oldRootNamespaceCore = null;
			string oldRootNamespaceEditor = null;

			// Update root namespaces
			if (File.Exists(EditorHelper.SourceCodeProjectCorePluginFile))
			{
				XDocument projXml = XDocument.Load(EditorHelper.SourceCodeProjectCorePluginFile);
				foreach (XElement element in projXml.Descendants("RootNamespace", true))
				{
					if (oldRootNamespaceCore == null) oldRootNamespaceCore = element.Value;
					element.Value = newRootNamespaceCore;
				}
				projXml.Save(EditorHelper.SourceCodeProjectCorePluginFile);
			}

			if (File.Exists(EditorHelper.SourceCodeProjectEditorPluginFile))
			{
				XDocument projXml = XDocument.Load(EditorHelper.SourceCodeProjectEditorPluginFile);
				foreach (XElement element in projXml.Descendants("RootNamespace", true))
				{
					if (oldRootNamespaceEditor == null) oldRootNamespaceEditor = element.Value;
					element.Value = newRootNamespaceEditor;
				}
				projXml.Save(EditorHelper.SourceCodeProjectEditorPluginFile);
			}

			// Guess old plugin class names
			string oldPluginNameCore = oldRootNamespaceCore + "CorePlugin";
			string oldPluginNameEditor = oldRootNamespaceCore + "EditorPlugin";
			string regExpr;
			string regExprReplace;

			// Replace namespace names: Core
			if (Directory.Exists(EditorHelper.SourceCodeProjectCorePluginDir))
			{
				regExpr = @"^(\s*namespace\s*)(.*)(" + oldRootNamespaceCore + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + newRootNamespaceCore + @"$4$5";
				foreach (string filePath in Directory.GetFiles(EditorHelper.SourceCodeProjectCorePluginDir, "*.cs", SearchOption.AllDirectories))
				{
					string fileContent = File.ReadAllText(filePath);
					fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);
					File.WriteAllText(filePath, fileContent, Encoding.UTF8);
				}
			}

			// Replace namespace names: Editor
			if (Directory.Exists(EditorHelper.SourceCodeProjectEditorPluginDir))
			{
				regExpr = @"^(\s*namespace\s*)(.*)(" + oldRootNamespaceEditor + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + newRootNamespaceEditor + @"$4$5";
				foreach (string filePath in Directory.GetFiles(EditorHelper.SourceCodeProjectEditorPluginDir, "*.cs", SearchOption.AllDirectories))
				{
					string fileContent = File.ReadAllText(filePath);
					fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);
					File.WriteAllText(filePath, fileContent, Encoding.UTF8);
				}
			}

			// Replace class names: Core
			if (File.Exists(EditorHelper.SourceCodeCorePluginFile))
			{
				string fileContent = File.ReadAllText(EditorHelper.SourceCodeCorePluginFile);

				// Replace class name
				regExpr = @"(\bclass\b)(.*)(" + oldPluginNameCore + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + pluginNameCore + @"$4$5";
				fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);

				regExpr = @"(\bclass\b)(.*)(" + @"__CorePluginClassName__" + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + pluginNameCore + @"$4$5";
				fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);

				File.WriteAllText(EditorHelper.SourceCodeCorePluginFile, fileContent, Encoding.UTF8);
			}

			// Replace class names: Editor
			if (File.Exists(EditorHelper.SourceCodeEditorPluginFile))
			{
				string fileContent = File.ReadAllText(EditorHelper.SourceCodeEditorPluginFile);

				// Replace class name
				regExpr = @"(\bclass\b)(.*)(" + oldPluginNameEditor + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + pluginNameEditor + @"$4$5";
				fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);

				regExpr = @"(\bclass\b)(.*)(" + @"__EditorPluginClassName__" + @")(.*)(\s*{)";
				regExprReplace = @"$1$2" + pluginNameEditor + @"$4$5";
				fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);

				// Repalce Id property
				regExpr = @"(\boverride\s*string\s*Id\s*{\s*get\s*{\s*return\s*" + '"' + @")(.*)(" + '"' + @"\s*;\s*}\s*})";
				regExprReplace = @"$1" + pluginNameEditor + @"$3";
				fileContent = Regex.Replace(fileContent, regExpr, regExprReplace, RegexOptions.Multiline);

				File.WriteAllText(EditorHelper.SourceCodeEditorPluginFile, fileContent, Encoding.UTF8);
			}
		}

		public static void NotifyObjPrefabApplied(object sender, ObjectSelection obj)
		{
			if (obj == null) return;
			if (obj.Empty) return;
			// For now, applying Prefabs will kill UndoRedo support since OnCopyTo is likely to detach objects
			// and thus invalidate old UndoRedoActions. This will only affect a small subset of operations, but
			// as for UndoRedo, it is better to fully support a small feature than poorly support a large feature.
			// The editor should always act reliable.
			UndoRedoManager.Clear();
			OnObjectPropertyChanged(sender, new PrefabAppliedEventArgs(obj));
		}
		/// <summary>
		/// Notify other elements of the editor that one or more objects have had properties changed.
		/// If the specified objects can be saved, they will be marked as unsaved.
		/// See <seealso cref="ObjectPropertyChanged"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="obj">The selection of obejcts that have been modified</param>
		/// <param name="info">The optional set of <see cref="PropertyInfo"/>s specifying which properties have changed on the objects</param>
		public static void NotifyObjPropChanged(object sender, ObjectSelection obj, params PropertyInfo[] info)
		{
			if (obj == null) return;
			if (obj.Empty) return;
			OnObjectPropertyChanged(sender, new ObjectPropertyChangedEventArgs(obj, info));
		}
		/// <summary>
		/// Notify other elements of the editor that one or more objects have had properties changed.
		/// See <seealso cref="ObjectPropertyChanged"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="obj">The selection of obejcts that have been modified</param>
		/// <param name="persistenceCritical">Determines whether or not the objects should be marked as unsaved</param>
		/// <param name="info">The optional set of <see cref="PropertyInfo"/>s specifying which properties have changed on the objects</param>
		public static void NotifyObjPropChanged(object sender, ObjectSelection obj, bool persistenceCritical, params PropertyInfo[] info)
		{
			if (obj == null) return;
			if (obj.Empty) return;
			OnObjectPropertyChanged(sender, new ObjectPropertyChangedEventArgs(obj, info, persistenceCritical));
		}

		public static T GetPlugin<T>() where T : EditorPlugin
		{
			return pluginManager.LoadedPlugins.OfType<T>().FirstOrDefault();
		}
		public static void AnalyzeCorePlugin(CorePlugin plugin)
		{
			Logs.Editor.Write("Analyzing Core Plugin: {0}", plugin.AssemblyName);
			Logs.Editor.PushIndent();

			// Query references to other Assemblies
			var asmRefQuery = from AssemblyName a in plugin.PluginAssembly.GetReferencedAssemblies()
							  select a.GetShortAssemblyName();
			string thisAsmName = typeof(DualityEditorApp).Assembly.GetShortAssemblyName();
			foreach (string asmName in asmRefQuery)
			{
				bool illegalRef = false;

				// Scan for illegally referenced Assemblies
				if (asmName == thisAsmName)
					illegalRef = true;
				else if (pluginManager.LoadedPlugins.Any(p => p.PluginAssembly.GetShortAssemblyName() == asmName))
					illegalRef = true;

				// Warn about them
				if (illegalRef)
				{
					Logs.Editor.WriteWarning(
						"Found illegally referenced Assembly '{0}'. " + 
						"CorePlugins should never reference or use DualityEditor or any of its EditorPlugins. Consider moving the critical code to an EditorPlugin.",
						asmName);
				}
			}
			
			// Try to retrieve all Types from the current Assembly
			Type[] exportedTypes;
			try
			{
				exportedTypes = plugin.PluginAssembly.GetExportedTypes();
			}
			catch (Exception e)
			{
				Logs.Editor.WriteError(
					"Unable to analyze exported types because an error occured: {0}",
					LogFormat.Exception(e));
				exportedTypes = null;
			}

			// Analyze exported types
			if (exportedTypes != null)
			{
				// Query Component types
				var cmpTypeQuery = from Type t in exportedTypes
								   where typeof(Component).IsAssignableFrom(t)
								   select t;
				foreach (var cmpType in cmpTypeQuery)
				{
					// Scan for public Fields
					FieldInfo[] fields = cmpType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
					if (fields.Length > 0)
					{
						Logs.Editor.WriteWarning(
							"Found public fields in Component class '{0}': {1}. " + 
							"The usage of public fields is strongly discouraged in Component classes. Consider using properties instead.",
							cmpType.GetTypeCSCodeName(true),
							fields.ToString(f => LogFormat.FieldInfo(f, false), ", "));
					}
				}
			}

			Logs.Editor.PopIndent();
		}

		public static bool DisplayConfirmDeleteObjects(ObjectSelection obj = null)
		{
			if (Sandbox.State == SandboxState.Playing) return true;
			DialogResult result = MessageBox.Show(
				Properties.GeneralRes.Msg_ConfirmDeleteSelectedObjects_Text, 
				Properties.GeneralRes.Msg_ConfirmDeleteSelectedObjects_Caption, 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);
			return result == DialogResult.Yes;
		}
		public static bool DisplayConfirmBreakPrefabLinkStructure(ObjectSelection obj = null)
		{
			if (obj == null) obj = DualityEditorApp.Selection;

			var linkQueryObj =
				from o in obj.GameObjects
				where (o.PrefabLink == null && o.AffectedByPrefabLink != null && o.AffectedByPrefabLink.AffectsObject(o)) || (o.PrefabLink != null && o.PrefabLink.ParentLink != null && o.PrefabLink.ParentLink.AffectsObject(o))
				select o.PrefabLink == null ? o.AffectedByPrefabLink : o.PrefabLink.ParentLink;
			var linkQueryCmp =
				from c in obj.Components
				where c.GameObj.AffectedByPrefabLink != null && c.GameObj.AffectedByPrefabLink.AffectsObject(c)
				select c.GameObj.AffectedByPrefabLink;
			var linkList = new List<PrefabLink>(linkQueryObj.Concat(linkQueryCmp).Distinct());
			if (linkList.Count == 0) return true;

			if (!DisplayConfirmBreakPrefabLink()) return false;
			UndoRedoManager.Do(new BreakPrefabLinkAction(linkList.Select(l => l.Obj)));
			return true;
		}
		public static bool DisplayConfirmBreakPrefabLink()
		{
			DialogResult result = MessageBox.Show(
				Properties.GeneralRes.Msg_ConfirmBreakPrefabLink_Desc, 
				Properties.GeneralRes.Msg_ConfirmBreakPrefabLink_Caption, 
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			return result == DialogResult.Yes;
		}
		
		private static void OnEventLoopIdling()
		{
			if (EventLoopIdling != null)
				EventLoopIdling(null, EventArgs.Empty);
		}
		private static void OnEditorIdling()
		{
			if (EditorIdling != null)
				EditorIdling(null, EventArgs.Empty);
		}
		private static void OnUpdatingEngine()
		{
			if (UpdatingEngine != null)
				UpdatingEngine(null, EventArgs.Empty);
		}
		private static void OnHightlightObject(object sender, ObjectSelection target, HighlightMode mode)
		{
			if (HighlightObject != null)
				HighlightObject(sender, new HighlightObjectEventArgs(target, mode));
		}
		private static void OnSelectionChanged(object sender, ObjectSelection.Category changedCategoryFallback, SelectionChangeReason changeReson)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;
			//if (selectionCurrent == selectionPrevious) return;
			selectionChanging = true;

			selectionActiveCat = changedCategoryFallback;
			if (SelectionChanged != null)
			{
				SelectionChanged(sender, new SelectionChangedEventArgs(
					selectionCurrent, 
					selectionPrevious, 
					changedCategoryFallback, 
					changeReson));
			}

			selectionChanging = false;
		}
		private static void OnObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs args)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			//Logs.Editor.Write("OnObjectPropertyChanged: {0}{2}\t{1}", args.PropNames.ToString(", "), args.Objects.Objects.ToString(", "), Environment.NewLine);
			if (args.PersistenceCritical)
			{
				// If a linked GameObject was modified, update its prefab link changelist
				if (!(args is PrefabAppliedEventArgs) && (args.Objects.GameObjects.Any() || args.Objects.Components.Any()))
				{
					HashSet<PrefabLink> changedLinks = new HashSet<PrefabLink>();
					foreach (object o in args.Objects)
					{
						Component cmp = o as Component;
						GameObject obj = o as GameObject;
						if (cmp == null && obj == null) continue;

						PrefabLink link = null;
						if (obj != null) link = obj.AffectedByPrefabLink;
						else if (cmp != null && cmp.GameObj != null) link = cmp.GameObj.AffectedByPrefabLink;

						if (link == null) continue;
						if (cmp != null && !link.AffectsObject(cmp)) continue;
						if (obj != null && !link.AffectsObject(obj)) continue;

						// Handle property changes regarding affected prefab links change lists
						foreach (PropertyInfo info in args.PropInfos)
						{
							if (PushPrefabLinkPropertyChange(link, o, info))
								changedLinks.Add(link);
						}
					}

					foreach (PrefabLink link in changedLinks)
					{
						NotifyObjPropChanged(null, new ObjectSelection(new[] { link.Obj }), ReflectionInfo.Property_GameObject_PrefabLink);
					}
				}

				// When modifying prefabs, apply changes to all linked objects
				if (args.Objects.Resources.OfType<Prefab>().Any())
				{
					foreach (Prefab prefab in args.Objects.Resources.OfType<Prefab>())
					{
						HashSet<PrefabLink> appliedLinks = PrefabLink.ApplyAllLinks(Scene.Current.AllObjects, p => p.Prefab == prefab);
						List<GameObject> changedObjects = new List<GameObject>(appliedLinks.Select(p => p.Obj));
						NotifyObjPrefabApplied(null, new ObjectSelection(changedObjects));
					}
				}

				// If a Resource's Properties are modified, mark Resource for saving
				if (args.Objects.ResourceCount > 0)
				{
					foreach (Resource res in args.Objects.Resources)
					{
						if (Sandbox.IsActive && res is Scene && (res as Scene).IsCurrent) continue;
						FlagResourceUnsaved(res);
					}
				}

				// If a GameObjects's Property is modified, notify changes to the current Scene
				if (args.Objects.GameObjects.Any(g => g.Scene == Scene.Current) ||
					args.Objects.Components.Any(c => c.GameObj.Scene == Scene.Current))
				{
					NotifyObjPropChanged(sender, new ObjectSelection(Scene.Current));
				}

				// If DualityAppData or DualityUserData is modified, save it
				if (args.Objects.OtherObjectCount > 0)
				{
					// This is probably not the best idea for generalized behaviour, but sufficient for now
					if (args.Objects.OtherObjects.Any(o => o is DualityAppData))
						DualityApp.SaveAppData();
					else if (args.Objects.OtherObjects.Any(o => o is DualityUserData))
						DualityApp.SaveUserData();
				}
			}

			// Fire the actual event
			if (ObjectPropertyChanged != null)
				ObjectPropertyChanged(sender, args);
		}
		private static bool PushPrefabLinkPropertyChange(PrefabLink link, object target, PropertyInfo info)
		{
			if (link == null) return false;

			if (info == ReflectionInfo.Property_GameObject_PrefabLink)
			{
				GameObject obj = target as GameObject;
				if (obj == null) return false;

				PrefabLink parentLink;
				if (obj.PrefabLink == link && (parentLink = link.ParentLink) != null)
				{
					parentLink.PushChange(obj, info, obj.PrefabLink.Clone());
					NotifyObjPropChanged(null, new ObjectSelection(new[] { parentLink.Obj }), info);
				}
				return false;
			}
			else
			{
				link.PushChange(target, info);
				return true;
			}
		}
		
		private static void Application_Idle(object sender, EventArgs e)
		{
			Application.Idle -= Application_Idle;
			
			// Trigger global event loop idle event.
			OnEventLoopIdling();

			// Perform some global operations, if no modal dialog is open
			if (mainForm.Visible && mainForm.CanFocus)
			{
				// Trigger global editor idle event.
				OnEditorIdling();

				// Trigger autosave after a while
				if (autosaveFrequency != AutosaveFrequency.Disabled)
				{
					TimeSpan timeSinceLastAutosave = DateTime.Now - autosaveLast;
					if ((autosaveFrequency == AutosaveFrequency.OneHour && timeSinceLastAutosave.TotalMinutes > 60) ||
						(autosaveFrequency == AutosaveFrequency.ThirtyMinutes && timeSinceLastAutosave.TotalMinutes > 30) ||
						(autosaveFrequency == AutosaveFrequency.TenMinutes && timeSinceLastAutosave.TotalMinutes > 10))
					{
						SaveAllProjectData();
						autosaveLast = DateTime.Now;
					}
				}
			}

			// Update Duality engine
			var watch = new System.Diagnostics.Stopwatch();
			while (AppStillIdle)
			{
				watch.Restart();
				if (!dualityAppSuspended)
				{
					bool fixedSingleStep = Sandbox.TakeSingleStep();
					try
					{
						DualityApp.EditorUpdate(
							editorObjects.ActiveObjects.Concat(updateObjects), 
							fixedSingleStep || (Sandbox.State == SandboxState.Playing && !Sandbox.IsFreezed), 
							fixedSingleStep);
						updateObjects.Clear();
					}
					catch (Exception exception)
					{
						Logs.Editor.WriteError("An error occurred during a core update: {0}", LogFormat.Exception(exception));
					}
					OnUpdatingEngine();
				}
				
				// Perform a buffer swap
				PerformBufferSwap();

				// Give the processor a rest if we have the time, don't use 100% CPU
				while (watch.Elapsed.TotalSeconds < 0.01d)
				{
					// Sleep a little
					System.Threading.Thread.Sleep(1);
					// App wants to do something? Stop waiting.
					if (!AppStillIdle) break;
				}
			}

			Application.Idle += Application_Idle;
		}
		private static void Scene_Leaving(object sender, EventArgs e)
		{
			if (!Scene.Current.IsEmpty)
			{
				if (selectionTempScene == null)
					selectionTempScene = new Dictionary<Guid,Type>();
				else
					selectionTempScene.Clear();

				foreach (GameObject obj in selectionCurrent.GameObjects)
					selectionTempScene.Add(obj.Id, null);
				foreach (Component cmp in selectionCurrent.Components.Where(c => c.GameObj != null))
					selectionTempScene.Add(cmp.GameObj.Id, cmp.GetType());
			}
			Deselect(null, ObjectSelection.Category.GameObjCmp);
		}
		private static void Scene_Entered(object sender, EventArgs e)
		{
			lastOpenScene = Scene.Current;
			if (selectionTempScene != null)
			{
				// Try to restore last GameObject / Component selection
				List<object> objList = new List<object>();
				foreach (var pair in selectionTempScene)
				{
					GameObject obj = Scene.Current.AllObjects.FirstOrDefault(sg => sg.Id == pair.Key);
					if (obj == null) continue;

					Component cmp = pair.Value != null ? obj.GetComponent(pair.Value) : null;

					if (cmp != null)
						objList.Add(cmp);
					else if (obj != null)
						objList.Add(obj);
				}

				// Append restored selection to current one.
				if (objList.Count > 0) Select(null, new ObjectSelection(objList), SelectMode.Append);				
			}
		}
		private static void Resource_ResourceSaved(object sender, ResourceSaveEventArgs e)
		{
			if (e.Path == null) return; // Ignore Resources without a path.
			if (e.IsDefaultContent) return; // Ignore default content
			if (e.SaveAsPath != e.Path) return; // Ignore "save as" actions
			FlagResourceSaved(e.Content.Res);
		}
		private static void Resource_ResourceSaving(object sender, ResourceSaveEventArgs e)
		{
			if (string.IsNullOrEmpty(e.SaveAsPath)) return; // Ignore unknown destinations
			if (backupsEnabled) BackupResource(e.SaveAsPath);
		}
		private static void Resource_ResourceDisposing(object sender, ResourceEventArgs e)
		{
			// Deselect disposed Resources
			if (selectionCurrent.Resources.Contains(e.Content.Res))
				Deselect(sender, new ObjectSelection(e.Content.Res), SelectionChangeReason.ObjectDisposing);
			// Unflag disposed Resources
			if (unsavedResources.Contains(e.Content.Res))
				FlagResourceSaved(e.Content.Res);
		}

		private static void mainForm_Activated(object sender, EventArgs e)
		{
			// Core plugin reload
			if (needsRecovery)
			{
				needsRecovery = false;
				Logs.Editor.Write("Recovering from full plugin reload restart...");
				Logs.Editor.PushIndent();
				corePluginReloader.State = ReloadCorePluginDialog.ReloaderState.RecoverFromRestart;
			}
			else if (corePluginReloader.ReloadSchedule.Count > 0)
			{
				corePluginReloader.State = ReloadCorePluginDialog.ReloaderState.ReloadPlugins;
			}
		}
		private static void mainForm_Deactivate(object sender, EventArgs e)
		{
			// Update source code, in case the user is switching to his IDE without hitting the "open source code" button again
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
				DualityEditorApp.UpdatePluginSourceCode();
		}

		private static void editorObjects_GameObjectsAdded(object sender, GameObjectGroupEventArgs e)
		{
			// Gather a list of components to activate
			int objCount = 0;
			List<ICmpInitializable> initList = new List<ICmpInitializable>();
			foreach (GameObject obj in e.Objects)
			{
				if (!obj.ActiveSingle) continue;
				obj.GatherInitComponents(initList, false);
				objCount++;
			}

			// If we collected components from more than one object, sort by exec order.
			// Otherwise, we can safely assume that the list is already sorted.
			if (objCount > 1) Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), false);

			// Invoke the init event on all gathered components in the right order
			foreach (ICmpInitializable component in initList)
				component.OnActivate();
		}
		private static void editorObjects_GameObjectsRemoved(object sender, GameObjectGroupEventArgs e)
		{
			// Gather a list of components to deactivate
			int objCount = 0;
			List<ICmpInitializable> initList = new List<ICmpInitializable>();
			foreach (GameObject obj in e.Objects)
			{
				if (!obj.ActiveSingle && !obj.Disposed) continue;
				obj.GatherInitComponents(initList, false);
				objCount++;
			}

			// If we collected components from more than one object, sort by exec order.
			// Otherwise, we can safely assume that the list is already sorted.
			if (objCount > 1)
				Component.ExecOrder.SortTypedItems(initList, item => item.GetType(), true);
			else
				initList.Reverse();

			// Invoke the init event on all gathered components in the right order
			foreach (ICmpInitializable component in initList)
				component.OnDeactivate();
		}
		private static void editorObjects_ComponentAdded(object sender, ComponentEventArgs e)
		{
			if (e.Component.Active)
			{
				ICmpInitializable cInit = e.Component as ICmpInitializable;
				if (cInit != null) cInit.OnActivate();
			}
		}
		private static void editorObjects_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component.Active)
			{
				ICmpInitializable cInit = e.Component as ICmpInitializable;
				if (cInit != null) cInit.OnDeactivate();
			}
		}

		private static void FileEventManager_PluginChanged(object sender, FileSystemEventArgs e)
		{
			if (!corePluginReloader.ReloadSchedule.Contains(e.FullPath))
			{
				corePluginReloader.ReloadSchedule.Add(e.FullPath);
				DualityApp.AppData.Version++;
			}
			corePluginReloader.State = ReloadCorePluginDialog.ReloaderState.WaitForPlugins;
		}
		private static void DualityApp_PluginsReady(object sender, DualityPluginEventArgs e)
		{
			foreach (CorePlugin plugin in e.Plugins)
			{
				AnalyzeCorePlugin(plugin);
			}
		}
		private static object EditorHintImageResolver(string manifestResourceName)
		{
			Assembly[] allAssemblies = DualityApp.GetDualityAssemblies().Concat(GetDualityEditorAssemblies()).Distinct().ToArray();
			foreach (Assembly assembly in allAssemblies)
			{
				string[] resourceNames = assembly.GetManifestResourceNames();
				if (resourceNames.Contains(manifestResourceName))
				{
					// Since images require to keep their origin stream open, we'll need to copy it to gain independence.
					using (Stream stream = assembly.GetManifestResourceStream(manifestResourceName))
					using (Bitmap bitmap = Bitmap.FromStream(stream) as Bitmap)
					{
						Bitmap independentBitmap = new Bitmap(bitmap.Width, bitmap.Height);
						independentBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
						using (Graphics graphics = Graphics.FromImage(independentBitmap))
						{
							graphics.DrawImageUnscaled(bitmap, 0, 0);
						}
						return independentBitmap;
					}
				}
			}
			return null;
		}
	}
}
