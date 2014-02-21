using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Duality;

namespace Duality.Editor
{
	public static class ExtMethodsGraphics
	{
		public static void DrawImageAlpha(this Graphics g, Image img, float alpha, Rectangle rect)
		{
			ImageAttributes attrib = new ImageAttributes();
			ColorMatrix matrix = new ColorMatrix(new[] {
				new float[] {	1.0f,	0.0f,	0.0f,	0.0f,	0.0f	},
				new float[] {	0.0f,	1.0f,	0.0f,	0.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	1.0f,	0.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	alpha,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	0.0f,	1.0f	}});
			attrib.SetColorMatrix(matrix);
			g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attrib);
		}
		public static void DrawImageAlpha(this Graphics g, Image img, float alpha, int x, int y)
		{
			DrawImageAlpha(g, img, alpha, new Rectangle(x, y, img.Width, img.Height));
		}
		public static void DrawImageMonochrome(this Graphics g, Image img, Rectangle rect)
		{
			ImageAttributes attrib = new ImageAttributes();
			ColorMatrix matrix = new ColorMatrix(new[] {
				new float[] {	0.3f,	0.3f,	0.3f,	0.0f,	0.0f	},
				new float[] {	0.59f,	0.59f,	0.59f,	0.0f,	0.0f	},
				new float[] {	0.11f,	0.11f,	0.11f,	0.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	1.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	0.0f,	1.0f	}});
			attrib.SetColorMatrix(matrix);
			g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attrib);
		}
		public static void DrawImageMonochrome(this Graphics g, Image img, int x, int y)
		{
			DrawImageMonochrome(g, img, new Rectangle(x, y, img.Width, img.Height));
		}
	}
}
