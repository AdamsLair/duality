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
	public class RectTest
	{
		[Test] public void Constuctors()
		{
			AssertRectEqual(new Rect(), 0, 0, 0, 0);
			AssertRectEqual(new Rect(1, 2, 3, 4), 1, 2, 3, 4);
			AssertRectEqual(new Rect(3, 4), 0, 0, 3, 4);
			AssertRectEqual(new Rect(new Vector2(5, 6)), 0, 0, 5, 6);
		}
		[Test] public void EqualityChecks()
		{
			Rect a = new Rect(1, 2, 3, 4);
			Rect b = new Rect(1, 2, 3, 4);
			Rect[] c = new[]
			{
				new Rect(0, 0, 0, 0),
				new Rect(1, 2, 3, 0),
				new Rect(1, 2, 0, 4),
				new Rect(1, 0, 3, 4),
				new Rect(0, 2, 3, 4),
				new Rect(4, 6, -3, -4)
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
		[Test] public void BasicProperties()
		{
			Rect rect = new Rect(1, 2, 3, 4);

			Assert.AreEqual(new Vector2(1, 2), rect.Pos);
			Assert.AreEqual(new Vector2(3, 4), rect.Size);
			Assert.AreEqual(MathF.Distance(1 + 3, 2 + 4), rect.BoundingRadius);
			Assert.AreEqual(1, rect.LeftX);
			Assert.AreEqual(2, rect.TopY);
			Assert.AreEqual(4, rect.RightX);
			Assert.AreEqual(6, rect.BottomY);
			Assert.AreEqual(1 + 3 / 2.0f, rect.CenterX);
			Assert.AreEqual(2 + 4 / 2.0f, rect.CenterY);

			// Since we checked all min/max/center values above, we can safely rely on them here
			Assert.AreEqual(new Vector2(rect.LeftX, rect.TopY), rect.TopLeft);
			Assert.AreEqual(new Vector2(rect.RightX, rect.TopY), rect.TopRight);
			Assert.AreEqual(new Vector2(rect.CenterX, rect.CenterY), rect.Center);
			Assert.AreEqual(new Vector2(rect.LeftX, rect.BottomY), rect.BottomLeft);
			Assert.AreEqual(new Vector2(rect.RightX, rect.BottomY), rect.BottomRight);

			// Test some assignment operations
			rect.Pos = new Vector2(2, 1);
			Assert.AreEqual(new Vector2(2, 1), rect.Pos);
			rect.Size = new Vector2(4, 3);
			Assert.AreEqual(new Vector2(4, 3), rect.Size);
		}
		[Test] public void BasicTransformation()
		{
			Rect original = new Rect(1, 2, 3, 4);
			Rect originalF = new Rect(1.4f, 2.4f, 3.4f, 4.4f);

			// Transform operations should always return copies
			original.Scaled(2.5f, 2.5f);
			original.WithOffset(1.5f, 1.5f);
			original.Transformed(2.5f, 2.5f);
			Assert.AreEqual(original, new Rect(1, 2, 3, 4));

			// Test some basic transformations
			Assert.AreEqual(new Rect(1, 2, 6, 8), original.Scaled(2, 2));
			Assert.AreEqual(new Rect(3, 4, 3, 4), original.WithOffset(2, 2));
			Assert.AreEqual(new Rect(2, 4, 6, 8), original.Transformed(2, 2));

			// Check normalizing
			Assert.AreEqual(new Rect(1, 2, 3, 4), new Rect(4, 6, -3, -4).Normalized());
		}
		[Test] public void ExpandTo()
		{
			Rect rect = Rect.Empty;

			// Transform operations should always return copies
			rect.ExpandedToContain(10, 10);
			Assert.AreEqual(rect, Rect.Empty);

			// Test expansion operations
			rect = rect.ExpandedToContain(new Rect(-1, -2, 1, 2));
			Assert.AreEqual(new Rect(-1, -2, 1, 2), rect);

			rect = rect.ExpandedToContain(new Rect(0, 0, 2, 1));
			Assert.AreEqual(new Rect(-1, -2, 3, 3), rect);

			rect = rect.ExpandedToContain(new Vector2(5, 0));
			Assert.AreEqual(new Rect(-1, -2, 6, 3), rect);
		}
		[Test] public void Containment()
		{
			Rect rect = new Rect(-5, -10, 10, 20);

			// A rect should contain all the points on its edge
			Assert.IsTrue(rect.Contains(rect.TopLeft));
			Assert.IsTrue(rect.Contains(rect.TopRight));
			Assert.IsTrue(rect.Contains(rect.Center));
			Assert.IsTrue(rect.Contains(rect.BottomLeft));
			Assert.IsTrue(rect.Contains(rect.BottomRight));

			// Points it shouldn't contain
			Assert.IsFalse(rect.Contains(new Vector2(rect.LeftX - 1.0f, rect.CenterY)));
			Assert.IsFalse(rect.Contains(new Vector2(rect.RightX + 1.0f, rect.CenterY)));
			Assert.IsFalse(rect.Contains(new Vector2(rect.CenterX, rect.TopY - 1.0f)));
			Assert.IsFalse(rect.Contains(new Vector2(rect.CenterX, rect.BottomY + 1.0f)));

			// A rect should contain itself, but not any offset variant
			Assert.IsTrue(rect.Contains(rect));
			Assert.IsFalse(rect.Contains(rect.WithOffset(1, 0)));
			Assert.IsFalse(rect.Contains(rect.WithOffset(-1, 0)));
			Assert.IsFalse(rect.Contains(rect.WithOffset(0, 1)));
			Assert.IsFalse(rect.Contains(rect.WithOffset(0, -1)));

			// It can contain a smaller rect, but not a bigger one
			Assert.IsTrue(rect.Contains(1, 2, 3, 4));
			Assert.IsFalse(rect.Contains(-6, -11, 12, 22));
			Assert.IsFalse(rect.Contains(-1, -11, 2, 22));
			Assert.IsFalse(rect.Contains(-6, -1, 12, 2));
		}
		[Test] public void CreateAlignment()
		{
			Assert.AreEqual(new Rect(-5, -5, 10, 10),   Rect.Align(Alignment.Center, 0, 0, 10, 10));

			Assert.AreEqual(new Rect(0, -5, 10, 10),	Rect.Align(Alignment.Left, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, -5, 10, 10),  Rect.Align(Alignment.Right, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(-5, 0, 10, 10),	Rect.Align(Alignment.Top, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(-5, -10, 10, 10),  Rect.Align(Alignment.Bottom, 0, 0, 10, 10));

			Assert.AreEqual(new Rect(0, 0, 10, 10),	 Rect.Align(Alignment.TopLeft, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, 0, 10, 10),   Rect.Align(Alignment.TopRight, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(0, -10, 10, 10),   Rect.Align(Alignment.BottomLeft, 0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, -10, 10, 10), Rect.Align(Alignment.BottomRight, 0, 0, 10, 10));
		}

		[Test] 
		[TestCase(-5, -10, 10, 20, Description = "Regular Rect")]
		[TestCase(5, 10, -10, -20, Description = "Negative Rect")]
		public void IntersectionCheking(int x, int y, int w, int h)
		{
			Rect rect = new Rect(x, y, w, h);

			// Intersection with self and offset-variants
			Assert.IsTrue(rect.Intersects(rect));
			Assert.IsTrue(rect.Intersects(rect.WithOffset(1, 0)));
			Assert.IsTrue(rect.Intersects(rect.WithOffset(-1, 0)));
			Assert.IsTrue(rect.Intersects(rect.WithOffset(0, 1)));
			Assert.IsTrue(rect.Intersects(rect.WithOffset(0, -1)));

			// Intersection with crossing rect: Horizontal
			Assert.IsTrue(rect.Intersects(rect.LeftX - 1, rect.TopY, rect.RightX - rect.LeftX + 2, rect.BottomY - rect.TopY));
			Assert.IsTrue(rect.Intersects(rect.LeftX - 1, rect.TopY - 1, rect.RightX - rect.LeftX + 2, 2));
			Assert.IsTrue(rect.Intersects(rect.LeftX - 1, rect.BottomY - 1, rect.RightX - rect.LeftX + 2, 2));

			// Intersection with crossing rect: Vertical
			Assert.IsTrue(rect.Intersects(rect.LeftX, rect.TopY - 1, rect.RightX - rect.LeftX, rect.BottomY - rect.TopY + 2));
			Assert.IsTrue(rect.Intersects(rect.LeftX - 1, rect.TopY - 1, 2, rect.BottomY - rect.TopY + 2));
			Assert.IsTrue(rect.Intersects(rect.RightX - 1, rect.TopY - 1, 2, rect.BottomY - rect.TopY + 2));

			// Intersection with corners
			Assert.IsTrue(rect.Intersects(rect.TopLeft.X - 1, rect.TopLeft.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.TopRight.X - 1, rect.TopRight.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.BottomLeft.X - 1, rect.BottomLeft.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 2, 2));

			// Non-intersection
			Assert.IsFalse(rect.Intersects(rect.LeftX - 2, rect.TopY, 1, rect.BottomY - rect.TopY));
			Assert.IsFalse(rect.Intersects(rect.RightX + 2, rect.TopY, 1, rect.BottomY - rect.TopY));
			Assert.IsFalse(rect.Intersects(rect.LeftX, rect.TopY - 2, rect.RightX - rect.LeftX, 1));
			Assert.IsFalse(rect.Intersects(rect.LeftX, rect.BottomY + 2, rect.RightX - rect.LeftX, 1));

			// Non-intersection with corners
			Assert.IsFalse(rect.Intersects(rect.TopLeft.X - 2, rect.TopLeft.Y - 2, 1, 1));
			Assert.IsFalse(rect.Intersects(rect.TopRight.X + 2, rect.TopRight.Y - 2, 1, 1));
			Assert.IsFalse(rect.Intersects(rect.BottomLeft.X - 2, rect.BottomLeft.Y + 2, 1, 1));
			Assert.IsFalse(rect.Intersects(rect.BottomRight.X + 2, rect.BottomRight.Y + 2, 1, 1));
		}

		[Test] 
		[TestCase(-5, -10, 10, 20, Description = "Regular Rect")]
		[TestCase(5, 10, -10, -20, Description = "Negative Rect")]
		public void IntersectionRect(int x, int y, int w, int h)
		{
			Rect rect = new Rect(x, y, w, h);
			Rect norm = rect.Normalized();

			// Intersection with self and offset-variants
			Assert.AreEqual(norm, rect.Intersection(rect));
			Assert.AreEqual(new Rect(norm.X + 1, norm.Y	, norm.W - 1, norm.H	), rect.Intersection(rect.WithOffset(1, 0)));
			Assert.AreEqual(new Rect(norm.X	, norm.Y	, norm.W - 1, norm.H	), rect.Intersection(rect.WithOffset(-1, 0)));
			Assert.AreEqual(new Rect(norm.X	, norm.Y + 1, norm.W	, norm.H - 1), rect.Intersection(rect.WithOffset(0, 1)));
			Assert.AreEqual(new Rect(norm.X	, norm.Y	, norm.W	, norm.H - 1), rect.Intersection(rect.WithOffset(0, -1)));

			// Intersection with corners
			Assert.AreEqual(new Rect(rect.TopLeft.X		, rect.TopLeft.Y		, 1, 1), rect.Intersection(rect.TopLeft.X - 1	, rect.TopLeft.Y - 1	, 2, 2));
			Assert.AreEqual(new Rect(rect.TopRight.X - 1   , rect.TopRight.Y	   , 1, 1), rect.Intersection(rect.TopRight.X - 1   , rect.TopRight.Y - 1   , 2, 2));
			Assert.AreEqual(new Rect(rect.BottomLeft.X	 , rect.BottomLeft.Y - 1 , 1, 1), rect.Intersection(rect.BottomLeft.X - 1 , rect.BottomLeft.Y - 1 , 2, 2));
			Assert.AreEqual(new Rect(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 1, 1), rect.Intersection(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 2, 2));

			// Non-intersection
			Assert.AreEqual(Rect.Empty, rect.Intersection(rect.WithOffset(MathF.Abs(rect.W), 0)));
			Assert.AreEqual(Rect.Empty, rect.Intersection(rect.WithOffset(-MathF.Abs(rect.W), 0)));
			Assert.AreEqual(Rect.Empty, rect.Intersection(rect.WithOffset(0, MathF.Abs(rect.H))));
			Assert.AreEqual(Rect.Empty, rect.Intersection(rect.WithOffset(0, -MathF.Abs(rect.H))));
		}

		private void AssertRectEqual(Rect rect, float x, float y, float w, float h)
		{
			Assert.AreEqual(x, rect.X);
			Assert.AreEqual(y, rect.Y);
			Assert.AreEqual(w, rect.W);
			Assert.AreEqual(h, rect.H);
		}
	}
}
