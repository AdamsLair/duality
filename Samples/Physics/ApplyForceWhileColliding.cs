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
    public class ApplyForceWhileColliding : Component, ICmpUpdatable, ICmpCollisionListener
	{
		[DontSerialize] private Dictionary<GameObject,int> touchingObjects = new Dictionary<GameObject,int>();

		void ICmpUpdatable.OnUpdate()
		{
			// Note: OnCollisionBegin will be called once per shape, not per object.
			// However, in this case we're interested in objects not shapes, so we'll
			// keep track of the shapes to make sense of which objects we're touching.
			foreach (GameObject obj in this.touchingObjects.Keys)
			{
				RigidBody body = obj.GetComponent<RigidBody>();

				// Apply a force at the position of the object
				Vector2 forceDirection = -Vector2.UnitY;
				Vector2 applyWorldPos = obj.Transform.Pos.Xy;
				body.ApplyWorldForce(
					forceDirection * 20.0f, 
					applyWorldPos);

				// Display a log to note that we did so
				VisualLog.Default.DrawVector(new Vector3(applyWorldPos), forceDirection * 15.0f);
			}
		}

		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			// Check if we already have any contacts with this object
			int contactCount;
			this.touchingObjects.TryGetValue(args.CollideWith, out contactCount);

			// Increase the number of contacts by this one
			contactCount++;

			// Store the contact count for this object. If it wasn't
			// stored before, this will also add it to the dictionary.
			this.touchingObjects[args.CollideWith] = contactCount;
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			// Check how many contacts we recorded with this object
			int contactCount;
			this.touchingObjects.TryGetValue(args.CollideWith, out contactCount);

			// Decrease the number of contacts by this one
			contactCount--;

			// If we no longer have any contacts to this object, remove
			// it from the dictionary. Otherwise, just store the new
			// contact count.
			if (contactCount == 0)
				this.touchingObjects.Remove(args.CollideWith);
			else
				this.touchingObjects[args.CollideWith] = contactCount;
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args) { }
	}
}
