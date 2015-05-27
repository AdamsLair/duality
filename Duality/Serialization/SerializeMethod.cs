using System;

namespace Duality.Serialization
{
	/// <summary>
	/// Represents a set of data serialization methods.
	/// </summary>
	public enum SerializeMethod
	{
		/// <summary>
		/// An unknown serialization method
		/// </summary>
		Unknown,

		/// <summary>
		/// Text-based XML serialization
		/// </summary>
		Xml,
		/// <summary>
		/// Binary serialization
		/// </summary>
		Binary
	}
}
