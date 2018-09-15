using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Defines the type of a <see cref="Backend.INativeGraphicsBuffer"/>.
	/// </summary>
	public enum GraphicsBufferType
	{
		/// <summary>
		/// A buffer storing vertex data.
		/// </summary>
		Vertex,
		/// <summary>
		/// A buffer storing data for indexing vertices.
		/// </summary>
		Index
	}
}
