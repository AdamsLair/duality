using System;

namespace Duality.Input
{
	/// <summary>
	/// Describes a source of extended user input such as joysticks or gamepads. This is usually an input device.
	/// </summary>
	public interface IJoystickInputSource : IUserInputSource
	{
		/// <summary>
		/// [GET] Returns the number of axes.
		/// </summary>
		int AxisCount { get; }
		/// <summary>
		/// [GET] Returns the number of buttons.
		/// </summary>
		int ButtonCount { get; }
		/// <summary>
		/// [GET] Returns the number of hats.
		/// </summary>
		int HatCount { get; }

		/// <summary>
		/// Returns whether the specified device button is currently pressed.
		/// </summary>
		/// <param name="buttonIndex"></param>
		/// <returns></returns>
		bool ButtonPressed(int buttonIndex);
		/// <summary>
		/// Returns the specified device axis current value.
		/// </summary>
		/// <param name="axisIndex"></param>
		/// <returns></returns>
		float AxisValue(int axisIndex);
		/// <summary>
		/// [GET] Returns the current position of the specified joystick hat.
		/// </summary>
		/// <param name="hatIndex"></param>
		/// <returns></returns>
		JoystickHatPosition HatPosition(int hatIndex);
	}
}
