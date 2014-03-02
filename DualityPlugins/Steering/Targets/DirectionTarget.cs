using OpenTK;
using System;

namespace Duality.Plugins.Steering
{
	[Serializable]
	public class DirectionTarget : ISteeringTarget
	{
		private Vector2 direction;

		public Vector2 Direction
		{
			get { return this.direction; }
			set { this.direction = value; }
		}

		public float CalculateCost(Agent agent, Vector2 sampleDirection)
		{
			float dirLen = this.direction.Length;
			return 0.5f * (1.0f - Vector2.Dot(this.direction / (dirLen > 0.0f ? dirLen : 1.0f), sampleDirection));
		}
		public override string ToString()
		{
			return string.Format("Direction: {0}°", MathF.RadToDeg(this.direction.Angle));
		}
	}
}
