using Duality;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// Creates velocity samples which are going to get tested with <see cref="IAgentCharacteristics"/>. 
	/// If the samples are poorly chosen or if there are simply not enough samples the agent won't be able to
	/// choose "good" velocities which lead to a bad steering quality. If on the other hand to many samples are
	/// generated the performance will suffer because for every sample the agent needs to calculate time of imapacts
	/// with obstacles.
	/// </summary>
	public interface IVelocitySampler
	{
		/// <summary>
		/// This method is called in every time step for every agent before the sampling starts.
		/// If your implementation is adaptive you should throw away your old state here and start over.
		/// </summary>
		void Reset();
		/// <summary>
		/// Get the current sample velocity. The implementation is free to use internal information gathered from
		/// previous calls to <see cref="IVelocitySampler.SetCurrentCost"/>. You should make sure that your implementation
		/// samples the zero-velocity.
		/// </summary>
		/// <returns>Velocity which should be evaluated</returns>
		Vector2 GetCurrentSample(Agent agent);
		/// <summary>
		/// Feeds the evaluated cost back into the sampler. The cost value can be used to adapt and intelligent choose the next
		/// velocities.
		/// </summary>
		/// <param name="cost">The cost which was returned from <see cref="IAgentCharacteristics.CalculateVelocityCost"/> 
		/// with the current velocity as parameter
		/// </param>
		/// <returns>
		/// <code>true</code> if more velocities should be sampled and <code>false</code> if 
		/// no new velocities should be sampled.
		/// </returns>
		bool SetCurrentCost(float cost);
	}
}
