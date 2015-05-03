using System;
using System.Collections.Generic;
using System.Linq;

namespace Duality.Drawing
{
	/// <summary>
	/// Represents the default strategy to determine which <see cref="ICmpRenderer">renderers</see> are currently visible
	/// to a certain drawing device.
	/// </summary>
	public class DefaultRendererVisibilityStrategy : IRendererVisibilityStrategy
	{
		[DontSerialize]
		private IEnumerable<ICmpRenderer> renderers;

		public IEnumerable<ICmpRenderer> QueryVisibleRenderers(IDrawDevice device)
		{
			if (this.renderers == null)
				return Enumerable.Empty<ICmpRenderer>();
			else
				return this.renderers.Where(r => (r as Component).Active && r.IsVisible(device));
		}
		public void Update(IEnumerable<ICmpRenderer> existingRenderers)
		{
			this.renderers = existingRenderers;
		}
	}
}
