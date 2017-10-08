using System.Collections.Generic;
using System.Diagnostics;

namespace Duality.Tests.Components
{
	/// <summary>
	/// A collision listener that stores collision events in lists
	/// </summary>
	public class CollisionEventReceiver : Component, ICmpCollisionListener
	{
		public enum CollisionType
		{
			/// <summary>
			/// This event was created during <see cref="ICmpCollisionListener.OnCollisionBegin(Component, CollisionEventArgs)"/>
			/// </summary>
			Begin,

			/// <summary>
			/// This event was created during <see cref="ICmpCollisionListener.OnCollisionSolve(Component, CollisionEventArgs)(Component, CollisionEventArgs)"/>
			/// </summary>
			Solve,

			/// <summary>
			/// This event was created during <see cref="ICmpCollisionListener.OnCollisionEnd(Component, CollisionEventArgs)(Component, CollisionEventArgs)"/>
			/// </summary>
			End
		}

		[DebuggerDisplay("{Type} event with {Args.CollideWith}")]
		public struct CollisionEvent
		{
			public readonly Component Sender;
			public readonly CollisionEventArgs Args;
			public readonly CollisionType Type;

			public CollisionEvent(Component sender, CollisionEventArgs args, CollisionType collisionType)
			{
				this.Sender = sender;
				this.Args = args;
				this.Type = collisionType;
			}
		}

		public readonly List<CollisionEvent> Collisions = new List<CollisionEvent>();

		public void OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			this.Collisions.Add(new CollisionEvent(sender, args, CollisionType.Begin));
		}

		public void OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
			this.Collisions.Add(new CollisionEvent(sender, args, CollisionType.Solve));
		}

		public void OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			this.Collisions.Add(new CollisionEvent(sender, args, CollisionType.End));
		}
	}
}
