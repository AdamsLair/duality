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

		public const string CmdArgDebug		= "debug";
		public const string CmdArgEditor	= "editor";
		public const string CmdArgProfiling = "profile";
		public const string PluginDirectory = "Plugins";
		public const string DataDirectory	= "Data";


		private	static	bool						initialized			= false;
		private	static	bool						isUpdating			= false;
		private	static	bool						runFromEditor		= false;
		private	static	bool						terminateScheduled	= false;
		private	static	IPluginLoader				pluginLoader		= null;
		private	static	IGraphicsBackend			graphicsBack		= null;
		private	static	IAudioBackend				audioBack			= null;
		private	static	Vector2						targetResolution	= Vector2.Zero;
		private	static	MouseInput					mouse				= new MouseInput();
		private	static	KeyboardInput				keyboard			= new KeyboardInput();
		private	static	JoystickInputCollection		joysticks			= new JoystickInputCollection();
		private	static	GamepadInputCollection		gamepads			= new GamepadInputCollection();
		private	static	SoundDevice					sound				= null;
		private	static	ExecutionEnvironment		environment			= ExecutionEnvironment.Unknown;
		private	static	ExecutionContext			execContext			= ExecutionContext.Terminated;
		private	static	DualityAppData				appData				= null;
		private	static	DualityUserData				userData			= null;
		private	static	List<object>				disposeSchedule		= new List<object>();

		private	static	List<IDualityBackend>			activeBackends	= new List<IDualityBackend>();
		private	static	Dictionary<string,CorePlugin>	plugins			= new Dictionary<string,CorePlugin>();
		private	static	List<Assembly>					disposedPlugins	= new List<Assembly>();
		private static	Dictionary<Type,List<Type>>		availTypeDict	= new Dictionary<Type,List<Type>>();
		
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
		public static event EventHandler UserDataChanged	= null;
		/// <summary>
		/// Called when the games AppData changes
		/// </summary>
		public static event EventHandler AppDataChanged		= null;
		/// <summary>
		/// Called when Duality is being terminated by choice (e.g. not because of crashes or similar).
		/// It is also called in an editor environment.
		/// </summary>
		public static event EventHandler Terminating		= null;
		/// <summary>
		/// Called when Duality needs to discard plugin data such as cached Types and values.
		/// </summary>
		public static event EventHandler DiscardPluginData	= null;
		/// <summary>
		/// Fired whenever a core plugin has been initialized. This is the case after loading or reloading one.
		/// </summary>
		public static event EventHandler<CorePluginEventArgs> PluginReady	= null;

		
		/// <summary>
		/// [GET] The plugin loader that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// </summary>
		public static IPluginLoader PluginLoader
		{
			get { return pluginLoader; }
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
					return Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
						"Duality", 
						"AppData", 
						PathHelper.GetValidFileName(appData.AppName), 
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
					OnExecContextChanged(previous);
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
		public static IEnumerable<CorePlugin> LoadedPlugins
		{
			get { return plugins.Values; }
		}
		/// <summary>
		/// [GET] Enumerates all plugin assemblies that have been loaded before, but have been discarded due to a runtime plugin reload operation.
		/// This is usually only the case when being executed from withing the editor or manually triggering a plugin reload. However,
		/// this is normally unnecessary.
		/// </summary>
		public static IEnumerable<Assembly> DisposedPlugins
		{
			get { return disposedPlugins; }
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
				int logArgIndex = commandLineArgs.IndexOfFirst("logfile");
				if (logArgIndex != -1 && logArgIndex + 1 < commandLineArgs.Length) logArgIndex++;
				else logArgIndex = -1;

				// Enter debug mode
				if (commandLineArgs.Contains(CmdArgDebug)) System.Diagnostics.Debugger.Launch();
				// Run from editor
				if (commandLineArgs.Contains(CmdArgEditor)) runFromEditor = true;
			}

			environment = env;
			execContext = context;

			// Assure Duality is properly terminated in any case and register additional AppDomain events
			AppDomain.CurrentDomain.ProcessExit			+= CurrentDomain_ProcessExit;
			AppDomain.CurrentDomain.UnhandledException	+= CurrentDomain_UnhandledException;

			// Initialize the plugin loader
			{
				pluginLoader = plugins ?? new Duality.Backend.Dummy.DummyPluginLoader();
				Log.Core.Write("Using '{0}' to load plugins.", pluginLoader.GetType().Name);

				pluginLoader.Init(pluginLoader_ResolveAssembly);
			}

			// Write systems specs as a debug log
			{
				string osFriendlyName = null;
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					if (Environment.OSVersion.Version >= new Version(10, 0, 0))
						osFriendlyName = "Windows 10";
					else if (Environment.OSVersion.Version >= new Version(6, 3, 0))
						osFriendlyName = "Windows 8.1";
					else if (Environment.OSVersion.Version >= new Version(6, 2, 0))
						osFriendlyName = "Windows 8";
					else if (Environment.OSVersion.Version >= new Version(6, 1, 0))
						osFriendlyName = "Windows 7";
					else if (Environment.OSVersion.Version >= new Version(6, 0, 0))
						osFriendlyName = "Windows Vista";
					else if (Environment.OSVersion.Version >= new Version(5, 2, 0))
						osFriendlyName = "Windows XP 64 Bit Edition";
					else if (Environment.OSVersion.Version >= new Version(5, 1, 0))
						osFriendlyName = "Windows XP";
					else if (Environment.OSVersion.Version >= new Version(5, 0, 0))
						osFriendlyName = "Windows 2000";
				}
				Log.Core.Write("Initializing Duality...{0}Operating System: {1}{0}64 Bit Process: {2}{0}CLR Version: {3}{0}Processor Count: {4}", 
					Environment.NewLine,
					Environment.OSVersion + (osFriendlyName != null ? (" (" + osFriendlyName + ")") : ""),
					Environment.Is64BitProcess,
					Environment.Version,
					Environment.ProcessorCount);
			}

			LoadPlugins();
			LoadAppData();
			LoadUserData();

			// Initial changed event
			OnAppDataChanged();
			OnUserDataChanged();

			// Determine the default serialization method
			Serializer.InitDefaultMethod();

			// Initialize all core plugins
			InitPlugins();

			// Initialize the graphics backend
			InitBackend(out graphicsBack);

			// Initialize the audio backend
			InitBackend(out audioBack);
			sound = new SoundDevice();
			
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
			Terminate(false);
		}
		private static void Terminate(bool unexpected)
		{
			if (!initialized) return;

			if (unexpected)
			{
				Log.Core.WriteError("DualityApp terminated unexpectedly");
			}
			else
			{
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
				sound.Dispose();
				sound = null;
				ShutdownBackend(ref graphicsBack);
				ShutdownBackend(ref audioBack);
				ClearPlugins();
				pluginLoader.Terminate();
				Profile.SaveTextReport(environment == ExecutionEnvironment.Editor ? "perflog_editor.txt" : "perflog.txt");
				Log.Core.Write("DualityApp terminated");
			}

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
			OnBeforeUpdate();
			UpdateUserInput();
			Scene.Current.Update();
			sound.Update();
			OnAfterUpdate();
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
			OnBeforeUpdate();
			if (execContext == ExecutionContext.Game)
			{
				if (!freezeScene)	UpdateUserInput();

				if (!freezeScene)	Scene.Current.Update();
				else				Scene.Current.EditorUpdate();

				foreach (GameObject obj in updateObjects)
				{
					if (!freezeScene && obj.ParentScene == Scene.Current) continue;
					obj.Update();
				}
			}
			else if (execContext == ExecutionContext.Editor)
			{
				Scene.Current.EditorUpdate();
				foreach (GameObject obj in updateObjects) obj.Update();
			}
			sound.Update();
			OnAfterUpdate();
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
			if (!File.Exists(path) || execContext == ExecutionContext.Editor || runFromEditor) path = "DefaultUserData.dat";
			userData = Serializer.TryReadObject<DualityUserData>(path) ?? new DualityUserData();
		}
		/// <summary>
		/// Triggers Duality to save its <see cref="DualityAppData"/>.
		/// </summary>
		public static void SaveAppData()
		{
			Serializer.WriteObject(appData, AppDataPath, SerializeMethod.Xml);
		}
		/// <summary>
		/// Triggers Duality to save its <see cref="DualityUserData"/>.
		/// </summary>
		public static void SaveUserData()
		{
			string path = UserDataPath;
			Serializer.WriteObject(userData, UserDataPath, SerializeMethod.Xml);
			if (execContext == ExecutionContext.Editor)
			{
				Serializer.WriteObject(userData, "DefaultUserData.dat", SerializeMethod.Xml);
			}
		}

		private static void LoadPlugins()
		{
			if (plugins.Count > 0) throw new InvalidOperationException("Can't load plugins more than once.");

			Log.Core.Write("Scanning for core plugins...");
			Log.Core.PushIndent();

			foreach (string dllPath in pluginLoader.AvailableAssemblyPaths)
			{
				if (!dllPath.EndsWith(".core.dll", StringComparison.InvariantCultureIgnoreCase))
					continue;

				Log.Core.Write("{0}...", dllPath);
				Log.Core.PushIndent();
				LoadPlugin(dllPath);
				Log.Core.PopIndent();
			}

			Log.Core.PopIndent();
		}
		private static CorePlugin LoadPlugin(string pluginFilePath)
		{
			string asmName = Path.GetFileNameWithoutExtension(pluginFilePath);
			CorePlugin plugin = plugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;

			Assembly pluginAssembly = null;
			try
			{
				pluginAssembly = pluginLoader.LoadAssembly(pluginFilePath, true);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				plugin = null;
			}

			if (pluginAssembly != null)
			{
				plugin = LoadPlugin(pluginAssembly, pluginFilePath);
			}

			return plugin;
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
		public static CorePlugin LoadPlugin(Assembly pluginAssembly, string pluginFilePath)
		{
			disposedPlugins.Remove(pluginAssembly);

			string asmName = pluginAssembly.GetShortAssemblyName();
			CorePlugin plugin = plugins.Values.FirstOrDefault(p => p.AssemblyName == asmName);
			if (plugin != null) return plugin;

			Type pluginType = pluginAssembly.GetExportedTypes().FirstOrDefault(t => typeof(CorePlugin).IsAssignableFrom(t));
			if (pluginType == null)
			{
				Log.Core.WriteWarning("Can't find CorePlugin class. Discarding plugin...");
				disposedPlugins.Add(pluginAssembly);
			}
			else
			{
				try
				{
					plugin = (CorePlugin)pluginType.CreateInstanceOf();
					plugin.FilePath = pluginFilePath;
					plugin.FileHash = pluginLoader.GetAssemblyHash(pluginFilePath);
					plugins.Add(plugin.AssemblyName, plugin);
				}
				catch (Exception e)
				{
					Log.Core.WriteError("Error loading plugin: {0}", Log.Exception(e));
					disposedPlugins.Add(pluginAssembly);
					plugin = null;
				}
			}

			return plugin;
		}
		private static void RemovePlugin(CorePlugin plugin)
		{
			// Dispose plugin and discard plugin related data
			disposedPlugins.Add(plugin.PluginAssembly);
			OnDiscardPluginData();
			plugins.Remove(plugin.AssemblyName);
			try
			{
				plugin.Dispose();
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
			}

			// Discard temporary plugin-related data (cached Types, etc.)
			CleanupAfterPlugins(new[] { plugin });
		}
		private static void InitPlugins()
		{
			Log.Core.Write("Initializing core plugins...");
			Log.Core.PushIndent();
			CorePlugin[] initPlugins = plugins.Values.ToArray();
			foreach (CorePlugin plugin in initPlugins)
			{
				Log.Core.Write("{0}...", plugin.AssemblyName);
				Log.Core.PushIndent();
				InitPlugin(plugin);
				Log.Core.PopIndent();
			}
			Log.Core.PopIndent();
		}
		internal static void InitPlugin(CorePlugin plugin)
		{
			try
			{
				plugin.InitPlugin();
				OnPluginReady(plugin);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error initializing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				RemovePlugin(plugin);
			}
		}
		private static void ClearPlugins()
		{
			foreach (CorePlugin plugin in plugins.Values)
			{
				disposedPlugins.Add(plugin.PluginAssembly);
			}
			OnDiscardPluginData();
			foreach (CorePlugin plugin in plugins.Values)
			{
				try
				{
					plugin.Dispose();
				}
				catch (Exception e)
				{
					Log.Core.WriteError("Error disposing plugin {1}: {0}", Log.Exception(e), plugin.AssemblyName);
				}
			}
			CleanupAfterPlugins(plugins.Values);
			plugins.Clear();
		}
		/// <summary>
		/// Reloads the specified plugin. Does not initialize it.
		/// </summary>
		/// <param name="pluginFilePath"></param>
		internal static CorePlugin ReloadPlugin(string pluginFilePath)
		{
			if (!pluginFilePath.EndsWith(".core.dll", StringComparison.InvariantCultureIgnoreCase))
				return null;

			// If we're trying to reload an active backend plugin, stop
			foreach (var pair in plugins)
			{
				CorePlugin backendPlugin = pair.Value;
				if (PathHelper.ArePathsEqual(backendPlugin.FilePath, pluginFilePath))
				{
					foreach (IDualityBackend backend in activeBackends)
					{
						Type backendType = backend.GetType();
						if (backendPlugin.PluginAssembly == backendType.Assembly)
						{
							Log.Core.WriteError("Can't reload a plugin that contains an active backend: {0}", backend.Name);
							return null;
						}
					}
					break;
				}
			}
			
			// Load the updated plugin Assembly
			Assembly pluginAssembly = null;
			try
			{
				pluginAssembly = pluginLoader.LoadAssembly(pluginFilePath, true);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error loading plugin Assembly: {0}", Log.Exception(e));
				return null;
			}

			// If we're overwriting an old plugin here, add the old version to the "disposed" blacklist
			string assemblyName = pluginAssembly.GetShortAssemblyName();
			CorePlugin oldPlugin;
			if (plugins.TryGetValue(assemblyName, out oldPlugin))
			{
				plugins.Remove(assemblyName);
				disposedPlugins.Add(oldPlugin.PluginAssembly);
				OnDiscardPluginData();
				oldPlugin.Dispose();
			}

			// Load the new plugin from the updated Assembly
			CorePlugin plugin = LoadPlugin(pluginAssembly, pluginFilePath);
			
			// Discard temporary plugin-related data (cached Types, etc.)
			CleanupAfterPlugins(new[] { oldPlugin });

			return plugin;
		}

		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Assembly> GetDualityAssemblies()
		{
			yield return typeof(DualityApp).Assembly;
			foreach (CorePlugin p in LoadedPlugins) yield return p.PluginAssembly;
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
		public static IEnumerable<Type> GetAvailDualityTypes(Type baseType)
		{
			List<Type> availTypes;
			if (availTypeDict.TryGetValue(baseType, out availTypes)) return availTypes;

			availTypes = new List<Type>();
			IEnumerable<Assembly> asmQuery = GetDualityAssemblies();
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

		private static void UpdateUserInput()
		{
			mouse.Update();
			keyboard.Update();
			joysticks.Update();
			gamepads.Update();
		}

		internal static void InitBackend<T>(out T target, Func<Type,IEnumerable<Type>> typeFinder = null) where T : class, IDualityBackend
		{
			if (typeFinder == null) typeFinder = GetAvailDualityTypes;

			Log.Core.Write("Initializing {0}...", Log.Type(typeof(T)));
			Log.Core.PushIndent();

			// Generate a list of available backends for evaluation
			List<IDualityBackend> backends = new List<IDualityBackend>();
			foreach (Type backendType in typeFinder(typeof(IDualityBackend)))
			{
				if (backendType.IsInterface) continue;
				if (backendType.IsAbstract) continue;
				if (!backendType.IsClass) continue;
				if (!typeof(T).IsAssignableFrom(backendType)) continue;

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
				if (appData.SkipBackends != null && appData.SkipBackends.Any(s => string.Equals(s, backend.Id, StringComparison.InvariantCultureIgnoreCase)))
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
				activeBackends.Add(selectedBackend);
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
					activeBackends.Remove(backend);
					backend = null;
				}
				catch (Exception e)
				{
					Log.Core.WriteError("Failed: {0}", Log.Exception(e));
				}
			}
			Log.Core.PopIndent();
		}

		private static void OnBeforeUpdate()
		{
			foreach (CorePlugin plugin in plugins.Values) plugin.OnBeforeUpdate();
		}
		private static void OnAfterUpdate()
		{
			foreach (CorePlugin plugin in plugins.Values) plugin.OnAfterUpdate();
		}
		private static void OnPluginReady(CorePlugin plugin)
		{
			if (PluginReady != null)
				PluginReady(null, new CorePluginEventArgs(plugin));
		}
		private static void OnExecContextChanged(ExecutionContext previousContext)
		{
			foreach (CorePlugin plugin in plugins.Values) plugin.OnExecContextChanged(previousContext);
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
		private static void OnDiscardPluginData()
		{
			if (DiscardPluginData != null)
				DiscardPluginData(null, EventArgs.Empty);

			// Dispose any existing Resources that could reference plugin data
			VisualLog.ClearAll();
			if (!Scene.Current.IsEmpty)
				Scene.Current.Dispose();
			foreach (Resource r in ContentProvider.EnumeratePluginContent().ToArray())
				ContentProvider.RemoveContent(r.Path);
		}
		private static void CleanupAfterPlugins(IEnumerable<CorePlugin> oldPlugins)
		{
			oldPlugins = oldPlugins.NotNull().Distinct();
			if (!oldPlugins.Any()) oldPlugins = null;

			// Clean globally cached type values
			availTypeDict.Clear();
			ImageCodec.ClearTypeCache();
			ObjectCreator.ClearTypeCache();
			ReflectionHelper.ClearTypeCache();
			Component.ClearTypeCache();
			Serializer.ClearTypeCache();
			CloneProvider.ClearTypeCache();
			
			// Clean input sources that a disposed Assembly forgot to unregister.
			if (oldPlugins != null)
			{
				foreach (CorePlugin plugin in oldPlugins)
					CleanInputSources(plugin.PluginAssembly);
			}
			// Clean event bindings that are still linked to the disposed Assembly.
			if (oldPlugins != null)
			{
				foreach (CorePlugin plugin in oldPlugins)
					CleanEventBindings(plugin.PluginAssembly);
			}
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

			if (ReflectionHelper.CleanEventBindings(typeof(DualityApp),			invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)));
			if (ReflectionHelper.CleanEventBindings(typeof(Scene),				invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(Scene)));
			if (ReflectionHelper.CleanEventBindings(typeof(Resource),			invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(Resource)));
			if (ReflectionHelper.CleanEventBindings(typeof(ContentProvider),	invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(ContentProvider)));
			if (ReflectionHelper.CleanEventBindings(Log.LogData,				invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(Log)) + ".LogData");
			if (ReflectionHelper.CleanEventBindings(keyboard,					invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Keyboard");
			if (ReflectionHelper.CleanEventBindings(mouse,						invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Mouse");
			foreach (JoystickInput joystick in joysticks)
			{
				if (ReflectionHelper.CleanEventBindings(joystick,				invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Joysticks");
			}
			foreach (GamepadInput gamepad in gamepads)
			{
				if (ReflectionHelper.CleanEventBindings(gamepad,				invalidAssembly))	Log.Core.WriteWarning(warningText, Log.Type(typeof(DualityApp)) + ".Gamepads");
			}
		}
		private static void CleanInputSources(Assembly invalidAssembly)
		{
			string warningText = string.Format(
				"Found leaked input source '{1}' defined in invalid Assembly '{0}'. " +
				"This is a common problem when registering input sources from within a CorePlugin " +
				"without properly unregistering them later. Please make sure that all sources are " +
				"unregistered in CorePlugin::OnDisposePlugin().",
				invalidAssembly.GetShortAssemblyName(),
				"{0}");

			if (mouse.Source != null && mouse.Source.GetType().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(mouse.Source.GetType()));
				mouse.Source = null;
			}
			if (keyboard.Source != null && keyboard.Source.GetType().Assembly == invalidAssembly)
			{
				Log.Core.WriteWarning(warningText, Log.Type(keyboard.Source.GetType()));
				keyboard.Source = null;
			}
			foreach (JoystickInput joystick in joysticks.ToArray())
			{
				if (joystick.Source != null && joystick.Source.GetType().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(joystick.Source.GetType()));
					joysticks.RemoveSource(joystick.Source);
				}
			}
			foreach (GamepadInput gamepad in gamepads.ToArray())
			{
				if (gamepad.Source != null && gamepad.Source.GetType().Assembly == invalidAssembly)
				{
					Log.Core.WriteWarning(warningText, Log.Type(gamepad.Source.GetType()));
					gamepads.RemoveSource(gamepad.Source);
				}
			}
		}

		private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			Terminate(true);
		}
		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.Core.WriteError(Log.Exception(e.ExceptionObject as Exception));
			if (e.IsTerminating) Terminate(true);
		}
		private static Assembly pluginLoader_ResolveAssembly(ResolveAssemblyEventArgs args)
		{
			// First assume we are searching for a dynamically loaded plugin assembly
			CorePlugin plugin;
			if (plugins.TryGetValue(args.AssemblyName, out plugin))
			{
				return plugin.PluginAssembly;
			}
			// Not there? Search for other libraries in the Plugins folder
			else
			{
				//  Search for plugins that haven't been loaded yet, and load them first
				foreach (string libFile in pluginLoader.AvailableAssemblyPaths)
				{
					string libFileEnding = ".core.dll";
					if (!libFile.EndsWith(libFileEnding, StringComparison.InvariantCultureIgnoreCase))
						continue;

					string libName = libFile.Remove(libFile.Length - libFileEnding.Length, libFileEnding.Length);
					if (libName.Equals(args.AssemblyName, StringComparison.InvariantCultureIgnoreCase))
					{
						plugin = LoadPlugin(libFile);
						if (plugin != null) return plugin.PluginAssembly;
					}
				}

				// Search for other libraries that might be located inside the plugin directory
				foreach (string libFile in pluginLoader.AvailableAssemblyPaths)
				{
					string libName = Path.GetFileNameWithoutExtension(libFile);
					if (libName.Equals(args.AssemblyName, StringComparison.InvariantCultureIgnoreCase))
					{
						return pluginLoader.LoadAssembly(libFile, false);
					}
				}
			}

			// Admit that we didn't find anything.
			return null;
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
