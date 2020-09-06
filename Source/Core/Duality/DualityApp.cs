﻿using System;
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
using Duality.Launcher;

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

		public const string PluginDirectory = "Plugins";
		public const string DataDirectory   = "Data";


		private static bool                    initialized        = false;
		private static bool                    isUpdating         = false;
		private static bool                    runFromEditor      = false;
		private static bool                    terminateScheduled = false;
		private static IAssemblyLoader         assemblyLoader     = null;
		private static CorePluginManager       pluginManager      = new CorePluginManager();
		private static ISystemBackend          systemBack         = null;
		private static IGraphicsBackend        graphicsBack       = null;
		private static IAudioBackend           audioBack          = null;
		private static Point2                  windowSize         = Point2.Zero;
		private static MouseInput              mouse              = new MouseInput();
		private static KeyboardInput           keyboard           = new KeyboardInput();
		private static JoystickInputCollection joysticks          = new JoystickInputCollection();
		private static GamepadInputCollection  gamepads           = new GamepadInputCollection();
		private static SoundDevice             sound              = null;
		private static ExecutionEnvironment    environment        = ExecutionEnvironment.Unknown;
		private static ExecutionContext        execContext        = ExecutionContext.Terminated;
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
		/// Called when Duality is being terminated by choice (e.g. not because of crashes or similar).
		/// It is also called in an editor environment.
		/// </summary>
		public static event EventHandler Terminating = null;

		
		/// <summary>
		/// [GET] The plugin manager that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// If you want to load a plugin, use the <see cref="CorePluginManager"/> from this property.
		/// If you want to load a non-plugin Assembly, use the <see cref="AssemblyLoader"/>.
		/// </summary>
		public static CorePluginManager PluginManager
		{
			get { return pluginManager; }
		}
		/// <summary>
		/// [GET] The plugin loader that is used by Duality. Don't use this unless you know exactly what you're doing.
		/// If you want to load a plugin, use the <see cref="PluginManager"/>. 
		/// If you want to load a non-plugin Assembly, use the <see cref="IAssemblyLoader"/> from this property.
		/// </summary>
		public static IAssemblyLoader AssemblyLoader
		{
			get { return assemblyLoader; }
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
		/// [GET / SET] The native client area size of the current game window in pixels. 
		/// 
		/// Note: Setting this will not actually change Duality's state - this is a pure 
		/// "for your information" property that is set by the currently active backend.
		/// To change window size at runtime, use <see cref="UserData"/>.
		/// </summary>
		public static Point2 WindowSize
		{
			get { return windowSize; }
			set { windowSize = value; }
		}
		/// <summary>
		/// [GET] The target resolution of the game in rendering image space, e.g. the view
		/// size when rendering to the game window.
		/// </summary>
		public static Vector2 TargetViewSize
		{
			get
			{
				Point2 forcedRenderSize = DualityApp.AppData.Instance.ForcedRenderSize;
				if (forcedRenderSize.X > 0 && forcedRenderSize.Y > 0)
					return forcedRenderSize;
				else
					return windowSize;
			}
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
		/// [GET] Provides access to core application / game config, such as physics or backend settings. This is never null.
		/// </summary>
		public static SettingsContainer<DualityAppData> AppData { get; } = new SettingsContainer<DualityAppData>("AppData.xml");
		/// <summary>
		/// [GET] Provides access to game options / user data, such as display resolution or audio volume. This is never null.
		/// </summary>
		public static SettingsContainer<DualityUserData> UserData { get; } = new SettingsContainer<DualityUserData>("UserData.xml", LoadUserData, SaveUserData);
		/// <summary>
		/// [GET] Returns the <see cref="ExecutionContext"/> in which this DualityApp is currently running.
		/// </summary>
		public static ExecutionContext ExecContext
		{
			get { return execContext; }
		}
		/// <summary>
		/// [GET] Returns the <see cref="ExecutionEnvironment"/> in which this DualityApp is currently running.
		/// </summary>
		public static ExecutionEnvironment ExecEnvironment
		{
			get { return environment; }
		}


		/// <summary>
		/// Initializes this DualityApp. Should be called before performing any operations within Duality.
		/// </summary>
		/// <param name="env"></param>
		/// <param name="context">The <see cref="ExecutionContext"/> in which Duality runs.</param>
		/// <param name="plugins"></param>
		/// <param name="launcherArgs">
		/// Command line arguments to run this DualityApp with. 
		/// Usually these are just the ones from the host application, passed on.
		/// </param>
		public static void Init(ExecutionEnvironment env, ExecutionContext context, IAssemblyLoader plugins, LauncherArgs launcherArgs)
		{
			if (initialized) return;

			// Process command line options
			if (launcherArgs.IsDebugging) System.Diagnostics.Debugger.Launch();
			// Run from editor
			if (launcherArgs.IsRunFromEditor) runFromEditor = true;

			// If the core was compiled in debug mode and a debugger is attached, log 
			// to the Debug channel, so we can put the VS output window to good use.
#if DEBUG
			bool isDebugging = System.Diagnostics.Debugger.IsAttached;
			if (isDebugging)
			{
				// Only add a new Debug output if we don't already have one, and don't
				// log to a Console channel either. VS will automatically redirect Console
				// output to the Output window when debugging a non-Console application,
				// and we don't want to end up with double log entries.
				bool hasDebugOut = Logs.GlobalOutput.OfType<DebugLogOutput>().Any();
				bool hasConsoleOut = Logs.GlobalOutput.OfType<TextWriterLogOutput>().Any(w => w.GetType().Name.Contains("Console"));
				if (!hasDebugOut && !hasConsoleOut)
				{
					Logs.AddGlobalOutput(new DebugLogOutput());
				}
			}
			#endif

			environment = env;
			execContext = context;

			// Initialize the plugin manager
			{
				assemblyLoader = plugins ?? new Duality.Backend.Dummy.DummyAssemblyLoader();
				Logs.Core.Write("Using '{0}' to load plugins.", assemblyLoader.GetType().Name);

				assemblyLoader.Init();

				// Log assembly loading data for diagnostic purposes
				{
					Logs.Core.Write("Currently Loaded Assemblies:" + Environment.NewLine + "{0}",
						assemblyLoader.LoadedAssemblies.ToString(
							assembly => "  " + LogFormat.Assembly(assembly),
							Environment.NewLine));
					Logs.Core.Write("Plugin Base Directories:" + Environment.NewLine + "{0}",
						assemblyLoader.BaseDirectories.ToString(
							path => "  " + path,
							Environment.NewLine));
					Logs.Core.Write("Available Assembly Paths:" + Environment.NewLine + "{0}",
						assemblyLoader.AvailableAssemblyPaths.ToString(
							path => "  " + path,
							Environment.NewLine));
				}

				pluginManager.Init(assemblyLoader);
				pluginManager.PluginsRemoving += pluginManager_PluginsRemoving;
				pluginManager.PluginsRemoved += pluginManager_PluginsRemoved;
			}

			// Load all plugins. This needs to be done first, so backends and Types can be located.
			pluginManager.LoadPlugins();

			// Initialize the system backend for system info and file system access
			InitBackend(out systemBack);

			// Load application and user data and submit a change event, so all settings are applied
			DualityApp.AppData.Load();
			DualityApp.UserData.Load();

			// Initialize the graphics backend
			InitBackend(out graphicsBack);

			// Initialize the audio backend
			InitBackend(out audioBack);
			sound = new SoundDevice();

			// Initialize all core plugins, this may allocate Resources or establish references between plugins
			pluginManager.InitPlugins();
			
			initialized = true;

			// Write environment specs as a debug log
			Logs.Core.Write(
				"DualityApp initialized" + Environment.NewLine +
				"Debug Mode: {0}" + Environment.NewLine +
				"Command line arguments: {1}",
				System.Diagnostics.Debugger.IsAttached,
				launcherArgs.ToString());
		}
		/// <summary>
		/// Opens up a window for Duality to render into. This also initializes the part of Duality that requires a 
		/// valid rendering context. Should be called before performing any rendering related operations with Duality.
		/// </summary>
		public static INativeWindow OpenWindow(WindowOptions options)
		{
			if (!initialized) throw new InvalidOperationException("Can't initialize graphics / rendering because Duality itself isn't initialized yet.");

			Logs.Core.Write("Opening Window...");
			Logs.Core.PushIndent();
			INativeWindow window = graphicsBack.CreateWindow(options);
			Logs.Core.PopIndent();

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
			DefaultContent.Init();

			// Post-Window init is the last thing that happens before loading game
			// content and entering simulation. When done in a game context, notify
			// plugins that the game is about to start - otherwise, exec context changes
			// will trigger the same code later.
			if (execContext == ExecutionContext.Game)
				pluginManager.InvokeGameStarting();
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
				Logs.Core.Write("DualityApp terminated in sandbox mode.");
				terminateScheduled = false;
				return;
			}

			if (execContext != ExecutionContext.Editor)
			{
				OnTerminating();
				DualityApp.UserData.Save();
			}

			// Signal that the game simulation has ended.
			if (execContext == ExecutionContext.Game)
				pluginManager.InvokeGameEnded();

			// Dispose all content that is still loaded
			ContentProvider.ClearContent();

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
			pluginManager.PluginsRemoving -= pluginManager_PluginsRemoving;
			pluginManager.PluginsRemoved -= pluginManager_PluginsRemoved;
			assemblyLoader.Terminate();
			assemblyLoader = null;

			Logs.Core.Write("DualityApp terminated");

			initialized = false;
			execContext = ExecutionContext.Terminated;
		}

		/// <summary>
		/// Switches to a different <see cref="ExecContext"/> at runtime. Intended for the editor
		/// sandbox and similar features.
		/// </summary>
		/// <param name="targetContext"></param>
		internal static void SwitchExecutionContext(ExecutionContext targetContext)
		{
			if (execContext == targetContext) return;

			ExecutionContext previousContext = execContext;
			execContext = targetContext;

			if (previousContext == ExecutionContext.Game && targetContext != ExecutionContext.Game)
				pluginManager.InvokeGameEnded();

			pluginManager.InvokeExecContextChanged(previousContext);

			// Reset time tracking / tick counters, so we start back at frame and game time zero
			Time.Reset();

			if (previousContext != ExecutionContext.Game && targetContext == ExecutionContext.Game)
				pluginManager.InvokeGameStarting();
		}

		/// <summary>
		/// Specialized loading routine for <see cref="UserData"/>.
		/// </summary>
		private static DualityUserData LoadUserData()
		{
			string path = UserData.Path;

			// Load from editor-specified defaults when running in or from the editor, and as a runtime fallback
			if (!FileOp.Exists(path) || execContext == ExecutionContext.Editor || runFromEditor)
				path = "DefaultUserData.xml";

			return Serializer.TryReadObject<DualityUserData>(path, typeof(XmlSerializer));
		}

		/// <summary>
		/// Specialized saving routine for <see cref="UserData"/>.
		/// </summary>
		private static void SaveUserData(DualityUserData instance)
		{
			Serializer.WriteObject(instance, UserData.Path, typeof(XmlSerializer));

			// When in the editor, always write both default settings and current game / runtime settings
			if (execContext == ExecutionContext.Editor)
			{
				Serializer.WriteObject(instance, "DefaultUserData.xml", typeof(XmlSerializer));
			}
		}

		/// <summary>
		/// Performs a single update cycle.
		/// </summary>
		public static void Update()
		{
			Update(false);
		}

		/// <summary>
		/// Performs a single update cycle.
		/// </summary>
		/// <param name="forceFixedStep">If true use a timestep thats equal to <see cref="Time.MillisecondsPerFrame"/> for the update</param>
		public static void Update(bool forceFixedStep)
		{
			isUpdating = true;
			Profile.TimeUpdate.BeginMeasure();

			Time.FrameTick(forceFixedStep, true);
			Profile.FrameTick();
			VisualLogs.UpdateLogEntries();
			pluginManager.InvokeBeforeUpdate();
			UpdateUserInput();
			Scene.Current.Update();
			sound.Update();
			pluginManager.InvokeAfterUpdate();
			VisualLogs.PrepareRenderLogEntries();

			// Perform a cleanup step to catch all DisposeLater calls from this update
			RunCleanup();

			// Perform any previously scheduled Scene switch
			Scene.PerformScheduledSwitch();

			Profile.TimeUpdate.EndMeasure();
			isUpdating = false;

			if (terminateScheduled) Terminate();
		}
		internal static void EditorUpdate(IEnumerable<GameObject> updateObjects, bool simulateGame, bool forceFixedStep)
		{
			isUpdating = true;
			Profile.TimeUpdate.BeginMeasure();

			Time.FrameTick(forceFixedStep, simulateGame);
			Profile.FrameTick();

			if (simulateGame)
			{
				VisualLogs.UpdateLogEntries();
				pluginManager.InvokeBeforeUpdate();

				UpdateUserInput();
				Scene.Current.Update();

				List<ICmpUpdatable> updatables = new List<ICmpUpdatable>();
				foreach (GameObject obj in updateObjects)
				{
					if (obj.Scene == Scene.Current)
						continue;
					
					updatables.Clear();
					obj.GetComponents(updatables);
					for (int i = 0; i < updatables.Count; i++)
					{
						if (!(updatables[i] as Component).Active) continue;
						updatables[i].OnUpdate();
					}
				}

				pluginManager.InvokeAfterUpdate();
			}
			else
			{
				Scene.Current.EditorUpdate();

				List<ICmpUpdatable> updatables = new List<ICmpUpdatable>();
				foreach (GameObject obj in updateObjects)
				{
					updatables.Clear();
					obj.GetComponents(updatables);
					for (int i = 0; i < updatables.Count; i++)
					{
						if (!(updatables[i] as Component).Active) continue;
						updatables[i].OnUpdate();
					}
				}
			}

			sound.Update();
			VisualLogs.PrepareRenderLogEntries();

			// Perform a cleanup step to catch all DisposeLater calls from this update
			RunCleanup();

			if (simulateGame)
			{
				// Perform any previously scheduled Scene switch
				Scene.PerformScheduledSwitch();
			}

			Profile.TimeUpdate.EndMeasure();
			isUpdating = false;

			if (terminateScheduled) Terminate();
		}
		/// <summary>
		/// Performs a single render cycle.
		/// </summary>
		/// <param name="target">
		/// The <see cref="RenderTarget"/> which will be used for all rendering output. 
		/// "null" means rendering directly to the output buffer of the game window / screen.
		/// </param>
		/// <param name="viewportRect">The viewport to render to, in pixel coordinates.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		public static void Render(ContentRef<RenderTarget> target, Rect viewportRect, Vector2 imageSize)
		{
			Scene.Current.Render(target, viewportRect, imageSize);
		}

		/// <summary>
		/// Given the specified window size, this method calculates the window rectangle of the rendered
		/// viewport, as well as the game's rendered image size while taking into account application settings
		/// regarding forced rendering sizes.
		/// </summary>
		/// <param name="windowSize"></param>
		/// <param name="windowViewport"></param>
		/// <param name="renderTargetSize"></param>
		public static void CalculateGameViewport(Point2 windowSize, out Rect windowViewport, out Vector2 renderTargetSize)
		{
			Point2 forcedSize = DualityApp.AppData.Instance.ForcedRenderSize;
			TargetResize forcedResizeMode = DualityApp.AppData.Instance.ForcedRenderResizeMode;

			renderTargetSize = windowSize;
			windowViewport = new Rect(renderTargetSize);

			bool forcedResizeActive = 
				forcedResizeMode != TargetResize.None && 
				forcedSize.X > 0 && forcedSize.Y > 0 && 
				forcedSize != renderTargetSize;
			if (forcedResizeActive)
			{
				Vector2 adjustedViewportSize = forcedResizeMode.Apply(forcedSize, windowViewport.Size);

				// Clip viewport and target size, so they don't exceed the window size.
				// This, strictly speaking, violates the forced rendering size, but for
				// resize modes like Fill, there is no other way to solve this.
				if (adjustedViewportSize.X > windowSize.X)
				{
					forcedSize.X = MathF.RoundToInt((float)forcedSize.X * (float)windowSize.X / (float)adjustedViewportSize.X);
					adjustedViewportSize.X = windowSize.X;
				}
				if (adjustedViewportSize.Y > windowSize.Y)
				{
					forcedSize.Y = MathF.RoundToInt((float)forcedSize.Y * (float)windowSize.Y / (float)adjustedViewportSize.Y);
					adjustedViewportSize.Y = windowSize.Y;
				}

				renderTargetSize = forcedSize;
				windowViewport = Rect.Align(
					Alignment.Center, 
					windowViewport.Size.X * 0.5f, 
					windowViewport.Size.Y * 0.5f, 
					adjustedViewportSize.X, 
					adjustedViewportSize.Y);
			}
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
		/// end of each <see cref="Update()">frame update</see> and you shouldn't need to call this in general.
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
		}

		/// <summary>
		/// Enumerates all currently loaded assemblies that are part of Duality, i.e. Duality itsself and all loaded plugins.
		/// </summary>
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
		/// 	Logs.Core.Write("Renderer Type '{0}' from Assembly '{1}'", LogFormat.Type(rt), rt.Assembly.FullName);
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

			Logs.Core.Write("Initializing {0}...", LogFormat.Type(typeof(T)));
			Logs.Core.PushIndent();

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
					Logs.Core.WriteWarning("Unable to create an instance of {0}. Skipping it.", backendType.FullName);
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
				if (DualityApp.AppData.Instance?.SkipBackends != null && 
					DualityApp.AppData.Instance.SkipBackends.Any(s => string.Equals(s, backend.Id, StringComparison.OrdinalIgnoreCase)))
				{
					Logs.Core.Write("Backend '{0}' skipped because of AppData settings.", backend.Name);
					continue;
				}

				bool available = false;
				try
				{
					available = backend.CheckAvailable();
					if (!available)
					{
						Logs.Core.Write("Backend '{0}' reports to be unavailable. Skipping it.", backend.Name);
					}
				}
				catch (Exception e)
				{
					available = false;
					Logs.Core.WriteWarning("Backend '{0}' failed the availability check with an exception: {1}", backend.Name, LogFormat.Exception(e));
				}
				if (!available) continue;

				Logs.Core.Write("{0}...", backend.Name);
				Logs.Core.PushIndent();
				{
					try
					{
						backend.Init();
						selectedBackend = backend;
					}
					catch (Exception e)
					{
						Logs.Core.WriteError("Failed: {0}", LogFormat.Exception(e));
					}
				}
				Logs.Core.PopIndent();

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

			Logs.Core.PopIndent();
		}
		internal static void ShutdownBackend<T>(ref T backend) where T : class, IDualityBackend
		{
			if (backend == null) return;

			Logs.Core.Write("Shutting down {0}...", backend.Name);
			Logs.Core.PushIndent();
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
					Logs.Core.WriteError("Failed: {0}", LogFormat.Exception(e));
				}
			}
			Logs.Core.PopIndent();
		}

		private static void OnTerminating()
		{
			if (Terminating != null)
				Terminating(null, EventArgs.Empty);
		}
		
		private static void pluginManager_PluginsRemoving(object sender, DualityPluginEventArgs e)
		{
			// Save user and app data, they'll be reloaded after plugin reload is done,
			// as they can reference plugin data as well.
			DualityApp.UserData.Save();
			DualityApp.AppData.Save();

			// Dispose static Resources that could reference plugin data
			VisualLogs.ClearAll();
			if (!Scene.Current.IsEmpty)
				Scene.Current.Dispose();

			// Gather all other Resources that could reference plugin data
			List<Resource> pluginContent = new List<Resource>();
			Assembly coreAssembly = typeof(Resource).GetTypeInfo().Assembly;
			foreach (Resource resource in ContentProvider.GetLoadedContent<Resource>())
			{
				if (resource.IsDefaultContent) continue;

				Assembly assembly = resource.GetType().GetTypeInfo().Assembly;
				bool canReferencePluginData =
					resource is Prefab || 
					resource is Scene || 
					assembly != coreAssembly;

				if (canReferencePluginData)
					pluginContent.Add(resource);
			}

			// Dispose gathered content to avoid carrying over old instances by accident
			foreach (Resource r in pluginContent)
				ContentProvider.RemoveContent(r);
		}
		private static void pluginManager_PluginsRemoved(object sender, DualityPluginEventArgs e)
		{
			// Clean globally cached type data
			ImageCodec.ClearTypeCache();
			ObjectCreator.ClearTypeCache();
			ReflectionHelper.ClearTypeCache();
			Component.RequireMap.ClearTypeCache();
			Component.ExecOrder.ClearTypeCache();
			Serializer.ClearTypeCache();
			CloneProvider.ClearTypeCache();

			// Clean input sources that a disposed Assembly forgot to unregister.
			foreach (CorePlugin plugin in e.Plugins)
				CleanInputSources(plugin.PluginAssembly);

			// Clean event bindings that are still linked to the disposed Assembly.
			foreach (CorePlugin plugin in e.Plugins)
				CleanEventBindings(plugin.PluginAssembly);

			// Reload user and app data
			DualityApp.AppData.Load();
			DualityApp.UserData.Load();
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

			if (ReflectionHelper.CleanEventBindings(typeof(DualityApp),      invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(DualityApp)));
			if (ReflectionHelper.CleanEventBindings(typeof(Scene),           invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(Scene)));
			if (ReflectionHelper.CleanEventBindings(typeof(Resource),        invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(Resource)));
			if (ReflectionHelper.CleanEventBindings(typeof(ContentProvider), invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(ContentProvider)));
			if (ReflectionHelper.CleanEventBindings(DualityApp.Keyboard,     invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(DualityApp)) + ".Keyboard");
			if (ReflectionHelper.CleanEventBindings(DualityApp.Mouse,        invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(DualityApp)) + ".Mouse");
			foreach (JoystickInput joystick in DualityApp.Joysticks)
				if (ReflectionHelper.CleanEventBindings(joystick,            invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(DualityApp)) + ".Joysticks");
			foreach (GamepadInput gamepad in DualityApp.Gamepads)
				if (ReflectionHelper.CleanEventBindings(gamepad,             invalidAssembly)) Logs.Core.WriteWarning(warningText, LogFormat.Type(typeof(DualityApp)) + ".Gamepads");
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
				Logs.Core.WriteWarning(warningText, LogFormat.Type(DualityApp.Mouse.Source.GetType()));
				DualityApp.Mouse.Source = null;
			}
			if (DualityApp.Keyboard.Source != null && DualityApp.Keyboard.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
			{
				Logs.Core.WriteWarning(warningText, LogFormat.Type(DualityApp.Keyboard.Source.GetType()));
				DualityApp.Keyboard.Source = null;
			}
			foreach (JoystickInput joystick in DualityApp.Joysticks.ToArray())
			{
				if (joystick.Source != null && joystick.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Logs.Core.WriteWarning(warningText, LogFormat.Type(joystick.Source.GetType()));
					DualityApp.Joysticks.RemoveSource(joystick.Source);
				}
			}
			foreach (GamepadInput gamepad in DualityApp.Gamepads.ToArray())
			{
				if (gamepad.Source != null && gamepad.Source.GetType().GetTypeInfo().Assembly == invalidAssembly)
				{
					Logs.Core.WriteWarning(warningText, LogFormat.Type(gamepad.Source.GetType()));
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
					Logs.Editor.WriteError("An error occurred: {0}", LogFormat.Exception(e));
				}
			}
			else
			{
				action();
			}
		}
	}
}
