using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	public class GamepadAxisEventArgs : UserInputEventArgs
	{
		private GamepadAxis axis;
		private float axisValue;
		private float axisDelta;

		public GamepadAxis Axis
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

		public GamepadAxisEventArgs(GamepadInput inputChannel, GamepadAxis axis, float axisValue, float axisDelta) : base(inputChannel)
		{
			this.axis = axis;
			this.axisValue = axisValue;
			this.axisDelta = axisDelta;
		}
	}
}
