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
	}
}
