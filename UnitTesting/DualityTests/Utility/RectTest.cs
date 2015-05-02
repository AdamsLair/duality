using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Drawing;

using NUnit.Framework;
using OpenTK;

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
			Assert.AreEqual(3 * 4, rect.Area);
			Assert.AreEqual(2 * (3 + 4), rect.Perimeter);
			Assert.AreEqual(MathF.Distance(1 + 3, 2 + 4), rect.BoundingRadius);
			Assert.AreEqual(1, rect.MinX);
			Assert.AreEqual(2, rect.MinY);
			Assert.AreEqual(4, rect.MaxX);
			Assert.AreEqual(6, rect.MaxY);
			Assert.AreEqual(1 + 3 / 2.0f, rect.CenterX);
			Assert.AreEqual(2 + 4 / 2.0f, rect.CenterY);

			// Since we checked all min/max/center values above, we can safely rely on them here
			Assert.AreEqual(new Vector2(rect.MinX, rect.MinY), rect.TopLeft);
			Assert.AreEqual(new Vector2(rect.CenterX, rect.MinY), rect.Top);
			Assert.AreEqual(new Vector2(rect.MaxX, rect.MinY), rect.TopRight);
			Assert.AreEqual(new Vector2(rect.MinX, rect.CenterY), rect.Left);
			Assert.AreEqual(new Vector2(rect.CenterX, rect.CenterY), rect.Center);
			Assert.AreEqual(new Vector2(rect.MaxX, rect.CenterY), rect.Right);
			Assert.AreEqual(new Vector2(rect.MinX, rect.MaxY), rect.BottomLeft);
			Assert.AreEqual(new Vector2(rect.CenterX, rect.MaxY), rect.Bottom);
			Assert.AreEqual(new Vector2(rect.MaxX, rect.MaxY), rect.BottomRight);

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
			original.Scale(2.5f, 2.5f);
			original.Round();
			original.Offset(1.5f, 1.5f);
			original.Ceiling();
			original.Transform(2.5f, 2.5f);
			original.Floor();
			Assert.AreEqual(original, new Rect(1, 2, 3, 4));

			// Test some basic transformations
			Assert.AreEqual(new Rect(1, 2, 6, 8), original.Scale(2, 2));
			Assert.AreEqual(new Rect(3, 4, 3, 4), original.Offset(2, 2));
			Assert.AreEqual(new Rect(2, 4, 6, 8), original.Transform(2, 2));
			Assert.AreEqual(new Rect(1, 2, 3, 4), originalF.Round());
			Assert.AreEqual(new Rect(1, 2, 3, 4), originalF.Floor());
			Assert.AreEqual(new Rect(2, 3, 4, 5), originalF.Ceiling());

			// Check normalizing
			Assert.AreEqual(new Rect(1, 2, 3, 4), new Rect(4, 6, -3, -4).Normalize());
		}
		[Test] public void ExpandTo()
		{
			Rect rect = Rect.Empty;

			// Transform operations should always return copies
			rect.ExpandToContain(10, 10);
			Assert.AreEqual(rect, Rect.Empty);

			// Test expansion operations
			rect = rect.ExpandToContain(new Rect(-1, -2, 1, 2));
			Assert.AreEqual(new Rect(-1, -2, 1, 2), rect);

			rect = rect.ExpandToContain(new Rect(0, 0, 2, 1));
			Assert.AreEqual(new Rect(-1, -2, 3, 3), rect);

			rect = rect.ExpandToContain(new Vector2(5, 0));
			Assert.AreEqual(new Rect(-1, -2, 6, 3), rect);
		}
		[Test] public void Containment()
		{
			Rect rect = new Rect(-5, -10, 10, 20);

			// A rect should contain all the points on its edge
			Assert.IsTrue(rect.Contains(rect.TopLeft));
			Assert.IsTrue(rect.Contains(rect.Top));
			Assert.IsTrue(rect.Contains(rect.TopRight));
			Assert.IsTrue(rect.Contains(rect.Left));
			Assert.IsTrue(rect.Contains(rect.Center));
			Assert.IsTrue(rect.Contains(rect.Right));
			Assert.IsTrue(rect.Contains(rect.BottomLeft));
			Assert.IsTrue(rect.Contains(rect.Bottom));
			Assert.IsTrue(rect.Contains(rect.BottomRight));

			// Points it shouldn't contain
			Assert.IsFalse(rect.Contains(rect.Left - Vector2.UnitX));
			Assert.IsFalse(rect.Contains(rect.Right + Vector2.UnitX));
			Assert.IsFalse(rect.Contains(rect.Top - Vector2.UnitY));
			Assert.IsFalse(rect.Contains(rect.Bottom + Vector2.UnitY));

			// A rect should contain itself, but not any offset variant
			Assert.IsTrue(rect.Contains(rect));
			Assert.IsFalse(rect.Contains(rect.Offset(1, 0)));
			Assert.IsFalse(rect.Contains(rect.Offset(-1, 0)));
			Assert.IsFalse(rect.Contains(rect.Offset(0, 1)));
			Assert.IsFalse(rect.Contains(rect.Offset(0, -1)));

			// It can contain a smaller rect, but not a bigger one
			Assert.IsTrue(rect.Contains(1, 2, 3, 4));
			Assert.IsFalse(rect.Contains(-6, -11, 12, 22));
			Assert.IsFalse(rect.Contains(-1, -11, 2, 22));
			Assert.IsFalse(rect.Contains(-6, -1, 12, 2));
		}
		[Test] public void Alignment()
		{
			Assert.AreEqual(new Rect(-5, -5, 10, 10),   Rect.AlignCenter(0, 0, 10, 10));

			Assert.AreEqual(new Rect(0, -5, 10, 10),    Rect.AlignLeft(0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, -5, 10, 10),  Rect.AlignRight(0, 0, 10, 10));
			Assert.AreEqual(new Rect(-5, 0, 10, 10),    Rect.AlignTop(0, 0, 10, 10));
			Assert.AreEqual(new Rect(-5, -10, 10, 10),  Rect.AlignBottom(0, 0, 10, 10));

			Assert.AreEqual(new Rect(0, 0, 10, 10),     Rect.AlignTopLeft(0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, 0, 10, 10),   Rect.AlignTopRight(0, 0, 10, 10));
			Assert.AreEqual(new Rect(0, -10, 10, 10),   Rect.AlignBottomLeft(0, 0, 10, 10));
			Assert.AreEqual(new Rect(-10, -10, 10, 10), Rect.AlignBottomRight(0, 0, 10, 10));
		}

		[Test] 
		[TestCase(-5, -10, 10, 20, Description = "Regular Rect")]
		[TestCase(5, 10, -10, -20, Description = "Negative Rect")]
		public void IntersectionCheking(int x, int y, int w, int h)
		{
			Rect rect = new Rect(x, y, w, h);

			// Intersection with self and offset-variants
			Assert.IsTrue(rect.Intersects(rect));
			Assert.IsTrue(rect.Intersects(rect.Offset(1, 0)));
			Assert.IsTrue(rect.Intersects(rect.Offset(-1, 0)));
			Assert.IsTrue(rect.Intersects(rect.Offset(0, 1)));
			Assert.IsTrue(rect.Intersects(rect.Offset(0, -1)));

			// Intersection with crossing rect: Horizontal
			Assert.IsTrue(rect.Intersects(rect.MinX - 1, rect.MinY, rect.MaxX - rect.MinX + 2, rect.MaxY - rect.MinY));
			Assert.IsTrue(rect.Intersects(rect.MinX - 1, rect.MinY - 1, rect.MaxX - rect.MinX + 2, 2));
			Assert.IsTrue(rect.Intersects(rect.MinX - 1, rect.MaxY - 1, rect.MaxX - rect.MinX + 2, 2));

			// Intersection with crossing rect: Vertical
			Assert.IsTrue(rect.Intersects(rect.MinX, rect.MinY - 1, rect.MaxX - rect.MinX, rect.MaxY - rect.MinY + 2));
			Assert.IsTrue(rect.Intersects(rect.MinX - 1, rect.MinY - 1, 2, rect.MaxY - rect.MinY + 2));
			Assert.IsTrue(rect.Intersects(rect.MaxX - 1, rect.MinY - 1, 2, rect.MaxY - rect.MinY + 2));

			// Intersection with corners
			Assert.IsTrue(rect.Intersects(rect.TopLeft.X - 1, rect.TopLeft.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.TopRight.X - 1, rect.TopRight.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.BottomLeft.X - 1, rect.BottomLeft.Y - 1, 2, 2));
			Assert.IsTrue(rect.Intersects(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 2, 2));

			// Non-intersection
			Assert.IsFalse(rect.Intersects(rect.MinX - 2, rect.MinY, 1, rect.MaxY - rect.MinY));
			Assert.IsFalse(rect.Intersects(rect.MaxX + 2, rect.MinY, 1, rect.MaxY - rect.MinY));
			Assert.IsFalse(rect.Intersects(rect.MinX, rect.MinY - 2, rect.MaxX - rect.MinX, 1));
			Assert.IsFalse(rect.Intersects(rect.MinX, rect.MaxY + 2, rect.MaxX - rect.MinX, 1));

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
			Rect norm = rect.Normalize();

			// Intersection with self and offset-variants
			Assert.AreEqual(norm, rect.Intersection(rect));
			Assert.AreEqual(new Rect(norm.X + 1, norm.Y    , norm.W - 1, norm.H    ), rect.Intersection(rect.Offset(1, 0)));
			Assert.AreEqual(new Rect(norm.X    , norm.Y    , norm.W - 1, norm.H    ), rect.Intersection(rect.Offset(-1, 0)));
			Assert.AreEqual(new Rect(norm.X    , norm.Y + 1, norm.W    , norm.H - 1), rect.Intersection(rect.Offset(0, 1)));
			Assert.AreEqual(new Rect(norm.X    , norm.Y    , norm.W    , norm.H - 1), rect.Intersection(rect.Offset(0, -1)));

			// Intersection with corners
			Assert.AreEqual(new Rect(rect.TopLeft.X        , rect.TopLeft.Y        , 1, 1), rect.Intersection(rect.TopLeft.X - 1    , rect.TopLeft.Y - 1    , 2, 2));
			Assert.AreEqual(new Rect(rect.TopRight.X - 1   , rect.TopRight.Y       , 1, 1), rect.Intersection(rect.TopRight.X - 1   , rect.TopRight.Y - 1   , 2, 2));
			Assert.AreEqual(new Rect(rect.BottomLeft.X     , rect.BottomLeft.Y - 1 , 1, 1), rect.Intersection(rect.BottomLeft.X - 1 , rect.BottomLeft.Y - 1 , 2, 2));
			Assert.AreEqual(new Rect(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 1, 1), rect.Intersection(rect.BottomRight.X - 1, rect.BottomRight.Y - 1, 2, 2));

			// Non-intersection
			Assert.AreEqual(0, rect.Intersection(rect.Offset(MathF.Abs(rect.W), 0)).Area);
			Assert.AreEqual(0, rect.Intersection(rect.Offset(-MathF.Abs(rect.W), 0)).Area);
			Assert.AreEqual(0, rect.Intersection(rect.Offset(0, MathF.Abs(rect.H))).Area);
			Assert.AreEqual(0, rect.Intersection(rect.Offset(0, -MathF.Abs(rect.H))).Area);
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
