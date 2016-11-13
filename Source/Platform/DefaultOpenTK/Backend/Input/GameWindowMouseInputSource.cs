using System;
using Duality.Input;
using OpenTK;

namespace Duality.Backend.DefaultOpenTK
{
	public class GameWindowMouseInputSource : IMouseInputSource
	{
		public delegate void CursorPosSetter(int v);

		private GameWindow window;
		private bool       cursorInView;

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
			set
			{
				OpenTK.Input.MouseState state = OpenTK.Input.Mouse.GetCursorState();
				System.Drawing.Point screenPoint = this.window.PointToScreen(new System.Drawing.Point(value, 0));
				OpenTK.Input.Mouse.SetPosition(screenPoint.X, state.Y);
			}
		}
		public int Y
		{
			get { return this.window.Mouse.Y; }
			set
			{
				OpenTK.Input.MouseState state = OpenTK.Input.Mouse.GetCursorState();
				System.Drawing.Point screenPoint = this.window.PointToScreen(new System.Drawing.Point(0, value));
				OpenTK.Input.Mouse.SetPosition(state.X, screenPoint.Y);
			}
		}
		public float Wheel
		{
			get { return this.window.Mouse.WheelPrecise; }
		}
		public bool this[MouseButton key]
		{
			get { return this.window.Mouse[GetOpenTKMouseButton(key)]; }
		}
		
		public GameWindowMouseInputSource(GameWindow window)
		{
			this.window = window;
			this.window.MouseEnter += this.window_MouseEnter;
			this.window.MouseLeave += this.window_MouseLeave;
		}

		public void UpdateState() { }

		private void window_MouseLeave(object sender, EventArgs e)
		{
			this.cursorInView = false;
		}
		private void window_MouseEnter(object sender, EventArgs e)
		{
			this.cursorInView = true;
		}
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
