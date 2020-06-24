using System;
using Duality;
using Duality.Launcher;
using Duality.Resources;

namespace DualityGame
{
	internal static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var launcherArgs = new LauncherArgs(args);

			if (launcherArgs.IsDebugging|| launcherArgs.IsRunFromEditor) ShowConsole();

			using (var launcher = new DualityLauncher(launcherArgs))
			{
				launcher.Run();
			}
		}

		private static bool hasConsole = false;
		private static void ShowConsole()
		{
			if (hasConsole) return;
			SafeNativeMethods.AllocConsole();
			hasConsole = true;
		}
	}
}
