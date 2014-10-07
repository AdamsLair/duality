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
	public static class GameplayHelper
	{
		public static void Shockwave(Vector2 at, float radius, float impulse, float maxVelocity, Predicate<RigidBody> affectsObject)
		{
			List<RigidBody> nearBodies = RigidBody.QueryRectGlobal(at - new Vector2(radius, radius), new Vector2(radius, radius) * 2);
			foreach (RigidBody body in nearBodies)
			{
				if (body.WorldMassCenter == at) continue;
				if (!affectsObject(body)) continue;

				Vector2 bodyPos = body.WorldMassCenter;
				Vector2 bodyDir = (bodyPos - at).Normalized;
				Vector2 maxRadiusPos = at + bodyDir * radius;

				List<RayCastData> hitData = RigidBody.RayCast(at, maxRadiusPos, d =>
				{
					if (d.Body == body)
						return d.Fraction;
					else if (d.Shape.IsSensor || !affectsObject(d.Body))
						return -1.0f;
					else
						return d.Fraction;
				});

				if (hitData.Count > 0)
				{
					RayCastData firstHit = hitData[0];
					if (firstHit.Body == body)
					{
						float distanceFactor = MathF.Pow(1.0f - hitData[0].Fraction, 1.5f);
						float maxImpulse = body.Mass * maxVelocity;
						body.ApplyWorldImpulse(bodyDir * MathF.Min(distanceFactor * impulse, maxImpulse), hitData[0].Pos);
					}
				}
			}
		}
		public static void SplashDamage(Vector2 at, float radius, float damage, Predicate<Ship> affectsObject)
		{
			List<RigidBody> nearBodies = RigidBody.QueryRectGlobal(at - new Vector2(radius, radius), new Vector2(radius, radius) * 2);
			foreach (RigidBody body in nearBodies)
			{
				Ship ship = body.GameObj.GetComponent<Ship>();
				if (ship == null) continue;
				if (!affectsObject(ship)) continue;

				Vector2 bodyPos = body.WorldMassCenter;
				float distance = (bodyPos - at).Length;
				if (distance > radius) continue;

				float distanceFactor = 1.0f - MathF.Clamp(distance / radius, 0.0f, 1.0f);
				ship.DoDamage(distanceFactor * damage);
			}
		}
	}
}
