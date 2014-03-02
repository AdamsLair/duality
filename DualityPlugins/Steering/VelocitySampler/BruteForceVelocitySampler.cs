using Duality;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Duality.Plugins.Steering
{
	/// <summary>
	/// Simple brute force implementation of <see cref="IVelocitySampler"/>. Velocities are equally distributed in all directions
	/// independent of the costs which are fed back.
	/// </summary>
	[Serializable]
	public class BruteForceVelocitySampler : IVelocitySampler
	{
		private int layerCount				= 3;
		private int outerLayerSampleCount	= 128;
		[NonSerialized] private int currentSampleIdx = 0;

		public void Reset()
		{
			this.currentSampleIdx = 0;
		}
		public Vector2 GetCurrentSample(Agent agent)
		{
			if (this.currentSampleIdx >= this.layerCount * this.outerLayerSampleCount)
				return Vector2.Zero;

			var layerIdx = this.currentSampleIdx % layerCount;
			var directionIdx = (this.currentSampleIdx / this.layerCount) % this.outerLayerSampleCount;

			float angle = ((float)directionIdx / this.outerLayerSampleCount) * MathF.RadAngle360;
			float speedFactor = (float)(layerIdx + 1) / this.layerCount;
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
			return string.Format("BruteForce, {0} Layers, {1} Samples", this.layerCount, this.outerLayerSampleCount);
		}
	}
}
