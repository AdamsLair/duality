using System;
using System.Collections.Generic;

using Duality.Input;

namespace Duality.Backend.DefaultOpenTK
{
	public class GlobalJoystickInputSource : IJoystickInputSource
	{
		private static List<GlobalJoystickInputSource> cachedDevices = new List<GlobalJoystickInputSource>();

		private Guid productId;
		private int deviceIndex;
		private int detectedHatCount;
		private OpenTK.Input.JoystickState state;
		private OpenTK.Input.JoystickCapabilities caps;

		public string Id
		{
			get { return string.Format("Joystick {0}", this.deviceIndex); }
		}
		public Guid ProductId
		{
			get { return this.productId; }
		}
		public string ProductName
		{
			get { return "Joystick"; }
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


		public GlobalJoystickInputSource(int deviceIndex)
		{
			this.deviceIndex = deviceIndex;
		}

		public void UpdateState()
		{
			this.productId = OpenTK.Input.Joystick.GetGuid(this.deviceIndex);
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

		public bool ButtonPressed(int buttonIndex)
		{
			if (buttonIndex >= 0 && buttonIndex < this.ButtonCount)
				return this.state.IsButtonDown(GetOpenTKJoystickButton(buttonIndex));
			else
				return false;
		}
		public float AxisValue(int axisIndex)
		{
			if (axisIndex >= 0 && axisIndex < this.AxisCount)
				return this.state.GetAxis(GetOpenTKJoystickAxis(axisIndex));
			else
				return 0.0f;
		}
		public JoystickHatPosition HatPosition(int hatIndex)
		{
			if (hatIndex >= 0 && hatIndex < this.HatCount)
				return GetDualityJoystickHatPosition(this.state.GetHat(GetOpenTKJoystickHat(hatIndex)).Position);
			else
				return JoystickHatPosition.Centered;
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
						"Detected new Joystick Input: \"{0}\" ({1} | {2}) at index {3}" + Environment.NewLine + 
						"Capabilities: {4} axes, {5} buttons, {6} hats", 
						joystick.Id, 
						joystick.ProductId, 
						joystick.ProductName,
						deviceIndex, 
						joystick.AxisCount, 
						joystick.ButtonCount, 
						joystick.HatCount);
				}
				else if (deviceIndex >= MinDeviceCheckCount)
					break;
			}
		}

		private static OpenTK.Input.JoystickButton GetOpenTKJoystickButton(int buttonIndex)
		{
			switch (buttonIndex)
			{
				case 0:  return OpenTK.Input.JoystickButton.Button0;
				case 1:  return OpenTK.Input.JoystickButton.Button1;
				case 2:  return OpenTK.Input.JoystickButton.Button2;
				case 3:  return OpenTK.Input.JoystickButton.Button3;
				case 4:  return OpenTK.Input.JoystickButton.Button4;
				case 5:  return OpenTK.Input.JoystickButton.Button5;
				case 6:  return OpenTK.Input.JoystickButton.Button6;
				case 7:  return OpenTK.Input.JoystickButton.Button7;
				case 8:  return OpenTK.Input.JoystickButton.Button8;
				case 9:  return OpenTK.Input.JoystickButton.Button9;
				case 10: return OpenTK.Input.JoystickButton.Button10;
				case 11: return OpenTK.Input.JoystickButton.Button11;
				case 12: return OpenTK.Input.JoystickButton.Button12;
				case 13: return OpenTK.Input.JoystickButton.Button13;
				case 14: return OpenTK.Input.JoystickButton.Button14;
				case 15: return OpenTK.Input.JoystickButton.Button15;
				case 16: return OpenTK.Input.JoystickButton.Button16;
				case 17: return OpenTK.Input.JoystickButton.Button17;
				case 18: return OpenTK.Input.JoystickButton.Button18;
				case 19: return OpenTK.Input.JoystickButton.Button19;
				case 20: return OpenTK.Input.JoystickButton.Button20;
				case 21: return OpenTK.Input.JoystickButton.Button21;
				case 22: return OpenTK.Input.JoystickButton.Button22;
				case 23: return OpenTK.Input.JoystickButton.Button23;
				case 24: return OpenTK.Input.JoystickButton.Button24;
				case 25: return OpenTK.Input.JoystickButton.Button25;
				case 26: return OpenTK.Input.JoystickButton.Button26;
				case 27: return OpenTK.Input.JoystickButton.Button27;
				case 28: return OpenTK.Input.JoystickButton.Button28;
				case 29: return OpenTK.Input.JoystickButton.Button29;
				case 30: return OpenTK.Input.JoystickButton.Button30;
				case 31: return OpenTK.Input.JoystickButton.Button31;
			}

			return OpenTK.Input.JoystickButton.Last;
		}
		private static OpenTK.Input.JoystickAxis GetOpenTKJoystickAxis(int axisIndex)
		{
			switch (axisIndex)
			{
				case 0:  return OpenTK.Input.JoystickAxis.Axis0;
				case 1:  return OpenTK.Input.JoystickAxis.Axis1;
				case 2:  return OpenTK.Input.JoystickAxis.Axis2;
				case 3:  return OpenTK.Input.JoystickAxis.Axis3;
				case 4:  return OpenTK.Input.JoystickAxis.Axis4;
				case 5:  return OpenTK.Input.JoystickAxis.Axis5;
				case 6:  return OpenTK.Input.JoystickAxis.Axis6;
				case 7:  return OpenTK.Input.JoystickAxis.Axis7;
				case 8:  return OpenTK.Input.JoystickAxis.Axis8;
				case 9:  return OpenTK.Input.JoystickAxis.Axis9;
				case 10: return OpenTK.Input.JoystickAxis.Axis10;
				case 11: return OpenTK.Input.JoystickAxis.Last; // OpenTK only has 11 axes, not 12
			}

			return OpenTK.Input.JoystickAxis.Last;
		}
		private static OpenTK.Input.JoystickHat GetOpenTKJoystickHat(int hatIndex)
		{
			switch (hatIndex)
			{
				case 0: return OpenTK.Input.JoystickHat.Hat0;
				case 1: return OpenTK.Input.JoystickHat.Hat1;
				case 2: return OpenTK.Input.JoystickHat.Hat2;
				case 3: return OpenTK.Input.JoystickHat.Hat3;
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
