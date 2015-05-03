using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components.Physics;
using Duality.Resources;

namespace DualStickSpaceShooter
{
	[RequiredComponent(typeof(RigidBody))]
	public class Trigger : Component, ICmpCollisionListener, ICmpUpdatable
	{
		private List<GameObject> targets = null;
		private	ContentRef<Sound> triggerSound = null;
		private ParticleEffect triggerEffect = null;
		private bool fireOnce = true;
		private int collisionCounter = 0;

		public List<GameObject> Targets
		{
			get { return this.targets; }
			set { this.targets = value; }
		}
		public ContentRef<Sound> TriggerSound
		{
			get { return this.triggerSound; }
			set { this.triggerSound = value; }
		}
		public ParticleEffect TriggerEffect
		{
			get { return this.triggerEffect; }
			set { this.triggerEffect = value; }
		}
		public bool FireOnce
		{
			get { return this.fireOnce; }
			set { this.fireOnce = value; }
		}

		private void AddCollision(GameObject with)
		{
			int oldCounter = this.collisionCounter;
			this.collisionCounter++;
			if (this.collisionCounter > 0 && (oldCounter <= 0 || !this.fireOnce))
			{
				if (this.triggerSound != null)
				{
					DualityApp.Sound.PlaySound3D(this.triggerSound, this.GameObj);
				}
				if (this.targets != null)
				{
					foreach (GameObject target in this.targets)
					{
						this.SendMessage(target, new TriggerEnteredMessage(with));
					}
				}
				else
				{
					this.SendMessage(new TriggerEnteredMessage(with));
				}
			}
		}
		private void RemoveCollision(GameObject with)
		{
			int oldCounter = this.collisionCounter;
			this.collisionCounter--;
			if (this.collisionCounter <= 0 && (oldCounter > 0 || !this.fireOnce))
			{
				
				if (this.targets != null)
				{
					foreach (GameObject target in this.targets)
					{
						this.SendMessage(target, new TriggerLeftMessage(with));
					}
				}
				else
				{
					this.SendMessage(new TriggerLeftMessage(with));
				}
			}
		}

		void ICmpUpdatable.OnUpdate()
		{
			if (this.triggerEffect != null)
			{
				float radius = 100.0f;

				CircleShapeInfo triggerCircle = this.GameObj.RigidBody.Shapes.OfType<CircleShapeInfo>().FirstOrDefault();
				if (triggerCircle != null)
					radius = triggerCircle.Radius;
				else
					radius = this.GameObj.RigidBody.BoundRadius;

				if (this.collisionCounter > 0)
				{
					foreach (ParticleEmitter emitter in this.triggerEffect.Emitters)
					{
						emitter.RandomPos = new Range(
							emitter.RandomPos.MinValue + ((0.0f - emitter.RandomPos.MinValue) * 0.01f * Time.TimeMult), 
							radius);
					}
				}
				else
				{
					foreach (ParticleEmitter emitter in this.triggerEffect.Emitters)
					{
						emitter.RandomPos = new Range(
							emitter.RandomPos.MinValue + ((radius - emitter.RandomPos.MinValue) * 0.01f * Time.TimeMult), 
							radius);
					}
				}
			}
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			this.AddCollision(args.CollideWith);
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			this.RemoveCollision(args.CollideWith);
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
