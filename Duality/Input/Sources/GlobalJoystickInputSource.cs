using System;
using OpenTK.Input;

namespace Duality
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
					return this.state.IsButtonDown(button);
				else
					return false;
			}
		}
		public float this[JoystickAxis axis]
		{
			get 
			{
				if (this.AxisCount > (int)axis)
					return this.state.GetAxis(axis);
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
	}
}
