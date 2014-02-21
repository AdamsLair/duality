using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using OpenTK.Graphics.OpenGL;
using OpenTK;

using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Properties;

namespace Duality.Components
{
	/// <summary>
	/// A Camera is responsible for rendering the current <see cref="Duality.Resources.Scene"/>.
	/// </summary>
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageCamera)]
	public sealed class Camera : Component, ICmpInitializable
	{
		/// <summary>
		/// Describes a single pass in the overall rendering process.
		/// </summary>
		[Serializable]
		public class Pass
		{
			private ColorRgba					clearColor		= ColorRgba.TransparentBlack;
			private float						clearDepth		= 1.0f;
			private ClearFlag					clearFlags		= ClearFlag.All;
			private RenderMatrix				matrixMode		= RenderMatrix.PerspectiveWorld;
			private	VisibilityFlag				visibilityMask	= VisibilityFlag.AllGroups;
			private	BatchInfo					input			= null;
			private	ContentRef<RenderTarget>	output			= ContentRef<RenderTarget>.Null;

			/// <summary>
			/// Fired when collecting drawcalls for this pass. Note that not all passes do collect drawcalls (see <see cref="Input"/>)
			/// </summary>
			public event EventHandler<CollectDrawcallEventArgs> CollectDrawcalls	= null;
			
			/// <summary>
			/// The input to use for rendering. This can for example be a <see cref="Duality.Resources.Texture"/> that
			/// has been rendered to before and is now bound to perform a postprocessing step. If this is null, the current
			/// <see cref="Duality.Resources.Scene"/> is used as input - which is usually the case in the first rendering pass.
			/// </summary>
			public BatchInfo Input
			{
				get { return this.input; }
				set { this.input = value; }
			}
			/// <summary>
			/// The output to render to in this pass. If this is null, the screen is used as rendering target.
			/// </summary>
			public ContentRef<RenderTarget> Output
			{
				get { return this.output; }
				set { this.output = value; }
			}
			/// <summary>
			/// [GET / SET] The clear color to apply when clearing the color buffer
			/// </summary>
			public ColorRgba ClearColor
			{
				get { return this.clearColor; }
				set { this.clearColor = value; }
			}
			/// <summary>
			/// [GET / SET] The clear depth to apply when clearing the depth buffer
			/// </summary>
			public float ClearDepth
			{
				get { return this.clearDepth; }
				set { this.clearDepth = value; }
			}
			/// <summary>
			/// [GET / SET] Specifies which buffers to clean before rendering this pass
			/// </summary>
			public ClearFlag ClearFlags
			{
				get { return this.clearFlags; }
				set { this.clearFlags = value; }
			}
			/// <summary>
			/// [GET / SET] How to set up the coordinate space before rendering
			/// </summary>
			public RenderMatrix MatrixMode
			{
				get { return this.matrixMode; }
				set { this.matrixMode = value; }
			}
			/// <summary>
			/// [GET / SET] A Pass-local bitmask flagging all visibility groups that are considered visible to this drawing device.
			/// </summary>
			public VisibilityFlag VisibilityMask
			{
				get { return this.visibilityMask; }
				set { this.visibilityMask = value; }
			}

			public Pass() {}
			public Pass(Pass copyFrom)
			{
				this.input = copyFrom.input;
				this.output = copyFrom.output;
				this.clearColor = copyFrom.clearColor;
				this.clearDepth = copyFrom.clearDepth;
				this.clearFlags = copyFrom.clearFlags;
				this.matrixMode = copyFrom.matrixMode;
				this.visibilityMask = copyFrom.visibilityMask;

				this.MakeAvailable();
			}
			public Pass(Pass copyFrom, BatchInfo inputOverride)
			{
				this.input = inputOverride;
				this.output = copyFrom.output;
				this.clearColor = copyFrom.clearColor;
				this.clearDepth = copyFrom.clearDepth;
				this.clearFlags = copyFrom.clearFlags;
				this.matrixMode = copyFrom.matrixMode;
				this.visibilityMask = copyFrom.visibilityMask;

				this.MakeAvailable();
			}
			public void MakeAvailable()
			{
				this.output.MakeAvailable();
			}

			internal void NotifyCollectDrawcalls(IDrawDevice device)
			{
				Profile.TimeCollectDrawcalls.BeginMeasure();

				if (this.CollectDrawcalls != null)
					this.CollectDrawcalls(this, new CollectDrawcallEventArgs(device));

				Profile.TimeCollectDrawcalls.EndMeasure();
			}

			public override string ToString()
			{
				ContentRef<Texture> inputTex = input == null ? ContentRef<Texture>.Null : input.MainTexture;
				return string.Format("{0} => {1}{2}",
					inputTex.IsExplicitNull ? (input == null ? "Camera" : "Undefined") : inputTex.Name,
					output.IsExplicitNull ? "Screen" : output.Name,
					(this.visibilityMask & VisibilityFlag.ScreenOverlay) != VisibilityFlag.None ? " (Overlay)" : "");
			}
		}


		private	float	nearZ					= 0.0f;
		private	float	farZ					= 10000.0f;
		private	float	focusDist				= DrawDevice.DefaultFocusDist;
		private	PerspectiveMode	perspective		= PerspectiveMode.Parallax;
		private	VisibilityFlag	visibilityMask	= VisibilityFlag.All;
		private	List<Pass>	passes				= new List<Pass>();

		[NonSerialized]	private	DrawDevice			drawDevice		= null;
		[NonSerialized]	private	List<ICmpRenderer>	pickingMap		= null;
		[NonSerialized]	private	RenderTarget		pickingRT		= null;
		[NonSerialized]	private	Texture				pickingTex		= null;
		[NonSerialized]	private	int					pickingLast		= -1;
		[NonSerialized]	private	byte[]				pickingBuffer	= new byte[4 * 256 * 256];
		[NonSerialized]	private	List<Predicate<ICmpRenderer>>	editorRenderFilter	= new List<Predicate<ICmpRenderer>>();

		
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
		[EditorHintRange(0.0f,float.MaxValue)]
		public float FocusDist
		{
			get { return this.focusDist; }
			set { this.focusDist = MathF.Max(value, 0.01f); }
		}
		/// <summary>
		/// [GET / SET] Specified the perspective effect that is applied when rendering the world.
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
		/// [GET / SET] The background color of the rendered image.
		/// </summary>
		public ColorRgba ClearColor
		{
			get
			{
				Pass clearPass = this.passes.FirstOrDefault(p => (p.ClearFlags & ClearFlag.Color) != ClearFlag.None);
				if (clearPass == null) return ColorRgba.TransparentBlack;
				return clearPass.ClearColor;
			}
			set
			{
				Pass clearPass = this.passes.FirstOrDefault(p => (p.ClearFlags & ClearFlag.Color) != ClearFlag.None);
				if (clearPass != null) clearPass.ClearColor = value;
			}
		}
		/// <summary>
		/// [GET / SET] A set of passes that describes the Cameras rendering process. Is never null nor empty.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback)]
		public List<Pass> Passes
		{
			get { return this.passes; }
			set 
			{ 
				if (value != null)
					this.passes = value.Select(v => v ?? new Pass()).ToList();
				else
					this.passes = new List<Pass>();
			}
		}


		public Camera()
		{
			// Set up default rendering
			Pass worldPass = new Pass();
			Pass overlayPass = new Pass();
			overlayPass.MatrixMode = RenderMatrix.OrthoScreen;
			overlayPass.ClearFlags = ClearFlag.None;
			overlayPass.VisibilityMask = VisibilityFlag.AllGroups | VisibilityFlag.ScreenOverlay;

			this.passes.Add(worldPass);
			this.passes.Add(overlayPass);
		}
		protected override void OnCopyTo(Component target, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(target, provider);
			Camera t = target as Camera;
			t.nearZ				= this.nearZ;
			t.farZ				= this.farZ;
			t.focusDist	= this.focusDist;
			t.visibilityMask	= this.visibilityMask;
			t.passes			= this.passes != null ? new List<Pass>(this.passes.Select(p => new Pass(p))) : null;
		}
		public void MakeAvailable()
		{
			foreach (var pass in this.passes)
				pass.MakeAvailable();
		}

		/// <summary>
		/// Renders the current <see cref="Duality.Resources.Scene"/>.
		/// </summary>
		/// <param name="viewportRect">The viewport area to which will be rendered.</param>
		public void Render(Rect viewportRect)
		{
			this.MakeAvailable();
			this.UpdateDeviceConfig();

			string counterName = Path.Combine("Cameras", this.gameobj.Name);
			Profile.BeginMeasure(counterName);
			Profile.TimeRender.BeginMeasure();

			foreach (Pass t in this.passes)
			{
				this.RenderSinglePass(viewportRect, t);
			}
			RenderTarget.Bind(RenderTarget.None);
			this.drawDevice.VisibilityMask = this.visibilityMask;
			this.drawDevice.RenderMode = RenderMatrix.PerspectiveWorld;
			this.drawDevice.UpdateMatrices(); // Reset matrices for projection calculations during update

			Profile.TimeRender.EndMeasure();
			Profile.EndMeasure(counterName);
		}
		/// <summary>
		/// Renders a picking map of the current <see cref="Duality.Resources.Scene"/>.
		/// If picking is required, this will be (automatically) done each frame a picking operation needs to
		/// be performed. 
		/// </summary>
		/// <param name="viewportSize">Sife of the viewport area to which will be rendered.</param>
		/// <returns>True, if the picking map has been rendered. False, if this frames cached version is used.</returns>
		public bool RenderPickingMap(Vector2 viewportSize)
		{
			if (this.pickingLast == Time.FrameCount) return false;
			this.pickingLast = Time.FrameCount;
			Profile.TimeVisualPicking.BeginMeasure();

			// Render picking map
			{
				this.MakeAvailable();
				this.UpdateDeviceConfig();
				this.SetupPickingRT(viewportSize);

				// Setup DrawDevice
				this.drawDevice.PickingIndex = 1;
				this.drawDevice.Target = this.pickingRT;
				this.drawDevice.VisibilityMask = this.visibilityMask & VisibilityFlag.AllGroups;
				this.drawDevice.RenderMode = RenderMatrix.PerspectiveWorld;
				this.drawDevice.ViewportRect = new Rect(this.pickingTex.PixelWidth, this.pickingTex.PixelHeight);

				// Render Scene
				this.drawDevice.BeginRendering(ClearFlag.All, ColorRgba.Black, 1.0f);
				this.CollectDrawcalls();
				this.drawDevice.EndRendering();
				this.drawDevice.PickingIndex = 0;

				GL.Finish();
				RenderTarget.Bind(RenderTarget.None);
			}

			// Move data to local buffer
			int pxNum = this.pickingTex.PixelWidth * this.pickingTex.PixelHeight;
			int pxByteNum = pxNum * 4;
			if (pxByteNum > this.pickingBuffer.Length) Array.Resize(ref this.pickingBuffer, Math.Max(this.pickingBuffer.Length * 2, pxByteNum));

			ContentRef<RenderTarget> lastTex = RenderTarget.BoundRT;
			RenderTarget.Bind(this.pickingRT);
			GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
			GL.ReadPixels(0, 0, this.pickingTex.PixelWidth, this.pickingTex.PixelHeight, PixelFormat.Rgba, PixelType.UnsignedByte, this.pickingBuffer);
			RenderTarget.Bind(lastTex);
			GL.ReadBuffer(ReadBufferMode.Back);

			Profile.TimeVisualPicking.EndMeasure();
			return true;
		}
		/// <summary>
		/// Picks the <see cref="Duality.ICmpRenderer"/> that owns the pixel at the specified position.
		/// </summary>
		/// <param name="x">x-Coordinate of the pixel to check.</param>
		/// <param name="y">y-Coordinate of the pixel to check.</param>
		/// <param name="viewportRect">The viewport area to which will be rendered.</param>
		/// <returns>The <see cref="Duality.ICmpRenderer"/> that owns the pixel.</returns>
		public ICmpRenderer PickRendererAt(Rect viewportRect, int x, int y)
		{
			if (x < viewportRect.MinimumX || x >= viewportRect.MaximumX) return null;
			if (y < viewportRect.MinimumY || y >= viewportRect.MaximumY) return null;
			
			this.RenderPickingMap(viewportRect.Size);

			x = MathF.Clamp(x - (int)viewportRect.X, 0, this.pickingTex.PixelWidth - 1);
			y = MathF.Clamp(y - (int)viewportRect.Y, 0, this.pickingTex.PixelHeight - 1);

			int rendererId = 
				(this.pickingBuffer[4 * (x + y * this.pickingTex.PixelWidth) + 0] << 16) |
				(this.pickingBuffer[4 * (x + y * this.pickingTex.PixelWidth) + 1] << 8) |
				(this.pickingBuffer[4 * (x + y * this.pickingTex.PixelWidth) + 2] << 0);
			if (rendererId > this.pickingMap.Count)
			{
				Log.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
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
		/// Picks the <see cref="Duality.ICmpRenderer"/> that owns the pixel at the specified position.
		/// </summary>
		/// <param name="x">x-Coordinate of the pixel to check.</param>
		/// <param name="y">y-Coordinate of the pixel to check.</param>
		/// <returns>The <see cref="Duality.ICmpRenderer"/> that owns the pixel.</returns>
		public ICmpRenderer PickRendererAt(int x, int y)
		{
			return this.PickRendererAt(new Rect(DualityApp.TargetResolution), x, y);
		}
		/// <summary>
		/// Picks all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> contained within the specified
		/// rectangular area.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect.</param>
		/// <param name="y">y-Coordinate of the Rect.</param>
		/// <param name="w">Width of the Rect.</param>
		/// <param name="h">Height of the Rect.</param>
		/// <param name="viewportRect">The viewport area to which will be rendered.</param>
		/// <returns>A set of all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> that have been picked.</returns>
		public HashSet<ICmpRenderer> PickRenderersIn(Rect viewportRect, int x, int y, int w, int h)
		{
			Rect dstRect = new Rect(x, y, w, h);
			if (!dstRect.Intersects(viewportRect)) return new HashSet<ICmpRenderer>();
			dstRect = dstRect.Intersection(viewportRect);
			dstRect.X -= viewportRect.X;
			dstRect.Y -= viewportRect.Y;

			this.RenderPickingMap(viewportRect.Size);

			x = Math.Max((int)dstRect.X, 0);
			y = Math.Max((int)dstRect.Y, 0);
			w = Math.Min((int)dstRect.W, this.pickingTex.PixelWidth - x);
			h = Math.Min((int)dstRect.H, this.pickingTex.PixelHeight - y);

			HashSet<ICmpRenderer> result = new HashSet<ICmpRenderer>();
			int rendererIdLast = 0;
			for (int j = 0; j < h; ++j)
			{
				int offset = 4 * (x + (y + j) * this.pickingTex.PixelWidth);
				for (int i = 0; i < w; ++i)
				{
					int rendererId =	(this.pickingBuffer[offset]		<< 16) |
										(this.pickingBuffer[offset + 1] << 8) |
										(this.pickingBuffer[offset + 2] << 0);
					if (rendererId != rendererIdLast)
					{
						if (rendererId - 1 > this.pickingMap.Count)
							Log.Core.WriteWarning("Unexpected picking result: {0}", ColorRgba.FromIntArgb(rendererId));
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
		/// Picks all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> contained within the specified
		/// rectangular area.
		/// </summary>
		/// <param name="x">x-Coordinate of the Rect.</param>
		/// <param name="y">y-Coordinate of the Rect.</param>
		/// <param name="w">Width of the Rect.</param>
		/// <param name="h">Height of the Rect.</param>
		/// <returns>A set of all <see cref="Duality.ICmpRenderer">ICmpRenderers</see> that have been picked.</returns>
		public HashSet<ICmpRenderer> PickRenderersIn(int x, int y, int w, int h)
		{
			return this.PickRenderersIn(new Rect(DualityApp.TargetResolution), x, y, w, h);
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
		}
		private void RenderSinglePass(Rect viewportRect, Pass p)
		{
			this.drawDevice.VisibilityMask = this.visibilityMask & p.VisibilityMask;
			this.drawDevice.RenderMode = p.MatrixMode;
			this.drawDevice.Target = p.Output;
			this.drawDevice.ViewportRect = p.Output.IsAvailable ? new Rect(p.Output.Res.Width, p.Output.Res.Height) : viewportRect;

			if (p.Input == null)
			{
				// Render Scene
				this.drawDevice.BeginRendering(p.ClearFlags, p.ClearColor, p.ClearDepth);
				try
				{
					this.CollectDrawcalls();
					p.NotifyCollectDrawcalls(this.drawDevice);
				}
				catch (Exception e)
				{
					Log.Core.WriteError("There was an error while {0} was collecting drawcalls: {1}", this.ToString(), Log.Exception(e));
				}
				this.drawDevice.EndRendering();
			}
			else
			{
				Profile.TimePostProcessing.BeginMeasure();
				this.drawDevice.BeginRendering(p.ClearFlags, p.ClearColor, p.ClearDepth);

				Texture mainTex = p.Input.MainTexture.Res;
				Vector2 uvRatio = mainTex != null ? mainTex.UVRatio : Vector2.One;
				Vector2 inputSize = mainTex != null ? new Vector2(mainTex.PixelWidth, mainTex.PixelHeight) : Vector2.One;
				Rect targetRect;
				if (DualityApp.ExecEnvironment == DualityApp.ExecutionEnvironment.Editor &&
					!this.drawDevice.Target.IsAvailable)
					targetRect = Rect.AlignCenter(this.drawDevice.TargetSize.X * 0.5f, this.drawDevice.TargetSize.Y * 0.5f, inputSize.X, inputSize.Y);
				else
					targetRect = new Rect(this.drawDevice.TargetSize);

				IDrawDevice device = this.drawDevice;
				device.AddVertices(p.Input, VertexMode.Quads,
					new VertexC1P3T2(targetRect.MinimumX,	targetRect.MinimumY,	0.0f,	0.0f,		0.0f),
					new VertexC1P3T2(targetRect.MaximumX,	targetRect.MinimumY,	0.0f,	uvRatio.X,	0.0f),
					new VertexC1P3T2(targetRect.MaximumX,	targetRect.MaximumY,	0.0f,	uvRatio.X,	uvRatio.Y),
					new VertexC1P3T2(targetRect.MinimumX,	targetRect.MaximumY,	0.0f,	0.0f,		uvRatio.Y));

				this.drawDevice.EndRendering();
				Profile.TimePostProcessing.EndMeasure();
			}
		}
		private void CollectDrawcalls()
		{
			// If no visibility groups are met, don't bother looking for renderers
			if ((this.drawDevice.VisibilityMask & VisibilityFlag.AllGroups) == VisibilityFlag.None) return;

			// Query renderers
			IEnumerable<ICmpRenderer> rendererQuery = Scene.Current.QueryVisibleRenderers(this.drawDevice);
			foreach (Predicate<ICmpRenderer> p in this.editorRenderFilter) rendererQuery = rendererQuery.Where(r => p(r));

			// Collect drawcalls
			if (this.drawDevice.IsPicking)
			{
				this.pickingMap = new List<ICmpRenderer>(rendererQuery);
				foreach (ICmpRenderer r in this.pickingMap)
				{
					r.Draw(this.drawDevice);
					this.drawDevice.PickingIndex++;
				}
			}
			else
			{
				Profile.TimeCollectDrawcalls.BeginMeasure();

				foreach (ICmpRenderer r in rendererQuery)
					r.Draw(this.drawDevice);

				Profile.TimeCollectDrawcalls.EndMeasure();
			}
		}
		private void SetupPickingRT(Vector2 size)
		{
			if (this.pickingTex == null || 
				this.pickingTex.PixelWidth != MathF.RoundToInt(size.X) || 
				this.pickingTex.PixelHeight != MathF.RoundToInt(size.Y))
			{
				if (this.pickingTex != null) this.pickingTex.Dispose();
				if (this.pickingRT != null) this.pickingRT.Dispose();
				this.pickingTex = new Texture(
					MathF.RoundToInt(size.X), MathF.RoundToInt(size.Y), Texture.SizeMode.Default, 
					TextureMagFilter.Nearest, TextureMinFilter.Nearest);
				this.pickingRT = new RenderTarget(AAQuality.Off, this.pickingTex);
			}
		}

		internal void AddEditorRendererFilter(Predicate<ICmpRenderer> filter)
		{
			if (this.editorRenderFilter.Contains(filter)) return;
			this.editorRenderFilter.Add(filter);
		}
		internal void RemoveEditorRendererFilter(Predicate<ICmpRenderer> filter)
		{
			this.editorRenderFilter.Remove(filter);
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
