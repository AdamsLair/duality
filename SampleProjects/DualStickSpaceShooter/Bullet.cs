using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Components;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	public class Bullet : Component, ICmpCollisionListener, ICmpUpdatable
	{
		private const float ForceFactor = 0.005f;

		private float						lifetime	= 8000.0f;
		private	ContentRef<BulletBlueprint> blueprint	= null;

		public void InitFrom(BulletBlueprint blueprint)
		{
			this.lifetime = blueprint.Lifetime;
			this.blueprint = blueprint;
		}
		public void Fire(Vector2 sourceDragVel, Vector2 position, float angle, out Vector2 recoilImpulse)
		{
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.RigidBody;
			BulletBlueprint	blueprint	= this.blueprint.Res;

			Vector2 direction = Vector2.FromAngleLength(angle, 1.0f);

			transform.Pos = new Vector3(position, 0.0f);
			transform.Angle = angle;
			body.LinearVelocity = direction * blueprint.LaunchSpeed + sourceDragVel;

			recoilImpulse = -direction * blueprint.LaunchSpeed * blueprint.RecoilForce * ForceFactor;
		}
		
		void ICmpUpdatable.OnUpdate()
		{
			this.lifetime -= Time.MsPFMult * Time.TimeMult;
			if (this.lifetime <= 0.0f) this.GameObj.DisposeLater();
		}
		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.RigidBody;
			RigidBody		otherBody	= args.CollideWith.RigidBody;
			BulletBlueprint	blueprint	= this.blueprint.Res;

			otherBody.ApplyWorldImpulse(body.LinearVelocity * blueprint.ImpactForce * ForceFactor, transform.Pos.Xy);

			this.GameObj.DisposeLater();
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args) {}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
