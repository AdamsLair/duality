using System;

using Duality;
using Duality.Editor;

using OpenTK;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class DefaultAgentCharacteristics : IAgentCharacteristics
	{
		private const float MinToiExponent		= 0.02f;
		private const float MaxToiExponent		= 40.0f;
		private const float MinSpeedFactor		= 0.5f;
		private const float MaxSpeedFactor		= 5.0f;
		private const float MinDirectionFactor	= 0.3f;
		private const float MaxDirectionFactor	= 5.0f;

		private float aggressiveness = 0.5f;

		[NonSerialized] private AdvancedAgentCharacteristics baseImpl = new AdvancedAgentCharacteristics();
		[NonSerialized] private bool updateBaseImplParams = true;


		public float MaxSpeed
		{
			get { return this.baseImpl.MaxSpeed; }
		}
		public float PreferredSpeed
		{
			get { return this.baseImpl.PreferredSpeed; }
			set { this.baseImpl.PreferredSpeed = value; }
		}
		[EditorHintRange(0.0f, 1.0f)]
		public float Aggressiveness
		{
			get { return this.aggressiveness; }
			set
			{
				this.aggressiveness = value;
				this.updateBaseImplParams = true;
			}
		}

		public float CalculateVelocityCost(Agent agent, Vector2 sampleVelocity, float toiPenality)
		{
			if (this.updateBaseImplParams)
			{
				//this.baseImpl.DirectionFactor = MathF.Lerp(MinDirectionFactor, MaxDirectionFactor, this.aggressiveness);
				//this.baseImpl.SpeedFactor = MathF.Lerp(MinSpeedFactor, MaxSpeedFactor, this.aggressiveness);
				//this.baseImpl.ToiExponent = MathF.Lerp(MinToiExponent, MaxToiExponent, this.aggressiveness);
				//this.baseImpl.ToiExponent = 1f / (0.5f * MathF.Log(1f - MathF.Lerp(this.aggressiveness, 0.0001f, 0.9999f)) / MathF.Log(0.5f));
				this.baseImpl.ToiExponent = 3.0f * MathF.Log(1.0f - MathF.Lerp(0.0001f, 0.9999f, this.aggressiveness)) / MathF.Log(0.5f);

				this.updateBaseImplParams = false;
			}
			return this.baseImpl.CalculateVelocityCost(agent, sampleVelocity, toiPenality);
		}

		public override string ToString()
		{
			return string.Format("Default Characteristics");
		}
	}
}
