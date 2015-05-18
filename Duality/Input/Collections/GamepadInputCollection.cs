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
	}
}
