using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Duality.Editor.Controls
{
	public class CueTextBox : TextBox
	{
		#region Native methods
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
		private const int ECM_FIRST = 0x1500;
		private const int EM_SETCUEBANNER = ECM_FIRST + 1;
		#endregion

		private string cueText = null;
		public string CueText
		{
			get
			{
				return this.cueText;
			}
			set
			{
				this.cueText = value;
				SendMessage(this.Handle, EM_SETCUEBANNER, IntPtr.Zero, cueText ?? "");
			}
		}
	}
}
