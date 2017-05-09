using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Editor;
using Duality.Drawing;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;

namespace Duality.Samples.Physics
{
	[EditorHintCategory("PhysicsSample")]
    public class CollisionEventLogger : Component, ICmpCollisionListener
	{
		void ICmpCollisionListener.OnCollisionBegin(Component sender, CollisionEventArgs args)
		{
			// Display the body we just began colliding with.
			// Note that args.CollisionData is not available here.
			VisualLog.Default
				.DrawPoint(Vector3.UnitX * -5.0f)
				.AnchorAt(this.GameObj)
				.WithColor(ColorRgba.Green)
				.KeepAlive(1000.0f);
		}
		void ICmpCollisionListener.OnCollisionEnd(Component sender, CollisionEventArgs args)
		{
			// Display the body we just stopped colliding with.
			// Note that args.CollisionData is not available here.
			VisualLog.Default
				.DrawPoint(Vector3.UnitX * 5.0f)
				.AnchorAt(this.GameObj)
				.WithColor(ColorRgba.Blue)
				.KeepAlive(1000.0f);
		}
		void ICmpCollisionListener.OnCollisionSolve(Component sender, CollisionEventArgs args)
		{
			// Display all the collision data we get while it happens.
			Vector3 collisionPos = new Vector3(args.CollisionData.Pos, 0.0f);
			VisualLog.Default
				.DrawPoint(collisionPos)
				.KeepAlive(50.0f);

			float normalArrowLength = MathF.Min(100.0f, 3.0f * MathF.Sqrt(MathF.Abs(args.CollisionData.NormalImpulse)));
			float normalArrowDir = MathF.Sign(args.CollisionData.NormalImpulse);
			if (normalArrowLength >= 15.0f)
			{
				VisualLog.Default
					.DrawVector(collisionPos, args.CollisionData.Normal * normalArrowLength * normalArrowDir)
					.KeepAlive(1000.0f);
			}

			float tangentArrowLength = MathF.Min(100.0f, 3.0f * MathF.Sqrt(MathF.Abs(args.CollisionData.TangentImpulse)));
			float tangentArrowDir = MathF.Sign(args.CollisionData.TangentImpulse);
			if (tangentArrowLength >= 15.0f)
			{
				VisualLog.Default
					.DrawVector(collisionPos, args.CollisionData.Tangent * tangentArrowLength * tangentArrowDir)
					.KeepAlive(1000.0f);
			}
		}
	}
}
