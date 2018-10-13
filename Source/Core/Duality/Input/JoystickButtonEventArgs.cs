using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickButtonEventArgs : UserInputEventArgs
	{
		private int buttonIndex;
		private bool pressed;

		public int ButtonIndex
		{
			get { return this.buttonIndex; }
		}
		public bool Pressed
		{
			get { return this.pressed; }
		}

		public JoystickButtonEventArgs(JoystickInput inputChannel, int buttonIndex, bool pressed) : base(inputChannel)
		{
			this.buttonIndex = buttonIndex;
			this.pressed = pressed;
		}
	}
}
