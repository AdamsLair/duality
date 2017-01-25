using System.Text;

namespace Duality
{
	public static class ExtMethodsTargetResize
	{
		/// <summary>
		/// Resizes rect boundaries to match the specified target size.
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="baseSize"></param>
		/// <param name="targetSize"></param>
		/// <returns></returns>
		public static Vector2 Apply(this TargetResize mode, Vector2 baseSize, Vector2 targetSize)
		{
			Vector2 sizeRatio;
			float scale;
			switch (mode)
			{
				default:
				case TargetResize.None:
					return baseSize;
				case TargetResize.Stretch:
					return targetSize;
				case TargetResize.Fit:
					if (baseSize == Vector2.Zero)
						baseSize = Vector2.One;
					sizeRatio = targetSize / baseSize;
					if (sizeRatio.Y < sizeRatio.X)
						scale = sizeRatio.Y;
					else
						scale = sizeRatio.X;
					return baseSize * scale;
				case TargetResize.Fill:
					if (baseSize == Vector2.Zero)
						baseSize = Vector2.One;
					sizeRatio = targetSize / baseSize;
					if (sizeRatio.Y > sizeRatio.X)
						scale = sizeRatio.Y;
					else
						scale = sizeRatio.X;
					return baseSize * scale;
			}
		}
	}
}
