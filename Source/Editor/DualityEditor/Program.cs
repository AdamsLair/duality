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
using Duality.Editor.PackageManagement;

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
			
			// Restore or remove packages to match package config
			VerifyPackageSetup();

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
		private static void VerifyPackageSetup()
		{
			PackageManager packageManager = new PackageManager();

			// On the first install startup, display a generic license agreement for Duality
			if (packageManager.IsFirstInstall)
			{
				LicenseAcceptDialog licenseDialog = new LicenseAcceptDialog
				{
					DescriptionText = GeneralRes.LicenseAcceptDialog_FirstStartGeneric,
					LicenseUrl = new Uri(DualityMainLicenseUrl)
				};
				DialogResult result = licenseDialog.ShowDialog();
				if (result != DialogResult.OK)
				{
					Application.Exit();
					return;
				}
			}

			// Perform the initial package update - even before initializing the editor
			if (packageManager.IsPackageSyncRequired)
			{
				Log.Editor.Write("Updating Packages...");
				Log.Editor.PushIndent();
				ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
					GeneralRes.TaskInstallPackages_Caption, 
					GeneralRes.TaskInstallPackages_Desc, 
					SynchronizePackages, 
					packageManager);
				setupDialog.ShowInTaskbar = true;
				setupDialog.MainThreadRequired = false;
				setupDialog.ShowDialog();
				Log.Editor.PopIndent();
			}
			// Restart to apply the update
			if (packageManager.ApplyUpdate())
			{
				Application.Exit();
				return;
			}
			// If we have nothing to apply, but still require a sync, something went wrong.
			// Should this happen on our first start, we'll remind the user that the install
			// requires an internet connection and refuse to start.
			else if (packageManager.IsPackageSyncRequired && packageManager.IsFirstInstall)
			{
				DialogResult result = MessageBox.Show( 
					GeneralRes.Msg_ErrorFirstDualityInstall_Desc, 
					GeneralRes.Msg_ErrorFirstDualityInstall_Caption, 
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				Application.Exit();
				return;
			}
		}
		private static IEnumerable SynchronizePackages(ProcessingBigTaskDialog.WorkerInterface workerInterface)
		{
			PackageManager manager = workerInterface.Data as PackageManager;

			// Set the working state and yield, so the UI can update properly in case we're in the main thread
			workerInterface.Progress = 0.0f;
			workerInterface.StateDesc = GeneralRes.TaskPrepareInfo;
			yield return null;

			// Retrieve all registered Duality packages and sort them so we don't accidentally install an old dependency
			LocalPackage[] packagesToVerify = manager.LocalPackages.ToArray();
			manager.OrderByDependencies(packagesToVerify);
			yield return null;

			// Uninstall all "shadow" Duality packages that are installed, but not registered
			Log.Editor.Write("Uninstalling unregistered packages...");
			Log.Editor.PushIndent();
			manager.UninstallNonRegisteredPackages();
			Log.Editor.PopIndent();
			yield return null;

			// Iterate over previously reigstered local packages and verify / install them.
			Log.Editor.Write("Verifying registered packages...");
			Log.Editor.PushIndent();
			foreach (LocalPackage package in packagesToVerify)
			{
				// Update the task dialog's UI
				if (package.Version != null)
					workerInterface.StateDesc = string.Format("Package '{0}', Version {1}...", package.Id, package.Version);
				else
					workerInterface.StateDesc = string.Format("Package '{0}'...", package.Id);
				workerInterface.Progress += 0.5f / packagesToVerify.Length;
				yield return null;

				// Verify / Install the local package as needed
				try
				{
					manager.VerifyPackage(package);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("An error occurred verifying Package '{0}', Version {1}: {2}", 
						package.Id, 
						package.Version, 
						Log.Exception(e));
				}
				workerInterface.Progress += 0.5f / packagesToVerify.Length;
				yield return null;
			}
			Log.Editor.PopIndent();

			yield break;
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
				Log.Core.WriteWarning("Unable to archive old logfile: {0}", Log.Exception(e));
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
				Log.AddGlobalOutput(logfileOutput);
			}
			catch (Exception e)
			{
				Log.Core.WriteWarning("Unable to create logfile: {0}", Log.Exception(e));
			}
		}
		private static void CloseLogfile()
		{
			if (logfileOutput != null)
			{
				Log.RemoveGlobalOutput(logfileOutput);
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
				Log.Editor.WriteError(Log.Exception(e.ExceptionObject as Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Log.Editor.WriteError(Log.Exception(e.Exception));
			}
			catch (Exception) { /* Ensure we're not causing any further exception by logging... */ }
		}
	}
}
