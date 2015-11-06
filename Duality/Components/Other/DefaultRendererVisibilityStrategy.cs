using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

namespace Duality.Components
{
	/// <summary>
	/// Represents the default strategy to determine which <see cref="ICmpRenderer">renderers</see> are currently visible
	/// to a certain drawing device.
	/// </summary>
	public class DefaultRendererVisibilityStrategy : IRendererVisibilityStrategy
	{
		[DontSerialize] private RawList<ICmpRenderer> renderers = new RawList<ICmpRenderer>();

		public void QueryVisibleRenderers(IDrawDevice device, RawList<ICmpRenderer> targetList)
		{
			// Empty the cached list of visible renderers
			targetList.Count = 0;
			targetList.Reserve(this.renderers.Count);

			// Copy references to all renderers that are visible to the target device
			ICmpRenderer[] targetData = targetList.Data;
			ICmpRenderer[] data = this.renderers.Data;
			int visibleCount = 0;
			for (int i = 0; i < data.Length; i++)
			{
				if (i >= this.renderers.Count) break;

				if ((data[i] as Component).Active && data[i].IsVisible(device))
				{
					targetData[visibleCount] = data[i];
					visibleCount++;
				}
			}
			targetList.Count = visibleCount;
		}
		public void Update() { }

		public void AddRenderer(ICmpRenderer renderer)
		{
			this.renderers.Add(renderer);
		}
		public void RemoveRenderer(ICmpRenderer renderer)
		{
			this.renderers.Remove(renderer);
		}
		public void CleanupRenderers()
		{
			this.renderers.RemoveAll(i => i == null || (i as Component).Disposed);

			// Make sure to remove all references outside the valid range
			ICmpRenderer[] data = this.renderers.Data;
			for (int i = this.renderers.Count; i < data.Length; i++)
			{
				data[i] = null;
			}
		}
	}
}
