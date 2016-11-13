using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class GamepadButtonEventArgs : UserInputEventArgs
	{
		private GamepadButton button;
		private bool pressed;

		public GamepadButton Button
		{
			get { return this.button; }
		}
		public bool Pressed
		{
			get { return this.pressed; }
		}

		public GamepadButtonEventArgs(GamepadInput inputChannel, GamepadButton button, bool pressed) : base(inputChannel)
		{
			this.button = button;
			this.pressed = pressed;
		}
	}
}
