using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class TargetResizeTest
	{
		[Test] public void None()
		{
			Assert.AreEqual(new Vector2(0, 0), TargetResize.None.Apply(new Vector2(0, 0), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(1, 1), TargetResize.None.Apply(new Vector2(1, 1), new Vector2(0, 0)));
			Assert.AreEqual(new Vector2(1, 1), TargetResize.None.Apply(new Vector2(1, 1), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(1, 2), TargetResize.None.Apply(new Vector2(1, 2), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(2, 1), TargetResize.None.Apply(new Vector2(2, 1), new Vector2(3, 5)));
		}
		[Test] public void Stretch()
		{
			Assert.AreEqual(new Vector2(3, 5), TargetResize.Stretch.Apply(new Vector2(0, 0), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(0, 0), TargetResize.Stretch.Apply(new Vector2(1, 1), new Vector2(0, 0)));
			Assert.AreEqual(new Vector2(3, 5), TargetResize.Stretch.Apply(new Vector2(1, 1), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(3, 5), TargetResize.Stretch.Apply(new Vector2(1, 2), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(3, 5), TargetResize.Stretch.Apply(new Vector2(2, 1), new Vector2(3, 5)));
		}
		[Test] public void Fit()
		{
			Assert.AreEqual(new Vector2(3, 3), TargetResize.Fit.Apply(new Vector2(0, 0), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(0, 0), TargetResize.Fit.Apply(new Vector2(1, 1), new Vector2(0, 0)));
			Assert.AreEqual(new Vector2(3, 3), TargetResize.Fit.Apply(new Vector2(1, 1), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(2.5f, 5), TargetResize.Fit.Apply(new Vector2(1, 2), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(3, 1.5f), TargetResize.Fit.Apply(new Vector2(2, 1), new Vector2(3, 5)));
		}
		[Test] public void Fill()
		{
			Assert.AreEqual(new Vector2(5, 5), TargetResize.Fill.Apply(new Vector2(0, 0), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(0, 0), TargetResize.Fill.Apply(new Vector2(1, 1), new Vector2(0, 0)));
			Assert.AreEqual(new Vector2(5, 5), TargetResize.Fill.Apply(new Vector2(1, 1), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(3, 6), TargetResize.Fill.Apply(new Vector2(1, 2), new Vector2(3, 5)));
			Assert.AreEqual(new Vector2(10, 5), TargetResize.Fill.Apply(new Vector2(2, 1), new Vector2(3, 5)));
		}
	}
}
