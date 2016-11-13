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
		/// [GET] Returns whether the specified device button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		/// <returns></returns>
		bool this[JoystickButton button] { get; }
		/// <summary>
		/// [GET] Returns the specified device axis current value.
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		float this[JoystickAxis axis] { get; }
		/// <summary>
		/// [GET] Returns the current position of the specified joystick hat.
		/// </summary>
		/// <param name="hat"></param>
		/// <returns></returns>
		JoystickHatPosition this[JoystickHat hat] { get; }
	}
}
