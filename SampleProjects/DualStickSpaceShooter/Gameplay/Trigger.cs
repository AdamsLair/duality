using System;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Components.Physics;

namespace DualStickSpaceShooter
{
	[Serializable]
	[RequiredComponent(typeof(RigidBody))]
	public class Trigger : Component, ICmpCollisionListener
	{
		private List<GameObject> targets = null;
		private bool fireOnce = true;
		private int collisionCounter = 0;

		public List<GameObject> Targets
		{
			get { return this.targets; }
			set { this.targets = value; }
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
