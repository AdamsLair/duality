using System;
using System.Text;

namespace Duality.Input
{
	/// <summary>
	/// Describes a source of user keyboard input. This is usually an input device.
	/// </summary>
	public interface IKeyboardInputSource : IUserInputSource
	{
		/// <summary>
		/// [GET] Returns the concatenated character input that was typed since the last input update.
		/// </summary>
		string CharInput { get; }
		/// <summary>
		/// [GET] Returns whether a specific key is currently pressed.
		/// </summary>
		/// <param name="key"></param>
		bool this[Key key] { get; }
	}
}
