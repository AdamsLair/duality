﻿using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Editor.Properties;
using Duality.Editor.Forms;
using Duality.Editor.PackageManagement;

namespace Duality.Editor
{
	static class Program
	{
		private const string DualityMainLicenseUrl = @"https://github.com/AdamsLair/duality/raw/master/LICENSE";

		[STAThread]
		private static void Main(string[] args)
		{
			// Parse command line arguments
			bool recover = false;
			foreach (string a in args)
			{
				if (a == "debug")
					System.Diagnostics.Debugger.Launch();
				else if (a == "recover")
					recover = true;
			}

			// Culture setup
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

			// Set up file logging
			StreamWriter logfileWriter = null;
			TextWriterLogOutput logfileOutput = null;
			try
			{
				// If there is an existing logfile, preserve it under a different name
				if (File.Exists(DualityEditorApp.EditorLogfilePath))
				{
					if (File.Exists(DualityEditorApp.EditorPrevLogfilePath))
						File.Delete(DualityEditorApp.EditorPrevLogfilePath);
					File.Move(DualityEditorApp.EditorLogfilePath, DualityEditorApp.EditorPrevLogfilePath);
				}

				// Create a new logfile
				logfileWriter = new StreamWriter(DualityEditorApp.EditorLogfilePath);
				logfileWriter.AutoFlush = true;
				logfileOutput = new TextWriterLogOutput(logfileWriter);
				Logs.AddGlobalOutput(logfileOutput);
			}
			catch (Exception e)
			{
				Logs.Core.WriteWarning("Text Logfile unavailable: {0}", LogFormat.Exception(e));
			}

			// Winforms Setup
			Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += Application_ThreadException;
			
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
					Logs.Editor.Write("Updating Packages...");
					Logs.Editor.PushIndent();
					ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
						GeneralRes.TaskInstallPackages_Caption, 
						GeneralRes.TaskInstallPackages_Desc, 
						SynchronizePackages, 
						packageManager);
					setupDialog.ShowInTaskbar = true;
					setupDialog.MainThreadRequired = false;
					setupDialog.ShowDialog();
					Logs.Editor.PopIndent();
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

			// Run the editor
			SplashScreen splashScreen = new SplashScreen(recover);
			splashScreen.Show();
			Application.Run();

			// Clean up the log file
			if (logfileWriter != null)
			{
				Logs.RemoveGlobalOutput(logfileOutput);
				logfileWriter.Flush();
				logfileWriter.Close();
				logfileWriter = null;
				logfileOutput = null;
			}
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Logs.Editor.WriteError("An error occurred: {0}", LogFormat.Exception(e.Exception));
			}
			catch (Exception) { /* Assure we're not causing any further exception by logging... */ }
		}
		private static System.Collections.IEnumerable SynchronizePackages(ProcessingBigTaskDialog.WorkerInterface workerInterface)
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
			Logs.Editor.Write("Uninstalling unregistered packages...");
			Logs.Editor.PushIndent();
			manager.UninstallNonRegisteredPackages();
			Logs.Editor.PopIndent();
			yield return null;

			// Iterate over previously reigstered local packages and verify / install them.
			Logs.Editor.Write("Verifying registered packages...");
			Logs.Editor.PushIndent();
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
					Logs.Editor.WriteError("An error occurred verifying Package '{0}', Version {1}: {2}", 
						package.Id, 
						package.Version, 
						LogFormat.Exception(e));
				}
				workerInterface.Progress += 0.5f / packagesToVerify.Length;
				yield return null;
			}
			Logs.Editor.PopIndent();

			yield break;
		}
	}
}