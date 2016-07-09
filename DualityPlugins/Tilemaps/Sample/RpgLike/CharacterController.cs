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

			// We don't want our characters to run head first into a wall at full force,
			// just because the player demands it. That's just silly. So instead, we'll
			// do a physics raycast in the movement direction and gradually slow down based
			// on how far away the obstacle is.
			Vector2 adjustedTargetMovement = this.targetMovement / MathF.Max(1.0f, this.targetMovement.Length);;
			if (adjustedTargetMovement.Length > 0.01f && this.moveSenseRadius > 0.0f)
			{
				Vector2 targetDir = this.targetMovement.Normalized;
				float senseRadius = this.moveSenseRadius * adjustedTargetMovement.Length;

				RayCastData firstHit;
				Vector2 rayStart = transform.Pos.Xy;
				Vector2 rayEnd = rayStart + targetDir * senseRadius;
				bool hitAnything = RigidBody.RayCast(rayStart, rayEnd, data => data.Fraction, out firstHit);
				bool isWorldGeometry = firstHit.Body != null && firstHit.Body.BodyType == BodyType.Static;

				if (hitAnything && isWorldGeometry)
				{
					float movementRatio = 0.1f + 0.9f * ((firstHit.Pos - rayStart).Length / (rayEnd - rayStart).Length);
					adjustedTargetMovement *= movementRatio;
				}
			}

			// Determine how fast we want to be and apply a force to reach the target velocity
			Vector2 targetVelocity = adjustedTargetMovement * this.speed;
			Vector2 appliedForce = (targetVelocity - body.LinearVelocity) * body.Mass * this.acceleration;
			body.ApplyLocalForce(appliedForce);

			// Do we have an animated actor? Let it know how to animate!
			ActorAnimator animator = this.GameObj.GetComponent<ActorAnimator>();
			if (animator != null)
			{
				if (targetVelocity.Length > 0.01f)
				{
					animator.AnimationSpeed = adjustedTargetMovement.Length;
					animator.AnimationDirection = adjustedTargetMovement.Angle;
					animator.PlayAnimation("Walk");
				}
				else
				{
					animator.AnimationSpeed = 1.0f;
					animator.PlayAnimation("Idle");
				}
			}
		}
	}
}
