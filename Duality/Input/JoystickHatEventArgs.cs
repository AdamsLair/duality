using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickHatEventArgs : UserInputEventArgs
	{
		private JoystickHat hat;
		private JoystickHatPosition hatPos;
		private JoystickHatPosition lastHatPos;

		public JoystickHat Hat
		{
			get { return this.hat; }
		}
		public JoystickHatPosition HatPosition
		{
			get { return this.hatPos; }
		}
		public JoystickHatPosition LastHatPosition
		{
			get { return this.lastHatPos; }
		}

		public JoystickHatEventArgs(JoystickInput inputChannel, JoystickHat hat, JoystickHatPosition hatPos, JoystickHatPosition lastHatPos) : base(inputChannel)
		{
			this.hat = hat;
			this.hatPos = hatPos;
			this.lastHatPos = lastHatPos;
		}
	}
}
