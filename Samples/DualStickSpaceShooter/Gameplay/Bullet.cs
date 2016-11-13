using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Audio;
using Duality.Editor;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
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
			RigidBody		body		= this.GameObj.GetComponent<RigidBody>();
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

			// Did we collide with a ship? If it's the same that fired the bullet, ignore this
			Ship otherShip = args.CollideWith.GetComponent<Ship>();
			if (otherShip != null && otherShip.Owner == this.owner) return;

			// Get all the objet references we'll need
			RigidBody		otherBody	= args.CollideWith.GetComponent<RigidBody>();
			Transform		transform	= this.GameObj.Transform;
			RigidBody		body		= this.GameObj.GetComponent<RigidBody>();
			BulletBlueprint	blueprint	= this.blueprint.Res;

			// Okay, let's determine where *exactly* our bullet hit
			RayCastData firstHit;
			bool hitAnything = RigidBody.RayCast(transform.Pos.Xy - body.LinearVelocity * 2, transform.Pos.Xy + body.LinearVelocity * 2, data =>
			{
				if (data.Shape.IsSensor) return -1.0f;
				return data.Fraction;
			}, out firstHit);

			Vector3 hitPos;
			float hitAngle;
			if (hitAnything)
			{
				hitPos = new Vector3(firstHit.Pos, 0.0f);
				hitAngle = (-firstHit.Normal).Angle;
			}
			else
			{
				// Note that it is possible for the raycast to not hit anything, 
				// because it is essentially a line, while our bullet is wider than zero.
				hitPos = transform.Pos;
				hitAngle = transform.Angle;
			}

			// Push around whatever we've just hit and do damage, if it was a ship
			otherBody.ApplyWorldImpulse(body.LinearVelocity * MathF.Min(otherBody.Mass, blueprint.ImpactMass), transform.Pos.Xy);
			if (otherShip != null)
			{
				otherShip.DoDamage(blueprint.Damage);
			}

			// If we hit a part of the world, spawn the world hit effect
			if (otherShip == null && blueprint.HitWorldEffect != null)
			{
				GameObject effectObj = blueprint.HitWorldEffect.Res.Instantiate(hitPos, hitAngle);
				Scene.Current.AddObject(effectObj);
			}

			// Also spawn a generic hit effect in the color of the bullet
			if (blueprint.HitEffect != null)
			{
				GameObject effectObj = blueprint.HitEffect.Res.Instantiate(hitPos, hitAngle);
				ParticleEffect effect = effectObj.GetComponent<ParticleEffect>();
				if (effect != null && this.owner != null)
				{
					ColorHsva color = this.owner.Color.ToHsva();
					foreach (ParticleEmitter emitter in effect.Emitters)
					{
						emitter.MaxColor = emitter.MaxColor.WithSaturation(color.S).WithHue(color.H);
						emitter.MinColor = emitter.MinColor.WithSaturation(color.S).WithHue(color.H);
					}
				}
				Scene.Current.AddObject(effectObj);
			}

			// Play hit sounds
			if (blueprint.HitSound != null)
			{
				SoundInstance inst = DualityApp.Sound.PlaySound3D(blueprint.HitSound, hitPos);
				inst.Pitch = MathF.Rnd.NextFloat(0.95f, 1.05f);
			}
			HitSoundController otherHitSound = otherBody.GameObj.GetComponent<HitSoundController>();
			if (otherHitSound != null)
			{
				otherHitSound.NotifyHit(MathF.Rnd.NextFloat(0.75f, 1.0f));
			}

			// Delete the bullet
			this.GameObj.DisposeLater();
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args) {}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
