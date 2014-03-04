using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Properties;

using OpenTK;
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
	[Serializable]
	[ExplicitResourceReference(typeof(Texture))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageRenderTarget)]
	public class RenderTarget : Resource
	{
		/// <summary>
		/// A RenderTarget resources file extension.
		/// </summary>
		public new const string FileExt = ".RenderTarget" + Resource.FileExt;
		
		/// <summary>
		/// Refers to a null reference RenderTarget.
		/// </summary>
		/// <seealso cref="ContentRef{T}.Null"/>
		public static readonly ContentRef<RenderTarget> None = ContentRef<RenderTarget>.Null;

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
		internal static RenderbufferStorage TexFormatToRboFormat(PixelInternalFormat format)
		{
			switch (format)
			{
				case PixelInternalFormat.Alpha:
				case PixelInternalFormat.Alpha8:
					return RenderbufferStorage.Alpha8;

				case PixelInternalFormat.R8:
				case PixelInternalFormat.Luminance:
					return RenderbufferStorage.R8;

				case PixelInternalFormat.Rg8:
				case PixelInternalFormat.LuminanceAlpha:
					return RenderbufferStorage.Rg8;

				case PixelInternalFormat.Rgb:
				case PixelInternalFormat.Rgb8:
					return RenderbufferStorage.Rgb8;

				default:
				case PixelInternalFormat.Rgba:
				case PixelInternalFormat.Rgba8:
					return RenderbufferStorage.Rgba8;

				case PixelInternalFormat.Rgba16f:
					return RenderbufferStorage.Rgba16f;

				case PixelInternalFormat.Rgba16:
					return RenderbufferStorage.Rgba16;
			}
		}


		[Serializable]
		private struct TargetInfo
		{
			[NonSerialized]	public	int	glRboIdColorMSAA;
			public	ContentRef<Texture>	target;

			public TargetInfo(ContentRef<Texture> target)
			{
				this.target = target;
				this.glRboIdColorMSAA = 0;
			}
		}

		
		private	List<TargetInfo>	targetInfo		= new List<TargetInfo>();
		private	AAQuality			multisampling	= AAQuality.Off;
		[NonSerialized] private	int	samples	= 0;
		[NonSerialized]	private	int	glFboId;
		[NonSerialized] private	int	glRboIdDepth;
		[NonSerialized] private	int	glFboIdMSAA;

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
		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			RenderTarget c = r as RenderTarget;
			c.multisampling	= this.multisampling;
			c.targetInfo	= new List<TargetInfo>(this.targetInfo);
			c.SetupOpenGLRes();
		}
	}
}
