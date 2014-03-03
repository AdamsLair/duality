using System;

using Duality;
using Duality.Editor;

using OpenTK;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// Defines the basic reactive behavior of an Agent
	/// </summary>
	public interface IAgentCharacteristics
	{
		/// <summary>
		/// [GET / SET] The preferred speed of the agent.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		float PreferredSpeed { get; set; }
		/// <summary>
		/// [GET] The maximum speed of the agent.
		/// </summary>
		[EditorHintFlags(MemberFlags.Invisible)]
		float MaxSpeed { get; }

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
}
