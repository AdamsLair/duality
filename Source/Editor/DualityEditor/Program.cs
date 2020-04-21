using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

namespace Duality.Editor
{
	internal static class Program
	{
		private const string DualityMainLicenseUrl = @"https://github.com/AdamsLair/duality/raw/master/LICENSE";

		private static StreamWriter logfileWriter;
		private static TextWriterLogOutput logfileOutput;
		private static bool recoverFromPluginReload;


		[STAThread]
		private static void Main(string[] args)
		{
			// Parse command line arguments
			ParseCommandLineArguments(args);

			// Culture setup
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

			// Set up a text logfile
			ArchiveOldLogfile();
			CreateLogfile();

			// Winforms Setup
			PrepareWinFormsApplication();
			
			// Run the editor
			SplashScreen splashScreen = new SplashScreen(recoverFromPluginReload);
			splashScreen.Show();
			Application.Run();

			// Clean up the log file
			CloseLogfile();
		}

		private static void ParseCommandLineArguments(string[] args)
		{
			recoverFromPluginReload = false;
			foreach (string argument in args)
			{
				if (argument == "debug")
					System.Diagnostics.Debugger.Launch();
				else if (argument == "recover")
					recoverFromPluginReload = true;
			}
		}
		private static void PrepareWinFormsApplication()
		{
			Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += Application_ThreadException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}

		private static void ArchiveOldLogfile()
		{
			try
			{
				// If there is an existing logfile, archive it for diagnostic purposes
				FileInfo prevLogfile = new FileInfo(DualityEditorApp.EditorLogfilePath);
				if (prevLogfile.Exists)
				{
					if (!Directory.Exists(DualityEditorApp.EditorPrevLogfileDir))
						Directory.CreateDirectory(DualityEditorApp.EditorPrevLogfileDir);

					string timestampToken = prevLogfile.LastWriteTimeUtc.ToString("yyyy-MM-dd-T-HH-mm-ss");
					string prevLogfileName = string.Format(DualityEditorApp.EditorPrevLogfileName, timestampToken);
					string prevLogFilePath = Path.Combine(DualityEditorApp.EditorPrevLogfileDir, prevLogfileName);

					prevLogfile.MoveTo(prevLogFilePath);
				}
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning("Unable to archive old logfile: {0}", LogFormat.Exception(e));
			}
		}
		private static void CreateLogfile()
		{
			if (logfileOutput != null || logfileWriter != null)
				CloseLogfile();

			try
			{
				logfileWriter = new StreamWriter(DualityEditorApp.EditorLogfilePath);
				logfileWriter.AutoFlush = true;
				logfileOutput = new TextWriterLogOutput(logfileWriter);
				Logs.AddGlobalOutput(logfileOutput);
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning("Unable to create logfile: {0}", LogFormat.Exception(e));
			}
		}
		private static void CloseLogfile()
		{
			if (logfileOutput != null)
			{
				Logs.RemoveGlobalOutput(logfileOutput);
				logfileOutput = null;
			}
			if (logfileWriter != null)
			{
				logfileWriter.Flush();
				logfileWriter.Close();
				logfileWriter = null;
			}
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				Logs.Editor.WriteError(LogFormat.Exception(e.ExceptionObject as Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Logs.Editor.WriteError(LogFormat.Exception(e.Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}
	}
}
