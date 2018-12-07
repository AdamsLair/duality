using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class JoystickAxisEventArgs : UserInputEventArgs
	{
		private int axisIndex;
		private float axisValue;
		private float axisDelta;

		public int AxisIndex
		{
			get { return this.axisIndex; }
		}
		public float Value
		{
			get { return this.axisValue; }
		}
		public float Delta
		{
			get { return this.axisDelta; }
		}

		public JoystickAxisEventArgs(JoystickInput inputChannel, int axisIndex, float axisValue, float axisDelta) : base(inputChannel)
		{
			this.axisIndex = axisIndex;
			this.axisValue = axisValue;
			this.axisDelta = axisDelta;
		}
	}
}
