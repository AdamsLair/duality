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
		[Test] public void GetSetCasting()
		{
			ShaderParameterCollection data = new ShaderParameterCollection();
			string[] names = new string[] { "Foo", "Bar" };

			// Set some value with various names and types which can be used interchangably
			data.SetArray(names[0], new[] { 1.0f, 2.0f, 3.0f, 4.0f });
			data.SetArray(names[1], new[] { 1, 2, 3, 4 });

			// Add more data that is unrelated to the test
			data.SetValue("Unrelated", 42);
			data.SetTexture("Unrelated2", Texture.Checkerboard);

			// Assert that we can retrieve set values using any supported type
			foreach (string name in names)
			{
				// Float-based types
				Assert.AreEqual(1.0f, data.GetValue<float>(name));
				Assert.AreEqual(new Vector2(1.0f, 2.0f), data.GetValue<Vector2>(name));
				Assert.AreEqual(new Vector3(1.0f, 2.0f, 3.0f), data.GetValue<Vector3>(name));
				Assert.AreEqual(new Vector4(1.0f, 2.0f, 3.0f, 4.0f), data.GetValue<Vector4>(name));

				// Int-based types
				Assert.AreEqual(1, data.GetValue<int>(name));
				Assert.AreEqual(new Point2(1, 2), data.GetValue<Point2>(name));

				// Arrays of float-based types
				CollectionAssert.AreEqual(new [] { 1.0f, 2.0f,3.0f, 4.0f }, data.GetArray<float>(name) );
				CollectionAssert.AreEqual(new [] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) }, data.GetArray<Vector2>(name) );
				CollectionAssert.AreEqual(new [] { new Vector3(1.0f, 2.0f, 3.0f) }, data.GetArray<Vector3>(name) );

				// Arrays of int-based types
				CollectionAssert.AreEqual(new [] { 1, 2, 3, 4 }, data.GetArray<int>(name) );
				CollectionAssert.AreEqual(new [] { new Point2(1, 2), new Point2(3, 4) }, data.GetArray<Point2>(name) );

				// Booleans
				Assert.AreEqual(true, data.GetValue<bool>(name));

				// Types we don't have enough data for
				Assert.AreEqual(default(Matrix3), data.GetValue<Matrix3>(name));
				Assert.AreEqual(default(Matrix4), data.GetValue<Matrix4>(name));
				Assert.AreEqual(null, data.GetArray<Matrix3>(name));
				Assert.AreEqual(null, data.GetArray<Matrix4>(name));
			}

			// Check if the unrelated data is still there as well
			Assert.AreEqual(42, data.GetValue<int>("Unrelated"));
			Assert.AreEqual(Texture.Checkerboard, data.GetTexture("Unrelated2"));
		}
		[Test] public void GetSetArray()
		{
			ShaderParameterCollection data = new ShaderParameterCollection();
			
			AssertSetGetArray(data, new float[] { 1.0f, 2.0f, 3.0f, 4.0f });
			AssertSetGetArray(data, new Vector2[] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) });
			AssertSetGetArray(data, new Vector3[] { new Vector3(1.0f, 2.0f, 3.0f), new Vector3(4.0f, 5.0f, 6.0f) });
			AssertSetGetArray(data, new Vector4[] { new Vector4(1.0f, 2.0f, 3.0f, 4.0f), new Vector4(5.0f, 6.0f, 7.0f, 8.0f) });
			AssertSetGetArray(data, new Matrix3[] { new Matrix3(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f), Matrix3.Identity });
			AssertSetGetArray(data, new Matrix4[] { new Matrix4(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f), Matrix4.Identity });
			AssertSetGetArray(data, new int[] { 1, 2, 3, 4 });
			AssertSetGetArray(data, new Point2[] { new Point2(1, 2), new Point2(3, 4) });
			AssertSetGetArray(data, new bool[] { true, false, true, false });
		}
		[Test] public void GetSetValue()
		{
			ShaderParameterCollection data = new ShaderParameterCollection();
			
			AssertSetGetValue(data, 1.0f);
			AssertSetGetValue(data, new Vector2(1.0f, 2.0f));
			AssertSetGetValue(data, new Vector3(1.0f, 2.0f, 3.0f));
			AssertSetGetValue(data, new Vector4(1.0f, 2.0f, 3.0f, 4.0f));
			AssertSetGetValue(data, new Matrix3(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f));
			AssertSetGetValue(data, new Matrix4(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f));
			AssertSetGetValue(data, 1);
			AssertSetGetValue(data, new Point2(1, 2));
			AssertSetGetValue(data, true);
		}
		[Test] public void GetSetTexture()
		{
			ShaderParameterCollection data = new ShaderParameterCollection();
			
			// Set the specified data and expect to get it back when asking
			data.SetTexture("Foo", Texture.DualityIcon);
			Assert.AreEqual(Texture.DualityIcon, data.GetTexture("Foo"));
		}
		[Test] public void EqualityCheck()
		{
			// Create a base instance of shader parameters to compare to
			ShaderParameterCollection first = new ShaderParameterCollection();

			first.SetArray("A", new[] { 1.0f, 2.0f, 3.0f, 4.0f });
			first.SetArray("B", new[] { new Vector4(1.0f, 2.0f, 3.0f, 4.0f) });
			first.SetArray("C", new[] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) });
			first.SetArray("D", new[] { true, false });
			first.SetArray("E", new[] { 1, 2, 3, 4 });
			first.SetArray("F", new[] { new Point2(1, 2), new Point2(3, 4) });
			first.SetTexture("G", Texture.Checkerboard);
			first.SetArray("H", new[] { Matrix3.Identity });
			first.SetArray("I", new[] { Matrix4.Identity });

			// Assert that the base instance equals itself and copies of itself
			Assert.AreEqual(first, first);
			Assert.AreEqual(first, new ShaderParameterCollection(first));

			// Create a second parameter instance and assert that it becomes 
			// equal after setting all the same values
			ShaderParameterCollection second = new ShaderParameterCollection();

			Assert.AreNotEqual(first, second);

			second.SetArray("A", new[] { 1.0f, 2.0f, 3.0f, 4.0f });
			second.SetArray("B", new[] { new Vector4(1.0f, 2.0f, 3.0f, 4.0f) });
			second.SetArray("C", new[] { new Vector2(1.0f, 2.0f), new Vector2(3.0f, 4.0f) });
			second.SetArray("D", new[] { true, false });
			second.SetArray("E", new[] { 1, 2, 3, 4 });
			second.SetArray("F", new[] { new Point2(1, 2), new Point2(3, 4) });
			second.SetTexture("G", Texture.Checkerboard);
			second.SetArray("H", new[] { Matrix3.Identity });
			second.SetArray("I", new[] { Matrix4.Identity });

			Assert.AreEqual(first, second);

			// Assert that actual value change will not keep equality
			Assert.AreNotEqual(first, GetModifiedArray(second, "A", 5.0f));
			Assert.AreNotEqual(first, GetModifiedArray(second, "B", 5.0f));
			Assert.AreNotEqual(first, GetModifiedArray(second, "C", 5.0f));
			Assert.AreNotEqual(first, GetModifiedArray(second, "D", false));
			Assert.AreNotEqual(first, GetModifiedArray(second, "E", 5));
			Assert.AreNotEqual(first, GetModifiedArray(second, "F", 5));
			Assert.AreNotEqual(first, GetModifiedTexture(second, "G", Texture.DualityIcon));
			Assert.AreNotEqual(first, GetModifiedArray(second, "H", new Matrix3()));
			Assert.AreNotEqual(first, GetModifiedArray(second, "I", new Matrix4()));

			// Assert that type-only value changes will keep equality
			Assert.AreEqual(first, GetModifiedArray(second, "A", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModifiedArray(second, "B", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModifiedArray(second, "C", 1, 2, 3, 4));
			Assert.AreEqual(first, GetModifiedArray(second, "D", 1, 0));
			Assert.AreEqual(first, GetModifiedArray(second, "E", 1.0f, 2.0f, 3.0f, 4.0f));
			Assert.AreEqual(first, GetModifiedArray(second, "F", 1.0f, 2.0f, 3.0f, 4.0f));
			Assert.AreEqual(first, GetModifiedArray(second, "H", 
				1.0f, 0.0f, 0.0f, 
				0.0f, 1.0f, 0.0f, 
				0.0f, 0.0f, 1.0f));
			Assert.AreEqual(first, GetModifiedArray(second, "I", 
				1.0f, 0.0f, 0.0f, 0.0f, 
				0.0f, 1.0f, 0.0f, 0.0f,
				0.0f, 0.0f, 1.0f, 0.0f,
				0.0f, 0.0f, 0.0f, 1.0f));
		}
		[Test] public void CopyInstance()
		{
			// Create a new params instance via copy and modify its values
			ShaderParameterCollection first = new ShaderParameterCollection();
			first.SetArray("Foo", new[] { 1.0f, 2.0f, 3.0f, 4.0f });

			ShaderParameterCollection second = new ShaderParameterCollection(first);
			second.SetArray("Foo", new[] { 5.0f, 6.0f, 7.0f, 8.0f });

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

			const int ValueVariableCount = 100;
			const int TextureVariableCount = 10;
			const int VariableCount = ValueVariableCount + TextureVariableCount;

			// Generate random variable names
			StringBuilder builder = new StringBuilder();
			string[] names = new string[VariableCount];
			for (int i = 0; i < names.Length; i++)
			{
				names[i] = GetRandomString(rnd, builder);
			}

			// Prepare a uniform value for every variable
			ShaderParameterCollection data = new ShaderParameterCollection();
			float[][] values = new float[ValueVariableCount][];
			for (int i = 0; i < values.Length; i++)
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
				// Adjust one variable randomly. Choose randomly whether to change texture 
				// assignments or slightly adjust a uniform value. In any case, we'll need 
				// to do this efficiently, or the test will take way too long for fast iteration. 
				if (rnd.Next(10) == 0)
				{
					int changeIndex = rnd.Next(TextureVariableCount);
					string name = names[ValueVariableCount + changeIndex];
					ContentRef<Texture> tex = new ContentRef<Texture>(null, GetRandomString(rnd, builder));
					data.SetTexture(name, tex);
				}
				else
				{
					int changeIndex = rnd.Next(ValueVariableCount);
					string name = names[changeIndex];
					float[] value = values[changeIndex];
					value[rnd.Next(value.Length)] = 
						rnd.NextFloat(-1000.0f, 1000.0f) * 
						rnd.NextFloat(0.0f, 1.0f) * 
						rnd.NextFloat(0.0f, 1.0f);
					data.SetArray(name, value);
				}

				// Assert that we did not have a hash collision
				ulong maskedHash = hashMask & data.Hash;
				if (!observedHashes.Add(maskedHash))
				{
					Assert.Fail("Hash collision after {0} items", observedHashes.Count);
				}
			}
		}
		
		private static string GetRandomString(Random rnd, StringBuilder builder)
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
			return builder.ToString();
		}
		private static void AssertSetGetValue<T>(ShaderParameterCollection data, T value) where T : struct
		{
			// Set the specified data and expect to get it back when asking
			data.SetValue("Foo", value);
			Assert.AreEqual(value, data.GetValue<T>("Foo"));
		}
		private static void AssertSetGetArray<T>(ShaderParameterCollection data, T[] array) where T : struct
		{
			// Set the specified array data
			data.SetArray("Foo", array);

			// Assert that all data we put in can be retrieved unaltered, but 
			// we won't get the same array instance back, avoiding exposing internals.
			Assert.AreNotSame(array, data.GetArray<T>("Foo"));
			CollectionAssert.AreEqual(array, data.GetArray<T>("Foo"));
		}
		private static ShaderParameterCollection GetModifiedArray<T>(ShaderParameterCollection baseParams, string name, params T[] value) where T : struct
		{
			ShaderParameterCollection modified = new ShaderParameterCollection(baseParams);
			modified.SetArray(name, value);
			return modified;
		}
		private static ShaderParameterCollection GetModifiedTexture(ShaderParameterCollection baseParams, string name, ContentRef<Texture> tex)
		{
			ShaderParameterCollection modified = new ShaderParameterCollection(baseParams);
			modified.SetTexture(name, tex);
			return modified;
		}
	}
}
