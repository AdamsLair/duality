using System;

using Duality.Input;

using OpenTK;

using TKMouseState = OpenTK.Input.MouseState;
using TKMouseMoveEventArgs = OpenTK.Input.MouseMoveEventArgs;
using TKMouseWheelEventArgs = OpenTK.Input.MouseWheelEventArgs;
using TKMouseButtonEventArgs = OpenTK.Input.MouseButtonEventArgs;


namespace Duality.Backend.DefaultOpenTK
{
	public class GameWindowMouseInputSource : IMouseInputSource
	{
		public delegate void CursorPosSetter(int v);

		private GameWindow   window           = null;
		private bool         cursorInView     = false;
		private TKMouseState mouseState       = default(TKMouseState);
		private TKMouseState mouseStateBuffer = default(TKMouseState);

		public string Id
		{
			get { return "Mouse"; }
		}
		public Guid ProductId
		{
			get { return Guid.Empty; }
		}
		public string ProductName
		{
			get { return "Mouse"; }
		}
		public bool IsAvailable
		{
			get { return this.window != null && this.cursorInView; }
		}
		public Point2 Pos
		{
			get { return new Point2(this.mouseState.X, this.mouseState.Y); }
			set
			{
				System.Drawing.Point screenPoint = this.window.PointToScreen(new System.Drawing.Point(value.X, value.Y));
				OpenTK.Input.Mouse.SetPosition(screenPoint.X, screenPoint.Y);
			}
		}
		public float Wheel
		{
			get { return this.mouseState.WheelPrecise; }
		}
		public bool this[MouseButton key]
		{
			get { return this.mouseState[GetOpenTKMouseButton(key)]; }
		}
		
		public GameWindowMouseInputSource(GameWindow window)
		{
			this.window = window;
			this.window.MouseEnter += this.window_MouseEnter;
			this.window.MouseLeave += this.window_MouseLeave;
			this.window.MouseMove += this.window_MouseMove;
			this.window.MouseWheel += this.window_MouseWheel;
			this.window.MouseDown += this.window_MouseDown;
			this.window.MouseUp += this.window_MouseUp;
		}

		public void UpdateState()
		{
			this.mouseState = this.mouseStateBuffer;
		}

		private void window_MouseEnter(object sender, EventArgs e)
		{
			this.cursorInView = true;
		}
		private void window_MouseLeave(object sender, EventArgs e)
		{
			this.cursorInView = false;
		}
		private void window_MouseMove(object sender, TKMouseMoveEventArgs e)
		{
			this.mouseStateBuffer = e.Mouse;
		}
		private void window_MouseWheel(object sender, TKMouseWheelEventArgs e)
		{
			this.mouseStateBuffer = e.Mouse;
		}
		private void window_MouseDown(object sender, TKMouseButtonEventArgs e)
		{
			this.mouseStateBuffer = e.Mouse;
		}
		private void window_MouseUp(object sender, TKMouseButtonEventArgs e)
		{
			this.mouseStateBuffer = e.Mouse;
		}

		private static OpenTK.Input.MouseButton GetOpenTKMouseButton(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:   return OpenTK.Input.MouseButton.Left;
				case MouseButton.Right:  return OpenTK.Input.MouseButton.Right;
				case MouseButton.Middle: return OpenTK.Input.MouseButton.Middle;
				case MouseButton.Extra1: return OpenTK.Input.MouseButton.Button1;
				case MouseButton.Extra2: return OpenTK.Input.MouseButton.Button2;
				case MouseButton.Extra3: return OpenTK.Input.MouseButton.Button3;
				case MouseButton.Extra4: return OpenTK.Input.MouseButton.Button4;
				case MouseButton.Extra5: return OpenTK.Input.MouseButton.Button5;
				case MouseButton.Extra6: return OpenTK.Input.MouseButton.Button6;
				case MouseButton.Extra7: return OpenTK.Input.MouseButton.Button7;
				case MouseButton.Extra8: return OpenTK.Input.MouseButton.Button8;
				case MouseButton.Extra9: return OpenTK.Input.MouseButton.Button9;
			}

			return OpenTK.Input.MouseButton.LastButton;
		}
	}
}
