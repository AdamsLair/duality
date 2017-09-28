using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Tests.Properties;

using NUnit.Framework;

namespace Duality.Tests.Drawing
{
	public class ShaderParametersTest
	{
		private static IEnumerable<TestCaseData> GetSetTypeTestCases
		{
			get
			{
				// The first parameter is for NUnit to determine the test cases generic
				// type parameters, the second parameter is the actual data to be used.
				yield return new TestCaseData(
					default(float), 
					new float[] { 1.0f, 2.0f, 3.0f, 4.0f });
				yield return new TestCaseData(
					default(Vector2), 
					new Vector2[] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) });
				yield return new TestCaseData(
					default(Vector3), 
					new Vector3[] { new Vector3(1.0f, 2.0f, 3.0f), new Vector3(4.0f, 5.0f, 6.0f) });
				yield return new TestCaseData(
					default(Vector4), 
					new Vector4[] { new Vector4(1.0f, 2.0f, 3.0f, 4.0f), new Vector4(5.0f, 6.0f, 7.0f, 8.0f) });
				yield return new TestCaseData(
					default(Matrix3), 
					new Matrix3[] { new Matrix3(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f), Matrix3.Identity });
				yield return new TestCaseData(
					default(Matrix4), 
					new Matrix4[] { new Matrix4(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f), Matrix4.Identity });
				yield return new TestCaseData(
					default(int), 
					new int[] { 1, 2, 3, 4 });
				yield return new TestCaseData(
					default(Point2), 
					new Point2[] { new Point2(1, 2), new Point2(3, 4) });
				yield return new TestCaseData(
					default(bool), 
					new bool[] { true, false, true, false });
				yield return new TestCaseData(
					default(ContentRef<Texture>), 
					new ContentRef<Texture>[] { Texture.DualityIcon });
			}
		}

		[Test] public void GetSetCasting()
		{
			ShaderParameters data = new ShaderParameters();
			string[] names = new string[] { "Foo", "Bar" };

			// Set some value with various names and types which can be used interchangably
			data.SetArray(names[0], 1.0f, 2.0f, 3.0f, 4.0f);
			data.SetArray(names[1], 1, 2, 3, 4);

			// Add more data that is unrelated to the test
			data.SetArray("Unrelated", 42);
			data.SetArray("Unrelated2", Texture.Checkerboard);

			// Assert that we can retrieve set values using any supported type
			foreach (string name in names)
			{
				// Float-based types
				Assert.AreEqual(1.0f, data.Get<float>(name));
				Assert.AreEqual(new Vector2(1.0f, 2.0f), data.Get<Vector2>(name));
				Assert.AreEqual(new Vector3(1.0f, 2.0f, 3.0f), data.Get<Vector3>(name));
				Assert.AreEqual(new Vector4(1.0f, 2.0f, 3.0f, 4.0f), data.Get<Vector4>(name));

				// Int-based types
				Assert.AreEqual(1, data.Get<int>(name));
				Assert.AreEqual(new Point2(1, 2), data.Get<Point2>(name));

				// Arrays of float-based types
				CollectionAssert.AreEqual(new [] { 1.0f, 2.0f,3.0f, 4.0f }, data.GetArray<float>(name) );
				CollectionAssert.AreEqual(new [] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) }, data.GetArray<Vector2>(name) );
				CollectionAssert.AreEqual(new [] { new Vector3(1.0f, 2.0f, 3.0f) }, data.GetArray<Vector3>(name) );

				// Arrays of int-based types
				CollectionAssert.AreEqual(new [] { 1, 2, 3, 4 }, data.GetArray<int>(name) );
				CollectionAssert.AreEqual(new [] { new Point2(1, 2), new Point2(3, 4) }, data.GetArray<Point2>(name) );

				// Booleans
				Assert.AreEqual(true, data.Get<bool>(name));

				// Types we don't have enough data for
				Assert.AreEqual(default(Matrix3), data.Get<Matrix3>(name));
				Assert.AreEqual(default(Matrix4), data.Get<Matrix4>(name));
				Assert.AreEqual(null, data.GetArray<Matrix3>(name));
				Assert.AreEqual(null, data.GetArray<Matrix4>(name));
			}

			// Check if the unrelated data is still there as well
			Assert.AreEqual(42, data.Get<int>("Unrelated"));
			Assert.AreEqual(Texture.Checkerboard, data.Get<ContentRef<Texture>>("Unrelated2"));
		}
		[TestCaseSource("GetSetTypeTestCases")]
		[Test] public void GetSetArrayTypes<T>(T genericType, T[] inputArray) where T : struct
		{
			ShaderParameters data = new ShaderParameters();

			// Set the specified array data
			data.SetArray("Foo", inputArray);

			// Assert that all data we put in can be retrieved unaltered, but 
			// we won't get the same array instance back, avoiding exposing internals.
			Assert.AreNotSame(inputArray, data.GetArray<T>("Foo"));
			CollectionAssert.AreEqual(inputArray, data.GetArray<T>("Foo"));
		}
		[TestCaseSource("GetSetTypeTestCases")]
		[Test] public void GetSetTypes<T>(T genericType, T[] inputArray) where T : struct
		{
			ShaderParameters data = new ShaderParameters();

			// Set the specified data and expect to get it back when asking
			data.Set("Foo", inputArray[0]);
			Assert.AreEqual(inputArray[0], data.Get<T>("Foo"));
		}
		[Test] public void EqualityCheck()
		{
			// Create a base instance of shader parameters to compare to
			ShaderParameters first = new ShaderParameters();

			first.SetArray("A", 1.0f, 2.0f, 3.0f, 4.0f);
			first.SetArray("B", new Vector4(1.0f, 2.0f, 3.0f, 4.0f));
			first.SetArray("C", new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f));
			first.SetArray("D", true, false);
			first.SetArray("E", 1, 2, 3, 4);
			first.SetArray("F", new Point2(1, 2), new Point2(3, 4));
			first.SetArray("G", Texture.Checkerboard);
			first.SetArray("H", Matrix3.Identity);
			first.SetArray("I", Matrix4.Identity);

			// Assert that the base instance equals itself and copies of itself
			Assert.AreEqual(first, first);
			Assert.AreEqual(first, new ShaderParameters(first));

			// Create a second parameter instance and assert that it becomes 
			// equal after setting all the same values
			ShaderParameters second = new ShaderParameters();

			Assert.AreNotEqual(first, second);

			second.SetArray("A", 1.0f, 2.0f, 3.0f, 4.0f);
			second.SetArray("B", new Vector4(1.0f, 2.0f, 3.0f, 4.0f));
			second.SetArray("C", new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f));
			second.SetArray("D", true, false);
			second.SetArray("E", 1, 2, 3, 4);
			second.SetArray("F", new Point2(1, 2), new Point2(3, 4));
			second.SetArray("G", Texture.Checkerboard);
			second.SetArray("H", Matrix3.Identity);
			second.SetArray("I", Matrix4.Identity);

			Assert.AreEqual(first, second);

			// Assert that actual value change will not keep equality
			Assert.AreNotEqual(first, GetModified(second, "A", 5.0f));
			Assert.AreNotEqual(first, GetModified(second, "B", 5.0f));
			Assert.AreNotEqual(first, GetModified(second, "C", 5.0f));
			Assert.AreNotEqual(first, GetModified(second, "D", false));
			Assert.AreNotEqual(first, GetModified(second, "E", 5));
			Assert.AreNotEqual(first, GetModified(second, "F", 5));
			Assert.AreNotEqual(first, GetModified(second, "G", Texture.DualityIcon));
			Assert.AreNotEqual(first, GetModified(second, "H", new Matrix3()));
			Assert.AreNotEqual(first, GetModified(second, "I", new Matrix4()));

			// Assert that type-only value changes will keep equality
			Assert.AreEqual(first, GetModified(second, "A", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModified(second, "B", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModified(second, "C", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModified(second, "D", 1, 0));
			Assert.AreEqual(first, GetModified(second, "E", 1.0f, 2.0f, 3.0f, 4.0f));
			Assert.AreEqual(first, GetModified(second, "F", 1.0f, 2.0f, 3.0f, 4.0f));
			Assert.AreEqual(first, GetModified(second, "H", 
				1.0f, 0.0f, 0.0f, 
				0.0f, 1.0f, 0.0f, 
				0.0f, 0.0f, 1.0f));
			Assert.AreEqual(first, GetModified(second, "I", 
				1.0f, 0.0f, 0.0f, 0.0f, 
				0.0f, 1.0f, 0.0f, 0.0f,
				0.0f, 0.0f, 1.0f, 0.0f,
				0.0f, 0.0f, 0.0f, 1.0f));
		}
		[Test] public void CopyInstance()
		{
			// Create a new params instance via copy and modify its values
			ShaderParameters first = new ShaderParameters();
			first.SetArray("Foo", 1.0f, 2.0f, 3.0f, 4.0f);

			ShaderParameters second = new ShaderParameters(first);
			second.SetArray("Foo", 5.0f, 6.0f, 7.0f, 8.0f);

			// Assert that each instance has its own values and no accidental shallow copy was made
			CollectionAssert.AreEqual(new[] { 1.0f, 2.0f, 3.0f, 4.0f }, first.GetArray<float>("Foo"));
			CollectionAssert.AreEqual(new[] { 5.0f, 6.0f, 7.0f, 8.0f }, second.GetArray<float>("Foo"));
		}

		[TestCase(1000)]
		[TestCase(10000)]
		[TestCase(100000)]
		[TestCase(1000000)]
		[Test] public void HashCollisions(int instanceCount)
		{
			// Keep randomness deterministic so we can reproduce test failures
			Random rnd = new Random(1);

			// Generate random variable names
			StringBuilder builder = new StringBuilder();
			string[] names = new string[100];
			for (int i = 0; i < names.Length; i++)
			{
				builder.Clear();
				for (int k = rnd.Next(3, 20); k >= 0; k--)
				{
					// Digits
					if (rnd.Next(10) == 0)
						builder.Append((char)rnd.Next(48, 58));
					// Letters (caps)
					else if (rnd.Next(5) == 0)
						builder.Append((char)rnd.Next(65, 91));
					// Letters (small)
					else
						builder.Append((char)rnd.Next(97, 123));
				}
				names[i] = builder.ToString();
			}

			// Prepare a uniform value for every variable
			ShaderParameters data = new ShaderParameters();
			float[][] values = new float[names.Length][];
			for (int i = 0; i < names.Length; i++)
			{
				values[i] = new float[rnd.Next(1, 17)];
				for (int k = 0; k < values[i].Length; k++)
				{
					values[i][k] = 
						rnd.NextFloat(-1000.0f, 1000.0f) * 
						rnd.NextFloat(0.0f, 1.0f) * 
						rnd.NextFloat(0.0f, 1.0f);
				}
				data.SetArray(names[i], values[i]);
			}

			// We'll make the test a little harder by masking each hash value to 
			// effectively steal a few bits. If we can do well at less than 64 bits,
			// we can be even more confident that our 64 bit hash is "collision free".
			// The value below steals 20 bits all over the available range.
			ulong hashMask = 0xE3F5DAFCB3F57AFC;

			// For every instance, randomly adjust one variable and check for collisions
			HashSet<ulong> observedHashes = new HashSet<ulong>();
			for (int i = 0; i < instanceCount; i++)
			{
				// Adjust one variable randomly. We'll need to do this efficiently, or
				// the test will take way too long for fast iteration.
				int changeIndex = rnd.Next(names.Length);
				string name = names[changeIndex];
				float[] value = values[changeIndex];
				value[rnd.Next(value.Length)] = 
					rnd.NextFloat(-1000.0f, 1000.0f) * 
					rnd.NextFloat(0.0f, 1.0f) * 
					rnd.NextFloat(0.0f, 1.0f);
				data.SetArray(name, value);

				// Assert that we did not have a hash collision
				ulong maskedHash = hashMask & data.Hash;
				if (!observedHashes.Add(maskedHash))
				{
					Assert.Fail("Hash collision after {0} items", observedHashes.Count);
				}
			}
		}

		private static ShaderParameters GetModified<T>(ShaderParameters baseParams, string name, params T[] value) where T : struct
		{
			ShaderParameters modified = new ShaderParameters(baseParams);
			modified.SetArray(name, value);
			return modified;
		}
	}
}
