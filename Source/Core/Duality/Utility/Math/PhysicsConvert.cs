namespace Duality
{
	public static class PhysicsUnit
	{
		/// <summary>
		/// SI unit: radians
		/// </summary>
		public const float AngleToDuality           = 1.0f;
		/// <summary>
		/// SI unit: m
		/// </summary>
		public const float LengthToDuality          = 100.0f;
		/// <summary>
		/// SI unit: kg
		/// </summary>
		public const float MassToDuality            = 100.0f;
		/// <summary>
		/// SI unit: kg / m²
		/// </summary>
		public const float DensityToDuality         = MassToDuality / (LengthToDuality * LengthToDuality);
		/// <summary>
		/// SI unit: s
		/// </summary>
		public const float TimeToDuality            = Time.FramesPerSecond;
		/// <summary>
		/// SI unit: m/s
		/// </summary>
		public const float VelocityToDuality        = LengthToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: radians/s
		/// </summary>
		public const float AngularVelocityToDuality = AngleToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: m/s²
		/// </summary>
		public const float AccelerationToDuality    = VelocityToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: kg * m/s²
		/// </summary>
		public const float ForceToDuality           = MassToDuality * AccelerationToDuality;
		/// <summary>
		/// SI unit: kg * m²/s²
		/// </summary>
		public const float TorqueToDuality          = ForceToDuality * LengthToDuality;
		/// <summary>
		/// SI unit: kg * m/s
		/// </summary>
		public const float ImpulseToDuality         = ForceToDuality * TimeToDuality;
		/// <summary>
		/// SI unit: kg * m²/s
		/// </summary>
		public const float AngularImpulseToDuality  = TorqueToDuality * TimeToDuality;
		/// <summary>
		/// SI unit: kg * m²
		/// </summary>
		public const float InertiaToDuality         = MassToDuality * (LengthToDuality * LengthToDuality);

		public const float AngleToPhysical              = 1.0f / AngleToDuality;
		public const float LengthToPhysical             = 1.0f / LengthToDuality;
		public const float MassToPhysical               = 1.0f / MassToDuality;
		public const float DensityToPhysical            = 1.0f / DensityToDuality;
		public const float TimeToPhysical               = 1.0f / TimeToDuality;
		public const float VelocityToPhysical           = 1.0f / VelocityToDuality;
		public const float AngularVelocityToPhysical    = 1.0f / AngularVelocityToDuality;
		public const float AccelerationToPhysical       = 1.0f / AccelerationToDuality;
		public const float ForceToPhysical              = 1.0f / ForceToDuality;
		public const float TorqueToPhysical             = 1.0f / TorqueToDuality;
		public const float ImpulseToPhysical            = 1.0f / ImpulseToDuality;
		public const float AngularImpulseToPhysical     = 1.0f / AngularImpulseToDuality;
		public const float InertiaToPhysical            = 1.0f / InertiaToDuality;
	}
}
