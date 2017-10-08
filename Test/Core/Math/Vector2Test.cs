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
	public class Vector2Test
	{
		private const float Epsilon = 0.0001f;

		[Test] public void Constuctors()
		{
			AssertVectorEqual(new Vector2(), 0.0f, 0.0f);
			AssertVectorEqual(new Vector2(1.2f), 1.2f, 1.2f);
			AssertVectorEqual(new Vector2(1.2f, 3.4f), 1.2f, 3.4f);
		}
		[Test] public void EqualityChecks()
		{
			Vector2 a = new Vector2(1.2f, 3.4f);
			Vector2 b = new Vector2(1.2f, 3.4f);
			Vector2[] c = new[]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(1.2f, 0.0f),
				new Vector2(0.0f, 3.4f),
				new Vector2(-1.2f, -3.4f)
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
		[Test] public void MathOperators()
		{
			AssertVectorEpsilonEqual(-new Vector2(1.2f, 3.4f), -1.2f, -3.4f);
			AssertVectorEpsilonEqual(new Vector2(1.2f, 3.4f) + new Vector2(1.1f, 2.2f), 2.3f, 5.6f);
			AssertVectorEpsilonEqual(new Vector2(1.2f, 3.4f) - new Vector2(1.1f, 2.2f), 0.1f, 1.2f);
			AssertVectorEpsilonEqual(new Vector2(1.2f, 3.4f) * new Vector2(2.0f, 3.0f), 2.4f, 10.2f);
			AssertVectorEpsilonEqual(new Vector2(1.2f, 3.4f) * 2.0f, 2.4f, 6.8f);
			AssertVectorEpsilonEqual(2.0f * new Vector2(1.2f, 3.4f), 2.4f, 6.8f);
			AssertVectorEpsilonEqual(new Vector2(2.4f, 10.2f) / new Vector2(2.0f, 3.0f), 1.2f, 3.4f);
			AssertVectorEpsilonEqual(new Vector2(2.4f, 6.8f) / 2.0f, 1.2f, 3.4f);
		}
		[Test] public void MinMax()
		{
			AssertVectorEqual(Vector2.Min(new Vector2(1.2f, 3.4f), new Vector2(3.4f, 1.2f)), 1.2f, 1.2f);
			AssertVectorEqual(Vector2.Max(new Vector2(1.2f, 3.4f), new Vector2(3.4f, 1.2f)), 3.4f, 3.4f);
		}
		[Test] public void DotProduct()
		{
			Assert.AreEqual(0.0f, Vector2.Dot(new Vector2(1.0f, 0.0f), new Vector2(0.0f, 1.0f)), Epsilon);
			Assert.AreEqual(1.0f, Vector2.Dot(new Vector2(1.0f, 0.0f), new Vector2(1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector2.Dot(new Vector2(1.0f, 0.0f), new Vector2(-1.0f, 0.0f)), Epsilon);

			Assert.AreEqual(0.0f, Vector2.Dot(new Vector2(0.0f, 1.0f), new Vector2(1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(1.0f, Vector2.Dot(new Vector2(0.0f, 1.0f), new Vector2(0.0f, 1.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector2.Dot(new Vector2(0.0f, 1.0f), new Vector2(0.0f, -1.0f)), Epsilon);

			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector2 vector = rnd.NextVector2();
				Assert.AreEqual(vector.LengthSquared, Vector2.Dot(vector, vector), Epsilon);
			}
		}
		[Test] public void Normalized()
		{
			// Normalizing direction vectors
			for (int i = 0; i < 10; i++)
			{
				float length = MathF.Pow(1.25f, i);
				AssertVectorEpsilonEqual(new Vector2(length, 0.0f).Normalized, 1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector2(0.0f, length).Normalized, 0.0f, 1.0f);
				AssertVectorEpsilonEqual(new Vector2(-length, 0.0f).Normalized, -1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector2(0.0f, -length).Normalized, 0.0f, -1.0f);
				AssertVectorEpsilonEqual(new Vector2(length, length).Normalized, 1.0f / MathF.Sqrt(2), 1.0f / MathF.Sqrt(2));
			}

			// Normalizing random vectors - direction should be preserved
			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				float angle = rnd.NextFloat(0.0f, MathF.TwoPi);
				float length = rnd.NextFloat(0.1f, 2.0f);
				Vector2 vector = Vector2.FromAngleLength(angle, length);
				Vector2 normalizedVector = vector.Normalized;

				Assert.AreEqual(vector.Angle, normalizedVector.Angle, Epsilon);
				Assert.AreEqual(1.0f, normalizedVector.Length, Epsilon);
				AssertVectorEpsilonEqual(normalizedVector * length, vector.X, vector.Y);
			}

			// Normalizing a zero-length vector should return a zero-length vector
			AssertVectorEqual(new Vector2(0.0f, 0.0f).Normalized, 0.0f, 0.0f);

			// Normalizing an increasingly small vector should never produce any invalid vectors
			for (int i = 0; i < 50; i++)
			{
				float scale = MathF.Pow(10.0f, -i);
				Vector2 vector = new Vector2(scale, scale);
				Vector2 normalizedVector = vector.Normalized;

				Assert.IsFalse(float.IsNaN(normalizedVector.X));
				Assert.IsFalse(float.IsNaN(normalizedVector.Y));
				Assert.IsFalse(float.IsInfinity(normalizedVector.X));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Y));
			}
		}
		[Test] public void Normalize()
		{
			// Normalizing random vectors - expect the same results as with Normalized
			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector2 vector = rnd.NextVector2();
				Vector2 normalizedVector = vector;
				normalizedVector.Normalize();

				AssertVectorEpsilonEqual(normalizedVector, vector.Normalized.X, vector.Normalized.Y);
			}

			// Normalizing a zero-length vector should return a zero-length vector
			AssertVectorEqual(new Vector2(0.0f, 0.0f).Normalized, 0.0f, 0.0f);

			// Normalizing an increasingly small vector should never produce any invalid vectors
			for (int i = 0; i < 50; i++)
			{
				float scale = MathF.Pow(10.0f, -i);
				Vector2 vector = new Vector2(scale, scale);
				Vector2 normalizedVector = vector;
				normalizedVector.Normalize();

				Assert.IsFalse(float.IsNaN(normalizedVector.X));
				Assert.IsFalse(float.IsNaN(normalizedVector.Y));
				Assert.IsFalse(float.IsInfinity(normalizedVector.X));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Y));
			}
		}
		[Test] public void Perpendicular()
		{
			AssertVectorEqual(new Vector2(0.0f, -1.0f).PerpendicularLeft, -1.0f, 0.0f);
			AssertVectorEqual(new Vector2(1.0f, 0.0f).PerpendicularLeft, 0.0f, -1.0f);
			AssertVectorEqual(new Vector2(0.0f, 1.0f).PerpendicularLeft, 1.0f, 0.0f);
			AssertVectorEqual(new Vector2(-1.0f, 0.0f).PerpendicularLeft, 0.0f, 1.0f);

			AssertVectorEqual(new Vector2(0.0f, -1.0f).PerpendicularRight, 1.0f, 0.0f);
			AssertVectorEqual(new Vector2(1.0f, 0.0f).PerpendicularRight, 0.0f, 1.0f);
			AssertVectorEqual(new Vector2(0.0f, 1.0f).PerpendicularRight, -1.0f, 0.0f);
			AssertVectorEqual(new Vector2(-1.0f, 0.0f).PerpendicularRight, 0.0f, -1.0f);

			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector2 vector = rnd.NextVector2();
				Vector2 left = vector.PerpendicularLeft;
				Vector2 right = vector.PerpendicularRight;

				Assert.AreEqual(MathF.RadAngle90, MathF.CircularDist(vector.Angle, left.Angle), Epsilon);
				Assert.AreEqual(MathF.RadAngle90, MathF.CircularDist(vector.Angle, right.Angle), Epsilon);

				Assert.AreEqual(-1.0f, MathF.TurnDir(vector.Angle, left.Angle));
				Assert.AreEqual(1.0f, MathF.TurnDir(vector.Angle, right.Angle));
			}
		}
		[Test] public void FromAngleLength()
		{
			AssertVectorEpsilonEqual(Vector2.FromAngleLength(0.0f, 1.0f), 0.0f, -1.0f);
			AssertVectorEpsilonEqual(Vector2.FromAngleLength(MathF.RadAngle90, 1.0f), 1.0f, 0.0f);
			AssertVectorEpsilonEqual(Vector2.FromAngleLength(MathF.RadAngle180, 1.0f), 0.0f, 1.0f);
			AssertVectorEpsilonEqual(Vector2.FromAngleLength(MathF.RadAngle270, 1.0f), -1.0f, 0.0f);

			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				float angle = rnd.NextFloat(0.0f, MathF.TwoPi);
				float length = rnd.NextFloat(0.1f, 2.0f);
				Vector2 vector = Vector2.FromAngleLength(angle, length);

				Assert.AreEqual(angle, vector.Angle, Epsilon);
				Assert.AreEqual(length, vector.Length, Epsilon);
			}
		}
		[Test] public void Angle()
		{
			Assert.AreEqual(0.0f, new Vector2(0.0f, -1.0f).Angle, Epsilon);
			Assert.AreEqual(MathF.RadAngle90, new Vector2(1.0f, 0.0f).Angle, Epsilon);
			Assert.AreEqual(MathF.RadAngle180, new Vector2(0.0f, 1.0f).Angle, Epsilon);
			Assert.AreEqual(MathF.RadAngle270, new Vector2(-1.0f, 0.0f).Angle, Epsilon);
		}
		[Test] public void Length()
		{
			Assert.AreEqual(1.0f, new Vector2(1.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector2(0.0f, 1.0f).Length, Epsilon);
			Assert.AreEqual(MathF.Sqrt(2), new Vector2(1.0f, 1.0f).Length, Epsilon);
		}
		[Test] public void LengthSquared()
		{
			Assert.AreEqual(1.0f, new Vector2(1.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector2(0.0f, 1.0f).LengthSquared, Epsilon);
			Assert.AreEqual(2.0f, new Vector2(1.0f, 1.0f).LengthSquared, Epsilon);
		}
		[Test] public void Indexing()
		{
			Assert.AreEqual(1.2f, new Vector2(1.2f, 3.4f)[0]);
			Assert.AreEqual(3.4f, new Vector2(1.2f, 3.4f)[1]);
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector2(1.2f, 3.4f)[-1]; });
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector2(1.2f, 3.4f)[2]; });

			Vector2 vector = new Vector2(0.0f, 0.0f);
			vector[0] = 1.2f;
			vector[1] = 3.4f;
			Assert.AreEqual(1.2f, vector.X);
			Assert.AreEqual(3.4f, vector.Y);

			Assert.Throws<IndexOutOfRangeException>(() => { vector[-1] = 0; });
			Assert.Throws<IndexOutOfRangeException>(() => { vector[2] = 0; });
		}

		private void AssertVectorEqual(Vector2 vector, float x, float y)
		{
			Assert.AreEqual(x, vector.X);
			Assert.AreEqual(y, vector.Y);
		}
		private void AssertVectorEpsilonEqual(Vector2 vector, float x, float y)
		{
			Assert.AreEqual(x, vector.X, Epsilon);
			Assert.AreEqual(y, vector.Y, Epsilon);
		}
	}
}
