using System;
using System.Runtime.InteropServices;

namespace Duality.Launcher
{
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern Int32 AllocConsole();
	}
}
