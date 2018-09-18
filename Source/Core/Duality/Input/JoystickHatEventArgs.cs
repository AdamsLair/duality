using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickHatEventArgs : UserInputEventArgs
	{
		private int hatIndex;
		private JoystickHatPosition hatPos;
		private JoystickHatPosition lastHatPos;

		public int HatIndex
		{
			get { return this.hatIndex; }
		}
		public JoystickHatPosition HatPosition
		{
			get { return this.hatPos; }
		}
		public JoystickHatPosition LastHatPosition
		{
			get { return this.lastHatPos; }
		}

		public JoystickHatEventArgs(JoystickInput inputChannel, int hatIndex, JoystickHatPosition hatPos, JoystickHatPosition lastHatPos) : base(inputChannel)
		{
			this.hatIndex = hatIndex;
			this.hatPos = hatPos;
			this.lastHatPos = lastHatPos;
		}
	}
}
