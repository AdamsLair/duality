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
		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			this.SendMessage(new TriggerEnteredMessage(args.CollideWith));
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			this.SendMessage(new TriggerLeftMessage(args.CollideWith));
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) {}
	}
}
