using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Resources;
using NUnit.Framework;

namespace Duality.Tests.Physics
{
	[TestFixture]
	public class CircleShapeInfoTest
	{
		[Test] public void IntersectWithTest()
		{
			Scene scene = new Scene();
			Scene.SwitchTo(scene);
			GameObject gameobj = new GameObject("Ball");
			Transform transform = gameobj.AddComponent<Transform>();
			RigidBody rigidbody = gameobj.AddComponent<RigidBody>();
			CircleShapeInfo circleShape = new CircleShapeInfo(1, new Vector2(0, 0), 1);
			rigidbody.AddShape(circleShape);

			Assert.IsTrue(circleShape.IntersectsWith(new Vector2(0.70f, 0.70f), new Vector2(10, 10)));
			Assert.IsFalse(circleShape.IntersectsWith(new Vector2(0.75f, 0.75f), new Vector2(10, 10)));
		}
	}
}
