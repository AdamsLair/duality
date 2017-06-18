namespace Duality
{
	public static class PhysicsUnit
	{
		/// <summary>
		/// SI unit: radians
		/// </summary>
		public static readonly float AngleToDuality           = 1.0f;
		/// <summary>
		/// SI unit: m
		/// </summary>
		public static readonly float LengthToDuality          = 100.0f;
		/// <summary>
		/// SI unit: kg
		/// </summary>
		public static readonly float MassToDuality            = 100.0f;
		/// <summary>
		/// SI unit: kg / m²
		/// </summary>
		public static readonly float DensityToDuality         = 1.0f; // Legacy support: Expressed as kg / m². Should actually be: "MassToDuality / (LengthToDuality * LengthToDuality);"
		/// <summary>
		/// SI unit: s
		/// </summary>
		public static readonly float TimeToDuality            = Time.FramesPerSecond;
		/// <summary>
		/// SI unit: m/s
		/// </summary>
		public static readonly float VelocityToDuality        = LengthToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: radians/s
		/// </summary>
		public static readonly float AngularVelocityToDuality = AngleToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: m/s²
		/// </summary>
		public static readonly float AccelerationToDuality    = VelocityToDuality / TimeToDuality;
		/// <summary>
		/// SI unit: kg * m/s²
		/// </summary>
		public static readonly float ForceToDuality           = MassToDuality * AccelerationToDuality;
		/// <summary>
		/// SI unit: kg * m²/s²
		/// </summary>
		public static readonly float TorqueToDuality          = ForceToDuality * LengthToDuality;
		/// <summary>
		/// SI unit: kg * m/s
		/// </summary>
		public static readonly float ImpulseToDuality         = ForceToDuality * TimeToDuality;
		/// <summary>
		/// SI unit: kg * m²/s
		/// </summary>
		public static readonly float AngularImpulseToDuality  = TorqueToDuality * TimeToDuality;
		/// <summary>
		/// SI unit: kg * m²
		/// </summary>
		public static readonly float InertiaToDuality         = MassToDuality * (LengthToDuality * LengthToDuality);

		public static readonly float AngleToPhysical              = 1.0f / AngleToDuality;
		public static readonly float LengthToPhysical             = 1.0f / LengthToDuality;
		public static readonly float MassToPhysical               = 1.0f / MassToDuality;
		public static readonly float DensityToPhysical            = 1.0f / DensityToDuality;
		public static readonly float TimeToPhysical               = 1.0f / TimeToDuality;
		public static readonly float VelocityToPhysical           = 1.0f / VelocityToDuality;
		public static readonly float AngularVelocityToPhysical    = 1.0f / AngularVelocityToDuality;
		public static readonly float AccelerationToPhysical       = 1.0f / AccelerationToDuality;
		public static readonly float ForceToPhysical              = 1.0f / ForceToDuality;
		public static readonly float TorqueToPhysical             = 1.0f / TorqueToDuality;
		public static readonly float ImpulseToPhysical            = 1.0f / ImpulseToDuality;
		public static readonly float AngularImpulseToPhysical     = 1.0f / AngularImpulseToDuality;
		public static readonly float InertiaToPhysical            = 1.0f / InertiaToDuality;
	}
}
