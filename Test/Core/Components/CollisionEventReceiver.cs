using System.Collections.Generic;

namespace Duality.Tests.Components
{
	/// <summary>
	/// A collision listener that stores collision events in lists
	/// </summary>
	public class CollisionEventReceiver : Component, ICmpCollisionListener
	{
		public struct CollisionEvent
		{
			public readonly Component Sender;
			public readonly CollisionEventArgs Args;

			public CollisionEvent(Component sender, CollisionEventArgs args)
			{
				this.Sender = sender;
				this.Args = args;
			}
		}

		public readonly List<CollisionEvent> CollisionBegin = new List<CollisionEvent>();
		public readonly List<CollisionEvent> CollisionEnd = new List<CollisionEvent>();
		public readonly List<CollisionEvent> CollisionSolve = new List<CollisionEvent>();

		public void OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			this.CollisionBegin.Add(new CollisionEvent(sender, args));
		}

		public void OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			this.CollisionEnd.Add(new CollisionEvent(sender, args));
		}

		public void OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
			this.CollisionSolve.Add(new CollisionEvent(sender, args));
		}
	}
}
