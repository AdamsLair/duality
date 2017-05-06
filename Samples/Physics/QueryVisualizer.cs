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
    public class QueryVisualizer : Component, ICmpUpdatable
	{
		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;

			// Query all objects that might be in a 200x200 rect around this object
			Vector2 queryRectSize = new Vector2(200, 200);
			List<RigidBody> queriedBodies = RigidBody.QueryRectGlobal(transform.Pos.Xy - queryRectSize * 0.5f, queryRectSize);

			// Display all objects that were returned from the query
			foreach (RigidBody body in queriedBodies)
			{
				if (body.GameObj == this.GameObj) continue;

				VisualLog.Default
					.DrawCircle(Vector3.Zero, body.BoundRadius)
					.AnchorAt(body.GameObj)
					.WithColor(ColorRgba.Green.WithAlpha(64));
			}

			// Display the queried area
			VisualLog.Default
				.DrawPolygon(transform.Pos, new Vector2[]
				{
					queryRectSize * new Vector2(-0.5f, -0.5f),
					queryRectSize * new Vector2(0.5f, -0.5f),
					queryRectSize * new Vector2(0.5f, 0.5f),
					queryRectSize * new Vector2(-0.5f, 0.5f)
				})
				.WithColor(ColorRgba.White.WithAlpha(128))
				.WithOffset(1.0f);
			VisualLog.Default
				.DrawText(
					transform.Pos - new Vector3(queryRectSize * 0.5f) + new Vector3(10.0f, 10.0f, 0.0f),
					string.Format("{0} bodies", queriedBodies.Count - 1))
				.WithOffset(-1.0f);
		}
	}
}
