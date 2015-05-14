using System;
using System.Collections.Generic;

using Duality.Editor;
using Duality.Properties;
using Duality.Drawing;
using Duality.Cloning;
using Duality.Backend;

namespace Duality.Resources
{
	/// <summary>
	/// A Texture refers to pixel data stored in video memory
	/// </summary>
	/// <seealso cref="Duality.Resources.Pixmap"/>
	/// <seealso cref="Duality.Resources.RenderTarget"/>
	[ExplicitResourceReference(typeof(Pixmap))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageTexture)]
	public class Texture : Resource
	{
		/// <summary>
		/// [GET] A Texture showing the Duality icon.
		/// </summary>
		public static ContentRef<Texture> DualityIcon		{ get; private set; }
		/// <summary>
		/// [GET] A Texture showing the Duality icon without the text on it.
		/// </summary>
		public static ContentRef<Texture> DualityIconB		{ get; private set; }
		/// <summary>
		/// A Texture showing the Duality logo.
		/// </summary>
		public static ContentRef<Texture> DualityLogoBig	{ get; private set; }
		/// <summary>
		/// A Texture showing the Duality logo.
		/// </summary>
		public static ContentRef<Texture> DualityLogoMedium	{ get; private set; }
		/// <summary>
		/// A Texture showing the Duality logo.
		/// </summary>
		public static ContentRef<Texture> DualityLogoSmall	{ get; private set; }
		/// <summary>
		/// [GET] A plain white 1x1 Texture. Can be used as a dummy.
		/// </summary>
		public static ContentRef<Texture> White				{ get; private set; }
		/// <summary>
		/// [GET] A 256x256 black and white checkerboard texture.
		/// </summary>
		public static ContentRef<Texture> Checkerboard		{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath				= ContentProvider.VirtualContentPath + "Texture:";
			const string ContentPath_DualityIcon		= VirtualContentPath + "DualityIcon";
			const string ContentPath_DualityIconB		= VirtualContentPath + "DualityIconB";
			const string ContentPath_DualityLogoBig		= VirtualContentPath + "DualityLogoBig";
			const string ContentPath_DualityLogoMedium	= VirtualContentPath + "DualityLogoMedium";
			const string ContentPath_DualityLogoSmall	= VirtualContentPath + "DualityLogoSmall";
			const string ContentPath_White				= VirtualContentPath + "White";
			const string ContentPath_Checkerboard		= VirtualContentPath + "Checkerboard";

			ContentProvider.AddContent(ContentPath_DualityIcon, new Texture(Pixmap.DualityIcon));
			ContentProvider.AddContent(ContentPath_DualityIconB, new Texture(Pixmap.DualityIconB));
			ContentProvider.AddContent(ContentPath_DualityLogoBig, new Texture(Pixmap.DualityLogoBig));
			ContentProvider.AddContent(ContentPath_DualityLogoMedium, new Texture(Pixmap.DualityLogoMedium));
			ContentProvider.AddContent(ContentPath_DualityLogoSmall, new Texture(Pixmap.DualityLogoSmall));
			ContentProvider.AddContent(ContentPath_White, new Texture(Pixmap.White));
			ContentProvider.AddContent(ContentPath_Checkerboard, new Texture(
				Pixmap.Checkerboard, 
				TextureSizeMode.Default,
				TextureMagFilter.Nearest,
				TextureMinFilter.Nearest,
				TextureWrapMode.Repeat,
				TextureWrapMode.Repeat));

			DualityIcon			= ContentProvider.RequestContent<Texture>(ContentPath_DualityIcon);
			DualityIconB		= ContentProvider.RequestContent<Texture>(ContentPath_DualityIconB);
			DualityLogoBig		= ContentProvider.RequestContent<Texture>(ContentPath_DualityLogoBig);
			DualityLogoMedium	= ContentProvider.RequestContent<Texture>(ContentPath_DualityLogoMedium);
			DualityLogoSmall	= ContentProvider.RequestContent<Texture>(ContentPath_DualityLogoSmall);
			White				= ContentProvider.RequestContent<Texture>(ContentPath_White);
			Checkerboard		= ContentProvider.RequestContent<Texture>(ContentPath_Checkerboard);
		}


		/// <summary>
		/// Creates a new Texture Resource based on the specified Pixmap, saves it and returns a reference to it.
		/// </summary>
		/// <param name="pixmap"></param>
		/// <returns></returns>
		public static ContentRef<Texture> CreateFromPixmap(ContentRef<Pixmap> pixmap)
		{
			string texPath = PathHelper.GetFreePath(pixmap.FullName, FileExt);
			Texture tex = new Texture(pixmap);
			tex.Save(texPath);
			return tex;
		}

		
		private	ContentRef<Pixmap>	basePixmap	= null;
		private	Vector2				size		= Vector2.Zero;
		private	TextureSizeMode		texSizeMode	= TextureSizeMode.Default;
		private	TextureMagFilter	filterMag	= TextureMagFilter.Linear;
		private	TextureMinFilter	filterMin	= TextureMinFilter.LinearMipmapLinear;
		private	TextureWrapMode		wrapX		= TextureWrapMode.Clamp;
		private	TextureWrapMode		wrapY		= TextureWrapMode.Clamp;
		private	TexturePixelFormat	pixelformat	= TexturePixelFormat.Rgba;
		private	bool				anisoFilter	= false;

		[DontSerialize] private	INativeTexture nativeTex = null;
		[DontSerialize] private	int		pxWidth		= 0;
		[DontSerialize] private	int		pxHeight	= 0;
		[DontSerialize] private	float	pxDiameter	= 0.0f;
		[DontSerialize] private	int		texWidth	= 0;
		[DontSerialize] private	int		texHeight	= 0;
		[DontSerialize] private	Vector2	uvRatio		= new Vector2(1.0f, 1.0f);
		[DontSerialize] private	bool	needsReload	= true;
		[DontSerialize] private	Rect[]	atlas		= null;


		/// <summary>
		/// [GET] The Textures internal texel width
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int TexelWidth
		{
			get { return this.texWidth; }
		}
		/// <summary>
		/// [GET] The Textures internal texel height
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int TexelHeight
		{
			get { return this.texHeight; }
		}
		/// <summary>
		/// [GET] The Textures original pixel width
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int PixelWidth
		{
			get { return this.pxWidth; }
		}
		/// <summary>
		/// [GET] The Textures original pixel height
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public int PixelHeight
		{
			get { return this.pxHeight; }
		}
		/// <summary>
		/// [GET] The backends native texture. You shouldn't use this unless you know exactly what you're doing.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public INativeTexture Native
		{
			get { return this.nativeTex; }
		}
		/// <summary>
		/// [GET] UV (Texture) coordinates for the Textures lower right
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector2 UVRatio
		{
			get { return this.uvRatio; }
		}
		/// <summary>
		/// [GET] Returns whether or not the texture uses mipmaps.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool HasMipmaps
		{
			get { return 
				this.filterMin == TextureMinFilter.LinearMipmapLinear ||
				this.filterMin == TextureMinFilter.LinearMipmapNearest ||
				this.filterMin == TextureMinFilter.NearestMipmapLinear ||
				this.filterMin == TextureMinFilter.NearestMipmapNearest; }
		}
		/// <summary>
		/// Indicates that the textures parameters have been changed in a way that will make it
		/// necessary to reload its data before using it next time.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool NeedsReload
		{
			get { return this.needsReload; }
		} 
		/// <summary>
		/// [GET / SET] The Textures size. Readonly, when created from a <see cref="BasePixmap"/>.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(0, int.MaxValue)]
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(0)]
		public Vector2 Size
		{
			get { return this.size; }
			set
			{
				if (this.basePixmap.IsExplicitNull && this.size != value)
				{
					this.AdjustSize(value.X, value.Y);
					this.needsReload = true;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The Textures magnifying filter
		/// </summary>
		public TextureMagFilter FilterMag
		{
			get { return this.filterMag; }
			set { if (this.filterMag != value) { this.filterMag = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] The Textures minifying filter
		/// </summary>
		public TextureMinFilter FilterMin
		{
			get { return this.filterMin; }
			set { if (this.filterMin != value) { this.filterMin = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] Specifies whether this texture uses anisotropic filtering.
		/// </summary>
		public bool AnisotropicFilter
		{
			get { return this.anisoFilter; }
			set { if (this.anisoFilter != value) { this.anisoFilter = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] The Textures horizontal wrap mode
		/// </summary>
		public TextureWrapMode WrapX
		{
			get { return this.wrapX; }
			set { if (this.wrapX != value) { this.wrapX = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] The Textures vertical wrap mode
		/// </summary>
		public TextureWrapMode WrapY
		{
			get { return this.wrapY; }
			set { if (this.wrapY != value) { this.wrapY = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] The Textures pixel format
		/// </summary>
		public TexturePixelFormat PixelFormat
		{
			get { return this.pixelformat; }
			set { if (this.pixelformat != value) { this.pixelformat = value; this.needsReload = true; } }
		}
		/// <summary>
		/// [GET / SET] Handles how the Textures base Pixmap is adjusted in order to fit GPU texture size requirements (Power of Two dimensions)
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public TextureSizeMode TexSizeMode
		{
			get { return this.texSizeMode; }
			set 
			{ 
				if (this.texSizeMode != value) 
				{ 
					this.texSizeMode = value; 
					this.AdjustSize(this.size.X, this.size.Y);
					this.needsReload = true;
				}
			}
		}
		/// <summary>
		/// [GET / SET] Reference to a Pixmap that contains the pixel data that is or has been uploaded to the Texture
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public ContentRef<Pixmap> BasePixmap
		{
			get { return this.basePixmap; }
			set { if (this.basePixmap.Res != value.Res) { this.basePixmap = value; this.needsReload = true; } }
		}


		/// <summary>
		/// Sets up a new, uninitialized Texture.
		/// </summary>
		public Texture() : this(0, 0) {}
		/// <summary>
		/// Creates a new Texture based on a <see cref="Duality.Resources.Pixmap"/>.
		/// </summary>
		/// <param name="basePixmap">The <see cref="Duality.Resources.Pixmap"/> to use as source for pixel data.</param>
		/// <param name="sizeMode">Specifies behaviour in case the source data has non-power-of-two dimensions.</param>
		/// <param name="filterMag">The OpenGL filter mode for drawing the Texture bigger than it is.</param>
		/// <param name="filterMin">The OpenGL fitler mode for drawing the Texture smaller than it is.</param>
		/// <param name="wrapX">The OpenGL wrap mode on the texel x axis.</param>
		/// <param name="wrapY">The OpenGL wrap mode on the texel y axis.</param>
		/// <param name="format">The format in which OpenGL stores the pixel data.</param>
		public Texture(ContentRef<Pixmap> basePixmap, 
			TextureSizeMode sizeMode	= TextureSizeMode.Default, 
			TextureMagFilter filterMag	= TextureMagFilter.Linear, 
			TextureMinFilter filterMin	= TextureMinFilter.LinearMipmapLinear,
			TextureWrapMode wrapX		= TextureWrapMode.Clamp,
			TextureWrapMode wrapY		= TextureWrapMode.Clamp,
			TexturePixelFormat format	= TexturePixelFormat.Rgba)
		{
			this.filterMag = filterMag;
			this.filterMin = filterMin;
			this.wrapX = wrapX;
			this.wrapY = wrapY;
			this.pixelformat = format;
			this.LoadData(basePixmap, sizeMode);
		}
		/// <summary>
		/// Creates a new empty Texture with the specified size.
		/// </summary>
		/// <param name="width">The Textures width.</param>
		/// <param name="height">The Textures height</param>
		/// <param name="sizeMode">Specifies behaviour in case the specified size has non-power-of-two dimensions.</param>
		/// <param name="filterMag">The OpenGL filter mode for drawing the Texture bigger than it is.</param>
		/// <param name="filterMin">The OpenGL fitler mode for drawing the Texture smaller than it is.</param>
		/// <param name="wrapX">The OpenGL wrap mode on the texel x axis.</param>
		/// <param name="wrapY">The OpenGL wrap mode on the texel y axis.</param>
		/// <param name="format">The format in which OpenGL stores the pixel data.</param>
		public Texture(int width, int height, 
			TextureSizeMode sizeMode	= TextureSizeMode.Default, 
			TextureMagFilter filterMag	= TextureMagFilter.Linear, 
			TextureMinFilter filterMin	= TextureMinFilter.LinearMipmapLinear,
			TextureWrapMode wrapX		= TextureWrapMode.Clamp,
			TextureWrapMode wrapY		= TextureWrapMode.Clamp,
			TexturePixelFormat format	= TexturePixelFormat.Rgba)
		{
			this.filterMag = filterMag;
			this.filterMin = filterMin;
			this.wrapX = wrapX;
			this.wrapY = wrapY;
			this.pixelformat = format;
			this.texSizeMode = sizeMode;
			this.AdjustSize(width, height);
			this.SetupNativeRes();
		}

		/// <summary>
		/// Reloads this Textures pixel data. If the referred <see cref="Duality.Resources.Pixmap"/> has been modified,
		/// changes will now be visible.
		/// </summary>
		public void ReloadData()
		{
			this.LoadData(this.basePixmap, this.texSizeMode);
		}
		/// <summary>
		/// Loads the specified <see cref="Duality.Resources.Pixmap">Pixmaps</see> pixel data.
		/// </summary>
		/// <param name="basePixmap">The <see cref="Duality.Resources.Pixmap"/> that is used as pixel data source.</param>
		public void LoadData(ContentRef<Pixmap> basePixmap)
		{
			this.LoadData(basePixmap, this.texSizeMode);
		}
		/// <summary>
		/// Loads the specified <see cref="Duality.Resources.Pixmap">Pixmaps</see> pixel data.
		/// </summary>
		/// <param name="basePixmap">The <see cref="Duality.Resources.Pixmap"/> that is used as pixel data source.</param>
		/// <param name="sizeMode">Specifies behaviour in case the source data has non-power-of-two dimensions.</param>
		public void LoadData(ContentRef<Pixmap> basePixmap, TextureSizeMode sizeMode)
		{
			if (this.nativeTex == null) this.nativeTex = DualityApp.GraphicsBackend.CreateTexture();
			this.needsReload = false;
			this.basePixmap = basePixmap;
			this.texSizeMode = sizeMode;

			if (!this.basePixmap.IsExplicitNull)
			{
				Pixmap.Layer pixelData = null;
				Pixmap basePixmapRes = this.basePixmap.IsAvailable ? this.basePixmap.Res : null;
				if (basePixmapRes != null)
				{
					pixelData = basePixmapRes.MainLayer;
					this.atlas = basePixmapRes.Atlas != null ? basePixmapRes.Atlas.ToArray() : null;
				}

				if (pixelData == null)
					pixelData = Pixmap.Checkerboard.Res.MainLayer;

				this.AdjustSize(pixelData.Width, pixelData.Height);
				this.SetupNativeRes();
				if (this.texSizeMode != TextureSizeMode.NonPowerOfTwo &&
					(this.pxWidth != this.texWidth || this.pxHeight != this.texHeight))
				{
					if (this.texSizeMode == TextureSizeMode.Enlarge)
					{
						Pixmap.Layer oldData = pixelData;
						pixelData = oldData.CloneResize(this.texWidth, this.texHeight);
						// Fill border pixels manually - that's cheaper than ColorTransparentPixels here.
						oldData.DrawOnto(pixelData, BlendMode.Solid, this.pxWidth, 0, 1, this.pxHeight, this.pxWidth - 1, 0);
						oldData.DrawOnto(pixelData, BlendMode.Solid, 0, this.pxHeight, this.pxWidth, 1, 0, this.pxHeight - 1);
					}
					else
						pixelData = pixelData.CloneRescale(this.texWidth, this.texHeight, Pixmap.FilterMethod.Linear);
				}

				// Load pixel data to video memory
				this.nativeTex.LoadData(this.pixelformat, pixelData.Width, pixelData.Height, pixelData.Data);
					
				// Adjust atlas to represent UV coordinates
				if (this.atlas != null)
				{
					Vector2 scale;
					scale.X = this.uvRatio.X / this.pxWidth;
					scale.Y = this.uvRatio.Y / this.pxHeight;
					for (int i = 0; i < this.atlas.Length; i++)
					{
						this.atlas[i].X *= scale.X;
						this.atlas[i].W *= scale.X;
						this.atlas[i].Y *= scale.Y;
						this.atlas[i].H *= scale.Y;
					}
				}
			}
			else
			{
				this.atlas = null;
				this.AdjustSize(this.size.X, this.size.Y);
				this.SetupNativeRes();
			}
		}

		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <returns></returns>
		public Pixmap.Layer GetPixelData()
		{
			Pixmap.Layer result = new Pixmap.Layer();
			this.GetPixelData(result);
			return result;
		}
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="target">The target image to store the retrieved pixel data in.</param>
		public void GetPixelData(Pixmap.Layer target)
		{
			ColorRgba[] data = new ColorRgba[this.texWidth * this.texHeight];
			this.GetPixelDataInternal(data);
			target.SetPixelDataRgba(data, this.texWidth, this.texHeight);
		}
		/// <summary>
		/// Retrieves the pixel data that is currently stored in video memory.
		/// </summary>
		/// <param name="targetBuffer">The buffer (RGBA format) to store all the pixel data in. 
		/// Its byte length should be at least <see cref="TexelWidth"/> * <see cref="TexelHeight"/> * 4.</param>
		/// <returns>The number of bytes that were read.</returns>
		public int GetPixelData<T>(T[] targetBuffer) where T : struct
		{
			return this.GetPixelDataInternal(targetBuffer);
		}

		private int GetPixelDataInternal<T>(T[] buffer) where T : struct
		{
			int readBytes = this.texWidth * this.texHeight * 4;
			if (readBytes == 0) return 0;

			int readElements = readBytes / System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
			if (buffer.Length < readElements)
			{
				throw new ArgumentException(
					string.Format("The target buffer is too small. Its length needs to be at least {0}.", readBytes), 
					"buffer");
			}

			this.nativeTex.GetData(buffer);

			return readElements;
		}

		/// <summary>
		/// Does a safe (null-checked, clamped) texture <see cref="Duality.Resources.Pixmap.Atlas"/> lookup.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="uv"></param>
		public void LookupAtlas(int index, out Rect uv)
		{
			if (this.atlas == null)
			{
				uv.X = uv.Y = 0.0f;
				uv.W = this.uvRatio.X;
				uv.H = this.uvRatio.Y;
			}
			else
			{
				uv = this.atlas[MathF.Clamp(index, 0, this.atlas.Length - 1)];
			}
		}
		/// <summary>
		/// Does a safe (null-checked, clamped) texture <see cref="Duality.Resources.Pixmap.Atlas"/> lookup.
		/// </summary>
		/// <param name="index"></param>
		public Rect LookupAtlas(int index)
		{
			Rect result;
			this.LookupAtlas(index, out result);
			return result;
		}

		/// <summary>
		/// Processes the specified size based on the Textures <see cref="TextureSizeMode"/>.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		protected void AdjustSize(float width, float height)
		{
			this.size = new Vector2(MathF.Abs(width), MathF.Abs(height));
			this.pxWidth = MathF.RoundToInt(this.size.X);
			this.pxHeight = MathF.RoundToInt(this.size.Y);
			this.pxDiameter = MathF.Distance(this.pxWidth, this.pxHeight);

			if (this.texSizeMode == TextureSizeMode.NonPowerOfTwo)
			{
				this.texWidth = this.pxWidth;
				this.texHeight = this.pxHeight;
				this.uvRatio = Vector2.One;
			}
			else
			{
				this.texWidth = MathF.NextPowerOfTwo(this.pxWidth);
				this.texHeight = MathF.NextPowerOfTwo(this.pxHeight);
				if (this.pxWidth != this.texWidth || this.pxHeight != this.texHeight)
				{
					if (this.texSizeMode == TextureSizeMode.Enlarge)
					{
						this.uvRatio.X = (float)this.pxWidth / (float)this.texWidth;
						this.uvRatio.Y = (float)this.pxHeight / (float)this.texHeight;
					}
					else
						this.uvRatio = Vector2.One;
				}
				else
					this.uvRatio = Vector2.One;
			}
		}
		/// <summary>
		/// Sets up the Textures OpenGL resources, clearing previously uploaded pixel data.
		/// </summary>
		protected void SetupNativeRes()
		{
			if (this.nativeTex == null) this.nativeTex = DualityApp.GraphicsBackend.CreateTexture();

			this.nativeTex.SetupEmpty(
				this.pixelformat,
				this.texWidth, this.texHeight,
				this.filterMin, this.filterMag,
				this.wrapX, this.wrapY,
				this.anisoFilter ? 4 : 0,
				this.HasMipmaps);
		}

		protected override void OnLoaded()
		{
			this.LoadData(this.basePixmap, this.texSizeMode);
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);

			// Dispose unmanaged Resources
			if (this.nativeTex != null)
			{
				this.nativeTex.Dispose();
				this.nativeTex = null;
			}

			// Get rid of big data references, so the GC can collect them.
			this.basePixmap.Detach();
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			Texture c = target as Texture;
			c.LoadData(this.basePixmap, this.texSizeMode);
		}
	}
}
