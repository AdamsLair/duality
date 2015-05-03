using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	public static class GameplayHelper
	{
		public static void Shockwave(Vector2 at, float radius, float impulse, float maxVelocity, Predicate<RigidBody> affectsObject)
		{
			// Iterate over all RigidBodies with a line-of-sight to the shockwave center and push them away
			IterateLineOfSightBodies(at, radius, affectsObject, (body, hitData) =>
			{
				float distanceFactor = MathF.Pow(1.0f - hitData.Fraction, 1.5f);
				float maxImpulse = body.Mass * maxVelocity;
				body.ApplyWorldImpulse(-hitData.Normal * MathF.Min(distanceFactor * impulse, maxImpulse), hitData.Pos);
			});
		}
		public static void ExplosionDamage(Vector2 at, float radius, float damage, Predicate<RigidBody> affectsObject)
		{
			// Iterate over all RigidBodies with a line-of-sight to the explosion center and damage them
			IterateLineOfSightBodies(at, radius, affectsObject, (body, hitData) =>
			{
				Ship ship = body.GameObj.GetComponent<Ship>();
				if (ship == null) return;

				float distanceFactor = MathF.Pow(1.0f - hitData.Fraction, 1.5f);
				ship.DoDamage(distanceFactor * damage);
			});
		}

		private delegate void LineOfSightCallback(RigidBody body, RayCastData hitData);
		private static void IterateLineOfSightBodies(Vector2 at, float radius, Predicate<RigidBody> affectsObject, LineOfSightCallback callback)
		{
			// Iterate over all RigidBodies in the area
			List<RigidBody> nearBodies = RigidBody.QueryRectGlobal(at - new Vector2(radius, radius), new Vector2(radius, radius) * 2);
			foreach (RigidBody body in nearBodies)
			{
				if (body.WorldMassCenter == at) continue;
				if (!affectsObject(body)) continue;

				Vector2 bodyPos = body.WorldMassCenter;
				Vector2 bodyDir = (bodyPos - at).Normalized;
				Vector2 maxRadiusPos = at + bodyDir * radius;

				// Perform a raycast to find out whether the current body has a direct line of sight to the center
				RayCastData firstHit;
				bool hitAnything = RigidBody.RayCast(at, maxRadiusPos, d =>
				{
					// Clip the cast ray
					if (d.Body == body)
						return d.Fraction;
					else if (d.Body.BodyType != BodyType.Static || d.Shape.IsSensor || !affectsObject(d.Body))
						return -1.0f;
					else
						return d.Fraction;
				}, out firstHit);

				// If the current body is really the first one to be hit, push it away
				if (hitAnything && firstHit.Body == body)
				{
					callback(body, firstHit);
				}
			}
		}
	}
}
