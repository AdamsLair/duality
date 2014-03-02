using Duality;
using OpenTK;
using System;

namespace Duality.Plugins.Navigation
{
	/// <summary>
	/// Defines the basic reactive behavior of an Agent
	/// </summary>
	public interface ICharacteristics
	{
		/// <summary>
		/// [GET] The maximum speed of the agent which has this characteristics attached
		/// </summary>
		float MaxSpeed
		{
			get;
		}

		/// <summary>
		/// Calculates the "cost" of a given velocity which are used to decide which velocity an agent should actually
		/// choose. There are multiple things this method needs to consider:
		/// <list type="bullet">
		///		<listheader>
		///			<term>Target</term>	
		///			<description>Where does the agent want to move to?</description>
		///		</listheader>
		///		<listheader>
		///			<term>Speed</term>	
		///			<description>How fast does the agent want to travel?</description>
		///		</listheader>
		///		<listheader>
		///			<term>Time of impact</term>	
		///			<description>There are velocities which will lead to collisions with obstacles in the future</description>
		///		</listheader>
		///		<listheader>
		///			<term>Side</term>	
		///			<description>It's often usful to prefer a "side" on which an agent avoids obstacles because it can help to reduce oscillations</description>
		///		</listheader>
		/// </list>
		/// Based on those the function should calculate the cost for a given velocity. To to this it should somehow combine different weighted scores.
		/// </summary>
		/// <param name="agent">The agent for which the cost should be evaluated</param>
		/// <param name="sampleVelocity">The velocity which should be evaluated</param>
		/// <param name="toiPenality">
		///	Normalized time of impact (between 0 and 1) for the velocity. A value of 0 means we are already colliding 
		///	and a value of 1 means that a collision will occure earliest at the <see cref="Agent.ToiHorizon" />.
		/// </param>
		/// <returns>The cost for the given velocity which should be between 0 and 1</returns>
		float CalculateVelocityCost(Agent agent, Vector2 sampleVelocity, float toiPenality);
	}

	[Serializable]
	public class AdvancedCharacteristics : ICharacteristics
	{
		private float directionFactor = 0.75f;
		private float velocityPreservationFactor = 0f;
		private float prefSpeed = 2f;
		private float speedFactor = 0.9f;
		private float toiFactor = 3f;
		private float toiExponent = 4f;

		public float VelocityPreservationFactor
		{
			get { return velocityPreservationFactor; }
			set { velocityPreservationFactor = value; }
		}
		
		public float DirectionFactor
		{
			get { return directionFactor; }
			set { directionFactor = value; }
		}

		public float SpeedFactor
		{
			get { return speedFactor; }
			set { speedFactor = value; }
		}

		public float ToiFactor
		{
			get { return toiFactor; }
			set { toiFactor = value; }
		}

		public float ToiExponent
		{
			get { return toiExponent; }
			set { toiExponent = value; }
		}

		public float MaxSpeed
		{
			get { return prefSpeed; }
		}

		public float PrefSpeed
		{
			get { return prefSpeed; }
			set { prefSpeed = value; }
		}

		public float CalculateVelocityCost(Agent agent, Vector2 sampleVelocity, float toiPenality)
		{
			float score = 0f;

			var deltaVelocityPenality = (sampleVelocity - agent.GameObj.Transform.Vel.Xy).Length / (2f * MaxSpeed);
			score += velocityPreservationFactor * deltaVelocityPenality;

			if (agent.Target != null)
			{
				if (sampleVelocity.LengthSquared > float.Epsilon)
					score += directionFactor * agent.Target.CalculateCost(agent, sampleVelocity.Normalized);
				score += speedFactor * (prefSpeed - sampleVelocity.Length) / MaxSpeed;
			}
			score += toiFactor * MathF.Pow(toiPenality, toiExponent);

			score /= velocityPreservationFactor + speedFactor + directionFactor + toiFactor;
			return score;
		}
	}

	[Serializable]
	public class DefaultCharacteristics : ICharacteristics
	{
		private const float MIN_TOI_EXPONENT = 0.02f;
		private const float MAX_TOI_EXPONENT = 40f;
		private const float MIN_SPEED_FACTOR = 0.5f;
		private const float MAX_SPEED_FACTOR = 5f;
		private const float MIN_DIRECTION_FACTOR = 0.3f;
		private const float MAX_DIRECTION_FACTOR = 5f;

		[NonSerialized]
		private AdvancedCharacteristics baseImpl = new AdvancedCharacteristics();
		[NonSerialized]
		private bool updateBaseImplParams = true;

		private float aggresivity = 0.5f;

		public DefaultCharacteristics()
		{
			baseImpl.VelocityPreservationFactor = 0f;
		}

		public float MaxSpeed
		{
			get { return baseImpl.MaxSpeed; }
		}

		public float PrefSpeed
		{
			get { return baseImpl.PrefSpeed; }
			set { baseImpl.PrefSpeed = value; }
		}

		public float Aggresivity
		{
			get { return aggresivity; }
			set
			{
				aggresivity = value;
				updateBaseImplParams = true;
			}
		}

		public float CalculateVelocityCost(Agent agent, Vector2 sampleVelocity, float toiPenality)
		{
			if(updateBaseImplParams)
			{
				//baseImpl.DirectionFactor = MathF.Lerp(MIN_DIRECTION_FACTOR, MAX_DIRECTION_FACTOR, aggresivity);
				//baseImpl.SpeedFactor = MathF.Lerp(MIN_SPEED_FACTOR, MAX_SPEED_FACTOR, aggresivity);
				//baseImpl.ToiExponent = MathF.Lerp(MIN_TOI_EXPONENT, MAX_TOI_EXPONENT, aggresivity);
				//baseImpl.ToiExponent = 1f / (0.5f * MathF.Log(1f - MathF.Lerp(aggresivity, 0.0001f, 0.9999f)) / MathF.Log(0.5f));
				baseImpl.ToiExponent = 3f * MathF.Log(1f - MathF.Lerp(0.0001f, 0.9999f, aggresivity)) / MathF.Log(0.5f);

				updateBaseImplParams = false;
			}
			return baseImpl.CalculateVelocityCost(agent, sampleVelocity, toiPenality);
		}
	}
}
