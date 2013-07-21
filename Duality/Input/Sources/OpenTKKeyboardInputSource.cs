using System;
using OpenTK.Input;

namespace Duality
{
	public class OpenTKKeyboardInputSource : IKeyboardInputSource
	{
		private	KeyboardDevice	device;
		private bool			hasFocus;
		private	int				repeatCounter;

		public bool KeyRepeat
		{
			get { return this.device.KeyRepeat; }
			set { this.device.KeyRepeat = value; }
		}
		public bool HasFocus
		{
			get { return this.hasFocus; }
			set { this.hasFocus = value; }
		}
		public int KeyRepeatCounter
		{
			get { return this.repeatCounter; }
		}
		public bool this[Key key]
		{
			get { return this.device[key]; }
		}

		public OpenTKKeyboardInputSource(KeyboardDevice device)
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
	}
}
