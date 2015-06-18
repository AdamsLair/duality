using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using SysDrawFont = System.Drawing.Font;
using SysDrawFontStyle = System.Drawing.FontStyle;
using FontStyle = Duality.Drawing.FontStyle;

using Duality.Drawing;
using Duality.Editor;
using Duality.Cloning;
using Duality.Properties;

namespace Duality.Resources
{
	/// <summary>
	/// Represents a font. While any system font or imported TrueType font can be used, they are internally
	/// pre-rasterized and stored in a <see cref="Duality.Resources.Texture"/> with an <see cref="Duality.Resources.Pixmap.Atlas"/>.
	/// </summary>
	[ExplicitResourceReference()]
	[EditorHintCategory(CoreResNames.CategoryGraphics)]
	[EditorHintImage(CoreResNames.ImageFont)]
	public class Font : Resource
	{
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

		internal static void InitDefaultContent()
		{
			Assembly embeddingAssembly = typeof(Font).GetTypeInfo().Assembly;

			Font genericMonospace8;
			Font genericMonospace10;
			using (Stream stream = embeddingAssembly.GetManifestResourceStream("Duality.EmbeddedResources.Cousine8.Font.res"))
			{
				genericMonospace8 = Resource.Load<Font>(stream);
			}
			using (Stream stream = embeddingAssembly.GetManifestResourceStream("Duality.EmbeddedResources.Cousine10.Font.res"))
			{
				genericMonospace10 = Resource.Load<Font>(stream);
			}

			InitDefaultContent<Font>(new Dictionary<string,Font>
			{
				{ "GenericMonospace8", genericMonospace8 },
				{ "GenericMonospace10", genericMonospace10 },
			});
		}

		
		/// <summary>
		/// A string containing all characters that are supported by Duality.
		/// </summary>
		public static readonly string			SupportedChars		= "? abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,;.:-_<>|#'+*~@^°!\"§$%&/()=`²³{[]}\\´öäüÖÄÜß";
		private const string					CharBaseLineRef		= "acemnorsuvwxz";
		private const string					CharDescentRef		= "pqgjy";
		private const string					CharBodyAscentRef	= "acemnorsuvwxz";
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
			public int Width;
			/// <summary>
			/// The height of the glyph
			/// </summary>
			public int Height;
			/// <summary>
			/// The glyphs X offset when rendering it.
			/// </summary>
			public int OffsetX;
			/// <summary>
			/// The glyphs kerning samples to the left.
			/// </summary>
			public int[] KerningSamplesLeft;
			/// <summary>
			/// The glyphs kerning samples to the right.
			/// </summary>
			public int[] KerningSamplesRight;
		}

		
		private	float		size				= 8.0f;
		private	FontStyle	style				= FontStyle.Regular;
		private	RenderMode	renderMode			= RenderMode.SharpBitmap;
		private	float		spacing				= 0.0f;
		private	float		lineHeightFactor	= 1.0f;
		private	bool		monospace			= true;
		private	bool		kerning				= false;
		private	GlyphData[]	glyphs				= null;
		private	Pixmap		pixelData			= null;
		private	int			maxGlyphWidth		= 0;
		private	int			height				= 0;
		private	int			ascent				= 0;
		private	int			bodyAscent			= 0;
		private	int			descent				= 0;
		private	int			baseLine			= 0;
		// Embedded custom font family
		private	byte[]		embeddedFont		= null;
		// Data that is automatically acquired while loading the font
		[DontSerialize] private SysDrawFont	internalFont	= null;
		[DontSerialize] private string		familyName		= null;
		[DontSerialize] private	Material	material		= null;
		[DontSerialize] private	Texture		texture			= null;
		[DontSerialize] private	bool		needsReload		= true;


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
		[EditorHintFlags(MemberFlags.AffectsOthers)]
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
			get { return this.material; }
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
		/// [GET] Returns a chunk of memory that contains this Fonts embedded TrueType data for rendering glyphs.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public byte[] EmbeddedTrueTypeFont
		{
			get { return this.embeddedFont; }
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
				this.embeddedFont = File.ReadAllBytes(this.sourcePath);
				this.familyName = LoadFontFamilyFromMemory(this.embeddedFont).Name;
			}
		}
		/// <summary>
		/// Saves the Fonts custom font family to file.
		/// </summary>
		/// <param name="path">The path of the file to which to save the font family data.</param>
		public void SaveCustomFamilyData(string path = null)
		{
			if (this.embeddedFont == null) throw new InvalidOperationException("There is no custom family data defined that could be saved.");
			if (path == null) path = this.sourcePath;

			// We're saving this Pixmaps pixel data for the first time
			if (!this.IsDefaultContent && this.sourcePath == null) this.sourcePath = path;

			File.WriteAllBytes(path, this.embeddedFont);
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
					this.glyphs[i].OffsetX -= (int)Math.Round((this.maxGlyphWidth - this.glyphs[i].Width) / 2.0f);
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

				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					PixelData glyphTemp = this.GetGlyphBitmap(SupportedChars[i]);

					this.glyphs[i].KerningSamplesLeft	= new int[kerningY.Length];
					this.glyphs[i].KerningSamplesRight	= new int[kerningY.Length];

					if (SupportedChars[i] != ' ')
					{
						// Left side samples
						{
							int[] leftData = this.glyphs[i].KerningSamplesLeft;
							int leftMid = glyphTemp.Width / 2;
							int lastSampleY = 0;
							for (int sampleIndex = 0; sampleIndex < leftData.Length; sampleIndex++)
							{
								leftData[sampleIndex] = leftMid;

								int beginY = MathF.Clamp(lastSampleY, 0, glyphTemp.Height - 1);
								int endY = MathF.Clamp(kerningY[sampleIndex], 0, glyphTemp.Height);
								if (sampleIndex == leftData.Length - 1) endY = glyphTemp.Height;
								lastSampleY = endY;

								for (int y = beginY; y < endY; y++)
								{
									int x = 0;
									while (glyphTemp[x, y].A <= 64)
									{
										x++;
										if (x >= leftMid) break;
									}
									leftData[sampleIndex] = Math.Min(leftData[sampleIndex], x);
								}
							}
						}

						// Right side samples
						{
							int[] rightData = this.glyphs[i].KerningSamplesRight;
							int rightMid = (glyphTemp.Width + 1) / 2;
							int lastSampleY = 0;
							for (int sampleIndex = 0; sampleIndex < rightData.Length; sampleIndex++)
							{
								rightData[sampleIndex] = rightMid;

								int beginY = MathF.Clamp(lastSampleY, 0, glyphTemp.Height - 1);
								int endY = MathF.Clamp(kerningY[sampleIndex], 0, glyphTemp.Height);
								if (sampleIndex == rightData.Length - 1) endY = glyphTemp.Height;
								lastSampleY = endY;

								for (int y = beginY; y < endY; y++)
								{
									int x = glyphTemp.Width - 1;
									while (glyphTemp[x, y].A <= 64)
									{
										x--;
										if (x <= rightMid) break;
									}
									rightData[sampleIndex] = Math.Min(rightData[sampleIndex], glyphTemp.Width - 1 - x);
								}
							}
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < SupportedChars.Length; ++i)
				{
					this.glyphs[i].KerningSamplesLeft	= null;
					this.glyphs[i].KerningSamplesRight	= null;
				}
			}
		}
		private void UpdateInternalFont()
		{
			if (this.internalFont != null) this.internalFont.Dispose();
			this.internalFont = null;
			
			SysDrawFontStyle style = SysDrawFontStyle.Regular;
			if (this.style.HasFlag(FontStyle.Bold)) style |= SysDrawFontStyle.Bold;
			if (this.style.HasFlag(FontStyle.Italic)) style |= SysDrawFontStyle.Italic;

			// Load custom font, if not available yet
			if (GetFontFamily(this.familyName) == null && this.embeddedFont != null)
				LoadFontFamilyFromMemory(this.embeddedFont);

			FontFamily family = GetFontFamily(this.familyName);
			if (family != null)
			{
				try
				{
					this.internalFont = new SysDrawFont(family, this.size, style);
				}
				catch (Exception)
				{
					this.internalFont = new SysDrawFont(FontFamily.GenericMonospace, this.size, style);
				}
			}
			else
			{
				this.internalFont = new SysDrawFont(FontFamily.GenericMonospace, this.size, style);
			}
		}
		private void ReleaseResources()
		{
			if (this.material != null) this.material.Dispose();
			if (this.texture != null) this.texture.Dispose();
			if (this.pixelData != null) this.pixelData.Dispose();

			this.material = null;
			this.texture = null;
			this.pixelData = null;

			this.needsReload = true;
		}
		private void GenerateResources()
		{
			if (this.material != null || this.texture != null || this.pixelData != null)
				this.ReleaseResources();

			TextRenderingHint textRenderingHint;
			if (this.renderMode == RenderMode.MonochromeBitmap)
				textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			else
				textRenderingHint = TextRenderingHint.AntiAliasGridFit;

			int cols;
			int rows;
			cols = rows = (int)Math.Ceiling(Math.Sqrt(SupportedChars.Length));

			PixelData pixelLayer = new PixelData(MathF.RoundToInt(cols * this.internalFont.Size * 1.2f), MathF.RoundToInt(rows * this.internalFont.Height * 1.2f));
			PixelData glyphTemp;
			PixelData glyphTempTypo;
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
					glyphTemp = BitmapToPixelData(bm);
					
					// Rasterize a single glyph in typographic mode for metric analysis
					if (!isSpace)
					{
						Point2 glyphTempOpaqueTopLeft;
						Point2 glyphTempOpaqueSize;
						glyphTemp.GetOpaqueBoundaries(out glyphTempOpaqueTopLeft, out glyphTempOpaqueSize);

						glyphTemp.SubImage(glyphTempOpaqueTopLeft.X, 0, glyphTempOpaqueSize.X, glyphTemp.Height);

						if (CharBodyAscentRef.Contains(SupportedChars[i]))
							this.bodyAscent += glyphTempOpaqueSize.Y;
						if (CharBaseLineRef.Contains(SupportedChars[i]))
							this.baseLine += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						if (CharDescentRef.Contains(SupportedChars[i]))
							this.descent += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						
						bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), this.internalFont.Height + 1);
						using (Graphics glyphGraphics = Graphics.FromImage(bm))
						{
							glyphGraphics.Clear(Color.Transparent);
							glyphGraphics.TextRenderingHint = textRenderingHint;
							glyphGraphics.DrawString(str, this.internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatTypo);
						}
						glyphTempTypo = BitmapToPixelData(bm);
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
					this.glyphs[i].Width = glyphTemp.Width;
					this.glyphs[i].Height = glyphTemp.Height;
					this.glyphs[i].OffsetX = glyphTemp.Width - glyphTempTypo.Width;
					if (isSpace)
					{
						this.glyphs[i].Width /= 2;
						this.glyphs[i].OffsetX /= 2;
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

			// Determine Font properties
			{
				float lineSpacing = this.internalFont.FontFamily.GetLineSpacing(this.internalFont.Style);
				float emHeight = this.internalFont.FontFamily.GetEmHeight(this.internalFont.Style);
				float cellAscent = this.internalFont.FontFamily.GetCellAscent(this.internalFont.Style);
				float cellDescent = this.internalFont.FontFamily.GetCellDescent(this.internalFont.Style);
				float height = this.internalFont.GetHeight();

				this.height = this.internalFont.Height;
				this.ascent = (int)Math.Round(cellAscent * this.internalFont.Size / emHeight);
				this.bodyAscent /= CharBodyAscentRef.Length;
				this.baseLine /= CharBaseLineRef.Length;
				this.descent = (int)Math.Round(((float)this.descent / CharDescentRef.Length) - (float)this.baseLine);
				//this.descent = (int)Math.Round(cellDescent * height / lineSpacing);
				//this.baseLine = (int)Math.Round(cellAscent * height / lineSpacing);
			}

			// Create internal Pixmap and Texture Resources
			this.pixelData = new Pixmap(pixelLayer);
			this.pixelData.Atlas = new List<Rect>(atlas);
			this.GenerateTexMat();
		}
		private void GenerateTexMat()
		{
			if (this.material != null) this.material.Dispose();
			if (this.texture != null) this.texture.Dispose();

			if (this.pixelData == null)
				return;

			this.texture = new Texture(this.pixelData, 
				TextureSizeMode.Enlarge, 
				this.IsPixelGridAligned ? TextureMagFilter.Nearest : TextureMagFilter.Linear,
				this.IsPixelGridAligned ? TextureMinFilter.Nearest : TextureMinFilter.LinearMipmapLinear);

			// Select DrawTechnique to use
			ContentRef<DrawTechnique> technique;
			if (this.renderMode == RenderMode.MonochromeBitmap)
				technique = DrawTechnique.Mask;
			else if (this.renderMode == RenderMode.GrayscaleBitmap)
				technique = DrawTechnique.Alpha;
			else if (this.renderMode == RenderMode.SmoothBitmap)
				technique = DrawTechnique.Alpha;
			else
				technique = DrawTechnique.SharpAlpha;

			// Create and configure internal BatchInfo
			BatchInfo matInfo = new BatchInfo(technique, ColorRgba.White, this.texture);
			if (technique == DrawTechnique.SharpAlpha)
			{
				matInfo.SetUniform("smoothness", this.size * 4.0f);
			}
			this.material = new Material(matInfo);
		}

		/// <summary>
		/// Retrieves information about a single glyph.
		/// </summary>
		/// <param name="glyph">The glyph to retrieve information about.</param>
		/// <param name="data">A struct holding the retrieved information.</param>
		/// <returns>True, if successful, false if the specified glyph is not supported.</returns>
		public bool GetGlyphData(char glyph, out GlyphData data)
		{
			int glyphId = (int)glyph;
			if (glyphId >= CharLookup.Length)
			{
				data = this.glyphs[0];
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
		public PixelData GetGlyphBitmap(char glyph)
		{
			Rect targetRect;
			int charIndex = (int)glyph > CharLookup.Length ? 0 : CharLookup[(int)glyph];
			this.pixelData.LookupAtlas(charIndex, out targetRect);
			PixelData subImg = new PixelData(
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
			
			if (this.texture == null)
				return len;

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

				vertices[i * 4 + 1].Pos.X = glyphPos.X + glyphData.Width;
				vertices[i * 4 + 1].Pos.Y = glyphPos.Y;
				vertices[i * 4 + 1].Pos.Z = 0.0f;
				vertices[i * 4 + 1].TexCoord = uvRect.TopRight;
				vertices[i * 4 + 1].Color = ColorRgba.White;

				vertices[i * 4 + 2].Pos.X = glyphPos.X + glyphData.Width;
				vertices[i * 4 + 2].Pos.Y = glyphPos.Y + glyphData.Height;
				vertices[i * 4 + 2].Pos.Z = 0.0f;
				vertices[i * 4 + 2].TexCoord = uvRect.BottomRight;
				vertices[i * 4 + 2].Color = ColorRgba.White;

				vertices[i * 4 + 3].Pos.X = glyphPos.X;
				vertices[i * 4 + 3].Pos.Y = glyphPos.Y + glyphData.Height;
				vertices[i * 4 + 3].Pos.Z = 0.0f;
				vertices[i * 4 + 3].TexCoord = uvRect.BottomLeft;
				vertices[i * 4 + 3].Color = ColorRgba.White;

				curOffset += glyphXAdv;
			}

			return len;
		}
		
		/// <summary>
		/// Renders a text to the specified target <see cref="Duality.Resources.Pixmap"/> <see cref="Duality.Drawing.PixelData"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void RenderToBitmap(string text, PixelData target, float x = 0.0f, float y = 0.0f)
		{
			this.RenderToBitmap(text, target, x, y, ColorRgba.White);
		}
		/// <summary>
		/// Renders a text to the specified target <see cref="Duality.Drawing.PixelData"/>.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="target"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="clr"></param>
		public void RenderToBitmap(string text, PixelData target, float x, float y, ColorRgba clr)
		{
			if (this.pixelData == null)
				return;

			PixelData bitmap = this.pixelData.MainLayer;
			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);
				Vector2 dataCoord = uvRect.Pos * new Vector2(this.pixelData.Width, this.pixelData.Height) / this.texture.UVRatio;
				
				bitmap.DrawOnto(target, 
					BlendMode.Alpha, 
					MathF.RoundToInt(x + curOffset + glyphXOff), 
					MathF.RoundToInt(y),
					glyphData.Width, 
					glyphData.Height,
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
			if (this.texture == null) return Vector2.Zero;

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
				textSize.Y = Math.Max(textSize.Y, glyphData.Height);

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
			if (this.texture == null) return Vector2.Zero;

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
			if (this.texture == null) return text;

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
				textSize.Y = Math.Max(textSize.Y, glyphData.Height);

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
			if (this.texture == null) return Rect.Empty;

			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				if (i == index) return new Rect(curOffset + glyphXOff, 0, glyphData.Width, glyphData.Height);

				curOffset += glyphXAdv;
			}

			return Rect.Empty;
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
			if (this.texture == null) return -1;

			float curOffset = 0.0f;
			GlyphData glyphData;
			Rect uvRect;
			Rect glyphRect;
			float glyphXOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff);

				glyphRect = new Rect(curOffset + glyphXOff, 0, glyphData.Width, glyphData.Height);
				if (glyphRect.Contains(x, y)) return i;

				curOffset += glyphXAdv;
			}

			return -1;
		}

		private void ProcessTextAdv(string text, int index, out GlyphData glyphData, out Rect uvRect, out float glyphXAdv, out float glyphXOff)
		{
			char glyph = text[index];
			int charIndex = (int)glyph > CharLookup.Length ? 0 : CharLookup[(int)glyph];
			this.texture.LookupAtlas(charIndex, out uvRect);

			this.GetGlyphData(glyph, out glyphData);
			glyphXOff = -glyphData.OffsetX;

			if (this.kerning && !this.monospace && !this.needsReload)
			{
				char glyphNext = index + 1 < text.Length ? text[index + 1] : ' ';
				GlyphData glyphDataNext;
				this.GetGlyphData(glyphNext, out glyphDataNext);

				int minSum = int.MaxValue;
				for (int k = 0; k < glyphData.KerningSamplesRight.Length; k++)
					minSum = Math.Min(minSum, glyphData.KerningSamplesRight[k] + glyphDataNext.KerningSamplesLeft[k]);

				glyphXAdv = (this.monospace ? this.maxGlyphWidth : -glyphData.OffsetX + glyphData.Width) + this.spacing - minSum;
			}
			else
				glyphXAdv = (this.monospace ? this.maxGlyphWidth : -glyphData.OffsetX + glyphData.Width) + this.spacing;
		}

		protected override void OnLoaded()
		{
			this.GenerateTexMat();
			base.OnLoaded();
		}
		protected override void OnDisposing(bool manually)
		{
			base.OnDisposing(manually);
			this.ReleaseResources();
		}
		protected override void OnCopyDataTo(object target, ICloneOperation operation)
		{
			base.OnCopyDataTo(target, operation);
			Font c = target as Font;
			c.ReloadData();
		}

		/// <summary>
		/// Retrieves a <see cref="System.Drawing.FontFamily"/> by its name.
		/// </summary>
		/// <param name="name">The name of the FontFamily.</param>
		/// <returns>The FontFamily that has been retrieved, or null if no matching family was found.</returns>
		private static FontFamily GetFontFamily(string name)
		{
			if (string.IsNullOrEmpty(name)) return null;

			FontFamily result;
			if (!loadedFontRegistry.TryGetValue(name, out result))
			{
				foreach (FontFamily installedFamily in FontFamily.Families)
				{
					if (installedFamily.Name == name)
					{
						loadedFontRegistry[name] = installedFamily;
						return installedFamily;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Loads a <see cref="System.Drawing.FontFamily"/> from memory.
		/// </summary>
		/// <param name="memory">The memory chunk to load the FontFamily from.</param>
		/// <returns>The FontFamily that has been loaded.</returns>
		private static FontFamily LoadFontFamilyFromMemory(byte[] memory)
		{
			FontFamily result = null;
			FontFamily[] familiesBefore = fontManager.Families.ToArray();

			GCHandle handle = GCHandle.Alloc(memory, GCHandleType.Pinned);
			try
			{
				IntPtr fontMemPtr = handle.AddrOfPinnedObject();
				fontManager.AddMemoryFont(fontMemPtr, memory.Length);
				result = fontManager.Families.Except(familiesBefore).FirstOrDefault();
			}
			finally
			{
				handle.Free();
			}
			
			loadedFontRegistry[result.Name] = result;
			return result;
		}

		private static PixelData BitmapToPixelData(Bitmap bitmap)
		{
			// Retrieve data
			System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			
			int pixels = data.Width * data.Height;
			int[] argbValues = new int[pixels];
			System.Runtime.InteropServices.Marshal.Copy(data.Scan0, argbValues, 0, pixels);
			bitmap.UnlockBits(data);
			
			PixelData pixelData = new PixelData(bitmap.Width, bitmap.Height);
			ColorRgba[] rawData = pixelData.Data;
			System.Threading.Tasks.Parallel.ForEach(System.Collections.Concurrent.Partitioner.Create(0, rawData.Length), range =>
			{
				for (int i = range.Item1; i < range.Item2; i++)
				{
					rawData[i].A = (byte)((argbValues[i] & 0xFF000000) >> 24);
					rawData[i].R = (byte)((argbValues[i] & 0x00FF0000) >> 16);
					rawData[i].G = (byte)((argbValues[i] & 0x0000FF00) >> 8);
					rawData[i].B = (byte)((argbValues[i] & 0x000000FF) >> 0);
				}
			});
			pixelData.ColorTransparentPixels();

			return pixelData;
		}
	}
}
