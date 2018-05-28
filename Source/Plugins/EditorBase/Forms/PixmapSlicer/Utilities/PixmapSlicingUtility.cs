using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base
{
	public static class ExtMethodsPixmapSlicerRectSide
	{
		public static PixmapSlicingUtility.Side Opposite(this PixmapSlicingUtility.Side side)
		{
			switch (side)
			{
				case PixmapSlicingUtility.Side.Left:	return PixmapSlicingUtility.Side.Right;
				case PixmapSlicingUtility.Side.Right:	return PixmapSlicingUtility.Side.Left;
				case PixmapSlicingUtility.Side.Top:		return PixmapSlicingUtility.Side.Bottom;
				case PixmapSlicingUtility.Side.Bottom:	return PixmapSlicingUtility.Side.Top;
				default: return PixmapSlicingUtility.Side.None;
			}
		}
	}

	public static class PixmapSlicingUtility
	{
		public enum Side
		{
			None, Left, Right, Top, Bottom
		}

		/// <summary>
		/// Returns whether or not the given coordinates are within <paramref name="range"/> of the rect.
		/// Also outputs which side the coordinates are closest to when the points are within range.
		/// </summary>
		public static bool WithinRangeToBorder(this Rect rect, float x, float y, float range, out Side side)
		{
			float doubleRange = range * 2;
			Rect smaller = rect.Scaled((rect.W - doubleRange) / rect.W, (rect.H - doubleRange) / rect.H).WithOffset(range, range);
			Rect larger = rect.Scaled((rect.W + doubleRange) / rect.W, (rect.H + doubleRange) / rect.H).WithOffset(-range, -range);

			bool borderAreaContains = !smaller.Contains(x, y) && larger.Contains(x, y);

			if (!borderAreaContains)
			{
				side = Side.None;
				return false;
			}

			float dLeft = MathF.Abs(rect.X - x);
			float dRight = MathF.Abs(rect.RightX - x);
			float minH = MathF.Min(dLeft, dRight);
			Side sideH = dLeft < dRight ? Side.Left : Side.Right;

			float dTop = MathF.Abs(rect.Y - y);
			float dBottom = MathF.Abs(rect.BottomY - y);
			float minV = MathF.Min(dTop, dBottom);
			Side sideV = dTop < dBottom ? Side.Top : Side.Bottom;

			side = minH < minV ? sideH : sideV;
			return true;

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
