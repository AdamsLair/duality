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
	}
}
