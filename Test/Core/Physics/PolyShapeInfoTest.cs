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
	class PolyShapeInfoTest
	{
		[Test]
		public void IntersectsWith()
		{
			PolyShapeInfo triangleShape = new PolyShapeInfo(new[] { new Vector2(0.0f, 0.0f), new Vector2(100f, 0f), new Vector2(100f, 100f) }, 1);
			Assert.IsTrue(triangleShape.IntersectsWith(new Vector2(60f, 30f), new Vector2(100f, 100f)));
			Assert.IsTrue(triangleShape.IntersectsWith(new Vector2(95.5f, 96.5f), new Vector2(100f, 100f)));
		}
	}
}
