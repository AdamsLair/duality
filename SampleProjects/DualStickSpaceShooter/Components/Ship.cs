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
		private	Vector2						targetThrust;
		private	float						targetAngle;
		private	float						targetAngleRatio;
		private	float						thrusterPower;
		private	float						turnPower;
		private	bool						isDead;
		private	float						hitpoints;
		private	float						healRate;
		private	float						maxHitpoints;
		private	float						maxSpeed;
		private	float						maxTurnSpeed;
		private	ContentRef<Prefab>[]		deathEffects;
		private	ContentRef<BulletBlueprint>	bulletType;
		private	float						weaponTimer;
		private	float						weaponDelay;
		private	Player						owner;

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
		[EditorHintFlags(MemberFlags.Invisible)]
		public float TargetAngleRatio
		{
			get { return this.targetAngleRatio; }
			set { this.targetAngleRatio = value; }
		}
		public float ThrusterPower
		{
			get { return this.thrusterPower; }
			set { this.thrusterPower = value; }
		}
		public float TurnPower
		{
			get { return this.turnPower; }
			set { this.turnPower = value; }
		}
		public bool IsDead
		{
			get { return this.isDead; }
		}
		[EditorHintDecimalPlaces(0)]
		public float Hitpoints
		{
			get { return this.hitpoints; }
			set { this.hitpoints = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float HealRate
		{
			get { return this.healRate; }
			set { this.healRate = value; }
		}
		[EditorHintDecimalPlaces(0)]
		public float MaxHitpoints
		{
			get { return this.maxHitpoints; }
			set { this.maxHitpoints = value; }
		}
		public float MaxSpeed
		{
			get { return this.maxSpeed; }
			set { this.maxSpeed = value; }
		}
		public float MaxTurnSpeed
		{
			get { return this.maxTurnSpeed; }
			set { this.maxTurnSpeed = value; }
		}
		public ContentRef<Prefab>[] DeathEffects
		{
			get { return this.deathEffects; }
			set { this.deathEffects = value; }
		}
		public ContentRef<BulletBlueprint> BulletType
		{
			get { return this.bulletType; }
			set { this.bulletType = value; }
		}
		public float WeaponDelay
		{
			get { return this.weaponDelay; }
			set { this.weaponDelay = value; }
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

		public void DoDamage(float damage)
		{
			this.hitpoints -= damage;
			if (this.hitpoints < 0.0f) this.Die();
		}
		public void Die()
		{
			// Ignore, if already dead
			if (this.isDead) return;
			this.isDead = true;

			// Notify everyone who is interested that we're dead
			this.SendMessage(new ShipDeathMessage());
			
			// Spawn death effects
			if (this.deathEffects != null)
			{
				Transform transform = this.GameObj.Transform;
				RigidBody body = this.GameObj.RigidBody;
				for (int i = 0; i < this.deathEffects.Length; i++)
				{
					GameObject effectObj = this.deathEffects[i].Res.Instantiate(transform.Pos);
					ParticleEffect effect = effectObj.GetComponent<ParticleEffect>();
					if (effect != null && this.owner != null)
					{
						ParticleEffect.EmissionData emitData = effect.EmitData;
						emitData.MaxColor = this.owner.Color.ToHsva().WithValue(emitData.MaxColor.V);
						emitData.MinColor = this.owner.Color.ToHsva().WithValue(emitData.MinColor.V);
						emitData.BaseVel += new Vector3(body.LinearVelocity);
						effect.EmitData = emitData;
					}
					Scene.Current.AddObject(effectObj);
				}
			}

			// Safely dispose the ships GameObject and set hitpoints to zero
			this.hitpoints = 0.0f;
			this.GameObj.DisposeLater();
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

			this.FireBullet(body, transform, new Vector2(0.0f, -15.0f), 0.0f);
		}

		public virtual void OnUpdate()
		{
			Transform transform		= this.GameObj.Transform;
			RigidBody body			= this.GameObj.RigidBody;

			// Heal when damaged
			this.hitpoints = MathF.Clamp(this.hitpoints + this.healRate * Time.SPFMult * Time.TimeMult, 0.0f, this.maxHitpoints);
			VisualLog.Default.DrawText(transform.Pos.X + 15.0f, transform.Pos.Y, transform.Pos.Z, string.Format("{0}", (int)this.hitpoints)).Align(Alignment.Left);

			// Apply force according to the desired thrust
			Vector2 actualVelocity = body.LinearVelocity;
			Vector2 targetVelocity = this.targetThrust * this.maxSpeed;
			Vector2 velocityDiff = (targetVelocity - actualVelocity);
			float sameDirectionFactor = Vector2.Dot(
				velocityDiff / MathF.Max(0.001f, velocityDiff.Length), 
				this.targetThrust / MathF.Max(0.001f, this.targetThrust.Length));
			Vector2 thrusterActivity = this.targetThrust.Length * MathF.Max(sameDirectionFactor, 0.0f) * velocityDiff / MathF.Max(velocityDiff.Length, 1.0f);
			body.ApplyWorldForce(thrusterActivity * this.thrusterPower);

			// Turn to the desired fire angle
			if (this.targetAngleRatio > 0.0f)
			{
				float shortestTurnDirection	= MathF.TurnDir(transform.Angle, this.targetAngle);
				float shortestTurnLength	= MathF.CircularDist(transform.Angle, this.targetAngle);
				float turnDirection;
				float turnLength;
				if (MathF.Abs(body.AngularVelocity) > this.maxTurnSpeed * 0.25f)
				{
					turnDirection	= MathF.Sign(body.AngularVelocity);
					turnLength		= (turnDirection == shortestTurnDirection) ? shortestTurnLength : (MathF.RadAngle360 - shortestTurnLength);
				}
				else
				{
					turnDirection	= shortestTurnDirection;
					turnLength		= shortestTurnLength;
				}
				float turnSpeedRatio	= MathF.Min(turnLength * 0.25f, MathF.RadAngle30) / MathF.RadAngle30;
				float turnVelocity		= turnSpeedRatio * this.maxTurnSpeed * this.targetAngleRatio * turnDirection;
				body.AngularVelocity	+= (turnVelocity - body.AngularVelocity) * this.turnPower * Time.TimeMult;
			}

			// Weapon cooldown
			this.weaponTimer = MathF.Max(0.0f, this.weaponTimer - Time.MsPFMult * Time.TimeMult);
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
