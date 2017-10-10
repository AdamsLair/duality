using Duality.Components.Physics;

using NUnit.Framework;

namespace Duality.Tests.Physics
{
	[TestFixture]
	public class ChainShapeInfoTest
	{
		[Test] public void ChainShapeAndBoxNoCollision()
		{
			ChainShapeInfo chainShape = new ChainShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(190f, 110f, 10, 10);
			Assert.IsFalse(chainShape.IntersectsWith(box));
		}

		[Test] public void ChainShapeVertInBoxTest()
		{
			ChainShapeInfo chainShape = new ChainShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(200f, 100f, 10, 10);
			Assert.IsTrue(chainShape.IntersectsWith(box));
		}

		[Test] public void ChainShapeIntersectsBoxTest()
		{
			ChainShapeInfo chainShape = new ChainShapeInfo(new[] {
				new Vector2(0,0),
				new Vector2(100,0),
				new Vector2(200,100),
			});
			Box box = new Box(145f, 45f, 10, 10);
			Assert.IsTrue(chainShape.IntersectsWith(box));
		}
	}
}
