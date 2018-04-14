using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class QueryVisualizer : Component, ICmpUpdatable
	{
		[DontSerialize] private List<RigidBody> queriedBodies = new List<RigidBody>();

		void ICmpUpdatable.OnUpdate()
		{
			Transform transform = this.GameObj.Transform;

			// Query all objects that might be in a 200x200 rect around this object
			Vector2 queryRectSize = new Vector2(200, 200);
			Scene.Physics.QueryRect(
				transform.Pos.Xy - queryRectSize * 0.5f, 
				queryRectSize, 
				this.queriedBodies);

			// Display all objects that were returned from the query
			foreach (RigidBody body in this.queriedBodies)
			{
				if (body.GameObj == this.GameObj) continue;

				VisualLogs.Default
					.DrawCircle(Vector3.Zero, body.BoundRadius)
					.AnchorAt(body.GameObj)
					.WithColor(ColorRgba.Green.WithAlpha(64));
			}

			// Display the queried area
			VisualLogs.Default
				.DrawPolygon(transform.Pos, new Vector2[]
				{
					queryRectSize * new Vector2(-0.5f, -0.5f),
					queryRectSize * new Vector2(0.5f, -0.5f),
					queryRectSize * new Vector2(0.5f, 0.5f),
					queryRectSize * new Vector2(-0.5f, 0.5f)
				})
				.WithColor(ColorRgba.White.WithAlpha(128))
				.WithOffset(1.0f);
			VisualLogs.Default
				.DrawText(
					transform.Pos - new Vector3(queryRectSize * 0.5f) + new Vector3(10.0f, 10.0f, 0.0f),
					string.Format("{0} bodies", this.queriedBodies.Count - 1))
				.WithOffset(-1.0f);

			// Clear the list of queried bodies, to be re-used next frame
			this.queriedBodies.Clear();
		}
	}
}
