using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Input;


namespace Duality
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
