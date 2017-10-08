using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Input;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
	[RequiredComponent(typeof(RigidBody))]
    public class OneWayCollision : Component, ICmpInitializable
	{
		private bool CollisionFilter(CollisionFilterData data)
		{
			Transform otherTransform = data.OtherGameObj.Transform;
			Transform transform = data.MyGameObj.Transform;
			
			// Project the other bodies position onto this objects forward axis
			Vector2 forward = transform.Forward.Xy;
			Vector2 relativePos = (otherTransform.Pos - transform.Pos).Xy;
			float projectedLength = Vector2.Dot(relativePos, forward);

			// If the projected length is positive, the other object is on top of this one.
			// We'll require it to be higher by a small threshold and a significant part of
			// the other bodies overall size. We don't want to flip on collisions while the
			// other object is passing through mid-way.
			if (projectedLength <= 5.0f + data.OtherBody.BoundRadius * 0.35f) return false;

			// Also, as long as the other object is moving through the right way, don't interfere
			float projectedVelocity = Vector2.Dot(data.OtherBody.LinearVelocity, forward);
			if (projectedVelocity > 0.0f) return false;

			// If no special case applies, collide
			return true;
		}

		void ICmpInitializable.OnInit(Component.InitContext context)
		{
			if (context == InitContext.Activate)
			{
				RigidBody body = this.GameObj.GetComponent<RigidBody>();
				body.CollisionFilter += this.CollisionFilter;
			}
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
			{
				RigidBody body = this.GameObj.GetComponent<RigidBody>();
				body.CollisionFilter -= this.CollisionFilter;
			}
		}
	}
}
