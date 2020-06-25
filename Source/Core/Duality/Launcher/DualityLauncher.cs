using System;
using System.Collections.Generic;
using System.IO;
using Duality.Backend;

namespace Duality.Launcher
{
	public class DualityLauncher : IDisposable
	{
		private readonly LauncherArgs launcherArgs;
		private readonly List<ILogOutput> logOutputs = new List<ILogOutput>();
		private readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
		private readonly INativeWindow window;

		public DualityLauncher(LauncherArgs launcherArgs = null)
		{
			if (launcherArgs == null)
			{
				launcherArgs = new LauncherArgs();
			}

			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
			this.launcherArgs = launcherArgs;

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
				this.launcherArgs);

			// Initialize the Duality core
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Launcher,
				DualityApp.ExecutionContext.Game,
				new DefaultAssemblyLoader(),
				this.launcherArgs);

			// Open up a new window
			WindowOptions options = new WindowOptions
			{
				Size = DualityApp.UserData.WindowSize,
				ScreenMode = this.launcherArgs.IsDebugging ? ScreenMode.Window : DualityApp.UserData.WindowMode,
				RefreshMode = this.launcherArgs.IsProfiling ? RefreshMode.NoSync : DualityApp.UserData.WindowRefreshMode,
				Title = DualityApp.AppData.AppName,
				SystemCursorVisible = this.launcherArgs.IsDebugging || DualityApp.UserData.SystemCursorVisible
			};
			this.window = DualityApp.OpenWindow(options);
		}

		public void Run()
		{
			this.window.Run();
		}

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