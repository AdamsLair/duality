using System;
using OpenTK.Input;

namespace Duality
{
	public class OpenTKJoystickInputSource : IJoystickInputSource
	{
		private	JoystickDevice	device;
		
		public string Description
		{
			get { return this.device.Description; }
		}
		public bool IsAvailable
		{
			get { return true; }
		}
		public int AxisCount
		{
			get { return this.device.Axis.Count; }
		}
		public int ButtonCount
		{
			get { return this.device.Button.Count; }
		}
		public bool this[JoystickButton button]
		{
			get 
			{
				if (this.device.Button.Count > (int)button)
					return this.device.Button[button];
				else
					return false;
			}
		}
		public float this[JoystickAxis axis]
		{
			get 
			{
				if (this.device.Axis.Count > (int)axis)
					return this.device.Axis[axis];
				else
					return 0.0f;
			}
		}

		public OpenTKJoystickInputSource(JoystickDevice device)
		{
			this.device = device;
		}
	}
}
