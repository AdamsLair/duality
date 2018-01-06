using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Duality.Backend.DefaultOpenTK
{
	[DontSerialize]
	public class NativeRenderTarget : INativeRenderTarget
	{
		private static int maxFboSamples = -1;
		public static int MaxRenderTargetSamples
		{
			get 
			{
				if (maxFboSamples == -1) GL.GetInteger(GetPName.MaxSamples, out maxFboSamples);
				return maxFboSamples;
			}
		}
		
		private	static NativeRenderTarget curBound = null;
		public static NativeRenderTarget BoundRT
		{
			get { return curBound; }
		}
		public static void Bind(NativeRenderTarget nextBound)
		{
			if (curBound == nextBound) return;

			// When binding a different target, execute pending post-render steps for the previous one
			if (curBound != null && curBound != nextBound)
				curBound.ApplyPostRender();

			// Bind new RenderTarget
			ApplyGLBind(nextBound);

			// Update binding info
			curBound = nextBound;
			if (curBound != null)
				curBound.pendingPostRender = true;
		}
		private static void ApplyGLBind(NativeRenderTarget target)
		{
			if (target == null)
			{
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
				GL.ReadBuffer(ReadBufferMode.Back);
				GL.DrawBuffer(DrawBufferMode.Back);
			}
			else
			{
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, target.samples > 0 ? target.handleMsaaFBO : target.handleMainFBO);
				DrawBuffersEnum[] buffers = new DrawBuffersEnum[target.targetInfos.Count];
				for (int i = 0; i < buffers.Length; i++)
				{
					buffers[i] = (DrawBuffersEnum)((int)DrawBuffersEnum.ColorAttachment0 + i);
				}
				GL.DrawBuffers(target.targetInfos.Count, buffers);
			}
		}


		private struct TargetInfo
		{
			public NativeTexture Target;
			public int HandleMsaaColorRBO;
		}
		

		private bool pendingPostRender = false;
		private int  handleMainFBO  = 0;
		private int  handleDepthRBO = 0;
		private int  handleMsaaFBO  = 0;
		private int  samples        = 0;
		private bool depthBuffer    = false;
		private RawList<TargetInfo> targetInfos = new RawList<TargetInfo>();


		public int Handle
		{
			get { return this.handleMainFBO; }
		}
		public int Width
		{
			get { return this.targetInfos.FirstOrDefault().Target != null ? this.targetInfos.FirstOrDefault().Target.Width : 0; }
		}
		public int Height
		{
			get { return this.targetInfos.FirstOrDefault().Target != null ? this.targetInfos.FirstOrDefault().Target.Height : 0; }
		}
		public int Samples
		{
			get { return this.samples; }
		}


		public void ApplyPostRender()
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			if (!this.pendingPostRender) return;
			
			// Resolve multisampling to the main FBO
			if (this.samples > 0)
			{
				GL.Ext.BindFramebuffer(FramebufferTarget.ReadFramebuffer, this.handleMsaaFBO);
				GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, this.handleMainFBO);
				for (int i = 0; i < this.targetInfos.Count; i++)
				{
					GL.ReadBuffer((ReadBufferMode)((int)ReadBufferMode.ColorAttachment0 + i));
					GL.DrawBuffer((DrawBufferMode)((int)DrawBufferMode.ColorAttachment0 + i));
					GL.Ext.BlitFramebuffer(
						0, 0, this.targetInfos.Data[i].Target.Width, this.targetInfos.Data[i].Target.Height,
						0, 0, this.targetInfos.Data[i].Target.Width, this.targetInfos.Data[i].Target.Height,
						ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
				}
			}
			
			// Generate mipmaps for the target textures
			int lastTexId = -1;
			for (int i = 0; i < this.targetInfos.Count; i++)
			{
				if (!this.targetInfos.Data[i].Target.HasMipmaps)
					continue;

				if (lastTexId == -1)
				{ 
					GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
					GL.GetInteger(GetPName.TextureBinding2D, out lastTexId);
				}

				int texId = this.targetInfos.Data[i].Target.Handle;
				GL.BindTexture(TextureTarget.Texture2D, texId);
				GL.Ext.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			}

			// Reset OpenGL state
			if (lastTexId != -1) GL.BindTexture(TextureTarget.Texture2D, lastTexId);
			ApplyGLBind(curBound);

			this.pendingPostRender = false;
		}

		void INativeRenderTarget.Setup(IReadOnlyList<INativeTexture> targets, AAQuality multisample, bool depthBuffer)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();

			if (targets == null) return;
			if (targets.Count == 0) return;
			if (targets.All(i => i == null)) return;
			
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(MaxRenderTargetSamples, 1.0f), 2.0f));
			int targetAALevel = highestAALevel;
			switch (multisample)
			{
				case AAQuality.High:	targetAALevel = highestAALevel;		break;
				case AAQuality.Medium:	targetAALevel = highestAALevel / 2; break;
				case AAQuality.Low:		targetAALevel = highestAALevel / 4; break;
				case AAQuality.Off:		targetAALevel = 0;					break;
			}
			int targetSampleCount = MathF.RoundToInt(MathF.Pow(2.0f, targetAALevel));
			GraphicsMode sampleMode = 
				GraphicsBackend.ActiveInstance.AvailableGraphicsModes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? 
				GraphicsBackend.ActiveInstance.AvailableGraphicsModes.Last();
			this.samples = sampleMode.Samples;
			this.depthBuffer = depthBuffer;

			// Synchronize target information
			{
				this.targetInfos.Reserve(targets.Count);
				int localIndex = 0;
				for (int i = 0; i < targets.Count; i++)
				{
					if (targets[i] == null) continue;

					this.targetInfos.Count = Math.Max(this.targetInfos.Count, localIndex + 1);
					this.targetInfos.Data[localIndex].Target = targets[i] as NativeTexture;

					localIndex++;
				}
			}

			// Setup OpenGL resources
			if (this.samples > 0)
				this.SetupMultisampled();
			else
				this.SetupNonMultisampled();
		}
		void INativeRenderTarget.GetData<T>(T[] buffer, ColorDataLayout dataLayout, ColorDataElementType dataElementType, int targetIndex, int x, int y, int width, int height)
		{
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();
			
			this.ApplyPostRender();
			if (curBound != this) ApplyGLBind(this);
			{
				GL.Ext.BindFramebuffer(FramebufferTarget.ReadFramebuffer, this.handleMainFBO);
				GL.ReadBuffer((ReadBufferMode)((int)ReadBufferMode.ColorAttachment0 + targetIndex));
				GL.ReadPixels(x, y, width, height, dataLayout.ToOpenTK(), dataElementType.ToOpenTK(), buffer);
			}
			ApplyGLBind(curBound);
		}
		void IDisposable.Dispose()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;
			DefaultOpenTKBackendPlugin.GuardSingleThreadState();
			
			// If there are changes pending to be applied to the bound textures,
			// they should be executed before the render target is gone.
			this.ApplyPostRender();

			if (this.handleMainFBO != 0)
			{
				GL.Ext.DeleteFramebuffers(1, ref this.handleMainFBO);
				this.handleMainFBO = 0;
			}
			if (this.handleDepthRBO != 0)
			{
				GL.Ext.DeleteRenderbuffers(1, ref this.handleDepthRBO);
				this.handleDepthRBO = 0;
			}
			if (this.handleMsaaFBO != 0)
			{
				GL.Ext.DeleteFramebuffers(1, ref this.handleMsaaFBO);
				this.handleMsaaFBO = 0;
			}
			for (int i = 0; i < this.targetInfos.Count; i++)
			{
				if (this.targetInfos.Data[i].HandleMsaaColorRBO != 0)
				{
					GL.Ext.DeleteRenderbuffers(1, ref this.targetInfos.Data[i].HandleMsaaColorRBO);
					this.targetInfos.Data[i].HandleMsaaColorRBO = 0;
				}
			}
		}

		private void SetupNonMultisampled()
		{
			// Generate FBO
			if (this.handleMainFBO == 0) GL.Ext.GenFramebuffers(1, out this.handleMainFBO);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.handleMainFBO);

			// Attach textures
			int oglWidth = 0;
			int oglHeight = 0;
			for (int i = 0; i < this.targetInfos.Count; i++)
			{
				NativeTexture tex = this.targetInfos[i].Target;

				FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
				GL.Ext.FramebufferTexture2D(
					FramebufferTarget.FramebufferExt, 
					attachment, 
					TextureTarget.Texture2D, 
					tex.Handle, 
					0);
				oglWidth = tex.Width;
				oglHeight = tex.Height;
			}

			// Generate or delete depth renderbuffer
			if (this.depthBuffer)
			{
				if (this.handleDepthRBO == 0) GL.Ext.GenRenderbuffers(1, out this.handleDepthRBO);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, this.handleDepthRBO);
				GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, RenderbufferStorage.DepthComponent24, oglWidth, oglHeight);
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, this.handleDepthRBO);
			}
			else
			{
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, 0);
				if (this.handleDepthRBO != 0) GL.Ext.DeleteRenderbuffers(1, ref this.handleDepthRBO);
				this.handleDepthRBO = 0;
			}

			// Check status
			FramebufferErrorCode status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
			if (status != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new BackendException(string.Format("Incomplete Framebuffer: {0}", status));
			}

			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
		}
		private void SetupMultisampled()
		{
			// Generate texture target FBO
			if (this.handleMainFBO == 0) GL.Ext.GenFramebuffers(1, out this.handleMainFBO);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.handleMainFBO);

			// Attach textures
			int oglWidth = 0;
			int oglHeight = 0;
			for (int i = 0; i < this.targetInfos.Count; i++)
			{
				NativeTexture tex = this.targetInfos[i].Target;

				FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
				GL.Ext.FramebufferTexture2D(
					FramebufferTarget.FramebufferExt, 
					attachment, 
					TextureTarget.Texture2D, 
					tex.Handle, 
					0);
				oglWidth = tex.Width;
				oglHeight = tex.Height;
			}

			// Check status
			FramebufferErrorCode status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
			if (status != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new BackendException(string.Format("Incomplete Framebuffer: {0}", status));
			}

			// Generate rendering FBO
			if (this.handleMsaaFBO == 0) GL.Ext.GenFramebuffers(1, out this.handleMsaaFBO);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.handleMsaaFBO);

			// Attach color renderbuffers
			for (int i = 0; i < this.targetInfos.Count; i++)
			{
				TargetInfo info = this.targetInfos.Data[i];

				FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
				RenderbufferStorage rbColorFormat = TexFormatToRboFormat(info.Target.Format);

				if (info.HandleMsaaColorRBO == 0) GL.GenRenderbuffers(1, out info.HandleMsaaColorRBO);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, info.HandleMsaaColorRBO);
				GL.Ext.RenderbufferStorageMultisample(RenderbufferTarget.RenderbufferExt, this.samples, rbColorFormat, oglWidth, oglHeight);
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, attachment, RenderbufferTarget.RenderbufferExt, info.HandleMsaaColorRBO);

				this.targetInfos.Data[i] = info;
			}
			GL.Ext.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

			// Generate or delete depth renderbuffer
			if (this.depthBuffer)
			{
				if (this.handleDepthRBO == 0) GL.Ext.GenRenderbuffers(1, out this.handleDepthRBO);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, this.handleDepthRBO);
				GL.Ext.RenderbufferStorageMultisample(RenderbufferTarget.RenderbufferExt, this.samples, RenderbufferStorage.DepthComponent24, oglWidth, oglHeight);
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, this.handleDepthRBO);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);
			}
			else
			{
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, 0);
				if (this.handleDepthRBO != 0) GL.Ext.DeleteRenderbuffers(1, ref this.handleDepthRBO);
				this.handleDepthRBO = 0;
			}

			// Check status
			status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
			if (status != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new BackendException(string.Format("Incomplete Multisample Framebuffer: {0}", status));
			}
				
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
		}

		private static RenderbufferStorage TexFormatToRboFormat(TexturePixelFormat format)
		{
			switch (format)
			{
				case TexturePixelFormat.Single:				return RenderbufferStorage.R8;
				case TexturePixelFormat.Dual:				return RenderbufferStorage.Rg8;
				case TexturePixelFormat.Rgb:				return RenderbufferStorage.Rgb8;
				case TexturePixelFormat.Rgba:				return RenderbufferStorage.Rgba8;

				case TexturePixelFormat.FloatSingle:		return RenderbufferStorage.R16f;
				case TexturePixelFormat.FloatDual:			return RenderbufferStorage.Rg16f;
				case TexturePixelFormat.FloatRgb:			return RenderbufferStorage.Rgb16f;
				case TexturePixelFormat.FloatRgba:			return RenderbufferStorage.Rgba16f;

				case TexturePixelFormat.CompressedSingle:	return RenderbufferStorage.R8;
				case TexturePixelFormat.CompressedDual:		return RenderbufferStorage.Rg8;
				case TexturePixelFormat.CompressedRgb:		return RenderbufferStorage.Rgb8;
				case TexturePixelFormat.CompressedRgba:		return RenderbufferStorage.Rgba8;
			}

			return RenderbufferStorage.Rgba8;
		}
	}
}
