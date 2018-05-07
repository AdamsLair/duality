using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer.States;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.Utilities
{
	public static class Utilities
	{
		/// <summary>
		/// Returns the distance the given (x,y) coordinates are from this <see cref="Rect"/>.
		/// Additionally outputs the side of the rectangle that the given coordinate is closest to.
		/// </summary>
		public static float DistanceToBorder(this Rect rect, float x, float y, out Side side)
		{
			float dLeft = MathF.Abs(rect.X - x);
			float dRight = MathF.Abs(rect.RightX - x);
			float minH = MathF.Min(dLeft, dRight);
			Side sideH = dLeft < dRight ? Side.Left : Side.Right;

			float dTop = MathF.Abs(rect.Y - y);
			float dBottom = MathF.Abs(rect.BottomY - y);
			float minV = MathF.Min(dTop, dBottom);
			Side sideV = dTop < dBottom ? Side.Top : Side.Bottom;

			if (minH < minV)
			{
				side = sideH;
				return minH;
			}
			else
			{
				side = sideV;
				return minV;
			}
		}

		/// <summary>
		/// Find all atlast rectangles in the given <see cref="Pixmap"/>
		/// </summary>
		/// <param name="alpha">Pixels whose alpha is below this value will be considered transparent</param>
		/// <param name="minSize">Rectangles with width or higher smaller than this will be ignored</param>
		public static IEnumerable<Rect> FindRects(Pixmap pixmap, byte alpha = 0, int minSize = 2)
		{
			List<Rect> rects = new List<Rect>();

			for (int i = 0; i < pixmap.Width; i++)
			{
				for (int j = 0; j < pixmap.Height; j++)
				{
					if (pixmap.PixelData[0][i, j].A <= alpha)
						continue;

					// If this pixel is contained in a previously found rect,
					// skip to the bottom of that rect
					Rect rect = rects.FirstOrDefault(r => r.Contains(i, j));
					if (rect != default(Rect))
					{
						j = (int)MathF.Ceiling(rect.BottomY);
						continue;
					}

					rect = FindRect(pixmap, i, j, alpha);

					// Add if the rect is large enough
					if (rect.W > minSize && rect.H > minSize)
						rects.Add(rect);
				}
			}

			return rects;
		}

		/// <summary>
		/// Find an atlas rect containing the given pixel
		/// </summary>
		public static Rect FindRect(Pixmap pixmap, int startX, int startY, byte alpha = 0)
		{
			int left = startX;
			int top = startY;
			int right = startX + 1;
			int bottom = startY + 1;

			Rect r = new Rect(left, top, right - left, top - bottom);
			while (true)
			{
				// Try to enlarge the rect
				ScanLeft(pixmap, ref left, top, bottom, alpha);
				ScanRight(pixmap, ref right, top, bottom, alpha);
				ScanUp(pixmap, ref top, left, right, alpha);
				ScanDown(pixmap, ref bottom, left, right, alpha);

				// If the rect hasn't changed, we are done
				Rect newRect = new Rect(left, top, (right - left), (bottom - top));
				if (newRect.Equals(r))
					break;

				r = newRect;
			}

			return r;
		}

		private static void ScanLeft(Pixmap pixmap, ref int left, int yMin, int yMax, byte alpha)
		{
			int y;
			while (!IsVerticalLineTransparent(pixmap, left, yMin, yMax, out y, alpha))
			{
				while (left >= 0 && pixmap.PixelData[0][left, y].A > alpha) left--;
				if (left <= 0)
				{
					left = 0;
					break;
				}
			}
		}

		private static void ScanRight(Pixmap pixmap, ref int right, int yMin, int yMax, byte alpha)
		{
			int y;
			while (!IsVerticalLineTransparent(pixmap, right, yMin, yMax, out y, alpha))
			{
				while (right < pixmap.Width && pixmap.PixelData[0][right, y].A > alpha) right++;
				if (right >= pixmap.Width)
				{
					right = pixmap.Width - 1;
					break;
				}
			}
		}

		private static void ScanUp(Pixmap pixmap, ref int top, int xMin, int xMax, byte alpha)
		{
			int x;
			while (!IsHorizontalLineTransparent(pixmap, top, xMin, xMax, out x, alpha))
			{
				while (top >= 0 && pixmap.PixelData[0][x, top].A > alpha) top--;
				if (top <= 0)
				{
					top = 0;
					break;
				}
			}
		}

		private static void ScanDown(Pixmap pixmap, ref int bottom, int xMin, int xMax, byte alpha)
		{
			int x;
			while (!IsHorizontalLineTransparent(pixmap, bottom, xMin, xMax, out x, alpha))
			{
				while (bottom < pixmap.Height && pixmap.PixelData[0][x, bottom].A > alpha) bottom++;
				if (bottom >= pixmap.Height)
				{
					bottom = pixmap.Height - 1;
					break;
				}
			}
		}

		private static bool IsVerticalLineTransparent(Pixmap image, int x, int yMin, int yMax, out int failingY, byte alpha)
		{
			for (int y = yMin; y <= yMax; y++)
			{
				if (image.PixelData[0][x, y].A > alpha)
				{
					failingY = y;
					return false;
				}
			}

			failingY = -1;
			return true;
		}

		private static bool IsHorizontalLineTransparent(Pixmap image, int y, int xMin, int xMax, out int failingX, byte alpha)
		{
			for (int x = xMin; x <= xMax; x++)
			{
				if (image.PixelData[0][x, y].A > alpha)
				{
					failingX = x;
					return false;
				}
			}

			failingX = -1;
			return true;
		}
	}
}
