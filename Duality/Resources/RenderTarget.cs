using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Editor;
using Duality.Properties;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Backend;

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
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageRenderTarget)]
	public class RenderTarget : Resource
	{
		private	List<ContentRef<Texture>>	targets			= new List<ContentRef<Texture>>();
		private	AAQuality					multisampling	= AAQuality.Off;

		[DontSerialize] private	INativeRenderTarget native = null;

		
		/// <summary>
		/// [GET] The backends native rendering target. You shouldn't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeRenderTarget Native
		{
			get { return this.native; }
		}
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
					this.FreeNativeRes();
					this.SetupNativeRes();
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
			get { return this.targets.ToArray(); }
			set
			{
				this.FreeNativeRes();
				this.targets.Clear();
				this.targets.AddRange(value);
				this.SetupNativeRes();
			}
		}
		/// <summary>
		/// [GET] Width of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public int Width
		{
			get
			{
				ContentRef<Texture> target = this.targets.Count > 0 ? this.targets[0] : null;
				return target.IsAvailable ? target.Res.PixelWidth : 0;
			}
		}
		/// <summary>
		/// [GET] Height of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public int Height
		{
			get
			{
				ContentRef<Texture> target = this.targets.Count > 0 ? this.targets[0] : null;
				return target.IsAvailable ? target.Res.PixelHeight : 0;
			}
		}
		/// <summary>
		/// [GET] UVRatio of this RenderTarget. This values is derived by its <see cref="Targets"/>.
		/// </summary>
		public Vector2 UVRatio
		{
			get
			{
				ContentRef<Texture> target = this.targets.Count > 0 ? this.targets[0] : null;
				return target.IsAvailable ? target.Res.UVRatio : Vector2.One;
			}
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
			if (targets != null) foreach (var t in targets) this.targets.Add(t);
			this.SetupNativeRes();
		}
		
		/// <summary>
		/// Sets up this RenderTarget, so it can be actively used. This is done automatically - unless
		/// one of the target textures gets resized or set up with different parameters after this
		/// target was already initialized, you shouldn't need to call this.
		/// </summary>
		public void SetupTarget()
		{
			this.SetupNativeRes();
		}

		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="targetIndex">The <see cref="Targets"/> index to read from.</param>
		/// <param name="x">The x position of the rectangular area to read.</param>
		/// <param name="y">The y position of the rectangular area to read.</param>
		/// <param name="width">The width of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Width"/>.</param>
		/// <param name="height">The height of the rectangular area to read. Defaults to the <see cref="RenderTarget"/> <see cref="Height"/>.</param>
		public PixelData GetPixelData(int targetIndex = 0, int x = 0, int y = 0, int width = -1, int height = -1)
		{
			PixelData target = new PixelData();
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
		public void GetPixelData(PixelData target, int targetIndex = 0, int x = 0, int y = 0, int width = -1, int height = -1)
		{
			NormalizeReadRect(ref x, ref y, ref width, ref height, this.Width, this.Height);

			ColorRgba[] data = new ColorRgba[width * height];

			this.GetPixelDataInternal(data, targetIndex, x, y, width, height);
			target.SetData(data, width, height);
		}
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="buffer">The buffer (Rgba8 format) to store all the pixel data in. Its byte length needs to be at least width * height * 4.</param>
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
			int readBytes = width * height * 4;
			if (readBytes == 0) return 0;

			int readElements = readBytes / System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
			if (buffer.Length < readElements)
			{
				throw new ArgumentException(
					string.Format("The target buffer is too small. Its length needs to be at least {0}.", readElements), 
					"targetBuffer");
			}
			
			this.native.GetData(buffer, ColorDataLayout.Rgba, ColorDataElementType.Byte, targetIndex, x, y, width, height);

			return readElements;
		}

		/// <summary>
		/// Frees up any native resources that this RenderTarget might have occupied.
		/// </summary>
		protected void FreeNativeRes()
		{
			if (this.native != null)
			{
				this.native.Dispose();
				this.native = null;
			}
		}
		/// <summary>
		/// Sets up all necessary native resources for this RenderTarget.
		/// </summary>
		protected void SetupNativeRes()
		{
			foreach (var target in this.targets.Where(t => t != null).Res())
			{
				if (target.PixelWidth == 0 || target.PixelHeight == 0)
				{
					Log.Core.WriteError("Error initializing RenderTarget {0}: Texture {1} has a dimension of 0 {2}",
						this.FullName, target.FullName, Environment.NewLine);
					return;
				}
			}

			if (this.native == null) this.native = DualityApp.GraphicsBackend.CreateRenderTarget();

			INativeTexture[] targets = this.targets
				.Select(t => t.Res != null ? t.Res.Native : null)
				.ToArray();

			try
			{
				this.native.Setup(targets, this.multisampling);
			}
			catch (Exception e)
			{
				Log.Core.WriteError("Error initializing RenderTarget {0}:{2}{1}", this.FullName, Log.Exception(e), Environment.NewLine);
			}
		}

		protected override void OnLoaded()
		{
			this.SetupNativeRes();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.FreeNativeRes();
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			RenderTarget c = target as RenderTarget;
			c.SetupNativeRes();
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
