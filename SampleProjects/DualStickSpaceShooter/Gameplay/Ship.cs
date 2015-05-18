using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Resources;
using Duality.Audio;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(Transform))]
	[RequiredComponent(typeof(SpriteRenderer))]
	[RequiredComponent(typeof(RigidBody))]
	public class Ship : Component, ICmpUpdatable, ICmpInitializable
	{
		private	ContentRef<ShipBlueprint>	blueprint				= null;
		private	Vector2						targetThrust			= Vector2.Zero;
		private	float						targetAngle				= 0.0f;
		private	float						targetAngleRatio		= 0.0f;
		private	bool						isDead					= false;
		private	float						hitpoints				= 1.0f;
		private	float						weaponTimer				= 0.0f;
		private	Player						owner					= null;
		private	ParticleEffect				damageEffect			= null;
		[DontSerialize] private	SoundInstance flightLoop			= null;

		public ContentRef<ShipBlueprint> Blueprint
		{
			get { return this.blueprint; }
			set { this.blueprint = value; }
		}
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
		public bool IsDead
		{
			get { return this.isDead; }
		}
		public float Hitpoints
		{
			get { return this.hitpoints; }
			set { this.hitpoints = value; }
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
			this.hitpoints -= damage / this.blueprint.Res.MaxHitpoints;
			if (this.hitpoints < 0.0f) this.Die();

			if (this.owner != null)
			{
				CameraController camControl = Scene.Current.FindComponent<CameraController>();
				camControl.ShakeScreen(MathF.Pow(damage, 0.75f));
			}
		}
		public void Die()
		{
			// Ignore, if already dead
			if (this.isDead) return;
			this.isDead = true;

			// Notify everyone who is interested that we're dead
			this.SendMessage(new ShipDeathMessage());
			
			// Spawn death effects
			ShipBlueprint blueprint = this.blueprint.Res;
			if (blueprint.DeathEffects != null)
			{
				Transform transform = this.GameObj.Transform;
				RigidBody body = this.GameObj.RigidBody;
				for (int i = 0; i < blueprint.DeathEffects.Length; i++)
				{
					Prefab deathEffectPrefab = blueprint.DeathEffects[i].Res;
					GameObject effectObj = deathEffectPrefab.Instantiate(transform.Pos);
					ParticleEffect effect = effectObj.GetComponent<ParticleEffect>();
					if (effect != null && this.owner != null)
					{
						foreach (ParticleEmitter emitter in effect.Emitters)
						{
							if (emitter == null) continue;
							emitter.MaxColor = this.owner.Color.ToHsva().WithValue(emitter.MaxColor.V);
							emitter.MinColor = this.owner.Color.ToHsva().WithValue(emitter.MinColor.V);
							emitter.BaseVel += new Vector3(body.LinearVelocity);
						}
					}
					Scene.Current.AddObject(effectObj);
				}
			}

			// Safely dispose the ships GameObject and set hitpoints to zero
			this.hitpoints = 0.0f;
			if (this.owner != null)
				this.GameObj.Active = false;
			else
				this.GameObj.DisposeLater();
		}
		public void Revive()
		{
			// Ignore, if not dead
			if (!this.isDead) return;
			this.isDead = false;

			// Make sure to reset the rigidbodies movement state
			RigidBody body = this.GameObj.RigidBody;
			body.LinearVelocity = Vector2.Zero;
			body.AngularVelocity = 0.0f;

			// Activate the ship again and give it back all of its hitpoints
			this.hitpoints = 1.0f;
			this.GameObj.Active = true;
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
			this.weaponTimer += this.blueprint.Res.WeaponDelay;

			Transform transform = this.GameObj.Transform;
			RigidBody body = this.GameObj.RigidBody;

			this.FireBullet(body, transform, new Vector2(0.0f, -15.0f), 0.0f);
		}

		private void FireBullet(RigidBody body, Transform transform, Vector2 localPos, float localAngle)
		{
			ShipBlueprint blueprint = this.blueprint.Res;
			if (blueprint.BulletType == null) return;

			Bullet bullet = blueprint.BulletType.Res.CreateBullet();

			Vector2 recoilImpulse;
			Vector2 worldPos = transform.GetWorldPoint(localPos);
			bullet.Fire(this.owner, body.LinearVelocity, worldPos, transform.Angle + localAngle, out recoilImpulse);
			body.ApplyWorldImpulse(recoilImpulse);

			Scene.Current.AddObject(bullet.GameObj);

			SoundInstance inst = null;
			if (Player.AlivePlayers.Count() > 1)
				inst = DualityApp.Sound.PlaySound3D(this.owner.WeaponSound, new Vector3(worldPos));
			else
				inst = DualityApp.Sound.PlaySound(this.owner.WeaponSound);
			inst.Volume = MathF.Rnd.NextFloat(0.6f, 1.0f);
			inst.Pitch = MathF.Rnd.NextFloat(0.9f, 1.11f);
		}
		
		void ICmpUpdatable.OnUpdate()
		{
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.RigidBody;
			ShipBlueprint	blueprint	= this.blueprint.Res;

			// Heal when damaged
			if (this.hitpoints < 1.0f)
			{
				this.hitpoints = MathF.Clamp(this.hitpoints + blueprint.HealRate * Time.SPFMult * Time.TimeMult / blueprint.MaxHitpoints, 0.0f, 1.0f);
			}

			// Apply force according to the desired thrust
			Vector2 actualVelocity = body.LinearVelocity;
			Vector2 targetVelocity = this.targetThrust * blueprint.MaxSpeed;
			Vector2 velocityDiff = (targetVelocity - actualVelocity);
			float sameDirectionFactor = Vector2.Dot(
				velocityDiff / MathF.Max(0.001f, velocityDiff.Length), 
				this.targetThrust / MathF.Max(0.001f, this.targetThrust.Length));
			Vector2 thrusterActivity = this.targetThrust.Length * MathF.Max(sameDirectionFactor, 0.0f) * velocityDiff / MathF.Max(velocityDiff.Length, 1.0f);
			body.ApplyWorldForce(thrusterActivity * blueprint.ThrusterPower);

			// Turn to the desired fire angle
			if (this.targetAngleRatio > 0.0f)
			{
				float shortestTurnDirection	= MathF.TurnDir(transform.Angle, this.targetAngle);
				float shortestTurnLength	= MathF.CircularDist(transform.Angle, this.targetAngle);
				float turnDirection;
				float turnLength;
				if (MathF.Abs(body.AngularVelocity) > blueprint.MaxTurnSpeed * 0.25f)
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
				float turnVelocity		= turnSpeedRatio * turnDirection * blueprint.MaxTurnSpeed * this.targetAngleRatio;
				body.AngularVelocity	+= (turnVelocity - body.AngularVelocity) * blueprint.TurnPower * Time.TimeMult;
			}

			// Weapon cooldown
			this.weaponTimer = MathF.Max(0.0f, this.weaponTimer - Time.MsPFMult * Time.TimeMult);

			// Play the owners special flight sound, when available
			if (this.owner != null && this.owner.FlightLoop != null)
			{
				SoundListener listener = Scene.Current.FindComponent<SoundListener>();
				Vector3 listenerPos = listener.GameObj.Transform.Pos;

				// Determine the target panning manually, because we don't want a true 3D sound here (doppler, falloff, ...)
				float targetPanning;
				if (listenerPos.Xy == transform.Pos.Xy || Player.AlivePlayers.Count() <= 1)
					targetPanning = 0.0f;
				else
					targetPanning = -Vector2.Dot(Vector2.UnitX, (listenerPos - transform.Pos).Xy.Normalized);

				// Determine the target volume
				float targetVolume = MathF.Clamp(this.targetThrust.Length, 0.0f, 1.0f);

				// Clean up disposed flight loop
				if (this.flightLoop != null && this.flightLoop.Disposed)
					this.flightLoop = null;

				// Start the flight loop when requested
				if (targetVolume > 0.0f && this.flightLoop == null)
				{
					if ((int)Time.MainTimer.TotalMilliseconds % 2976 <= (int)Time.MsPFMult)
					{
						this.flightLoop = DualityApp.Sound.PlaySound(this.owner.FlightLoop);
						this.flightLoop.Looped = true;
					}
				}

				// Configure existing flight loop
				if (this.flightLoop != null)
				{
					this.flightLoop.Volume += (targetVolume - this.flightLoop.Volume) * 0.05f * Time.TimeMult;
					this.flightLoop.Panning += (targetPanning - this.flightLoop.Panning) * 0.05f * Time.TimeMult;
				}
			}

			// Display the damage effect when damaged
			if (this.hitpoints < 0.85f && blueprint.DamageEffect != null)
			{
				// Create a new damage effect instance, if not present yet
				if (this.damageEffect == null)
				{
					GameObject damageObj = blueprint.DamageEffect.Res.Instantiate(transform.Pos);
					damageObj.Parent = this.GameObj;

					this.damageEffect = damageObj.GetComponent<ParticleEffect>();
					if (this.damageEffect == null) throw new NullReferenceException();
				}

				// Configure the damage effect
				foreach (ParticleEmitter emitter in this.damageEffect.Emitters)
				{
					if (emitter == null) continue;
					emitter.BurstDelay = new Range(50.0f + this.hitpoints * 50.0f, 100.0f + this.hitpoints * 300.0f);
					if (this.owner != null)
					{
						ColorHsva targetColor = this.owner.Color.ToHsva();
						emitter.MinColor = new ColorHsva(targetColor.H, targetColor.S, emitter.MinColor.V, emitter.MinColor.A);
						emitter.MaxColor = new ColorHsva(targetColor.H, targetColor.S, emitter.MaxColor.V, emitter.MaxColor.A);
					}
				}
			}
			// Get rid of existing damage effects, if no longer needed
			else if (this.damageEffect != null)
			{
				// Stop emitting and dispose when empty
				foreach (ParticleEmitter emitter in this.damageEffect.Emitters)
				{
					if (emitter == null) continue;
					emitter.BurstDelay = float.MaxValue;
				}
				this.damageEffect.DisposeWhenEmpty = true;
				this.damageEffect = null;
			}
		}
		void ICmpInitializable.OnInit(Component.InitContext context) {}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				if (this.flightLoop != null)
				{
					this.flightLoop.Dispose();
					this.flightLoop = null;
				}
			}
		}
	}
}
