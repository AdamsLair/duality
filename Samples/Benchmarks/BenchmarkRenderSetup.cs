using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Resources;

namespace Duality.Samples.Benchmarks
{
	/// <summary>
	/// A special <see cref="RenderSetup"/> that will render the entire scene under
	/// controlled conditions (resolution, antialiasing), then blit the result to
	/// its target output and draw diagnostic info on top of it.
	/// </summary>
	public class BenchmarkRenderSetup : RenderSetup
	{
		private Point2 renderingSize = new Point2(800, 600);
		private float resolutionScale = 1.0f;
		private AAQuality antialiasingQuality = AAQuality.Off;

		[DontSerialize] private RenderTarget sceneTarget;
		[DontSerialize] private Texture sceneTargetTex;
		[DontSerialize] private DrawDevice drawDevice;
		[DontSerialize] private Canvas overlayCanvas;
		[DontSerialize] private CanvasBuffer overlayBuffer;
		
		
		/// <summary>
		/// [GET / SET] The size at which the scene is rendered.
		/// </summary>
		public Point2 RenderingSize
		{
			get { return this.renderingSize; }
			set { this.renderingSize = Point2.Max(value, new Point2(1, 1)); }
		}
		/// <summary>
		/// [GET / SET] A scaling factor that is applied to the <see cref="RenderingSize"/>
		/// in order to render at a higher or lower resolution using the same view.
		/// </summary>
		public float ResolutionScale
		{
			get { return this.resolutionScale; }
			set { this.resolutionScale = MathF.Max(value, 0.0f); }
		}
		/// <summary>
		/// [GET / SET] The multisampling quality level to use when rendering the scene.
		/// </summary>
		public AAQuality AntialiasingQuality
		{
			get { return this.antialiasingQuality; }
			set { this.antialiasingQuality = value; }
		}
		/// <summary>
		/// [GET] The internal <see cref="RenderingSize"/>, scaled by <see cref="ResolutionScale"/>.
		/// It is used to determine the size of the offscreen <see cref="RenderTarget"/> to which the
		/// scene is rendered.
		/// </summary>
		private Point2 ScaledRenderingSize
		{
			get
			{
				// Determine the highest scale value before we'd arrive at our
				// hard pixel limit for the render target.
				float maxScale = MathF.Min(
					4096.0f / this.renderingSize.X,
					4096.0f / this.renderingSize.Y);
				float clampedScale = MathF.Min(maxScale, this.resolutionScale);

				// Scale the rendering size, clamp results
				Vector2 scaledSize = (this.renderingSize * clampedScale);
				return new Point2(
					MathF.Clamp(MathF.RoundToInt(scaledSize.X), 1, 4096), 
					MathF.Clamp(MathF.RoundToInt(scaledSize.Y), 1, 4096));
			}
		}


		/// <summary>
		/// Cleans up the internal offscreen render target that was set up
		/// in <see cref="SetupSceneTarget"/>.
		/// </summary>
		private void CleanupSceneTarget()
		{
			if (this.sceneTarget != null)
			{
				this.sceneTarget.Dispose();
				this.sceneTarget = null;
			}
			if (this.sceneTargetTex != null)
			{
				this.sceneTargetTex.Dispose();
				this.sceneTargetTex = null;
			}
		}
		/// <summary>
		/// Sets up the internal offscreen render target that is used for rendering
		/// the scene under controlled conditions.
		/// </summary>
		private void SetupSceneTarget()
		{
			Point2 targetSize = this.ScaledRenderingSize;

			// Initialize texture and render target
			if (this.sceneTargetTex == null)
			{
				this.sceneTargetTex = new Texture(
					targetSize.X,
					targetSize.Y,
					TextureSizeMode.NonPowerOfTwo,
					TextureMagFilter.Linear,
					TextureMinFilter.Linear,
					TextureWrapMode.Clamp,
					TextureWrapMode.Clamp);
			}
			if (this.sceneTarget == null)
			{
				this.sceneTarget = new RenderTarget(
					this.antialiasingQuality,
					true,
					this.sceneTargetTex);
			}

			// Adjust existing texture and render target to changed match settings
			if (this.sceneTargetTex.Size != targetSize ||
				this.sceneTarget.Multisampling != this.antialiasingQuality)
			{
				this.sceneTargetTex.Size = targetSize;
				this.sceneTargetTex.ReloadData();

				this.sceneTarget.Multisampling = this.antialiasingQuality;
				this.sceneTarget.SetupTarget();
			}
		}

		/// <summary>
		/// Collects drawcalls for the diagnostic benchmark overlay in screen space.
		/// </summary>
		private void DrawDiagnosticOverlay(Scene scene)
		{
			// Initialize a canvas and canvas buffer for rendering if not done yet
			if (this.overlayCanvas == null)
			{
				this.overlayBuffer = new CanvasBuffer();
				this.overlayCanvas = new Canvas(this.drawDevice, this.overlayBuffer);
			}

			// Notify the buffer that we're doing a new frame now, so it can
			// re-use buffered vertex arrays from previous frames
			this.overlayBuffer.Reset();

			// Collect drawcalls from all overlay renderers
			IEnumerable<ICmpBenchmarkOverlayRenderer> overlayRenderers = scene.FindComponents<ICmpBenchmarkOverlayRenderer>();
			foreach (ICmpBenchmarkOverlayRenderer renderer in overlayRenderers)
			{
				renderer.DrawOverlay(this.overlayCanvas);
			}
		}

		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.CleanupSceneTarget();
		}
		protected override void OnRenderScene(Scene scene, ContentRef<RenderTarget> target, Rect viewportRect, Vector2 imageSize)
		{
			// Render the entire scene into an internal offscreen target matching settings
			this.SetupSceneTarget();
			base.OnRenderScene(
				scene, 
				this.sceneTarget, 
				new Rect(this.sceneTarget.Size), 
				this.renderingSize);
			
			// Ensure we have a drawing device for screen space operations
			if (this.drawDevice == null)
			{
				this.drawDevice = new DrawDevice();
				this.drawDevice.Perspective = PerspectiveMode.Flat;
				this.drawDevice.RenderMode = RenderMatrix.ScreenSpace;
			}

			// Configure the drawing device to match parameters and settings
			this.drawDevice.Target = target;
			this.drawDevice.TargetSize = imageSize;
			this.drawDevice.ViewportRect = viewportRect;

			// Blit the results to screen
			BatchInfo blitMaterial = new BatchInfo(
				DrawTechnique.Solid, 
				ColorRgba.White, 
				this.sceneTargetTex);
			bool sceneTargetFitsOutput = 
				this.sceneTarget.Size.X <= imageSize.X && 
				this.sceneTarget.Size.Y <= imageSize.Y;
			TargetResize blitResize = sceneTargetFitsOutput ? 
				TargetResize.None : 
				TargetResize.Fit;

			this.drawDevice.ClearFlags = ClearFlag.All;
			this.drawDevice.PrepareForDrawcalls();
			this.drawDevice.AddFullscreenQuad(blitMaterial, blitResize);
			this.drawDevice.Render();

			// Draw a screen space diagnostic overlay
			this.drawDevice.ClearFlags = ClearFlag.Depth;
			this.drawDevice.PrepareForDrawcalls();
			this.DrawDiagnosticOverlay(scene);
			this.drawDevice.Render();
		}
	}
}