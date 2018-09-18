using System;

namespace Duality.Input
{
	/// <summary>
	/// Provides access to user keyboard input.
	/// </summary>
	public sealed class KeyboardInput : IUserInput
	{
		private class State
		{
			public bool		IsAvailable		= false;
			public bool[]	KeyPressed		= new bool[(int)Key.Last + 1];
			public string	CharInput		= string.Empty;

			public State() {}
			public State(State baseState)
			{
				baseState.CopyTo(this);
			}
			public void CopyTo(State other)
			{
				other.IsAvailable		= this.IsAvailable;
				other.CharInput			= this.CharInput;
				this.KeyPressed.CopyTo(other.KeyPressed, 0);
			}
			public void UpdateFromSource(IKeyboardInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				this.CharInput = source.CharInput ?? string.Empty;
				for (int i = 0; i < this.KeyPressed.Length; i++)
				{
					this.KeyPressed[i] = source[(Key)i];
				}
			}
		}

		private	IKeyboardInputSource	source			= null;
		private	State					currentState	= new State();
		private	State					lastState		= new State();


		/// <summary>
		/// [GET / SET] The keyboard inputs data source.
		/// </summary>
		public IKeyboardInputSource Source
		{
			get { return this.source; }
			set
			{
				if (this.source != value)
				{
					this.source = value;
				}
			}
		}
		IUserInputSource IUserInput.Source
		{
			get { return this.Source; }
			set { this.Source = value as IKeyboardInputSource; }
		}
		/// <summary>
		/// [GET] The unique id of this input.
		/// </summary>
		public string Id
		{
			get { return "Keyboard"; }
		}
		/// <summary>
		/// [GET] Returns whether this input is currently available.
		/// </summary>
		public bool IsAvailable
		{
			get { return this.currentState.IsAvailable; }
		}
		/// <summary>
		/// [GET] Returns the concatenated character input that was typed since the last input update.
		/// </summary>
		public string CharInput
		{
			get { return this.currentState.CharInput; }
		}
		/// <summary>
		/// [GET] Returns whether a specific key is currently pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool this[Key key]
		{
			get { return this.currentState.KeyPressed[(int)key]; }
		}

		/// <summary>
		/// Fired when a key is no longer pressed.
		/// </summary>
		public event EventHandler<KeyboardKeyEventArgs> KeyUp;
		/// <summary>
		/// Fired once when a key is pressed.
		/// </summary>
		public event EventHandler<KeyboardKeyEventArgs> KeyDown;
		/// <summary>
		/// Fired when keyboard input is no longer available to Duality.
		/// </summary>
		public event EventHandler NoLongerAvailable;
		/// <summary>
		/// Fired when keyboard input becomes available to Duality.
		/// </summary>
		public event EventHandler BecomesAvailable;
		

		internal KeyboardInput() {}
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
			for (int i = 0; i < this.currentState.KeyPressed.Length; i++)
			{
				if (this.currentState.KeyPressed[i] && !this.lastState.KeyPressed[i])
				{
					if (this.KeyDown != null)
						this.KeyDown(this, new KeyboardKeyEventArgs(this, (Key)i, this.currentState.KeyPressed[i]));
				}
				if (!this.currentState.KeyPressed[i] && this.lastState.KeyPressed[i])
				{
					if (this.KeyUp != null)
						this.KeyUp(this, new KeyboardKeyEventArgs(this, (Key)i, this.currentState.KeyPressed[i]));
				}
			}
		}
		void IUserInput.Update()
		{
			this.Update();
		}

		/// <summary>
		/// Returns whether the specified key is currently pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool KeyPressed(Key key)
		{
			return this.currentState.KeyPressed[(int)key];
		}
		/// <summary>
		/// Returns whether the specified key was hit this frame.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool KeyHit(Key key)
		{
			return this.currentState.KeyPressed[(int)key] && !this.lastState.KeyPressed[(int)key];
		}
		/// <summary>
		/// Returns whether the specified key was released this frame.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool KeyReleased(Key key)
		{
			return !this.currentState.KeyPressed[(int)key] && this.lastState.KeyPressed[(int)key];
		}
	}
}
