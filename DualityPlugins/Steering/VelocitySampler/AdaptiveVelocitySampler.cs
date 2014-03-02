using Duality;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// Samples velocities based on the velocity the agent chose. The sampling
	/// density is higher velocities close to the last best velocity.
	/// This reduces samples needed massively compared to <see cref="BruteForceVelocitySampler"/>
	/// but can potentially lead to undesired behavior.
	/// </summary>
	[Serializable]
	public class AdaptiveVelocitySampler : IVelocitySampler
	{
		private int layerCount				= 5;
		private int outerLayerSampleCount	= 11;

		[NonSerialized] private int currentSampleIdx = 0;

		public void Reset()
		{
			this.currentSampleIdx = 0;
		}
		public Vector2 GetCurrentSample(Agent agent)
		{
			int commonSampleCount = this.layerCount * this.outerLayerSampleCount;
			Vector2 oldVelocity = agent.SuggestedVel / agent.Characteristics.MaxSpeed;
			
			// this can happen if the max speed changed during the last update
			if (oldVelocity.LengthSquared > 1f)
				oldVelocity.Normalize();
			
			if (this.currentSampleIdx >= commonSampleCount + 1)
				return Vector2.Zero;
			if (this.currentSampleIdx >= commonSampleCount)
				return oldVelocity;

			int layerIdx = this.currentSampleIdx % this.layerCount;
			int directionIdx = (this.currentSampleIdx / this.layerCount) % this.outerLayerSampleCount;


			float undistortedSpeedFactor = (float)(layerIdx + 1) / this.layerCount;
			float undistortedAngle = MathF.Lerp(-1.0f, 1.0f, ((float)directionIdx / this.outerLayerSampleCount));

			float speedFactor = undistortedSpeedFactor;
			float angle = MathF.Atan2(oldVelocity.Y, oldVelocity.X) + MathF.Pow(Math.Abs(undistortedAngle), 0.8f) * undistortedAngle * MathF.RadAngle180;
				
			return new Vector2(MathF.Cos(angle) * speedFactor, MathF.Sin(angle) * speedFactor);
		}
		public bool SetCurrentCost(float cost)
		{
			this.currentSampleIdx++;
			if (this.currentSampleIdx <= this.layerCount * this.outerLayerSampleCount)
				return true;
			else
				return false;
		}

		public override string ToString()
		{
			return string.Format("Adaptive, {0} Layers, {1} Samples", this.layerCount, this.outerLayerSampleCount);
		}
	}
}
