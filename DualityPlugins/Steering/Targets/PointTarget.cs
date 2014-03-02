using OpenTK;
using System;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class PointTarget : ISteeringTarget
	{
		private Vector2 location;

		public Vector2 Location
		{
			get { return this.location; }
			set { this.location = value; }
		}

		public float CalculateCost(Agent agent, Vector2 sampleDirection)
		{
			var agentPos = agent.GameObj.Transform.Pos.Xy;
			var posDelta = this.location - agentPos;
			return 0.5f * (1f - Vector2.Dot(posDelta.Normalized, sampleDirection));
		}
		public override string ToString()
		{
			return string.Format("Point: {0}, {1}", (int)this.location.X, (int)this.location.Y);
		}
	}
}
