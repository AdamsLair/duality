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
		/// A generic monospace Font (Size 8) that has been loaded from your systems font library.
		/// This is usually "Courier New".
		/// </summary>
		public static ContentRef<Font> GenericMonospace8	{ get; private set; }
		/// <summary>
		/// A generic monospace Font (Size 10) that has been loaded from your systems font library.
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

		
		private FontData          fontData         = null;
		private FontRenderMode    renderMode       = FontRenderMode.SharpBitmap;
		private float             spacing          = 0.0f;
		private float             lineHeightFactor = 1.0f;
		private bool              kerning          = true;
		// Data that is automatically acquired while loading the font
		[DontSerialize] private int[]             charLookup    = null;
		[DontSerialize] private Material          material      = null;
		[DontSerialize] private Texture           texture       = null;
		[DontSerialize] private Pixmap            pixmap        = null;
		[DontSerialize] private FontKerningLookup kerningLookup = null;


		/// <summary>
		/// [GET / SET] Specifies how the glyphs of this <see cref="Font"/> are rendered in a text.
		/// </summary>
		[EditorHintFlags(MemberFlags.AffectsOthers)]
		public FontRenderMode RenderMode
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
		/// [GET / SET] Additional spacing between each character.
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
		/// [GET / SET] Whether this Font uses kerning, a technique where characters are moved closer together based on their actual shape,
		/// which usually looks much nicer. It has no visual effect when active at the same time with <see cref="FontMetrics.Monospace"/>, however
		/// kerning sample data will be available on glyphs.
		/// </summary>
		/// <seealso cref="FontGlyphData"/>
		public bool Kerning
		{
			get { return this.kerning; }
			set { this.kerning = value; }
		}
		/// <summary>
		/// [GET] Returns whether this Font is (requesting to be) aligned to the pixel grid.
		/// </summary>
		public bool IsPixelGridAligned
		{
			get
			{ 
				return 
					this.renderMode == FontRenderMode.MonochromeBitmap || 
					this.renderMode == FontRenderMode.GrayscaleBitmap;
			}
		}
		/// <summary>
		/// [GET] The Fonts height.
		/// </summary>
		public int Height
		{
			get { return this.fontData.Metrics.Height; }
		}
		/// <summary>
		/// [GET] The y offset in pixels between two lines.
		/// </summary>
		public int LineSpacing
		{
			get { return MathF.RoundToInt(this.fontData.Metrics.Height * this.lineHeightFactor); }
		}
		/// <summary>
		/// [GET] Provides access to various metrics that are inherent to this <see cref="Font"/> instance,
		/// such as size, height, and various typographic measures.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		public FontMetrics Metrics
		{
			get { return this.fontData.Metrics; }
		}


		/// <summary>
		/// Applies a new set of rendered glyphs to the <see cref="Font"/>, adjusts its typeface metadata 
		/// re-generates texture and material.
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="atlas"></param>
		/// <param name="glyphs"></param>
		/// <param name="metrics"></param>
		public void SetGlyphData(FontData fontData)
		{
			this.ReleaseResources();

			this.fontData = fontData;

			this.GenerateCharLookup();
			this.GeneratePixmap();
			this.GenerateTexture();
			this.GenerateMaterial();
			this.GenerateKerningLookup();
		}
		private void ReleaseResources()
		{
			if (this.material != null) this.material.Dispose();
			if (this.texture != null) this.texture.Dispose();
			if (this.pixmap != null) this.pixmap.Dispose();

			this.material = null;
			this.texture = null;
			this.pixmap = null;
		}
		private void GeneratePixmap()
		{
			if (this.pixmap != null) this.pixmap.Dispose();

			if (this.fontData == null)
				return;
			
			this.pixmap = new Pixmap(fontData.Bitmap);
			this.pixmap.Atlas = fontData.Atlas.ToList();
		}
		private void GenerateTexture()
		{
			if (this.texture != null) this.texture.Dispose();

			if (this.pixmap == null)
				return;

			this.texture = new Texture(this.pixmap, 
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
			if (this.renderMode == FontRenderMode.MonochromeBitmap)
				technique = DrawTechnique.Mask;
			else if (this.renderMode == FontRenderMode.GrayscaleBitmap)
				technique = DrawTechnique.Alpha;
			else if (this.renderMode == FontRenderMode.SmoothBitmap)
				technique = DrawTechnique.Alpha;
			else
				technique = DrawTechnique.SharpAlpha;

			// Create and configure internal BatchInfo
			BatchInfo matInfo = new BatchInfo(technique, ColorRgba.White, this.texture);
			if (technique == DrawTechnique.SharpAlpha)
			{
				matInfo.SetUniform("smoothness", this.fontData.Metrics.Size * 4.0f);
			}
			this.material = new Material(matInfo);
		}
		private void GenerateCharLookup()
		{
			FontGlyphData[] glyphs = fontData.Glyphs;
			if (glyphs == null)
			{
				this.charLookup = new int[0];
				return;
			}

			int maxCharVal = 0;
			for (int i = 0; i < glyphs.Length; i++)
			{
				maxCharVal = Math.Max(maxCharVal, (int)glyphs[i].Glyph);
			}

			this.charLookup = new int[maxCharVal + 1];
			for (int i = 0; i < glyphs.Length; i++)
			{
				this.charLookup[(int)glyphs[i].Glyph] = i;
			}
		}
		private void GenerateKerningLookup()
		{
			this.kerningLookup = new FontKerningLookup(this.fontData.KerningPairs);
		}

		/// <summary>
		/// Retrieves information about a single glyph.
		/// </summary>
		/// <param name="glyph">The glyph to retrieve information about.</param>
		/// <param name="data">A struct holding the retrieved information.</param>
		/// <returns>True, if successful, false if the specified glyph is not supported.</returns>
		public bool GetGlyphData(char glyph, out FontGlyphData data)
		{
			int glyphId = (int)glyph;
			if (glyphId >= this.charLookup.Length)
			{
				data = this.fontData.Glyphs[0];
				return false;
			}
			else
			{
				data = this.fontData.Glyphs[this.charLookup[glyphId]];
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
			this.pixmap.LookupAtlas(charIndex, out targetRect);
			PixelData subImg = new PixelData(
				MathF.RoundToInt(targetRect.W), 
				MathF.RoundToInt(targetRect.H));
			this.pixmap.MainLayer.DrawOnto(subImg, BlendMode.Solid, 
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
			FontGlyphData glyphData;
			Rect uvRect;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);

				Vector2 glyphPos;
				glyphPos.X = MathF.Round(curOffset - glyphData.Offset.X);
				glyphPos.Y = MathF.Round(0 - glyphData.Offset.Y);

				vertices[i * 4 + 0].Pos.X = glyphPos.X;
				vertices[i * 4 + 0].Pos.Y = glyphPos.Y;
				vertices[i * 4 + 0].Pos.Z = 0.0f;
				vertices[i * 4 + 0].TexCoord = uvRect.TopLeft;
				vertices[i * 4 + 0].Color = ColorRgba.White;

				vertices[i * 4 + 1].Pos.X = glyphPos.X + glyphData.Size.X;
				vertices[i * 4 + 1].Pos.Y = glyphPos.Y;
				vertices[i * 4 + 1].Pos.Z = 0.0f;
				vertices[i * 4 + 1].TexCoord = uvRect.TopRight;
				vertices[i * 4 + 1].Color = ColorRgba.White;

				vertices[i * 4 + 2].Pos.X = glyphPos.X + glyphData.Size.X;
				vertices[i * 4 + 2].Pos.Y = glyphPos.Y + glyphData.Size.Y;
				vertices[i * 4 + 2].Pos.Z = 0.0f;
				vertices[i * 4 + 2].TexCoord = uvRect.BottomRight;
				vertices[i * 4 + 2].Color = ColorRgba.White;

				vertices[i * 4 + 3].Pos.X = glyphPos.X;
				vertices[i * 4 + 3].Pos.Y = glyphPos.Y + glyphData.Size.Y;
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
			if (this.pixmap == null)
				return;

			PixelData bitmap = this.pixmap.MainLayer;
			float curOffset = 0.0f;
			FontGlyphData glyphData;
			Rect uvRect;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);
				Vector2 dataCoord = uvRect.Pos * bitmap.Size / this.texture.UVRatio;
				
				bitmap.DrawOnto(target, 
					BlendMode.Alpha, 
					MathF.RoundToInt(x + curOffset - glyphData.Offset.X), 
					MathF.RoundToInt(y - glyphData.Offset.Y),
					(int)glyphData.Size.X, 
					(int)glyphData.Size.Y,
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
			if (this.texture == null || text == null) return Vector2.Zero;

			Vector2 textSize = Vector2.Zero;
			float curOffset = 0.0f;
			FontGlyphData glyphData;
			Rect uvRect;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);

				textSize.X = Math.Max(textSize.X, curOffset + glyphXAdv - this.spacing);
				textSize.Y = Math.Max(textSize.Y, glyphData.Size.Y);

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
			FontGlyphData glyphData;
			Rect uvRect;
			float glyphXAdv;
			int lastValidLength = 0;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);

				textSize.X = Math.Max(textSize.X, curOffset + glyphXAdv);
				textSize.Y = Math.Max(textSize.Y, glyphData.Size.Y);

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
			FontGlyphData glyphData;
			Rect uvRect;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);

				if (i == index) return new Rect(curOffset - glyphData.Offset.X, 0 - glyphData.Offset.Y, glyphData.Size.X, glyphData.Size.Y);

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
			FontGlyphData glyphData;
			Rect uvRect;
			Rect glyphRect;
			float glyphXAdv;
			for (int i = 0; i < text.Length; i++)
			{
				this.ProcessTextAdv(text, i, out glyphData, out uvRect, out glyphXAdv);

				glyphRect = new Rect(curOffset - glyphData.Offset.X, 0 - glyphData.Offset.Y, glyphData.Size.X, glyphData.Size.Y);
				if (glyphRect.Contains(x, y)) return i;

				curOffset += glyphXAdv;
			}

			return -1;
		}

		private void ProcessTextAdv(string text, int index, out FontGlyphData glyphData, out Rect uvRect, out float glyphXAdv)
		{
			char glyph = text[index];
			int charIndex = (int)glyph > this.charLookup.Length ? 0 : this.charLookup[(int)glyph];
			this.texture.LookupAtlas(charIndex, out uvRect);

			this.GetGlyphData(glyph, out glyphData);

			glyphXAdv = glyphData.Advance + this.spacing;

			if (this.kerning)
			{
				char glyphNext = index + 1 < text.Length ? text[index + 1] : ' ';
				float advanceOffset = this.kerningLookup.GetAdvanceOffset(glyph, glyphNext);
				glyphXAdv += advanceOffset;
			}
		}

		protected override void OnLoaded()
		{
			this.GenerateCharLookup();
			this.GeneratePixmap();
			this.GenerateTexture();
			this.GenerateMaterial();
			this.GenerateKerningLookup();
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
			c.GeneratePixmap();
			c.GenerateTexture();
			c.GenerateMaterial();
			c.GenerateKerningLookup();
		}
	}
}
