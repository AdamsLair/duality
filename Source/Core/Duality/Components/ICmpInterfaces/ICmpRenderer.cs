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
		void GetCullingInfo(out CullingInfo info);
		/// <summary>
		/// Draws the object.
		/// </summary>
		/// <param name="device">The <see cref="IDrawDevice"/> to which the object is drawn.</param>
		void Draw(IDrawDevice device);
	}
}
