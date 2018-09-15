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
    public class ApplyImpulseOnCollide : Component, ICmpCollisionListener
	{
		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args) { }
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args) { }
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) 
		{
			RigidBodyCollisionEventArgs bodyArgs = args as RigidBodyCollisionEventArgs;
			if (bodyArgs == null) return;
			if (bodyArgs.OtherShape.IsSensor) return;

			Transform myTransform = this.GameObj.Transform;
			RigidBody otherBody = bodyArgs.OtherShape.Parent;

			// Apply an impulse at the (world-space) position of the collision
			Vector2 impulseDirection = bodyArgs.CollisionData.Normal;
			Vector2 applyWorldPos = bodyArgs.CollisionData.Pos;
			otherBody.ApplyWorldImpulse(
				impulseDirection * 100.0f, 
				applyWorldPos);

			// Display a log to note that we did so
			VisualLogs.Default
				.DrawVector(new Vector3(applyWorldPos), impulseDirection * 15.0f)
				.KeepAlive(1000.0f);
		}
	}
}
