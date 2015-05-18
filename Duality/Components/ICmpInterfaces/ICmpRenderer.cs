using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality
{
	/// <summary>
	/// Implement this interface in <see cref="Component">Components</see> that are considered renderable. 
	/// </summary>
	public interface ICmpRenderer
	{
		/// <summary>
		/// [GET] The Renderers bounding radius.
		/// </summary>
		float BoundRadius { get; }

		/// <summary>
		/// Determines whether or not this renderer is visible to the specified <see cref="IDrawDevice"/>.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which visibility is determined.</param>
		/// <returns>True, if this renderer is visible to the <see cref="IDrawDevice"/>. False, if not.</returns>
		bool IsVisible(IDrawDevice device);
		/// <summary>
		/// Draws the object.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which the object is drawn.</param>
		void Draw(IDrawDevice device);
	}
}
