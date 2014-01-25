using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality
{
	public interface IUserInput
	{
		/// <summary>
		/// [GET] A text description of this input.
		/// </summary>
		string Description { get; }
		/// <summary>
		/// [GET] Returns whether this input is currently available.
		/// </summary>
		bool IsAvailable { get; }
		/// <summary>
		/// [GET / SET] The source where this input derives its state from.
		/// </summary>
		IUserInputSource Source { get; set; }
		
		/// <summary>
		/// Fired when the input becomes available to Duality.
		/// </summary>
		event EventHandler BecomesAvailable;
		/// <summary>
		/// Fired when the input is no longer available to Duality.
		/// </summary>
		event EventHandler NoLongerAvailable;

		/// <summary>
		/// Updates the inputs current state.
		/// </summary>
		void Update();
	}
}
