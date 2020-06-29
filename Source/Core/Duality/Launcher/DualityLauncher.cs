using System;
using System.Collections.Generic;
using System.IO;
using Duality.Backend;

namespace Duality.Launcher
{
	/// <summary>
	/// A class that allows you to easily initialize duality, run it and clean it up afterwards.
	/// As static state is used under the hood please make sure to only have 1 instance at a time of this class.
	/// </summary>
	public class DualityLauncher : IDisposable
	{
		private readonly List<ILogOutput> logOutputs = new List<ILogOutput>();
		private readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
		private readonly INativeWindow window;

		/// <summary>
		/// Initializes duality but does not yet run it.
		/// </summary>
		/// <param name="launcherArgs"></param>
		public DualityLauncher(LauncherArgs launcherArgs = null)
		{
			if (launcherArgs == null)
			{
				launcherArgs = new LauncherArgs();
			}

			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

			// Set up console logging
			this.AddGlobalOutput(new ConsoleLogOutput());

			// Set up file logging
			try
			{
				StreamWriter logfileWriter = new StreamWriter("logfile.txt");
				logfileWriter.AutoFlush = true;
				this.disposables.Push(logfileWriter);

				this.AddGlobalOutput(new TextWriterLogOutput(logfileWriter));
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning("Text Logfile unavailable: {0}", LogFormat.Exception(e));
			}

			// Set up a global exception handler to log errors
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			// Write initial log message before actually booting Duality
			Logs.Core.Write("Running DualityLauncher with flags: {1}{0}",
				Environment.NewLine,
				launcherArgs);

			// Initialize the Duality core
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Launcher,
				DualityApp.ExecutionContext.Game,
				new DefaultAssemblyLoader(),
				launcherArgs);

			// Open up a new window
			WindowOptions options = new WindowOptions
			{
				Size = DualityApp.UserData.Value.WindowSize,
				ScreenMode = launcherArgs.IsDebugging ? ScreenMode.Window : DualityApp.UserData.Value.WindowMode,
				RefreshMode = launcherArgs.IsProfiling ? RefreshMode.NoSync : DualityApp.UserData.Value.WindowRefreshMode,
				Title = DualityApp.AppData.Value.AppName,
				SystemCursorVisible = launcherArgs.IsDebugging || DualityApp.UserData.Value.SystemCursorVisible
			};
			this.window = DualityApp.OpenWindow(options);
		}

		/// <summary>
		/// Runs duality. This will block till the game ends.
		/// Don't call this if you want full control of the update loop (such as in unit tests).
		/// </summary>
		public void Run()
		{
			this.window.Run();
		}

		/// <summary>
		/// Adds a global log output and also makes sure its removed when <see cref="Dispose"/> is called
		/// </summary>
		/// <param name="logOutput"></param>
		public void AddGlobalOutput(ILogOutput logOutput)
		{
			this.logOutputs.Add(logOutput);
			Logs.AddGlobalOutput(logOutput);
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Logs.Core.WriteError(LogFormat.Exception(e.ExceptionObject as Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}

		public void Dispose()
		{
			this.window.Dispose();

			// Shut down the Duality core
			DualityApp.Terminate();

			AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

			foreach (ILogOutput logOutput in this.logOutputs)
			{
				Logs.RemoveGlobalOutput(logOutput);
			}
			this.logOutputs.Clear();

			foreach (IDisposable disposable in this.disposables)
			{
				disposable.Dispose();
			}
			this.disposables.Clear();
		}
	}
}