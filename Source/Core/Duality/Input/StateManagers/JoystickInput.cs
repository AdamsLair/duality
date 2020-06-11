using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Input
{
	/// <summary>
	/// Provides access to extended user input such as joysticks or gamepads.
	/// </summary>
	public sealed class JoystickInput : IUserInput
	{
		private static readonly int MaxAxisCount = 12;
		private static readonly int MaxButtonCount = 32;
		private static readonly int MaxHatCount = 4;

		private class State
		{
			public bool                  IsAvailable   = false;
			public float[]               AxisValue     = new float[MaxAxisCount];
			public bool[]                ButtonPressed = new bool[MaxButtonCount];
			public JoystickHatPosition[] HatPosition   = new JoystickHatPosition[MaxHatCount];

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
				this.HatPosition.CopyTo(other.HatPosition, 0);
			}
			public void UpdateFromSource(IJoystickInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				for (int i = 0; i < this.ButtonPressed.Length; i++)
					this.ButtonPressed[i] = source.ButtonPressed(i);

				for (int i = 0; i < this.AxisValue.Length; i++)
					this.AxisValue[i] = source.AxisValue(i);

				for (int i = 0; i < this.HatPosition.Length; i++)
					this.HatPosition[i] = source.HatPosition(i);
			}
		}

		private IJoystickInputSource source       = null;
		private State                currentState = new State();
		private State                lastState    = new State();
		private string               id           = null;
		private Guid                 productId    = Guid.Empty;
		private string               productName  = "Unknown";
		private int                  axisCount    = 0;
		private int                  buttonCount  = 0;
		private int                  hatCount     = 0;
		private bool                 isDummy      = false;


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
						this.id = this.source.Id;
						this.UpdateSourceCapabilities();
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
		/// [GET] A string containing a unique id for this instance.
		/// </summary>
		public string Id
		{
			get { return this.id; }
		}
		/// <summary>
		/// [GET] The unique ID of the product that is providing this input.
		/// </summary>
		public Guid ProductId
		{
			get { return this.productId; }
		}
		/// <summary>
		/// [GET] The name of the product that is providing this input.
		/// </summary>
		public string ProductName
		{
			get { return this.productName; }
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
		/// [GET] Returns the number of joystick hats.
		/// </summary>
		public int HatCount
		{
			get { return this.hatCount; }
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
		public event EventHandler<JoystickAxisEventArgs> AxisMove;
		/// <summary>
		/// Fired whenever a joystick hat moves.
		/// </summary>
		public event EventHandler<JoystickHatEventArgs> HatMove;
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

				// Update how many buttons, hats and axes there are - some sources aren't constant here.
				this.UpdateSourceCapabilities();

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
							this,
							i, 
							this.currentState.ButtonPressed[i]));
					}
				}
				if (!this.currentState.ButtonPressed[i] && this.lastState.ButtonPressed[i])
				{
					if (this.ButtonUp != null)
					{
						this.ButtonUp(this, new JoystickButtonEventArgs(
							this,
							i, 
							this.currentState.ButtonPressed[i]));
					}
				}
			}
			for (int i = 0; i < this.currentState.AxisValue.Length; i++)
			{
				if (this.currentState.AxisValue[i] != this.lastState.AxisValue[i])
				{
					if (this.AxisMove != null)
					{
						this.AxisMove(this, new JoystickAxisEventArgs(
							this,
							i,
							this.currentState.AxisValue[i],
							this.currentState.AxisValue[i] - this.lastState.AxisValue[i]));
					}
				}
			}
			for (int i = 0; i < this.currentState.HatPosition.Length; i++)
			{
				if (this.currentState.HatPosition[i] != this.lastState.HatPosition[i])
				{
					if (this.HatMove != null)
					{
						this.HatMove(this, new JoystickHatEventArgs(
							this,
							i,
							this.currentState.HatPosition[i],
							this.lastState.HatPosition[i]));
					}
				}
			}
		}
		private void UpdateSourceCapabilities()
		{
			this.productId = this.source.ProductId;
			this.productName = this.source.ProductName;
			this.axisCount = this.source.AxisCount;
			this.buttonCount = this.source.ButtonCount;
			this.hatCount = this.source.HatCount;
		}

		/// <summary>
		/// Returns whether the specified button is currently pressed.
		/// </summary>
		/// <param name="buttonIndex"></param>
		public bool ButtonPressed(int buttonIndex)
		{
			return this.currentState.ButtonPressed[buttonIndex];
		}
		/// <summary>
		/// Returns whether the specified button was hit this frame.
		/// </summary>
		/// <param name="buttonIndex"></param>
		public bool ButtonHit(int buttonIndex)
		{
			return this.currentState.ButtonPressed[buttonIndex] && !this.lastState.ButtonPressed[buttonIndex];
		}
		/// <summary>
		/// Returns whether the specified button was released this frame.
		/// </summary>
		/// <param name="buttonIndex"></param>
		public bool ButtonReleased(int buttonIndex)
		{
			return !this.currentState.ButtonPressed[buttonIndex] && this.lastState.ButtonPressed[buttonIndex];
		}
		
		/// <summary>
		/// Returns the specified axis value.
		/// </summary>
		/// <param name="axisIndex"></param>
		public float AxisValue(int axisIndex)
		{
			return this.currentState.AxisValue[axisIndex];
		}
		/// <summary>
		/// Returns the specified axis value change since last frame.
		/// </summary>
		/// <param name="axisIndex"></param>
		public float AxisSpeed(int axisIndex)
		{
			return this.currentState.AxisValue[axisIndex] - this.lastState.AxisValue[axisIndex];
		}
		
		/// <summary>
		/// Returns the current position of the specified joystick hat.
		/// </summary>
		/// <param name="hatIndex"></param>
		public JoystickHatPosition HatPosition(int hatIndex)
		{
			return this.currentState.HatPosition[hatIndex];
		}
		/// <summary>
		/// Returns all new hat displacement that occurred since last frame.
		/// </summary>
		/// <param name="hatIndex"></param>
		public JoystickHatPosition HatHit(int hatIndex)
		{
			return this.currentState.HatPosition[hatIndex] & (~this.lastState.HatPosition[hatIndex]);
		}
		/// <summary>
		/// Returns all old hat displacement that stopped since last frame.
		/// </summary>
		/// <param name="hatIndex"></param>
		public JoystickHatPosition HatReleased(int hatIndex)
		{
			return this.lastState.HatPosition[hatIndex] & (~this.currentState.HatPosition[hatIndex]);
		}
	}
}
