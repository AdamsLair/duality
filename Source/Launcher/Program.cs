using System;

namespace Duality.Launcher
{
	internal static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			DualityLauncher.Run(args);
		}
	}
}
