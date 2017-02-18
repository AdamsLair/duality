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
	public class Vector4Test
	{
		private const float Epsilon = 0.000001f;

		[Test] public void Constuctors()
		{
			AssertVectorEqual(new Vector4(), 0.0f, 0.0f, 0.0f, 0.0f);
			AssertVectorEqual(new Vector4(1.2f), 1.2f, 1.2f, 1.2f, 1.2f);
			AssertVectorEqual(new Vector4(new Vector2(1.2f, 3.4f)), 1.2f, 3.4f, 0.0f, 0.0f);
			AssertVectorEqual(new Vector4(new Vector2(1.2f, 3.4f), 5.6f), 1.2f, 3.4f, 5.6f, 0.0f);
			AssertVectorEqual(new Vector4(new Vector2(1.2f, 3.4f), 5.6f, 7.8f), 1.2f, 3.4f, 5.6f,  7.8f);
			AssertVectorEqual(new Vector4(new Vector3(1.2f, 3.4f, 5.6f)), 1.2f, 3.4f, 5.6f, 0.0f);
			AssertVectorEqual(new Vector4(new Vector3(1.2f, 3.4f, 5.6f), 7.8f), 1.2f, 3.4f, 5.6f, 7.8f);
			AssertVectorEqual(new Vector4(1.2f, 3.4f, 5.6f, 7.8f), 1.2f, 3.4f, 5.6f, 7.8f);
		}
		[Test] public void EqualityChecks()
		{
			Vector4 a = new Vector4(1.2f, 3.4f, 5.6f, 7.8f);
			Vector4 b = new Vector4(1.2f, 3.4f, 5.6f, 7.8f);
			Vector4[] c = new[]
			{
				new Vector4(0.0f, 0.0f, 0.0f, 0.0f),
				new Vector4(1.2f, 0.0f, 0.0f, 0.0f),
				new Vector4(1.2f, 3.4f, 0.0f, 0.0f),
				new Vector4(1.2f, 0.0f, 5.6f, 0.0f),
				new Vector4(1.2f, 0.0f, 0.0f, 7.8f),
				new Vector4(1.2f, 3.4f, 0.0f, 7.8f),
				new Vector4(1.2f, 0.0f, 5.6f, 7.8f),
				new Vector4(0.0f, 3.4f, 0.0f, 0.0f),
				new Vector4(0.0f, 0.0f, 5.6f, 0.0f),
				new Vector4(0.0f, 3.4f, 5.6f, 0.0f),
				new Vector4(0.0f, 3.4f, 0.0f, 7.8f),
				new Vector4(0.0f, 0.0f, 5.6f, 7.8f),
				new Vector4(0.0f, 3.4f, 5.6f, 7.8f),
				new Vector4(-1.2f, -3.4f, -5.6f, -7.8f)
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
			AssertVectorEpsilonEqual(-new Vector4(1.2f, 3.4f, 5.6f, 7.8f), -1.2f, -3.4f, -5.6f, -7.8f);
			AssertVectorEpsilonEqual(new Vector4(1.2f, 3.4f, 5.6f, 7.8f) + new Vector4(1.1f, 2.2f, 3.3f, 4.4f), 2.3f, 5.6f, 8.9f, 12.2f);
			AssertVectorEpsilonEqual(new Vector4(1.2f, 3.4f, 5.6f, 7.8f) - new Vector4(1.1f, 2.2f, 3.3f, 4.4f), 0.1f, 1.2f, 2.3f, 3.4f);
			AssertVectorEpsilonEqual(new Vector4(1.2f, 3.4f, 5.6f, 7.8f) * new Vector4(2.0f, 3.0f, 4.0f, 5.0f), 2.4f, 10.2f, 22.4f, 39.0f);
			AssertVectorEpsilonEqual(new Vector4(1.2f, 3.4f, 5.6f, 7.8f) * 2.0f, 2.4f, 6.8f, 11.2f, 15.6f);
			AssertVectorEpsilonEqual(2.0f * new Vector4(1.2f, 3.4f, 5.6f, 7.8f), 2.4f, 6.8f, 11.2f, 15.6f);
			AssertVectorEpsilonEqual(new Vector4(2.4f, 10.2f, 22.4f, 39.0f) / new Vector4(2.0f, 3.0f, 4.0f, 5.0f), 1.2f, 3.4f, 5.6f, 7.8f);
			AssertVectorEpsilonEqual(new Vector4(2.4f, 6.8f, 11.2f, 15.6f) / 2.0f, 1.2f, 3.4f, 5.6f, 7.8f);
		}
		[Test] public void MinMax()
		{
			AssertVectorEqual(Vector4.Min(new Vector4(1.2f, 3.4f, 5.6f, 7.8f), new Vector4(7.8f, 5.6f, 3.4f, 1.2f)), 1.2f, 3.4f, 3.4f, 1.2f);
			AssertVectorEqual(Vector4.Max(new Vector4(1.2f, 3.4f, 5.6f, 7.8f), new Vector4(7.8f, 5.6f, 3.4f, 1.2f)), 7.8f, 5.6f, 5.6f, 7.8f);
		}
		[Test] public void DotProduct()
		{
			Assert.AreEqual(1.0f, Vector4.Dot(new Vector4(1.0f, 0.0f, 0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector4.Dot(new Vector4(1.0f, 0.0f, 0.0f, 0.0f), new Vector4(-1.0f, 0.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(1.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(1.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(1.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)), Epsilon);

			Assert.AreEqual(1.0f, Vector4.Dot(new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector4.Dot(new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, -1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 1.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)), Epsilon);

			Assert.AreEqual(1.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 1.0f, 0.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 1.0f, 0.0f), new Vector4(0.0f, 0.0f, -1.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 1.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 1.0f, 0.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 1.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)), Epsilon);

			Assert.AreEqual(1.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)), Epsilon);
			Assert.AreEqual(-1.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 0.0f, -1.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 0.0f)), Epsilon);
			Assert.AreEqual(0.0f, Vector4.Dot(new Vector4(0.0f, 0.0f, 0.0f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 0.0f)), Epsilon);

			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector4 vector = new Vector4(rnd.NextVector3(), rnd.NextFloat(-1.0f, 1.0f));
				Assert.AreEqual(vector.LengthSquared, Vector4.Dot(vector, vector), Epsilon);
			}
		}
		[Test] public void Normalized()
		{
			for (int i = 0; i < 10; i++)
			{
				float length = MathF.Pow(1.25f, i);
				AssertVectorEpsilonEqual(new Vector4(length, 0.0f, 0.0f, 0.0f).Normalized, 1.0f, 0.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, length, 0.0f, 0.0f).Normalized, 0.0f, 1.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, 0.0f, length, 0.0f).Normalized, 0.0f, 0.0f, 1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, 0.0f, 0.0f, length).Normalized, 0.0f, 0.0f, 0.0f, 1.0f);
				AssertVectorEpsilonEqual(new Vector4(-length, 0.0f, 0.0f, 0.0f).Normalized, -1.0f, 0.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, -length, 0.0f, 0.0f).Normalized, 0.0f, -1.0f, 0.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, 0.0f, -length, 0.0f).Normalized, 0.0f, 0.0f, -1.0f, 0.0f);
				AssertVectorEpsilonEqual(new Vector4(0.0f, 0.0f, 0.0f, -length).Normalized, 0.0f, 0.0f, 0.0f, -1.0f);
				AssertVectorEpsilonEqual(new Vector4(length, length, length, length).Normalized, 1.0f / MathF.Sqrt(4), 1.0f / MathF.Sqrt(4), 1.0f / MathF.Sqrt(4), 1.0f / MathF.Sqrt(4));
			}
		}
		[Test] public void Normalize()
		{
			Random rnd = new Random(1);
			for (int i = 0; i < 100; i++)
			{
				Vector4 vector = new Vector4(rnd.NextVector3(), rnd.NextFloat(-1.0f, 1.0f));
				Vector4 normalizedVector = vector;
				normalizedVector.Normalize();

				AssertVectorEpsilonEqual(normalizedVector, vector.Normalized.X, vector.Normalized.Y, vector.Normalized.Z, vector.Normalized.W);
			}
		}
		[Test] public void Length()
		{
			Assert.AreEqual(1.0f, new Vector4(1.0f, 0.0f, 0.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 1.0f, 0.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 0.0f, 1.0f, 0.0f).Length, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f).Length, Epsilon);
			Assert.AreEqual(MathF.Sqrt(4), new Vector4(1.0f, 1.0f, 1.0f, 1.0f).Length, Epsilon);
		}
		[Test] public void LengthSquared()
		{
			Assert.AreEqual(1.0f, new Vector4(1.0f, 0.0f, 0.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 1.0f, 0.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 0.0f, 1.0f, 0.0f).LengthSquared, Epsilon);
			Assert.AreEqual(1.0f, new Vector4(0.0f, 0.0f, 0.0f, 1.0f).LengthSquared, Epsilon);
			Assert.AreEqual(4.0f, new Vector4(1.0f, 1.0f, 1.0f, 1.0f).LengthSquared, Epsilon);
		}
		[Test] public void Indexing()
		{
			Assert.AreEqual(1.2f, new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[0]);
			Assert.AreEqual(3.4f, new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[1]);
			Assert.AreEqual(5.6f, new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[2]);
			Assert.AreEqual(7.8f, new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[3]);
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[-1]; });
			Assert.Throws<IndexOutOfRangeException>(() => { float x = new Vector4(1.2f, 3.4f, 5.6f, 7.8f)[4]; });

			Vector4 vector = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
			vector[0] = 1.2f;
			vector[1] = 3.4f;
			vector[2] = 5.6f;
			vector[3] = 7.8f;
			Assert.AreEqual(1.2f, vector.X);
			Assert.AreEqual(3.4f, vector.Y);
			Assert.AreEqual(5.6f, vector.Z);
			Assert.AreEqual(7.8f, vector.W);

			Assert.Throws<IndexOutOfRangeException>(() => { vector[-1] = 0; });
			Assert.Throws<IndexOutOfRangeException>(() => { vector[4] = 0; });
		}

		private void AssertVectorEqual(Vector4 vector, float x, float y, float z, float w)
		{
			Assert.AreEqual(x, vector.X);
			Assert.AreEqual(y, vector.Y);
			Assert.AreEqual(z, vector.Z);
			Assert.AreEqual(w, vector.W);
		}
		private void AssertVectorEpsilonEqual(Vector4 vector, float x, float y, float z, float w)
		{
			Assert.AreEqual(x, vector.X, Epsilon);
			Assert.AreEqual(y, vector.Y, Epsilon);
			Assert.AreEqual(z, vector.Z, Epsilon);
			Assert.AreEqual(w, vector.W, Epsilon);
		}
	}
}
