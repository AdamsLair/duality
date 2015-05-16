using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

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
