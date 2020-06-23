using System;
using System.IO;
using Duality.Backend;
using Duality.Resources;

namespace Duality.Launcher
{
	public class DualityLauncher
	{
		public static void Run(LauncherArgs launcherArgs)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

			// Set up console logging
			Logs.AddGlobalOutput(new ConsoleLogOutput());

			// Set up file logging
			StreamWriter logfileWriter = null;
			TextWriterLogOutput logfileOutput = null;
			try
			{
				logfileWriter = new StreamWriter("logfile.txt");
				logfileWriter.AutoFlush = true;
				logfileOutput = new TextWriterLogOutput(logfileWriter);
				Logs.AddGlobalOutput(logfileOutput);
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
				Size = DualityApp.UserData.WindowSize,
				ScreenMode = launcherArgs.IsDebugging ? ScreenMode.Window : DualityApp.UserData.WindowMode,
				RefreshMode = (launcherArgs.IsDebugging || launcherArgs.IsProfiling) ? RefreshMode.NoSync : DualityApp.UserData.WindowRefreshMode,
				Title = DualityApp.AppData.AppName,
				SystemCursorVisible = launcherArgs.IsDebugging || DualityApp.UserData.SystemCursorVisible
			};
			using (INativeWindow window = DualityApp.OpenWindow(options))
			{
				// Load the starting Scene
				Scene.SwitchTo(DualityApp.AppData.StartScene);

				// Enter the applications update / render loop
				window.Run();

				// Shut down the Duality core
				DualityApp.Terminate();
			}

			// Clean up the log file
			if (logfileWriter != null)
			{
				Logs.RemoveGlobalOutput(logfileOutput);
				logfileWriter.Flush();
				logfileWriter.Close();
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Logs.Core.WriteError(LogFormat.Exception(e.ExceptionObject as Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}
	}
}