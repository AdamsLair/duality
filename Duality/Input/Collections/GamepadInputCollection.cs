using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Duality.Input
{
	/// <summary>
	/// Provides access to a set of <see cref="GamepadInput">GamepadInputs</see>.
	/// </summary>
	public sealed class GamepadInputCollection : UserInputCollection<GamepadInput,IGamepadInputSource>
	{
		protected override GamepadInput CreateInput(IGamepadInputSource source)
		{
			GamepadInput input = new GamepadInput();
			input.Source = source;
			return input;
		}
		protected override GamepadInput CreateDummyInput()
		{
			return new GamepadInput(true);
		}

		public void AddGlobalDevices()
		{
			const int MinDeviceCheckCount = 8;
			int deviceIndex = 0;
			while (true)
			{
				GlobalGamepadInputSource gamepad = new GlobalGamepadInputSource(deviceIndex);
				gamepad.UpdateState();

				if (gamepad.IsAvailable)
					this.AddSource(gamepad);
				else if (deviceIndex >= MinDeviceCheckCount)
					break;

				deviceIndex++;
			}
		}
	}
}
