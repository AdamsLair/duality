using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Drawing;

namespace CustomRenderingSetup
{
	/// <summary>
	/// A custom rendering setup that has a special code path for a programmatic bloom filter
	/// which is triggered in <see cref="RenderStep"/> instances matching the <see cref="RenderStep.Id"/> "Bloom".
	/// </summary>
	public class CustomBloomRenderSetup : RenderSetup
	{
		private static readonly int PyramidSize = 4;

		private ContentRef<DrawTechnique> techFilterBrightness = null;
		private ContentRef<DrawTechnique> techDownsample       = null;
		private ContentRef<DrawTechnique> techBlur             = null;
		private ContentRef<DrawTechnique> techCombineFinal     = null;
		private float minBrightness = 0.75f;
		private float bloomStrength = 1.0f;

		[DontSerialize] private RenderTarget[] targetPingPongA = new RenderTarget[PyramidSize];
		[DontSerialize] private RenderTarget[] targetPingPongB = new RenderTarget[PyramidSize];


		/// <summary>
		/// [GET / SET] A <see cref="DrawTechnique"/> that extracts bloom brightness from a rendered source image.
		/// </summary>
		public ContentRef<DrawTechnique> TechFilterBrightness
		{
			get { return this.techFilterBrightness; }
			set { this.techFilterBrightness = value; }
		}
		/// <summary>
		/// [GET / SET] A <see cref="DrawTechnique"/> that downsamples a source image by half.
		/// </summary>
		public ContentRef<DrawTechnique> TechDownsample
		{
			get { return this.techDownsample; }
			set { this.techDownsample = value; }
		}
		/// <summary>
		/// [GET / SET] A <see cref="DrawTechnique"/> that blurs the source image in one direction.
		/// </summary>
		public ContentRef<DrawTechnique> TechBlur
		{
			get { return this.techBlur; }
			set { this.techBlur = value; }
		}
		/// <summary>
		/// [GET / SET] A <see cref="DrawTechnique"/> that combines a main texture with a set of bloom textures at
		/// various scales into a final image.
		/// </summary>
		public ContentRef<DrawTechnique> TechCombineFinal
		{
			get { return this.techCombineFinal; }
			set { this.techCombineFinal = value; }
		}
		/// <summary>
		/// [GET / SET] The minimum brightness value that a pixel needs to surpass in order to become part of
		/// the bloom step.
		/// </summary>
		[EditorHintRange(0.0f, 1.0f)]
		public float MinBrightness
		{
			get { return this.minBrightness; }
			set { this.minBrightness = value; }
		}
		/// <summary>
		/// [GET / SET] Strength multiplier for the final combine step that merges all bloom images with the
		/// main rendered image.
		/// </summary>
		[EditorHintRange(0.0f, 1.0f)]
		public float BloomStrength
		{
			get { return this.bloomStrength; }
			set { this.bloomStrength = value; }
		}


		protected override void OnRenderSingleStep(RenderStep step, Scene scene, DrawDevice drawDevice)
		{
			if (step.Id == "Bloom")
				this.ProcessBloomStep(step, drawDevice);
			else
				base.OnRenderSingleStep(step, scene, drawDevice);
		}
		private void ProcessBloomStep(RenderStep step, DrawDevice drawDevice)
		{
			ContentRef<RenderTarget> outputTarget = drawDevice.Target;
			Vector2 imageSize = drawDevice.TargetSize;
			Rect viewportRect = drawDevice.ViewportRect;

			this.SetupTargets((Point2)drawDevice.TargetSize);

			// Extract bright spots from the rendered image
			{
				BatchInfo material = new BatchInfo(this.techFilterBrightness, ColorRgba.White);
				material.MainTexture = step.Input.MainTexture;
				material.Parameters.Set("minBrightness", this.minBrightness);
				material.Parameters.Set("bloomStrength", this.bloomStrength);
				this.Blit(drawDevice, material, this.targetPingPongA[0]);
			}

			// Downsample to lowest target
			for (int i = 1; i < this.targetPingPongA.Length; i++)
			{
				BatchInfo material = new BatchInfo(this.techDownsample, ColorRgba.White);
				material.MainTexture = this.targetPingPongA[i - 1].Targets[0];
				this.Blit(drawDevice, material, this.targetPingPongA[i]);
			}

			// Blur all targets, separating horizontal and vertical blur
			for (int i = 0; i < this.targetPingPongA.Length; i++)
			{
				BatchInfo material = new BatchInfo(this.techBlur, ColorRgba.White);

				material.MainTexture = this.targetPingPongA[i].Targets[0];
				material.Parameters.Set("blurDirection", new Vector2(1.0f, 0.0f));
				this.Blit(drawDevice, material, this.targetPingPongB[i]);

				material.MainTexture = this.targetPingPongB[i].Targets[0];
				material.Parameters.Set("blurDirection", new Vector2(0.0f, 1.0f));
				this.Blit(drawDevice, material, this.targetPingPongA[i]);
			}

			// Combine all targets into the final image using the draw device's original target
			{
				BatchInfo material = new BatchInfo(this.techCombineFinal, ColorRgba.White);
				material.MainTexture = step.Input.MainTexture;
				material.Parameters.Set("blurFullTex", this.targetPingPongA[0].Targets[0]);
				material.Parameters.Set("blurHalfTex", this.targetPingPongA[1].Targets[0]);
				material.Parameters.Set("blurQuarterTex", this.targetPingPongA[2].Targets[0]);
				material.Parameters.Set("blurEighthTex", this.targetPingPongA[3].Targets[0]);
				this.Blit(drawDevice, material, outputTarget.Res, imageSize, viewportRect);
			}
		}
		
		private void Blit(DrawDevice device, BatchInfo source, RenderTarget target)
		{
			device.Target = target;
			device.TargetSize = target.Size;
			device.ViewportRect = new Rect(target.Size);

			device.PrepareForDrawcalls();
			device.AddFullscreenQuad(source, TargetResize.Stretch);
			device.Render();
		}
		private void Blit(DrawDevice device, BatchInfo source, RenderTarget target, Vector2 targetSize, Rect viewportRect)
		{
			device.Target = target;
			device.TargetSize = targetSize;
			device.ViewportRect = viewportRect;

			device.PrepareForDrawcalls();
			device.AddFullscreenQuad(source, TargetResize.Stretch);
			device.Render();
		}

		private void SetupTargets(Point2 size)
		{
			for (int i = 0; i < this.targetPingPongA.Length; i++)
			{
				this.SetupTarget(ref this.targetPingPongA[i], size);
				this.SetupTarget(ref this.targetPingPongB[i], size);
				size /= 2;
			}
		}
		private void SetupTarget(ref RenderTarget renderTarget, Point2 size)
		{
			// Create a new rendering target and backing texture, if not existing yet
			if (renderTarget == null)
			{
				Texture tex = new Texture(
					size.X, 
					size.Y, 
					TextureSizeMode.NonPowerOfTwo, 
					TextureMagFilter.Linear, 
					TextureMinFilter.Linear);
				renderTarget = new RenderTarget
				{
					Targets = new ContentRef<Texture>[] { tex },
					Multisampling = AAQuality.Off,
					DepthBuffer = false
				};
			}

			// Resize the existing target to match the specified size
			if (renderTarget.Size != size)
			{
				Texture tex = renderTarget.Targets[0].Res;
				tex.Size = size;
				tex.ReloadData();
				renderTarget.SetupTarget();
			}
		}
	}
}