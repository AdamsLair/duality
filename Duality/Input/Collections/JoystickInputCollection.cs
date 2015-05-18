using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Duality.Input
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
	}
}
