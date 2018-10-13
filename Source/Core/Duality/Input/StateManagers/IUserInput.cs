﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Input
{
	public interface IUserInput
	{
		/// <summary>
		/// [GET] The unique ID of this input.
		/// </summary>
		string Id { get; }
		/// <summary>
		/// [GET] The unique ID of the product that is providing this input.
		/// </summary>
		Guid ProductId { get; }
		/// <summary>
		/// [GET] The name of the product that is providing this input.
		/// </summary>
		string ProductName { get; }
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
