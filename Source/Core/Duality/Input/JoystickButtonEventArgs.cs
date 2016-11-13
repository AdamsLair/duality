using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickButtonEventArgs : UserInputEventArgs
	{
		private JoystickButton button;
		private bool pressed;

		public JoystickButton Button
		{
			get { return this.button; }
		}
		public bool Pressed
		{
			get { return this.pressed; }
		}

		public JoystickButtonEventArgs(JoystickInput inputChannel, JoystickButton button, bool pressed) : base(inputChannel)
		{
			this.button = button;
			this.pressed = pressed;
		}
	}
}
