using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class GamepadButtonEventArgs : EventArgs
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

		public GamepadButtonEventArgs(GamepadButton button, bool pressed)
		{
			this.button = button;
			this.pressed = pressed;
		}
	}
}
