using System;
using System.Linq;

namespace Duality.Launcher
{
	internal static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			bool isDebugging = System.Diagnostics.Debugger.IsAttached || args.Contains(DualityApp.CmdArgDebug);
			bool isRunFromEditor = args.Contains(DualityApp.CmdArgEditor);
			if (isDebugging || isRunFromEditor) ShowConsole();
			DualityLauncher.Run(args);
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
