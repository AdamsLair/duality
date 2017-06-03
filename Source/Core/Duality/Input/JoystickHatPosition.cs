using System;
using System.Collections.Generic;
using System.Linq;


namespace Duality.Input
{
	[Flags]
	public enum JoystickHatPosition
	{
		Centered = 0x0,

		Up       = 0x1,
		Right    = 0x2,
		Down     = 0x4,
		Left     = 0x8,
	}
}
