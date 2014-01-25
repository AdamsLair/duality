using System;
using OpenTK.Input;

namespace Duality
{
	public class GameWindowKeyboardInputSource : IKeyboardInputSource
	{
		private	KeyboardDevice	device;
		private bool			hasFocus;
		private	int				repeatCounter;
		
		public string Description
		{
			get { return "Keyboard"; }
		}
		public bool IsAvailable
		{
			get { return this.device != null && this.hasFocus; }
		}
		public bool KeyRepeat
		{
			get { return this.device.KeyRepeat; }
			set { this.device.KeyRepeat = value; }
		}
		public int KeyRepeatCounter
		{
			get { return this.repeatCounter; }
		}
		public bool this[Key key]
		{
			get { return this.device[key]; }
		}

		public GameWindowKeyboardInputSource(KeyboardDevice device)
		{
			this.device = device;
			this.device.GotFocus += this.device_GotFocus;
			this.device.LostFocus += this.device_LostFocus;
			this.device.KeyDown += this.device_KeyDown;
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

		public void UpdateState() {}
	}
}
