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
		private const string DefaultChars      = "? abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,;.:-_<>|#'+*~@^°!\"§$%&/()=`²³{[]}\\´öäüÖÄÜß";
		private const string CharBaseLineRef   = "acemnorsuvwxz";
		private const string CharDescentRef    = "pqgjyQ|";
		private const string CharBodyAscentRef = "acemnorsuvwxz";

		private static Dictionary<int,PrivateFontCollection> fontManagers;

		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> based on its embedded TrueType representation.
		/// <param name="extendedSet">Extended set of characters for renderning.</param>
		/// </summary>
		public static void RenderGlyphs(this DualityFont font, FontRenderGlyphCharSet extendedSet = null)
		{
			if (font.EmbeddedTrueTypeFont == null) throw new InvalidOperationException("Can't render glyphs of a Duality Font without embedded vector Font information.");

			if (fontManagers == null)
				fontManagers = new Dictionary<int,PrivateFontCollection>();

			// Allocate one PrivateFontCollection for each embedded TrueType Font
			// This is an unfortunate requirement to keep track of which Font is which,
			// since a byte[] doesn't give it away, and a Font collection won't tell us
			// which one we just added.
			PrivateFontCollection manager;
			int fontId = font.EmbeddedTrueTypeFont.GetHashCode();
			if (!fontManagers.TryGetValue(fontId, out manager))
			{
				manager = new PrivateFontCollection();
				fontManagers.Add(fontId, manager);
			}

			// Load custom font family using System.Drawing
			if (manager.Families.Length == 0)
			{
				IntPtr fontBuffer = Marshal.AllocCoTaskMem(font.EmbeddedTrueTypeFont.Length);
				Marshal.Copy(font.EmbeddedTrueTypeFont, 0, fontBuffer, font.EmbeddedTrueTypeFont.Length);
				manager.AddMemoryFont(fontBuffer, font.EmbeddedTrueTypeFont.Length);
			}

			// Render the font's glyphs
			RenderGlyphs(font, manager.Families.FirstOrDefault(), extendedSet);

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
		public static void RenderGlyphs(this DualityFont font, FontFamily fontFamily, FontRenderGlyphCharSet extendedSet = null)
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
				catch (Exception e)
				{
					Log.Editor.WriteError(
						"Failed to create System Font '{1} {2}, {3}' for rendering Duality Font glyphs: {0}",
						Log.Exception(e),
						fontFamily.Name,
						font.Size,
						style);
				}
			}

			// If creating the font failed, fall back to a default one
			if (internalFont == null)
				internalFont = new SysDrawFont(FontFamily.GenericMonospace, font.Size, style);

			// Render the font's glyphs
			using (internalFont)
			{
				RenderGlyphs(font, internalFont, extendedSet);
			}
		}
		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> using the specified system font.
		/// This method assumes that the system font's size and style match the one specified in
		/// the specified Duality font.
		/// </summary>
		private static void RenderGlyphs(DualityFont target, SysDrawFont internalFont, FontRenderGlyphCharSet extendedSet = null)
		{
			string allChars = DefaultChars;
			string allCharBaseLineRef = CharBaseLineRef;
			string allCharDescentRef = CharDescentRef;
			string allCharBodyAscentRef = CharBodyAscentRef;

			if (extendedSet != null)
			{
				allChars += extendedSet.Chars;
				allCharBaseLineRef += extendedSet.CharBaseLine;
				allCharDescentRef += extendedSet.CharDescent;
				allCharBodyAscentRef += extendedSet.CharBodyAscent;
			}

			DualityFont.GlyphData[] glyphs = new DualityFont.GlyphData[allChars.Length];
			for (int i = 0; i < glyphs.Length; i++)
			{
				glyphs[i].Glyph = allChars[i];
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

			PixelData pixelLayer = new PixelData(
				MathF.RoundToInt(cols * internalFont.Size * 1.2f), 
				MathF.RoundToInt(rows * internalFont.Height * 1.2f),
				ColorRgba.TransparentBlack);
			PixelData glyphTemp;
			PixelData glyphTempTypo;
			Bitmap bm;
			Bitmap measureBm = new Bitmap(1, 1);
			Rect[] atlas = new Rect[glyphs.Length];
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

						if (allCharBodyAscentRef.Contains(glyphs[i].Glyph))
							bodyAscent += glyphTempOpaqueSize.Y;
						if (allCharBaseLineRef.Contains(glyphs[i].Glyph))
							baseLine += glyphTempOpaqueTopLeft.Y + glyphTempOpaqueSize.Y;
						if (allCharDescentRef.Contains(glyphs[i].Glyph))
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
					atlas[i].X = x;
					atlas[i].Y = y;
					atlas[i].W = glyphTemp.Width;
					atlas[i].H = (internalFont.Height + 1);

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
				bodyAscent /= allCharBodyAscentRef.Length;
				baseLine /= allCharBaseLineRef.Length;
				descent = (int)Math.Round(((float)descent / allCharDescentRef.Length) - (float)baseLine);
			}

			// Apply rendered glyph data to the Duality Font
			target.SetGlyphData(pixelLayer, atlas, glyphs, (int)internalFont.Height, ascent, bodyAscent, descent, baseLine);
		}
	}

	/// <summary>
	/// Represents a character set that can be used to specify additional characters in <see cref="ExtMethodsDualityFont.RenderGlyphs"/>.
	/// </summary>
	public class FontRenderGlyphCharSet
	{
		/// <summary>
		/// [GET / SET] All characters that will be available in the rendered character set.
		/// </summary>
		public string Chars { get; set; }
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font"/> <see cref="Duality.Resources.Font.BaseLine"/> parameter.
		/// </summary>
		public string CharBaseLine { get; set; }
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font"/> <see cref="Duality.Resources.Font.Descent"/> parameter.
		/// </summary>
		public string CharDescent { get; set; }
		/// <summary>
		/// [GET / SET] Characters which will contribute to calculating the <see cref="Duality.Resources.Font"/> <see cref="Duality.Resources.Font.Ascent"/> parameter.
		/// </summary>
		public string CharBodyAscent { get; set; }
	}
}
