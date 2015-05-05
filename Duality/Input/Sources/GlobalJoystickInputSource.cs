using System;
using OpenTK.Input;

namespace Duality.Input
{
	public class GlobalJoystickInputSource : IJoystickInputSource
	{
		private	int	deviceIndex;
		private	JoystickState state;
		private	JoystickCapabilities caps;
		
		public string Description
		{
			get { return string.Format("Joystick {0}", this.deviceIndex); }
		}
		public bool IsAvailable
		{
			get { return this.caps.IsConnected; }
		}
		public int AxisCount
		{
			get { return this.caps.AxisCount; }
		}
		public int ButtonCount
		{
			get { return this.caps.ButtonCount; }
		}
		public bool this[JoystickButton button]
		{
			get 
			{
				if (this.ButtonCount > (int)button)
					return this.state.IsButtonDown(GetOpenTKJoystickButton(button));
				else
					return false;
			}
		}
		public float this[JoystickAxis axis]
		{
			get 
			{
				if (this.AxisCount > (int)axis)
					return this.state.GetAxis(GetOpenTKJoystickAxis(axis));
				else
					return 0.0f;
			}
		}

		public GlobalJoystickInputSource(int deviceIndex)
		{
			this.deviceIndex = deviceIndex;
		}

		public void UpdateState()
		{
			this.caps = Joystick.GetCapabilities(this.deviceIndex);
			this.state = Joystick.GetState(this.deviceIndex);
		}

		private static OpenTK.Input.JoystickButton GetOpenTKJoystickButton(JoystickButton button)
		{
			switch (button)
			{
				case JoystickButton.Button0:	return OpenTK.Input.JoystickButton.Button0;
				case JoystickButton.Button1:	return OpenTK.Input.JoystickButton.Button1;
				case JoystickButton.Button2:	return OpenTK.Input.JoystickButton.Button2;
				case JoystickButton.Button3:	return OpenTK.Input.JoystickButton.Button3;
				case JoystickButton.Button4:	return OpenTK.Input.JoystickButton.Button4;
				case JoystickButton.Button5:	return OpenTK.Input.JoystickButton.Button5;
				case JoystickButton.Button6:	return OpenTK.Input.JoystickButton.Button6;
				case JoystickButton.Button7:	return OpenTK.Input.JoystickButton.Button7;
				case JoystickButton.Button8:	return OpenTK.Input.JoystickButton.Button8;
				case JoystickButton.Button9:	return OpenTK.Input.JoystickButton.Button9;
				case JoystickButton.Button10:	return OpenTK.Input.JoystickButton.Button10;
				case JoystickButton.Button11:	return OpenTK.Input.JoystickButton.Button11;
				case JoystickButton.Button12:	return OpenTK.Input.JoystickButton.Button12;
				case JoystickButton.Button13:	return OpenTK.Input.JoystickButton.Button13;
				case JoystickButton.Button14:	return OpenTK.Input.JoystickButton.Button14;
				case JoystickButton.Button15:	return OpenTK.Input.JoystickButton.Button15;
				case JoystickButton.Button16:	return OpenTK.Input.JoystickButton.Button16;
				case JoystickButton.Button17:	return OpenTK.Input.JoystickButton.Button17;
				case JoystickButton.Button18:	return OpenTK.Input.JoystickButton.Button18;
				case JoystickButton.Button19:	return OpenTK.Input.JoystickButton.Button19;
				case JoystickButton.Button20:	return OpenTK.Input.JoystickButton.Button20;
				case JoystickButton.Button21:	return OpenTK.Input.JoystickButton.Button21;
				case JoystickButton.Button22:	return OpenTK.Input.JoystickButton.Button22;
				case JoystickButton.Button23:	return OpenTK.Input.JoystickButton.Button23;
				case JoystickButton.Button24:	return OpenTK.Input.JoystickButton.Button24;
				case JoystickButton.Button25:	return OpenTK.Input.JoystickButton.Button25;
				case JoystickButton.Button26:	return OpenTK.Input.JoystickButton.Button26;
				case JoystickButton.Button27:	return OpenTK.Input.JoystickButton.Button27;
				case JoystickButton.Button28:	return OpenTK.Input.JoystickButton.Button28;
				case JoystickButton.Button29:	return OpenTK.Input.JoystickButton.Button29;
				case JoystickButton.Button30:	return OpenTK.Input.JoystickButton.Button30;
				case JoystickButton.Button31:	return OpenTK.Input.JoystickButton.Button31;
			}

			return OpenTK.Input.JoystickButton.Last;
		}
		private static OpenTK.Input.JoystickAxis GetOpenTKJoystickAxis(JoystickAxis axis)
		{
			switch (axis)
			{
				case JoystickAxis.Axis0:	return OpenTK.Input.JoystickAxis.Axis0;
				case JoystickAxis.Axis1:	return OpenTK.Input.JoystickAxis.Axis1;
				case JoystickAxis.Axis2:	return OpenTK.Input.JoystickAxis.Axis2;
				case JoystickAxis.Axis3:	return OpenTK.Input.JoystickAxis.Axis3;
				case JoystickAxis.Axis4:	return OpenTK.Input.JoystickAxis.Axis4;
				case JoystickAxis.Axis5:	return OpenTK.Input.JoystickAxis.Axis5;
				case JoystickAxis.Axis6:	return OpenTK.Input.JoystickAxis.Axis6;
				case JoystickAxis.Axis7:	return OpenTK.Input.JoystickAxis.Axis7;
				case JoystickAxis.Axis8:	return OpenTK.Input.JoystickAxis.Axis8;
				case JoystickAxis.Axis9:	return OpenTK.Input.JoystickAxis.Axis9;
				case JoystickAxis.Axis10:	return OpenTK.Input.JoystickAxis.Axis10;
				case JoystickAxis.Axis11:	return OpenTK.Input.JoystickAxis.Last;		// OpenTK only has 11 axes, not 12
			}

			return OpenTK.Input.JoystickAxis.Last;
		}
	}
}
