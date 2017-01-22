using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Drawing;

namespace CustomRenderingSetup
{
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


		public ContentRef<DrawTechnique> TechFilterBrightness
		{
			get { return this.techFilterBrightness; }
			set { this.techFilterBrightness = value; }
		}
		public ContentRef<DrawTechnique> TechDownsample
		{
			get { return this.techDownsample; }
			set { this.techDownsample = value; }
		}
		public ContentRef<DrawTechnique> TechBlur
		{
			get { return this.techBlur; }
			set { this.techBlur = value; }
		}
		public ContentRef<DrawTechnique> TechCombineFinal
		{
			get { return this.techCombineFinal; }
			set { this.techCombineFinal = value; }
		}
		public float MinBrightness
		{
			get { return this.minBrightness; }
			set { this.minBrightness = value; }
		}
		public float BloomStrength
		{
			get { return this.bloomStrength; }
			set { this.bloomStrength = value; }
		}


		protected override void OnProcessRenderStep(RenderStep step, DrawDevice drawDevice)
		{
			if (step.Id == "Bloom")
				this.ProcessBloomStep(step, drawDevice);
			else
				base.OnProcessRenderStep(step, drawDevice);
		}
		private void ProcessBloomStep(RenderStep step, DrawDevice drawDevice)
		{
			Vector2 imageSize = drawDevice.TargetSize;
			Rect viewportRect = drawDevice.ViewportRect;

			this.SetupTargets((Point2)drawDevice.TargetSize);

			// Extract bright spots from the rendered image
			{
				BatchInfo material = new BatchInfo(this.techFilterBrightness, ColorRgba.White);
				material.MainTexture = step.Input.MainTexture;
				material.SetUniform("minBrightness", this.minBrightness);
				material.SetUniform("bloomStrength", this.bloomStrength);
				this.Blit(drawDevice, material, this.targetPingPongA[0]);
			}

			// Downsample to lowest target
			for (int i = 1; i < this.targetPingPongA.Length; i++)
			{
				this.Blit(drawDevice, this.targetPingPongA[i - 1], this.targetPingPongA[i], this.techDownsample);
			}

			// Blur all targets, separating horizontal and vertical blur
			for (int i = 0; i < this.targetPingPongA.Length; i++)
			{
				BatchInfo material = new BatchInfo(this.techBlur, ColorRgba.White);

				material.MainTexture = this.targetPingPongA[i].Targets[0];
				material.SetUniform("blurDirection", 1.0f, 0.0f);
				this.Blit(drawDevice, material, this.targetPingPongB[i]);

				material.MainTexture = this.targetPingPongB[i].Targets[0];
				material.SetUniform("blurDirection", 0.0f, 1.0f);
				this.Blit(drawDevice, material, this.targetPingPongA[i]);
			}

			// Combine all targets into the final image
			{
				BatchInfo material = new BatchInfo(this.techCombineFinal, ColorRgba.White);
				material.MainTexture = step.Input.MainTexture;
				material.SetTexture("blurFullTex", this.targetPingPongA[0].Targets[0]);
				material.SetTexture("blurHalfTex", this.targetPingPongA[1].Targets[0]);
				material.SetTexture("blurQuarterTex", this.targetPingPongA[2].Targets[0]);
				material.SetTexture("blurEighthTex", this.targetPingPongA[3].Targets[0]);
				this.Blit(drawDevice, material, viewportRect);
			}
		}
		
		private void Blit(DrawDevice device, RenderTarget source, RenderTarget target, ContentRef<DrawTechnique> technique)
		{
			this.Blit(device, source.Targets[0], target, technique);
		}
		private void Blit(DrawDevice device, ContentRef<Texture> source, RenderTarget target, ContentRef<DrawTechnique> technique)
		{
			this.Blit(device, new BatchInfo(technique, ColorRgba.White, source), target);
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
		private void Blit(DrawDevice device, RenderTarget source, Rect screenRect, ContentRef<DrawTechnique> technique)
		{
			this.Blit(device, source.Targets[0], screenRect, technique);
		}
		private void Blit(DrawDevice device, ContentRef<Texture> texture, Rect screenRect, ContentRef<DrawTechnique> technique)
		{
			this.Blit(device, new BatchInfo(technique, ColorRgba.White, texture), screenRect);
		}
		private void Blit(DrawDevice device, BatchInfo source, Rect screenRect)
		{
			device.Target = null;
			device.ViewportRect = screenRect;

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