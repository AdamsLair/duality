using System;
using System.Collections.Generic;

using Duality.Input;

namespace Duality.Backend.DefaultOpenTK
{
	public class GlobalJoystickInputSource : IJoystickInputSource
	{
		private static List<GlobalJoystickInputSource> cachedDevices = new List<GlobalJoystickInputSource>();

		private	int	deviceIndex;
		private	int	detectedHatCount;
		private	OpenTK.Input.JoystickState state;
		private	OpenTK.Input.JoystickCapabilities caps;
		
		public string Id
		{
			get { return string.Format("Joystick {0}", this.deviceIndex); }
		}
		public bool IsAvailable
		{
			get { return this.caps.IsConnected && (this.caps.AxisCount > 0 || this.caps.ButtonCount > 0 || this.caps.HatCount > 0); }
		}
		public int AxisCount
		{
			get { return this.caps.AxisCount; }
		}
		public int ButtonCount
		{
			get { return this.caps.ButtonCount; }
		}
		public int HatCount
		{
			get 
			{
				// Due to OpenTK sometimes reporting the wrong hat count, adjust its value when required
				if (this.caps.IsConnected)
					return Math.Max(this.detectedHatCount, this.caps.HatCount);
				else
					return this.caps.HatCount;
			}
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
		public JoystickHatPosition this[JoystickHat hat]
		{
			get 
			{
				if (this.HatCount > (int)hat)
					return GetDualityJoystickHatPosition(this.state.GetHat(GetOpenTKJoystickHat(hat)).Position);
				else
					return JoystickHatPosition.Centered;
			}
		}

		public GlobalJoystickInputSource(int deviceIndex)
		{
			this.deviceIndex = deviceIndex;
		}

		public void UpdateState()
		{
			this.caps = OpenTK.Input.Joystick.GetCapabilities(this.deviceIndex);
			this.state = OpenTK.Input.Joystick.GetState(this.deviceIndex);

			// Due to OpenTK sometimes reporting the wrong hat count, adjust its value when required
			{
				int highestInputHat = 0;

				if (this.state.GetHat(OpenTK.Input.JoystickHat.Hat3).Position != OpenTK.Input.HatPosition.Centered) highestInputHat = 4;
				else if (this.state.GetHat(OpenTK.Input.JoystickHat.Hat2).Position != OpenTK.Input.HatPosition.Centered) highestInputHat = 3;
				else if (this.state.GetHat(OpenTK.Input.JoystickHat.Hat1).Position != OpenTK.Input.HatPosition.Centered) highestInputHat = 2;
				else if (this.state.GetHat(OpenTK.Input.JoystickHat.Hat0).Position != OpenTK.Input.HatPosition.Centered) highestInputHat = 1;

				this.detectedHatCount = Math.Max(this.detectedHatCount, highestInputHat);
			}
		}

		public static void UpdateAvailableDecives(JoystickInputCollection inputManager)
		{
			const int MinDeviceCheckCount = 8;
			const int MaxDeviceCheckCount = 32;

			// Determine which devices are currently active already, so we can skip their indices
			List<int> skipIndices = null;
			foreach (JoystickInput input in inputManager)
			{
				GlobalJoystickInputSource existingDevice = input.Source as GlobalJoystickInputSource;
				if (existingDevice != null)
				{
					if (skipIndices == null) skipIndices = new List<int>();
					skipIndices.Add(existingDevice.deviceIndex);
				}
			}

			// Iterate over device indices and see what responds
			int deviceIndex = -1;
			while (deviceIndex < MaxDeviceCheckCount)
			{
				deviceIndex++;

				if (skipIndices != null && skipIndices.Contains(deviceIndex))
					continue;
				
				while (deviceIndex >= cachedDevices.Count)
				{
					cachedDevices.Add(new GlobalJoystickInputSource(cachedDevices.Count));
				}
				GlobalJoystickInputSource joystick = cachedDevices[deviceIndex];
				joystick.UpdateState();

				if (joystick.IsAvailable)
				{
					inputManager.AddSource(joystick);
					Logs.Core.Write(
						"Detected new Joystick Input: \"{0}\" at index {1}" + Environment.NewLine + 
						"Capabilities: {2} axes, {3} buttons, {4} hats", 
						joystick.Id, deviceIndex, 
						joystick.AxisCount, joystick.ButtonCount, joystick.HatCount);
				}
				else if (deviceIndex >= MinDeviceCheckCount)
					break;
			}
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
		private static OpenTK.Input.JoystickHat GetOpenTKJoystickHat(JoystickHat hat)
		{
			switch (hat)
			{
				case JoystickHat.Hat0:	return OpenTK.Input.JoystickHat.Hat0;
				case JoystickHat.Hat1:	return OpenTK.Input.JoystickHat.Hat1;
				case JoystickHat.Hat2:	return OpenTK.Input.JoystickHat.Hat2;
				case JoystickHat.Hat3:	return OpenTK.Input.JoystickHat.Hat3;
			}

			return OpenTK.Input.JoystickHat.Last;
		}
		private static JoystickHatPosition GetDualityJoystickHatPosition(OpenTK.Input.HatPosition hatPos)
		{
			switch (hatPos)
			{
				case OpenTK.Input.HatPosition.Centered:	 return JoystickHatPosition.Centered;
				case OpenTK.Input.HatPosition.Up:        return JoystickHatPosition.Up;
				case OpenTK.Input.HatPosition.UpLeft:    return JoystickHatPosition.Up | JoystickHatPosition.Left;
				case OpenTK.Input.HatPosition.UpRight:   return JoystickHatPosition.Up | JoystickHatPosition.Right;
				case OpenTK.Input.HatPosition.Right:     return JoystickHatPosition.Right;
				case OpenTK.Input.HatPosition.Left:      return JoystickHatPosition.Left;
				case OpenTK.Input.HatPosition.Down:      return JoystickHatPosition.Down;
				case OpenTK.Input.HatPosition.DownLeft:  return JoystickHatPosition.Down | JoystickHatPosition.Left;
				case OpenTK.Input.HatPosition.DownRight: return JoystickHatPosition.Down | JoystickHatPosition.Right;
			}

			return JoystickHatPosition.Centered;
		}
	}
}
