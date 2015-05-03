using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(RigidBody))]
	public class HitSoundController : Component, ICmpCollisionListener
	{
		private ContentRef<Sound>	hitSound	= null;
		private	float				pitch		= 1.0f;
		private	float				volume		= 1.0f;

		public ContentRef<Sound> HitSound
		{
			get { return this.hitSound; }
			set { this.hitSound = value; }
		}
		public float Pitch
		{
			get { return this.pitch; }
			set { this.pitch = value; }
		}
		public float Volume
		{
			get { return this.volume; }
			set { this.volume = value; }
		}

		public void NotifyHit(float strength)
		{
			if (this.hitSound != null && strength > 0.02f)
			{
				SoundInstance inst = DualityApp.Sound.PlaySound3D(this.hitSound, this.GameObj);
				inst.Volume = this.volume * MathF.Clamp(strength, 0.0f, 1.0f);
				inst.Pitch = this.pitch;
			}
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args) {}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args) {}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs.MyShape.IsSensor) return;
			if (bodyArgs.OtherShape.IsSensor) return;

			float impactStrength = bodyArgs.CollisionData.NormalImpulse / (4.0f * bodyArgs.MyShape.Parent.Mass);
			this.NotifyHit(MathF.Pow(impactStrength, 1.5f));
		}
	}
}
