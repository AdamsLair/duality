using OpenTK;

namespace Duality
{
	public static class PhysicsConvert
	{
		private const float ToDuality = 100.0f;
		private const float ToPhysical = 1.0f / ToDuality;

		public static float ToPhysicalUnit(float dualityUnit)
		{
			return dualityUnit * ToPhysical;
		}
		public static Vector2 ToPhysicalUnit(Vector2 dualityUnit)
		{
			Vector2.Multiply(ref dualityUnit, ToPhysical, out dualityUnit);
			return dualityUnit;
		}

		public static float ToDualityUnit(float physicalUnit)
		{
			return physicalUnit * ToDuality;
		}
		public static Vector2 ToDualityUnit(Vector2 physicalUnit)
		{
			Vector2.Multiply(ref physicalUnit, ToDuality, out physicalUnit);
			return physicalUnit;
		}
	}
}
