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
	public class Vector3Test
	{
		private const float Epsilon = 0.000001f;

		[Test] public void Constuctors()
		{
			AssertVectorEqual(new Vector3(), 0.0f, 0.0f, 0.0f);
			AssertVectorEqual(new Vector3(1.2f), 1.2f, 1.2f, 1.2f);
			AssertVectorEqual(new Vector3(new Vector2(1.2f, 3.4f)), 1.2f, 3.4f, 0.0f);
			AssertVectorEqual(new Vector3(new Vector2(1.2f, 3.4f), 5.6f), 1.2f, 3.4f, 5.6f);
			AssertVectorEqual(new Vector3(1.2f, 3.4f, 5.6f), 1.2f, 3.4f, 5.6f);
		}
		[Test] public void EqualityChecks()
		{
			Vector3 a = new Vector3(1.2f, 3.4f, 5.6f);
			Vector3 b = new Vector3(1.2f, 3.4f, 5.6f);
			Vector3[] c = new[]
			{
				new Vector3(0.0f, 0.0f, 0.0f),
				new Vector3(1.2f, 0.0f, 0.0f),
				new Vector3(1.2f, 3.4f, 0.0f),
				new Vector3(1.2f, 0.0f, 5.6f),
				new Vector3(0.0f, 3.4f, 0.0f),
				new Vector3(0.0f, 0.0f, 5.6f),
				new Vector3(0.0f, 3.4f, 5.6f),
				new Vector3(-1.2f, -3.4f, -5.6f)
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
			AssertVectorEpsilonEqual(-new Vector3(1.2f, 3.4f, 5.6f), -1.2f, -3.4f, -5.6f);
			AssertVectorEpsilonEqual(new Vector3(1.2f, 3.4f, 5.6f) + new Vector3(1.1f, 2.2f, 3.3f), 2.3f, 5.6f, 8.9f);
			AssertVectorEpsilonEqual(new Vector3(1.2f, 3.4f, 5.6f) - new Vector3(1.1f, 2.2f, 3.3f), 0.1f, 1.2f, 2.3f);
			AssertVectorEpsilonEqual(new Vector3(1.2f, 3.4f, 5.6f) * new Vector3(2.0f, 3.0f, 4.0f), 2.4f, 10.2f, 22.4f);
			AssertVectorEpsilonEqual(new Vector3(1.2f, 3.4f, 5.6f) * 2.0f, 2.4f, 6.8f, 11.2f);
			AssertVectorEpsilonEqual(2.0f * new Vector3(1.2f, 3.4f, 5.6f), 2.4f, 6.8f, 11.2f);
			AssertVectorEpsilonEqual(new Vector3(2.4f, 10.2f, 22.4f) / new Vector3(2.0f, 3.0f, 4.0f), 1.2f, 3.4f, 5.6f);
			AssertVectorEpsilonEqual(new Vector3(2.4f, 6.8f, 11.2f) / 2.0f, 1.2f, 3.4f, 5.6f);
		}
		[Test] public void MinMax()
		{
			AssertVectorEqual(Vector3.Min(new Vector3(1.2f, 3.4f, 5.6f), new Vector3(5.6f, 3.5f, 1.2f)), 1.2f, 3.4f, 1.2f);
			AssertVectorEqual(Vector3.Max(new Vector3(1.2f, 3.4f, 5.6f), new Vector3(5.6f, 3.5f, 1.2f)), 5.6f, 3.5f, 5.6f);
		}
		[Test] public void DotProduct()
		{
			Assert.AreEqual(1.0f, Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(-1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f)), Epsilon);

			Assert.AreEqual(1.0f, Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f)), Epsilon);

			Assert.AreEqual(1.0f, Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, -1.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector3.Dot(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f)), Epsilon);

			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector3 vector = rnd.NextVector3();
				Assert.AreEqual(vector.LengthSquared, Vector3.Dot(vector, vector), Epsilon);
			}
		}
		[Test] public void Normalized()
		{
			// Normalizing direction vectors
			for (int i = 0; i < 10; i++)
			{
				float length = MathF.Pow(1.25f, i);
				AssertVectorEpsilonEqual(new Vector3(length, 0.0f, 0.0f).Normalized, 1.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector3(0.0f, length, 0.0f).Normalized, 0.0f, 1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector3(0.0f, 0.0f, length).Normalized, 0.0f, 0.0f, 1.0f);
				AssertVectorEpsilonEqual(new Vector3(-length, 0.0f, 0.0f).Normalized, -1.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector3(0.0f, -length, 0.0f).Normalized, 0.0f, -1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector3(0.0f, 0.0f, -length).Normalized, 0.0f, 0.0f, -1.0f);
				AssertVectorEpsilonEqual(new Vector3(length, length, length).Normalized, 1.0f / MathF.Sqrt(3), 1.0f / MathF.Sqrt(3), 1.0f / MathF.Sqrt(3));
			}

			// Normalizing random vectors - direction should be preserved
			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				float length = rnd.NextFloat(0.1f, 2.0f);
				Vector3 vector = rnd.NextVector3(length);
				Vector3 normalizedVector = vector.Normalized;

				float angleToX = Vector3.AngleBetween(vector, Vector3.UnitX);
				float angleToY = Vector3.AngleBetween(vector, Vector3.UnitY);
				float angleToZ = Vector3.AngleBetween(vector, Vector3.UnitZ);

				Assert.AreEqual(1.0f, normalizedVector.Length, Epsilon);
				AssertVectorEpsilonEqual(normalizedVector * length, vector.X, vector.Y, vector.Z);
				Assert.AreEqual(angleToX, Vector3.AngleBetween(normalizedVector, Vector3.UnitX), Epsilon);
				Assert.AreEqual(angleToY, Vector3.AngleBetween(normalizedVector, Vector3.UnitY), Epsilon);
				Assert.AreEqual(angleToZ, Vector3.AngleBetween(normalizedVector, Vector3.UnitZ), Epsilon);
			}

			// Normalizing a zero-length vector should return a zero-length vector
			AssertVectorEqual(new Vector3(0.0f, 0.0f, 0.0f).Normalized, 0.0f, 0.0f, 0.0f);

			// Normalizing an increasingly small vector should never produce any invalid vectors
			for (int i = 0; i < 50; i++)
			{
				float scale = MathF.Pow(10.0f, -i);
				Vector3 vector = new Vector3(scale, scale, scale);
				Vector3 normalizedVector = vector.Normalized;

				Assert.IsFalse(float.IsNaN(normalizedVector.X));
				Assert.IsFalse(float.IsNaN(normalizedVector.Y));
				Assert.IsFalse(float.IsNaN(normalizedVector.Z));
				Assert.IsFalse(float.IsInfinity(normalizedVector.X));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Y));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Z));
			}
		}
		[Test] public void Normalize()
		{
			// Normalizing random vectors - expect the same results as with Normalized
			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector3 vector = rnd.NextVector3();
				Vector3 normalizedVector = vector;
				normalizedVector.Normalize();

				AssertVectorEpsilonEqual(normalizedVector, vector.Normalized.X, vector.Normalized.Y, vector.Normalized.Z);
			}

			// Normalizing a zero-length vector should return a zero-length vector
			AssertVectorEqual(new Vector3(0.0f, 0.0f, 0.0f).Normalized, 0.0f, 0.0f, 0.0f);

			// Normalizing an increasingly small vector should never produce any invalid vectors
			for (int i = 0; i < 50; i++)
			{
				float scale = MathF.Pow(10.0f, -i);
				Vector3 vector = new Vector3(scale, scale, scale);
				Vector3 normalizedVector = vector;
				normalizedVector.Normalize();

				Assert.IsFalse(float.IsNaN(normalizedVector.X));
				Assert.IsFalse(float.IsNaN(normalizedVector.Y));
				Assert.IsFalse(float.IsNaN(normalizedVector.Z));
				Assert.IsFalse(float.IsInfinity(normalizedVector.X));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Y));
				Assert.IsFalse(float.IsInfinity(normalizedVector.Z));
			}
		}
		[Test] public void Length()
		{
			Assert.AreEqual(1.0f, new Vector3(1.0f, 0.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector3(0.0f, 1.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector3(0.0f, 0.0f, 1.0f).Length, Epsilon);
			Assert.AreEqual(MathF.Sqrt(3), new Vector3(1.0f, 1.0f, 1.0f).Length, Epsilon);
		}
		[Test] public void LengthSquared()
		{
			Assert.AreEqual(1.0f, new Vector3(1.0f, 0.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector3(0.0f, 1.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector3(0.0f, 0.0f, 1.0f).LengthSquared, Epsilon);
			Assert.AreEqual(3.0f, new Vector3(1.0f, 1.0f, 1.0f).LengthSquared, Epsilon);
		}
		[Test] public void Indexing()
		{
			Assert.AreEqual(1.2f, new Vector3(1.2f, 3.4f, 5.6f)[0]);
			Assert.AreEqual(3.4f, new Vector3(1.2f, 3.4f, 5.6f)[1]);
			Assert.AreEqual(5.6f, new Vector3(1.2f, 3.4f, 5.6f)[2]);
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector3(1.2f, 3.4f, 5.6f)[-1]; });
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector3(1.2f, 3.4f, 5.6f)[3]; });

			Vector3 vector = new Vector3(0.0f, 0.0f, 0.0f);
			vector[0] = 1.2f;
			vector[1] = 3.4f;
			vector[2] = 5.6f;
			Assert.AreEqual(1.2f, vector.X);
			Assert.AreEqual(3.4f, vector.Y);
			Assert.AreEqual(5.6f, vector.Z);

			Assert.Throws<IndexOutOfRangeException>(() => { vector[-1] = 0; });
			Assert.Throws<IndexOutOfRangeException>(() => { vector[3] = 0; });
		}

		private void AssertVectorEqual(Vector3 vector, float x, float y, float z)
		{
			Assert.AreEqual(x, vector.X);
			Assert.AreEqual(y, vector.Y);
			Assert.AreEqual(z, vector.Z);
		}
		private void AssertVectorEpsilonEqual(Vector3 vector, float x, float y, float z)
		{
			Assert.AreEqual(x, vector.X, Epsilon);
			Assert.AreEqual(y, vector.Y, Epsilon);
			Assert.AreEqual(z, vector.Z, Epsilon);
		}
	}
}
