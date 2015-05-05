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
		/// [GET / SET] Whether a key that is pressed and hold down should fire the KeyDown event repeatedly.
		/// </summary>
		bool KeyRepeat { get; set; }
		/// <summary>
		/// [GET] Returns the current key repeat counter value. A key repeat event will be fired for each increment that isn't accompanied by a state change.
		/// </summary>
		int KeyRepeatCounter { get; }
		/// <summary>
		/// [GET] Returns the concatenated character input that was typed since the last input update.
		/// </summary>
		string CharInput { get; }
		/// <summary>
		/// [GET] Returns whether a specific key is currently pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool this[Key key] { get; }
	}
}
