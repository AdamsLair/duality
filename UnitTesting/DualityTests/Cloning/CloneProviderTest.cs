using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Cloning
{
	[TestFixture]
	public class CloneProviderTest
	{
		private struct TestData : IEquatable<TestData>
		{
			public int IntField;
			public float FloatField;

			public TestData(Random rnd)
			{
				this.IntField	= rnd.Next();
				this.FloatField	= rnd.NextFloat();
			}

			public static bool operator ==(TestData first, TestData second)
			{
				return first.Equals(second);
			}
			public static bool operator !=(TestData first, TestData second)
			{
				return !first.Equals(second);
			}
			public override int GetHashCode()
			{
				return MathF.CombineHashCode(
					this.IntField.GetHashCode(),
					this.FloatField.GetHashCode());
			}
			public override bool Equals(object obj)
			{
				if (obj is TestData)
					return this.Equals((TestData)obj);
				else
					return base.Equals(obj);
			}
			public bool Equals(TestData other)
			{
				return 
					other.IntField == this.IntField &&
					other.FloatField == this.FloatField;
			}
		}
		private class TestObject : IEquatable<TestObject>
		{
			public string StringField;
			public TestData DataField;
			public List<int> ListField;
			public List<string> ListField2;
			public Dictionary<string,TestObject> DictField;
			
			public TestObject(Random rnd, int maxChildren = 5)
			{
				this.StringField	= rnd.Next().ToString();
				this.DataField		= new TestData(rnd);
				this.ListField		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).ToList();
				this.ListField2		= Enumerable.Range(rnd.Next(-1000, 1000), rnd.Next(0, 50)).Select(i => i.ToString()).ToList();
				this.DictField		= new Dictionary<string,TestObject>();

				for (int i = rnd.Next(0, maxChildren); i > 0; i--)
				{
					this.DictField.Add(rnd.Next().ToString(), new TestObject(rnd, maxChildren / 2));
				}
			}

			public static bool operator ==(TestObject first, TestObject second)
			{
				return first.Equals(second);
			}
			public static bool operator !=(TestObject first, TestObject second)
			{
				return !first.Equals(second);
			}
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}
			public override bool Equals(object obj)
			{
				if (obj is TestObject)
					return this.Equals((TestObject)obj);
				else
					return base.Equals(obj);
			}
			public bool Equals(TestObject other)
			{
				return 
					other.StringField == this.StringField &&
					other.DataField == this.DataField &&
					other.ListField.SequenceEqual(this.ListField) &&
					other.ListField2.SequenceEqual(this.ListField2) &&
					other.DictField.SetEqual(this.DictField);
			}
		}
		private class TestResource : Resource
		{
			public string TestProperty { get; set; }
			public ReferencedObject TestReference { get; set; }
			public List<ReferencedObject> TestReferenceList { get; set; }
		}
		private class TestComponent : Component
		{
			public string TestProperty { get; set; }
			public ReferencedObject TestReference { get; set; }
			public List<ReferencedObject> TestReferenceList { get; set; }
		}
		private class ReferencedObject
		{
			public string TestProperty { get; set; }
		}
		

		[Test] public void ClonePlainOldData()
		{
			Random rnd = new Random();
			TestData data = new TestData(rnd);

			TestData dataResult = data.DeepClone();

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsTrue(!object.ReferenceEquals(data, dataResult));
		}
		[Test] public void CloneComplexObject()
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);

			TestObject dataResult = data.DeepClone();

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsTrue(!object.ReferenceEquals(data, dataResult));
		}
		[Test] public void CloneComplexObjectExplicitUnwrap()
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd);

			CloneProvider provider = new CloneProvider();
			provider.ExplicitUnwrap.Add(typeof(System.Collections.ICollection));

			// Need to trick CloneProvider to deep-clone TestObject, because we just explicitly told it not to unwrap it.
			TestObject dataResult = new TestObject(rnd, 0);
			provider.CopyObjectTo(
				data, 
				dataResult,
				data.GetType().GetAllFields(ReflectionHelper.BindInstanceAll));

			// Now check whether the lower layers are shallow-copies, except for collections.
			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsFalse(object.ReferenceEquals(data, dataResult));
			Assert.IsFalse(object.ReferenceEquals(data.ListField, dataResult.ListField));
			Assert.IsFalse(object.ReferenceEquals(data.DictField, dataResult.DictField));
			foreach (var pair in data.DictField)
			{
				Assert.IsTrue(pair.Value.Equals(dataResult.DictField[pair.Key]));
				Assert.IsTrue(object.ReferenceEquals(pair.Value, dataResult.DictField[pair.Key]));
			}
		}
		[Test] public void CloneContentRef()
		{
			TestResource resource = new TestResource { TestProperty = "TestString" };
			ContentRef<TestResource> reference = new ContentRef<TestResource>(resource, "SomeTestPath");

			// Expect the Resource to be cloned
			TestResource resourceClone = resource.DeepClone();
			Assert.AreNotSame(resource, resourceClone);
			Assert.AreEqual(resource.TestProperty, resourceClone.TestProperty);

			// Expect only the reference to be cloned
			ContentRef<TestResource> referenceClone = reference.DeepClone();
			Assert.AreEqual(reference.Path, referenceClone.Path);
			Assert.AreEqual(reference.ResWeak, referenceClone.ResWeak);
		}
		[Test] public void CloneResource()
		{
			TestResource resource = CreateTestResource();
			TestResource resourceClone = resource.DeepClone();
			TestResourceClone(resource, resourceClone);
		}
		[Test] public void CloneComponent()
		{
			TestComponent source = CreateTestComponent();
			TestComponent target = source.DeepClone();
			TestComponentClone(source, target);
		}
		[Test] public void CloneGameObject()
		{
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(CreateTestComponent());
			GameObject target = source.DeepClone();
			
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.Name, target.Name);
			TestComponentClone(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
		}
		[Test] public void CloneScene()
		{
			Scene source = new Scene();
			{
				GameObject objA = new GameObject("ObjectA");
				GameObject objB = new GameObject("ObjectB");
				GameObject objC = new GameObject("ObjectC", objA);
				objA.AddComponent(CreateTestComponent());
				objB.AddComponent(CreateTestComponent());
				objC.AddComponent(CreateTestComponent());
				source.AddObject(objA);
				source.AddObject(objB);
				source.AddObject(objC);
			}
			Scene target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.AllObjects.Count(), target.AllObjects.Count());
			foreach (GameObject sourceObj in source.AllObjects)
			{
				GameObject targetObj = target.FindGameObject(sourceObj.FullName);

				Assert.AreNotSame(sourceObj, targetObj);
				Assert.AreEqual(sourceObj.Name, targetObj.Name);
				TestComponentClone(sourceObj.GetComponent<TestComponent>(), targetObj.GetComponent<TestComponent>());
			}
		}

		private static TestResource CreateTestResource()
		{
			Random rnd = new Random();
			return new TestResource
			{
				TestProperty = "TestStringA" + rnd.NextByte(), 
				TestReference = new ReferencedObject { TestProperty = "TestStringB" + rnd.NextByte() },
				TestReferenceList = new List<ReferencedObject>
				{
					new ReferencedObject { TestProperty = "TestStringC" + rnd.NextByte() },
					new ReferencedObject { TestProperty = "TestStringD" + rnd.NextByte() }
				}
			};
		}
		private static TestComponent CreateTestComponent()
		{
			Random rnd = new Random();
			return new TestComponent
			{
				TestProperty = "TestStringA" + rnd.NextByte(), 
				TestReference = new ReferencedObject { TestProperty = "TestStringB" + rnd.NextByte() },
				TestReferenceList = new List<ReferencedObject>
				{
					new ReferencedObject { TestProperty = "TestStringC" + rnd.NextByte() },
					new ReferencedObject { TestProperty = "TestStringD" + rnd.NextByte() }
				}
			};
		}
		private void TestResourceClone(TestResource source, TestResource target)
		{
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.TestProperty, target.TestProperty);
			Assert.AreSame(source.TestReference, target.TestReference);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
			CollectionAssert.AreEqual(source.TestReferenceList, target.TestReferenceList);
		}
		private void TestComponentClone(TestComponent source, TestComponent target)
		{
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.TestProperty, target.TestProperty);
			Assert.AreSame(source.TestReference, target.TestReference);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
			CollectionAssert.AreEqual(source.TestReferenceList, target.TestReferenceList);
		}
	}
}
