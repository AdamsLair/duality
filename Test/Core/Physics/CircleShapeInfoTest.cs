using Duality.Components.Physics;

using NUnit.Framework;

namespace Duality.Tests.Physics
{
	[TestFixture]
	public class CircleShapeInfoTest
	{
		[Test] public void CircleShapeContainsBoxTest()
		{
			CircleShapeInfo circleShape = new CircleShapeInfo(100, new Vector2(0, 0), 1);
			var box = new Box(-10f, -10, 10, 10);
			Assert.IsTrue(circleShape.IntersectsWith(box));
		}

		[Test] public void BoxContainsCircleShapeTest()
		{
			CircleShapeInfo circleShape = new CircleShapeInfo(100, new Vector2(0, 0), 1);
			var box = new Box(-110f, -110, 220, 220);
			Assert.IsTrue(circleShape.IntersectsWith(box));
		}

		[Test] public void CircleShapeIntersectsBox()
		{
			CircleShapeInfo circleShape = new CircleShapeInfo(100, new Vector2(0, 0), 1);
			var box = new Box(-200f, 90, 400, 100);
			Assert.IsTrue(circleShape.IntersectsWith(box));
		}

		[Test] public void CircleShapeAndBoxNoCollision()
		{
			CircleShapeInfo circleShape = new CircleShapeInfo(100, new Vector2(0, 0), 1);
			var box = new Box(-200f, 110, 400, 100);
			Assert.IsFalse(circleShape.IntersectsWith(box));
		}
	}
}
