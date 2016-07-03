using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Threading;

using Duality.Backend;
using Duality.Resources;
using Duality.Serialization;
using Duality.Drawing;
using Duality.Audio;
using Duality.Cloning;
using Duality.Input;
using Duality.IO;

namespace Duality
{
	/// <summary>
	/// This class controls Duality's main program flow control and general maintenance functionality.
	/// It initializes the engine, loads plugins, provides access to user input, houses global data structures
	/// and handles logfiles internally.
	/// </summary>
	public static class DualityApp
	{
		/// <summary>
		/// Describes the context in which the current DualityApp runs.
		/// </summary>
		public enum ExecutionContext
		{
			/// <summary>
			/// Duality has been terminated. There is no guarantee that any object is still valid or usable.
			/// </summary>
			Terminated,
			/// <summary>
			/// The context in which Duality is executed is unknown.
			/// </summary>
			Unknown,
			/// <summary>
			/// Duality runs in a game environment.
			/// </summary>
			Game,
			/// <summary>
			/// Duality runs in an editing environment.
			/// </summary>
			Editor
		}
		/// <summary>
		/// Describes the environment in which the current DualityApp runs.
		/// </summary>
		public enum ExecutionEnvironment
		{
			/// <summary>
			/// The environment in which Duality is executed is unknown.
			/// </summary>
			Unknown,
			/// <summary>
			/// Duality runs in the DualityLauncher
			/// </summary>
			Launcher,
			/// <summary>
			/// Duality runs in the DualityEditor
			/// </summary>
			Editor
		}

		public const string CmdArgDebug     = "debug";
		public const string CmdArgEditor    = "editor";
		public const string CmdArgProfiling = "profile";
		public const string PluginDirectory = "Plugins";
		public const string DataDirectory   = "Data";


		private static bool                    initialized        = false;
		private static bool                    isUpdating         = false;
		private static bool                    runFromEditor      = false;
		private static bool                    terminateScheduled = false;
		private static IPluginLoader           pluginLoader       = null;
		private static CorePluginManager       pluginManager      = new CorePluginManager();
		private static ISystemBackend          systemBack         = null;
		private static IGraphicsBackend        graphicsBack       = null;
		private static IAudioBackend           audioBack          = null;
		private static Vector2                 targetResolution   = Vector2.Zero;
		private static MouseInput              mouse              = new MouseInput();
		private static KeyboardInput           keyboard           = new KeyboardInput();
		private static JoystickInputCollection joysticks          = new JoystickInputCollection();
		private static GamepadInputCollection  gamepads           = new GamepadInputCollection();
		private static SoundDevice             sound              = null;
		private static ExecutionEnvironment    environment        = ExecutionEnvironment.Unknown;
		private static ExecutionContext        execContext        = ExecutionContext.Terminated;
		private static DualityAppData          appData            = null;
		private static DualityUserData         userData           = null;
		private static List<object>            disposeSchedule    = new List<object>();
		
		/// <summary>
		/// Called when the game becomes focused or loses focus.
		/// </summary>
		public static event EventHandler FocusChanged
		{
			add
			{
				keyboard.BecomesAvailable += value;
				keyboard.NoLongerAvailable += value;
			}
			remove
			{
				keyboard.BecomesAvailable -= value;
				keyboard.NoLongerAvailable -= value;
			}
		}
		/// <summary>
		/// Called when the games UserData changes
		/// </summary>
		public static event EventHandler UserDataChanged = null;
		/// <summary>
		/// Called when the games AppData changes
		/// </summary>
		public static event EventHandler AppDataChanged = null;
		/// <summary>
		/// Called when Duality is being terminated by choice (e.g. not because of crashes or similar).
		/// It is also called in an editor environment.
		/// </summary>
		public static event EventHandler Terminating = null;
		/// <summary>
		/// Called when Duality needs to discard plugin data such as cached Types and values.
		/// </summary>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		public static event EventHandler DiscardPluginData = null;
		/// <summary>
		/// Fired whenever a core plugin has been initialized. This is the case after loading or reloading one.
		/// </summary>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		public static event EventHandler<CorePluginEventArgs> PluginReady = null;

		
		/// <summary>
		/// [GET] The plugin manager that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// If you want to load a plugin, use the <see cref="CorePluginManager"/> from this property.
		/// If you want to load a non-plugin Assembly, use the <see cref="PluginLoader"/>.
		/// </summary>
		public static CorePluginManager PluginManager
		{
			get { return pluginManager; }
		}
		/// <summary>
		/// [GET] The plugin loader that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// If you want to load a plugin, use the <see cref="PluginManager"/>. 
		/// If you want to load a non-plugin Assembly, use the <see cref="IPluginLoader"/> from this property.
		/// </summary>
		public static IPluginLoader PluginLoader
		{
			get { return pluginLoader; }
		}
		/// <summary>
		/// [GET] The system backend that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// </summary>
		public static ISystemBackend SystemBackend
		{
			get { return systemBack; }
		}
		/// <summary>
		/// [GET] The graphics backend that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// </summary>
		public static IGraphicsBackend GraphicsBackend
		{
			get { return graphicsBack; }
		}
		/// <summary>
		/// [GET] The audio backend that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// </summary>
		public static IAudioBackend AudioBackend
		{
			get { return audioBack; }
		}
		/// <summary>
		/// [GET / SET] The size of the current rendering surface (full screen, a single window, etc.) in pixels. Setting this will not actually change
		/// Duality's state - this is a pure "for your information" property.
		/// </summary>
		public static Vector2 TargetResolution
		{
			get { return targetResolution; }
			set { targetResolution = value; }
		}
		/// <summary>
		/// [GET] Returns whether the Duality application is currently focused, i.e. can be considered
		/// to be the users main activity right now.
		/// </summary>
		public static bool IsFocused
		{
			get { return keyboard.IsAvailable; }
		}
		/// <summary>
		/// [GET] Provides access to mouse user input.
		/// </summary>
		public static MouseInput Mouse
		{
			get { return mouse; }
		}
		/// <summary>
		/// [GET] Provides access to keyboard user input
		/// </summary>
		public static KeyboardInput Keyboard
		{
			get { return keyboard; }
		}
		/// <summary>
		/// [GET] Provides access to extended user input via joystick or gamepad.
		/// </summary>
		public static JoystickInputCollection Joysticks
		{
			get { return joysticks; }
		}
		/// <summary>
		/// [GET] Provides access to gamepad user input.
		/// </summary>
		public static GamepadInputCollection Gamepads
		{
			get { return gamepads; }
		}
		/// <summary>
		/// [GET] Provides access to the main <see cref="SoundDevice"/>.
		/// </summary>
		public static SoundDevice Sound
		{
			get { return sound; }
		}
		/// <summary>
		/// [GET / SET] Provides access to Duality's current <see cref="DualityAppData">application data</see>. This is never null.
		/// Any kind of data change event is fired as soon as you re-assign this property. Be sure to do that after changing its data.
		/// </summary>
		public static DualityAppData AppData
		{
			get { return appData; }
			set 
			{ 
				appData = value ?? new DualityAppData();
				// We're currently missing direct changes without invoking this setter
				OnAppDataChanged();
			}
		}
		/// <summary>
		/// [GET / SET] Provides access to Duality's current <see cref="DualityUserData">user data</see>. This is never null.
		/// Any kind of data change event is fired as soon as you re-assign this property. Be sure to do that after changing its data.
		/// </summary>
		public static DualityUserData UserData
		{
			get { return userData; }
			set 
			{ 
				userData = value ?? new DualityUserData();
				// We're currently missing direct changes without invoking this setter
				OnUserDataChanged();
			}
		}
		/// <summary>
		/// [GET] Returns the path where this DualityApp's <see cref="DualityAppData">application data</see> is located at.
		/// </summary>
		public static string AppDataPath
		{
			get { return "AppData.dat"; }
		}
		/// <summary>
		/// [GET] Returns the path where this DualityApp's <see cref="DualityUserData">user data</see> is located at.
		/// </summary>
		public static string UserDataPath
		{
			get
			{
				if (AppData.LocalUserData)
				{
					return "UserData.dat";
				}
				else
				{
					return PathOp.Combine(
						systemBack.GetNamedPath(NamedDirectory.MyDocuments),
						"Duality", 
						"AppData", 
						PathOp.GetValidFileName(appData.AppName), 
						"UserData.dat");
				}
			}
		}
		/// <summary>
		/// [GET] Returns the <see cref="ExecutionContext"/> in which this DualityApp is currently running.
		/// </summary>
		public static ExecutionContext ExecContext
		{
			get { return execContext; }
			internal set 
			{
				if (execContext != value)
				{
					ExecutionContext previous = execContext;
					execContext = value;
					pluginManager.InvokeExecContextChanged(previous);
				}
			}
		}
		/// <summary>
		/// [GET] Returns the <see cref="ExecutionEnvironment"/> in which this DualityApp is currently running.
		/// </summary>
		public static ExecutionEnvironment ExecEnvironment
		{
			get { return environment; }
		}
		/// <summary>
		/// [GET] Enumerates all currently loaded plugins.
		/// </summary>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		public static IEnumerable<CorePlugin> LoadedPlugins
		{
			get { return pluginManager.LoadedPlugins; }
		}
		/// <summary>
		/// [GET] Enumerates all plugin assemblies that have been loaded before, but have been discarded due to a runtime plugin reload operation.
		/// This is usually only the case when being executed from withing the editor or manually triggering a plugin reload. However,
		/// this is normally unnecessary.
		/// </summary>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		public static IEnumerable<Assembly> DisposedPlugins
		{
			get { return pluginManager.DisposedPlugins; }
		}


		/// <summary>
		/// Initializes this DualityApp. Should be called before performing any operations within Duality.
		/// </summary>
		/// <param name="context">The <see cref="ExecutionContext"/> in which Duality runs.</param>
		/// <param name="commandLineArgs">
		/// Command line arguments to run this DualityApp with. 
		/// Usually these are just the ones from the host application, passed on.
		/// </param>
		public static void Init(ExecutionEnvironment env, ExecutionContext context, IPluginLoader plugins, string[] commandLineArgs)
		{
			if (initialized) return;

			// Process command line options
			if (commandLineArgs != null)
			{
				// Enter debug mode
				if (commandLineArgs.Contains(CmdArgDebug)) System.Diagnostics.Debugger.Launch();
				// Run from editor
				if (commandLineArgs.Contains(CmdArgEditor)) runFromEditor = true;
			}

			environment = env;
			execContext = context;
			
			// Initialize the plugin manager
			{
				pluginLoader = plugins ?? new Duality.Backend.Dummy.DummyPluginLoader();
				Log.Core.Write("Using '{0}' to load plugins.", pluginLoader.GetType().Name);

				pluginLoader.Init();

				// Log assembly loading data for diagnostic purposes
				{
					Log.Core.Write("Currently Loaded Assemblies:" + Environment.NewLine + "{0}",
						pluginLoader.LoadedAssemblies.ToString(
							assembly => "  " + Log.Assembly(assembly),
							Environment.NewLine));
					Log.Core.Write("Plugin Base Directories:" + Environment.NewLine + "{0}",
						pluginLoader.BaseDirectories.ToString(
							path => "  " + path,
							Environment.NewLine));
					Log.Core.Write("Available Assembly Paths:" + Environment.NewLine + "{0}",
						pluginLoader.AvailableAssemblyPaths.ToString(
							path => "  " + path,
							Environment.NewLine));
				}

				pluginManager.Init(pluginLoader);
				pluginManager.PluginsReady += pluginManager_PluginsReady;
				pluginManager.PluginsRemoving += pluginManager_PluginsRemoving;
				pluginManager.PluginsRemoved += pluginManager_PluginsRemoved;
			}

			// Load all plugins. This needs to be done first, so backends and Types can be located.
			pluginManager.LoadPlugins();

			// Initialize the system backend for system info and file system access
			InitBackend(out systemBack);

			// Load application and user data and submit a change event, so all settings are applied
			LoadAppData();
			LoadUserData();
			OnAppDataChanged();
			OnUserDataChanged();

			// Initialize the graphics backend
			InitBackend(out graphicsBack);

			// Initialize the audio backend
			InitBackend(out audioBack);
			sound = new SoundDevice();

			// Initialize all core plugins, this may allocate Resources or establish references between plugins
			pluginManager.InitPlugins();
			
			initialized = true;

			// Write environment specs as a debug log
			Log.Core.Write(
				"DualityApp initialized" + Environment.NewLine +
				"Debug Mode: {0}" + Environment.NewLine +
				"Command line arguments: {1}",
				System.Diagnostics.Debugger.IsAttached,
				commandLineArgs != null ? commandLineArgs.ToString(", ") : "null");
		}
		/// <summary>
		/// Opens up a window for Duality to render into. This also initializes the part of Duality that requires a 
		/// valid rendering context. Should be called before performing any rendering related operations with Duality.
		/// </summary>
		public static INativeWindow OpenWindow(WindowOptions options)
		{
			if (!initialized) throw new InvalidOperationException("Can't initialize graphics / rendering because Duality itself isn't initialized yet.");

			Log.Core.Write("Opening Window...");
			Log.Core.PushIndent();
			INativeWindow window = graphicsBack.CreateWindow(options);
			Log.Core.PopIndent();

			InitPostWindow();

			return window;
		}
		/// <summary>
		/// Initializes the part of Duality that requires a valid rendering context. 
		/// Should be called before performing any rendering related operations with Duality.
		/// Is called implicitly when using <see cref="OpenWindow"/>.
		/// </summary>
		public static void InitPostWindow()
		{
			ContentProvider.InitDefaultContent();
		}
		/// <summary>
		/// Terminates this DualityApp. This does not end the current Process, but will instruct the engine to
		/// leave main loop and message processing as soon as possible.
		/// </summary>
		public static void Terminate()
		{
			if (!initialized) return;
			if (isUpdating)
			{
				terminateScheduled = true;
				return;
			}

			if (environment == ExecutionEnvironment.Editor && execContext == ExecutionContext.Game)
			{
				Scene.Current.Dispose();
				Log.Core.Write("DualityApp Sandbox terminated");
				terminateScheduled = false;
				return;
			}

			if (execContext != ExecutionContext.Editor)
			{
				OnTerminating();
				SaveUserData();
			}

			// Discard plugin data (Resources, current Scene) ahead of time. Otherwise, it'll get shut down in ClearPlugins, after the backend is gone.
			pluginManager.DiscardPluginData();

			sound.Dispose();
			sound = null;
			ShutdownBackend(ref graphicsBack);
			ShutdownBackend(ref audioBack);
			pluginManager.ClearPlugins();

			// Since this performs file system operations, it needs to happen before shutting down the system backend.
			Profile.SaveTextReport(environment == ExecutionEnvironment.Editor ? "perflog_editor.txt" : "perflog.txt");

			ShutdownBackend(ref systemBack);

			// Shut down the plugin manager and plugin loader
			pluginManager.Terminate();
			pluginManager.PluginsReady -= pluginManager_PluginsReady;
			pluginManager.PluginsRemoving -= pluginManager_PluginsRemoving;
			pluginManager.PluginsRemoved -= pluginManager_PluginsRemoved;
			pluginLoader.Terminate();
			pluginLoader = null;

			Log.Core.Write("DualityApp terminated");

			initialized = false;
			execContext = ExecutionContext.Terminated;
		}

		/// <summary>
		/// Applies the specified screen resolution to both game and display device. This is a shorthand for
		/// assigning a modified version of <see cref="DualityUserData"/> to <see cref="UserData"/>.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fullscreen"></param>
		public static void ApplyResolution(int width, int height, bool fullscreen)
		{
			userData.GfxWidth = width;
			userData.GfxHeight = height;
			userData.GfxMode = fullscreen ? ScreenMode.Fullscreen : ScreenMode.Window;
			OnUserDataChanged();
		}

		/// <summary>
		/// Performs a single update cycle.
		/// </summary>
		public static void Update()
		{
			isUpdating = true;
			Profile.TimeUpdate.BeginMeasure();

			Time.FrameTick();
			Profile.FrameTick();
			VisualLog.UpdateLogEntries();
			pluginManager.InvokeBeforeUpdate();
			UpdateUserInput();
			Scene.Current.Update();
			sound.Update();
			pluginManager.InvokeAfterUpdate();
			VisualLog.PrepareRenderLogEntries();
			RunCleanup();

			Profile.TimeUpdate.EndMeasure();
			isUpdating = false;

			if (terminateScheduled) Terminate();
		}
		internal static void EditorUpdate(IEnumerable<GameObject> updateObjects, bool freezeScene, bool forceFixedStep)
		{
			isUpdating = true;
			Profile.TimeUpdate.BeginMeasure();

			Time.FrameTick(forceFixedStep);
			Profile.FrameTick();
			if (execContext == ExecutionContext.Game && !freezeScene)
			{
				VisualLog.UpdateLogEntries();
			}
			pluginManager.InvokeBeforeUpdate();
			if (execContext == ExecutionContext.Game)
			{
				if (!freezeScene)	UpdateUserInput();

				if (!freezeScene)	Scene.Current.Update();
				else				Scene.Current.EditorUpdate();

				foreach (GameObject obj in updateObjects)
				{
					if (!freezeScene && obj.ParentScene == Scene.Current)
						continue;
					
					obj.IterateComponents<ICmpUpdatable>(
						l => l.OnUpdate(),
						l => (l as Component).Active);
				}
			}
			else if (execContext == ExecutionContext.Editor)
			{
				Scene.Current.EditorUpdate();
				foreach (GameObject obj in updateObjects)
				{
					obj.IterateComponents<ICmpUpdatable>(
						l => l.OnUpdate(),
						l => (l as Component).Active);
				}
			}
			sound.Update();
			pluginManager.InvokeAfterUpdate();
			VisualLog.PrepareRenderLogEntries();
			RunCleanup();

			Profile.TimeUpdate.EndMeasure();
			isUpdating = false;

			if (terminateScheduled) Terminate();
		}
		/// <summary>
		/// Performs a single render cycle.
		/// </summary>
		/// <param name="camPredicate">Optional predicate to select which Cameras may be rendered and which not.</param>
		public static void Render(Rect viewportRect, Predicate<Duality.Components.Camera> camPredicate = null)
		{
			Scene.Current.Render(viewportRect, camPredicate);
		}

		/// <summary>
		/// Schedules the specified object for disposal. It is guaranteed to be disposed by the end of the current update cycle.
		/// </summary>
		/// <param name="o">The object to schedule for disposal.</param>
		public static void DisposeLater(object o)
		{
			disposeSchedule.Add(o);
		}
		/// <summary>
		/// Performs all scheduled disposal calls and cleans up internal data. This is done automatically at the
		/// end of each <see cref="Update">frame update</see> and you shouldn't need to call this in general.
		/// Invoking this method while an update is still in progress may result in undefined behavior. Don't do this.
		/// </summary>
		public static void RunCleanup()
		{
			// Perform scheduled object disposals
			object[] disposeScheduleArray = disposeSchedule.ToArray();
			disposeSchedule.Clear();
			foreach (object o in disposeScheduleArray)
			{
				IManageableObject m = o as IManageableObject;
				if (m != null) { m.Dispose(); continue; }
				IDisposable d = o as IDisposable;
				if (d != null) { d.Dispose(); continue; }
			}

			// Perform late finalization and remove disposed object references
			Resource.RunCleanup();
			Scene.Current.RunCleanup();
		}

		/// <summary>
		/// Triggers Duality to (re)load its <see cref="DualityAppData"/>.
		/// </summary>
		public static void LoadAppData()
		{
			appData = Serializer.TryReadObject<DualityAppData>(AppDataPath) ?? new DualityAppData();
		}
		/// <summary>
		/// Triggers Duality to (re)load its <see cref="DualityUserData"/>.
		/// </summary>
		public static void LoadUserData()
		{
			string path = UserDataPath;
			if (!FileOp.Exists(path) || execContext == ExecutionContext.Editor || runFromEditor) path = "DefaultUserData.dat";
			userData = Serializer.TryReadObject<DualityUserData>(path) ?? new DualityUserData();
		}
		/// <summary>
		/// Triggers Duality to save its <see cref="DualityAppData"/>.
		/// </summary>
		public static void SaveAppData()
		{
			Serializer.WriteObject(appData, AppDataPath, typeof(XmlSerializer));
		}
		/// <summary>
		/// Triggers Duality to save its <see cref="DualityUserData"/>.
		/// </summary>
		public static void SaveUserData()
		{
			string path = UserDataPath;
			Serializer.WriteObject(userData, UserDataPath, typeof(XmlSerializer));
			if (execContext == ExecutionContext.Editor)
			{
				Serializer.WriteObject(userData, "DefaultUserData.dat", typeof(XmlSerializer));
			}
		}

		/// <summary>
		/// Adds an already loaded plugin Assembly to the internal Duality CorePlugin registry.
		/// You shouldn't need to call this method in general, since Duality manages its plugins
		/// automatically. 
		/// </summary>
		/// <remarks>
		/// This method can be useful in certain cases when it is necessary to treat an Assembly as a
		/// Duality plugin, even though it isn't located in the Plugins folder, or is not available
		/// as a file at all. A typical case for this is Unit Testing where the testing Assembly may
		/// specify additional Duality types such as Components, Resources, etc.
		/// </remarks>
		/// <param name="pluginAssembly"></param>
		/// <param name="pluginFilePath"></param>
		/// <returns></returns>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		public static CorePlugin LoadPlugin(Assembly pluginAssembly, string pluginFilePath)
		{
			return pluginManager.LoadPlugin(pluginAssembly, pluginFilePath);
		}
		[Obsolete("Use DualityApp.PluginManager instead.")]
		internal static void InitPlugin(CorePlugin plugin)
		{
			pluginManager.InitPlugin(plugin);
		}
		/// <summary>
		/// Reloads the specified plugin. Does not initialize it.
		/// </summary>
		/// <param name="pluginFilePath"></param>
		[Obsolete("Use DualityApp.PluginManager instead.")]
		internal static CorePlugin ReloadPlugin(string pluginFilePath)
		{
			return pluginManager.ReloadPlugin(pluginFilePath);
		}

		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Assembly> GetDualityAssemblies()
		{
			return pluginManager.GetAssemblies();
		}
		/// <summary>
		/// Enumerates all available Duality <see cref="System.Type">Types</see> that are assignable
		/// to the specified Type. 
		/// </summary>
		/// <param name="baseType">The base type to use for matching the result types.</param>
		/// <returns>An enumeration of all Duality types deriving from the specified type.</returns>
		/// <example>
		/// The following code logs all available kinds of <see cref="Duality.Components.Renderer">Renderers</see>:
		/// <code>
		/// var rendererTypes = DualityApp.GetAvailDualityTypes(typeof(Duality.Components.Renderer));
		/// foreach (Type rt in rendererTypes)
		/// {
		/// 	Log.Core.Write("Renderer Type '{0}' from Assembly '{1}'", Log.Type(rt), rt.Assembly.FullName);
		/// }
		/// </code>
		/// </example>
		public static IEnumerable<TypeInfo> GetAvailDualityTypes(Type baseType)
		{
			return pluginManager.GetTypes(baseType);
		}

		private static void UpdateUserInput()
		{
			mouse.Update();
			keyboard.Update();
			joysticks.Update();
			gamepads.Update();
		}

		internal static void InitBackend<T>(out T target, Func<Type,IEnumerable<TypeInfo>> typeFinder = null) where T : class, IDualityBackend
		{
			if (typeFinder == null) typeFinder = GetAvailDualityTypes;

			Log.Core.Write("Initializing {0}...", Log.Type(typeof(T)));
			Log.Core.PushIndent();

			// Generate a list of available backends for evaluation
			List<IDualityBackend> backends = new List<IDualityBackend>();
			foreach (TypeInfo backendType in typeFinder(typeof(IDualityBackend)))
			{
				if (backendType.IsInterface) continue;
				if (backendType.IsAbstract) continue;
				if (!backendType.IsClass) continue;
				if (!typeof(T).GetTypeInfo().IsAssignableFrom(backendType)) continue;

				IDualityBackend backend = backendType.CreateInstanceOf() as IDualityBackend;
				if (backend == null)
				{
					Log.Core.WriteWarning("Unable to create an instance of {0}. Skipping it.", backendType.FullName);
					continue;
				}
				backends.Add(backend);
			}

			// Sort backends from best to worst
			backends.StableSort((a, b) => b.Priority > a.Priority ? 1 : -1);

			// Try to initialize each one and select the first that works
			T selectedBackend = null;
			foreach (T backend in backends)
			{
				if (appData != null && 
					appData.SkipBackends != null && 
					appData.SkipBackends.Any(s => string.Equals(s, backend.Id, StringComparison.OrdinalIgnoreCase)))
				{
					Log.Core.Write("Backend '{0}' skipped because of AppData settings.", backend.Name);
					continue;
				}

				bool available = false;
				try
				{
					available = backend.CheckAvailable();
					if (!available)
					{
						Log.Core.Write("Backend '{0}' reports to be unavailable. Skipping it.", backend.Name);
					}
				}
				catch (Exception e)
				{
					available = false;
					Log.Core.WriteWarning("Backend '{0}' failed the availability check with an exception: {1}", backend.Name, Log.Exception(e));
				}
				if (!available) continue;

				Log.Core.Write("{0}...", backend.Name);
				Log.Core.PushIndent();
				{
					try
					{
						backend.Init();
						selectedBackend = backend;
					}
					catch (Exception e)
					{
						Log.Core.WriteError("Failed: {0}", Log.Exception(e));
					}
				}
				Log.Core.PopIndent();

				if (selectedBackend != null)
					break;
			}

			// If we found a proper backend and initialized it, add it to the list of active backends
			if (selectedBackend != null)
			{
				target = selectedBackend;

				TypeInfo selectedBackendType = selectedBackend.GetType().GetTypeInfo();
				pluginManager.LockPlugin(selectedBackendType.Assembly);
			}
			else
			{
				target = null;
			}

			Log.Core.PopIndent();
		}
		internal static void ShutdownBackend<T>(ref T backend) where T : class, IDualityBackend
		{
			if (backend == null) return;

			Log.Core.Write("Shutting down {0}...", backend.Name);
			Log.Core.PushIndent();
			{
				try
				{
					backend.Shutdown();

					TypeInfo backendType = backend.GetType().GetTypeInfo();
					pluginManager.UnlockPlugin(backendType.Assembly);

					backend = null;
				}
				catch (Exception e)
				{
					Log.Core.WriteError("Failed: {0}", Log.Exception(e));
				}
			}
			Log.Core.PopIndent();
		}

		private static void OnUserDataChanged()
		{
			if (UserDataChanged != null)
				UserDataChanged(null, EventArgs.Empty);
		}
		private static void OnAppDataChanged()
		{
			if (AppDataChanged != null)
				AppDataChanged(null, EventArgs.Empty);
		}
		private static void OnTerminating()
		{
			if (Terminating != null)
				Terminating(null, EventArgs.Empty);
		}
		
		private static void pluginManager_PluginsReady(object sender, DualityPluginEventArgs e)
		{
			if (PluginReady != null)
				PluginReady(sender, new CorePluginEventArgs(e.Plugins.OfType<CorePlugin>()));
		}
		private static void pluginManager_PluginsRemoving(object sender, DualityPluginEventArgs e)
		{
			// Wrapper method for delivering the old API until removing it in v3.0
			if (DiscardPluginData != null)
				DiscardPluginData(sender, e);

			// Dispose any existing Resources that could reference plugin data
			VisualLog.ClearAll();
			if (!Scene.Current.IsEmpty)
				Scene.Current.Dispose();
			foreach (Resource r in ContentProvider.EnumeratePluginContent().ToArray())
				ContentProvider.RemoveContent(r.Path);
		}
		private static void pluginManager_PluginsRemoved(object sender, DualityPluginEventArgs e)
		{
			// Clean globally cached type data
			ImageCodec.ClearTypeCache();
			ObjectCreator.ClearTypeCache();
			ReflectionHelper.ClearTypeCache();
			Component.ClearTypeCache();
			Serializer.ClearTypeCache();
			CloneProvider.ClearTypeCache();

			// Clean input sources that a disposed Assembly forgot to unregister.
			foreach (CorePlugin plugin in e.Plugins)
				CleanInputSources(plugin.PluginAssembly);

			// Clean event bindings that are still linked to the disposed Assembly.
			foreach (CorePlugin plugin in e.Plugins)
				CleanEventBindings(plugin.PluginAssembly);
		}

		private static void CleanEventBindings(Assembly invalidAssembly)
		{
			// Note that this method is only a countermeasure against common mistakes. It doesn't guarantee
			// full error safety in all cases. Event bindings inbetween different plugins aren't checked,
			// for example.

			string warningText = string.Format(
				"Found leaked event bindings to invalid Assembly '{0}' from {1}. " +
				"This is a common problem when registering global events from within a CorePlugin " +
				"without properly unregistering them later. Please make sure that all events are " +
				"unregistered in CorePlugin::OnDisposePlugin().",
				invalidAssembly.GetShortAssemblyName(),
				"{0}");

			if (ReflectionHelper.CleanEventBindings(typeof(DualityApp),      invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)));
			if (ReflectionHelper.CleanEventBindings(typeof(Scene),           invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(Scene)));
			if (ReflectionHelper.CleanEventBindings(typeof(Resource),        invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(Resource)));
			if (ReflectionHelper.CleanEventBindings(typeof(ContentProvider), invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(ContentProvider)));
			if (ReflectionHelper.CleanEventBindings(DualityApp.Keyboard,     invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Keyboard");
			if (ReflectionHelper.CleanEventBindings(DualityApp.Mouse,        invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Mouse");
			foreach (JoystickInput joystick in DualityApp.Joysticks)
				if (ReflectionHelper.CleanEventBindings(joystick,            invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Joysticks");
			foreach (GamepadInput gamepad in DualityApp.Gamepads)
				if (ReflectionHelper.CleanEventBindings(gamepad,             invalidAssembly)) Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Gamepads");
		}
		private static void CleanInputSources(Assembly invalidAssembly)
		{
			string warningText = string.Format(
				"Found leaked input source '{1}' defined in invalid Assembly '{0}'. " +
				"This is a common problem when registering input sources from within a CorePlugin " +
				"without properly unregistering them later. Please make sure that all sources are " +
				"unregistered in CorePlugin::OnDisposePlugin() or sooner.",
				invalidAssembly.GetShortAssemblyName(),
				"{0}");

			if (DualityApp.Mouse.Source != null && DualityApp.Mouse.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(DualityApp.Mouse.Source.GetType()));
				DualityApp.Mouse.Source = null;
			}
			if (DualityApp.Keyboard.Source != null && DualityApp.Keyboard.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(DualityApp.Keyboard.Source.GetType()));
				DualityApp.Keyboard.Source = null;
			}
			foreach (JoystickInput joystick in DualityApp.Joysticks.ToArray())
			{
				if (joystick.Source != null && joystick.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(joystick.Source.GetType()));
					DualityApp.Joysticks.RemoveSource(joystick.Source);
				}
			}
			foreach (GamepadInput gamepad in DualityApp.Gamepads.ToArray())
			{
				if (gamepad.Source != null && gamepad.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(gamepad.Source.GetType()));
					DualityApp.Gamepads.RemoveSource(gamepad.Source);
				}
			}
		}


		/// <summary>
		/// This method performs an action only when compiling your plugin in debug mode.
		/// In release mode, any calls to this method (and thus the specified action) are omitted
		/// by the compiler. It is essentially syntactical sugar for one-line #if DEBUG blocks.
		/// This method is intended to be used conveniently in conjunction with lambda expressions.
		/// </summary>
		/// <param name="action"></param>
		[System.Diagnostics.Conditional("DEBUG")]
		public static void Dbg(Action action)
		{
			action();
		}
		/// <summary>
		/// When executed from within the editor environment, this method wraps the specified
		/// action in a safe try-catch block in order to be able to recover gracefully. In regular
		/// game execution, it will simply invoke the action without safety measures.
		/// </summary>
		/// <param name="action"></param>
		public static void EditorGuard(Action action)
		{
			if (ExecEnvironment == ExecutionEnvironment.Editor)
			{
				try
				{
					action();
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("An error occurred: {0}", Log.Exception(e));
				}
			}
			else
			{
				action();
			}
		}
	}
}
