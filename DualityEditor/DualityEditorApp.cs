using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using Duality;
using Duality.Components;
using Duality.Serialization;
using Duality.Resources;
using Duality.Drawing;
using Duality.Backend;
using Duality.Editor.Backend;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.PackageManagement;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor
{
	public enum SelectMode
	{
		Set,
		Append,
		Toggle
	}

	[Flags]
	public enum HighlightMode
	{
		None		= 0x0,

		/// <summary>
		/// Highlights an objects conceptual representation, e.g. flashing its entry in an object overview.
		/// </summary>
		Conceptual	= 0x1,
		/// <summary>
		/// Highlights an objects spatial location, e.g. focusing it spatially in a scene view.
		/// </summary>
		Spatial		= 0x2,

		All			= Conceptual | Spatial
	}

	public enum AutosaveFrequency
	{
		Disabled,
		TenMinutes,
		ThirtyMinutes,
		OneHour
	}

	public static class DualityEditorApp
	{
		public	const	string	DesignTimeDataFile		= "DesignTimeData.dat";
		public	const	string	UserDataFile			= "EditorUserData.xml";
		private	const	string	UserDataDockSeparator	= "<!-- DockPanel Data -->";

		public	const	string	ActionContextMenu		= "ContextMenu";
		public	const	string	ActionContextOpenRes	= "OpenRes";
		
		private	static MainForm						mainForm			= null;
		private	static IEditorGraphicsBackend		graphicsBack		= null;
		private	static INativeEditorGraphicsContext			mainGraphicsContext	= null;
		private	static List<EditorPlugin>			plugins				= new List<EditorPlugin>();
		private	static Dictionary<Type,List<Type>>	availTypeDict		= new Dictionary<Type,List<Type>>();
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
		private	static bool							backupsEnabled		= true;
		private	static AutosaveFrequency			autosaveFrequency	= AutosaveFrequency.ThirtyMinutes;
		private	static DateTime						autosaveLast		= DateTime.Now;
		private	static string						launcherApp			= null;
		private	static InputEventMessageFilter		menuKeyInterceptor	= null;
		private	static PackageManager				packageManager		= null;


		public	static	event	EventHandler	Terminating			= null;
		public	static	event	EventHandler	EventLoopIdling		= null;
		public	static	event	EventHandler	EditorIdling		= null;
		public	static	event	EventHandler	UpdatingEngine		= null;
		public	static	event	EventHandler	SaveAllTriggered	= null;
		public	static	event	EventHandler<HighlightObjectEventArgs>			HighlightObject			= null;
		public	static	event	EventHandler<SelectionChangedEventArgs>			SelectionChanged		= null;
		public	static	event	EventHandler<ObjectPropertyChangedEventArgs>	ObjectPropertyChanged	= null;
		
		
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
		public static IEnumerable<EditorPlugin> Plugins
		{
			get { return plugins; }
		}
		public static IEnumerable<Resource> UnsavedResources
		{
			get { return unsavedResources.Where(r => !r.Disposed && !r.IsDefaultContent && !r.IsRuntimeResource && (r != Scene.Current || !Sandbox.IsActive)); }
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

			// Register Assembly Resolve hook for inter-Plugin dependency handling
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			// Initialize Package Management system
			packageManager = new PackageManager();

			// Initialize Duality
			EditorHintImageAttribute.ImageResolvers += EditorHintImageResolver;
			DualityApp.PluginReady += DualityApp_PluginReady;
			DualityApp.Init(DualityApp.ExecutionEnvironment.Editor, DualityApp.ExecutionContext.Editor, new[] {"logfile", "logfile_editor"});
			
			// Need to load editor plugins before initializing the graphics context, so the backend is available
			LoadPlugins();

			// Need to initialize graphics context and default content before instantiating anything that could require any of them
			InitMainGraphicsContext();
			DualityApp.InitPostWindow();

			LoadUserData();
			InitPlugins();


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
			editorObjects.GameObjectAdded += editorObjects_Registered;
			editorObjects.GameObjectRemoved += editorObjects_Unregistered;
			editorObjects.ComponentAdded += editorObjects_ComponentAdded;
			editorObjects.ComponentRemoving += editorObjects_ComponentRemoved;

			// Initialize secondary editor components
			DesignTimeObjectData.Init();
			FileImportProvider.Init();
			ConvertOperation.Init();
			PreviewProvider.Init();
			Sandbox.Init();
			HelpSystem.Init();
			FileEventManager.Init();
			UndoRedoManager.Init();

			// Initialize editor actions
			foreach (Type actionType in GetAvailDualityEditorTypes(typeof(IEditorAction)))
			{
				if (actionType.IsAbstract) continue;
				IEditorAction action = actionType.CreateInstanceOf() as IEditorAction;
				if (action != null) editorActions.Add(action);
			}

			// Install a global message filter to intercept the menu key
			if (menuKeyInterceptor == null)
			{
				menuKeyInterceptor = new InputEventMessageFilter();
				menuKeyInterceptor.SystemKeyDown += menuKeyInterceptor_SystemKeyDown;
			}
			Application.AddMessageFilter(menuKeyInterceptor);

			// If there are no Scenes in the current project, init the first one with some default objects.
			if (!Directory.EnumerateFiles(DualityApp.DataDirectory, "*" + Scene.FileExt, SearchOption.AllDirectories).Any())
			{
				GameObject mainCam = new GameObject("MainCamera");
				mainCam.AddComponent<Transform>().Pos = new Vector3(0, 0, -DrawDevice.DefaultFocusDist);
				mainCam.AddComponent<Camera>();
				mainCam.AddComponent<SoundListener>();
				Scene.Current.AddObject(mainCam);
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

			// Not cancelling? Then actually start terminating.
			if (!cancel)
			{
				if (Terminating != null) Terminating(null, EventArgs.Empty);
				
				// Unregister message hook
				if (menuKeyInterceptor != null)
				{
					Application.RemoveMessageFilter(menuKeyInterceptor);
				}

				// Save UserData
				DualityEditorApp.SaveUserData();
				DualityApp.SaveAppData();

				// Unregister events
				EditorHintImageAttribute.ImageResolvers -= EditorHintImageResolver;
				DualityApp.PluginReady -= DualityApp_PluginReady;
				mainForm.Activated -= mainForm_Activated;
				mainForm.Deactivate -= mainForm_Deactivate;
				Scene.Leaving -= Scene_Leaving;
				Scene.Entered -= Scene_Entered;
				Application.Idle -= Application_Idle;
				Resource.ResourceSaved -= Resource_ResourceSaved;
				Resource.ResourceSaving -= Resource_ResourceSaving;
				FileEventManager.PluginChanged -= FileEventManager_PluginChanged;

				// Terminate editor actions
				editorActions.Clear();

				// Terminate secondary editor components
				UndoRedoManager.Terminate();
				FileEventManager.Terminate();
				HelpSystem.Terminate();
				Sandbox.Terminate();
				PreviewProvider.Terminate();
				ConvertOperation.Terminate();
				FileImportProvider.Terminate();
				DesignTimeObjectData.Terminate();

				// Shut down the editor backend
				DualityApp.ShutdownBackend(ref graphicsBack);

				// Terminate Duality
				DualityApp.Terminate();
			}

			return !cancel;
		}

		private static void LoadPlugins()
		{
			Log.Editor.Write("Scanning for editor plugins...");
			Log.Editor.PushIndent();

			foreach (string dllPath in DualityApp.GetPluginLibPaths("*.editor.dll"))
			{
				Log.Editor.Write("Loading '{0}'...", dllPath);
				Log.Editor.PushIndent();
				try
				{
					Assembly pluginAssembly = Assembly.Load(File.ReadAllBytes(dllPath));
					Type pluginType = pluginAssembly.GetExportedTypes().FirstOrDefault(t => typeof(EditorPlugin).IsAssignableFrom(t));
					if (pluginType == null)
					{
						Log.Editor.WriteWarning("Can't find EditorPlugin class. Discarding plugin...");
						continue;
					}
					EditorPlugin plugin = (EditorPlugin)pluginType.CreateInstanceOf();
					plugins.Add(plugin);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("Error loading plugin: {0}", Log.Exception(e));
				}
				Log.Editor.PopIndent();
			}

			Log.Editor.PopIndent();
		}
		private static void InitPlugins()
		{
			Log.Editor.Write("Initializing editor plugins...");
			Log.Editor.PushIndent();
			foreach (EditorPlugin plugin in plugins.ToArray())
			{
				Log.Editor.Write("{0}...", plugin.Id);
				Log.Editor.PushIndent();
				try
				{
					plugin.InitPlugin(mainForm);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("Error initializing plugin: {0}", Log.Exception(e));
					plugins.Remove(plugin);
				}
				Log.Editor.PopIndent();
			}
			Log.Editor.PopIndent();
		}
		
		public static IEnumerable<Assembly> GetDualityEditorAssemblies()
		{
			yield return typeof(MainForm).Assembly;
			foreach (Assembly a in plugins.Select(ep => ep.GetType().Assembly)) yield return a;
		}
		public static IEnumerable<Type> GetAvailDualityEditorTypes(Type baseType)
		{
			List<Type> availTypes;
			if (availTypeDict.TryGetValue(baseType, out availTypes)) return availTypes;

			availTypes = new List<Type>();
			IEnumerable<Assembly> asmQuery = GetDualityEditorAssemblies();
			foreach (Assembly asm in asmQuery)
			{
				// Try to retrieve all Types from the current Assembly
				Type[] types;
				try { types = asm.GetExportedTypes(); }
				catch (Exception) { continue; }

				// Add the matching subset of these types to the result
				availTypes.AddRange(
					from t in types
					where baseType.IsAssignableFrom(t)
					orderby t.Name
					select t);
			}
			availTypeDict[baseType] = availTypes;

			return availTypes;
		}
		public static IEnumerable<IEditorAction> GetEditorActions(Type subjectType, IEnumerable<object> objects, string context = ActionContextMenu)
		{
			return editorActions.Where(a => 
				a.SubjectType.IsAssignableFrom(subjectType) && 
				a.MatchesContext(context) && 
				a.CanPerformOn(objects));
		}

		private static void SaveUserData()
		{
			Log.Editor.Write("Saving user data...");
			Log.Editor.PushIndent();

			using (FileStream str = File.Create(UserDataFile))
			{
				StreamWriter writer = new StreamWriter(str);
				// --- Save custom user data here ---
				XDocument xmlDoc = new XDocument();
				XElement rootElement = new XElement("UserData");
				xmlDoc.Add(rootElement);
				XElement editorAppElement = new XElement("EditorApp");
				rootElement.Add(editorAppElement);
				editorAppElement.SetAttributeValue("backups", backupsEnabled.ToString(System.Globalization.CultureInfo.InvariantCulture));
				editorAppElement.SetAttributeValue("autosaves", autosaveFrequency.ToString());
				editorAppElement.SetAttributeValue("launcher", launcherApp);
				foreach (EditorPlugin plugin in plugins)
				{
					XElement pluginXmlElement = new XElement("Plugin_" + plugin.Id);
					rootElement.Add(pluginXmlElement);
					plugin.SaveUserData(pluginXmlElement);
				}
				xmlDoc.Save(writer.BaseStream);
				// ----------------------------------
				writer.WriteLine();
				writer.WriteLine(UserDataDockSeparator);
				writer.Flush();
				mainForm.MainDockPanel.SaveAsXml(str, writer.Encoding);
			}

			Log.Editor.PopIndent();
		}
		private static void LoadUserData()
		{
			if (!File.Exists(UserDataFile))
			{
				File.WriteAllText(UserDataFile, Properties.GeneralRes.DefaultEditorUserData);
				if (!File.Exists(UserDataFile)) return;
			}

			Log.Editor.Write("Loading user data...");
			Log.Editor.PushIndent();

			using (StreamReader reader = new StreamReader(UserDataFile))
			{
				string line;
				// Retrieve pre-DockPanel section
				StringBuilder editorData = new StringBuilder();
				while ((line = reader.ReadLine()) != null && line.Trim() != UserDataDockSeparator) 
					editorData.AppendLine(line);
				// Retrieve DockPanel section
				StringBuilder dockPanelData = new StringBuilder();
				while ((line = reader.ReadLine()) != null) 
					dockPanelData.AppendLine(line);

				// Load DockPanel Data
				Log.Editor.Write("Loading DockPanel data...");
				Log.Editor.PushIndent();
				MemoryStream dockPanelDataStream = new MemoryStream(reader.CurrentEncoding.GetBytes(dockPanelData.ToString()));
				try
				{
					mainForm.MainDockPanel.LoadFromXml(dockPanelDataStream, DeserializeDockContent);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("Cannot load DockPanel data due to malformed or non-existent Xml: {0}", Log.Exception(e));
				}
				Log.Editor.PopIndent();

				// --- Read custom user data from StringBuilder here ---
				Log.Editor.Write("Loading editor user data...");
				Log.Editor.PushIndent();
				try
				{
					XDocument xmlDoc = XDocument.Parse(editorData.ToString());
					IEnumerable<XElement> editorAppElemQuery = xmlDoc.Descendants("EditorApp");
					if (editorAppElemQuery.Any())
					{
						XElement editorAppElement = editorAppElemQuery.First();
						bool.TryParse(editorAppElement.GetAttributeValue("backups"), out backupsEnabled);
						Enum.TryParse<AutosaveFrequency>(editorAppElement.GetAttributeValue("autosaves"), out autosaveFrequency);
						launcherApp = editorAppElement.GetAttributeValue("launcher");
					}
					foreach (XElement child in xmlDoc.Descendants())
					{
						if (child.Name.LocalName.StartsWith("Plugin_"))
						{
							string pluginName = child.Name.LocalName.Substring(7, child.Name.LocalName.Length - 7);
							foreach (EditorPlugin plugin in plugins)
							{
								if (plugin.Id == pluginName)
								{
									plugin.LoadUserData(child);
									break;
								}
							}
						}
					}
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("Cannot load plugin user data due to malformed or non-existent Xml: {0}", Log.Exception(e));
				}
				Log.Editor.PopIndent();
				// -----------------------------------------------------
			}

			Log.Editor.PopIndent();
		}
		private static IDockContent DeserializeDockContent(string persistName)
		{
			Log.Editor.Write("Deserializing layout: '" + persistName + "'");

			Type dockContentType = null;
			Assembly dockContentAssembly = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblies)
			{
				if ((dockContentType = a.GetType(persistName)) != null)
				{
					dockContentAssembly = a;
					break;
				}
			}
			
			if (dockContentType == null) 
				return null;
			else
			{
				// First ask plugins from the dock contents assembly for existing instances
				IDockContent deserializeDockContent = null;
				foreach (EditorPlugin plugin in plugins)
				{
					if (plugin.GetType().Assembly == dockContentAssembly)
					{
						deserializeDockContent = plugin.DeserializeDockContent(dockContentType);
						if (deserializeDockContent != null) break;
					}
				}

				// If none exists, create one
				return deserializeDockContent ?? (dockContentType.CreateInstanceOf() as IDockContent);
			}
		}

		private static void InitMainGraphicsContext()
		{
			if (mainGraphicsContext != null) return;

			if (graphicsBack == null)
				DualityApp.InitBackend(out graphicsBack, GetAvailDualityEditorTypes);

			try
			{
				mainGraphicsContext = graphicsBack.CreateContext();
			}
			catch (Exception e)
			{
				mainGraphicsContext = null;
				Log.Editor.WriteError("Can't create editor graphics context, because an error occurred: {0}", Log.Exception(e));
			}
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
			OnSelectionChanged(sender, sel.Categories);
		}
		public static void Deselect(object sender, ObjectSelection sel)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Remove(sel);
			OnSelectionChanged(sender, ObjectSelection.Category.None);
		}
		public static void Deselect(object sender, ObjectSelection.Category category)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Clear(category);
			OnSelectionChanged(sender, ObjectSelection.Category.None);
		}
		public static void Deselect(object sender, Predicate<object> predicate)
		{
			selectionPrevious = selectionCurrent;
			selectionCurrent = selectionCurrent.Clear(predicate);
			OnSelectionChanged(sender, ObjectSelection.Category.None);
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
				string path = PathHelper.GetFreePath(basePath, Scene.FileExt);
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
			if (ContentProvider.IsDefaultContentPath(path)) return;
			if (!File.Exists(path)) return;
			if (!PathHelper.IsPathLocatedIn(path, DualityApp.DataDirectory)) return;

			// We don't want to screw anything up by trying to backup stuff, so just catch and log everything.
			try
			{
				string fileName = Path.GetFileName(path);
				string resourceName = ContentProvider.GetNameFromPath(path);
				string pathCompleteExt = fileName.Remove(0, resourceName.Length);
				string fileBackupDir = Path.Combine(EditorHelper.BackupDirectory, PathHelper.MakeFilePathRelative(path, DualityApp.DataDirectory));
				string fileBackupName = DateTime.Now.ToString("yyyy-MM-dd T HH-mm", System.Globalization.CultureInfo.InvariantCulture) + pathCompleteExt;

				// Copy the file to the backup directory
				if (!Directory.Exists(fileBackupDir)) Directory.CreateDirectory(fileBackupDir);
				File.Copy(path, Path.Combine(fileBackupDir, fileBackupName), true);
			}
			catch (Exception e)
			{
				Log.Editor.WriteError("Backup of file '{0}' failed: {1}", path, Log.Exception(e));
			}
		}
		
		public static void UpdatePluginSourceCode()
		{
			// Initially generate source code, if not existing yet
			if (!File.Exists(EditorHelper.SourceCodeSolutionFile)) InitPluginSourceCode();
			
			// Replace exec path in user files, since VS doesn't support relative paths there..
			{
				XDocument userDoc;
				const string userFileCore = EditorHelper.SourceCodeProjectCorePluginFile + ".user";
				const string userFileEditor = EditorHelper.SourceCodeProjectEditorPluginFile + ".user";

				if (!File.Exists(userFileCore))
				{
					using (MemoryStream gamePluginStream = new MemoryStream(Properties.GeneralRes.GamePluginTemplate))
					using (ZipArchive gamePluginZip = null)
					{
						foreach (var e in gamePluginZip.Entries)
						{
							if (string.Equals(Path.GetFileName(e.FullName), Path.GetFileName(userFileCore), StringComparison.InvariantCultureIgnoreCase))
							{
								e.Extract(EditorHelper.SourceCodeDirectory, true);
								break;
							}
						}
					}
				}
				if (File.Exists(userFileCore))
				{
					userDoc = XDocument.Load(userFileCore);
					foreach (XElement element in userDoc.Descendants("StartProgram", true))
						element.Value = Path.GetFullPath(DualityEditorApp.LauncherAppPath);
					foreach (XElement element in userDoc.Descendants("StartWorkingDirectory", true))
						element.Value = Path.GetFullPath(".");
					userDoc.Save(userFileCore);
				}
				
				if (!File.Exists(userFileEditor))
				{
					using (MemoryStream gamePluginStream = new MemoryStream(Properties.GeneralRes.GamePluginTemplate))
					using (ZipArchive gamePluginZip = null)
					{
						foreach (var e in gamePluginZip.Entries)
						{
							if (string.Equals(Path.GetFileName(e.FullName), Path.GetFileName(userFileEditor), StringComparison.InvariantCultureIgnoreCase))
							{
								e.Extract(EditorHelper.SourceCodeDirectory, true);
								break;
							}
						}
					}
				}
				if (File.Exists(userFileEditor))
				{
					userDoc = XDocument.Load(userFileEditor);
					foreach (XElement element in userDoc.Descendants("StartProgram", true))
						element.Value = Path.GetFullPath("DualityEditor.exe");
					foreach (XElement element in userDoc.Descendants("StartWorkingDirectory", true))
						element.Value = Path.GetFullPath(".");
					userDoc.Save(userFileEditor);
				}
			}

			// Keep auto-generated files up-to-date
			File.WriteAllText(EditorHelper.SourceCodeGameResFile, EditorHelper.GenerateGameResSrcFile());
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
			// Create solution file if not existing yet
			if (!File.Exists(EditorHelper.SourceCodeSolutionFile))
			{
				using (MemoryStream gamePluginStream = new MemoryStream(Properties.GeneralRes.GamePluginTemplate))
				using (ZipArchive gamePluginZip = new ZipArchive(gamePluginStream))
				{
					gamePluginZip.ExtractAll(EditorHelper.SourceCodeDirectory, false);
				}
			}

			// If Visual Studio is available, don't use the express version
			if (File.Exists(EditorHelper.SourceCodeSolutionFile) && (int)EditorHelper.VisualStudioEdition >= (int)VisualStudioEdition.Standard)
			{
				string solution = File.ReadAllText(EditorHelper.SourceCodeSolutionFile);
				File.WriteAllText(EditorHelper.SourceCodeSolutionFile, solution.Replace("# Visual C# Express 2010", "# Visual Studio 2010"), Encoding.UTF8);
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
		public static void NotifyObjPropChanged(object sender, ObjectSelection obj, params PropertyInfo[] info)
		{
			if (obj == null) return;
			if (obj.Empty) return;
			OnObjectPropertyChanged(sender, new ObjectPropertyChangedEventArgs(obj, info));
		}
		public static void NotifyObjPropChanged(object sender, ObjectSelection obj, bool persistenceCritical, params PropertyInfo[] info)
		{
			if (obj == null) return;
			if (obj.Empty) return;
			OnObjectPropertyChanged(sender, new ObjectPropertyChangedEventArgs(obj, info, persistenceCritical));
		}

		public static T GetPlugin<T>() where T : EditorPlugin
		{
			return plugins.OfType<T>().FirstOrDefault();
		}
		public static void AnalyzeCorePlugin(CorePlugin plugin)
		{
			Log.Editor.Write("Analyzing Core Plugin: {0}", plugin.AssemblyName);
			Log.Editor.PushIndent();

			// Query references to other Assemblies
			var asmRefQuery = from AssemblyName a in plugin.PluginAssembly.GetReferencedAssemblies()
							  select a.GetShortAssemblyName();
			string thisAsmName = typeof(DualityEditorApp).Assembly.GetShortAssemblyName();
			foreach (var asmName in asmRefQuery)
			{
				bool illegalRef = false;

				// Scan for illegally referenced Assemblies
				if (asmName == thisAsmName)
					illegalRef = true;
				else if (plugins.Any(p => p.PluginAssembly.GetShortAssemblyName() == asmName))
					illegalRef = true;

				// Warn about them
				if (illegalRef)
				{
					Log.Editor.WriteWarning(
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
				Log.Editor.WriteError(
					"Unable to analyze exported types because an error occured: {0}",
					Log.Exception(e));
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
						Log.Editor.WriteWarning(
							"Found public fields in Component class '{0}': {1}. " + 
							"The usage of public fields is strongly discouraged in Component classes. Consider using properties instead.",
							cmpType.GetTypeCSCodeName(true),
							fields.ToString(f => Log.FieldInfo(f, false), ", "));
					}
				}
			}

			Log.Editor.PopIndent();
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
		private static void OnSelectionChanged(object sender, ObjectSelection.Category changedCategoryFallback)
		{
			//if (selectionCurrent == selectionPrevious) return;
			selectionChanging = true;

			selectionActiveCat = changedCategoryFallback;
			if (SelectionChanged != null)
				SelectionChanged(sender, new SelectionChangedEventArgs(selectionCurrent, selectionPrevious, changedCategoryFallback));

			selectionChanging = false;
		}
		private static void OnObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs args)
		{
			//Log.Editor.Write("OnObjectPropertyChanged: {0}{2}\t{1}", args.PropNames.ToString(", "), args.Objects.Objects.ToString(", "), Environment.NewLine);
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
				if (args.Objects.GameObjects.Any(g => g.ParentScene == Scene.Current) ||
					args.Objects.Components.Any(c => c.GameObj.ParentScene == Scene.Current))
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
					DualityApp.ExecutionContext lastContext = DualityApp.ExecContext;
					if (fixedSingleStep) DualityApp.ExecContext = DualityApp.ExecutionContext.Game;

					try
					{
						DualityApp.EditorUpdate(
							editorObjects.ActiveObjects.Concat(updateObjects), 
							Sandbox.IsFreezed, 
							fixedSingleStep && Sandbox.State != SandboxState.Playing);
						updateObjects.Clear();
					}
					catch (Exception exception)
					{
						Log.Editor.WriteError("An error occurred during a core update: {0}", Log.Exception(exception));
					}
					OnUpdatingEngine();

					if (fixedSingleStep) DualityApp.ExecContext = lastContext;
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
				Deselect(sender, new ObjectSelection(e.Content.Res));
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
				Log.Editor.Write("Recovering from full plugin reload restart...");
				Log.Editor.PushIndent();
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
		private static void menuKeyInterceptor_SystemKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Menu)
			{
				// Intercept the menu / Alt key, since it only causes problems (stealing the message loop in unfortunate situations, etc.)
				e.Handled = true;
			}
		}

		private static void editorObjects_Registered(object sender, GameObjectEventArgs e)
		{
			if (e.Object.Active)
				e.Object.OnActivate();
		}
		private static void editorObjects_Unregistered(object sender, GameObjectEventArgs e)
		{
			if (e.Object.Active || e.Object.Disposed)
				e.Object.OnDeactivate();
		}
		private static void editorObjects_ComponentAdded(object sender, ComponentEventArgs e)
		{
			if (e.Component.Active)
			{
				ICmpInitializable cInit = e.Component as ICmpInitializable;
				if (cInit != null) cInit.OnInit(Component.InitContext.Activate);
			}
		}
		private static void editorObjects_ComponentRemoved(object sender, ComponentEventArgs e)
		{
			if (e.Component.Active)
			{
				ICmpInitializable cInit = e.Component as ICmpInitializable;
				if (cInit != null) cInit.OnShutdown(Component.ShutdownContext.Deactivate);
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
		private static void DualityApp_PluginReady(object sender, CorePluginEventArgs e)
		{
			AnalyzeCorePlugin(e.Plugin);
		}
		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			// If this method gets called, assume we are searching for a dynamically loaded plugin assembly
			string assemblyNameStub = ReflectionHelper.GetShortAssemblyName(args.Name);
			EditorPlugin plugin = plugins.FirstOrDefault(p => assemblyNameStub == p.AssemblyName);
			if (plugin != null)
				return plugin.PluginAssembly;
			else
				return null;
		}
		private static Image EditorHintImageResolver(string resourceClassName, string propertyName)
		{
			string shortClassName = resourceClassName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
			if (string.IsNullOrEmpty(shortClassName)) return null;

			foreach (Assembly editorPlugin in GetDualityEditorAssemblies())
			{
				Type[] editorTypes;
				try { editorTypes = editorPlugin.GetTypes(); }
				catch (Exception) { continue; }

				foreach (Type editorClass in editorTypes)
				{
					if (editorClass.Name == shortClassName)
					{
						try
						{
							PropertyInfo resourceProperty = editorClass.GetProperty(propertyName, ReflectionHelper.BindStaticAll);
							if (resourceProperty != null && typeof(Image).IsAssignableFrom(resourceProperty.PropertyType))
							{
								return resourceProperty.GetValue(null, null) as Image;
							}
						}
						catch (Exception) {}
					}
				}
			}
			return null;
		}
	}
}
