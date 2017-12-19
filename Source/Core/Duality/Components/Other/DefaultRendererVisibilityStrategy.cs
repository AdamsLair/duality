using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Cloning;

namespace Duality.Components
{
	/// <summary>
	/// Represents the default strategy to determine which <see cref="ICmpRenderer">renderers</see> are currently visible
	/// to a certain drawing device.
	/// </summary>
	public class DefaultRendererVisibilityStrategy : IRendererVisibilityStrategy
	{
		[DontSerialize]
		[CloneField(CloneFieldFlags.DontSkip)]
		private int totalRendererCount = 0;
		[DontSerialize]
		[CloneField(CloneFieldFlags.DontSkip)]
		private Dictionary<Type,RawList<ICmpRenderer>> renderersByType = new Dictionary<Type,RawList<ICmpRenderer>>();
		[DontSerialize]
		private RawList<CullingInfo> cullingInfo = new RawList<CullingInfo>();
		[DontSerialize]
		private RawList<ICmpRenderer> cullingRenderers = new RawList<ICmpRenderer>();

		public bool IsRendererQuerySorted
		{
			get { return true; }
		}

		public void QueryVisibleRenderers(DrawDevice device, RawList<ICmpRenderer> visibleRenderers)
		{
			int activeCount = this.cullingInfo.Count;
			CullingInfo[] cullingData = this.cullingInfo.Data;
			ICmpRenderer[] rendererData = this.cullingRenderers.Data;

			visibleRenderers.Clear();
			visibleRenderers.Reserve(activeCount);

			ICmpRenderer[] visibleRendererData = visibleRenderers.Data;
			int visibleCount = 0;

			VisibilityFlag mask = device.VisibilityMask;
			for (int i = 0; i < activeCount; i++)
			{
				// Check group and overlay / world visibility
				if ((cullingData[i].Visibility & VisibilityFlag.AllGroups & mask) == VisibilityFlag.None) continue;
				if ((cullingData[i].Visibility & VisibilityFlag.ScreenOverlay) != (mask & VisibilityFlag.ScreenOverlay)) continue;

				// Check spatial visibility
				if (!device.IsSphereVisible(cullingData[i].Position, cullingData[i].Radius)) continue;

				// Add renderer to visible result list
				visibleRendererData[visibleCount] = rendererData[i];
				visibleCount++;
			}

			visibleRenderers.Count = visibleCount;
		}
		public void Update()
		{
			// Clear previous data and allocate space for the update
			this.cullingInfo.Clear();
			this.cullingInfo.Reserve(this.totalRendererCount);
			this.cullingRenderers.Clear();
			this.cullingRenderers.Reserve(this.totalRendererCount);

			// Retrieve culling data for all currently active renderers, sorted by type
			int activeCount = 0;
			CullingInfo[] cullingData = this.cullingInfo.Data;
			ICmpRenderer[] rendererData = this.cullingRenderers.Data;
			foreach (var pair in this.renderersByType)
			{
				ICmpRenderer[] renderers = pair.Value.Data;
				for (int i = 0; i < renderers.Length; i++)
				{
					if (i >= pair.Value.Count) break;
					if (!(renderers[i] as Component).Active) continue;

					rendererData[activeCount] = renderers[i];
					renderers[i].GetCullingInfo(out cullingData[activeCount]);
					activeCount++;
				}
			}

			// Adjust item count to match the active renderers
			this.cullingInfo.Count = activeCount;
			this.cullingRenderers.Count = activeCount;
		}

		public void AddRenderer(ICmpRenderer renderer)
		{
			Type type = renderer.GetType();
			RawList<ICmpRenderer> list;
			if (!this.renderersByType.TryGetValue(type, out list))
			{
				list = new RawList<ICmpRenderer>();
				this.renderersByType.Add(type, list);
			}
			list.Add(renderer);
			this.totalRendererCount++;
		}
		public void RemoveRenderer(ICmpRenderer renderer)
		{
			Type type = renderer.GetType();
			RawList<ICmpRenderer> list;
			if (!this.renderersByType.TryGetValue(type, out list))
			{
				list = new RawList<ICmpRenderer>();
				this.renderersByType.Add(type, list);
			}
			list.Remove(renderer);
			this.totalRendererCount--;
		}
		public void CleanupRenderers()
		{
			foreach (var pair in this.renderersByType)
			{
				int oldCount = pair.Value.Count;
				pair.Value.RemoveAll(i => i == null || (i as Component).Disposed);
				this.totalRendererCount -= oldCount - pair.Value.Count;
			}
		}
	}
}
