using System.Windows.Forms;
using OpenTK.Input;

namespace Duality.Editor
{
	public static class ExtMethodsMouseButtons
	{
		public static MouseButton ToOpenTKSingle(this MouseButtons buttons)
		{
			if ((buttons & MouseButtons.Left) != MouseButtons.None) return MouseButton.Left;
			else if ((buttons & MouseButtons.Right) != MouseButtons.None) return MouseButton.Right;
			else if ((buttons & MouseButtons.Middle) != MouseButtons.None) return MouseButton.Middle;
			else return MouseButton.LastButton;
		}
		public static int ToOpenTK(this MouseButtons buttons)
		{
			int btn = 0;
			if ((buttons & MouseButtons.Left) != MouseButtons.None) btn |= 1 << (int)MouseButton.Left;
			else if ((buttons & MouseButtons.Right) != MouseButtons.None) btn |= 1 << (int)MouseButton.Right;
			else if ((buttons & MouseButtons.Middle) != MouseButtons.None) btn |= 1 << (int)MouseButton.Middle;
			return btn;
		}
	}
}
