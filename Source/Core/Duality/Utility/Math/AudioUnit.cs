namespace Duality
{
	public static class AudioUnit
	{
		/// <summary>
		/// SI unit: radians
		/// </summary>
		public static readonly float AngleToDuality     = 1.0f;
		/// <summary>
		/// SI unit: m
		/// </summary>
		public static readonly float LengthToDuality    = 100.0f;
		/// <summary>
		/// SI unit: s
		/// </summary>
		public static readonly float TimeToDuality      = Time.FramesPerSecond;
		/// <summary>
		/// SI unit: m/s
		/// </summary>
		public static readonly float VelocityToDuality  = LengthToDuality / TimeToDuality;

		public static readonly float AngleToPhysical    = 1.0f / AngleToDuality;
		public static readonly float LengthToPhysical   = 1.0f / LengthToDuality;
		public static readonly float TimeToPhysical     = 1.0f / TimeToDuality;
		public static readonly float VelocityToPhysical = 1.0f / VelocityToDuality;
	}
}
