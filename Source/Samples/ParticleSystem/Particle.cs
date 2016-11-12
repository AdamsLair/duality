using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Drawing;

namespace ParticleSystem
{
	public struct Particle
	{
		public Vector3   Position;
		public Vector3   Velocity;
		public float     Angle;
		public float     AngleVelocity;
		public float     TimeToLive;
		public float     AgeFactor;
		public int       SpriteIndex;
		public ColorRgba Color;
	}
}
