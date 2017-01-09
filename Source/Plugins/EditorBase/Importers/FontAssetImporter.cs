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

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.AssetManagement;

namespace Duality.Editor.Plugins.Base
{
	public class FontAssetImporter : IAssetImporter
	{
		public static readonly string SourceFileExtPrimary = ".ttf";
		private static readonly string[] SourceFileExts = new[] { SourceFileExtPrimary };

		private Dictionary<int, PrivateFontCollection> fontManagers;
		

		public string Id
		{
			get { return "BasicFontAssetImporter"; }
		}
		public string Name
		{
			get { return "TrueType Font Importer"; }
		}
		public int Priority
		{
			get { return 0; }
		}


		public void PrepareImport(IAssetImportEnvironment env)
		{
			// Ask to handle all input that matches the conditions in AcceptsInput
			foreach (AssetImportInput input in env.HandleAllInput(this.AcceptsInput))
			{
				// For all handled input items, specify which Resource the importer intends to create / modify
				env.AddOutput<DualityFont>(input.AssetName, input.Path);
			}
		}
		public void Import(IAssetImportEnvironment env)
		{
			// Handle all available input. No need to filter or ask for this anymore, as
			// the preparation step already made a selection with AcceptsInput. We won't
			// get any input here that didn't match.
			foreach (AssetImportInput input in env.Input)
			{
				// Request a target Resource with a name matching the input
				ContentRef<DualityFont> targetRef = env.GetOutput<DualityFont>(input.AssetName);

				// If we successfully acquired one, proceed with the import
				if (targetRef.IsAvailable)
				{
					DualityFont target = targetRef.Res;

					// Retrieve import parameters
					float       size            = env.GetOrInitParameter(targetRef, "Size"           , 16.0f            );
					FontStyle   style           = env.GetOrInitParameter(targetRef, "Style"          , FontStyle.Regular);
					string      extendedCharSet = env.GetOrInitParameter(targetRef, "ExtendedCharSet", string.Empty     );
					bool        antialiasing    = env.GetOrInitParameter(targetRef, "AntiAlias"      , true             );
					bool        monospace       = env.GetOrInitParameter(targetRef, "Monospace"      , false            );

					// Load the TrueType Font and render all the required glyphs
					byte[] trueTypeData = File.ReadAllBytes(input.Path);
					RenderedFontData fontData = this.RenderGlyphs(
						trueTypeData, 
						size, 
						style, 
						!string.IsNullOrEmpty(extendedCharSet) ? new FontCharSet(extendedCharSet) : null, 
						antialiasing, 
						monospace);

					// Transfer our rendered Font data to the Font Resource
					target.SetGlyphData(
						fontData.Bitmap, 
						fontData.Atlas, 
						fontData.GlyphData, 
						fontData.Metrics);

					// Add the requested output to signal that we've done something with it
					env.AddOutput(targetRef, input.Path);
				}
			}
		}
		
		private bool AcceptsInput(AssetImportInput input)
		{
			string inputFileExt = Path.GetExtension(input.Path);
			bool matchingFileExt = SourceFileExts.Any(acceptedExt => string.Equals(inputFileExt, acceptedExt, StringComparison.InvariantCultureIgnoreCase));
			return matchingFileExt;
		}

		// Export is not supported
		void IAssetImporter.PrepareExport(IAssetExportEnvironment env) { }
		void IAssetImporter.Export(IAssetExportEnvironment env) { }

		/// <summary>
		/// Holds the internal result of rendering a TrueType font.
		/// </summary>
		private struct RenderedFontData
		{
			/// <summary>
			/// The pixel data that was produced during rendering. A single
			/// bitmap contains all the rendered glyphs.
			/// </summary>
			public PixelData Bitmap;
			/// <summary>
			/// The bitmap / pixel atlas of the glyph pixel data.
			/// </summary>
			public Rect[] Atlas;
			/// <summary>
			/// Information about each rendered glyph, e.g. its size,
			/// offset, as well as how far the text will advance after it.
			/// </summary>
			public FontGlyphData[] GlyphData;
			/// <summary>
			/// Overall font metrics that were generated or retrieved.
			/// </summary>
			public FontMetrics Metrics;
		}

		/// <summary>
		/// Renders the <see cref="Duality.Resources.Font"/> based on its embedded TrueType representation.
		/// <param name="extendedSet">Extended set of characters for renderning.</param>
		/// </summary>
		private RenderedFontData RenderGlyphs(byte[] trueTypeFontData, float emSize, FontStyle style, FontCharSet extendedSet, bool antialiasing, bool monospace)
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
		private RenderedFontData RenderGlyphs(FontFamily fontFamily, float emSize, FontStyle style, FontCharSet extendedSet, bool antialiasing, bool monospace)
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
		private RenderedFontData RenderGlyphs(SysDrawFont internalFont, FontCharSet charSet, bool antialiazing, bool monospace)
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
					glyphs[i].OffsetY = 0; // ttf fonts are rendered on blocks that are the whole size of the height - so no need for offset
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
			if (monospace)
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
			return new RenderedFontData
			{
				Bitmap = pixelLayer,
				Atlas = atlas,
				GlyphData = glyphs,
				Metrics = metrics
			};
		}
	}
}
