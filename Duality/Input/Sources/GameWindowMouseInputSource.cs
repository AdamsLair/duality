using System;
using OpenTK;

namespace Duality.Input
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
			get { return this.window.Mouse[GetOpenTKMouseButton(key)]; }
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

		private static OpenTK.Input.MouseButton GetOpenTKMouseButton(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:		return OpenTK.Input.MouseButton.Left;
				case MouseButton.Right:		return OpenTK.Input.MouseButton.Right;
				case MouseButton.Middle:	return OpenTK.Input.MouseButton.Middle;
				case MouseButton.Extra1:	return OpenTK.Input.MouseButton.Button1;
				case MouseButton.Extra2:	return OpenTK.Input.MouseButton.Button2;
				case MouseButton.Extra3:	return OpenTK.Input.MouseButton.Button3;
				case MouseButton.Extra4:	return OpenTK.Input.MouseButton.Button4;
				case MouseButton.Extra5:	return OpenTK.Input.MouseButton.Button5;
				case MouseButton.Extra6:	return OpenTK.Input.MouseButton.Button6;
				case MouseButton.Extra7:	return OpenTK.Input.MouseButton.Button7;
				case MouseButton.Extra8:	return OpenTK.Input.MouseButton.Button8;
				case MouseButton.Extra9:	return OpenTK.Input.MouseButton.Button9;
			}

			return OpenTK.Input.MouseButton.LastButton;
		}
	}
}
