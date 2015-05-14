using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Duality;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Font = Duality.Resources.Font;


namespace Duality.Editor.Plugins.Base.PreviewGenerators
{
	public class FontPreviewGenerator : PreviewGenerator<Font>
	{
		public override void Perform(Font font, PreviewImageQuery query)
		{
			int desiredWidth = query.DesiredWidth;
			int desiredHeight = query.DesiredHeight;
			const string text = "/acThe quick brown fox jumps over the lazy dog.";
			FormattedText formatText = new FormattedText();
			formatText.MaxWidth = Math.Max(1, desiredWidth - 10);
			formatText.MaxHeight = Math.Max(1, desiredHeight - 10);
			formatText.WordWrap = FormattedText.WrapMode.Word;
			formatText.Fonts = new[] { new ContentRef<Font>(font) };
			formatText.ApplySource(text);
			Pixmap.Layer textLayer = new Pixmap.Layer(desiredWidth, MathF.RoundToInt(formatText.Size.Y));
			formatText.RenderToBitmap(text, textLayer, 5, 0);

			Bitmap resultBitmap = textLayer.ToBitmap();

			// Debug Font metrics
			//const bool drawDebugFontMetrics = true;
			//if (drawDebugFontMetrics)
			//{
			//    var metrics = formatText.TextMetrics;
			//    Color fgColor = Color.White;
			//    Color baseLineColor = Color.FromArgb(255, 0, 0);
			//    Color bodyAscentColor = Color.FromArgb(0, 192, 0);
			//    Color ascentColor = Color.FromArgb(64, 64, 255);
			//    Color descentColor = Color.FromArgb(255, 0, 255);
			//    using (Graphics g = Graphics.FromImage(resultBitmap))
			//    {
			//        for (int i = 0; i < metrics.LineCount; i++)
			//        {
			//            Rect lineBounds = metrics.LineBounds[i];
			//            g.DrawRectangle(new Pen(Color.FromArgb(128, fgColor)), lineBounds.X + 5, lineBounds.Y, lineBounds.W, lineBounds.H - 1);
			//            g.DrawLine(new Pen(Color.FromArgb(192, baseLineColor)), 0, lineBounds.Y + font.BaseLine, resultBitmap.Width, lineBounds.Y + font.BaseLine);
			//            g.DrawLine(new Pen(Color.FromArgb(192, bodyAscentColor)), 0, lineBounds.Y + font.BaseLine - font.BodyAscent, resultBitmap.Width, lineBounds.Y + font.BaseLine - font.BodyAscent);
			//            g.DrawLine(new Pen(Color.FromArgb(192, ascentColor)), 0, lineBounds.Y + font.BaseLine - font.Ascent, resultBitmap.Width, lineBounds.Y + font.BaseLine - font.Ascent);
			//            g.DrawLine(new Pen(Color.FromArgb(192, descentColor)), 0, lineBounds.Y + font.BaseLine + font.Descent, resultBitmap.Width, lineBounds.Y + font.BaseLine + font.Descent);
			//        }
			//    }
			//}

			query.Result = resultBitmap.Resize(desiredWidth, desiredHeight, Alignment.Left);
		}
	}
}
