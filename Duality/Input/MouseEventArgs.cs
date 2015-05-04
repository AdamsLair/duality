using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class MouseEventArgs : EventArgs
	{
		private int x;
		private int y;
		
		public int X
		{
			get { return this.x; }
		}
		public int Y
		{
			get { return this.y; }
		}
		public Vector2 Position
		{
			get { return new Vector2(this.x, this.y); }
		}

		public MouseEventArgs(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public class MouseMoveEventArgs : MouseEventArgs
	{
		private int deltaX;
		private int deltaY;
		
		public int DeltaX
		{
			get { return this.deltaX; }
		}
		public int DeltaY
		{
			get { return this.deltaY; }
		}
		public Vector2 Delta
		{
			get { return new Vector2(this.deltaX, this.deltaY); }
		}

		public MouseMoveEventArgs(int x, int y, int deltaX, int deltaY) : base(x, y)
		{
			this.deltaX = deltaX;
			this.deltaY = deltaY;
		}
	}

	public class MouseButtonEventArgs : MouseEventArgs
	{
		private MouseButton button;
		private bool pressed;

		public MouseButton Button
		{
			get { return this.button; }
		}
		public bool IsPressed
		{
			get { return this.pressed; }
		}

		public MouseButtonEventArgs(int x, int y, MouseButton button, bool pressed) : base(x, y)
		{
			this.button = button;
			this.pressed = pressed;
		}
	}

	public class MouseWheelEventArgs : MouseEventArgs
	{
		private float wheelValue;
		private float wheelDelta;

		public float WheelValue
		{
			get { return this.wheelValue; }
		}
		public float WheelDelta
		{
			get { return this.wheelDelta; }
		}

		public MouseWheelEventArgs(int x, int y, int value, int delta) : base(x, y)
		{
			this.wheelValue = value;
			this.wheelDelta = delta;
		}
	}
}
