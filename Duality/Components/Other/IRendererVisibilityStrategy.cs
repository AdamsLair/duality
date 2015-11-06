using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Components
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
		/// <param name="targetList">The list that should be updated by this method.</param>
		/// <returns></returns>
		void QueryVisibleRenderers(IDrawDevice device, RawList<ICmpRenderer> targetList);
		/// <summary>
		/// Updates the strategy to account for changes in the <see cref="Duality.Resources.Scene"/>
		/// after each frame update.
		/// </summary>
		void Update();

		void AddRenderer(ICmpRenderer renderer);
		void RemoveRenderer(ICmpRenderer renderer);
		void CleanupRenderers();
	}
}
