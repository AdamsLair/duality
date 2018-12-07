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
		/// Retrieves information that can be used to decide whether this renderer could 
		/// be visible to any given observer or not.
		/// </summary>
		/// <param name="info"></param>
		void GetCullingInfo(out CullingInfo info);
		/// <summary>
		/// Draws the object.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which the object is drawn.</param>
		void Draw(IDrawDevice device);
	}
}
