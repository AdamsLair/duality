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
        [Test] public void ConstuctorsAndGetters()
        {
            Rect rect = new Rect();

            Assert.AreEqual(rect.X, 0.0);
            Assert.AreEqual(rect.Y, 0.0);
            Assert.AreEqual(rect.W, 0.0);
            Assert.AreEqual(rect.H, 0.0);

            rect = new Rect(1, 2, 3, 4);
            Assert.AreEqual(rect.X, 1.0);
            Assert.AreEqual(rect.Y, 2.0);
            Assert.AreEqual(rect.W, 3.0);
            Assert.AreEqual(rect.H, 4.0);

            rect = new Rect(3, 4);
            Assert.AreEqual(rect.X, 0.0);
            Assert.AreEqual(rect.Y, 0.0);
            Assert.AreEqual(rect.W, 3.0);
            Assert.AreEqual(rect.H, 4.0);

            rect = new Rect(-3, -4);
            Assert.AreEqual(rect.X, 0.0);
            Assert.AreEqual(rect.Y, 0.0);
            Assert.AreEqual(rect.W, -3.0);
            Assert.AreEqual(rect.H, -4.0);

            Assert.AreEqual(rect.MaximumX, 0.0);
            Assert.AreEqual(rect.MinimumX, -3.0);
            Assert.AreEqual(rect.MaximumY, 0.0);
            Assert.AreEqual(rect.MinimumY, -4.0);

            Vector2 size = new Vector2(5, 6);
            rect = new Rect(size);
            Assert.AreEqual(rect.X, 0.0);
            Assert.AreEqual(rect.Y, 0.0);
            Assert.AreEqual(rect.W, 5.0);
            Assert.AreEqual(rect.H, 6.0);

            rect.Pos = new Vector2(7, 8);
            Assert.AreEqual(rect.X, 7.0);
            Assert.AreEqual(rect.Y, 8.0);
            Assert.AreEqual(rect.W, 5.0);
            Assert.AreEqual(rect.H, 6.0);

            Assert.AreEqual(rect.Size, new Vector2(5, 6));

            Assert.AreEqual(rect.Pos, new Vector2(7, 8));
            Assert.AreEqual(rect.MaximumX, 12.0f);
            Assert.AreEqual(rect.MinimumX, 7.0f);
            Assert.AreEqual(rect.MaximumY, 14.0f);
            Assert.AreEqual(rect.MinimumY, 8.0f);

            Assert.AreEqual(rect.CenterX, 9.5f);
            Assert.AreEqual(rect.CenterY, 11.0f);
            Assert.AreEqual(rect.Center, new Vector2(9.5f, 11.0f));

            Assert.AreEqual(rect.TopLeft, new Vector2(7, 8));
            Assert.AreEqual(rect.TopRight, new Vector2(12, 8));
            Assert.AreEqual(rect.Top, new Vector2(9.5f, 8.0f));

            Assert.AreEqual(rect.BottomLeft, new Vector2(7, 14));
            Assert.AreEqual(rect.BottomRight, new Vector2(12.0f, 14.0f));
            Assert.AreEqual(rect.Bottom, new Vector2(9.5f, 14.0f));

            Assert.AreEqual(rect.Area, 30.0);
            Assert.AreEqual(rect.Perimeter, 22.0);
        }

        [Test] public void Resize()
        {
            int x = 4;
            int y = 5;
            int w = 5;
            int h = 6;
            Rect rect = new Rect(x, y, w, h);

            Rect resized = rect.Scale(5, 6);
            Assert.AreNotEqual(rect, resized);

            Assert.AreEqual(resized.X, x);
            Assert.AreEqual(resized.Y, y);
            Assert.AreEqual(resized.W, 25.0);
            Assert.AreEqual(resized.H, 36.0);

            Vector2 size = new Vector2(5, 6);
            resized = rect.Scale(size);
            Assert.AreEqual(resized.X, x);
            Assert.AreEqual(resized.Y, y);
            Assert.AreEqual(resized.W, 25.0);
            Assert.AreEqual(resized.H, 36.0);

            size = new Vector2(-5, -6);
            resized = rect.Scale(size);
            Assert.AreEqual(resized.X, x);
            Assert.AreEqual(resized.Y, y);
            Assert.AreEqual(resized.W, -25.0);
            Assert.AreEqual(resized.H, -36.0);
        }

        [Test] public void Transform()
        {
            Rect rect = new Rect(4, 5, 6, 7);

            Rect moved = rect.Transform(5, 6);
            Assert.AreNotEqual(rect, moved);

            Assert.AreEqual(moved.X, 20.0);
            Assert.AreEqual(moved.Y, 30.0);
            Assert.AreEqual(moved.W, 30.0);
            Assert.AreEqual(moved.H, 42.0);

            Vector2 size = new Vector2(5, 6);
            moved = rect.Transform(size);
            Assert.AreEqual(moved.X, 20.0);
            Assert.AreEqual(moved.Y, 30.0);
            Assert.AreEqual(moved.W, 30.0);
            Assert.AreEqual(moved.H, 42.0);

            size = new Vector2(-5, -6);
            moved = rect.Transform(size);
            Assert.AreEqual(moved.X, -20.0);
            Assert.AreEqual(moved.Y, -30.0);
            Assert.AreEqual(moved.W, -30.0);
            Assert.AreEqual(moved.H, -42.0);
        }

        [Test] public void Move()
        {
            int x = 4;
            int y = 5;
            int w = 5;
            int h = 5;
            Rect rect = new Rect(x, y, w, h);
           
            Rect moved = rect.Offset(-4, -5);
            Assert.AreNotEqual(rect, moved);

            Assert.AreEqual(moved.X, 0.0);
            Assert.AreEqual(moved.Y, 0.0);
            Assert.AreEqual(moved.W, w);
            Assert.AreEqual(moved.H, h);

            moved = rect.Offset(-8, -10);
            Assert.AreEqual(moved.X, -4.0);
            Assert.AreEqual(moved.Y, -5.0);
            Assert.AreEqual(moved.W, w);
            Assert.AreEqual(moved.H, h);

            moved = rect.Offset(1020, 1024);
            Assert.AreEqual(moved.X, 1024.0);
            Assert.AreEqual(moved.Y, 1029.0);
            Assert.AreEqual(moved.W, w);
            Assert.AreEqual(moved.H, h);
        }

        [Test] public void Expanding()
        {
            Rect rect = new Rect(20, 25, 30, 40);

            Rect expanded = rect.ExpandToContain(1, 1);
            Assert.AreNotEqual(rect, expanded);

            Assert.AreEqual(expanded.X, 1.0);
            Assert.AreEqual(expanded.Y, 1.0);
            Assert.AreEqual(expanded.W, 49.0);
            Assert.AreEqual(expanded.H, 64.0);

            expanded = rect.ExpandToContain(21, 26);
            Assert.AreEqual(expanded.X, 20.0);
            Assert.AreEqual(expanded.Y, 25.0);
            Assert.AreEqual(expanded.W, 30.0);
            Assert.AreEqual(expanded.H, 40.0);

            expanded = rect.ExpandToContain(55, 70);
            Assert.AreEqual(expanded.X, 20);
            Assert.AreEqual(expanded.Y, 25);
            Assert.AreEqual(expanded.W, 35.0);
            Assert.AreEqual(expanded.H, 45.0);

            expanded = rect.ExpandToContain(0, 0, 5, 5);
            Assert.AreNotEqual(rect, expanded);

            Assert.AreEqual(expanded.X, 0);
            Assert.AreEqual(expanded.Y, 0);
            Assert.AreEqual(expanded.W, 50.0);
            Assert.AreEqual(expanded.H, 65.0);

            Rect testRect = new Rect(0, 0, 5, 5);
            expanded = rect.ExpandToContain(testRect);
            Assert.AreNotEqual(rect, expanded);

            Assert.AreEqual(expanded.X, 0);
            Assert.AreEqual(expanded.Y, 0);
            Assert.AreEqual(expanded.W, 50.0);
            Assert.AreEqual(expanded.H, 65.0);
        }

        [Test] public void Round()
        {
            Rect rect = new Rect(20.6f, 25.4f, 30.4f, 40.6f);
            Rect testResult = new Rect(21f, 25f, 30f, 41f);
            
            Rect result = rect.Round();
            Assert.AreNotEqual(rect, result);

            Assert.AreEqual(result.X, testResult.X);
            Assert.AreEqual(result.Y, testResult.Y);
            Assert.AreEqual(result.W, testResult.W);
            Assert.AreEqual(result.H, testResult.H);
        }

        [Test] public void Ceiling()
        {
            Rect rect = new Rect(20.5f, 25.4f, 30.4f, 40.5f);
            Rect testResult = new Rect(21f, 26f, 31f, 41f);

            Rect result = rect.Ceiling();
            Assert.AreNotEqual(rect, result);

            Assert.AreEqual(result.X, testResult.X);
            Assert.AreEqual(result.Y, testResult.Y);
            Assert.AreEqual(result.W, testResult.W);
            Assert.AreEqual(result.H, testResult.H);
        }

        [Test] public void Floor()
        {
            Rect rect = new Rect(20.5f, 25.4f, 30.4f, 40.5f);
            Rect testResult = new Rect(20f, 25f, 30f, 40f);

            Rect result = rect.Floor();
            Assert.AreNotEqual(rect, result);

            Assert.AreEqual(result.X, testResult.X);
            Assert.AreEqual(result.Y, testResult.Y);
            Assert.AreEqual(result.W, testResult.W);
            Assert.AreEqual(result.H, testResult.H);
        }

        [Test] public void Contains()
        {
            Rect rect = new Rect(0.0f, 0.0f, 50.0f, 50.0f);

            Assert.IsTrue(rect.Contains(10.0f, 10.0f));
            Assert.IsTrue(rect.Contains(0.0f, 0.0f));
            Assert.IsTrue(rect.Contains(50.0f, 50.0f));
            Assert.IsTrue(!rect.Contains(50.0001f, 50.0001f));
            Assert.IsTrue(!rect.Contains(-0.001f, -0.001f));

            Assert.IsTrue(rect.Contains(new Vector2(10.0f, 10.0f)));
            Assert.IsTrue(rect.Contains(new Vector2(0.0f, 0.0f)));
            Assert.IsTrue(rect.Contains(new Vector2(50.0f, 50.0f)));
            Assert.IsTrue(!rect.Contains(new Vector2(50.0001f, 50.0001f)));
            Assert.IsTrue(!rect.Contains(new Vector2(-0.001f, -0.001f)));

            Assert.IsTrue(rect.Contains(new Rect(10.0f, 10.0f, 10.0f, 10.0f)));
            Assert.IsTrue(rect.Contains(new Rect(0.0f, 0.0f, 10.0f, 10.0f)));
            Assert.IsTrue(rect.Contains(new Rect(40.0f, 40.0f, 10.0f, 10.0f)));
            Assert.IsTrue(!rect.Contains(new Rect(50.0f, 50.0f, 10.0f, 10.0f)));
            Assert.IsTrue(!rect.Contains(new Rect(-5.0f, -5.0f, 10.0f, 10.0f)));
            Assert.IsTrue(!rect.Contains(new Rect(-10.0f, -10.0f, 10.0f, 10.0f)));
        }

        [Test] public void Intersects()
        {
            Rect rect = new Rect(0.0f, 0.0f, 50.0f, 50.0f);

            Assert.IsTrue(rect.Intersects(new Rect(10.0f, 10.0f, 10.0f, 10.0f)));
            Assert.IsTrue(rect.Intersects(new Rect(-10.0f, -10.0f, 20.0f, 20.0f)));
            Assert.IsTrue(rect.Intersects(new Rect(45.0f, 0.0f, 20.0f, -20.0f)));
            Assert.IsTrue(rect.Intersects(new Rect(45.0f, 45.0f, 20.0f, 20.0f)));
            Assert.IsTrue(rect.Intersects(new Rect(50.0f, 50.0f, 20.0f, 20.0f)));
            Assert.IsTrue(rect.Intersects(new Rect(-10.0f, -10.0f, 10.0f, 10.0f)));
            Assert.IsTrue(!rect.Intersects(new Rect(-10.0f, -10.0f, 5.0f, 5.0f)));
            Assert.IsTrue(!rect.Intersects(new Rect(51.0f, 0.0f, 5.0f, 5.0f)));
            Assert.IsTrue(!rect.Intersects(new Rect(51.0f, 51.0f, 5.0f, 5.0f)));
            Assert.IsTrue(!rect.Intersects(new Rect(0.0f, 51.0f, 5.0f, 5.0f)));
        }
    }
}
