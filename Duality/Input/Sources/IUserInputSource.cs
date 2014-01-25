using System;
using OpenTK.Input;

namespace Duality
{
	/// <summary>
	/// Base interface for describing a source of user input.
	/// </summary>
	public interface IUserInputSource
	{
		/// <summary>
		/// [GET] A string containing a unique description for this instance.
		/// </summary>
		string Description { get; }
		/// <summary>
		/// [GET] Returns whether this input is currently available.
		/// </summary>
		bool IsAvailable { get; }

		/// <summary>
		/// Updates the sources current state.
		/// </summary>
		void UpdateState();
	}
}
