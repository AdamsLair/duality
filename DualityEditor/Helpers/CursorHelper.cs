using System;
using System.Windows.Forms;
using System.Drawing;

using Duality.Editor.Properties;

namespace Duality.Editor
{
	public static class CursorHelper
	{
		public static readonly Cursor Arrow;
		public static readonly Cursor ArrowAction;
		public static readonly Cursor ArrowActionMove;
		public static readonly Cursor ArrowActionRotate;
		public static readonly Cursor ArrowActionScale;
		public static readonly Cursor HandGrab;
		public static readonly Cursor HandGrabbing;

		static CursorHelper()
		{
			HandGrab			= CreateCursor(GeneralResCache.CursorHandGrab, 8, 8);
			HandGrabbing		= CreateCursor(GeneralResCache.CursorHandGrabbing, 8, 8);
			Arrow				= CreateCursor(GeneralResCache.CursorArrow, 0, 0);
			ArrowAction			= CreateCursor(GeneralResCache.CursorArrowAction, 0, 0);
			ArrowActionMove		= CreateCursor(GeneralResCache.CursorArrowActionMove, 0, 0);
			ArrowActionRotate	= CreateCursor(GeneralResCache.CursorArrowActionRotate, 0, 0);
			ArrowActionScale	= CreateCursor(GeneralResCache.CursorArrowActionScale, 0, 0);
		}

		public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
		{
			IntPtr ptr = bmp.GetHicon();
			NativeMethods.IconInfo tmp = new NativeMethods.IconInfo();
			NativeMethods.GetIconInfo(ptr, ref tmp);
			tmp.xHotspot = xHotSpot;
			tmp.yHotspot = yHotSpot;
			tmp.fIcon = false;
			ptr = NativeMethods.CreateIconIndirect(ref tmp);
			return new Cursor(ptr);
		}
	}
}
