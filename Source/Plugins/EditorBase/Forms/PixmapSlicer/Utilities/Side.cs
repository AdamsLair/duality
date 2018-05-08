namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer.Utilities
{
	public enum Side
	{
		None, Left, Right, Top, Bottom
	}

	public static class SideExtensions
	{
		public static Side Opposite(this Side side)
		{
			switch (side)
			{
				case Side.Left: return Side.Right;
				case Side.Right: return Side.Left;
				case Side.Top: return Side.Bottom;
				case Side.Bottom: return Side.Top;
				default: return Side.None;
			}
		}
	}
}