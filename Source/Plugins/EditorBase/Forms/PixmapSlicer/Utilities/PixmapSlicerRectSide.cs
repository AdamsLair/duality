namespace Duality.Editor.Plugins.Base
{
	public enum PixmapSlicerRectSide
	{
		None, Left, Right, Top, Bottom
	}

	public static class ExtMethodsPixmapSlicerRectSide
	{
		public static PixmapSlicerRectSide Opposite(this PixmapSlicerRectSide side)
		{
			switch (side)
			{
				case PixmapSlicerRectSide.Left: return PixmapSlicerRectSide.Right;
				case PixmapSlicerRectSide.Right: return PixmapSlicerRectSide.Left;
				case PixmapSlicerRectSide.Top: return PixmapSlicerRectSide.Bottom;
				case PixmapSlicerRectSide.Bottom: return PixmapSlicerRectSide.Top;
				default: return PixmapSlicerRectSide.None;
			}
		}
	}
}