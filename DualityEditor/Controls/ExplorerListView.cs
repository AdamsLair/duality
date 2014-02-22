using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Duality.Editor.Controls
{
	public class ExplorerListView : ListView
	{
		#region Native methods
		private const int LVM_FIRST = 0x1000;
		private const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
		private const int LVS_EX_DOUBLEBUFFER = 0x00010000;
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("uxtheme", CharSet = CharSet.Unicode)]
		private extern static Int32 SetWindowTheme(IntPtr hWnd, String textSubAppName, String textSubIdList);
		#endregion

		private bool styleSet = false;


		public ExplorerListView()
		{
			this.Enter += (s, e) => this.InitExplorerStyle();
			this.GotFocus += (s, e) => this.InitExplorerStyle();
		}

		private int MakeLong(short lowPart, short highPart)
		{
			return (int)(((ushort)lowPart) | (uint)(highPart << 16));
		}
		private void InitExplorerStyle()
		{
			if (styleSet) return;
			SetWindowTheme(this.Handle, "explorer", null);
			SendMessage(this.Handle, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, LVS_EX_DOUBLEBUFFER);
			styleSet = true;
		}
	}
}
