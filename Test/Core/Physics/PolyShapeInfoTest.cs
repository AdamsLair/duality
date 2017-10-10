using Duality.Components.Physics;

using NUnit.Framework;

namespace Duality.Tests.Physics
{
	[TestFixture]
	class PolyShapeInfoTest
	{
		[Test] public void PolyShapeVertInBox()
		{
			PolyShapeInfo triangleShape = new PolyShapeInfo(new[] { new Vector2(0.0f, 0.0f), new Vector2(100f, 0f), new Vector2(100f, 100f) }, 1);
			Box box = new Box(50f, 90f, 100f, 100f);
			Assert.IsTrue(triangleShape.IntersectsWith(box));
		}

		[Test] public void BoxVertInPolyShape()
		{
			PolyShapeInfo triangleShape = new PolyShapeInfo(new[] { new Vector2(0.0f, 0.0f), new Vector2(100f, 0f), new Vector2(100f, 100f) }, 1);
			Box box = new Box(25f, 49f, 25f, 25f);
			Assert.IsTrue(triangleShape.IntersectsWith(box));
		}

		[Test] public void PolyShapeAndBoxNoCollision()
		{
			PolyShapeInfo triangleShape = new PolyShapeInfo(new[] { new Vector2(0.0f, 0.0f), new Vector2(100f, 0f), new Vector2(100f, 100f) }, 1);
			Box box = new Box(25f, 150f, 25f, 25f);
			Assert.IsFalse(triangleShape.IntersectsWith(box));
		}
	}
}
