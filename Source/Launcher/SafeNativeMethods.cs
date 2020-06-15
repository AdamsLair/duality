using System;

namespace Duality.Launcher
{
	internal static class SafeNativeMethods
	{
		public static void AllocConsole()
		{
			try
			{
				_AllocConsole();
			}
			catch (Exception) { }
		}
		private static void _AllocConsole()
		{
			NativeMethods.AllocConsole();
		}
	}
}
