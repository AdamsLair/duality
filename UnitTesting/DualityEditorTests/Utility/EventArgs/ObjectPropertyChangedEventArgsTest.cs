using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

using NUnit.Framework;

using Duality.Editor;

namespace Duality.Editor.Tests
{
	[TestFixture]
	public class ObjectPropertyChangedEventArgsTest
	{
		private class UnrelatedObject
		{
			public int Not { get; set; }

			public static PropertyInfo NotProperty = typeof(UnrelatedObject).GetTypeInfo().DeclaredProperties.First(p => p.Name == "Not");
		}
		private class TestObject
		{
			public int Foo { get; set; }
			public object Bar { get; set; }

			public static PropertyInfo FooProperty = typeof(TestObject).GetTypeInfo().DeclaredProperties.First(p => p.Name == "Foo");
			public static PropertyInfo BarProperty = typeof(TestObject).GetTypeInfo().DeclaredProperties.First(p => p.Name == "Bar");
		}

		[Test] public void RegularChange()
		{
			TestObject[] obj = new TestObject[]
			{
				new TestObject { Foo = 42, Bar = "Hello" },
				new TestObject { Foo = 10, Bar = "World" },
				new TestObject { Foo = 0, Bar = "Not" }
			};
			ObjectPropertyChangedEventArgs args = new ObjectPropertyChangedEventArgs(
				new ObjectSelection(obj.Take(2)),
				new PropertyInfo[] { TestObject.FooProperty });

			Assert.IsTrue(args.HasObject(obj[0]));
			Assert.IsTrue(args.HasObject(obj[1]));
			Assert.IsFalse(args.HasObject(obj[2]));

			Assert.IsTrue(args.HasAnyObject(obj));
			Assert.IsTrue(args.HasAnyObject(obj.Skip(1)));
			Assert.IsFalse(args.HasAnyObject(obj.Skip(2)));

			Assert.IsTrue(args.HasProperty(TestObject.FooProperty));
			Assert.IsFalse(args.HasProperty(TestObject.BarProperty));
			Assert.IsFalse(args.HasProperty(UnrelatedObject.NotProperty));

			Assert.IsTrue(args.HasAnyProperty(TestObject.FooProperty));
			Assert.IsTrue(args.HasAnyProperty(TestObject.FooProperty, TestObject.BarProperty));
			Assert.IsFalse(args.HasAnyProperty(TestObject.BarProperty));
			Assert.IsFalse(args.HasAnyProperty(UnrelatedObject.NotProperty));

			Assert.IsFalse(args.CompleteChange);
		}
		[Test] public void CompleteChange()
		{
			TestObject[] obj = new TestObject[]
			{
				new TestObject { Foo = 42, Bar = "Hello" },
				new TestObject { Foo = 10, Bar = "World" },
				new TestObject { Foo = 0, Bar = "Not" }
			};
			ObjectPropertyChangedEventArgs args = new ObjectPropertyChangedEventArgs(
				new ObjectSelection(obj.Take(2)));

			Assert.IsTrue(args.HasObject(obj[0]));
			Assert.IsTrue(args.HasObject(obj[1]));
			Assert.IsFalse(args.HasObject(obj[2]));

			Assert.IsTrue(args.HasAnyObject(obj));
			Assert.IsTrue(args.HasAnyObject(obj.Skip(1)));
			Assert.IsFalse(args.HasAnyObject(obj.Skip(2)));

			Assert.IsTrue(args.HasProperty(TestObject.FooProperty));
			Assert.IsTrue(args.HasProperty(TestObject.BarProperty));
			Assert.IsFalse(args.HasProperty(UnrelatedObject.NotProperty));

			Assert.IsTrue(args.HasAnyProperty(TestObject.FooProperty));
			Assert.IsTrue(args.HasAnyProperty(TestObject.FooProperty, TestObject.BarProperty));
			Assert.IsTrue(args.HasAnyProperty(TestObject.BarProperty));
			Assert.IsFalse(args.HasAnyProperty(UnrelatedObject.NotProperty));

			Assert.IsTrue(args.CompleteChange);
		}
	}
}
