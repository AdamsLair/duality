using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Drawing;

using OpenTK;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class GenericOperatorTest
	{
		[Test] public void IntMath()
		{
			Assert.AreEqual(2 + 3, GenericOperator.Add(2, 3));
			Assert.AreEqual(2 - 3, GenericOperator.Subtract(2, 3));
			Assert.AreEqual(2 * 3, GenericOperator.Multiply(2, 3));
			Assert.AreEqual(2 / 3, GenericOperator.Divide(2, 3));

			Assert.AreEqual(Math.Abs(-5), GenericOperator.Abs(-5));
			Assert.AreEqual(5 % 4, GenericOperator.Modulo(5, 4));
			Assert.AreEqual(-5, GenericOperator.Negate(5));

			Assert.AreEqual(5 == 4, GenericOperator.Equal(5, 4));
			Assert.AreEqual(5 == 5, GenericOperator.Equal(5, 5));
			Assert.AreEqual(5 > 4, GenericOperator.GreaterThan(5, 4));
			Assert.AreEqual(5 >= 5, GenericOperator.GreaterThanOrEqual(5, 5));
			Assert.AreEqual(5 < 4, GenericOperator.LessThan(5, 4));
			Assert.AreEqual(5 <= 5, GenericOperator.LessThanOrEqual(5, 5));
		}
		[Test] public void FloatMath()
		{
			Assert.AreEqual(2f + 3f, GenericOperator.Add(2f, 3f));
			Assert.AreEqual(2f - 3f, GenericOperator.Subtract(2f, 3f));
			Assert.AreEqual(2f * 3f, GenericOperator.Multiply(2f, 3f));
			Assert.AreEqual(2f / 3f, GenericOperator.Divide(2f, 3f));

			Assert.AreEqual(Math.Abs(-5f), GenericOperator.Abs(-5f));
			Assert.AreEqual(5f % 4f, GenericOperator.Modulo(5f, 4f));
			Assert.AreEqual(-5f, GenericOperator.Negate(5f));

			Assert.AreEqual(5f == 4f, GenericOperator.Equal(5f, 4f));
			Assert.AreEqual(5f == 5f, GenericOperator.Equal(5f, 5f));
			Assert.AreEqual(5f > 4f, GenericOperator.GreaterThan(5f, 4f));
			Assert.AreEqual(5f >= 5f, GenericOperator.GreaterThanOrEqual(5f, 5f));
			Assert.AreEqual(5f < 4f, GenericOperator.LessThan(5f, 4f));
			Assert.AreEqual(5f <= 5f, GenericOperator.LessThanOrEqual(5f, 5f));
		}
		[Test] public void VectorMath()
		{
			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) + new Vector2(1.0f, 3.0f),
				GenericOperator.Add(new Vector2(2.0f, 0.5f), new Vector2(1.0f, 3.0f)));
			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) - new Vector2(1.0f, 3.0f),
				GenericOperator.Subtract(new Vector2(2.0f, 0.5f), new Vector2(1.0f, 3.0f)));
			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) * new Vector2(1.0f, 3.0f),
				GenericOperator.Multiply(new Vector2(2.0f, 0.5f), new Vector2(1.0f, 3.0f))); 
			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) / new Vector2(1.0f, 3.0f),
				GenericOperator.Divide(new Vector2(2.0f, 0.5f), new Vector2(1.0f, 3.0f)));

			Assert.AreEqual(
				-new Vector2(2.0f, 0.5f),
				GenericOperator.Negate(new Vector2(2.0f, 0.5f)));

			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) == new Vector2(1.0f, 3.0f),
				GenericOperator.Equal(new Vector2(2.0f, 0.5f), new Vector2(1.0f, 3.0f)));
			Assert.AreEqual(
				new Vector2(2.0f, 0.5f) == new Vector2(2.0f, 0.5f),
				GenericOperator.Equal(new Vector2(2.0f, 0.5f), new Vector2(2.0f, 0.5f))); 
		}
		[Test] public void ColorRgbaMath()
		{
			Assert.AreEqual(
				ColorRgba.Red + ColorRgba.Blue,
				GenericOperator.Add(ColorRgba.Red, ColorRgba.Blue));
			Assert.AreEqual(
				ColorRgba.Red - ColorRgba.Blue,
				GenericOperator.Subtract(ColorRgba.Red, ColorRgba.Blue));
			Assert.AreEqual(
				ColorRgba.Red * ColorRgba.Blue,
				GenericOperator.Multiply(ColorRgba.Red, ColorRgba.Blue));

			Assert.AreEqual(
				ColorRgba.Red == ColorRgba.Blue,
				GenericOperator.Equal(ColorRgba.Red, ColorRgba.Blue));
			Assert.AreEqual(
				ColorRgba.Red == ColorRgba.Red,
				GenericOperator.Equal(ColorRgba.Red, ColorRgba.Red));
		}
		[Test] public void LogicalOperation()
		{
			Assert.AreEqual(0x0123456 & 0x654321, GenericOperator.And(0x0123456, 0x654321));
			Assert.AreEqual(0x0123456 | 0x654321, GenericOperator.Or(0x0123456, 0x654321));
			Assert.AreEqual(0x0123456 ^ 0x654321, GenericOperator.Xor(0x0123456, 0x654321));
			Assert.AreEqual(~0x0123456, GenericOperator.Not(0x0123456));
		}
		[Test] public void ConvertOperation()
		{
			Assert.AreEqual((float)5, GenericOperator.Convert<int,float>(5));
			Assert.AreEqual((int)5.5f, GenericOperator.Convert<float,int>(5.5f));
			Assert.AreEqual((decimal)5.5f, GenericOperator.Convert<float,decimal>(5.5f));
			Assert.AreEqual((int)ColorRgba.Red, GenericOperator.Convert<ColorRgba,int>(ColorRgba.Red));
		}
		[Test] public void LerpOperation()
		{
			Assert.AreEqual(0.6f, GenericOperator.Lerp(0.0f, 1.0f, 0.6f));

			Assert.AreEqual(0, GenericOperator.Lerp(0, 1, 0.61f));
			Assert.AreEqual(1, GenericOperator.Lerp(0, 2, 0.61f));
			Assert.AreEqual(6, GenericOperator.Lerp(0, 10, 0.61f));

			Assert.AreEqual(ColorRgba.Lerp(ColorRgba.Red, ColorRgba.Blue, 0.6f), GenericOperator.Lerp(ColorRgba.Red, ColorRgba.Blue, 0.6f));
			Assert.AreEqual(Vector2.Lerp(Vector2.UnitX, Vector2.UnitY, 0.6f), GenericOperator.Lerp(Vector2.UnitX, Vector2.UnitY, 0.6f));
		}
	}
}
