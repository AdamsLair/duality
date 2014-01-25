using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using OpenTK.Input;


namespace Duality
{
	/// <summary>
	/// Provides access to gamepad user input.
	/// </summary>
	public sealed class GamepadInput : IUserInput
	{
		private class State
		{
			public bool		IsAvailable		= false;
			public float[]	AxisValue		= new float[(int)GamepadAxis.Last + 1];
			public bool[]	ButtonPressed	= new bool[(int)GamepadButton.Last + 1];

			public State() {}
			public State(State baseState)
			{
				baseState.CopyTo(this);
			}
			public void CopyTo(State other)
			{
				other.IsAvailable = this.IsAvailable;
				this.AxisValue.CopyTo(other.AxisValue, 0);
				this.ButtonPressed.CopyTo(other.ButtonPressed, 0);
			}
			public void UpdateFromSource(IGamepadInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				for (int i = 0; i < this.ButtonPressed.Length; i++)
				{
					this.ButtonPressed[i] = source[(GamepadButton)i];
				}
				for (int i = 0; i < this.AxisValue.Length; i++)
				{
					this.AxisValue[i] = source[(GamepadAxis)i];
				}
			}
		}

		private	IGamepadInputSource	source			= null;
		private	State				currentState	= new State();
		private	State				lastState		= new State();
		private	string				description		= null;
		private	bool				isDummy			= false;


		/// <summary>
		/// [GET / SET] The gamepad inputs data source.
		/// </summary>
		public IGamepadInputSource Source
		{
			get { return this.source; }
			set
			{
				if (this.source != value && !this.isDummy)
				{
					this.source = value;
					if (this.source != null)
					{
						this.description = this.source.Description;
					}
				}
			}
		}
		IUserInputSource IUserInput.Source
		{
			get { return this.Source; }
			set { this.Source = value as IGamepadInputSource; }
		}
		/// <summary>
		/// [GET] A string containing a unique description for this instance.
		/// </summary>
		public string Description
		{
			get { return this.description; }
		}
		/// <summary>
		/// [GET] Returns whether this input is currently available.
		/// </summary>
		public bool IsAvailable
		{
			get { return this.source != null && this.source.IsAvailable; }
		}

		/// <summary>
		/// [GET] The left thumbsticks current value.
		/// </summary>
		public Vector2 LeftThumbstick
		{
			get
			{
				return new Vector2(
					this.currentState.AxisValue[(int)GamepadAxis.LeftThumbstickX], 
					this.currentState.AxisValue[(int)GamepadAxis.LeftThumbstickY]);
			}
		}
		/// <summary>
		/// [GET] The left thumbsticks movement speed.
		/// </summary>
		public Vector2 LeftThumbstickSpeed
		{
			get
			{
				return	new Vector2(
					this.currentState.AxisValue[(int)GamepadAxis.LeftThumbstickX], 
					this.currentState.AxisValue[(int)GamepadAxis.LeftThumbstickY]) - 
						new Vector2(
					this.lastState.AxisValue[(int)GamepadAxis.LeftThumbstickX], 
					this.lastState.AxisValue[(int)GamepadAxis.LeftThumbstickY]) ;
			}
		}
		/// <summary>
		/// [GET] The right thumbsticks current value.
		/// </summary>
		public Vector2 RightThumbstick
		{
			get
			{
				return new Vector2(
					this.currentState.AxisValue[(int)GamepadAxis.RightThumbstickX], 
					this.currentState.AxisValue[(int)GamepadAxis.RightThumbstickY]);
			}
		}
		/// <summary>
		/// [GET] The right thumbsticks current movement speed.
		/// </summary>
		public Vector2 RightThumbstickSpeed
		{
			get
			{
				return	new Vector2(
					this.currentState.AxisValue[(int)GamepadAxis.RightThumbstickX], 
					this.currentState.AxisValue[(int)GamepadAxis.RightThumbstickY]) - 
						new Vector2(
					this.lastState.AxisValue[(int)GamepadAxis.RightThumbstickX], 
					this.lastState.AxisValue[(int)GamepadAxis.RightThumbstickY]) ;
			}
		}
		/// <summary>
		/// [GET] The gamepads DPad state, expressed as a directional vector.
		/// </summary>
		public Vector2 DPad
		{
			get
			{
				Vector2 vec = Vector2.Zero;
				if (this.currentState.ButtonPressed[(int)GamepadButton.DPadLeft])	vec -= Vector2.UnitX;
				if (this.currentState.ButtonPressed[(int)GamepadButton.DPadRight])	vec += Vector2.UnitX;
				if (this.currentState.ButtonPressed[(int)GamepadButton.DPadUp])		vec -= Vector2.UnitY;
				if (this.currentState.ButtonPressed[(int)GamepadButton.DPadDown])	vec += Vector2.UnitY;
				return vec;
			}
		}

		/// <summary>
		/// [GET] Returns whether the specified gamepad button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool this[GamepadButton button]
		{
			get { return this.currentState.ButtonPressed[(int)button]; }
		}
		/// <summary>
		/// [GET] Returns the specified gamepad axis current value.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float this[GamepadAxis axis]
		{
			get { return this.currentState.AxisValue[(int)axis]; }
		}
		
		/// <summary>
		/// Fired once when a device button is no longer pressed.
		/// </summary>
		public event EventHandler<GamepadButtonEventArgs> ButtonUp;
		/// <summary>
		/// Fired once when a device button is pressed.
		/// </summary>
		public event EventHandler<GamepadButtonEventArgs> ButtonDown;
		/// <summary>
		/// Fired whenever a device axis changes its value.
		/// </summary>
		public event EventHandler<GamepadAxisEventArgs> Move;
		/// <summary>
		/// Fired when the joystick is no longer available to Duality.
		/// </summary>
		public event EventHandler NoLongerAvailable;
		/// <summary>
		/// Fired when the joystick becomes available to Duality.
		/// </summary>
		public event EventHandler BecomesAvailable;
		

		internal GamepadInput(bool dummy = false)
		{
			this.isDummy = dummy;
		}
		void IUserInput.Update()
		{
			// Memorize last state
			this.currentState.CopyTo(this.lastState);

			// Update source state
			this.source.UpdateState();

			// Obtain new state
			this.currentState.UpdateFromSource(this.source);

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
			for (int i = 0; i < this.currentState.ButtonPressed.Length; i++)
			{
				if (this.currentState.ButtonPressed[i] && !this.lastState.ButtonPressed[i])
				{
					if (this.ButtonDown != null)
					{
						this.ButtonDown(this, new GamepadButtonEventArgs(
							(GamepadButton)i, 
							this.currentState.ButtonPressed[i]));
					}
				}
				if (!this.currentState.ButtonPressed[i] && this.lastState.ButtonPressed[i])
				{
					if (this.ButtonUp != null)
					{
						this.ButtonUp(this, new GamepadButtonEventArgs(
							(GamepadButton)i, 
							this.currentState.ButtonPressed[i]));
					}
				}
			}
			for (int i = 0; i < this.currentState.AxisValue.Length; i++)
			{
				if (this.currentState.AxisValue[i] != this.lastState.AxisValue[i])
				{
					if (this.Move != null)
					{
						this.Move(this, new GamepadAxisEventArgs(
							(GamepadAxis)i,
							this.currentState.AxisValue[i],
							this.currentState.AxisValue[i] - this.lastState.AxisValue[i]));
					}
				}
			}
		}
		
		/// <summary>
		/// Returns whether the specified button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonPressed(GamepadButton button)
		{
			return this.currentState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was hit this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonHit(GamepadButton button)
		{
			return this.currentState.ButtonPressed[(int)button] && !this.lastState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was released this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonReleased(GamepadButton button)
		{
			return !this.currentState.ButtonPressed[(int)button] && this.lastState.ButtonPressed[(int)button];
		}
		
		/// <summary>
		/// Returns the specified axis value.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float AxisValue(GamepadAxis axis)
		{
			return this.currentState.AxisValue[(int)axis];
		}
		/// <summary>
		/// Returns the specified axis value change since last frame.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float AxisSpeed(GamepadAxis axis)
		{
			return this.currentState.AxisValue[(int)axis] - this.lastState.AxisValue[(int)axis];
		}
		
		/// <summary>
		/// Sets the gamepads current vibration values.
		/// </summary>
		/// <param name="left">Left vibration between 0.0 and 1.0</param>
		/// <param name="right">Right vibration between 0.0 and 1.0</param>
		public void SetVibration(float left, float right)
		{
			if (this.source == null) return;
			this.source.SetVibration(left, right);
		}
	}
}
