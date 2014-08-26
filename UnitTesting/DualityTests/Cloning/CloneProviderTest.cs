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
		#pragma warning disable 659  // GetHashCode not implemented

		public CloneProviderTest()
		{
			System.Diagnostics.Debugger.Launch();
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
			Assert.IsFalse(data.AnyReferenceEquals(dataResult));
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
			TestResource resource = CreateTestSource<TestResource>();
			TestResource resourceClone = resource.DeepClone();
			TestClone(resource, resourceClone);
		}
		[Test] public void CloneComponent()
		{
			TestComponent source = CreateTestSource<TestComponent>();
			TestComponent target = source.DeepClone();
			TestClone(source, target);
		}
		[Test] public void CloneGameObject()
		{
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(CreateTestSource<TestComponent>());
			GameObject target = source.DeepClone();
			
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.Name, target.Name);
			TestClone(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
		}
		[Test] public void CloneScene()
		{
			Scene source = new Scene();
			{
				// Create a basic object hierarchy
				GameObject objA = new GameObject("ObjectA");
				GameObject objB = new GameObject("ObjectB");
				GameObject objC = new GameObject("ObjectC", objA);

				// Create some Components containing data
				objA.AddComponent(CreateTestSource<TestComponent>());
				objB.AddComponent(CreateTestSource<TestComponent>());
				objC.AddComponent(CreateTestSource<TestComponent>());

				// Introduce some cross-object references
				objA.GetComponent<TestComponent>().GameObjectReference = objC;
				objA.GetComponent<TestComponent>().ComponentReference = objB.GetComponent<TestComponent>();
				objB.GetComponent<TestComponent>().GameObjectReference = objA;
				objB.GetComponent<TestComponent>().ComponentReference = objC.GetComponent<TestComponent>();
				objC.GetComponent<TestComponent>().GameObjectReference = objB;
				objC.GetComponent<TestComponent>().ComponentReference = objA.GetComponent<TestComponent>();

				// Add it all to the Scene
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
				Assert.AreEqual(sourceObj.FullName, targetObj.FullName);

				// See if the data Components are cloned and intact
				TestClone(sourceObj.GetComponent<TestComponent>(), targetObj.GetComponent<TestComponent>());

				// Check cross-object references
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>().GameObjectReference, targetObj.GetComponent<TestComponent>().GameObjectReference);
				Assert.AreEqual(sourceObj.GetComponent<TestComponent>().GameObjectReference.FullName, targetObj.GetComponent<TestComponent>().GameObjectReference.FullName);
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>().ComponentReference, targetObj.GetComponent<TestComponent>().ComponentReference);
				Assert.AreEqual(sourceObj.GetComponent<TestComponent>().ComponentReference.GameObj.FullName, targetObj.GetComponent<TestComponent>().ComponentReference.GameObj.FullName);
			}
		}
		[Test] public void IdentityPreservation()
		{
			Random rnd = new Random();
			{
				IdentityTestObjectA data = new IdentityTestObjectA(rnd);
				IdentityTestObjectA dataResult = data.DeepClone();
				IdentityTestObjectA dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
			{
				IdentityTestObjectB data = new IdentityTestObjectB(rnd);
				IdentityTestObjectB dataResult = data.DeepClone();
				IdentityTestObjectB dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
			{
				IdentityTestObjectC data = new IdentityTestObjectC(rnd);
				IdentityTestObjectC dataResult = data.DeepClone();
				IdentityTestObjectC dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
			{
				IdentityTestObjectD data = new IdentityTestObjectD(rnd);
				IdentityTestObjectD dataResult = data.DeepClone();
				IdentityTestObjectD dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
		}
		[Test] public void SkippedObjects()
		{
			Random rnd = new Random();

			SkipFieldTestObject data = new SkipFieldTestObject(rnd);
			SkipFieldTestObject dataResult = data.DeepClone();

			Assert.AreEqual(data.StringField, dataResult.StringField);
			Assert.AreNotEqual(data.SkipField, dataResult.SkipField);
			Assert.AreNotEqual(data.SkippedObject, dataResult.SkippedObject);
		}
		[Test] public void OwnershipBehavior()
		{
			Random rnd = new Random();
			OwnershipTestObject data = new OwnershipTestObject(rnd);

			OwnershipTestObject dataResult = data.DeepClone();

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsFalse(data.AnyReferenceEquals(dataResult));
		}
		[Test] public void WeakReferenceBehavior()
		{
			WeakReferenceTestObject data = new WeakReferenceTestObject(new[]
			{
				new WeakReferenceTestObject(),
				new WeakReferenceTestObject(new[]
				{
					new WeakReferenceTestObject(),
					new WeakReferenceTestObject()
				})
			});
			WeakReferenceTestObject dataPart = data.Children[1];

			// Clone the full graph and see if its complete
			WeakReferenceTestObject dataResultFull = data.DeepClone();
			Assert.AreSame(null, dataResultFull.Parent);
			Assert.AreEqual(2, dataResultFull.Children.Count);
			Assert.AreEqual(2, dataResultFull.Children[1].Children.Count);
			Assert.IsTrue(dataResultFull.CheckChildIntegrity());
			Assert.IsFalse(data.AnyReferenceEquals(dataResultFull));

			// Clone part of the graph and see if the right parts are missing
			WeakReferenceTestObject dataResultPart = dataPart.DeepClone();
			Assert.AreSame(null, dataResultPart.Parent);
			Assert.AreEqual(2, dataResultPart.Children.Count);
			Assert.IsTrue(dataResultFull.CheckChildIntegrity());
			Assert.IsFalse(dataPart.AnyReferenceEquals(dataResultPart));
		}
		[Test] public void CircularOwnership()
		{
			OwnedObject data = new OwnedObject();
			data.TestProperty = new OwnedObject();
			data.TestProperty.TestProperty = data;

			OwnedObject dataResult = data.DeepClone();

			Assert.IsTrue(dataResult.TestProperty.TestProperty == dataResult);
		}

		private static T CreateTestSource<T>() where T : ICloneTestObject, new()
		{
			T source = new T();
			InitSourceObject(source);
			return source;
		}
		private static void InitSourceObject(ICloneTestObject source)
		{
			Random rnd = new Random();
			source.TestProperty = "TestStringA" + rnd.NextByte();
			source.TestReference = new ReferencedObject { TestProperty = "TestStringB" + rnd.NextByte() };
			source.TestReferenceList = new List<ReferencedObject>
			{
				new ReferencedObject { TestProperty = "TestStringC" + rnd.NextByte() },
				new ReferencedObject { TestProperty = "TestStringD" + rnd.NextByte() }
			};
		}
		private static void TestClone(ICloneTestObject source, ICloneTestObject target)
		{
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.TestProperty, target.TestProperty);
			Assert.AreSame(source.TestReference, target.TestReference);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
			CollectionAssert.AreEqual(source.TestReferenceList, target.TestReferenceList);
		}

		
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
			public bool AnyReferenceEquals(TestObject other)
			{
				if (object.ReferenceEquals(this, other)) return true;
				if (object.ReferenceEquals(this.ListField, other.ListField) && !object.ReferenceEquals(this.ListField, null)) return true;
				if (object.ReferenceEquals(this.ListField2, other.ListField2) && !object.ReferenceEquals(this.ListField2, null)) return true;
				if (object.ReferenceEquals(this.DictField, other.DictField) && !object.ReferenceEquals(this.DictField, null)) return true;
				foreach (var key in this.DictField.Keys)
				{
					var a = this.DictField[key];
					var b = other.DictField[key];
					if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
				}
				return false;
			}
		}
		private class SkipFieldTestObject
		{
			public string StringField;
			[CloneBehavior(CloneFlags.Skip)]
			public int SkipField;
			public AlwaysSkippedObject SkippedObject;
			
			public SkipFieldTestObject(Random rnd)
			{
				this.StringField = rnd.Next().ToString();
				this.SkipField = rnd.Next();
				this.SkippedObject = new AlwaysSkippedObject();
			}
		}
		[CloneBehavior(CloneFlags.Skip)]
		private class AlwaysSkippedObject {}
		private class IdentityTestObjectA
		{
			public string StringField;
			[CloneBehavior(CloneFlags.IdentityRelevant)]
			public Guid Identity;
			
			public IdentityTestObjectA(Random rnd)
			{
				this.StringField = rnd.Next().ToString();
				this.Identity = Guid.NewGuid();
			}
		}
		private class IdentityTestObjectB
		{
			public string StringField;
			[CloneBehavior(CloneFlags.IdentityRelevant)]
			public ReferencedObject Identity;
			
			public IdentityTestObjectB(Random rnd)
			{
				this.StringField = rnd.Next().ToString();
				this.Identity = new ReferencedObject();
			}
		}
		private class IdentityTestObjectC
		{
			public string StringField;
			[CloneBehavior(CloneFlags.IdentityRelevant)]
			public int Identity;
			
			public IdentityTestObjectC(Random rnd)
			{
				this.StringField = rnd.Next().ToString();
				this.Identity = rnd.Next();
			}
		}
		private class IdentityTestObjectD
		{
			public string StringField;
			public IdentityRelevantObject Identity;
			
			public IdentityTestObjectD(Random rnd)
			{
				this.StringField = rnd.Next().ToString();
				this.Identity = new IdentityRelevantObject();
			}
		}
		[CloneBehavior(CloneFlags.IdentityRelevant)]
		private class IdentityRelevantObject {}
		private class OwnershipTestObject : IEquatable<OwnershipTestObject>
		{
			[CloneBehavior(CloneMode.ChildObject)]
			public ReferencedObject NestedObject;
			[CloneBehavior(typeof(ReferencedObject), CloneMode.ChildObject)]
			public Dictionary<string,ReferencedObject> ObjectStore;
			
			public OwnershipTestObject(Random rnd)
			{
				this.NestedObject = new ReferencedObject { TestProperty = rnd.Next().ToString() };
				this.ObjectStore = new Dictionary<string,ReferencedObject>();

				for (int i = rnd.Next(0, 3); i > 0; i--)
				{
					string name = rnd.Next().ToString();
					this.ObjectStore.Add(name, new ReferencedObject { TestProperty = name });
				}
			}

			public override bool Equals(object obj)
			{
				if (obj is OwnershipTestObject)
					return this.Equals((OwnershipTestObject)obj);
				else
					return base.Equals(obj);
			}
			public bool Equals(OwnershipTestObject other)
			{
				if (other.NestedObject != null && this.NestedObject != null)
				{
					if (other.NestedObject.TestProperty != this.NestedObject.TestProperty) return false;
				}
				if (other.ObjectStore != null && this.ObjectStore != null)
				{
					if (other.ObjectStore.Count != this.ObjectStore.Count) return false;
					foreach (var pair in other.ObjectStore)
					{
						if (!this.ObjectStore.ContainsKey(pair.Key)) return false;
						var otherObj = this.ObjectStore[pair.Key];
						if (otherObj != null && pair.Value != null)
						{
							if (otherObj.TestProperty != pair.Value.TestProperty) return false;
						}
					}
				}
				return true;
			}
			public bool AnyReferenceEquals(OwnershipTestObject other)
			{
				if (object.ReferenceEquals(this, other)) return true;
				if (object.ReferenceEquals(this.NestedObject, other.NestedObject) && !object.ReferenceEquals(this.NestedObject, null)) return true;
				if (object.ReferenceEquals(this.ObjectStore, other.ObjectStore) && !object.ReferenceEquals(this.ObjectStore, null)) return true;
				foreach (var key in this.ObjectStore.Keys)
				{
					var a = this.ObjectStore[key];
					var b = other.ObjectStore[key];
					if (object.ReferenceEquals(a, b) && !object.ReferenceEquals(a, null)) return true;
				}
				return false;
			}
		}
		private class WeakReferenceTestObject
		{
			[CloneBehavior(CloneMode.WeakReference)]
			public WeakReferenceTestObject Parent;
			[CloneBehavior(typeof(WeakReferenceTestObject), CloneMode.ChildObject)]
			public List<WeakReferenceTestObject> Children;

			public WeakReferenceTestObject() : this(new WeakReferenceTestObject[0]) {}
			public WeakReferenceTestObject(IEnumerable<WeakReferenceTestObject> children)
			{
				this.Children = children.ToList();
				foreach (var child in this.Children)
				{
					child.Parent = this;
				}
			}

			public bool CheckChildIntegrity()
			{
				foreach (var child in this.Children)
				{
					if (child.Parent != this) return false;
					if (!child.CheckChildIntegrity()) return false;
				}
				return true;
			}
			public bool AnyReferenceEquals(WeakReferenceTestObject other)
			{
				if (object.ReferenceEquals(this, other)) return true;
				if (object.ReferenceEquals(this.Parent, other.Parent) && !object.ReferenceEquals(this.Parent, null)) return true;
				if (object.ReferenceEquals(this.Children, other.Children) && !object.ReferenceEquals(this.Children, null)) return true;
				if (this.Children.Zip(other.Children, (a, b) => a.AnyReferenceEquals(b)).Contains(true)) return true;
				return false;
			}
		}
		private interface ICloneTestObject
		{
			string TestProperty { get; set; }
			ReferencedObject TestReference { get; set; }
			List<ReferencedObject> TestReferenceList { get; set; }
		}
		private class TestResource : Resource, ICloneTestObject
		{
			public string TestProperty { get; set; }
			public ReferencedObject TestReference { get; set; }
			public List<ReferencedObject> TestReferenceList { get; set; }
		}
		private class TestComponent : Component, ICloneTestObject
		{
			public string TestProperty { get; set; }
			public ReferencedObject TestReference { get; set; }
			public List<ReferencedObject> TestReferenceList { get; set; }
			public GameObject GameObjectReference { get; set; }
			public Component ComponentReference { get; set; }
		}
		[CloneBehavior(CloneMode.Reference)]
		private class ReferencedObject
		{
			public string TestProperty { get; set; }
		}
		private class OwnedObject
		{
			public OwnedObject TestProperty { get; set; }
		}
	}
}
