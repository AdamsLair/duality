using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	public class Bullet : Component, ICmpCollisionListener, ICmpUpdatable
	{
		private float						lifetime	= 8000.0f;
		private	ContentRef<BulletBlueprint> blueprint	= null;
		private	Player						owner		= null;

		public void InitFrom(BulletBlueprint blueprint)
		{
			this.lifetime = blueprint.Lifetime;
			this.blueprint = blueprint;
		}
		public void Fire(Player owner, Vector2 sourceDragVel, Vector2 position, float angle, out Vector2 recoilImpulse)
		{
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.RigidBody;
			SpriteRenderer	sprite		= this.GameObj.GetComponent<SpriteRenderer>();
			BulletBlueprint	blueprint	= this.blueprint.Res;

			Vector2 direction = Vector2.FromAngleLength(angle, 1.0f);

			body.LinearVelocity = direction * blueprint.LaunchSpeed + sourceDragVel;
			transform.Pos = new Vector3(position, 0.0f);
			transform.MoveByAbs(body.LinearVelocity * Time.TimeMult);
			transform.Angle = angle;
			sprite.Offset = 1;

			if (owner != null)
			{
				sprite.ColorTint = owner.Color;
				this.owner = owner;
			}

			recoilImpulse = -direction * blueprint.LaunchSpeed * blueprint.ImpactMass;
		}
		
		void ICmpUpdatable.OnUpdate()
		{
			this.lifetime -= Time.MsPFMult * Time.TimeMult;
			if (this.lifetime <= 0.0f) this.GameObj.DisposeLater();
		}
		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs == null) return;
			if (bodyArgs.OtherShape.IsSensor) return;

			Ship otherShip = args.CollideWith.GetComponent<Ship>();
			if (otherShip != null && otherShip.Owner == this.owner) return;

			RigidBody		otherBody	= args.CollideWith.RigidBody;
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.RigidBody;
			BulletBlueprint	blueprint	= this.blueprint.Res;

			otherBody.ApplyWorldImpulse(body.LinearVelocity * MathF.Min(otherBody.Mass, blueprint.ImpactMass), transform.Pos.Xy);

			this.GameObj.DisposeLater();
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args) {}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
