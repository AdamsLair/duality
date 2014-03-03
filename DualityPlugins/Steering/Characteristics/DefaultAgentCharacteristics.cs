using System;

using Duality;
using Duality.Editor;

using OpenTK;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class DefaultAgentCharacteristics : IAgentCharacteristics
	{
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
				// [aggressiveness]
				// interpretation: aggressive agents are likely to ignore other agents more => avoid them only if absolutly necessary
				// implementation: 
				// we use a function which is
				// - zero if aggressiveness == 0
				// - one if aggressiveness == 0.5
				// - infinity if aggressiveness == 1 [to prevent numerical problem we don't actually use infinity but just a high value]
				// ... and use it as toi exponent. This will lead to a toi-cost-function which is very steep close to one 
				// => the agent will avoid other agents in the last moment
				// => if aggressiveness is 0 the opposit happens - the agent avoids other agents very early
				// the function we use to calculate is basically: c * log(1 - x) / log(0.5) | where x is the aggressiveness-factor
				this.baseImpl.ToiExponent = AdvancedAgentCharacteristics.DEFAULT_TOI_EXPONENT * MathF.Log(1.0f - MathF.Lerp(0.0001f, 0.9999f, this.aggressiveness)) / MathF.Log(0.5f);

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
