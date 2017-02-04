using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class MouseEventArgs : UserInputEventArgs
	{
		private Vector2 pos;
		
		public Vector2 Pos
		{
			get { return this.pos; }
		}

		public MouseEventArgs(MouseInput inputChannel, Vector2 pos) : base(inputChannel)
		{
			this.pos = pos;
		}
	}

	public class MouseMoveEventArgs : MouseEventArgs
	{
		private Vector2 vel;
		
		public Vector2 Vel
		{
			get { return this.vel; }
		}

		public MouseMoveEventArgs(MouseInput inputChannel, Vector2 pos, Vector2 vel) : base(inputChannel, pos)
		{
			this.vel = vel;
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

		public MouseButtonEventArgs(MouseInput inputChannel, Vector2 pos, MouseButton button, bool pressed) : base(inputChannel, pos)
		{
			this.button = button;
			this.pressed = pressed;
		}
	}

	public class MouseWheelEventArgs : MouseEventArgs
	{
		private float wheelValue;
		private float wheelSpeed;

		public float WheelValue
		{
			get { return this.wheelValue; }
		}
		public float WheelSpeed
		{
			get { return this.wheelSpeed; }
		}

		public MouseWheelEventArgs(MouseInput inputChannel, Vector2 pos, float value, float delta) : base(inputChannel, pos)
		{
			this.wheelValue = value;
			this.wheelSpeed = delta;
		}
	}
}
