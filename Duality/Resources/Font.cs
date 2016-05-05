using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Reflection;

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
			/// The glyph that is encoded.
			/// </summary>
			public char Glyph;
			/// <summary>
			/// The width of the glyph.
			/// </summary>
			public int Width;
			/// <summary>
			/// The height of the glyph.
			/// </summary>
			public int Height;
			/// <summary>
			/// The glyphs X offset when rendering it.
			/// </summary>
			public int OffsetX;
			/// <summary>
			/// The glyphs Y offset when rendering it.
			/// </summary>
			public int OffsetY;
			/// <summary>
			/// The glyphs kerning samples to the left.
			/// </summary>
			public int[] KerningSamplesLeft;
			/// <summary>
			/// The glyphs kerning samples to the right.
			/// </summary>
			public int[] KerningSamplesRight;

			public override string ToString()
			{
				return string.Format("Glyph '{0}', {1}x{2}, OffsetX {3}, OffsetY {4}", 
					this.Glyph, 
					this.Width, 
					this.Height, 
					this.OffsetX,
					this.OffsetY);
			}
		}

		
		private	float		size				= 16.0f;
		private	FontStyle	style				= FontStyle.Regular;
		private	RenderMode	renderMode			= RenderMode.SharpBitmap;
		private	float		spacing				= 0.0f;
		private	float		lineHeightFactor	= 1.0f;
		private	bool		monospace			= false;
		private	bool		kerning				= true;
		private	GlyphData[]	glyphs				= null;
		private	Pixmap		pixelData			= null;
		private	int			maxGlyphWidth		= 0;
		private	int			height				= 0;
		private	int			ascent				= 0;
		private	int			bodyAscent			= 0;
		private	int			descent				= 0;
		private	int			baseLine			= 0;
		private FontMetrics	metrics				= null;
		// Data that is automatically acquired while loading the font
		[DontSerialize] private int[]		charLookup		= null;
		[DontSerialize] private	Material	material		= null;
		[DontSerialize] private	Texture		texture			= null;


		/// <summary>
		/// [GET] The size of the Font.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		[Obsolete("Size information is import-only. Modify importer settings instead. For read-only access, use the Metrics property.")]
		public float Size
		{
			get { return this.size; }
			set {} // Remove this on the next breaking change cycle
		}
		/// <summary>
		/// [GET] The style of the font. 
		/// 
		/// This property is obsolete and will be removed in the next major version step.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		[Obsolete("Style information is import-only. Modify importer settings instead.")]
		public FontStyle Style
		{
			get { return this.style; }
			set {} // Remove this on the next breaking change cycle
		}
		/// <summary>
		/// [GET / SET] Specifies how the glyphs of this <see cref="Font"/> are rendered in a text.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public RenderMode GlyphRenderMode
		{
			get { return this.renderMode; }
			set
			{
				if (this.renderMode != value)
				{
					this.renderMode = value;
					this.GenerateTexture(); // Filtering depends on pixel-grid alignment.
					this.GenerateMaterial();
				}
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
		/// [GET] Whether this is considered a monospace Font. If true, each character occupies exactly the same space.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		[Obsolete("Monospace information is import-only. Modify importer settings instead. For read-only access, use the Metrics property.")]
		public bool MonoSpace
		{
			get { return this.monospace; }
			set {} // Remove this on the next breaking change cycle
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
			set { this.kerning = value; }
		}
		/// <summary>
		/// [GET] Returns whether this Font requires to re-render its glyphs in order to match the
		/// changes that have been made to its Properties.
		/// 
		/// This property is obsolete and will be removed in the next major version step.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		[Obsolete("This property is obsolete with the new Font importer and custom import parameters.")]
		public bool GlyphsDirty
		{
			get { return false; }
		}
		
		/// <summary>
		/// [GET] Returns a chunk of memory that contains this Fonts embedded TrueType data for rendering glyphs.
		/// 
		/// This property is obsolete and will be removed in the next major version step.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		[Obsolete("This property is obsolete with the new Font importer and custom import parameters.")]
		public byte[] EmbeddedTrueTypeFont
		{
			get { return null; }
			set { }
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
		/// [GET] Provides access to various metrics that are inherent to this <see cref="Font"/> instance,
		/// such as size, height, and various typographic measures.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public FontMetrics Metrics
		{
			get 
			{
				// Remove this on the next major version step.
				if (this.metrics == null)
				{
					this.metrics = new FontMetrics(
						this.size, 
						this.height, 
						this.ascent, 
						this.bodyAscent, 
						this.descent, 
						this.baseLine,
						this.monospace);
				}
				return this.metrics;
			}
		}


		/// <summary>
		/// Applies a new set of rendered glyphs to the <see cref="Font"/>, adjusts its typeface metadata and clears out the <see cref="GlyphsDirty"/> flag.
		/// This method is used by the editor to update a Font after adjusting its properties.
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="atlas"></param>
		/// <param name="glyphs"></param>
		/// <param name="metrics"></param>
		public void SetGlyphData(PixelData bitmap, Rect[] atlas, GlyphData[] glyphs, FontMetrics metrics)
		{
			this.ReleaseResources();

			this.glyphs = glyphs;
			this.GenerateCharLookup();

			this.pixelData = new Pixmap(bitmap);
			this.pixelData.Atlas = atlas.ToList();

			this.metrics = metrics;

			// Copy metrics data into local fields.
			// Remove this on the next major version step.
			this.size = metrics.Size;
			this.height = metrics.Height;
			this.ascent = metrics.Ascent;
			this.bodyAscent = metrics.BodyAscent;
			this.descent = metrics.Descent;
			this.baseLine = metrics.BaseLine;
			this.monospace = metrics.Monospace;

			this.maxGlyphWidth = 0;
			for (int i = 0; i < this.glyphs.Length; i++)
			{
				this.maxGlyphWidth = Math.Max(this.maxGlyphWidth, this.glyphs[i].Width);
			}

			this.UpdateKerningData();
			this.GenerateTexture();
			this.GenerateMaterial();
		}
		/// <summary>
		/// Updates this Fonts kerning sample data.
		/// </summary>
		private void UpdateKerningData()
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

			for (int i = 0; i < this.glyphs.Length; ++i)
			{
				PixelData glyphTemp = this.GetGlyphBitmap(this.glyphs[i].Glyph);

				this.glyphs[i].KerningSamplesLeft	= new int[kerningY.Length];
				this.glyphs[i].KerningSamplesRight	= new int[kerningY.Length];

				if (this.glyphs[i].Glyph != ' ' && this.glyphs[i].Glyph != '\t' && this.glyphs[i].Height > 0 && this.glyphs[i].Width > 0)
				{
					// Left side samples
					{
						int[] leftData = this.glyphs[i].KerningSamplesLeft;
						int leftMid = glyphTemp.Width / 2;
						int lastSampleY = 0;
						for (int sampleIndex = 0; sampleIndex < leftData.Length; sampleIndex++)
						{
							leftData[sampleIndex] = leftMid;

							int sampleY = kerningY[sampleIndex] + this.glyphs[i].OffsetY;
							int beginY = MathF.Clamp(lastSampleY, 0, glyphTemp.Height - 1);
							int endY = MathF.Clamp(sampleY, 0, glyphTemp.Height);
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
								
							int sampleY = kerningY[sampleIndex] + this.glyphs[i].OffsetY;
							int beginY = MathF.Clamp(lastSampleY, 0, glyphTemp.Height - 1);
							int endY = MathF.Clamp(sampleY, 0, glyphTemp.Height);
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
		private void ReleaseResources()
		{
			if (this.material != null) this.material.Dispose();
			if (this.texture != null) this.texture.Dispose();
			if (this.pixelData != null) this.pixelData.Dispose();

			this.material = null;
			this.texture = null;
			this.pixelData = null;
		}
		private void GenerateTexture()
		{
			if (this.texture != null) this.texture.Dispose();

			if (this.pixelData == null)
				return;

			this.texture = new Texture(this.pixelData, 
				TextureSizeMode.Enlarge, 
				this.IsPixelGridAligned ? TextureMagFilter.Nearest : TextureMagFilter.Linear,
				this.IsPixelGridAligned ? TextureMinFilter.Nearest : TextureMinFilter.LinearMipmapLinear);
		}
		private void GenerateMaterial()
		{
			if (this.material != null) this.material.Dispose();

			if (this.texture == null)
				return;

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
		private void GenerateCharLookup()
		{
			if (this.glyphs == null)
			{
				this.charLookup = new int[0];
				return;
			}

			int maxCharVal = 0;
			for (int i = 0; i < this.glyphs.Length; i++)
			{
				maxCharVal = Math.Max(maxCharVal, (int)this.glyphs[i].Glyph);
			}

			this.charLookup = new int[maxCharVal + 1];
			for (int i = 0; i < this.glyphs.Length; i++)
			{
				this.charLookup[(int)this.glyphs[i].Glyph] = i;
			}
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
			if (glyphId >= this.charLookup.Length)
			{
				data = this.glyphs[0];
				return false;
			}
			else
			{
				data = this.glyphs[this.charLookup[glyphId]];
				return true;
			}
		}
		/// <summary>
		/// Retrieves the rasterized <see cref="Duality.Drawing.PixelData"/> for a single glyph.
		/// </summary>
		/// <param name="glyph">The glyph of which to retrieve the Bitmap.</param>
		/// <returns>The Bitmap that has been retrieved, or null if the glyph is not supported.</returns>
		public PixelData GetGlyphBitmap(char glyph)
		{
			Rect targetRect;
			int charIndex = (int)glyph > this.charLookup.Length ? 0 : this.charLookup[(int)glyph];
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
			float glyphYOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);

				Vector2 glyphPos;
				glyphPos.X = MathF.Round(curOffset + glyphXOff);
				glyphPos.Y = MathF.Round(0 + glyphYOff);

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
			float glyphYOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);
				Vector2 dataCoord = uvRect.Pos * new Vector2(this.pixelData.Width, this.pixelData.Height) / this.texture.UVRatio;
				
				bitmap.DrawOnto(target, 
					BlendMode.Alpha, 
					MathF.RoundToInt(x + curOffset + glyphXOff), 
					MathF.RoundToInt(y + glyphYOff),
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
			float glyphYOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);

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
			float glyphYOff;
			float glyphXAdv;
			int lastValidLength = 0;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);

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
			float glyphYOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);

				if (i == index) return new Rect(curOffset + glyphXOff, 0 + glyphYOff, glyphData.Width, glyphData.Height);

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
			float glyphYOff;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv, out glyphXOff, out glyphYOff);

				glyphRect = new Rect(curOffset + glyphXOff, 0 + glyphYOff, glyphData.Width, glyphData.Height);
				if (glyphRect.Contains(x, y)) return i;

				curOffset += glyphXAdv;
			}

			return -1;
		}

		private void ProcessTextAdv(string text, int index, out GlyphData glyphData, out Rect uvRect, out float glyphXAdv, out float glyphXOff, out float glyphYOff)
		{
			char glyph = text[index];
			int charIndex = (int)glyph > this.charLookup.Length ? 0 : this.charLookup[(int)glyph];
			this.texture.LookupAtlas(charIndex, out uvRect);

			this.GetGlyphData(glyph, out glyphData);
			glyphXOff = -glyphData.OffsetX;
			glyphYOff = -glyphData.OffsetY;

			if (this.kerning && !this.monospace)
			{
				char glyphNext = index + 1 < text.Length ? text[index + 1] : ' ';
				GlyphData glyphDataNext;
				this.GetGlyphData(glyphNext, out glyphDataNext);

				int minSum = int.MaxValue;
				for (int k = 0; k < glyphData.KerningSamplesRight.Length; k++)
					minSum = Math.Min(minSum, glyphData.KerningSamplesRight[k] + glyphDataNext.KerningSamplesLeft[k]);

				glyphXAdv = glyphData.Width - glyphData.OffsetX + this.spacing - minSum;
			}
			else
				glyphXAdv = (this.monospace ? this.maxGlyphWidth : (glyphData.Width - glyphData.OffsetX)) + this.spacing;
		}

		protected override void OnLoaded()
		{
			this.GenerateCharLookup();
			this.GenerateTexture();
			this.GenerateMaterial();
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
			c.GenerateCharLookup();
			c.GenerateTexture();
			c.GenerateMaterial();
		}
	}
}
