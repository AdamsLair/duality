using System;
using System.Collections.Generic;
using System.Linq;

using Duality.IO;
using Duality.Editor;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;

namespace Duality.Components
{
	/// <summary>
	/// A Camera is responsible for rendering the current <see cref="Duality.Resources.Scene"/>.
	/// </summary>
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageCamera)]
	public sealed class Camera : Component, ICmpInitializable
	{
		private float                    nearZ                 = 0.0f;
		private float                    farZ                  = 10000.0f;
		private float                    focusDist             = DrawDevice.DefaultFocusDist;
		private Rect                     targetRect            = new Rect(1.0f, 1.0f);
		private PerspectiveMode          perspective           = PerspectiveMode.Parallax;
		private VisibilityFlag           visibilityMask        = VisibilityFlag.All;
		private ColorRgba                clearColor            = ColorRgba.TransparentBlack;
		private ContentRef<RenderTarget> renderTarget          = null;
		private ContentRef<RenderSetup>  renderSetup           = null;

		[DontSerialize] private DrawDevice                    drawDevice         = null;
		[DontSerialize] private List<ICmpRenderer>            pickingMap         = null;
		[DontSerialize] private RenderTarget                  pickingRT          = null;
		[DontSerialize] private Texture                       pickingTex         = null;
		[DontSerialize] private byte[]                        pickingBuffer      = null;
		

		/// <summary>
		/// [GET / SET] The lowest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		public float NearZ
		{
			get { return this.nearZ; }
			set { this.nearZ = value; }
		}
		/// <summary>
		/// [GET / SET] The highest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		public float FarZ
		{
			get { return this.farZ; }
			set { this.farZ = value; }
		}
		/// <summary>
		/// [GET / SET] Reference distance for calculating the view projection. When using <see cref="PerspectiveMode.Parallax"/>, 
		/// an object this far away from the Camera will always appear in its original size and without offset.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		[EditorHintRange(0.0f, float.MaxValue)]
		public float FocusDist
		{
			get { return this.focusDist; }
			set { this.focusDist = MathF.Max(value, 0.01f); }
		}
		/// <summary>
		/// [GET / SET] The rectangular area this camera will render into, relative to the
		/// total available viewport during rendering.
		/// </summary>
		[EditorHintDecimalPlaces(2)]
		[EditorHintIncrement(0.1f)]
		[EditorHintRange(0.0f, 1.0f)]
		public Rect TargetRect
		{
			get { return this.targetRect; }
			set
			{
				Rect intersection = value.Intersection(new Rect(1.0f, 1.0f));
				if (intersection == Rect.Empty) return;
				this.targetRect = intersection;
			}
		}
		/// <summary>
		/// [GET / SET] Specifies the perspective effect that is applied when rendering the world.
		/// </summary>
		public PerspectiveMode Perspective
		{
			get { return this.perspective; }
			set { this.perspective = value; }
		}
		/// <summary>
		/// [GET / SET] A bitmask flagging all visibility groups that are considered visible to this drawing device.
		/// </summary>
		public VisibilityFlag VisibilityMask
		{
			get { return this.visibilityMask; }
			set { this.visibilityMask = value; }
		}
		/// <summary>
		/// [GET / SET] The default background color of the rendered image.
		/// </summary>
		public ColorRgba ClearColor
		{
			get { return this.clearColor; }
			set { this.clearColor = value; }
		}
		/// <summary>
		/// [GET / SET] When set, the camera will render all output that would normally end up
		/// on screen to the specified <see cref="RenderTarget"/> instead.
		/// </summary>
		public ContentRef<RenderTarget> Target
		{
			get { return this.renderTarget; }
			set { this.renderTarget = value; }
		}
		/// <summary>
		/// [GET / SET] The <see cref="RenderSetup"/> that should be used by this camera. Will
		/// fall back to the application-default <see cref="DualityAppData.RenderingSetup"/> when unavailable.
		/// </summary>
		public ContentRef<RenderSetup> RenderingSetup
		{
			get { return this.renderSetup; }
			set { this.renderSetup = value; }
		}
		/// <summary>
		/// [GET] The rendering setup that will be used by this camera.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public RenderSetup ActiveRenderSetup
		{
			get
			{
				return 
					this.renderSetup.Res ?? 
					DualityApp.AppData.RenderingSetup.Res ?? 
					RenderSetup.Default.Res;
			}
		}


		/// <summary>
		/// Renders the current <see cref="Duality.Resources.Scene"/>.
		/// </summary>
		/// <param name="viewportRect">The viewport area to which will be rendered.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		public void Render(Rect viewportRect, Vector2 imageSize)
		{
			string counterName = PathOp.Combine("Cameras", this.gameobj.Name);
			Profile.BeginMeasure(counterName);
			Profile.TimeRender.BeginMeasure();
			
			// Configure the wrapped drawing device, so rendering matrices and settings
			// are set up properly.
			this.UpdateDeviceConfig();

			// Render the scene that contains this camera from its current point of view
			// using the previously configured drawing device.
			RenderSetup setup = this.ActiveRenderSetup;
			setup.RenderPointOfView(
				this.GameObj.ParentScene, 
				this.drawDevice, 
				viewportRect, 
				imageSize, 
				this.targetRect);

			Profile.TimeRender.EndMeasure();
			Profile.EndMeasure(counterName);
		}
		/// <summary>
		/// Renders a picking map of the current <see cref="Duality.Resources.Scene"/>.
		/// This method needs to be called each frame a picking operation is to be performed.
		/// </summary>
		/// <param name="viewportSize">Size of the viewport area to which will be rendered.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		/// <param name="renderOverlay">Whether or not to render screen overlay renderers onto the picking target.</param>
		public void RenderPickingMap(Point2 viewportSize, Vector2 imageSize, bool renderOverlay)
		{
			Profile.TimeVisualPicking.BeginMeasure();

			this.UpdateDeviceConfig();
			this.drawDevice.Target = this.pickingRT;

			if (this.pickingMap == null) this.pickingMap = new List<ICmpRenderer>();
			this.pickingMap.Clear();

			PickingRenderSetup pickingSetup = RenderSetup.Picking.Res;
			pickingSetup.RenderPointOfView(
				this.GameObj.ParentScene, 
				this.drawDevice, 
				new Rect(viewportSize), 
				imageSize, 
				new Rect(0, 0, 1, 1));

			// Move data to local buffer
			int pxNum = this.pickingTex.ContentWidth * this.pickingTex.ContentHeight;
			int pxByteNum = pxNum * 4;

			if (this.pickingBuffer == null)
				this.pickingBuffer = new byte[pxByteNum];
			else if (pxByteNum > this.pickingBuffer.Length)
				Array.Resize(ref this.pickingBuffer, Math.Max(this.pickingBuffer.Length * 2, pxByteNum));

			this.pickingRT.GetPixelData(this.pickingBuffer);

			Profile.TimeVisualPicking.EndMeasure();
		}
		/// <summary>
		/// Picks the <see cref="Duality.ICmpRenderer"/> that owns the pixel at the specified position.
		/// The resulting information is only accurate if <see cref="RenderPickingMap"/> has been called this frame.
		/// </summary>
		/// <param name="x">x-Coordinate of the pixel to check.</param>
		/// <param name="y">y-Coordinate of the pixel to check.</param>
		/// <returns>The <see cref="Duality.ICmpRenderer"/> that owns the pixel.</returns>
		public ICmpRenderer PickRendererAt(int x, int y)
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
		/// Picks all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> contained within the specified
		/// rectangular area.
		/// The resulting information is only accurate if <see cref="RenderPickingMap"/> has been called this frame.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect.</param>
		/// <param name="y">y-Coordinate of the Rect.</param>
		/// <param name="w">Width of the Rect.</param>
		/// <param name="h">Height of the Rect.</param>
		/// <returns>A set of all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> that have been picked.</returns>
		public IEnumerable<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
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

		/// <summary>
		/// Returns the scale factor of objects that are located at the specified (world space) z-Coordinate.
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		public float GetScaleAtZ(float z)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScaleAtZ(z);
		}
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector3 screenPos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetSpaceCoord(screenPos);
		}
		/// <summary>
		/// Transforms screen space coordinates to world space coordinates.
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector3 GetSpaceCoord(Vector2 screenPos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetSpaceCoord(screenPos);
		}
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector3 spacePos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScreenCoord(spacePos);
		}
		/// <summary>
		/// Transforms world space coordinates to screen space coordinates.
		/// </summary>
		/// <param name="spacePos"></param>
		/// <returns></returns>
		public Vector3 GetScreenCoord(Vector2 spacePos)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.GetScreenCoord(spacePos);
		}
		/// <summary>
		/// Returns whether the specified world-space position is visible in the Cameras view space.
		/// </summary>
		/// <param name="c">The position to test.</param>
		/// <param name="boundRad">The visual bounding radius to assume for the specified position.</param>
		/// <returns>True, if the position or a portion of its bounding circle is visible, false if not.</returns>
		public bool IsCoordInView(Vector3 c, float boundRad = 1.0f)
		{
			this.UpdateDeviceConfig();
			return this.drawDevice.IsCoordInView(c, boundRad);
		}

		private void SetupDevice()
		{
			if (this.drawDevice != null && !this.drawDevice.Disposed) return;
			this.drawDevice = new DrawDevice();
		}
		private void ReleaseDevice()
		{
			if (this.drawDevice == null) return;
			this.drawDevice.Dispose();
			this.drawDevice = null;
		}
		private void UpdateDeviceConfig()
		{
			// Lazy setup, in case someone uses this Camera despite being inactive. (Editor)
			if (this.drawDevice == null) this.SetupDevice();

			this.drawDevice.RefCoord = this.gameobj.Transform.Pos;
			this.drawDevice.RefAngle = this.gameobj.Transform.Angle;
			this.drawDevice.NearZ = this.nearZ;
			this.drawDevice.FarZ = this.farZ;
			this.drawDevice.FocusDist = this.focusDist;
			this.drawDevice.Perspective = this.perspective;
			this.drawDevice.VisibilityMask = this.visibilityMask;
			this.drawDevice.ClearColor = this.clearColor;
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				this.SetupDevice();
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				this.ReleaseDevice();
			}
		}
	}
}
