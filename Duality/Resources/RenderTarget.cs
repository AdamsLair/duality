using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Properties;
using Duality.Cloning;
using Duality.Drawing;

using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// Instead of rendering to screen, RenderTargets can serve as an alternative drawing surface for a <see cref="Duality.Components.Camera"/>.
	/// The image is applied to one or several <see cref="Duality.Resources.Texture">Textures</see>. By default, only the first attached Texture
	/// is actually used, but you can use a custom <see cref="Duality.Resources.FragmentShader"/> to use all available Textures for storing
	/// information.
	/// </summary>
	/// <seealso cref="Duality.Resources.Texture"/>
	[ExplicitResourceReference(typeof(Texture))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageRenderTarget)]
	public class RenderTarget : Resource
	{
		private static int			maxFboSamples	= -1;
		private	static RenderTarget curBound		= null;

		/// <summary>
		/// [GET] The currently bound RenderTarget
		/// </summary>
		public static ContentRef<RenderTarget> BoundRT
		{
			get { return new ContentRef<RenderTarget>(curBound); }
		}
		/// <summary>
		/// [GET] The maximum number of available <see cref="Multisampling">Antialiazing</see> samples.
		/// </summary>
		public static int MaxRenderTargetSamples
		{
			get 
			{
				if (maxFboSamples == -1) GL.GetInteger(GetPName.MaxSamples, out maxFboSamples);
				return maxFboSamples;
			}
		}

		/// <summary>
		/// Binds a RenderTarget in order to use it.
		/// </summary>
		/// <param name="target">The RenderTarget to be bound.</param>
		public static void Bind(ContentRef<RenderTarget> target)
		{
			RenderTarget nextBound = target.IsExplicitNull ? null : target.Res;
			if (curBound == nextBound) return;

			if (curBound != null && nextBound != curBound)
			{
				// Blit multisampled fbo
				if (curBound.Samples > 0)
				{
					GL.Ext.BindFramebuffer(FramebufferTarget.ReadFramebuffer, curBound.glFboIdMSAA);
					GL.Ext.BindFramebuffer(FramebufferTarget.DrawFramebuffer, curBound.glFboId);
					for (int i = 0; i < curBound.targetInfo.Count; i++)
					{
						GL.ReadBuffer((ReadBufferMode)((int)ReadBufferMode.ColorAttachment0 + i));
						GL.DrawBuffer((DrawBufferMode)((int)DrawBufferMode.ColorAttachment0 + i));
						GL.Ext.BlitFramebuffer(
							0, 0, curBound.targetInfo[i].target.Res.TexelWidth, curBound.targetInfo[i].target.Res.TexelHeight,
							0, 0, curBound.targetInfo[i].target.Res.TexelWidth, curBound.targetInfo[i].target.Res.TexelHeight,
							ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
					}
					GL.ReadBuffer(ReadBufferMode.Back);
					GL.DrawBuffer(DrawBufferMode.Back);
					GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
				}

				// Generate Mipmaps for last bound
				for (int i = 0; i < curBound.targetInfo.Count; i++)
				{
					if (curBound.targetInfo[i].target.Res.HasMipmaps)
					{
						GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

						int lastTexId;
						GL.GetInteger(GetPName.TextureBinding2D, out lastTexId);

						if (lastTexId != curBound.targetInfo[i].target.Res.OglTexId) 
							GL.BindTexture(TextureTarget.Texture2D, curBound.targetInfo[i].target.Res.OglTexId);

						GL.Ext.GenerateMipmap(GenerateMipmapTarget.Texture2D);

						if (lastTexId != curBound.targetInfo[i].target.Res.OglTexId) 
							GL.BindTexture(TextureTarget.Texture2D, lastTexId);
					}
				}
			}

			// Bind new RenderTarget
			if (nextBound == null)
			{
				curBound = null;
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
				GL.DrawBuffer(DrawBufferMode.Back);
			}
			else
			{
				curBound = target.Res;
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, curBound.Samples > 0 ? curBound.glFboIdMSAA : curBound.glFboId);
				DrawBuffersEnum[] buffers = new DrawBuffersEnum[curBound.targetInfo.Count];
				for (int i = 0; i < buffers.Length; i++)
				{
					buffers[i] = (DrawBuffersEnum)((int)DrawBuffersEnum.ColorAttachment0 + i);
				}
				GL.DrawBuffers(curBound.targetInfo.Count, buffers);
			}
		}
		internal static RenderbufferStorage TexFormatToRboFormat(TexturePixelFormat format)
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


		private struct TargetInfo
		{
			[DontSerialize]	public	int	glRboIdColorMSAA;
			public	ContentRef<Texture>	target;

			public TargetInfo(ContentRef<Texture> target)
			{
				this.target = target;
				this.glRboIdColorMSAA = 0;
			}
		}

		
		private	List<TargetInfo>	targetInfo		= new List<TargetInfo>();
		private	AAQuality			multisampling	= AAQuality.Off;
		[DontSerialize] private	int	samples	= 0;
		[DontSerialize] private	int	glFboId;
		[DontSerialize] private	int	glRboIdDepth;
		[DontSerialize] private	int	glFboIdMSAA;


		/// <summary>
		/// [GET / SET] Whether this RenderTarget is multisampled.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public AAQuality Multisampling
		{
			get { return this.multisampling; }
			set
			{
				if (this.multisampling != value)
				{
					this.multisampling = value;
					this.FreeOpenGLRes();
					this.SetupOpenGLRes();
				}
			}
		}
		/// <summary>
		/// [GET / SET] An array of <see cref="Duality.Resources.Texture">Textures</see> used as data
		/// destination by this RenderTarget.
		/// </summary>
		[EditorHintFlags(MemberFlags.ForceWriteback | MemberFlags.AffectsOthers)]
		public ContentRef<Texture>[] Targets
		{
			get { return this.targetInfo.Select(i => i.target).ToArray(); }
			set
			{
				this.FreeOpenGLRes();
				this.targetInfo.Clear();
				if (value != null) foreach (var t in value) this.targetInfo.Add(new TargetInfo(t));
				this.SetupOpenGLRes();
			}
		}
		/// <summary>
		/// [GET] Width of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public int Width
		{
			get { return this.targetInfo.FirstOrDefault().target.IsAvailable ? this.targetInfo.FirstOrDefault().target.Res.PixelWidth : 0; }
		}
		/// <summary>
		/// [GET] Height of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public int Height
		{
			get { return this.targetInfo.FirstOrDefault().target.IsAvailable ? this.targetInfo.FirstOrDefault().target.Res.PixelHeight : 0; }
		}
		/// <summary>
		/// [GET] UVRatio of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public Vector2 UVRatio
		{
			get { return this.targetInfo.FirstOrDefault().target.IsAvailable ? this.targetInfo.FirstOrDefault().target.Res.UVRatio : Vector2.One; }
		}
		/// <summary>
		/// [GET] The number of <see cref="Multisampling">Antialiazing</see> samples this RenderTarget uses.
		/// </summary>
		public int Samples
		{
			get { return this.samples; }
		}

		/// <summary>
		/// Creates a new, empty RenderTarget
		/// </summary>
		public RenderTarget() : this(AAQuality.Off, null) {}
		/// <summary>
		/// Creates a new RenderTarget based on a set of <see cref="Duality.Resources.Texture">Textures</see>
		/// </summary>
		/// <param name="multisampling">The level of multisampling that is requested from this RenderTarget.</param>
		/// <param name="targets">An array of <see cref="Duality.Resources.Texture">Textures</see> used as data destination.</param>
		public RenderTarget(AAQuality multisampling, params ContentRef<Texture>[] targets)
		{
			this.multisampling = multisampling;
			if (targets != null) foreach (var t in targets) this.targetInfo.Add(new TargetInfo(t));
			this.SetupOpenGLRes();
		}
		
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="targetIndex">The <see cref="Targets"/> index to read from.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Width"/>.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Height"/>.</param>
		public Pixmap.Layer GetPixelData(int targetIndex = 0, int x = 0, int y = 0, int width = -1, int height = -1)
		{
			Pixmap.Layer target = new Pixmap.Layer();
			this.GetPixelData(target, targetIndex, x, y, width, height);
			return target;
		}
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="target">The target image to store the retrieved pixel data in.</param>
		/// <param name="targetIndex">The <see cref="Targets"/> index to read from.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Width"/>.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Height"/>.</param>
		public void GetPixelData(Pixmap.Layer target, int targetIndex = 0, int x = 0, int y = 0, int width = -1, int height = -1)
		{
			NormalizeReadRect(ref x, ref y, ref width, ref height, this.Width, this.Height);

			ColorRgba[] data = new ColorRgba[width * height];

			this.GetPixelDataInternal(data, targetIndex, x, y, width, height);
			target.SetPixelDataRgba(data, width, height);
		}
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="buffer">The buffer (RGBA format) to store all the pixel data in. Its byte length needs to be at least width * height * 4.</param>
		/// <param name="targetIndex">The <see cref="Targets"/> index to read from.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Width"/>.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Height"/>.</param>
		/// <returns>The number of bytes that were read.</returns>
		public int GetPixelData<T>(T[] buffer, int targetIndex = 0, int x = 0, int y = 0, int width = -1, int height = -1) where T : struct
		{
			NormalizeReadRect(ref x, ref y, ref width, ref height, this.Width, this.Height);
			return this.GetPixelDataInternal(buffer, targetIndex, x, y, width, height);
		}

		private int GetPixelDataInternal<T>(T[] buffer, int targetIndex, int x, int y, int width, int height) where T : struct
		{
			DualityApp.GuardSingleThreadState();

			int readBytes = width * height * 4;
			if (readBytes == 0) return 0;

			int readElements = readBytes / System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
			if (buffer.Length < readElements)
			{
				throw new ArgumentException(
					string.Format("The target buffer is too small. Its length needs to be at least {0}.", readElements), 
					"targetBuffer");
			}
			
			ContentRef<RenderTarget> lastRt = RenderTarget.BoundRT;
			RenderTarget.Bind(this);
			{
				GL.ReadBuffer((ReadBufferMode)((int)ReadBufferMode.ColorAttachment0 + targetIndex));
				GL.ReadPixels(x, y, width, height, PixelFormat.Rgba, PixelType.UnsignedByte, buffer);
				GL.ReadBuffer(ReadBufferMode.Back);
			}
			RenderTarget.Bind(lastRt);

			return readElements;
		}

		/// <summary>
		/// Frees up any OpenGL resources that this RenderTarget might have occupied.
		/// </summary>
		public void FreeOpenGLRes()
		{
			DualityApp.GuardSingleThreadState();

			if (this.glFboId != 0)
			{
				GL.Ext.DeleteFramebuffers(1, ref this.glFboId);
				this.glFboId = 0;
			}
			if (this.glRboIdDepth != 0)
			{
				GL.Ext.DeleteRenderbuffers(1, ref this.glRboIdDepth);
				this.glRboIdDepth = 0;
			}
			if (this.glFboIdMSAA != 0)
			{
				GL.Ext.DeleteFramebuffers(1, ref this.glFboIdMSAA);
				this.glFboIdMSAA = 0;
			}
			for (int i = 0; i < this.targetInfo.Count; i++)
			{
				TargetInfo infoTemp = this.targetInfo[i];
				if (this.targetInfo[i].glRboIdColorMSAA != 0)
				{
					GL.Ext.DeleteRenderbuffers(1, ref infoTemp.glRboIdColorMSAA);
					infoTemp.glRboIdColorMSAA = 0;
				}
				this.targetInfo[i] = infoTemp;
			}
		}
		/// <summary>
		/// Sets up all necessary OpenGL resources for this RenderTarget.
		/// </summary>
		public void SetupOpenGLRes()
		{
			DualityApp.GuardSingleThreadState();
			if (this.targetInfo == null) return;
			if (this.targetInfo.Count == 0) return;
			if (this.targetInfo.All(i => !i.target.IsAvailable)) return;
			
			int highestAALevel = MathF.RoundToInt(MathF.Log(MathF.Max(MaxRenderTargetSamples, 1.0f), 2.0f));
			int targetAALevel = highestAALevel;
			switch (this.multisampling)
			{
				case AAQuality.High:	targetAALevel = highestAALevel;		break;
				case AAQuality.Medium:	targetAALevel = highestAALevel / 2; break;
				case AAQuality.Low:		targetAALevel = highestAALevel / 4; break;
				case AAQuality.Off:		targetAALevel = 0;					break;
			}
			int targetSampleCount = MathF.RoundToInt(MathF.Pow(2.0f, targetAALevel));
			OpenTK.Graphics.GraphicsMode sampleMode = 
				DualityApp.AvailableModes.LastOrDefault(m => m.Samples <= targetSampleCount) ?? 
				DualityApp.AvailableModes.Last();

			this.samples = sampleMode.Samples;

			#region Setup FBO & RBO: Non-multisampled
			if (this.samples == 0)
			{
				// Generate FBO
				if (this.glFboId == 0) GL.Ext.GenFramebuffers(1, out this.glFboId);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.glFboId);

				// Attach textures
				int oglWidth = 0;
				int oglHeight = 0;
				for (int i = 0; i < this.targetInfo.Count; i++)
				{
					if (!this.targetInfo[i].target.IsAvailable) continue;
					FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
					GL.Ext.FramebufferTexture2D(
						FramebufferTarget.FramebufferExt, 
						attachment, 
						TextureTarget.Texture2D, 
						this.targetInfo[i].target.Res.OglTexId, 
						0);
					oglWidth = this.targetInfo[i].target.Res.TexelWidth;
					oglHeight = this.targetInfo[i].target.Res.TexelHeight;
				}

				// Generate Depth Renderbuffer
				if (this.glRboIdDepth == 0) GL.Ext.GenRenderbuffers(1, out this.glRboIdDepth);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, this.glRboIdDepth);
				GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, RenderbufferStorage.DepthComponent24, oglWidth, oglHeight);
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, this.glRboIdDepth);

				// Check status
				FramebufferErrorCode status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
				if (status != FramebufferErrorCode.FramebufferCompleteExt)
				{
					Log.Core.WriteError("Can't create RenderTarget '{0}'. Incomplete Framebuffer: {1}", this.path, status);
				}

				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
			}
			#endregion

			#region Setup FBO & RBO: Multisampled
			if (this.samples > 0)
			{
				// Generate texture target FBO
				if (this.glFboId == 0) GL.Ext.GenFramebuffers(1, out this.glFboId);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.glFboId);

				// Attach textures
				int oglWidth = 0;
				int oglHeight = 0;
				for (int i = 0; i < this.targetInfo.Count; i++)
				{
					if (!this.targetInfo[i].target.IsAvailable) continue;
					FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
					GL.Ext.FramebufferTexture2D(
						FramebufferTarget.FramebufferExt, 
						attachment, 
						TextureTarget.Texture2D, 
						this.targetInfo[i].target.Res.OglTexId, 
						0);
					oglWidth = this.targetInfo[i].target.Res.TexelWidth;
					oglHeight = this.targetInfo[i].target.Res.TexelHeight;
				}

				// Check status
				FramebufferErrorCode status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
				if (status != FramebufferErrorCode.FramebufferCompleteExt)
				{
					Log.Core.WriteError("Can't create RenderTarget '{0}'. Incomplete Texture Framebuffer: {1}", this.path, status);
				}

				// Generate rendering FBO
				if (this.glFboIdMSAA == 0) GL.Ext.GenFramebuffers(1, out this.glFboIdMSAA);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, this.glFboIdMSAA);

				// Attach color renderbuffers
				for (int i = 0; i < this.targetInfo.Count; i++)
				{
					if (!this.targetInfo[i].target.IsAvailable) continue;
					TargetInfo info = this.targetInfo[i];
					FramebufferAttachment attachment = (FramebufferAttachment)((int)FramebufferAttachment.ColorAttachment0Ext + i);
					RenderbufferStorage rbColorFormat = TexFormatToRboFormat(info.target.Res.PixelFormat);

					if (info.glRboIdColorMSAA == 0) GL.GenRenderbuffers(1, out info.glRboIdColorMSAA);
					GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, info.glRboIdColorMSAA);
					GL.Ext.RenderbufferStorageMultisample(RenderbufferTarget.RenderbufferExt, this.samples, rbColorFormat, oglWidth, oglHeight);
					GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, attachment, RenderbufferTarget.RenderbufferExt, info.glRboIdColorMSAA);
					this.targetInfo[i] = info;
				}
				GL.Ext.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

				// Attach depth renderbuffer
				if (this.glRboIdDepth == 0) GL.Ext.GenRenderbuffers(1, out this.glRboIdDepth);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, this.glRboIdDepth);
				GL.Ext.RenderbufferStorageMultisample(RenderbufferTarget.RenderbufferExt, this.samples, RenderbufferStorage.DepthComponent24, oglWidth, oglHeight);
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, this.glRboIdDepth);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, 0);

				// Check status
				status = GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);
				if (status != FramebufferErrorCode.FramebufferCompleteExt)
				{
					Log.Core.WriteError("Can't create RenderTarget '{0}'. Incomplete Multisample Framebuffer: {1}", this.path, status);
				}
				
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
			}
			#endregion
		}

		protected override void OnLoaded()
		{
			this.SetupOpenGLRes();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
			{
				this.FreeOpenGLRes();
			}
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			RenderTarget c = target as RenderTarget;
			c.SetupOpenGLRes();
		}

		private static void NormalizeReadRect(ref int x, ref int y, ref int width, ref int height, int realWidth, int realHeight)
		{
			if (width == -1) width = realWidth;
			if (height == -1) height = realHeight;

			x = Math.Max(x, 0);
			y = Math.Max(y, 0);
			width = MathF.Clamp(width, 0, realWidth - x);
			height = MathF.Clamp(height, 0, realHeight - x);
		}
	}
}
