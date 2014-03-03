using System;

using Duality;
using Duality.Editor;

using OpenTK;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// Implementation of <see cref="IAgentCharacteristics"/> which offers parameters
	/// which are intuitve for end users. The purpose of this implementation is to 
	/// hide implementation details as much as possible. If you need more control you can
	/// either use <see cref="AdvancedAgentCharacteristics"/> or use a custom implementation.
	/// </summary>
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
				this.baseImpl.ToiExponent = AdvancedAgentCharacteristics.DEFAULT_TOI_EXPONENT * (1f / (1f - MathF.Lerp(0.01f, 0.99f, this.aggressiveness)) - 1f);

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
