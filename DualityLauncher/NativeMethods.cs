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
		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int X, int Y);
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out Point lpPoint);
		public static Point GetCursorPos()
		{
			Point result;
			if (!GetCursorPos(out result)) return Point.Empty;
			return result;
		}
		public static bool GetCursorPos(out int x, out int y)
		{
			Point result;
			bool returnVal = GetCursorPos(out result);
			x = result.X;
			y = result.Y;
			return returnVal;
		}
	}
}
