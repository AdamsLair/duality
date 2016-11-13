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
		/// [GET] Specifies whether the output from <see cref="QueryVisibleRenderers"/> is sorted by Component type.
		/// If it is, the system may use that information to provide more detailed profiling info that would otherwise
		/// be too time-consuming to collect.
		/// </summary>
		bool IsRendererQuerySorted { get; }

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

		/// <summary>
		/// Registers a new renderer.
		/// </summary>
		/// <param name="renderer"></param>
		void AddRenderer(ICmpRenderer renderer);
		/// <summary>
		/// Removes a previously registered renderer.
		/// </summary>
		/// <param name="renderer"></param>
		void RemoveRenderer(ICmpRenderer renderer);
		/// <summary>
		/// Removes disposed and invalid renderers from the rendering queue.
		/// </summary>
		void CleanupRenderers();
	}
}
