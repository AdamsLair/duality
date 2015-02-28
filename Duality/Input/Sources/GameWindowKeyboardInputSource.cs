using System;
using System.Text;

using OpenTK;
using OpenTK.Input;

namespace Duality
{
	public class GameWindowKeyboardInputSource : IKeyboardInputSource
	{
		private	GameWindow		window;
		private bool			hasFocus;
		private	int				repeatCounter;
		private	string			charInput;
		private	StringBuilder	charInputBuffer = new StringBuilder();
		
		public string Description
		{
			get { return "Keyboard"; }
		}
		public bool IsAvailable
		{
			get { return this.window != null && this.window.Keyboard != null && this.hasFocus; }
		}
		public bool KeyRepeat
		{
			get { return this.window.Keyboard.KeyRepeat; }
			set { this.window.Keyboard.KeyRepeat = value; }
		}
		public int KeyRepeatCounter
		{
			get { return this.repeatCounter; }
		}
		public string CharInput
		{
			get { return this.charInput ?? string.Empty; }
		}
		public bool this[Key key]
		{
			get { return this.window.Keyboard[key]; }
		}

		public GameWindowKeyboardInputSource(GameWindow window)
		{
			this.window = window;
			this.window.Keyboard.GotFocus += this.device_GotFocus;
			this.window.Keyboard.LostFocus += this.device_LostFocus;
			this.window.Keyboard.KeyDown += this.device_KeyDown;
			this.window.KeyPress += this.window_KeyPress;
		}

		private void device_LostFocus(object sender, EventArgs e)
		{
			this.hasFocus = false;
		}
		private void device_GotFocus(object sender, EventArgs e)
		{
			this.hasFocus = true;
		}
		private void device_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			this.repeatCounter++;
		}
		private void window_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.charInputBuffer.Append(e.KeyChar);
		}

		public void UpdateState()
		{
			this.charInput = this.charInputBuffer.ToString();
			this.charInputBuffer.Clear();
		}
	}
}
