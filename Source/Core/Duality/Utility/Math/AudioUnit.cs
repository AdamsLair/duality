namespace Duality
{
	public static class AudioUnit
	{
		/// <summary>
		/// SI unit: radians
		/// </summary>
		public const float AngleToDuality     = 1.0f;
		/// <summary>
		/// SI unit: m
		/// </summary>
		public const float LengthToDuality    = 100.0f;
		/// <summary>
		/// SI unit: s
		/// </summary>
		public const float TimeToDuality      = Time.FPSMult;
		/// <summary>
		/// SI unit: m/s
		/// </summary>
		public const float VelocityToDuality  = LengthToDuality / TimeToDuality;

		public const float AngleToPhysical    = 1.0f / AngleToDuality;
		public const float LengthToPhysical   = 1.0f / LengthToDuality;
		public const float TimeToPhysical     = 1.0f / TimeToDuality;
		public const float VelocityToPhysical = 1.0f / VelocityToDuality;
	}
}
