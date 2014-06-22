using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

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
			bool update = false;
			foreach (string a in args)
			{
				if (a == "debug")
					System.Diagnostics.Debugger.Launch();
				else if (a == "recover")
					recover = true;
				else if (a == "update")
					update = true;
			}

			// Culture setup
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

			// Only perform a quick update, don't do anything else.
			if (update)
			{
				Log.Editor.Write("Performing Duality Update");
				Log.Editor.PushIndent();
				try
				{
					PackageManager packageManager = new PackageManager();
					packageManager.VerifyPackages();
					packageManager.ApplyUpdate(false);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError(Log.Exception(e));
				}
				Log.Editor.PopIndent();
			}
			// Run the editor regularly
			else
			{
				Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.ThreadException += Application_ThreadException;

				SplashScreen splashScreen = new SplashScreen(recover);
				splashScreen.Show();

				Application.Run();
			}
		}
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Duality.Log.Editor.WriteError("An error occurred: {0}", Duality.Log.Exception(e.Exception));
			}
			catch (Exception) { /* Assure we're not causing any further exception by logging... */ }
		}
	}
}
