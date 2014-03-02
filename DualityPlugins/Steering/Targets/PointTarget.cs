using OpenTK;
using System;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class PointTarget : ISteeringTarget
	{
		private Vector2 point;

		public Vector2 Point
		{
			get { return this.point; }
			set { this.point = value; }
		}

		public float CalculateCost(Agent agent, Vector2 sampleDirection)
		{
			var agentPos = agent.GameObj.Transform.Pos.Xy;
			var posDelta = this.point - agentPos;
			return 0.5f * (1f - Vector2.Dot(posDelta.Normalized, sampleDirection));
		}
	}
}
