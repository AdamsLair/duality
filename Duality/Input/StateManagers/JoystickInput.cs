using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Input;


namespace Duality
{
	/// <summary>
	/// Provides access to extended user input such as joysticks or gamepads.
	/// </summary>
	public sealed class JoystickInput : IUserInput
	{
		private class State
		{
			public bool		IsAvailable		= false;
			public float[]	AxisValue		= new float[(int)JoystickAxis.Last + 1];
			public bool[]	ButtonPressed	= new bool[(int)JoystickButton.Last + 1];

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
			public void UpdateFromSource(IJoystickInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				for (int i = 0; i < this.ButtonPressed.Length; i++)
				{
					this.ButtonPressed[i] = source[(JoystickButton)i];
				}
				for (int i = 0; i < this.AxisValue.Length; i++)
				{
					this.AxisValue[i] = source[(JoystickAxis)i];
				}
			}
		}

		private	IJoystickInputSource	source			= null;
		private	State					currentState	= new State();
		private	State					lastState		= new State();
		private	string					description		= null;
		private	int						axisCount		= 0;
		private	int						buttonCount		= 0;
		private	bool					isDummy			= false;


		/// <summary>
		/// [GET / SET] The extended user inputs data source.
		/// </summary>
		public IJoystickInputSource Source
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
						this.axisCount = this.source.AxisCount;
						this.buttonCount = this.source.ButtonCount;
					}
				}
			}
		}
		IUserInputSource IUserInput.Source
		{
			get { return this.Source; }
			set { this.Source = value as IJoystickInputSource; }
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
		/// [GET] Returns the number of axes.
		/// </summary>
		public int AxisCount
		{
			get { return this.axisCount; }
		}
		/// <summary>
		/// [GET] Returns the number of buttons.
		/// </summary>
		public int ButtonCount
		{
			get { return this.buttonCount; }
		}

		/// <summary>
		/// [GET] Returns whether the specified device button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool this[JoystickButton button]
		{
			get { return this.currentState.ButtonPressed[(int)button]; }
		}
		/// <summary>
		/// [GET] Returns the specified device axis current value.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float this[JoystickAxis axis]
		{
			get { return this.currentState.AxisValue[(int)axis]; }
		}

		/// <summary>
		/// Fired once when a device button is no longer pressed.
		/// </summary>
		public event EventHandler<JoystickButtonEventArgs> ButtonUp;
		/// <summary>
		/// Fired once when a device button is pressed.
		/// </summary>
		public event EventHandler<JoystickButtonEventArgs> ButtonDown;
		/// <summary>
		/// Fired whenever a device axis changes its value.
		/// </summary>
		public event EventHandler<JoystickMoveEventArgs> Move;
		/// <summary>
		/// Fired when the joystick is no longer available to Duality.
		/// </summary>
		public event EventHandler NoLongerAvailable;
		/// <summary>
		/// Fired when the joystick becomes available to Duality.
		/// </summary>
		public event EventHandler BecomesAvailable;
		

		internal JoystickInput(bool dummy = false)
		{
			this.isDummy = dummy;
		}
		void IUserInput.Update()
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
			for (int i = 0; i < this.currentState.ButtonPressed.Length; i++)
			{
				if (this.currentState.ButtonPressed[i] && !this.lastState.ButtonPressed[i])
				{
					if (this.ButtonDown != null)
					{
						this.ButtonDown(this, new JoystickButtonEventArgs(
							(JoystickButton)i, 
							this.currentState.ButtonPressed[i]));
					}
				}
				if (!this.currentState.ButtonPressed[i] && this.lastState.ButtonPressed[i])
				{
					if (this.ButtonUp != null)
					{
						this.ButtonUp(this, new JoystickButtonEventArgs(
							(JoystickButton)i, 
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
						this.Move(this, new JoystickMoveEventArgs(
							(JoystickAxis)i,
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
		public bool ButtonPressed(JoystickButton button)
		{
			return this.currentState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was hit this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonHit(JoystickButton button)
		{
			return this.currentState.ButtonPressed[(int)button] && !this.lastState.ButtonPressed[(int)button];
		}
		/// <summary>
		/// Returns whether the specified button was released this frame.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		public bool ButtonReleased(JoystickButton button)
		{
			return !this.currentState.ButtonPressed[(int)button] && this.lastState.ButtonPressed[(int)button];
		}
		
		/// <summary>
		/// Returns the specified axis value.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float AxisValue(JoystickAxis axis)
		{
			return this.currentState.AxisValue[(int)axis];
		}
		/// <summary>
		/// Returns the specified axis value change since last frame.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public float AxisSpeed(JoystickAxis axis)
		{
			return this.currentState.AxisValue[(int)axis] - this.lastState.AxisValue[(int)axis];
		}
	}
}
