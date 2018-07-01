using System.Collections.Generic;
using System.Linq;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base
{
	public enum PixmapSlicingRectSide
	{
		None,

		Left,
		Right,
		Top,
		Bottom
	}

	public static class ExtMethodsPixmapSlicingRectSide
	{
		public static PixmapSlicingRectSide Opposite(this PixmapSlicingRectSide side)
		{
			switch (side)
			{
				case PixmapSlicingRectSide.Left:	return PixmapSlicingRectSide.Right;
				case PixmapSlicingRectSide.Right:	return PixmapSlicingRectSide.Left;
				case PixmapSlicingRectSide.Top:		return PixmapSlicingRectSide.Bottom;
				case PixmapSlicingRectSide.Bottom:	return PixmapSlicingRectSide.Top;
				default: return PixmapSlicingRectSide.None;
			}
		}
	}
}
