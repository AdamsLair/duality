using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Drawing;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	public static class PhysicsHelper
	{
		public static void Shockwave(Vector2 at, float radius, float impulse, float maxVelocity, Predicate<RigidBody> affectsBody)
		{
			List<RigidBody> bodiesInRadius = RigidBody.QueryRectGlobal(at - new Vector2(radius, radius), new Vector2(radius, radius) * 2);
			foreach (RigidBody body in bodiesInRadius)
			{
				if (body.WorldMassCenter == at) continue;
				if (!affectsBody(body)) continue;

				Vector2 bodyPos = body.WorldMassCenter;
				Vector2 bodyDir = (bodyPos - at).Normalized;
				Vector2 maxRadiusPos = at + bodyDir * radius;

				List<RayCastData> hitData = RigidBody.RayCast(at, maxRadiusPos, d =>
				{
					if (d.Body == body)
						return 0.0f;
					else if (d.Shape.IsSensor || !affectsBody(d.Body))
						return -1.0f;
					else
						return d.Fraction;
				});

				if (hitData.Count > 0)
				{
					RayCastData firstHit = hitData[0];
					if (firstHit.Body == body)
					{
						float distanceFactor = 1.0f - hitData[0].Fraction;
						float maxImpulse = body.Mass * maxVelocity;
						body.ApplyWorldImpulse(bodyDir * MathF.Min(distanceFactor * impulse, maxImpulse), hitData[0].Pos);
					}
				}
			}
		}
	}
}
