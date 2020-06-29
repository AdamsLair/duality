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
		private float                     nearZ            = 50.0f;
		private float                     farZ             = 10000.0f;
		private float                     focusDist        = DrawDevice.DefaultFocusDist;
		private Rect                      targetRect       = new Rect(1.0f, 1.0f);
		private ProjectionMode            projection       = ProjectionMode.Perspective;
		private VisibilityFlag            visibilityMask   = VisibilityFlag.All;
		private ColorRgba                 clearColor       = ColorRgba.TransparentBlack;
		private ContentRef<RenderTarget>  renderTarget     = null;
		private ContentRef<RenderSetup>   renderSetup      = null;
		private int                       priority         = 0;
		private ShaderParameterCollection shaderParameters = new ShaderParameterCollection();

		[DontSerialize] private DrawDevice         drawDevice      = null;
		[DontSerialize] private DrawDevice         transformDevice = null;
		[DontSerialize] private PickingRenderSetup pickingSetup    = null;
		[DontSerialize] private Vector2[]          cameraBounds    = new Vector2[4];
		

		/// <summary>
		/// [GET / SET] The lowest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(0)]
		[EditorHintIncrement(10.0f)]
		[EditorHintRange(1.0f, 1000000.0f, 10.0f, 200.0f)]
		public float NearZ
		{
			get { return this.nearZ; }
			set { this.nearZ = value; }
		}
		/// <summary>
		/// [GET / SET] The highest Z value that can be displayed by the device.
		/// </summary>
		[EditorHintDecimalPlaces(0)]
		[EditorHintIncrement(1000.0f)]
		[EditorHintRange(100.0f, 1000000.0f, 1000.0f, 100000.0f)]
		public float FarZ
		{
			get { return this.farZ; }
			set { this.farZ = value; }
		}
		/// <summary>
		/// [GET / SET] Reference distance for calculating the view projection. When using <see cref="ProjectionMode.Perspective"/>, 
		/// an object this far away from the Camera will always appear in its original size and without offset.
		/// </summary>
		[EditorHintDecimalPlaces(1)]
		[EditorHintIncrement(10.0f)]
		[EditorHintRange(10.0f, 1000000.0f, 10.0f, 2000.0f)]
		public float FocusDist
		{
			get { return this.focusDist; }
			set { this.focusDist = MathF.Max(value, 0.01f); }
		}
		/// <summary>
		/// [GET] The current point that the camera is focused on, as absolute world position.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector3 FocusPos
		{
			get { return new Vector3(this.GameObj.Transform.Pos.Xy, this.GameObj.Transform.Pos.Z + this.focusDist); }
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
		/// [GET / SET] Specifies the projection that is applied when rendering the world.
		/// </summary>
		public ProjectionMode Projection
		{
			get { return this.projection; }
			set { this.projection = value; }
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
		/// [GET] The <see cref="RenderSetup"/> that will be used when <see cref="RenderPickingMap"/> is called.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public PickingRenderSetup PickingSetup
		{
			get { return this.pickingSetup; }
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
					DualityApp.AppData.Value.RenderingSetup.Res ?? 
					RenderSetup.Default.Res;
			}
		}
		/// <summary>
		/// [GET / SET] Cameras with higher priority values render first.
		/// </summary>
		public int Priority
		{
			get { return this.priority; }
			set { this.priority = value; }
		}
		/// <summary>
		/// [GET] Provides access to the cameras shared <see cref="ShaderParameterCollection"/>,
		/// which allows to specify a parameter value globally across all materials rendered by
		/// this <see cref="Camera"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public ShaderParameterCollection ShaderParameters
		{
			get { return this.shaderParameters; }
		}
		/// <summary>
		/// [GET] Rendered image / screen space offset between the rendered <see cref="TargetRect"/> center
		/// and the screen center. Used for internal screen / world space transformations.
		/// 
		/// For example, if the <see cref="TargetRect"/> is set to render only the left half of the screen,
		/// this property will return the offset between the left halfs center and the actual screen center.
		/// </summary>
		private Vector2 TargetRectDelta
		{
			get { return (new Vector2(0.5f, 0.5f) - this.targetRect.Center) * DualityApp.TargetViewSize; }
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
			
			// Make sure the drawing device has all the latest settings for rendering
			this.UpdateDrawDevice();
			
			// Adjust the local render size and viewport according to the camera target rect
			Vector2 localImageSize = imageSize;
			Rect localViewport = viewportRect;
			localViewport.Pos += localViewport.Size * this.targetRect.Pos;
			localViewport.Size *= this.targetRect.Size;
			localImageSize *= this.targetRect.Size;

			// Render the scene that contains this camera from its current point of view
			// using the previously configured drawing device.
			RenderSetup setup = this.ActiveRenderSetup;
			setup.RenderPointOfView(
				// Parent scene might be null for editor-only cameras
				this.Scene ?? Scene.Current, 
				this.drawDevice, 
				localViewport, 
				localImageSize);

			// Update the transform devices target size with the size we're actually rendering in
			this.transformDevice.TargetSize = imageSize;

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

			// Make sure the drawing device has all the latest settings for rendering
			this.UpdateDrawDevice();

			if (this.pickingSetup == null) this.pickingSetup = new PickingRenderSetup();
			this.pickingSetup.RenderOverlay = renderOverlay;
			this.pickingSetup.RenderPointOfView(
				// Parent scene might be null for editor-only cameras
				this.Scene ?? Scene.Current, 
				this.drawDevice, 
				new Rect(viewportSize), 
				imageSize);

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
			if (this.pickingSetup == null) return null;
			return this.pickingSetup.LookupPickingMap(x, y);
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
			if (this.pickingSetup == null) return Enumerable.Empty<ICmpRenderer>();
			return this.pickingSetup.LookupPickingMap(x, y, w, h);
		}

		/// <summary>
		/// Returns the scale factor of objects that are located at the specified world space Z position.
		/// </summary>
		/// <param name="z"></param>
		public float GetScaleAtZ(float z)
		{
			this.UpdateTransformDevice();
			return this.transformDevice.GetScaleAtZ(z);
		}
		/// <summary>
		/// Transforms screen space to world space positions. The screen positions Z coordinate is
		/// interpreted as the target world Z coordinate.
		/// </summary>
		/// <param name="screenPos"></param>
		public Vector3 GetWorldPos(Vector3 screenPos)
		{
			this.UpdateTransformDevice();
			Vector2 offset = this.TargetRectDelta;
			screenPos.X += offset.X;
			screenPos.Y += offset.Y;
			return this.transformDevice.GetWorldPos(screenPos);
		}
		/// <summary>
		/// Transforms screen space to world space.
		/// </summary>
		/// <param name="screenPos"></param>
		public Vector3 GetWorldPos(Vector2 screenPos)
		{
			this.UpdateTransformDevice();
			screenPos += this.TargetRectDelta;
			return this.transformDevice.GetWorldPos(screenPos);
		}
		/// <summary>
		/// Transforms world space to screen space positions.
		/// </summary>
		public Vector2 GetScreenPos(Vector3 worldPos)
		{
			this.UpdateTransformDevice();
			return this.transformDevice.GetScreenPos(worldPos) - this.TargetRectDelta;
		}
		/// <summary>
		/// Transforms world space to screen space positions.
		/// </summary>
		public Vector2 GetScreenPos(Vector2 worldPos)
		{
			this.UpdateTransformDevice();
			return this.transformDevice.GetScreenPos(worldPos) - this.TargetRectDelta;
		}
		/// <summary>
		/// Returns whether the specified world space sphere is visible in the cameras view.
		/// </summary>
		/// <param name="worldPos">The spheres world space center position.</param>
		/// <param name="radius">The spheres world space radius.</param>
		public bool IsSphereInView(Vector3 worldPos, float radius = 1.0f)
		{
			this.UpdateTransformDevice();
			return this.transformDevice.IsSphereInView(worldPos, radius);
		}

		/// <summary>
		/// Return an axis-aligned Rect that contains the current viewport with the desired size.
		/// </summary>
		/// <param name="z">Z world-coordinate used to determine the extents of the viewport.</param>
		/// <returns>An axis-aligned Rect that fits the viewport. Could be bigger if the Camera is not rotated in 90° increments.</returns>
		public Rect GetWorldViewportBounds(float z)
		{
			return this.GetWorldViewportBounds(z, DualityApp.TargetViewSize * this.targetRect.Size);
		}
		/// <summary>
		/// Return an axis-aligned Rect that contains the current viewport with the desired size.
		/// </summary>
		/// <param name="z">Z world-coordinate used to determine the extents of the viewport.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		/// <returns>An axis-aligned Rect that fits the viewport. Could be bigger if the Camera is not rotated in 90° increments.</returns>
		public Rect GetWorldViewportBounds(float z, Vector2 imageSize)
		{
			this.GetWorldViewportCorners(this.cameraBounds, z, imageSize);

			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;

			for (int i = 0; i < 4; i++)
			{
				Vector2 corner = this.cameraBounds[i];

				minX = MathF.Min(minX, corner.X);
				minY = MathF.Min(minY, corner.Y);
				maxX = MathF.Max(maxX, corner.X);
				maxY = MathF.Max(maxY, corner.Y);
			}

			return Rect.Align(Alignment.TopLeft, minX, minY, maxX - minX, maxY - minY);
		}

		/// <summary>
		/// Fills an array with vectors that correspond to the 4 corners of the current viewport with the desired size.
		/// </summary>
		/// <param name="corners">An array that will contain the corners of the viewport, in counter-clockwise order, starting from the top-left. The array must fit at least 4 elements.</param>
		/// <param name="z">Z world-coordinate used to determine the extents of the viewport.</param>
		public void GetWorldViewportCorners(Vector2[] corners, float z)
		{
			this.GetWorldViewportCorners(corners, z, DualityApp.TargetViewSize * this.targetRect.Size);
		}
		/// <summary>
		/// Fills an array with vectors that correspond to the 4 corners of the current viewport with the desired size.
		/// </summary>
		/// <param name="corners">An array that will contain the corners of the viewport, in counter-clockwise order, starting from the top-left. The array must fit at least 4 elements.</param>
		/// <param name="z">Z world-coordinate used to determine the extents of the viewport.</param>
		/// <param name="imageSize">Target size of the rendered image before adjusting it to fit the specified viewport.</param>
		public void GetWorldViewportCorners(Vector2[] corners, float z, Vector2 imageSize)
		{
			if (corners == null || corners.Length < 4)
				throw new ArgumentException("Array must contain at least 4 elements", "corners");

			Vector2 center = DualityApp.TargetViewSize * this.targetRect.Center;
			Vector2 halfSize = imageSize / 2;

			corners[0] = center - halfSize;
			corners[2] = center + halfSize;
			corners[1] = new Vector2(corners[0].X, corners[2].Y);
			corners[3] = new Vector2(corners[2].X, corners[0].Y);

			// counter clockwise query of points
			corners[0] = this.GetWorldPos(new Vector3(corners[0], z)).Xy;
			corners[1] = this.GetWorldPos(new Vector3(corners[1], z)).Xy;
			corners[2] = this.GetWorldPos(new Vector3(corners[2], z)).Xy;
			corners[3] = this.GetWorldPos(new Vector3(corners[3], z)).Xy;
		}

		private void SetupDrawDevice()
		{
			if (this.drawDevice != null && !this.drawDevice.Disposed) return;

			// The draw device can just use default settings, because all rendering
			// will overwrite the relevant values, such as render mode and target size.
			// It will never be used by the Cameras transform methods.
			this.drawDevice = new DrawDevice();
		}
		private void ReleaseDrawDevice()
		{
			if (this.drawDevice == null) return;
			this.drawDevice.Dispose();
			this.drawDevice = null;
		}
		private void UpdateDrawDevice()
		{
			// On-demand setup, in case someone uses this Camera despite being inactive. (Editor)
			if (this.drawDevice == null) this.SetupDrawDevice();

			this.drawDevice.ViewerPos = this.gameobj.Transform.Pos;
			this.drawDevice.ViewerAngle = this.gameobj.Transform.Angle;
			this.drawDevice.NearZ = this.nearZ;
			this.drawDevice.FarZ = this.farZ;
			this.drawDevice.FocusDist = this.focusDist;
			this.drawDevice.Projection = this.projection;
			this.drawDevice.VisibilityMask = this.visibilityMask;
			this.drawDevice.ClearColor = this.clearColor;
			this.drawDevice.Target = this.renderTarget;

			this.shaderParameters.CopyTo(this.drawDevice.ShaderParameters);
		}

		private void SetupTransformDevice()
		{
			if (this.transformDevice != null && !this.transformDevice.Disposed) return;
			
			// The transform device used only for calculating transform results in
			// the camera methods. It is never used for rendering.
			this.transformDevice = new DrawDevice();
			this.transformDevice.TargetSize = DualityApp.TargetViewSize;
		}
		private void ReleaseTransformDevice()
		{
			if (this.transformDevice == null) return;
			this.transformDevice.Dispose();
			this.transformDevice = null;
		}
		private void UpdateTransformDevice()
		{
			// On-demand setup, in case someone uses this Camera despite being inactive. (Editor)
			if (this.transformDevice == null) this.SetupTransformDevice();

			this.transformDevice.ViewerPos = this.gameobj.Transform.Pos;
			this.transformDevice.ViewerAngle = this.gameobj.Transform.Angle;
			this.transformDevice.NearZ = this.nearZ;
			this.transformDevice.FarZ = this.farZ;
			this.transformDevice.FocusDist = this.focusDist;
			this.transformDevice.Projection = this.projection;
		}

		void ICmpInitializable.OnActivate()
		{
			this.SetupTransformDevice();
			this.SetupDrawDevice();
		}
		void ICmpInitializable.OnDeactivate()
		{
			this.ReleaseTransformDevice();
			this.ReleaseDrawDevice();
		}
	}
}
