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

		[DontSerialize] private List<ICmpRenderer> pickingMap    = null;
		[DontSerialize] private RenderTarget       pickingRT     = null;
		[DontSerialize] private Texture            pickingTex    = null;
		[DontSerialize] private byte[]             pickingBuffer = null;


		/// <summary>
		/// [GET / SET] Whether the picking lookup map will contain screen overlay renderers
		/// in addition to world-space renderers.
		/// </summary>
		public bool RenderOverlay
		{
			get { return this.renderOverlay; }
			set { this.renderOverlay = value; }
		}
		

		/// <summary>
		/// Performs an object lookup in the picking map that was rendered last 
		/// time <see cref="RenderSetup.RenderPointOfView"/> was called.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ICmpRenderer LookupPickingMap(int x, int y)
		{
			if (this.pickingBuffer == null) return null;

			if (x < 0 || x >= this.pickingTex.ContentWidth) return null;
			if (y < 0 || y >= this.pickingTex.ContentHeight) return null;

			int baseIndex = 4 * (x + y * this.pickingTex.ContentWidth);
			if (baseIndex + 4 >= this.pickingBuffer.Length) return null;

			int rendererId = 
				(this.pickingBuffer[baseIndex + 0] << 16) |
				(this.pickingBuffer[baseIndex + 1] << 8) |
				(this.pickingBuffer[baseIndex + 2] << 0);
			if (rendererId > this.pickingMap.Count)
			{
				Logs.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
				return null;
			}
			else if (rendererId != 0)
			{
				if ((this.pickingMap[rendererId - 1] as Component).Disposed)
					return null;
				else
					return this.pickingMap[rendererId - 1];
			}
			else
				return null;
		}
		/// <summary>
		/// Performs an object lookup in the picking map that was rendered last 
		/// time <see cref="RenderSetup.RenderPointOfView"/> was called.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		public IEnumerable<ICmpRenderer> LookupPickingMap(int x, int y, int w, int h)
		{
			if (this.pickingBuffer == null)
				return Enumerable.Empty<ICmpRenderer>();
			if ((x + w) + (y + h) * this.pickingTex.ContentWidth >= this.pickingBuffer.Length)
				return Enumerable.Empty<ICmpRenderer>();

			Rect dstRect = new Rect(x, y, w, h);
			Rect availRect = new Rect(this.pickingTex.ContentSize);

			if (!dstRect.Intersects(availRect)) return Enumerable.Empty<ICmpRenderer>();
			dstRect = dstRect.Intersection(availRect);

			x = Math.Max((int)dstRect.X, 0);
			y = Math.Max((int)dstRect.Y, 0);
			w = Math.Min((int)dstRect.W, this.pickingTex.ContentWidth - x);
			h = Math.Min((int)dstRect.H, this.pickingTex.ContentHeight - y);

			HashSet<ICmpRenderer> result = new HashSet<ICmpRenderer>();
			int rendererIdLast = 0;
			for (int j = 0; j < h; ++j)
			{
				int offset = 4 * (x + (y + j) * this.pickingTex.ContentWidth);
				for (int i = 0; i < w; ++i)
				{
					int rendererId =
						(this.pickingBuffer[offset]		<< 16) |
						(this.pickingBuffer[offset + 1] << 8) |
						(this.pickingBuffer[offset + 2] << 0);

					if (rendererId != rendererIdLast)
					{
						if (rendererId - 1 > this.pickingMap.Count)
							Logs.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
						else if (rendererId != 0 && !(this.pickingMap[rendererId - 1] as Component).Disposed)
							result.Add(this.pickingMap[rendererId - 1]);
						rendererIdLast = rendererId;
					}
					offset += 4;
				}
			}

			return result;
		}

		protected override void OnRenderPointOfView(Scene scene, DrawDevice drawDevice, Rect viewportRect, Vector2 imageSize)
		{
			// Set up the picking render target to match the proper size
			if (this.pickingTex == null)
			{
				this.pickingTex = new Texture(
					(int)viewportRect.W, 
					(int)viewportRect.H, 
					TextureSizeMode.Default, 
					TextureMagFilter.Nearest, 
					TextureMinFilter.Nearest);
			}
			if (this.pickingRT == null)
			{
				this.pickingRT = new RenderTarget(
					AAQuality.Off,
					true,
					this.pickingTex);
				this.pickingRT.DepthBuffer = true;
			}
			this.ResizeRenderTarget(this.pickingRT, (Point2)viewportRect.Size);

			ContentRef<RenderTarget> oldDeviceTarget = drawDevice.Target;
			ProjectionMode oldDeviceProjection = drawDevice.Projection;
			VisibilityFlag oldDeviceMask = drawDevice.VisibilityMask;

			drawDevice.PickingIndex = 1;
			drawDevice.ClearColor = ColorRgba.Black;
			drawDevice.ClearDepth = 1.0f;
			drawDevice.Target = this.pickingRT;
			drawDevice.TargetSize = imageSize;
			drawDevice.ViewportRect = new Rect(this.pickingRT.Size);
			
			if (this.pickingMap == null) this.pickingMap = new List<ICmpRenderer>();
			this.pickingMap.Clear();

			// Render the world
			{
				drawDevice.VisibilityMask = oldDeviceMask & VisibilityFlag.AllGroups;
				drawDevice.Projection = oldDeviceProjection;
				drawDevice.ClearFlags = ClearFlag.All;

				drawDevice.PrepareForDrawcalls();
				this.CollectRendererDrawcalls(scene, drawDevice);
				drawDevice.Render();
			}

			// Render screen overlays
			if (this.renderOverlay)
			{
				drawDevice.VisibilityMask = oldDeviceMask;
				drawDevice.Projection = ProjectionMode.Screen;
				drawDevice.ClearFlags = ClearFlag.None;

				drawDevice.PrepareForDrawcalls();
				this.CollectRendererDrawcalls(scene, drawDevice);
				drawDevice.Render();
			}

			drawDevice.PickingIndex = 0;
			drawDevice.VisibilityMask = oldDeviceMask;
			drawDevice.Projection = oldDeviceProjection;
			drawDevice.Target = oldDeviceTarget;

			// Move data to local buffer
			int pxNum = this.pickingTex.ContentWidth * this.pickingTex.ContentHeight;
			int pxByteNum = pxNum * 4;

			if (this.pickingBuffer == null)
				this.pickingBuffer = new byte[pxByteNum];
			else if (pxByteNum > this.pickingBuffer.Length)
				Array.Resize(ref this.pickingBuffer, Math.Max(this.pickingBuffer.Length * 2, pxByteNum));

			this.pickingRT.GetPixelData(this.pickingBuffer);
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
