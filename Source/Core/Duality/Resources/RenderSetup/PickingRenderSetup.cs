using System;
using System.Linq;
using System.Collections.Generic;

using Duality.Editor;
using Duality.Properties;
using Duality.Backend;
using Duality.Drawing;
using Duality.Components;


namespace Duality.Resources
{
	/// <summary>
	/// A specialized <see cref="RenderSetup"/> that will render a lookup texture of the scene in
	/// order to determine which <see cref="ICmpRenderer"/> is located at a certain screen position.
	/// </summary>
	[EditorHintFlags(MemberFlags.Invisible)]
	public class PickingRenderSetup : RenderSetup
	{
		private bool renderOverlay = false;

		/// <summary>
		/// [GET / SET] Whether the picking lookup map will contain screen overlay renderers
		/// in addition to world-space renderers.
		/// </summary>
		public bool RenderOverlay
		{
			get { return this.renderOverlay; }
			set { this.renderOverlay = value; }
		}

		protected override void OnRenderPointOfView(Scene scene, DrawDevice drawDevice, Rect viewportRect, Vector2 imageSize, Rect outputTargetRect)
		{
			VisibilityFlag oldDeviceMask = drawDevice.VisibilityMask;

			drawDevice.PickingIndex = 1;
			drawDevice.ClearColor = ColorRgba.Black;
			drawDevice.ClearDepth = 1.0f;

			// Set up the picking render target to match the proper size
			if (drawDevice.Target != null)
				this.ResizeRenderTarget(drawDevice.Target, (Point2)viewportRect.Size);
			
			if (this.pickingMap == null) this.pickingMap = new List<ICmpRenderer>();
			this.pickingMap.Clear();

			// Render the world
			{
				drawDevice.VisibilityMask = oldDeviceMask & VisibilityFlag.AllGroups;
				drawDevice.RenderMode = RenderMatrix.WorldSpace;
				drawDevice.ClearFlags = ClearFlag.All;

				drawDevice.PrepareForDrawcalls();
				this.CollectRendererDrawcalls(scene, drawDevice);
				drawDevice.Render();
			}

			// Render screen overlays
			if (this.renderOverlay)
			{
				drawDevice.VisibilityMask = oldDeviceMask;
				drawDevice.RenderMode = RenderMatrix.ScreenSpace;
				drawDevice.ClearFlags = ClearFlag.None;

				drawDevice.PrepareForDrawcalls();
				this.CollectRendererDrawcalls(scene, drawDevice);
				drawDevice.Render();
			}

			drawDevice.PickingIndex = 0;
			drawDevice.VisibilityMask = oldDeviceMask;
		}
		protected override void OnCollectRendererDrawcalls(DrawDevice drawDevice, RawList<ICmpRenderer> visibleRenderers, bool renderersSortedByType)
		{
			this.pickingMap.AddRange(visibleRenderers);
			foreach (ICmpRenderer r in visibleRenderers)
			{
				r.Draw(drawDevice);
				drawDevice.PickingIndex++;
			}
		}
	}
}
