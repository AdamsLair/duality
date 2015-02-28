using System;
using OpenTK;
using OpenTK.Input;

namespace Duality
{
	public class GameWindowMouseInputSource : IMouseInputSource
	{
		public delegate void CursorPosSetter(int v);

		private	GameWindow		window;
		private CursorPosSetter cursorPosSetterX;
		private CursorPosSetter cursorPosSetterY;
		private bool			cursorInView;

		public string Description
		{
			get { return "Mouse"; }
		}
		public bool IsAvailable
		{
			get { return this.window != null && this.window.Mouse != null && this.cursorInView; }
		}
		public int X
		{
			get { return this.window.Mouse.X; }
			set { if (this.cursorPosSetterX != null) this.cursorPosSetterX(value); }
		}
		public int Y
		{
			get { return this.window.Mouse.Y; }
			set { if (this.cursorPosSetterY != null) this.cursorPosSetterY(value); }
		}
		public float Wheel
		{
			get { return this.window.Mouse.WheelPrecise; }
		}
		public bool this[MouseButton key]
		{
			get { return this.window.Mouse[key]; }
		}
		
		public GameWindowMouseInputSource(GameWindow window, CursorPosSetter cursorPosSetterX, CursorPosSetter cursorPosSetterY)
		{
			this.window = window;
			this.cursorPosSetterX = cursorPosSetterX;
			this.cursorPosSetterY = cursorPosSetterY;

			this.window.Mouse.Enter += this.device_Enter;
			this.window.Mouse.Leave += this.device_Leave;
		}

		private void device_Enter(object sender, EventArgs e)
		{
			this.cursorInView = true;
		}
		private void device_Leave(object sender, EventArgs e)
		{
			this.cursorInView = false;
		}

		public void UpdateState() {}
	}
}
