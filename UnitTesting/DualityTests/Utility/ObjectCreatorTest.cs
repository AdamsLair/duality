using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

using Duality;
using Duality.Serialization;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class ObjectCreatorTest
	{
		[Test] public void CreateInstanceOf()
		{
			// Primitive types
			Assert.IsTrue(CheckCreateInstance<bool>());
			Assert.IsTrue(CheckCreateInstance<byte>());
			Assert.IsTrue(CheckCreateInstance<sbyte>());
			Assert.IsTrue(CheckCreateInstance<short>());
			Assert.IsTrue(CheckCreateInstance<ushort>());
			Assert.IsTrue(CheckCreateInstance<int>());
			Assert.IsTrue(CheckCreateInstance<uint>());
			Assert.IsTrue(CheckCreateInstance<float>());
			Assert.IsTrue(CheckCreateInstance<double>());

			// Pseudo-primitive types
			Assert.IsTrue(CheckCreateInstance<string>());
			Assert.IsTrue(CheckCreateInstance<decimal>());

			// Arrays
			Assert.IsTrue(CheckCreateInstance<int[]>());
			Assert.IsTrue(CheckCreateInstance<string[]>());
			Assert.IsTrue(CheckCreateInstance<object[]>());

			// Structs
			Assert.IsTrue(CheckCreateInstance<TestCreateStructRegular>());

			// Classes
			Assert.IsTrue(CheckCreateInstance<TestCreateClassRegular>());
			Assert.IsTrue(CheckCreateInstance<TestCreateClassPrivate>());
			Assert.IsTrue(CheckCreateInstance<TestCreateClassNonEmpty>());
			Assert.IsTrue(CheckCreateInstance<TestCreateClassRequiresNonNull>());

			// Some Collections
			Assert.IsTrue(CheckCreateInstance<List<int>>());
			Assert.IsTrue(CheckCreateInstance<Dictionary<string,object>>());

			// Things that can't be created
			Assert.IsFalse(CheckCreateInstance<ITestCreateInterface>());
			Assert.IsFalse(CheckCreateInstance<TestCreateClassAbstract>());
			Assert.IsFalse(CheckCreateInstance<TestCreateClassStaticError>());
		}

		private struct TestCreateStructRegular { }
		public class TestCreateClassRegular
		{
			public TestCreateClassRegular() { }
		}
		public class TestCreateClassPrivate
		{
			private TestCreateClassPrivate() { }
		}
		public class TestCreateClassNonEmpty
		{
			public TestCreateClassNonEmpty(string value) { }
		}
		public class TestCreateClassRequiresNonNull
		{
			public TestCreateClassRequiresNonNull(string value)
			{
				if (value == null) throw new ArgumentNullException("value");
			}
		}
		public abstract class TestCreateClassAbstract { }
		public interface ITestCreateInterface { }
		public class TestCreateClassStaticError
		{
			static TestCreateClassStaticError()
			{
				throw new NotImplementedException("This Type fails to initialize.");
			}
		}

		private bool CheckCreateInstance<T>()
		{
			if (typeof(T).IsValueType)
				return object.Equals(default(T), typeof(T).CreateInstanceOf());
			else
				return typeof(T).CreateInstanceOf() != null;
		}
	}
}
