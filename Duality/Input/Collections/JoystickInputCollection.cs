using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Provides access to a set of <see cref="JoystickInput">JoystickInputs</see>.
	/// </summary>
	public sealed class JoystickInputCollection : UserInputCollection<JoystickInput,IJoystickInputSource>
	{
		protected override JoystickInput CreateInput(IJoystickInputSource source)
		{
			JoystickInput input = new JoystickInput();
			input.Source = source;
			return input;
		}
		protected override JoystickInput CreateDummyInput()
		{
			return new JoystickInput(true);
		}

		public void AddGlobalDevices()
		{
			const int MinDeviceCheckCount = 8;
			int deviceIndex = 0;
			while (true)
			{
				GlobalJoystickInputSource joystick = new GlobalJoystickInputSource(deviceIndex);
				joystick.UpdateState();

				if (joystick.IsAvailable)
					this.AddSource(joystick);
				else if (deviceIndex >= MinDeviceCheckCount)
					break;

				deviceIndex++;
			}
		}
	}
}
