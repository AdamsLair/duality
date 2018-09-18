using System;

namespace Duality.Input
{
	/// <summary>
	/// Base interface for describing a source of user input.
	/// </summary>
	public interface IUserInputSource
	{
		/// <summary>
		/// [GET] The unique id of this input source.
		/// </summary>
		string Id { get; }
		/// <summary>
		/// [GET] The unique ID of the product that is providing this input.
		/// </summary>
		Guid ProductId { get; }
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
