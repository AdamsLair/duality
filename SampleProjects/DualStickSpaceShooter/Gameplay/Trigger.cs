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

		public List<GameObject> Targets
		{
			get { return this.targets; }
			set { this.targets = value; }
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			if (this.targets != null)
			{
				foreach (GameObject target in this.targets)
				{
					this.SendMessage(target, new TriggerEnteredMessage(args.CollideWith));
				}
			}
			else
			{
				this.SendMessage(new TriggerEnteredMessage(args.CollideWith));
			}
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			if (this.targets != null)
			{
				foreach (GameObject target in this.targets)
				{
					this.SendMessage(target, new TriggerLeftMessage(args.CollideWith));
				}
			}
			else
			{
				this.SendMessage(new TriggerLeftMessage(args.CollideWith));
			}
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
