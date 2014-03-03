using Duality;
using OpenTK;
using System;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class AdvancedAgentCharacteristics : IAgentCharacteristics
	{
		public const float DEFAULT_DIRECTION_FACTOR = 0.75f;
		public const float DEFAULT_VELOCITY_PRESERVATION_FACTOR = 0f;
		public const float DEFAULT_SPEED_FACTOR = 0.9f;
		public const float DEFAULT_TOI_FACTOR = 3f;
		public const float DEFAULT_TOI_EXPONENT = 4f;

		private float directionFactor = DEFAULT_DIRECTION_FACTOR;
		private float velocityPreservationFactor = DEFAULT_VELOCITY_PRESERVATION_FACTOR;
		private float prefSpeed	= 2.0f;
		private float speedFactor = DEFAULT_SPEED_FACTOR;
		private float toiFactor = DEFAULT_TOI_FACTOR;
		private float toiExponent = DEFAULT_TOI_EXPONENT;

		public float VelocityPreservationFactor
		{
			get { return this.velocityPreservationFactor; }
			set { this.velocityPreservationFactor = value; }
		}
		public float DirectionFactor
		{
			get { return this.directionFactor; }
			set { this.directionFactor = value; }
		}
		public float SpeedFactor
		{
			get { return this.speedFactor; }
			set { this.speedFactor = value; }
		}
		public float ToiFactor
		{
			get { return this.toiFactor; }
			set { this.toiFactor = value; }
		}
		public float ToiExponent
		{
			get { return this.toiExponent; }
			set { this.toiExponent = value; }
		}
		public float MaxSpeed
		{
			get { return this.prefSpeed; }
		}
		public float PreferredSpeed
		{
			get { return this.prefSpeed; }
			set { this.prefSpeed = value; }
		}

		public float CalculateVelocityCost(Agent agent, Vector2 sampleVelocity, float toiPenality)
		{
			float score = 0.0f;

			float deltaVelocityPenality = (sampleVelocity - agent.GameObj.Transform.Vel.Xy).Length / (2.0f * this.MaxSpeed);
			score += this.velocityPreservationFactor * deltaVelocityPenality;

			if (agent.Target != null)
			{
				if (sampleVelocity.LengthSquared > float.Epsilon)
					score += this.directionFactor * agent.Target.CalculateCost(agent, sampleVelocity.Normalized);
				score += this.speedFactor * (this.prefSpeed - sampleVelocity.Length) / this.MaxSpeed;
			}
			score += this.toiFactor * MathF.Pow(toiPenality, this.toiExponent);

			score /= this.velocityPreservationFactor + this.speedFactor + this.directionFactor + this.toiFactor;
			return score;
		}

		public override string ToString()
		{
			return string.Format("Advanced Characteristics");
		}
	}
}
