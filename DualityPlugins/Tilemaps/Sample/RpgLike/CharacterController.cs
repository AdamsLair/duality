using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;
using Duality.Plugins.Tilemaps.Sample.RpgLike.Properties;

namespace Duality.Plugins.Tilemaps.Sample.RpgLike
{
	/// <summary>
	/// Applies "retro RPG"-like character movement based on a physical model.
	/// </summary>
	[RequiredComponent(typeof(RigidBody))]
	[EditorHintCategory(SampleResNames.CategoryRpgLike)]
	[EditorHintImage(TilemapsResNames.ImageActorRenderer)]
	public class CharacterController : Component, ICmpUpdatable
	{
		private float   speed           = 1.0f;
		private float   acceleration    = 0.2f;
		private float   moveSenseRadius = 96.0f;
		private Vector2 targetMovement  = Vector2.Zero;

		public float Speed
		{
			get { return this.speed; }
			set { this.speed = value; }
		}
		public float Acceleration
		{
			get { return this.acceleration; }
			set { this.acceleration = value; }
		}
		public float MovementSensingRadius
		{
			get { return this.moveSenseRadius; }
			set { this.moveSenseRadius = value; }
		}
		public Vector2 TargetMovement
		{
			get { return this.targetMovement; }
			set { this.targetMovement = value; }
		}

		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.GetComponent<RigidBody>();

			Vector2 adjustedTargetMovement = this.targetMovement / MathF.Max(1.0f, this.targetMovement.Length);;
			if (adjustedTargetMovement.Length > 0.01f && this.moveSenseRadius > 0.0f)
			{
				Vector2 targetDir = this.targetMovement.Normalized;
				float senseRadius = this.moveSenseRadius * adjustedTargetMovement.Length;

				RayCastData firstHit;
				Vector2 rayStart = transform.Pos.Xy;
				Vector2 rayEnd = rayStart + targetDir * senseRadius;
				bool hitAnything = RigidBody.RayCast(rayStart, rayEnd, data => data.Fraction, out firstHit);

				if (hitAnything)
				{
					float movementRatio = 0.1f + 0.9f * ((firstHit.Pos - rayStart).Length / (rayEnd - rayStart).Length);
					adjustedTargetMovement *= movementRatio;
				}

				VisualLog.Default
					.DrawConnection(rayStart.X, rayStart.Y, 0.0f, hitAnything ? firstHit.Pos.X : rayEnd.X, hitAnything ? firstHit.Pos.Y : rayEnd.Y)
					.WithOffset(-100);
			}

			Vector2 targetVelocity = adjustedTargetMovement * this.speed;
			Vector2 appliedForce = (targetVelocity - body.LinearVelocity) * body.Mass * this.acceleration;
			body.ApplyLocalForce(appliedForce);
		}
	}
}
