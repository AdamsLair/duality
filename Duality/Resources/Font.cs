using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using SysDrawFont = System.Drawing.Font;

using Duality.ColorFormat;
using Duality.VertexFormat;
using Duality.EditorHints;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Duality.Resources
{
	/// <summary>
	/// Represents a font. While any system font or imported TrueType font can be used, they are internally
	/// pre-rasterized and stored in a <see cref="Duality.Resources.Texture"/> with an <see cref="Duality.Resources.Pixmap.Atlas"/>.
	/// </summary>
	[Serializable]
	[ExplicitResourceReference()]
	public class Font : Resource
	{
		/// <summary>
		/// A Font resources file extension.
		/// </summary>
		public new const string FileExt = ".Font" + Resource.FileExt;
		
		/// <summary>
		/// A generic <see cref="MonoSpace">monospace</see> Font (Size 8) that has been loaded from your systems font library.
		/// This is usually "Courier New".
		/// </summary>
		public static ContentRef<Font> GenericMonospace8	{ get; private set; }
		/// <summary>
		/// A generic <see cref="MonoSpace">monospace</see> Font (Size 10) that has been loaded from your systems font library.
		/// This is usually "Courier New".
		/// </summary>
		public static ContentRef<Font> GenericMonospace10	{ get; private set; }
		/// <summary>
		/// A generic serif Font (Size 12) that has been loaded from your systems font library.
		/// This is usually "Times New Roman".
		/// </summary>
		public static ContentRef<Font> GenericSerif12		{ get; private set; }
		/// <summary>
		/// A generic sans-serif Font (Size 12) that has been loaded from your systems font library.
		/// This is usually "Arial".
		/// </summary>
		public static ContentRef<Font> GenericSansSerif12	{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath				= ContentProvider.VirtualContentPath + "Font:";
			const string ContentPath_GenericMonospace10	= VirtualContentPath + "GenericMonospace10";
			const string ContentPath_GenericMonospace8	= VirtualContentPath + "GenericMonospace8";
			const string ContentPath_GenericSerif12		= VirtualContentPath + "GenericSerif12";
			const string ContentPath_GenericSansSerif12	= VirtualContentPath + "GenericSansSerif12";

			Font tmp;
			
			tmp = new Font();
			tmp.Family = FontFamily.GenericMonospace.Name;
			tmp.Size = 8;
			tmp.CharSpacing = 0;
			tmp.GlyphRenderMode = RenderMode.MonochromeBitmap;
			tmp.MonoSpace = true;
			tmp.ReloadData();
			ContentProvider.RegisterContent(ContentPath_GenericMonospace8, tmp);

			tmp = new Font();
			tmp.Family = FontFamily.GenericMonospace.Name;
			tmp.Size = 10;
			tmp.CharSpacing = 0;
			tmp.GlyphRenderMode = RenderMode.MonochromeBitmap;
			tmp.MonoSpace = true;
			tmp.ReloadData();
			ContentProvider.RegisterContent(ContentPath_GenericMonospace10, tmp);

			tmp = new Font();
			tmp.Family = FontFamily.GenericSerif.Name;
			tmp.Size = 12;
			tmp.GlyphRenderMode = RenderMode.MonochromeBitmap;
			tmp.Kerning = true;
			tmp.ReloadData();
			ContentProvider.RegisterContent(ContentPath_GenericSerif12, tmp);

			tmp = new Font();
			tmp.Family = FontFamily.GenericSansSerif.Name;
			tmp.Size = 12;
			tmp.GlyphRenderMode = RenderMode.MonochromeBitmap;
			tmp.Kerning = true;
			tmp.ReloadData();
			ContentProvider.RegisterContent(ContentPath_GenericSansSerif12, tmp);

			GenericMonospace8	= ContentProvider.RequestContent<Font>(ContentPath_GenericMonospace8);
			GenericMonospace10	= ContentProvider.RequestContent<Font>(ContentPath_GenericMonospace10);
			GenericSerif12		= ContentProvider.RequestContent<Font>(ContentPath_GenericSerif12);
			GenericSansSerif12	= ContentProvider.RequestContent<Font>(ContentPath_GenericSansSerif12);
		}

		
		/// <summary>
		/// Refers to a null reference Font.
		/// </summary>
		/// <seealso cref="ContentRef{T}.Null"/>
		public static readonly ContentRef<Font> None			= ContentRef<Font>.Null;
		/// <summary>
		/// A string containing all characters that are supported by Duality.
		/// </summary>
		public static readonly string			SupportedChars	= " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,;.:-_<>|#'+*~@^°!\"§$%&/()=?`²³{[]}\\´öäüÖÄÜß";
		private const string					BodyAscentRef = "acehmnorsuvwxz";
		private static readonly int[]			CharLookup;

		private	static	PrivateFontCollection			fontManager			= new PrivateFontCollection();
		private	static	Dictionary<string,FontFamily>	loadedFontRegistry	= new Dictionary<string,FontFamily>();

		static Font()
		{
			int maxCharVal = 0;
			for (int i = 0; i < SupportedChars.Length; i++) maxCharVal = Math.Max(maxCharVal, (int)SupportedChars[i]);

			int[] cl = new int[maxCharVal + 1];
			for (int i = 0; i < SupportedChars.Length; i++) cl[SupportedChars[i]] = i;

			CharLookup = cl;
		}


		/// <summary>
		/// Specifies how a text fitting algorithm works.
		/// </summary>
		public enum FitTextMode
		{
			/// <summary>
			/// Text is fit by character, i.e. can be separated anywhere.
			/// </summary>
			ByChar,
			/// <summary>
			/// Text is fit <see cref="ByWord">by word</see>, preferring leading whitespaces.
			/// </summary>
			ByWordLeadingSpace,
			/// <summary>
			/// Text is fit <see cref="ByWord">by word</see>, preferring trailing whitespaces.
			/// </summary>
			ByWordTrailingSpace,
			/// <summary>
			/// Text is fit by word boundaries, i.e. can only be separated between words.
			/// </summary>
			ByWord = ByWordTrailingSpace
		}

		/// <summary>
		/// Specifies how a Font is rendered. This affects both internal glyph rasterization and rendering.
		/// </summary>
		public enum RenderMode
		{
			/// <summary>
			/// A monochrome bitmap is used to store glyphs. Rendering is unfiltered and pixel-perfect.
			/// </summary>
			MonochromeBitmap,
			/// <summary>
			/// A greyscale bitmap is used to store glyphs. Rendering is unfiltered and pixel-perfect.
			/// </summary>
			GrayscaleBitmap,
			/// <summary>
			/// A greyscale bitmap is used to store glyphs. Rendering is properly filtered but may blur text display a little.
			/// </summary>
			SmoothBitmap,
			/// <summary>
			/// A greyscale bitmap is used to store glyphs. Rendering is properly filtered and uses a shader to enforce sharp masked edges.
			/// </summary>
			SharpBitmap
		}

		/// <summary>
		/// Contains data about a single glyph.
		/// </summary>
		public struct GlyphData
		{
			/// <summary>
			/// Thw width of the glyph
			/// </summary>
			public	int		width;
			/// <summary>
			/// The height of the glyph
			/// </summary>
			public	int		height;
			/// <summary>
			/// The glyphs X offset when rendering it.
			/// </summary>
			public	int		offsetX;
			/// <summary>
			/// The glyphs kerning samples to the left.
			/// </summary>
			public	int[]	kerningSamplesLeft;
			/// <summary>
			/// The glyphs kerning samples to the right.
			/// </summary>
			public	int[]	kerningSamplesRight;
		}

		
		private	string		familyName			= FontFamily.GenericMonospace.Name;
		private	float		size				= 10.0f;
		private	FontStyle	style				= FontStyle.Regular;
		private	RenderMode	renderMode			= RenderMode.SharpBitmap;
		private	float		spacing				= 0.0f;
		private	float		lineHeightFactor	= 1.0f;
		private	bool		monospace			= false;
		private	bool		kerning				= false;
		// Embedded custom font family
		private	byte[]		customFamilyData	= null;
		// Data that is automatically acquired while loading the font
		[NonSerialized]	private SysDrawFont	internalFont	= null;
		[NonSerialized]	private	GlyphData[]	glyphs			= null;
		[NonSerialized]	private	Material	mat				= null;
		[NonSerialized]	private	Pixmap		pixelData		= null;
		[NonSerialized]	private	Texture		texture			= null;
		[NonSerialized] private	bool		needsReload		= false;
		[NonSerialized] private	int			maxGlyphWidth	= 0;
		[NonSerialized] private	int			height			= 0;
		[NonSerialized] private	int			ascent			= 0;
		[NonSerialized]	private	int			bodyAscent		= 0;
		[NonSerialized] private	int			descent			= 0;
		[NonSerialized] private	int			baseLine		= 0;


		/// <summary>
		/// [GET / SET] The name of the font family that is used.
		/// </summary>
		public string Family
		{
			get { return this.familyName; }
			set 
			{
				// Do not allow changing the family if a custom family is used
				if (this.customFamilyData != null) return;

				this.familyName = value;
				this.needsReload = true;
			}
		}
		/// <summary>
		/// [GET / SET] The size of the Font.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		[EditorHintRange(1, 150)]
		[EditorHintIncrement(1)]
		[EditorHintDecimalPlaces(1)]
		public float Size
		{
			get { return this.size; }
			set 
			{ 
				if (this.size != value)
				{
					this.size = Math.Max(1.0f, value);
					this.UpdateInternalFont();

					this.spacing = this.internalFont.Size / 10.0f;
					this.needsReload = true;
				}
			}
		}
		/// <summary>
		/// [GET / SET] The style of the font.
		/// </summary>
		public FontStyle Style
		{
			get { return this.style; }
			set
			{
				this.style = value;
				this.needsReload = true;
			}
		}
		/// <summary>
		/// [GET / SET] Specifies how a Font is rendered. This affects both internal glyph rasterization and rendering.
		/// </summary>
		public RenderMode GlyphRenderMode
		{
			get { return this.renderMode; }
			set
			{
				this.renderMode = value;
				this.UpdateInternalFont();
				this.needsReload = true;
			}
		}
		/// <summary>
		/// [GET] The <see cref="Duality.Resources.Material"/> to use when rendering text of this Font.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public Material Material
		{
			get { return this.mat; }
		}
		/// <summary>
		/// [GET / SET] Additional spacing between each character. This is usually one tenth of the Fonts <see cref="Size"/>.
		/// </summary>
		public float CharSpacing
		{
			get { return this.spacing; }
			set { this.spacing = value; }
		}
		/// <summary>
		/// [GET / SET] A factor for the Fonts <see cref="Height"/> value that affects line spacings but not actual glyph sizes.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public float LineHeightFactor
		{
			get { return this.lineHeightFactor; }
			set { this.lineHeightFactor = value; }
		}
		/// <summary>
		/// [GET / SET] Whether this is considered a monospace Font. If true, each character occupies exactly the same space.
		/// </summary>
		public bool MonoSpace
		{
			get { return this.monospace; }
			set { this.monospace = value; this.needsReload = true; }
		}
		/// <summary>
		/// [GET / SET] Whether this Font uses kerning, a technique where characters are moved closer together based on their actual shape,
		/// which usually looks much nicer. It has no visual effect when active at the same time with <see cref="MonoSpace"/>, however
		/// kerning sample data will be available on glyphs.
		/// </summary>
		/// <seealso cref="GlyphData"/>
		public bool Kerning
		{
			get { return this.kerning; }
			set { this.kerning = value; this.needsReload = true; }
		}
		/// <summary>
		/// [GET] Returns whether this Font needs a <see cref="ReloadData">reload</see> in order to apply
		/// changes that have been made to its Properties.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public bool NeedsReload
		{
			get { return this.needsReload; }
		}
		
		/// <summary>
		/// [GET] Returns a chunk of memory that contains this Fonts custom family data.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public byte[] CustomFamilyData
		{
			get { return this.customFamilyData; }
		}

		/// <summary>
		/// [GET] Returns whether this Font is (requesting to be) aligned to the pixel grid.
		/// </summary>
		public bool IsPixelGridAligned
		{
			get
			{ 
				return 
					this.renderMode == RenderMode.MonochromeBitmap || 
					this.renderMode == RenderMode.GrayscaleBitmap;
			}
		}
		/// <summary>
		/// [GET] The Fonts height.
		/// </summary>
		public int Height
		{
			get { return this.height; }
		}
		/// <summary>
		/// [GET] The y offset in pixels between two lines.
		/// </summary>
		public int LineSpacing
		{
			get { return MathF.RoundToInt(this.height * this.lineHeightFactor); }
		}
		/// <summary>
		/// [GET] The Fonts ascent value.
		/// </summary>
		public int Ascent
		{
			get { return this.ascent; }
		}
		/// <summary>
		/// [GET] The Fonts body ascent value.
		/// </summary>
		public int BodyAscent
		{
			get { return this.bodyAscent; }
		}
		/// <summary>
		/// [GET] The Fonts descent value.
		/// </summary>
		public int Descent
		{
			get { return this.descent; }
		}
		/// <summary>
		/// [GET] The Fonts base line height.
		/// </summary>
		public int BaseLine
		{
			get { return this.baseLine; }
		}


		/// <summary>
		/// Creates a new Font based on a system font.
		/// </summary>
		/// <param name="familyName">The font family to use.</param>
		/// <param name="emSize">The Fonts <see cref="Size"/>.</param>
		/// <param name="style">The Fonts style.</param>
		public Font(string familyName, float emSize, FontStyle style) 
		{
			this.familyName = familyName;
			this.size = emSize;
			this.style = style;
			this.ReloadData();
		}
		/// <summary>
		/// Creates a new, empty Font.
		/// </summary>
		public Font()
		{
			this.ReloadData();
		}
		
		/// <summary>
		/// Replaces the Fonts custom font family with a new dataset that has been retrieved from file.
		/// </summary>
		/// <param name="path">The path of the file from which to retrieve the new font family data.</param>
		public void LoadCustomFamilyData(string path = null)
		{
			if (path == null) path = this.sourcePath;

			this.sourcePath = path;

			if (String.IsNullOrEmpty(this.sourcePath) || !File.Exists(this.sourcePath))
				this.sourcePath = null;
			else
			{
				this.customFamilyData = File.ReadAllBytes(this.sourcePath);
				this.familyName = LoadFontFamilyFromMemory(this.customFamilyData).Name;
			}
		}
		/// <summary>
		/// Saves the Fonts custom font family to file.
		/// </summary>
		/// <param name="path">The path of the file to which to save the font family data.</param>
		public void SaveCustomFamilyData(string path = null)
		{
			if (this.customFamilyData == null) throw new InvalidOperationException("There is no custom family data defined that could be saved.");
			if (path == null) path = this.sourcePath;

			// We're saving this Pixmaps pixel data for the first time
			if (!this.path.Contains(':') && this.sourcePath == null) this.sourcePath = path;

			File.WriteAllBytes(path, this.customFamilyData);
		}

		/// <summary>
		/// Reloads this Fonts internal data and rasterizes its glyphs.
		/// </summary>
		public void ReloadData()
		{
			this.ReleaseResources();
			this.UpdateInternalFont();

			this.needsReload = false;
			this.maxGlyphWidth = 0;
			this.height = 0;
			this.ascent = 0;
			this.bodyAscent = 0;
			this.descent = 0;
			this.baseLine = 0;
			this.glyphs = new GlyphData[SupportedChars.Length];

			this.GenerateResources();

			// Monospace offset adjustments
			if (this.monospace)
			{
				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					this.glyphs[i].offsetX -= (int)Math.Round((this.maxGlyphWidth - this.glyphs[i].width) / 2.0f);
				}
			}

			// Kerning data
			this.UpdateKerningData();
		}
		/// <summary>
		/// Updates this Fonts kerning sample data.
		/// </summary>
		public void UpdateKerningData()
		{
			if (this.kerning)
			{
				int kerningSamples = (this.Ascent + this.Descent) / 4;
				int[] kerningY;
				if (kerningSamples <= 6)
				{
					kerningSamples = 6;
					kerningY = new int[] {
						this.BaseLine - this.Ascent,
						this.BaseLine - this.BodyAscent,
						this.BaseLine - this.BodyAscent * 2 / 3,
						this.BaseLine - this.BodyAscent / 3,
						this.BaseLine,
						this.BaseLine + this.Descent};
				}
				else
				{
					kerningY = new int[kerningSamples];
					int bodySamples = kerningSamples * 2 / 3;
					int descentSamples = (kerningSamples - bodySamples) / 2;
					int ascentSamples = kerningSamples - bodySamples - descentSamples;

					for (int k = 0; k < ascentSamples; k++) 
						kerningY[k] = this.BaseLine - this.Ascent + k * (this.Ascent - this.BodyAscent) / ascentSamples;
					for (int k = 0; k < bodySamples; k++) 
						kerningY[ascentSamples + k] = this.BaseLine - this.BodyAscent + k * this.BodyAscent / (bodySamples - 1);
					for (int k = 0; k < descentSamples; k++) 
						kerningY[ascentSamples + bodySamples + k] = this.BaseLine + (k + 1) * this.Descent / descentSamples;
				}

				int[] c = new int[3];
				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					Pixmap.Layer glyphTemp = this.GetGlyphBitmap(SupportedChars[i]);

					this.glyphs[i].kerningSamplesLeft	= new int[kerningY.Length];
					this.glyphs[i].kerningSamplesRight	= new int[kerningY.Length];

					if (SupportedChars[i] != ' ')
					{
						int pxIndex;
						// Left side samples
						for (int sampleIndex = 0; sampleIndex < this.glyphs[i].kerningSamplesLeft.Length; sampleIndex++)
						{
							this.glyphs[i].kerningSamplesLeft[sampleIndex] = glyphTemp.Width / 2;
							for (int off = 0; off <= 2; off++)
							{
								pxIndex = MathF.Clamp(kerningY[sampleIndex] + off - 1, 0, glyphTemp.Height - 1) * glyphTemp.Width;
								c[off] = 0;
								while (glyphTemp.Data[pxIndex + c[off]].A == 0)
								{
									c[off]++;
									if (c[off] >= glyphTemp.Width / 2) break;
								}
								this.glyphs[i].kerningSamplesLeft[sampleIndex] = Math.Min(this.glyphs[i].kerningSamplesLeft[sampleIndex], c[off]);
							}
						}
						// Right side samples
						for (int sampleIndex = 0; sampleIndex < this.glyphs[i].kerningSamplesRight.Length; sampleIndex++)
						{
							this.glyphs[i].kerningSamplesRight[sampleIndex] = glyphTemp.Width / 2;
							for (int off = 0; off <= 2; off++)
							{
								pxIndex = MathF.Clamp(kerningY[sampleIndex] + off - 1, 0, glyphTemp.Height - 1) * glyphTemp.Width + glyphTemp.Width - 1;
								c[off] = 0;
								while (glyphTemp.Data[pxIndex - c[off]].A == 0)
								{
									c[off]++;
									if (c[off] >= glyphTemp.Width / 2) break;
								}
								this.glyphs[i].kerningSamplesRight[sampleIndex] = Math.Min(this.glyphs[i].kerningSamplesRight[sampleIndex], c[off]);
							}
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					this.glyphs[i].kerningSamplesLeft	= null;
					this.glyphs[i].kerningSamplesRight	= null;
				}
			}
		}
		private void UpdateInternalFont()
		{
			if (this.internalFont != null) this.internalFont.Dispose();
			this.internalFont = null;

			FontFamily family = GetFontFamily(this.familyName);
			if (family != null)
			{
				try
				{
					this.internalFont = new SysDrawFont(family, this.size, this.style);
				}
				catch (Exception)
				{
					this.internalFont = new SysDrawFont(FontFamily.GenericMonospace, this.size, this.style);
				}
			}
			else
			{
				this.internalFont = new SysDrawFont(FontFamily.GenericMonospace, this.size, this.style);
			}
		}
		private void ReleaseResources()
		{
			if (this.mat != null) this.mat.Dispose();
			if (this.texture != null) this.texture.Dispose();
			if (this.pixelData != null) this.pixelData.Dispose();

			this.mat = null;
			this.texture = null;
			this.pixelData = null;

			this.needsReload = true;
		}
		private void GenerateResources()
		{
			if (this.mat != null || this.texture != null || this.pixelData != null)
				this.ReleaseResources();

			TextRenderingHint textRenderingHint;
			if (this.renderMode == RenderMode.MonochromeBitmap)
				textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			else
				textRenderingHint = TextRenderingHint.AntiAliasGridFit;

			int cols;
			int rows;
			cols = rows = (int)Math.Ceiling(Math.Sqrt(SupportedChars.Length));

			Pixmap.Layer pixelLayer = new Pixmap.Layer(MathF.RoundToInt(cols * this.internalFont.Size * 1.2f), MathF.RoundToInt(rows * this.internalFont.Height * 1.2f));
			Pixmap.Layer glyphTemp;
			Pixmap.Layer glyphTempTypo;
			Bitmap bm;
			Bitmap measureBm = new Bitmap(1, 1);
			Rect[] atlas = new Rect[SupportedChars.Length];
			using (Graphics measureGraphics = Graphics.FromImage(measureBm))
			{
				Brush fntBrush = new SolidBrush(Color.Black);

				StringFormat formatDef = StringFormat.GenericDefault;
				formatDef.LineAlignment = StringAlignment.Near;
				formatDef.FormatFlags = 0;
				StringFormat formatTypo = StringFormat.GenericTypographic;
				formatTypo.LineAlignment = StringAlignment.Near;

				int x = 1;
				int y = 1;
				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					string str = SupportedChars[i].ToString(CultureInfo.InvariantCulture);
					bool isSpace = str == " ";
					SizeF charSize = measureGraphics.MeasureString(str, this.internalFont, pixelLayer.Width, formatDef);

					// Rasterize a single glyph for rendering
					bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), this.internalFont.Height + 1);
					using (Graphics glyphGraphics = Graphics.FromImage(bm))
					{
						glyphGraphics.Clear(Color.Transparent);
						glyphGraphics.TextRenderingHint = textRenderingHint;
						glyphGraphics.DrawString(str, this.internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatDef);
					}
					glyphTemp = new Pixmap.Layer(bm);
					
					// Rasterize a single glyph in typographic mode for metric analysis
					if (!isSpace)
					{
						Rectangle glyphTempBounds = glyphTemp.OpaqueBounds();
						glyphTemp.SubImage(glyphTempBounds.X, 0, glyphTempBounds.Width, glyphTemp.Height);
						if (BodyAscentRef.Contains(SupportedChars[i]))
							this.bodyAscent += glyphTempBounds.Height;
						
						bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), this.internalFont.Height + 1);
						using (Graphics glyphGraphics = Graphics.FromImage(bm))
						{
							glyphGraphics.Clear(Color.Transparent);
							glyphGraphics.TextRenderingHint = textRenderingHint;
							glyphGraphics.DrawString(str, this.internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatTypo);
						}
						glyphTempTypo = new Pixmap.Layer(bm);
						glyphTempTypo.Crop(true, false);
					}
					else
					{
						glyphTempTypo = glyphTemp;
					}

					// Update xy values if it doesn't fit anymore
					if (x + glyphTemp.Width + 2 > pixelLayer.Width)
					{
						x = 1;
						y += this.internalFont.Height + MathF.Clamp((int)MathF.Ceiling(this.internalFont.Height * 0.1875f), 3, 10);
					}
					
					// Memorize atlas coordinates & glyph data
					this.maxGlyphWidth = Math.Max(this.maxGlyphWidth, glyphTemp.Width);
					this.glyphs[i].width = glyphTemp.Width;
					this.glyphs[i].height = glyphTemp.Height;
					this.glyphs[i].offsetX = glyphTemp.Width - glyphTempTypo.Width;
					if (isSpace)
					{
						this.glyphs[i].width /= 2;
						this.glyphs[i].offsetX /= 2;
					}
					atlas[i].X = x;
					atlas[i].Y = y;
					atlas[i].W = glyphTemp.Width;
					atlas[i].H = (this.internalFont.Height + 1);

					// Draw it onto the font surface
					glyphTemp.DrawOnto(pixelLayer, BlendMode.Solid, x, y);

					x += glyphTemp.Width + MathF.Clamp((int)MathF.Ceiling(this.internalFont.Height * 0.125f), 2, 10);
				}
			}

			// White out texture except alpha channel.
			for (int i = 0; i < pixelLayer.Data.Length; i++)
			{
				pixelLayer.Data[i].R = 255;
				pixelLayer.Data[i].G = 255;
				pixelLayer.Data[i].B = 255;
			}

			this.height = this.internalFont.Height;
			this.ascent = (int)Math.Round(this.internalFont.FontFamily.GetCellAscent(this.internalFont.Style) * this.internalFont.Size / this.internalFont.FontFamily.GetEmHeight(this.internalFont.Style));
			this.bodyAscent /= BodyAscentRef.Length;
			this.descent = (int)Math.Round(this.internalFont.FontFamily.GetCellDescent(this.internalFont.Style) * this.internalFont.GetHeight() / this.internalFont.FontFamily.GetLineSpacing(this.internalFont.Style));
			this.baseLine = (int)Math.Round(this.internalFont.FontFamily.GetCellAscent(this.internalFont.Style) * this.internalFont.GetHeight() / this.internalFont.FontFamily.GetLineSpacing(this.internalFont.Style));

			this.pixelData = new Pixmap(pixelLayer);
			this.pixelData.Atlas = new List<Rect>(atlas);
			this.texture = new Texture(this.pixelData, 
				Texture.SizeMode.Enlarge, 
				this.IsPixelGridAligned ? TextureMagFilter.Nearest : TextureMagFilter.Linear,
				this.IsPixelGridAligned ? TextureMinFilter.Nearest : TextureMinFilter.LinearMipmapLinear);

			ContentRef<DrawTechnique> technique;
			if (this.renderMode == RenderMode.MonochromeBitmap)
				technique = DrawTechnique.Mask;
			else if (this.renderMode == RenderMode.GrayscaleBitmap)
				technique = DrawTechnique.Alpha;
			else if (this.renderMode == RenderMode.SmoothBitmap)
				technique = DrawTechnique.Alpha;
			else
				technique = DrawTechnique.SharpMask;

			this.mat = new Material(technique, ColorRgba.White, this.texture);
		}

		/// <summary>
		/// Retrieves information about a single glyph.
		/// </summary>
		/// <param name="glyph">The glyph to retrieve information about.</param>
		/// <param name="data">A struct holding the retrieved information.</param>
		/// <returns>True, if successful, false if not. This is the case if the specified glyph is not supported.</returns>
		public bool GetGlyphData(char glyph, out GlyphData data)
		{
			int glyphId = (int)glyph;
			if (glyphId >= CharLookup.Length)
			{
				data = new GlyphData();
				return false;
			}
			else
			{
				data = this.glyphs[CharLookup[glyphId]];
				return true;
			}
		}
		/// <summary>
		/// Retrieves the rasterized <see cref="System.Drawing.Bitmap"/> for a single glyph.
		/// </summary>
		/// <param name="glyph">The glyph of which to retrieve the Bitmap.</param>
		/// <returns>The Bitmap that has been retrieved, or null if the glyph is not supported.</returns>
		public Pixmap.Layer GetGlyphBitmap(char glyph)
		{
			Rect targetRect;
			this.pixelData.LookupAtlas(CharLookup[(int)glyph], out targetRect);
			Pixmap.Layer subImg = new Pixmap.Layer(
				MathF.RoundToInt(targetRect.W), 
				MathF.RoundToInt(targetRect.H));
			this.pixelData.MainLayer.DrawOnto(subImg, BlendMode.Solid, 
				-MathF.RoundToInt(targetRect.X), 
				-MathF.RoundToInt(targetRect.Y));
			return subImg;
		}

		/// <summary>
		/// Emits a set of vertices based on a text. To render this text, simply use that set of vertices combined with
		/// the Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="vertices">The set of vertices that is emitted. You can re-use the same array each frame.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="z">An Z-Offset applied to the position of each emitted vertex.</param>
		/// <returns>The number of emitted vertices. This values isn't necessarily equal to the emitted arrays length.</returns>
		public int EmitTextVertices(string text, ref VertexC1P3T2[] vertices, float x, float y, float z = 0.0f)
		{
			return this.EmitTextVertices(text, ref vertices, x, y, z, ColorRgba.White);
		}
		/// <summary>
		/// Emits a set of vertices based on a text. To render this text, simply use that set of vertices combined with
		/// the Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="vertices">The set of vertices that is emitted. You can re-use the same array each frame.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="z">An Z-Offset applied to the position of each emitted vertex.</param>
		/// <param name="clr">The color value that is applied to each emitted vertex.</param>
		/// <param name="angle">An angle by which the text is rotated (before applying the offset).</param>
		/// <param name="scale">A factor by which the text is scaled (before applying the offset).</param>
		/// <returns>The number of emitted vertices. This values isn't necessarily equal to the emitted arrays length.</returns>
		public int EmitTextVertices(string text, ref VertexC1P3T2[] vertices, float x, float y, float z, ColorRgba clr, float angle = 0.0f, float scale = 1.0f)
		{
			int len = this.EmitTextVertices(text, ref vertices);
			
			Vector3 offset = new Vector3(x, y, z);
			Vector2 xDot, yDot;
			MathF.GetTransformDotVec(angle, scale, out xDot, out yDot);

			for (int i = 0; i < len; i++)
			{
				MathF.TransformDotVec(ref vertices[i].Pos, ref xDot, ref yDot);
				Vector3.Add(ref vertices[i].Pos, ref offset, out vertices[i].Pos);
				vertices[i].Color = clr;
			}

			return len;
		}
		/// <summary>
		/// Emits a set of vertices based on a text. To render this text, simply use that set of vertices combined with
		/// the Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="vertices">The set of vertices that is emitted. You can re-use the same array each frame.</param>
		/// <param name="x">An X-Offset applied to the position of each emitted vertex.</param>
		/// <param name="y">An Y-Offset applied to the position of each emitted vertex.</param>
		/// <param name="clr">The color value that is applied to each emitted vertex.</param>
		/// <returns>The number of emitted vertices. This values isn't necessarily equal to the emitted arrays length.</returns>
		public int EmitTextVertices(string text, ref VertexC1P3T2[] vertices, float x, float y, ColorRgba clr)
		{
			int len = this.EmitTextVertices(text, ref vertices);
			
			Vector3 offset = new Vector3(x, y, 0);

			for (int i = 0; i < len; i++)
			{
				Vector3.Add(ref vertices[i].Pos, ref offset, out vertices[i].Pos);
				vertices[i].Color = clr;
			}

			return len;
		}
		/// <summary>
		/// Emits a set of vertices based on a text. To render this text, simply use that set of vertices combined with
		/// the Fonts <see cref="Material"/>.
		/// </summary>
		/// <param name="text">The text to render.</param>
		/// <param name="vertices">The set of vertices that is emitted. You can re-use the same array each frame.</param>
		/// <returns>The number of emitted vertices. This values isn't necessarily equal to the emitted arrays length.</returns>
		public int EmitTextVertices(string text, ref VertexC1P3T2[] vertices)
		{
			int len = text.Length * 4;
			if (vertices == null || vertices.Length < len) vertices = new VertexC1P3T2[len];
			
			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				Vector2 glyphPos;
				glyphPos.X = MathF.Round(curOffset + glyphXOff);
				glyphPos.Y = MathF.Round(0.0f);

				vertices[i * 4 + 0].Pos.X = glyphPos.X;
				vertices[i * 4 + 0].Pos.Y = glyphPos.Y;
				vertices[i * 4 + 0].Pos.Z = 0.0f;
				vertices[i * 4 + 0].TexCoord = uvRect.TopLeft;
				vertices[i * 4 + 0].Color = ColorRgba.White;

				vertices[i * 4 + 1].Pos.X = glyphPos.X + glyphData.width;
				vertices[i * 4 + 1].Pos.Y = glyphPos.Y;
				vertices[i * 4 + 1].Pos.Z = 0.0f;
				vertices[i * 4 + 1].TexCoord = uvRect.TopRight;
				vertices[i * 4 + 1].Color = ColorRgba.White;

				vertices[i * 4 + 2].Pos.X = glyphPos.X + glyphData.width;
				vertices[i * 4 + 2].Pos.Y = glyphPos.Y + glyphData.height;
				vertices[i * 4 + 2].Pos.Z = 0.0f;
				vertices[i * 4 + 2].TexCoord = uvRect.BottomRight;
				vertices[i * 4 + 2].Color = ColorRgba.White;

				vertices[i * 4 + 3].Pos.X = glyphPos.X;
				vertices[i * 4 + 3].Pos.Y = glyphPos.Y + glyphData.height;
				vertices[i * 4 + 3].Pos.Z = 0.0f;
				vertices[i * 4 + 3].TexCoord = uvRect.BottomLeft;
				vertices[i * 4 + 3].Color = ColorRgba.White;

				curOffset += glyphXAdv;
			}

			return len;
		}
		
		/// <summary>
		/// Renders a text to the specified target Image.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void RenderToBitmap(string text, Image target, float x = 0.0f, float y = 0.0f)
		{
			this.RenderToBitmap(text, target, x, y, ColorRgba.White);
		}
		/// <summary>
		/// Renders a text to the specified target Image.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="clr"></param>
		public void RenderToBitmap(string text, Image target, float x, float y, ColorRgba clr)
		{
			Bitmap pixelData = this.pixelData.MainLayer.ToBitmap();
			using (Graphics g = Graphics.FromImage(target))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				float curOffset = 0.0f;
				GlyphData glyphData;
				Rect uvRect;
				float glyphXOff;
				float glyphXAdv;
				var attrib = new System.Drawing.Imaging.ImageAttributes();
				attrib.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(new[] {
					new[] {clr.R / 255.0f,					0.0f, 			0.0f, 			0.0f, 0.0f},
					new[] {0.0f,			clr.G / 255.0f, 0.0f, 			0.0f, 			0.0f, 0.0f},
					new[] {0.0f,			0.0f, 			clr.B / 255.0f, 0.0f, 			0.0f, 0.0f},
					new[] {0.0f, 			0.0f, 			0.0f, 			clr.A / 255.0f, 0.0f, 0.0f},
					new[] {0.0f, 			0.0f, 			0.0f, 			0.0f, 			0.0f, 0.0f},
					new[] {0.0f, 			0.0f, 			0.0f, 			0.0f, 			0.0f, 0.0f} }));
				for (int i = 0; i < text.Length; i++)
				{
					this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);
					Vector2 dataCoord = uvRect.Pos * new Vector2(this.pixelData.Width, this.pixelData.Height) / this.texture.UVRatio;

					if (clr == ColorRgba.White)
					{
						g.DrawImage(pixelData,
							new Rectangle(MathF.RoundToInt(x + curOffset + glyphXOff), MathF.RoundToInt(y), glyphData.width, glyphData.height),
							new Rectangle(MathF.RoundToInt(dataCoord.X), MathF.RoundToInt(dataCoord.Y), glyphData.width, glyphData.height),
							GraphicsUnit.Pixel);
					}
					else
					{
						g.DrawImage(pixelData,
							new Rectangle(MathF.RoundToInt(x + curOffset + glyphXOff), MathF.RoundToInt(y), glyphData.width, glyphData.height),
							dataCoord.X, dataCoord.Y, glyphData.width, glyphData.height,
							GraphicsUnit.Pixel,
							attrib);
					}

					curOffset += glyphXAdv;
				}
			}
		}
		/// <summary>
		/// Renders a text to the specified target <see cref="Duality.Resources.Pixmap"/> <see cref="Duality.Resources.Pixmap.Layer"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void RenderToBitmap(string text, Pixmap.Layer target, float x = 0.0f, float y = 0.0f)
		{
			this.RenderToBitmap(text, target, x, y, ColorRgba.White);
		}
		/// <summary>
		/// Renders a text to the specified target <see cref="Duality.Resources.Pixmap"/> <see cref="Duality.Resources.Pixmap.Layer"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="clr"></param>
		public void RenderToBitmap(string text, Pixmap.Layer target, float x, float y, ColorRgba clr)
		{
			Pixmap.Layer pixelData = this.pixelData.MainLayer;

			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);
				Vector2 dataCoord = uvRect.Pos * new Vector2(this.pixelData.Width, this.pixelData.Height) / this.texture.UVRatio;
				
				pixelData.DrawOnto(target, 
					BlendMode.Alpha, 
					MathF.RoundToInt(x + curOffset + glyphXOff), 
					MathF.RoundToInt(y),
					glyphData.width, 
					glyphData.height,
					MathF.RoundToInt(dataCoord.X), 
					MathF.RoundToInt(dataCoord.Y), 
					clr);

				curOffset += glyphXAdv;
			}
		}

		/// <summary>
		/// Measures the size of a text rendered using this Font.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <returns>The size of the measured text.</returns>
		public Vector2 MeasureText(string text)
		{
			Vector2 textSize = Vector2.Zero;

			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				textSize.X = Math.Max(textSize.X, curOffset + glyphXAdv - this.spacing);
				textSize.Y = Math.Max(textSize.Y, glyphData.height);

				curOffset += glyphXAdv;
			}

			textSize.X = MathF.Round(textSize.X);
			textSize.Y = MathF.Round(textSize.Y);
			return textSize;
		}
		/// <summary>
		/// Measures the size of a multiline text rendered using this Font.
		/// </summary>
		/// <param name="text">The text to measure.</param>
		/// <returns>The size of the measured text.</returns>
		public Vector2 MeasureText(string[] text)
		{
			Vector2 textSize = Vector2.Zero;
			if (text == null) return textSize;

			for (int i = 0; i < text.Length; i++)
			{
				Vector2 lineSize = this.MeasureText(text[i]);
				textSize.X = MathF.Max(textSize.X, lineSize.X);
				textSize.Y += i == 0 ? this.Height : this.LineSpacing;
			}

			return textSize;
		}
		/// <summary>
		/// Returns a text that is cropped to fit a maximum width using this Font.
		/// </summary>
		/// <param name="text">The original text.</param>
		/// <param name="maxWidth">The maximum width it may occupy.</param>
		/// <param name="fitMode">The mode by which the text fitting algorithm operates.</param>
		/// <returns></returns>
		public string FitText(string text, float maxWidth, FitTextMode fitMode = FitTextMode.ByChar)
		{
			Vector2 textSize = Vector2.Zero;

			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			int lastValidLength = 0;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				textSize.X = Math.Max(textSize.X, curOffset + glyphXAdv);
				textSize.Y = Math.Max(textSize.Y, glyphData.height);

				if (textSize.X > maxWidth) return lastValidLength > 0 ? text.Substring(0, lastValidLength) : "";

				if (fitMode == FitTextMode.ByChar)
					lastValidLength = i;
				else if (text[i] == ' ')
					lastValidLength = fitMode == FitTextMode.ByWordLeadingSpace ? i : i + 1;

				curOffset += glyphXAdv;
			}

			return text;
		}
		/// <summary>
		/// Measures position and size of a specific glyph inside a text.
		/// </summary>
		/// <param name="text">The text that contains the glyph to measure.</param>
		/// <param name="index">The index of the glyph to measure.</param>
		/// <returns>A rectangle that describes the specified glyphs position and size.</returns>
		public Rect MeasureTextGlyph(string text, int index)
		{
			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				if (i == index) return new Rect(curOffset + glyphXOff, 0, glyphData.width, glyphData.height);

				curOffset += glyphXAdv;
			}

			return new Rect();
		}
		/// <summary>
		/// Returns the index of the glyph that is located at a certain location within a text.
		/// </summary>
		/// <param name="text">The text from which to pick a glyph.</param>
		/// <param name="x">X-Coordinate of the position where to look for a glyph.</param>
		/// <param name="y">Y-Coordinate of the position where to look for a glyph.</param>
		/// <returns>The index of the glyph that is located at the specified position.</returns>
		public int PickTextGlyph(string text, float x, float y)
		{
			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			Rect glyphRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				glyphRect = new Rect(curOffset + glyphXOff, 0, glyphData.width, glyphData.height);
				if (glyphRect.Contains(x, y)) return i;

				curOffset += glyphXAdv;
			}

			return -1;
		}

		private void ProcessTextAdv(string text, int index, out GlyphData glyphData, out Rect uvRect, out float glyphXAdv, out float glyphXOff)
		{
			char glyph = text[index];
			this.texture.LookupAtlas(CharLookup[(int)glyph], out uvRect);

			this.GetGlyphData(glyph, out glyphData);
			glyphXOff = -glyphData.offsetX;

			if (this.kerning && !this.monospace && !this.needsReload)
			{
				char glyphNext = index + 1 < text.Length ? text[index + 1] : ' ';
				GlyphData glyphDataNext;
				this.GetGlyphData(glyphNext, out glyphDataNext);

				int minSum = int.MaxValue;
				for (int k = 0; k < glyphData.kerningSamplesRight.Length; k++)
					minSum = Math.Min(minSum, glyphData.kerningSamplesRight[k] + glyphDataNext.kerningSamplesLeft[k]);

				glyphXAdv = (this.monospace ? this.maxGlyphWidth : -glyphData.offsetX + glyphData.width) + this.spacing - minSum;
			}
			else
				glyphXAdv = (this.monospace ? this.maxGlyphWidth : -glyphData.offsetX + glyphData.width) + this.spacing;
		}

		protected override void OnLoaded()
		{
			// Load custom font, if not available yet
			if (GetFontFamily(this.familyName) == null && this.customFamilyData != null)
				LoadFontFamilyFromMemory(this.customFamilyData);

			this.ReloadData();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.ReleaseResources();
		}
		protected override void OnCopyTo(Resource r, Duality.Cloning.CloneProvider provider)
		{
			base.OnCopyTo(r, provider);
			Font c = r as Font;
			c.customFamilyData = this.customFamilyData != null ? (byte[])this.customFamilyData.Clone() : null;
			c.familyName = this.familyName;
			c.size = this.size;
			c.style = this.style;
			c.renderMode = this.renderMode;
			c.monospace = this.monospace;
			c.kerning = this.kerning;
			c.spacing = this.spacing;
			c.ReloadData();
		}

		/// <summary>
		/// Retrieves a <see cref="System.Drawing.FontFamily"/> by its name.
		/// </summary>
		/// <param name="name">The name of the FontFamily.</param>
		/// <returns>The FontFamily that has been retrieved, or null if no matching family was found.</returns>
		public static FontFamily GetFontFamily(string name)
		{
			if (string.IsNullOrEmpty(name)) return null;

			FontFamily result;
			if (!loadedFontRegistry.TryGetValue(name, out result))
			{
				foreach (FontFamily installedFamily in FontFamily.Families)
				{
					if (installedFamily.Name == name) return installedFamily;
				}
			}
			return result;
		}
		/// <summary>
		/// Loads a <see cref="System.Drawing.FontFamily"/> from file.
		/// </summary>
		/// <param name="file">The file to load the FontFamily from.</param>
		/// <returns>The FontFamily that has been loaded.</returns>
		public static FontFamily LoadFontFamilyFromFile(string file)
		{
			fontManager.AddFontFile(file);
			FontFamily result = fontManager.Families[fontManager.Families.Length - 1];
			loadedFontRegistry[result.Name] = result;
			return result;
		}
		/// <summary>
		/// Loads a <see cref="System.Drawing.FontFamily"/> from memory.
		/// </summary>
		/// <param name="memory">The memory chunk to load the FontFamily from.</param>
		/// <returns>The FontFamily that has been loaded.</returns>
		public static FontFamily LoadFontFamilyFromMemory(byte[] memory)
		{
			FontFamily result = null;

			GCHandle handle = GCHandle.Alloc(memory, GCHandleType.Pinned);
			try
			{
				IntPtr fontMemPtr = handle.AddrOfPinnedObject();
				fontManager.AddMemoryFont(fontMemPtr, memory.Length);
				result = fontManager.Families[fontManager.Families.Length - 1];
			}
			finally
			{
				handle.Free();
			}
			
			loadedFontRegistry[result.Name] = result;
			return result;
		}
		/// <summary>
		/// Loads a <see cref="System.Drawing.FontFamily"/> from stream.
		/// </summary>
		/// <param name="stream">The stream to load the FontFamily from.</param>
		/// <returns>The FontFamily that has been loaded.</returns>
		public static FontFamily LoadFontFamilyFromStream(Stream stream)
		{
			byte[] buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			return LoadFontFamilyFromMemory(buffer);
		}

		/* Distance Field Equations
        private static Pixmap.Layer CalcDistField(Pixmap.Layer input, int scaleDownFactor, float spread)
        {
            Pixmap.Layer output = new Pixmap.Layer(MathF.Max(input.Width / scaleDownFactor, 1), MathF.Max(input.Height / scaleDownFactor, 1));
            int targetWidth = output.Width;
            int targetHeight = output.Height;
            float sourceSpread = spread * scaleDownFactor;

            for (int y = 0; y < targetHeight; ++y)
			{
                for (int x = 0; x < targetWidth; ++x)
                {
					int i = y * targetWidth + x;
                    float signedDistance = CalcDistFieldValue(input, x * scaleDownFactor + scaleDownFactor / 2, y * scaleDownFactor + scaleDownFactor / 2, sourceSpread);
					output.Data[i].R = input.Data[i].R;
					output.Data[i].G = input.Data[i].G;
					output.Data[i].B = input.Data[i].B;
                    output.Data[i].A = (byte)((signedDistance + sourceSpread) * 255.0f / (sourceSpread * 2));
                }
            }

            return output;
        }
        private static float CalcDistFieldValue(Pixmap.Layer input, int inputX, int inputY, float sourceSpread)
        {
            int w = input.Width;
            int h = input.Height;

            float inputVal = (input.Data[inputY * w + inputX].A / 255.0f) - 0.5f;

            int minX = MathF.Max(inputX - (int)sourceSpread - 1, 0);
            int maxX = MathF.Min(inputX + (int)sourceSpread + 1, w - 1);

            float distance = sourceSpread;
            for (int y = 0; y <= (int)sourceSpread + 1; ++y)
			{
                if (y > distance) continue;

                if (inputY - y >= 0)
				{
                    int y1 = inputY - y;
                    for (int x = minX; x <= maxX; ++x)
                    {
                        if (x - inputX > distance) continue;
                        float val = (input.Data[y1 * w + x].A / 255.0f) - 0.5f;
                        if (inputVal * val < 0)
						{
                            float distQuad = (y1 - inputY) * (y1 - inputY) + (x - inputX) * (x - inputX);
                            if (distQuad < distance * distance) distance = MathF.Sqrt(distQuad);
                        }
                    }
                }

                if (y != 0 && inputY + y < h)
				{
                    int y2 = inputY + y;
                    for (int x = minX; x <= maxX; ++x)
                    {
                        if (x - inputX > distance) continue;
                        float val = (input.Data[y2 * w + x].A / 255.0f) - 0.5f;
                        if (inputVal * val < 0)
						{
                            float distQuad = (y2 - inputY) * (y2 - inputY) + (x - inputX) * (x - inputX);
                            if (distQuad < distance * distance) distance = MathF.Sqrt(distQuad);
                        }
                    }
                }
            }

			return inputVal > 0 ? distance : -distance;
        }*/
	}
}
