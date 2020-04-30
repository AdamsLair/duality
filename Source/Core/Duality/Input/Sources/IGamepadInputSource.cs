﻿using System;

namespace Duality.Input
{
	/// <summary>
	/// Describes a source of gamepad user input. This is usually an input device.
	/// </summary>
	public interface IGamepadInputSource : IUserInputSource
	{
		/// <summary>
		/// [GET] Returns whether the specified gamepad button is currently pressed.
		/// </summary>
		/// <param name="button"></param>
		bool this[GamepadButton button] { get; }
		/// <summary>
		/// [GET] Returns the specified gamepad axis value.
		/// </summary>
		/// <param name="axis"></param>
		float this[GamepadAxis axis] { get; }

		/// <summary>
		/// Sets the gamepads current vibration values.
		/// </summary>
		/// <param name="left">Left vibration between 0.0 and 1.0</param>
		/// <param name="right">Right vibration between 0.0 and 1.0</param>
		void SetVibration(float left, float right);
	}
}
