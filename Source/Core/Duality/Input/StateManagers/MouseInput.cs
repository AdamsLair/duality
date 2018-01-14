using System;
using System.Collections.Specialized;

namespace Duality.Input
{
	/// <summary>
	/// Provides access to user mouse input.
	/// </summary>
	public sealed class MouseInput : IUserInput
	{
		private class State
		{
			public bool IsAvailable = false;
			public Point2 WindowPos = Point2.Zero;
			public Vector2 ViewPos = Vector2.Zero;
			public float Wheel = 0.0f;
			public bool[] ButtonPressed	= new bool[(int)MouseButton.Last + 1];

			public State() {}
			public State(State baseState)
			{
				baseState.CopyTo(this);
			}
			public void CopyTo(State other)
			{
				other.IsAvailable = this.IsAvailable;
				other.WindowPos   = this.WindowPos;
				other.ViewPos     = this.ViewPos;
				other.Wheel       = this.Wheel;
				this.ButtonPressed.CopyTo(other.ButtonPressed, 0);
			}
			public void UpdateFromSource(IMouseInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				this.WindowPos = source.Pos;
				this.Wheel = source.Wheel;
				for (int i = 0; i < this.ButtonPressed.Length; i++)
				{
					this.ButtonPressed[i] = source[(MouseButton)i];
				}

				// Map window position to game view position
				Rect viewportRect;
				Vector2 gameViewSize;
				DualityApp.CalculateGameViewport(DualityApp.WindowSize, out viewportRect, out gameViewSize);
				Vector2 relativePos = new Vector2(
					MathF.Clamp((this.WindowPos.X - viewportRect.X) / viewportRect.W, 0.0f, 1.0f),
					MathF.Clamp((this.WindowPos.Y - viewportRect.Y) / viewportRect.H, 0.0f, 1.0f));
				this.ViewPos = relativePos * gameViewSize;
			}
		}


		private IMouseInputSource source       = null;
		private State             currentState = new State();
		private State             lastState    = new State();

		
		/// <summary>
		/// [GET / SET] The mouse inputs data source.
		/// </summary>
		public IMouseInputSource Source
		{
			get { return this.source; }
			set { this.source = value; }
		}
		IUserInputSource IUserInput.Source
		{
			get { return this.Source; }
			set { this.Source = value as IMouseInputSource; }
		}
		/// <summary>
		/// [GET] A text description of this input.
		/// </summary>
		public string Description
		{
			get { return "Mouse"; }
		}
		/// <summary>
		/// [GET] Returns whether this input is currently available.
		/// </summary>
		public bool IsAvailable
		{
			get { return this.currentState.IsAvailable; }
		}
		/// <summary>
		/// [GET / SET] The current window-local cursor position in native window coordinates.
		/// </summary>
		public Point2 WindowPos
		{
			get { return this.currentState.WindowPos; }
			set
			{
				if (this.source != null)
					this.source.Pos = value;
			}
		}
		/// <summary>
		/// [GET] The current viewport-local cursor position.
		/// </summary>
		public Vector2 Pos
		{
			get { return this.currentState.ViewPos; }
		}
		/// <summary>
		/// [GET] The viewport-local cursor position change since last frame.
		/// </summary>
		public Vector2 Vel
		{
			get
			{
				return (this.currentState.IsAvailable && this.lastState.IsAvailable) ? 
					this.currentState.ViewPos - this.lastState.ViewPos : 
					Vector2.Zero;
			}
		}
		/// <summary>
		/// [GET] The current mouse wheel value
		/// </summary>
		public float Wheel
		{
			get { return this.currentState.Wheel; }
		}
		/// <summary>
		/// [GET] Returns the change of the mouse wheel value since last frame.
		/// </summary>
		public float WheelSpeed
		{
			get { return (this.currentState.IsAvailable && this.lastState.IsAvailable) ? this.currentState.Wheel - this.lastState.Wheel : 0.0f; }
		}
		/// <summary>
		/// [GET] Returns whether a specific <see cref="MouseButton"/> is currently pressed.
		/// </summary>
		/// <param name="btn"></param>
		/// <returns></returns>
		public bool this[MouseButton btn]
		{
			get { return this.currentState.ButtonPressed[(int)btn]; }
		}

		/// <summary>
		/// Fired when a <see cref="MouseButton"/> is no longer pressed.
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> ButtonUp;
		/// <summary>
		/// Fired once when a <see cref="MouseButton"/> is pressed.
		/// </summary>
		public event EventHandler<MouseButtonEventArgs> ButtonDown;
		/// <summary>
		/// Fired when the cursor moves.
		/// </summary>
		public event EventHandler<MouseMoveEventArgs> Move;
		/// <summary>
		/// Fired when the cursor leaves the viewport area.
		/// </summary>
		public event EventHandler NoLongerAvailable;
		/// <summary>
		/// Fired when the cursor enters the viewport area.
		/// </summary>
		public event EventHandler BecomesAvailable;
		/// <summary>
		/// Fired when the mouse wheel value changes.
		/// </summary>
		public event EventHandler<MouseWheelEventArgs> WheelChanged;

		
		internal MouseInput() {}
		internal void Update()
		{
			// Memorize last state
			this.currentState.CopyTo(this.lastState);

			if (this.source != null)
			{
				// Update source state
				this.source.UpdateState();

				// Obtain new state
				this.currentState.UpdateFromSource(this.source);
			}

			// Fire events
			if (this.currentState.IsAvailable && !this.lastState.IsAvailable)
			{
				if (this.BecomesAvailable != null)
					this.BecomesAvailable(this, EventArgs.Empty);
			}
			if (!this.currentState.IsAvailable && this.lastState.IsAvailable)
			{
				if (this.NoLongerAvailable != null)
					this.NoLongerAvailable(this, EventArgs.Empty);
			}
			if (this.currentState.ViewPos != this.lastState.ViewPos)
			{
				if (this.Move != null)
					this.Move(this, new MouseMoveEventArgs(
						this,
						this.Pos, 
						this.Vel));
			}
			if (this.currentState.Wheel != this.lastState.Wheel)
			{
				if (this.WheelChanged != null)
					this.WheelChanged(this, new MouseWheelEventArgs(
						this,
						this.Pos,
						this.Wheel,
						this.WheelSpeed));
			}
			for (int i = 0; i < this.currentState.ButtonPressed.Length; i++)
			{
				if (this.currentState.ButtonPressed[i] && !this.lastState.ButtonPressed[i])
				{
					if (this.ButtonDown != null)
						this.ButtonDown(this, new MouseButtonEventArgs(
							this,
							this.Pos, 
							(MouseButton)i, 
							this.currentState.ButtonPressed[i]));
				}
				if (!this.currentState.ButtonPressed[i] && this.lastState.ButtonPressed[i])
				{
					if (this.ButtonUp != null)
						this.ButtonUp(this, new MouseButtonEventArgs(
							this,
							this.Pos, 
							(MouseButton)i, 
							this.currentState.ButtonPressed[i]));
				}
			}
		}
		void IUserInput.Update()
		{
			this.Update();
		}
		
		/// <summary>
		/// Returns whether the specified button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonPressed(MouseButton button)
		{
			return this.currentState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was hit this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonHit(MouseButton button)
		{
			return this.currentState.ButtonPressed[(int)button] && !this.lastState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was released this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonReleased(MouseButton button)
		{
			return !this.currentState.ButtonPressed[(int)button] && this.lastState.ButtonPressed[(int)button];
		}
	}
}
