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
            checkRect(rect, 0.0f, 0.0f, 0.0f, 0.0f);

            rect = new Rect(1, 2, 3, 4);
            checkRect(rect, 1.0f, 2.0f, 3.0f, 4.0f);

            rect = new Rect(3, 4);
            checkRect(rect, 0.0f, 0.0f, 3.0f, 4.0f);

            rect = new Rect(-3, -4);
            checkRect(rect, 0.0f, 0.0f, -3.0f, -4.0f);

            Assert.AreNotSame(rect.MaximumX, 0.0);
            Assert.AreNotSame(rect.MinimumX, -3.0);
            Assert.AreNotSame(rect.MaximumY, 0.0);
            Assert.AreNotSame(rect.MinimumY, -4.0);

            Vector2 size = new Vector2(5, 6);
            rect = new Rect(size);
            checkRect(rect, 0.0f, 0.0f, 5.0f, 6.0f);

            rect.Pos = new Vector2(7, 8);
            checkRect(rect, 7.0f, 8.0f, 5.0f, 6.0f);

            Assert.AreNotSame(rect.Size, new Vector2(5, 6));

            Assert.AreNotSame(rect.Pos, new Vector2(7, 8));
            Assert.AreNotSame(rect.MaximumX, 12.0f);
            Assert.AreNotSame(rect.MinimumX, 7.0f);
            Assert.AreNotSame(rect.MaximumY, 14.0f);
            Assert.AreNotSame(rect.MinimumY, 8.0f);

            Assert.AreNotSame(rect.CenterX, 9.5f);
            Assert.AreNotSame(rect.CenterY, 11.0f);
            Assert.AreNotSame(rect.Center, new Vector2(9.5f, 11.0f));

            Assert.AreNotSame(rect.TopLeft, new Vector2(7, 8));
            Assert.AreNotSame(rect.TopRight, new Vector2(12, 8));
            Assert.AreNotSame(rect.Top, new Vector2(9.5f, 8.0f));

            Assert.AreNotSame(rect.BottomLeft, new Vector2(7, 14));
            Assert.AreNotSame(rect.BottomRight, new Vector2(12.0f, 14.0f));
            Assert.AreNotSame(rect.Bottom, new Vector2(9.5f, 14.0f));

            Assert.AreNotSame(rect.Area, 30.0);
            Assert.AreNotSame(rect.Perimeter, 22.0);
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
            checkRect(resized, x, y, 25.0f, 36.0f);

            Vector2 size = new Vector2(5, 6);
            resized = rect.Scale(size);
            checkRect(resized, x, y, 25.0f, 36.0f);

            size = new Vector2(-5, -6);
            resized = rect.Scale(size);
            checkRect(resized, x, y, -25.0f, -36.0f);
        }

        [Test] public void Transform()
        {
            Rect rect = new Rect(4, 5, 6, 7);

            Rect moved = rect.Transform(5, 6);
            Assert.AreNotEqual(rect, moved);

            checkRect(moved, 20.0f, 30.0f, 30.0f, 42.0f);

            Vector2 size = new Vector2(5, 6);
            moved = rect.Transform(size);

            checkRect(moved, 20.0f, 30.0f, 30.0f, 42.0f);

            size = new Vector2(-5, -6);
            moved = rect.Transform(size);
            checkRect(moved, -20.0f, -30.0f, -30.0f, -42.0f);
        }

        [Test] public void Move()
        {
            int x = 4;
            int y = 5;
            int w = 5;
            int h = 5;
            Rect rect = new Rect(x, y, w, h);
           
            Rect moved = rect.Offset(-4, -5);
            Assert.AreNotSame(rect, moved);
            checkRect(moved, 0.0f, 0.0f, w, h);

            moved = rect.Offset(-8, -10);
            checkRect(moved, -4.0f, -5.0f, w, h);

            moved = rect.Offset(1020, 1024);
            checkRect(moved, 1024.0f, 1029.0f, w, h);
        }

        [Test] public void Expanding()
        {
            Rect rect = new Rect(20, 25, 30, 40);

            Rect expanded = rect.ExpandToContain(1, 1);
            Assert.AreNotSame(rect, expanded);
            checkRect(expanded, 1.0f, 1.0f, 49.0f, 64.0f);

            expanded = rect.ExpandToContain(21, 26);
            checkRect(expanded, 20.0f, 25.0f, 30.0f, 40.0f);

            expanded = rect.ExpandToContain(55, 70);
            checkRect(expanded, 20.0f, 25.0f, 35.0f, 45.0f);
            
            expanded = rect.ExpandToContain(0, 0, 5, 5);
            Assert.AreNotSame(rect, expanded);
            checkRect(expanded, 0.0f, 0.0f, 50.0f, 65.0f);

            Rect testRect = new Rect(0, 0, 5, 5);
            expanded = rect.ExpandToContain(testRect);
            Assert.AreNotSame(rect, expanded);
            checkRect(expanded, 0.0f, 0.0f, 50.0f, 65.0f);
        }

        [Test] public void Round()
        {
            Rect rect = new Rect(20.6f, 25.4f, 30.4f, 40.6f);
            Rect testResult = new Rect(21f, 25f, 30f, 41f);
            
            Rect result = rect.Round();
            Assert.AreNotSame(rect, result);

            checkRect(result, testResult.X, testResult.Y, testResult.W, testResult.H);
        }

        [Test] public void Ceiling()
        {
            Rect rect = new Rect(20.5f, 25.4f, 30.4f, 40.5f);
            Rect testResult = new Rect(21f, 26f, 31f, 41f);

            Rect result = rect.Ceiling();
            Assert.AreNotSame(rect, result);
            checkRect(result, testResult.X, testResult.Y, testResult.W, testResult.H);
        }

        [Test] public void Floor()
        {
            Rect rect = new Rect(20.5f, 25.4f, 30.4f, 40.5f);
            Rect testResult = new Rect(20f, 25f, 30f, 40f);

            Rect result = rect.Floor();
            Assert.AreNotSame(rect, result);
            checkRect(result, testResult.X, testResult.Y, testResult.W, testResult.H);
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

        //        x     y       w       h       iX      iY      iW      iH   
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  10.0f,  10.0f,  10.0f,  10.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  -10.0f, -10.0f, 20.0f,  20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  40.0f,  0.0f,   20.0f,  -20.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  0.0f,   40.0f,  -20.0f, 20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  45.0f,  45.0f,  20.0f,  20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  50.0f,  50.0f,  20.0f,  20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  -10.0f, -10.0f, 10.0f,  10.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  51.0f,  51.0f,  -20.0f, -20.0f, Result = true, Description = "W and X less than zero")]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  -10.0f, -10.0f, 5.0f,   5.0f,   Result = false)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  51.0f,  0.0f,   5.0f,   5.0f,   Result = false)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  51.0f,  51.0f,  5.0f,   5.0f,   Result = false)]
        [TestCase(0.0f, 0.0f,   50.0f,  50.0f,  0.0f,   51.0f,  5.0f,   5.0f,   Result = false)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -10.0f, -10.0f, -10.0f, -10.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, 10.0f,  10.0f,  -20.0f, -20.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -40.0f, 0.0f,   -20.0f, 20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, 0.0f,   -40.0f, 20.0f,  -20.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -45.0f, -45.0f, -20.0f, -20.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -50.0f, -50.0f, -20.0f, -20.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, 10.0f,  10.0f,  -10.0f, -10.0f, Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -51.0f, -51.0f, 20.0f,  20.0f,  Result = true)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, 10.0f,  10.0f,  -5.0f,  -5.0f,  Result = false)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -51.0f, 0.0f,   -5.0f,  -5.0f,  Result = false)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -51.0f, -51.0f, -5.0f,  -5.0f,  Result = false)]
        [TestCase(0.0f, 0.0f,   -50.0f, -50.0f, -0.0f,  -51.0f, -5.0f,  -5.0f,  Result = false)]
        public bool Intersects(float x, float y, float w, float h, float intersectedX, float intersectedY, float intersectedW, float intersectedH)
        {
            Rect rect = new Rect(x, y, w, h);
            return rect.Intersects(new Rect(intersectedX, intersectedY, intersectedW, intersectedH));
        }

        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 0.0f, 50.0f,  50.0f, Result = true, Description = "equals")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f,  50.0f, Result = false, Description = "X greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 1.0f, 50.0f,  50.0f, Result = false, Description = "Y greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 6.0f,   50.0f, Result = false, Description = "W greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f,  60.0f, Result = false, Description = "H greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, -1.0f, 0.0f, 50.0f, 50.0f, Result = false, Description = "X smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, -1.0f, 50.0f, 50.0f, Result = false, Description = "Y smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, -6.0f,  50.0f, Result = false, Description = "W smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, -60.0f, Result = false, Description = "H smaller")]
        public bool EqualsFuntion(float x, float y, float w, float h, float equalsX, float equalsY, float equalsW, float equalsH)
        {
            Rect rect = new Rect(x, y, w, h);
            Rect equals = new Rect(equalsX, equalsY, equalsW, equalsH);

            return rect.Equals(equals);
        }

        [Test]
        public void EqualsToNoRect()
        {
            Rect rect = new Rect(0.0f, 0.0f, 50.0f, 50.0f);
            int notRect = 2;
            Assert.IsTrue(!rect.Equals(notRect));
        }

        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 0.0f, 50.0f, 50.0f, Result = true, Description = "equals")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, 50.0f, Result = false, Description = "X greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 1.0f, 50.0f, 50.0f, Result = false, Description = "Y greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 6.0f, 50.0f, Result = false, Description = "W greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, 60.0f, Result = false, Description = "H greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, -1.0f, 0.0f, 50.0f, 50.0f, Result = false, Description = "X smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, -1.0f, 50.0f, 50.0f, Result = false, Description = "Y smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, -6.0f, 50.0f, Result = false, Description = "W smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, -60.0f, Result = false, Description = "H smaller")]
        public bool EqualsOperator(float x, float y, float w, float h, float equalsX, float equalsY, float equalsW, float equalsH)
        {
            Rect rect = new Rect(x, y, w, h);
            Rect equals = new Rect(equalsX, equalsY, equalsW, equalsH);
            return rect == equals;
        }

        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 0.0f, 50.0f, 50.0f, Result = false, Description = "equals")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, 50.0f, Result = true, Description = "X greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, 1.0f, 50.0f, 50.0f, Result = true, Description = "Y greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 6.0f, 50.0f, Result = true, Description = "W greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, 60.0f, Result = true, Description = "H greater")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, -1.0f, 0.0f, 50.0f, 50.0f, Result = true, Description = "X smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 0.0f, -1.0f, 50.0f, 50.0f, Result = true, Description = "Y smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, -6.0f, 50.0f, Result = true, Description = "W smaller")]
        [TestCase(0.0f, 0.0f, 50.0f, 50.0f, 1.0f, 0.0f, 50.0f, -60.0f, Result = true, Description = "H smaller")]
        public bool NotEqualsOperator(float x, float y, float w, float h, float equalsX, float equalsY, float equalsW, float equalsH)
        {
            Rect rect = new Rect(x, y, w, h);
            Rect equals = new Rect(equalsX, equalsY, equalsW, equalsH);
            return rect != equals;
        }

        [Test]
        public void AlignMethods()
        {         
            foreach (Alignment aligment in  Enum.GetValues(typeof(Alignment)))
            {
                // note : maybe moc will be better
                Rect rect = Rect.Align(aligment, 10.0f, 10.0f, 10.0f, 10.0f);
                switch (aligment)
			    {
				    default:
				    case Alignment.TopLeft:	        Assert.IsTrue(rect.Equals(Rect.AlignTopLeft(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.TopRight:        Assert.IsTrue(rect.Equals(Rect.AlignTopRight(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.BottomLeft:      Assert.IsTrue(rect.Equals(Rect.AlignBottomLeft(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.BottomRight:     Assert.IsTrue(rect.Equals(Rect.AlignBottomRight(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.Center:          Assert.IsTrue(rect.Equals(Rect.AlignCenter(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.Bottom:          Assert.IsTrue(rect.Equals(Rect.AlignBottom(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.Left:            Assert.IsTrue(rect.Equals(Rect.AlignLeft(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.Right:           Assert.IsTrue(rect.Equals(Rect.AlignRight(10.0f, 10.0f, 10.0f, 10.0f))); break;
                    case Alignment.Top:             Assert.IsTrue(rect.Equals(Rect.AlignTop(10.0f, 10.0f, 10.0f, 10.0f))); break;
			    }
            }
        }

        [Test]
        public void AlignCenter()
        {
            Rect rect = Rect.AlignCenter(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 40.0f, 40.0f, 20.0f, 20.0f);

            rect = Rect.AlignCenter(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -60.0f, -40.0f, 20.0f, 20.0f);

            rect = Rect.AlignCenter(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 60.0f, 40.0f, -20.0f, -20.0f);

            rect = Rect.AlignCenter(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -40.0f, -60.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignTop()
        {
            Rect rect = Rect.AlignTop(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 40.0f, 50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTop(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -40.0f, -50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTop(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 60.0f, 30.0f, -20.0f, -20.0f);

            rect = Rect.AlignTop(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -40.0f, -70.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignBottom()
        {
            Rect rect = Rect.AlignBottom(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 40.0f, 30.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottom(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -60.0f, -30.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottom(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 40.0f, 50.0f, -20.0f, -20.0f);

            rect = Rect.AlignBottom(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -40.0f, -50.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignLeft()
        {
            Rect rect = Rect.AlignLeft(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 50.0f, 40.0f, 20.0f, 20.0f);

            rect = Rect.AlignLeft(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -50.0f, -60.0f, 20.0f, 20.0f);

            rect = Rect.AlignLeft(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 70.0f, 40.0f, -20.0f, -20.0f);

            rect = Rect.AlignLeft(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -50.0f, -30.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignRight()
        {
            Rect rect = Rect.AlignRight(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 30.0f, 40.0f, 20.0f, 20.0f);

            rect = Rect.AlignRight(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -70.0f, -60.0f, 20.0f, 20.0f);

            rect = Rect.AlignRight(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 50.0f, 40.0f, -20.0f, -20.0f);

            rect = Rect.AlignRight(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -30.0f, -40.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignTopLeft()
        {
            Rect rect = Rect.AlignTopLeft(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 50.0f, 50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTopLeft(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -50.0f, -50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTopLeft(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 30.0f, 30.0f, -20.0f, -20.0f);

            rect = Rect.AlignTopLeft(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -30.0f, -70.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignTopRight()
        {
            Rect rect = Rect.AlignTopRight(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 30.0f, 50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTopRight(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -70.0f, -50.0f, 20.0f, 20.0f);

            rect = Rect.AlignTopRight(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 50.0f, 30.0f, -20.0f, -20.0f);

            rect = Rect.AlignTopRight(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -50.0f, -70.0f, -20.0f, -20.0f);
        }

     
        [Test]
        public void AlignBottomLeft()
        {
            Rect rect = Rect.AlignBottomLeft(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 30.0f, 70.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottomLeft(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -50.0f, -30.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottomLeft(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 50.0f, 70.0f, -20.0f, -20.0f);

            rect = Rect.AlignBottomLeft(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -30.0f, -70.0f, -20.0f, -20.0f);
        }

        [Test]
        public void AlignBottomRight()
        {
            Rect rect = Rect.AlignBottomRight(50.0f, 50.0f, 20.0f, 20.0f);
            checkRect(rect, 30.0f, 70.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottomRight(-50.0f, -50.0f, 20.0f, 20.0f);
            checkRect(rect, -50.0f, -30.0f, 20.0f, 20.0f);

            rect = Rect.AlignBottomRight(50.0f, 50.0f, -20.0f, -20.0f);
            checkRect(rect, 30.0f, 70.0f, -20.0f, -20.0f);

            rect = Rect.AlignBottomRight(-50.0f, -50.0f, -20.0f, -20.0f);
            checkRect(rect, -50.0f, -50.0f, -20.0f, -20.0f);
        }

        public void checkRect(Rect rect, float x, float y, float w, float h)
        {
            Assert.AreEqual(rect.X, x);
            Assert.AreEqual(rect.Y, y);
            Assert.AreEqual(rect.W, w);
            Assert.AreEqual(rect.H, h);
        }
    }
}
