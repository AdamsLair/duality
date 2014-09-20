using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(RigidBody))]
	public class Ship : Component, ICmpUpdatable
	{
		private	Vector2						targetThrust	= Vector2.Zero;
		private	float						targetAngle		= 0.0f;
		private	ContentRef<BulletBlueprint>	bulletType		= null;
		private	float						weaponTimer		= 0.0f;
		private	float						weaponDelay		= 200.0f;

		[EditorHintFlags(MemberFlags.Invisible)]
		public Vector2 TargetThrust
		{
			get { return this.targetThrust; }
			set { this.targetThrust = value; }
		}
		[EditorHintFlags(MemberFlags.Invisible)]
		public float TargetAngle
		{
			get { return this.targetAngle; }
			set { this.targetAngle = value; }
		}
		public ContentRef<BulletBlueprint> BulletType
		{
			get { return this.bulletType; }
			set { this.bulletType = value; }
		}

		public virtual void OnUpdate()
		{
			Transform transform = this.GameObj.Transform;
			RigidBody body		= this.GameObj.RigidBody;

			// Turn to the desired fire angle
			float turnDirection		= MathF.TurnDir(transform.Angle, this.targetAngle);
			float turnLength		= MathF.CircularDist(transform.Angle, this.targetAngle);
			float turnVelocity		= MathF.Min(turnLength, MathF.RadAngle30) * 0.5f * turnDirection;
			body.AngularVelocity	= turnVelocity;

			// Apply force according to the desired thrust
			body.ApplyWorldForce(this.targetThrust * 0.5f);

			// Weapon cooldown
			this.weaponTimer = MathF.Max(0.0f, this.weaponTimer - Time.MsPFMult * Time.TimeMult);
		}

		public void FireWeapon()
		{
			if (!this.bulletType.IsAvailable) return;

			if (this.weaponTimer > 0.0f) return;
			this.weaponTimer += this.weaponDelay;

			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.RigidBody;

			this.FireBullet(body, transform, Vector2.Zero, 0.0f);
		}
		private void FireBullet(RigidBody body, Transform transform, Vector2 localPos, float localAngle)
		{
			Bullet bullet = this.bulletType.Res.CreateBullet();
			Scene.Current.AddObject(bullet.GameObj);

			Vector2 worldPos = transform.GetWorldPoint(localPos);
			Vector2 recoilImpulse;
			bullet.Fire(body.LinearVelocity, worldPos, transform.Angle + localAngle, out recoilImpulse);
			body.ApplyWorldImpulse(recoilImpulse);
		}
	}
}
