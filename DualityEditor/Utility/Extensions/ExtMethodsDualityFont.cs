using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;

using DualityFont = Duality.Resources.Font;
using SysDrawFont = System.Drawing.Font;
using SysDrawFontStyle = System.Drawing.FontStyle;
using FontStyle = Duality.Drawing.FontStyle;

using Duality.Drawing;

namespace Duality.Editor
{
	public static class ExtMethodsDualityFont
	{
		private const string	DefaultChars		= "? abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,;.:-_<>|#'+*~@^°!\"§$%&/()=`²³{[]}\\´öäüÖÄÜß";
		private const string	CharBaseLineRef		= "acemnorsuvwxz";
		private const string	CharDescentRef		= "pqgjyQ|";
		private const string	CharBodyAscentRef	= "acemnorsuvwxz";

		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> based on its embedded TrueType representation.
		/// </summary>
		public static void RenderGlyphs(this DualityFont font)
		{
			using (PrivateFontCollection fontManager = new PrivateFontCollection())
			{
				// Load custom font family using System.Drawing
				GCHandle handle = GCHandle.Alloc(font.EmbeddedTrueTypeFont, GCHandleType.Pinned);
				try
				{
					IntPtr fontMemPtr = handle.AddrOfPinnedObject();
					fontManager.AddMemoryFont(fontMemPtr, font.EmbeddedTrueTypeFont.Length);
				}
				finally
				{
					handle.Free();
				}

				// Render the font's glyphs
				RenderGlyphs(font, fontManager.Families.FirstOrDefault());
			}
		}
		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> using the specified system font family.
		/// </summary>
		public static void RenderGlyphs(this DualityFont font, FontFamily fontFamily)
		{
			// Determine System.Drawing font style
			SysDrawFontStyle style = SysDrawFontStyle.Regular;
			if (font.Style.HasFlag(FontStyle.Bold)) style |= SysDrawFontStyle.Bold;
			if (font.Style.HasFlag(FontStyle.Italic)) style |= SysDrawFontStyle.Italic;

			// Create a System.Drawing font
			SysDrawFont internalFont = null;
			if (fontFamily != null)
			{
				try { internalFont = new SysDrawFont(fontFamily, font.Size, style); }
				catch (Exception) { }
			}

			// If creating the font failed, fall back to a default one
			if (internalFont == null)
				internalFont = new SysDrawFont(FontFamily.GenericMonospace, font.Size, style);

			// Render the font's glyphs
			using (internalFont)
			{
				RenderGlyphs(font, internalFont);
			}
		}
		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> using the specified system font.
		/// This method assumes that the system font's size and style match the one specified in
		/// the specified Duality font.
		/// </summary>
		private static void RenderGlyphs(DualityFont target, SysDrawFont internalFont)
		{
			DualityFont.GlyphData[] glyphs = new DualityFont.GlyphData[DefaultChars.Length];
			for (int i = 0; i < glyphs.Length; i++)
			{
				glyphs[i].Glyph = DefaultChars[i];
			}

			int bodyAscent = 0;
			int baseLine = 0;
			int descent = 0;
			int ascent = 0;

			TextRenderingHint textRenderingHint;
			if (target.GlyphRenderMode == DualityFont.RenderMode.MonochromeBitmap)
				textRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
			else
				textRenderingHint = TextRenderingHint.AntiAliasGridFit;

			int cols;
			int rows;
			cols = rows = (int)Math.Ceiling(Math.Sqrt(glyphs.Length));

			PixelData pixelLayer = new PixelData(MathF.RoundToInt(cols * internalFont.Size * 1.2f), MathF.RoundToInt(rows * internalFont.Height * 1.2f));
			PixelData glyphTemp;
			PixelData glyphTempTypo;
			Bitmap bm;
			Bitmap measureBm = new Bitmap(1, 1);
			AtlasKeyValuePair[] atlas = new AtlasKeyValuePair[glyphs.Length];
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
					bool isSpace = str == " ";
					SizeF charSize = measureGraphics.MeasureString(str, internalFont, pixelLayer.Width, formatDef);

					// Rasterize a single glyph for rendering
					bm = new Bitmap((int)Math.Ceiling(Math.Max(1, charSize.Width)), internalFont.Height + 1);
					using (Graphics glyphGraphics = Graphics.FromImage(bm))
					{
						glyphGraphics.Clear(Color.Transparent);
						glyphGraphics.TextRenderingHint = textRenderingHint;
						glyphGraphics.DrawString(str, internalFont, fntBrush, new RectangleF(0, 0, bm.Width, bm.Height), formatDef);
					}
					glyphTemp = new PixelData();
					glyphTemp.FromBitmap(bm);
					
					// Rasterize a single glyph in typographic mode for metric analysis
					if (!isSpace)
					{
						Point2 glyphTempOpaqueTopLeft;
						Point2 glyphTempOpaqueSize;
						glyphTemp.GetOpaqueBoundaries(out glyphTempOpaqueTopLeft, out glyphTempOpaqueSize);

						glyphTemp.SubImage(glyphTempOpaqueTopLeft.X, 0, glyphTempOpaqueSize.X, glyphTemp.Height);

						if (CharBodyAscentRef.Contains(glyphs[i].Glyph))
							bodyAscent += glyphTempOpaqueSize.Y;
						if (CharBaseLineRef.Contains(glyphs[i].Glyph))
							baseLine += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						if (CharDescentRef.Contains(glyphs[i].Glyph))
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
						glyphTempTypo = glyphTemp;
					}

					// Update xy values if it doesn't fit anymore
					if (x + glyphTemp.Width + 2 > pixelLayer.Width)
					{
						x = 1;
						y += internalFont.Height + MathF.Clamp((int)MathF.Ceiling(internalFont.Height * 0.1875f), 3, 10);
					}
					
					// Memorize atlas coordinates & glyph data
					glyphs[i].Width = glyphTemp.Width;
					glyphs[i].Height = glyphTemp.Height;
					glyphs[i].OffsetX = glyphTemp.Width - glyphTempTypo.Width;
					if (isSpace)
					{
						glyphs[i].Width /= 2;
						glyphs[i].OffsetX /= 2;
					}

					Rect rect = new Rect();
					rect.X = x;
					rect.Y = y;
					rect.W = glyphTemp.Width;
					rect.H = (internalFont.Height + 1);

					atlas[i] = new AtlasKeyValuePair(glyphs[i].Glyph.ToString(), rect);

					// Draw it onto the font surface
					glyphTemp.DrawOnto(pixelLayer, BlendMode.Solid, x, y);

					x += glyphTemp.Width + MathF.Clamp((int)MathF.Ceiling(internalFont.Height * 0.125f), 2, 10);
				}
			}

			// White out texture except alpha channel.
			for (int i = 0; i < pixelLayer.Data.Length; i++)
			{
				pixelLayer.Data[i].R = 255;
				pixelLayer.Data[i].G = 255;
				pixelLayer.Data[i].B = 255;
			}

			// Monospace offset adjustments
			if (target.MonoSpace)
			{
				int maxGlyphWidth = 0;
				for (int i = 0; i < glyphs.Length; i++)
				{
					maxGlyphWidth = Math.Max(maxGlyphWidth, glyphs[i].Width);
				}
				for (int i = 0; i < glyphs.Length; ++i)
				{
					glyphs[i].OffsetX -= (int)Math.Round((maxGlyphWidth - glyphs[i].Width) / 2.0f);
				}
			}

			// Determine Font properties
			{
				float lineSpacing = internalFont.FontFamily.GetLineSpacing(internalFont.Style);
				float emHeight = internalFont.FontFamily.GetEmHeight(internalFont.Style);
				float cellAscent = internalFont.FontFamily.GetCellAscent(internalFont.Style);
				float cellDescent = internalFont.FontFamily.GetCellDescent(internalFont.Style);

				ascent = (int)Math.Round(cellAscent * internalFont.Size / emHeight);
				bodyAscent /= CharBodyAscentRef.Length;
				baseLine /= CharBaseLineRef.Length;
				descent = (int)Math.Round(((float)descent / CharDescentRef.Length) - (float)baseLine);
			}

			// Apply rendered glyph data to the Duality Font
			target.SetGlyphData(pixelLayer, atlas, glyphs, (int)internalFont.Height, ascent, bodyAscent, descent, baseLine);
		}
	}
}
