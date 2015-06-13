using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

using Duality;
using Duality.Resources;
using Duality.Backend;

namespace Duality.Launcher
{
	internal static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

			bool isDebugging = System.Diagnostics.Debugger.IsAttached || args.Contains(DualityApp.CmdArgDebug);
			bool isRunFromEditor = args.Contains(DualityApp.CmdArgEditor);
			bool isProfiling = args.Contains(DualityApp.CmdArgProfiling);
			if (isDebugging || isRunFromEditor) ShowConsole();
			
			// Set up console logging
			Log.AddGlobalOutput(new ConsoleLogOutput());

			// Set up file logging
			StreamWriter logfileWriter = null;
			TextWriterLogOutput logfileOutput = null;
			try
			{
				logfileWriter = new StreamWriter("logfile.txt");
				logfileWriter.AutoFlush = true;
				logfileOutput = new TextWriterLogOutput(logfileWriter);
				Log.AddGlobalOutput(logfileOutput);
			}
			catch (Exception e)
			{
				Log.Core.WriteWarning("Text Logfile unavailable: {0}", Log.Exception(e));
			}

			// Write initial log message before actually booting Duality
			Log.Core.Write("Running DualityLauncher with flags: {1}{0}", 
				Environment.NewLine,
				new[] { isDebugging ? "Debugging" : null, isProfiling ? "Profiling" : null, isRunFromEditor ? "RunFromEditor" : null }.NotNull().ToString(", "));

			// Initialize the Duality core
			DualityApp.Init(DualityApp.ExecutionEnvironment.Launcher, DualityApp.ExecutionContext.Game, args);
			
			// Open up a new window
			WindowOptions options = new WindowOptions
			{
				Width = DualityApp.UserData.GfxWidth,
				Height = DualityApp.UserData.GfxHeight,
				ScreenMode = isDebugging ? ScreenMode.Window : DualityApp.UserData.GfxMode,
				RefreshMode = (isDebugging || isProfiling) ? RefreshMode.NoSync : DualityApp.UserData.RefreshMode,
				Title = DualityApp.AppData.AppName,
				SystemCursorVisible = isDebugging || DualityApp.UserData.SystemCursorVisible
			};
			using (INativeWindow window = DualityApp.OpenWindow(options))
			{
				// Load the starting Scene
				Scene.SwitchTo(DualityApp.AppData.StartScene);

				// Enter the applications update / render loop
				window.Run();
			}

			// Shut down the Duality core
			DualityApp.Terminate();
			
			// Clean up the log file
			if (logfileWriter != null)
			{
				Log.RemoveGlobalOutput(logfileOutput);
				logfileWriter.Flush();
				logfileWriter.Close();
				logfileWriter = null;
				logfileOutput = null;
			}
		}

		private static bool hasConsole = false;
		public static void ShowConsole()
		{
			if (hasConsole) return;
			SafeNativeMethods.AllocConsole();
			hasConsole = true;
		}
	}
}
