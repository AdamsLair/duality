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

		public bool IsRendererQuerySorted
		{
			get { return true; }
		}

		public void QueryVisibleRenderers(IDrawDevice device, RawList<ICmpRenderer> targetList)
		{
			// Empty the cached list of visible renderers
			targetList.Count = 0;
			targetList.Reserve(this.totalRendererCount);

			// Copy references to all renderers that are visible to the target device
			int visibleCount = 0;
			ICmpRenderer[] targetData = targetList.Data;
			foreach (var pair in this.renderersByType)
			{
				ICmpRenderer[] data = pair.Value.Data;
				for (int i = 0; i < data.Length; i++)
				{
					if (i >= pair.Value.Count) break;

					CullingInfo cullingInfo;
					data[i].GetCullingInfo(out cullingInfo);

					bool isVisible = 
						(cullingInfo.Visibility & device.VisibilityMask & VisibilityFlag.AllGroups) != VisibilityFlag.None &&
						(cullingInfo.Visibility & VisibilityFlag.ScreenOverlay) == (device.VisibilityMask & VisibilityFlag.ScreenOverlay) &&
						device.IsCoordInView(cullingInfo.Position, cullingInfo.Radius);

					if ((data[i] as Component).Active && isVisible)
					{
						targetData[visibleCount] = data[i];
						visibleCount++;
					}
				}
			}
			targetList.Count = visibleCount;
		}
		public void Update() { }

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
