using System;
using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Describes a source of user mouse input. This is usually an input device.
	/// </summary>
	public interface IMouseInputSource : IUserInputSource
	{
		/// <summary>
		/// [GET / SET] The current viewport-local cursor X position.
		/// </summary>
		int X { get; set; }
		/// <summary>
		/// [GET / SET] The current viewport-local cursor Y position.
		/// </summary>
		int Y { get; set; }
		/// <summary>
		/// [GET] The current mouse wheel value
		/// </summary>
		float Wheel { get; }
		/// <summary>
		/// [GET] Returns whether a specific <see cref="MouseButton"/> is currently pressed.
		/// </summary>
		bool this[MouseButton btn] { get; }
	}
}
