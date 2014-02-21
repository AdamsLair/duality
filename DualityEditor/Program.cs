using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

using Duality.Editor.Forms;

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
			bool recover = false;
			foreach (string a in args)
			{
				if (a == "debug")
					System.Diagnostics.Debugger.Launch();
				else if (a == "recover")
					recover = true;
			}
			//Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US"); // en-US, de-DE
			//Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			Application.CurrentCulture = Thread.CurrentThread.CurrentCulture;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ThreadException += Application_ThreadException;

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
	}
}
