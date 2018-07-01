using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;

using DualityFont = Duality.Resources.Font;
using SysDrawFont = System.Drawing.Font;
using SysDrawFontStyle = System.Drawing.FontStyle;
using FontStyle = Duality.Drawing.FontStyle;
using UnicodeBlock = Duality.Drawing.UnicodeBlock;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.AssetManagement;
using System.Text;

namespace Duality.Editor.Plugins.Base
{
	public class FontAssetImporter : AssetImporter<DualityFont>
	{
		private static readonly string[] SourceFileExtensions = new[] { ".ttf", ".otf" };
		private static readonly UnicodeBlock[] DefaultBlocks = new[] { UnicodeBlock.BasicLatin, UnicodeBlock.Latin1Supplement };


		private Dictionary<int, PrivateFontCollection> fontManagers;


		public override string Id
		{
			get { return "BasicFontAssetImporter"; }
		}
		public override string Name
		{
			get { return "Font Importer"; }
		}
		public override int Priority
		{
			get { return PriorityGeneral; }
		}
		protected override string[] SourceFileExts
		{
			get { return SourceFileExtensions; }
		}


		protected override void ImportResource(ContentRef<DualityFont> resourceRef, AssetImportInput input, IAssetImportEnvironment env)
		{
			DualityFont resource = resourceRef.Res;

			// Retrieve import parameters
			float              size          = env.GetOrInitParameter(resourceRef, "Size",          16.0f);
			FontStyle          style         = env.GetOrInitParameter(resourceRef, "Style",         FontStyle.Regular);
			string             customCharSet = env.GetOrInitParameter(resourceRef, "CustomCharSet", string.Empty);
			List<UnicodeBlock> unicodeBlocks = env.GetOrInitParameter(resourceRef, "UnicodeBlocks", new List<UnicodeBlock>(DefaultBlocks));
			bool               antialiasing  = env.GetOrInitParameter(resourceRef, "AntiAlias",     true);
			bool               monospace     = env.GetOrInitParameter(resourceRef, "Monospace",     false);

			HashSet<char> fullCharSet = new HashSet<char>();

			if (!string.IsNullOrWhiteSpace(customCharSet))
			{
				string[] blocks = customCharSet.Split(',');
				ulong start = 0;
				ulong end = 0;

				foreach (string block in blocks)
				{
					string[] limits = block.Split(new[] { '-' }, 3);
					if (!ulong.TryParse(limits[0], NumberStyles.HexNumber, null, out start))
						Logs.Editor.WriteError("Cannot parse value " + limits[0] + "; CustomCharSet will be ignored. Please verify the value and repeat the import.");

					if (limits.Length == 1)
						end = start;
					else
					{
						if (limits.Length == 2 && !ulong.TryParse(limits[1], NumberStyles.HexNumber, null, out end))
							Logs.Editor.WriteError("Cannot parse value " + limits[1] + "; CustomCharSet will be ignored. Please verify the value and repeat the import.");

						else if (limits.Length > 2)
							Logs.Editor.WriteError("Unexpected values " + limits[2] + " in range " + block + " will be ignored. Please verify the value and repeat the import.");

						if (start > end)
							Logs.Editor.WriteWarning(start + " is bigger than " + end + "; block will be ignored. Please verify the value and repeat the import.");
					}

					for (char c = (char)start; c <= (char)end; c++)
				if (!char.IsControl(c)) fullCharSet.Add(c);
				}
			}

			if (unicodeBlocks != null)
			{
				Type unicodeBlockType = typeof(UnicodeBlock);
				Type unicodeRangeAttrType = typeof(UnicodeRangeAttribute);

				foreach (UnicodeBlock block in unicodeBlocks)
				{
					UnicodeRangeAttribute range = unicodeBlockType.GetMember(block.ToString())
						.First()
						.GetCustomAttributes(unicodeRangeAttrType, false)
						.FirstOrDefault() as UnicodeRangeAttribute;

					if (range != null)
					{
						for (char c = (char)range.CharStart; c <= (char)range.CharEnd; c++)
							if (!char.IsControl(c)) fullCharSet.Add(c);
					}
				}
			}

			// Load the TrueType Font and render all the required glyphs
			byte[] trueTypeData = File.ReadAllBytes(input.Path);
			FontData fontData = this.RenderGlyphs(
				trueTypeData,
				size,
				style,
				new FontCharSet(new string(fullCharSet.ToArray())),
				antialiasing,
				monospace);

			// Transfer our rendered Font data to the Font Resource
			resource.SetGlyphData(fontData);
		}
		protected override bool CanExport(DualityFont resource)
		{
			return false;
		}
		protected override void ExportResource(ContentRef<DualityFont> resourceRef, string path, IAssetExportEnvironment env) { }

		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> based on its embedded TrueType representation.
		/// <param name="extendedSet">Extended set of characters for renderning.</param>
		/// </summary>
		private FontData RenderGlyphs(byte[] trueTypeFontData, float emSize, FontStyle style, FontCharSet extendedSet, bool antialiasing, bool monospace)
		{
			if (this.fontManagers == null)
				this.fontManagers = new Dictionary<int, PrivateFontCollection>();

			// Allocate one PrivateFontCollection for each embedded TrueType Font
			// This is an unfortunate requirement to keep track of which Font is which,
			// since a byte[] doesn't give it away, and a Font collection won't tell us
			// which one we just added.
			PrivateFontCollection manager;
			int fontId = trueTypeFontData.GetHashCode();
			if (!this.fontManagers.TryGetValue(fontId, out manager))
			{
				manager = new PrivateFontCollection();
				this.fontManagers.Add(fontId, manager);
			}

			// Load custom font family using System.Drawing
			if (manager.Families.Length == 0)
			{
				IntPtr fontBuffer = Marshal.AllocCoTaskMem(trueTypeFontData.Length);
				Marshal.Copy(trueTypeFontData, 0, fontBuffer, trueTypeFontData.Length);
				manager.AddMemoryFont(fontBuffer, trueTypeFontData.Length);
			}

			// Render the font's glyphs
			return this.RenderGlyphs(
				manager.Families.FirstOrDefault(), 
				emSize, 
				style, 
				extendedSet, 
				antialiasing, 
				monospace);

			// Yes, we have a minor memory leak here - both the Font buffer and the private
			// Font collection. Unfortunately though, GDI+ won't let us dispose them
			// properly due to aggressive Font caching, see here:
			//
			// http://stackoverflow.com/questions/25583394/privatefontcollection-addmemoryfont-producing-random-errors-on-windows-server-20
			//
			// "Standard GDI+ lossage, disposing a Font does not actually destroy it. 
			// It gets put back into a cache, with the assumption that it will be used again. 
			// An important perf optimization, creating fonts is pretty expensive. That ends 
			// poorly for private fonts when you destroy their home, the font will use 
			// released memory. Producing bewildering results, including hard crashes. You'll 
			// need to keep the collection around, as well as the IntPtr."
			// – Hans Passant Aug 30 '14 at 16:13

		}
		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> using the specified system font family.
		/// </summary>
		private FontData RenderGlyphs(FontFamily fontFamily, float emSize, FontStyle style, FontCharSet extendedSet, bool antialiasing, bool monospace)
		{
			// Determine System.Drawing font style
			SysDrawFontStyle systemStyle = SysDrawFontStyle.Regular;
			if (style.HasFlag(FontStyle.Bold)) systemStyle |= SysDrawFontStyle.Bold;
			if (style.HasFlag(FontStyle.Italic)) systemStyle |= SysDrawFontStyle.Italic;

			// Create a System.Drawing font
			SysDrawFont internalFont = null;
			if (fontFamily != null)
			{
				try { internalFont = new SysDrawFont(fontFamily, emSize, systemStyle); }
				catch (Exception e)
				{
					Logs.Editor.WriteError(
						"Failed to create System Font '{1} {2}, {3}' for rendering Duality Font glyphs: {0}",
						LogFormat.Exception(e),
						fontFamily.Name,
						emSize,
						style);
				}
			}

			// If creating the font failed, fall back to a default one
			if (internalFont == null)
				internalFont = new SysDrawFont(FontFamily.GenericMonospace, emSize, systemStyle);

			// Render the font's glyphs
			using (internalFont)
			{
				return this.RenderGlyphs(
					internalFont, 
					FontCharSet.Default.MergedWith(extendedSet),
					antialiasing, 
					monospace);
			}
		}
		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> using the specified system font.
		/// This method assumes that the system font's size and style match the one specified in
		/// the specified Duality font.
		/// </summary>
		private FontData RenderGlyphs(SysDrawFont internalFont, FontCharSet charSet, bool antialiazing, bool monospace)
		{
			FontGlyphData[] glyphs = new FontGlyphData[charSet.Chars.Length];
			for (int i = 0; i < glyphs.Length; i++)
			{
				glyphs[i].Glyph = charSet.Chars[i];
			}

			int bodyAscent = 0;
			int baseLine = 0;
			int descent = 0;
			int ascent = 0;

			TextRenderingHint textRenderingHint;
			if (antialiazing)
				textRenderingHint = TextRenderingHint.AntiAliasGridFit;
			else
				textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

			int cols;
			int rows;
			cols = rows = (int)Math.Ceiling(Math.Sqrt(glyphs.Length));

			PixelData pixelLayer = new PixelData(
				MathF.RoundToInt(cols * internalFont.Size * 1.2f), 
				MathF.RoundToInt(rows * internalFont.Height * 1.2f),
				ColorRgba.TransparentBlack);
			Bitmap measureBm = new Bitmap(1, 1);
			Rect[] atlas = new Rect[glyphs.Length];
			PixelData[] glyphBitmaps = new PixelData[glyphs.Length];
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
				for (int i = 0; i < glyphs.Length; ++i)
				{
					string str = glyphs[i].Glyph.ToString(CultureInfo.InvariantCulture);
					bool isSpace = char.IsWhiteSpace(glyphs[i].Glyph);
					SizeF charSize = measureGraphics.MeasureString(str, internalFont, pixelLayer.Width, formatDef);

					// Rasterize a single glyph for rendering
					Bitmap bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), internalFont.Height + 1);
					using (Graphics glyphGraphics = Graphics.FromImage(bm))
					{
						glyphGraphics.Clear(Color.Transparent);
						glyphGraphics.TextRenderingHint = textRenderingHint;
						glyphGraphics.DrawString(str, internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatDef);
					}
					glyphBitmaps[i] = new PixelData();
					glyphBitmaps[i].FromBitmap(bm);
					
					// Rasterize a single glyph in typographic mode for metric analysis
					PixelData glyphTempTypo;
					if (!isSpace)
					{
						Point2 glyphTempOpaqueTopLeft;
						Point2 glyphTempOpaqueSize;
						glyphBitmaps[i].GetOpaqueBoundaries(out glyphTempOpaqueTopLeft, out glyphTempOpaqueSize);

						glyphBitmaps[i].SubImage(glyphTempOpaqueTopLeft.X, 0, glyphTempOpaqueSize.X, glyphBitmaps[i].Height);

						if (charSet.CharBodyAscentRef.Contains(glyphs[i].Glyph))
							bodyAscent += glyphTempOpaqueSize.Y;
						if (charSet.CharBaseLineRef.Contains(glyphs[i].Glyph))
							baseLine += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						if (charSet.CharDescentRef.Contains(glyphs[i].Glyph))
							descent += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						
						bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), internalFont.Height + 1);
						using (Graphics glyphGraphics = Graphics.FromImage(bm))
						{
							glyphGraphics.Clear(Color.Transparent);
							glyphGraphics.TextRenderingHint = textRenderingHint;
							glyphGraphics.DrawString(str, internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatTypo);
						}
						glyphTempTypo = new PixelData();
						glyphTempTypo.FromBitmap(bm);
						glyphTempTypo.Crop(true, false);
					}
					else
					{
						glyphTempTypo = glyphBitmaps[i];
					}

					// Update xy values if it doesn't fit anymore
					if (x + glyphBitmaps[i].Width + 2 > pixelLayer.Width)
					{
						x = 1;
						y += internalFont.Height + MathF.Clamp((int)MathF.Ceiling(internalFont.Height * 0.1875f), 3, 10);
					}
					
					// Memorize atlas coordinates & glyph data
					glyphs[i].Size = glyphBitmaps[i].Size;
					glyphs[i].Offset.X = glyphBitmaps[i].Width - glyphTempTypo.Width;
					glyphs[i].Offset.Y = 0; // TTF fonts are rendered on blocks that are the whole size of the height - so no need for offset
					if (isSpace)
					{
						glyphs[i].Size.X /= 2;
						glyphs[i].Offset.X /= 2;
					}
					glyphs[i].Advance = glyphs[i].Size.X - glyphs[i].Offset.X;

					atlas[i].X = x;
					atlas[i].Y = y;
					atlas[i].W = glyphBitmaps[i].Width;
					atlas[i].H = (internalFont.Height + 1);

					// Draw it onto the font surface
					glyphBitmaps[i].DrawOnto(pixelLayer, BlendMode.Solid, x, y);

					x += glyphBitmaps[i].Width + MathF.Clamp((int)MathF.Ceiling(internalFont.Height * 0.125f), 2, 10);
				}
			}

			// White out texture except alpha channel.
			for (int i = 0; i < pixelLayer.Data.Length; i++)
			{
				pixelLayer.Data[i].R = 255;
				pixelLayer.Data[i].G = 255;
				pixelLayer.Data[i].B = 255;
			}

			// Monospace offset and advance adjustments
			if (monospace)
			{
				float maxGlyphWidth = 0;
				for (int i = 0; i < glyphs.Length; i++)
				{
					maxGlyphWidth = Math.Max(maxGlyphWidth, glyphs[i].Size.X);
				}
				for (int i = 0; i < glyphs.Length; ++i)
				{
					glyphs[i].Offset.X -= (int)Math.Round((maxGlyphWidth - glyphs[i].Size.X) / 2.0f);
					glyphs[i].Advance = maxGlyphWidth;
				}
			}

			// Determine Font properties
			{
				float lineSpacing = internalFont.FontFamily.GetLineSpacing(internalFont.Style);
				float emHeight = internalFont.FontFamily.GetEmHeight(internalFont.Style);
				float cellAscent = internalFont.FontFamily.GetCellAscent(internalFont.Style);
				float cellDescent = internalFont.FontFamily.GetCellDescent(internalFont.Style);

				ascent = (int)Math.Round(cellAscent * internalFont.Size / emHeight);
				bodyAscent /= charSet.CharBodyAscentRef.Length;
				baseLine /= charSet.CharBaseLineRef.Length;
				descent = (int)Math.Round(((float)descent / charSet.CharDescentRef.Length) - (float)baseLine);
			}

			// Aggregate rendered and generated data into our return value
			FontMetrics metrics = new FontMetrics(
				size:       internalFont.SizeInPoints,
				height:     (int)internalFont.Height, 
				ascent:     ascent, 
				bodyAscent: bodyAscent, 
				descent:    descent, 
				baseLine:   baseLine,
				monospace:  monospace);

			// Determine kerning pairs
			FontKerningPair[] kerningPairs = null;
			if (monospace)
				kerningPairs = null;
			else
				kerningPairs = this.GatherKerningPairs(glyphs, metrics, glyphBitmaps);

			return new FontData(pixelLayer, atlas, glyphs, metrics, kerningPairs);
		}

		private FontKerningPair[] GatherKerningPairs(FontGlyphData[] glyphs, FontMetrics metrics, PixelData[] glyphBitmaps)
		{
			// Generate a sampling mask that decides at which heights we'll sample each glyph
			int[] kerningMask = this.GetKerningMask(metrics);

			// Gather samples from all glyphs that we have based on the image data we acquired
			int[][] leftSamples = new int[glyphs.Length][];
			int[][] rightSamples = new int[glyphs.Length][];
			for (int i = 0; i < glyphs.Length; i++)
			{
				this.GatherKerningSamples(
					glyphs[i].Glyph, 
					glyphs[i].Offset, 
					glyphBitmaps[i], 
					kerningMask, 
					ref leftSamples[i], 
					ref rightSamples[i]);
			}

			// Find all glyph combinations with a non-zero kerning offset
			List<FontKerningPair> pairs = new List<FontKerningPair>();
			for (int i = 0; i < glyphs.Length; i++)
			{
				for (int j = 0; j < glyphs.Length; j++)
				{
					// Calculate the smallest depth sum across all height samples
					int minSum = int.MaxValue;
					for (int k = 0; k < rightSamples[i].Length; k++)
						minSum = Math.Min(minSum, rightSamples[i][k] + leftSamples[j][k]);

					// The smallest one represents the amount of pixels between the two
					// glyphs that is completely empty. Out kerning offset will be the negative
					// of that to make the two glyphs appear closer together.
					float kerningOffset = -minSum;
					if (kerningOffset != 0.0f)
					{
						pairs.Add(new FontKerningPair(
							glyphs[i].Glyph, 
							glyphs[j].Glyph, 
							kerningOffset));
					}
				}
			}

			return pairs.ToArray();
		}
		private int[] GetKerningMask(FontMetrics metrics)
		{
			int kerningSamples = (metrics.Ascent + metrics.Descent) / 4;
			int[] kerningY;
			if (kerningSamples <= 6)
			{
				kerningSamples = 6;
				kerningY = new int[] {
					metrics.BaseLine - metrics.Ascent,
					metrics.BaseLine - metrics.BodyAscent,
					metrics.BaseLine - metrics.BodyAscent * 2 / 3,
					metrics.BaseLine - metrics.BodyAscent / 3,
					metrics.BaseLine,
					metrics.BaseLine + metrics.Descent};
			}
			else
			{
				kerningY = new int[kerningSamples];
				int bodySamples = kerningSamples * 2 / 3;
				int descentSamples = (kerningSamples - bodySamples) / 2;
				int ascentSamples = kerningSamples - bodySamples - descentSamples;

				for (int k = 0; k < ascentSamples; k++) 
					kerningY[k] = metrics.BaseLine - metrics.Ascent + k * (metrics.Ascent - metrics.BodyAscent) / ascentSamples;
				for (int k = 0; k < bodySamples; k++) 
					kerningY[ascentSamples + k] = metrics.BaseLine - metrics.BodyAscent + k * metrics.BodyAscent / (bodySamples - 1);
				for (int k = 0; k < descentSamples; k++) 
					kerningY[ascentSamples + bodySamples + k] = metrics.BaseLine + (k + 1) * metrics.Descent / descentSamples;
			}
			return kerningY;
		}
		private void GatherKerningSamples(char glyph, Vector2 glyphOffset, PixelData glyphBitmap, int[] sampleMask, ref int[] samplesLeft, ref int[] samplesRight)
		{
			samplesLeft = new int[sampleMask.Length];
			samplesRight= new int[sampleMask.Length];

			if (glyph == ' ') return;
			if (glyph == '\t') return;
			if (glyphBitmap.Width <= 0) return;
			if (glyphBitmap.Height <= 0) return;

			Point2 glyphSize = glyphBitmap.Size;

			// Left side samples
			{
				int leftMid = glyphSize.X / 2;
				int lastSampleY = 0;
				for (int sampleIndex = 0; sampleIndex < samplesLeft.Length; sampleIndex++)
				{
					samplesLeft[sampleIndex] = leftMid;

					int sampleY = sampleMask[sampleIndex] + (int)glyphOffset.Y;
					int beginY = MathF.Clamp(lastSampleY, 0, glyphSize.Y - 1);
					int endY = MathF.Clamp(sampleY, 0, glyphSize.Y);
					if (sampleIndex == samplesLeft.Length - 1) endY = glyphSize.Y;
					lastSampleY = endY;

					for (int y = beginY; y < endY; y++)
					{
						int x = 0;
						while (glyphBitmap[x, y].A <= 64)
						{
							x++;
							if (x >= leftMid) break;
						}
						samplesLeft[sampleIndex] = Math.Min(samplesLeft[sampleIndex], x);
					}
				}
			}

			// Right side samples
			{
				int rightMid = (glyphSize.X + 1) / 2;
				int lastSampleY = 0;
				for (int sampleIndex = 0; sampleIndex < samplesRight.Length; sampleIndex++)
				{
					samplesRight[sampleIndex] = rightMid;
								
					int sampleY = sampleMask[sampleIndex] + (int)glyphOffset.Y;
					int beginY = MathF.Clamp(lastSampleY, 0, glyphSize.Y - 1);
					int endY = MathF.Clamp(sampleY, 0, glyphSize.Y);
					if (sampleIndex == samplesRight.Length - 1) endY = glyphSize.Y;
					lastSampleY = endY;

					for (int y = beginY; y < endY; y++)
					{
						int x = glyphSize.X - 1;
						while (glyphBitmap[x, y].A <= 64)
						{
							x--;
							if (x <= rightMid) break;
						}
						samplesRight[sampleIndex] = Math.Min(samplesRight[sampleIndex], glyphSize.X - 1 - x);
					}
				}
			}

			return;
		}
	}
}
