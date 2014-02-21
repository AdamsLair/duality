using System;
using OpenTK;

namespace Duality
{
	/// <summary>
	/// Describes the state of object activation or disposal.
	/// </summary>
	public enum InitState
	{
		/// <summary>
		/// The object is currently initializing.
		/// </summary>
		Initializing,
		/// <summary>
		/// The object has been fully initialized and is fully operational.
		/// </summary>
		Initialized,
		/// <summary>
		/// The object is currently disposing.
		/// </summary>
		Disposing,
		/// <summary>
		/// The object has been fully disposed and can be considered "dead".
		/// </summary>
		Disposed
	}

	public static class ExtMethodsInitState
	{
		/// <summary>
		/// Returns whether the current <see cref="InitState"/> can be considered active. This is true
		/// after initialization and during disposal.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static bool IsActive(this InitState state)
		{
			return state == InitState.Initialized || state == InitState.Disposing;
		}
	}
}
