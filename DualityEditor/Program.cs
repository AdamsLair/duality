using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor.Properties;
using Duality.Editor.Forms;
using Duality.Editor.PackageManagement;

namespace Duality.Editor
{
	static class Program
	{
		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
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

			Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += Application_ThreadException;

			// Perform the initial package update - even before initializing the editor
			{
				PackageManager packageManager = new PackageManager();
				if (packageManager.IsPackageUpdateRequired)
				{
					Log.Editor.Write("Updating Packages...");
					Log.Editor.PushIndent();
					ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
						GeneralRes.TaskInstallPackages_Caption, 
						GeneralRes.TaskInstallPackages_Desc, 
						FirstTimeSetup, 
						packageManager);
					setupDialog.ShowInTaskbar = true;
					setupDialog.MainThreadRequired = false;
					setupDialog.ShowDialog();
					Log.Editor.PopIndent();
				}
				if (packageManager.ApplyUpdate())
				{
					Application.Exit();
					return;
				}
			}

			// Run the editor
			SplashScreen splashScreen = new SplashScreen(recover);
			splashScreen.Show();
			Application.Run();
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Duality.Log.Editor.WriteError("An error occurred: {0}", Duality.Log.Exception(e.Exception));
			}
			catch (Exception) { /* Assure we're not causing any further exception by logging... */ }
		}
		private static System.Collections.IEnumerable FirstTimeSetup(ProcessingBigTaskDialog.WorkerInterface workerInterface)
		{
			PackageManager manager = workerInterface.Data as PackageManager;

			workerInterface.Progress = 0.0f;
			workerInterface.StateDesc = GeneralRes.TaskPrepareInfo;
			yield return null;

			LocalPackage[] packagesToVerify = manager.LocalPackages.ToArray();
			manager.OrderByDependencies(packagesToVerify);

			foreach (LocalPackage package in packagesToVerify)
			{
				if (package.Version != null)
					workerInterface.StateDesc = string.Format("Package '{0}', Version {1}...", package.Id, package.Version);
				else
					workerInterface.StateDesc = string.Format("Package '{0}'...", package.Id);
				workerInterface.Progress += 0.5f / packagesToVerify.Length;
				yield return null;

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

			yield break;
		}
	}
}
