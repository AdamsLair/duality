using System;
using System.Runtime.InteropServices;

namespace DualityGame
{
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern Int32 AllocConsole();
	}
}
