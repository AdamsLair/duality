using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Input;


namespace Duality
{
	public class GamepadAxisEventArgs : EventArgs
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

		public GamepadAxisEventArgs(GamepadAxis axis, float axisValue, float axisDelta)
		{
			this.axis = axis;
			this.axisValue = axisValue;
			this.axisDelta = axisDelta;
		}
	}
}
