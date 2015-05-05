using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickAxisEventArgs : UserInputEventArgs
	{
		private JoystickAxis axis;
		private float axisValue;
		private float axisDelta;

		public JoystickAxis Axis
		{
			get { return this.axis; }
		}
		public float Value
		{
			get { return this.axisValue; }
		}
		public float Delta
		{
			get { return this.axisDelta; }
		}

		public JoystickAxisEventArgs(JoystickInput inputChannel, JoystickAxis axis, float axisValue, float axisDelta) : base(inputChannel)
		{
			this.axis = axis;
			this.axisValue = axisValue;
			this.axisDelta = axisDelta;
		}
	}
}
