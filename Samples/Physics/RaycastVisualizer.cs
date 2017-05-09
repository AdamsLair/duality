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
    public class RaycastVisualizer : Component, ICmpUpdatable
	{
		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;

			// Perform a raycast 250 units forward
			Vector2 startPos = transform.Pos.Xy;
			Vector2 endPos = startPos + transform.Forward.Xy * 250.0f;
			RayCastData nearestHit;
			bool hitAnything = RigidBody.RayCast(startPos, endPos, hitData => 
			{
				// Ignore this object, as we're the ones sending the raycast
				if (hitData.Body.GameObj == this.GameObj) return -1.0f;
				// Ignore sensor shapes, as we want to pass through them
				if (hitData.Shape.IsSensor) return -1.0f;
				// Clip the ray to the fraction where it hit the current shape
				return hitData.Fraction;
			}, out nearestHit);

			// Display a visual log with the raycast results
			if (hitAnything)
			{
				VisualLogs.Default
					.DrawCircle(Vector3.Zero, nearestHit.Body.BoundRadius)
					.AnchorAt(nearestHit.GameObj)
					.WithColor(ColorRgba.Green.WithAlpha(128));

				Rect hitShapeRect = nearestHit.Shape.AABB;
				VisualLogs.Default
					.DrawCircle(new Vector3(hitShapeRect.Center), hitShapeRect.BoundingRadius - hitShapeRect.Center.Length)
					.AnchorAt(nearestHit.GameObj)
					.WithColor(ColorRgba.Blue.WithAlpha(128));

				VisualLogs.Default
					.DrawConnection(new Vector3(startPos), nearestHit.Pos)
					.WithColor(ColorRgba.Red);
				VisualLogs.Default
					.DrawVector(new Vector3(nearestHit.Pos), nearestHit.Normal * 25)
					.WithColor(ColorRgba.Red);
			}
			else
			{
				VisualLogs.Default.DrawConnection(new Vector3(startPos), endPos);
			}
		}
	}
}
