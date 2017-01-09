using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Drawing;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class Point2Test
	{
		[Test] public void Constuctors()
		{
			AssertPointEqual(new Point2(), 0, 0);
			AssertPointEqual(new Point2(1, 2), 1, 2);
		}
		[Test] public void EqualityChecks()
		{
			Point2 a = new Point2(1, 2);
			Point2 b = new Point2(1, 2);
			Point2[] c = new[]
			{
				new Point2(0, 0),
				new Point2(1, 0),
				new Point2(0, 2),
				new Point2(-1, -2)
			};
			
			// Equality
			Assert.AreEqual(a, b);
			Assert.AreEqual(b, a);
			Assert.IsTrue(a == b);
			Assert.IsTrue(b == a);

			// Different kinds of inequality
			for (int i = 0; i < c.Length; i++)
			{
				Assert.AreNotEqual(a, c[i]);
				Assert.AreNotEqual(c[i], a);
				Assert.IsFalse(a == c[i]);
				Assert.IsFalse(c[i] == a);
			}
			Assert.AreNotEqual(a, null);
			Assert.AreNotEqual(a, 17.5f);
		}
		[Test] public void PointMath()
		{
			// Pure int-based math
			AssertPointEqual(-new Point2(1, 2), -1, -2);
			AssertPointEqual(new Point2(1, 2) + new Point2(3, 4), 4, 6);
			AssertPointEqual(new Point2(1, 2) - new Point2(3, 4), -2, -2);
			AssertPointEqual(new Point2(1, 2) * new Point2(3, 4), 3, 8);
			AssertPointEqual(new Point2(1, 2) * 2, 2, 4);
			AssertPointEqual(2 * new Point2(1, 2), 2, 4);
			AssertPointEqual(new Point2(3, 4) / new Point2(1, 2), 3, 2);
			AssertPointEqual(new Point2(3, 4) / 2, 1, 2);

			// Implicit cast to Vector2 when using float math
			AssertVectorEqual(new Point2(1, 2) + new Vector2(3, 4), 4.0f, 6.0f);
			AssertVectorEqual(new Point2(1, 2) - new Vector2(3, 4), -2.0f, -2.0f);
			AssertVectorEqual(new Point2(1, 2) * new Vector2(3, 4), 3.0f, 8.0f);
			AssertVectorEqual(new Point2(1, 2) * 2.0f, 2.0f, 4.0f);
			AssertVectorEqual(2.0f * new Point2(1, 2), 2.0f, 4.0f);
			AssertVectorEqual(new Point2(3, 4) / new Vector2(1, 2), 3.0f, 2.0f);
			AssertVectorEqual(new Point2(3, 4) / 2.0f, 1.5f, 2.0f);
		}
		[Test] public void Casting()
		{
			// Implicit cast to Vector2
			AssertVectorEqual(new Point2(1, 2), 1.0f, 2.0f);

			// Explicit cast to Point2
			AssertPointEqual((Point2)new Vector2(1, 2), 1, 2);
		}
		[Test] public void MinMax()
		{
			AssertPointEqual(Point2.Min(new Point2(1, 2), new Point2(2, 1)), 1, 1);
			AssertPointEqual(Point2.Max(new Point2(1, 2), new Point2(2, 1)), 2, 2);
		}
		[Test] public void Distance()
		{
			Assert.AreEqual(1.0f, Point2.Distance(new Point2(0, 0), new Point2(0, 1)));
			Assert.AreEqual(1.0f, Point2.Distance(new Point2(0, 0), new Point2(1, 0)));
			Assert.AreEqual(MathF.Sqrt(2), Point2.Distance(new Point2(0, 0), new Point2(1, 1)));
		}
		[Test] public void Indexing()
		{
			Assert.AreEqual(1, new Point2(1, 2)[0]);
			Assert.AreEqual(2, new Point2(1, 2)[1]);
			Assert.Throws<IndexOutOfRangeException>(() => { int x = new Point2(1, 2)[-1]; });
			Assert.Throws<IndexOutOfRangeException>(() => { int x = new Point2(1, 2)[2]; });

			Point2 point = new Point2(0, 0);
			point[0] = 1;
			point[1] = 2;
			Assert.AreEqual(1, point.X);
			Assert.AreEqual(2, point.Y);

			Assert.Throws<IndexOutOfRangeException>(() => { point[-1] = 0; });
			Assert.Throws<IndexOutOfRangeException>(() => { point[2] = 0; });
		}

		private void AssertPointEqual(Point2 point, int x, int y)
		{
			Assert.AreEqual(x, point.X);
			Assert.AreEqual(y, point.Y);
		}
		private void AssertVectorEqual(Vector2 vector, float x, float y)
		{
			Assert.AreEqual(x, vector.X);
			Assert.AreEqual(y, vector.Y);
		}
	}
}
