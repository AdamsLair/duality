using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(SpriteRenderer))]
	[RequiredComponent(typeof(RigidBody))]
	public class Ship : Component, ICmpUpdatable
	{
		private	Vector2						targetThrust	= Vector2.Zero;
		private	float						targetAngle		= 0.0f;
		private	float						maxTurnSpeed	= 0.5f;
		private	float						thrusterPower	= 5.0f;
		private	ContentRef<BulletBlueprint>	bulletType		= null;
		private	float						weaponTimer		= 0.0f;
		private	float						weaponDelay		= 200.0f;
		private	Player						owner			= null;

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
		public Player Owner
		{
			get { return this.owner; }
			set
			{
				this.owner = value;
				this.UpdatePlayerColor();
			}
		}

		public virtual void OnUpdate()
		{
			Transform transform		= this.GameObj.Transform;
			RigidBody body			= this.GameObj.RigidBody;

			// Turn to the desired fire angle
			float turnDirection		= MathF.TurnDir(transform.Angle, this.targetAngle);
			float turnLength		= MathF.CircularDist(transform.Angle, this.targetAngle);
			float turnVelocity		= MathF.Min(turnLength, MathF.RadAngle30) * this.maxTurnSpeed * turnDirection;
			body.AngularVelocity	= turnVelocity;

			// Apply force according to the desired thrust
			body.ApplyWorldForce(this.targetThrust * this.thrusterPower);

			// Weapon cooldown
			this.weaponTimer = MathF.Max(0.0f, this.weaponTimer - Time.MsPFMult * Time.TimeMult);
		}

		public void UpdatePlayerColor()
		{
			SpriteRenderer sprite = this.GameObj.GetComponent<SpriteRenderer>();
			if (sprite != null)
			{
				if (this.owner != null)
					sprite.ColorTint = this.owner.Color;
				else
					sprite.ColorTint = ColorRgba.White;
			}
		}
		public void FireWeapon()
		{
			if (this.weaponTimer > 0.0f) return;
			this.weaponTimer += this.weaponDelay;

			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.RigidBody;

			this.FireBullet(body, transform, Vector2.Zero, 0.0f);
		}
		private void FireBullet(RigidBody body, Transform transform, Vector2 localPos, float localAngle)
		{
			if (!this.bulletType.IsAvailable) return;

			Bullet bullet = this.bulletType.Res.CreateBullet();

			Vector2 recoilImpulse;
			Vector2 worldPos = transform.GetWorldPoint(localPos);
			bullet.Fire(this.owner, body.LinearVelocity, worldPos, transform.Angle + localAngle, out recoilImpulse);
			body.ApplyWorldImpulse(recoilImpulse);

			Scene.Current.AddObject(bullet.GameObj);
		}
	}
}
