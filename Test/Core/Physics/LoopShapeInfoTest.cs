using Duality.Components.Physics;

using NUnit.Framework;

namespace Duality.Tests.Physics
{
	[TestFixture]
	public class LoopShapeInfoTest
	{
		[Test] public void LoopShapeAndBoxNoCollision()
		{
			LoopShapeInfo loopShape = new LoopShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(190f, 110f, 10, 10);
			Assert.IsFalse(loopShape.IntersectsWith(box));
		}

		[Test] public void LoopShapeVertInBoxTest()
		{
			LoopShapeInfo loopShape = new LoopShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(200f, 100f, 10, 10);
			Assert.IsTrue(loopShape.IntersectsWith(box)); 
		}

		[Test] public void LoopShapeIntersectsBoxTest()
		{
			LoopShapeInfo loopShape = new LoopShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(145f, 45f, 10, 10);
			Assert.IsTrue(loopShape.IntersectsWith(box)); 
		}
	}
}
