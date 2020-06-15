using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Duality.Launcher
{
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll")]
		public static extern Int32 AllocConsole();
	}
}
