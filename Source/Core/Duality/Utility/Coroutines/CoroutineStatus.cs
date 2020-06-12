using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duality.Utility.Coroutines
{
	/// <summary>
	/// A Coroutine's current execution Status
	/// </summary>
	public enum CoroutineStatus : byte
	{
		/// <summary>
		/// The coroutine is paused
		/// </summary>
		Paused,
		/// <summary>
		/// The coroutine is running
		/// </summary>
		Running,
		/// <summary>
		/// The coroutine's execution has been completed without errors
		/// </summary>
		Complete,
		/// <summary>
		/// The coroutine's execution has been cancelled
		/// </summary>
		Cancelled,
		/// <summary>
		/// The coroutine has encountered an error
		/// </summary>
		Error
	}
}
