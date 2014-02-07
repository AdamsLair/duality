using System;
using System.Collections.Specialized;

using OpenTK;
using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Provides access to user mouse input.
	/// </summary>
	public sealed class MouseInput : IUserInput
	{
		private class State
		{
			public bool IsAvailable		= false;
			public int X				= -1;
			public int Y				= -1;
			public float Wheel			= 0.0f;
			public bool[] ButtonPressed	= new bool[(int)MouseButton.LastButton + 1];

			public State() {}
			public State(State baseState)
			{
				baseState.CopyTo(this);
			}
			public void CopyTo(State other)
			{
				other.IsAvailable	= this.IsAvailable;
				other.X				= this.X;
				other.Y				= this.Y;
				other.Wheel			= this.Wheel;
				this.ButtonPressed.CopyTo(other.ButtonPressed, 0);
			}
			public void UpdateFromSource(IMouseInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				this.X = source.X;
				this.Y = source.Y;
				this.Wheel = source.Wheel;
				for (int i = 0; i < this.ButtonPressed.Length; i++)
				{
					this.ButtonPressed[i] = source[(MouseButton)i];
				}
			}
		}


		private	IMouseInputSource	source			= null;
		private	State				currentState	= new State();
		private	State				lastState		= new State();

		
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
		/// [GET / SET] The current viewport-local cursor X position.
		/// </summary>
		public int X
		{
			get { return this.currentState.X; }
			set { if (this.source != null) this.source.X = value; }
		}
		/// <summary>
		/// [GET / SET] The current viewport-local cursor Y position.
		/// </summary>
		public int Y
		{
			get { return this.currentState.Y; }
			set { if (this.source != null) this.source.Y = value; }
		}
		/// <summary>
		/// [GET] The current viewport-local cursor position.
		/// </summary>
		public Vector2 Pos
		{
			get { return new Vector2(this.currentState.X, this.currentState.Y); }
		}
		/// <summary>
		/// [GET] Returns the X position change since last frame.
		/// </summary>
		public int XSpeed
		{
			get { return this.currentState.X - this.lastState.X; }
		}
		/// <summary>
		/// [GET] Returns the Y position change since last frame.
		/// </summary>
		public int YSpeed
		{
			get { return this.currentState.Y - this.lastState.Y; }
		}
		/// <summary>
		/// [GET] The viewport-local cursor position change since last frame.
		/// </summary>
		public Vector2 Vel
		{
			get { return new Vector2(this.currentState.X - this.lastState.X, this.currentState.Y - this.lastState.Y); }
		}
		/// <summary>
		/// [GET] The current mouse wheel value
		/// </summary>
		public int Wheel
		{
			get { return MathF.RoundToInt(this.currentState.Wheel); }
		}
		/// <summary>
		/// [GET] The current (precise, high resolution) mouse wheel value
		/// </summary>
		public float WheelPrecise
		{
			get { return this.currentState.Wheel; }
		}
		/// <summary>
		/// [GET] Returns the change of the mouse wheel value since last frame.
		/// </summary>
		public int WheelSpeed
		{
			get { return MathF.RoundToInt(this.currentState.Wheel - this.lastState.Wheel); }
		}
		/// <summary>
		/// [GET] Returns the (precise, high resolution) change of the mouse wheel value since last frame.
		/// </summary>
		public float WheelSpeedPrecise
		{
			get { return this.currentState.Wheel - this.lastState.Wheel; }
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
			if (this.currentState.IsAvailable)
			{
				if (this.currentState.X != this.lastState.X || this.currentState.Y != this.lastState.Y)
				{
					if (this.Move != null)
					{
						this.Move(this, new MouseMoveEventArgs(
							this.currentState.X, 
							this.currentState.Y, 
							this.currentState.X - this.lastState.X, 
							this.currentState.Y - this.lastState.Y));
					}
				}
				if (this.currentState.Wheel != this.lastState.Wheel)
				{
					if (this.WheelChanged != null)
					{
						this.WheelChanged(this, new MouseWheelEventArgs(
							this.currentState.X,
							this.currentState.Y,
							MathF.RoundToInt(this.currentState.Wheel),
							MathF.RoundToInt(this.currentState.Wheel - this.lastState.Wheel)));
					}
				}
				for (int i = 0; i < this.currentState.ButtonPressed.Length; i++)
				{
					if (this.currentState.ButtonPressed[i] && !this.lastState.ButtonPressed[i])
					{
						if (this.ButtonDown != null)
						{
							this.ButtonDown(this, new MouseButtonEventArgs(
								this.currentState.X, 
								this.currentState.Y, 
								(MouseButton)i, 
								this.currentState.ButtonPressed[i]));
						}
					}
					if (!this.currentState.ButtonPressed[i] && this.lastState.ButtonPressed[i])
					{
						if (this.ButtonUp != null)
						{
							this.ButtonUp(this, new MouseButtonEventArgs(
								this.currentState.X, 
								this.currentState.Y, 
								(MouseButton)i, 
								this.currentState.ButtonPressed[i]));
						}
					}
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
