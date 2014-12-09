using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents a strategy to determine which <see cref="ICmpRenderer">renderers</see> are currently visible
	/// to a certain drawing device.
	/// </summary>
	public interface IRendererVisibilityStrategy
	{
		/// <summary>
		/// Queries all renderers that are currently visible to the specified device.
		/// </summary>
		/// <param name="device"></param>
		/// <returns></returns>
		IEnumerable<ICmpRenderer> QueryVisibleRenderers(IDrawDevice device);
		/// <summary>
		/// Updates the strategy to account for changes in the <see cref="Duality.Resources.Scene"/>.
		/// </summary>
		/// <param name="existingRenderers"></param>
		void Update(IEnumerable<ICmpRenderer> existingRenderers);
	}
}
