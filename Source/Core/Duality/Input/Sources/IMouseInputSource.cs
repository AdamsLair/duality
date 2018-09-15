using System;

namespace Duality.Input
{
	/// <summary>
	/// Describes a source of user mouse input. This is usually an input device.
	/// </summary>
	public interface IMouseInputSource : IUserInputSource
	{
		/// <summary>
		/// [GET / SET] The current window-local cursor X position in native window coordinates.
		/// </summary>
		Point2 Pos { get; set; }
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
