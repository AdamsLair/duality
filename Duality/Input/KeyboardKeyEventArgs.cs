using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class KeyboardKeyEventArgs : EventArgs
	{
		private Key key;
		private bool pressed;

		public Key Key
		{
			get { return this.key; }
		}
		public bool IsPressed
		{
			get { return this.pressed; }
		}

		public KeyboardKeyEventArgs(Key key, bool pressed)
		{
			this.key = key;
			this.pressed = pressed;
		}
	}
}
