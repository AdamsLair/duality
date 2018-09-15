using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Components.Physics
{
	/// <summary>
	/// Describes the geometry of a vertex-based shape, i.e.
	/// how the vertices are to be interpreted.
	/// </summary>
	[Flags]
	public enum VertexShapeTrait
	{
		/// <summary>
		/// The shape does not have any of the available traits.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// A flag indicating that the first and the last vertex
		/// are connected.
		/// </summary>
		IsLoop = 0x1,
		/// <summary>
		/// A flag indicating that the described shape is solid, i.e.
		/// a point inside the described polygon is considered to be
		/// inside the shape.
		/// </summary>
		IsSolid = 0x2
	}
}
