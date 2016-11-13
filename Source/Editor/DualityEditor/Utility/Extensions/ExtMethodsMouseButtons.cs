using System.Windows.Forms;
using Duality.Input;

namespace Duality.Editor
{
	public static class ExtMethodsMouseButtons
	{
		public static MouseButton ToDualitySingle(this MouseButtons buttons)
		{
			if ((buttons & MouseButtons.Left) != MouseButtons.None) return MouseButton.Left;
			else if ((buttons & MouseButtons.Right) != MouseButtons.None) return MouseButton.Right;
			else if ((buttons & MouseButtons.Middle) != MouseButtons.None) return MouseButton.Middle;
			else if ((buttons & MouseButtons.XButton1) != MouseButtons.None) return MouseButton.Extra1;
			else if ((buttons & MouseButtons.XButton2) != MouseButtons.None) return MouseButton.Extra2;
			else return MouseButton.Last;
		}
		public static int ToDuality(this MouseButtons buttons)
		{
			int btn = 0;
			if ((buttons & MouseButtons.Left) != MouseButtons.None) btn |= 1 << (int)MouseButton.Left;
			else if ((buttons & MouseButtons.Right) != MouseButtons.None) btn |= 1 << (int)MouseButton.Right;
			else if ((buttons & MouseButtons.Middle) != MouseButtons.None) btn |= 1 << (int)MouseButton.Middle;
			else if ((buttons & MouseButtons.XButton1) != MouseButtons.None) btn |= 1 << (int)MouseButton.Extra1;
			else if ((buttons & MouseButtons.XButton2) != MouseButtons.None) btn |= 1 << (int)MouseButton.Extra2;
			return btn;
		}
	}
}
