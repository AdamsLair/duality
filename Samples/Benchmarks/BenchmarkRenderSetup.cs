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
		[DontSerialize] private DrawDevice blitDevice;
		
		
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
					MathF.Clamp((int)scaledSize.X, 1, 4096), 
					MathF.Clamp((int)scaledSize.Y, 1, 4096));
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
		/// Sets up a <see cref="DrawDevice"/> to be used for blitting an internal
		/// offscreen rendering target to the actual output surface.
		/// </summary>
		private void SetupBlitDevice()
		{
			if (this.blitDevice == null)
			{
				this.blitDevice = new DrawDevice();
				this.blitDevice.ClearFlags = ClearFlag.Depth;
				this.blitDevice.Perspective = PerspectiveMode.Flat;
				this.blitDevice.RenderMode = RenderMatrix.ScreenSpace;
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

			// Blit the results to screen
			this.SetupBlitDevice();
			this.blitDevice.TargetSize = imageSize;
			this.blitDevice.ViewportRect = viewportRect;

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

			this.blitDevice.PrepareForDrawcalls();
			this.blitDevice.AddFullscreenQuad(blitMaterial, blitResize);
			this.blitDevice.Render();
		}
	}
}