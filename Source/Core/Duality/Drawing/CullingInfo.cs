using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Duality.Resources;
using Duality.Backend;

namespace Duality.Drawing
{
	/// <summary>
	/// Holds culling information about a renderer object, which can be
	/// used to determine whether it could be visible to any given observer.
	/// </summary>
	public struct CullingInfo
	{
		/// <summary>
		/// The renderers reference position in world space coordinates.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Radius of the renderers bounding sphere originating at its world space <see cref="Position"/>.
		/// </summary>
		public float Radius;
		/// <summary>
		/// A bitmask that describes the renderers visibility in each rendering group.
		/// </summary>
		public VisibilityFlag Visibility;
	}
}
