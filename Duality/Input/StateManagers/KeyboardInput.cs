using System;
using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Provides access to user keyboard input.
	/// </summary>
	public sealed class KeyboardInput : IUserInput
	{
		private class State
		{
			public bool		IsAvailable		= false;
			public bool		KeyRepeat		= true;
			public int		KeyRepeatCount	= 0;
			public bool[]	KeyPressed		= new bool[(int)Key.LastKey + 1];

			public State() {}
			public State(State baseState)
			{
				baseState.CopyTo(this);
			}
			public void CopyTo(State other)
			{
				other.IsAvailable		= this.IsAvailable;
				other.KeyRepeat			= this.KeyRepeat;
				other.KeyRepeatCount	= this.KeyRepeatCount;
				this.KeyPressed.CopyTo(other.KeyPressed, 0);
			}
			public void UpdateFromSource(IKeyboardInputSource source)
			{
				this.IsAvailable = source != null ? source.IsAvailable : false;
				if (source == null) return;

				this.KeyRepeat = source.KeyRepeat;
				this.KeyRepeatCount = source.KeyRepeatCounter;
				for (int i = 0; i < this.KeyPressed.Length; i++)
				{
					this.KeyPressed[i] = source[(Key)i];
				}
			}
		}

		private	IKeyboardInputSource	source			= null;
		private	State					currentState	= new State();
		private	State					lastState		= new State();
		private	bool					anyNewKeydown	= false;


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

					if (this.source != null)
					{
						this.source.KeyRepeat = this.currentState.KeyRepeat;
					}
				}
			}
		}
		IUserInputSource IUserInput.Source
		{
			get { return this.Source; }
			set { this.Source = value as IKeyboardInputSource; }
		}
		/// <summary>
		/// [GET] A text description of this input.
		/// </summary>
		public string Description
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
		/// [GET / SET] Whether a key that is pressed and hold down should fire the <see cref="KeyDown"/> event repeatedly.
		/// </summary>
		public bool KeyRepeat
		{
			get { return this.currentState.KeyRepeat; }
			set 
			{
				this.currentState.KeyRepeat = value;
				if (this.source != null) this.source.KeyRepeat = this.currentState.KeyRepeat;
			}
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
		/// Fired once when a key is pressed. May be fired repeatedly, if <see cref="KeyRepeat"/> is true.
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
			this.anyNewKeydown = false;
			for (int i = 0; i < this.currentState.KeyPressed.Length; i++)
			{
				if (this.currentState.KeyPressed[i] && !this.lastState.KeyPressed[i])
				{
					this.anyNewKeydown = true;
					if (this.KeyDown != null)
						this.KeyDown(this, new KeyboardKeyEventArgs((Key)i));
				}
				if (!this.currentState.KeyPressed[i] && this.lastState.KeyPressed[i])
				{
					if (this.KeyUp != null)
						this.KeyUp(this, new KeyboardKeyEventArgs((Key)i));
				}
			}
			if (!this.anyNewKeydown && this.currentState.KeyRepeatCount != this.lastState.KeyRepeatCount && this.currentState.KeyRepeat)
			{
				for (int i = 0; i < this.currentState.KeyPressed.Length; i++)
				{
					if (this.currentState.KeyPressed[i])
					{
						if (this.KeyDown != null)
							this.KeyDown(this, new KeyboardKeyEventArgs((Key)i));
					}
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
			if (!this.currentState.KeyPressed[(int)key]) return false;
			return 
				(!this.lastState.KeyPressed[(int)key]) || 
				(this.currentState.KeyRepeat && !this.anyNewKeydown && this.currentState.KeyRepeatCount != this.lastState.KeyRepeatCount);
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
