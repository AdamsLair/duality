using OpenTK;
using System;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// This interface should but doesn't need to be used by implementations of <see cref="IAgentCharacteristics"/>.
	/// It defines which directions of velocities an agent preferes aka in which direction is the target of the agent
	/// </summary>
	public interface ISteeringTarget
	{
		/// <summary>
		/// Evaluates the cost function for a given velocity direction. 
		/// </summary>
		/// <param name="agent">The agent for which the cost should be evaluated</param>
		/// <param name="sampleDirection">
		///	The direction for which the cost should be evaluated
		///	This NOT the velocity but only the direction (vector is normalized) of it.
		/// </param>
		/// <returns>Cost for the given velocity which should be between 0 and 1</returns>
		float CalculateCost(Agent agent, Vector2 sampleDirection);
	}
}
