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
using Duality.Tests.Cloning.HelperObjects;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Cloning
{
	[TestFixture]
	public class CloneProviderTest
	{
		public static readonly Random SharedRandom = new Random();

		[Test] public void ClonePlainOldData()
		{
			TestData data = new TestData(true);

			TestData dataResult = data.DeepClone();

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsTrue(!object.ReferenceEquals(data, dataResult));
		}
		[Test] public void CloneComplexObject()
		{
			TestObject data = new TestObject(5);
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
			TestResource source = new TestResource();
			TestResource target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneComponent()
		{
			TestComponent source = new TestComponent();
			TestComponent target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneGameObject()
		{
			GameObject source = new GameObject("ObjectA");
			source.AddComponent<TestComponent>();
			GameObject target = source.DeepClone();
			
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.Name, target.Name);
			Assert.AreEqual(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
			Assert.AreNotSame(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
			Assert.AreNotSame(source.GetComponent<TestComponent>().TestReferenceList, target.GetComponent<TestComponent>().TestReferenceList);
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
				objA.AddComponent<TestComponent>();
				objB.AddComponent<TestComponent>();
				objC.AddComponent<TestComponent>();

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
				Assert.AreEqual(sourceObj.GetComponent<TestComponent>(), targetObj.GetComponent<TestComponent>());
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>(), targetObj.GetComponent<TestComponent>());
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>().TestReferenceList, targetObj.GetComponent<TestComponent>().TestReferenceList);

				// Check cross-object references
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>().GameObjectReference, targetObj.GetComponent<TestComponent>().GameObjectReference);
				Assert.AreEqual(sourceObj.GetComponent<TestComponent>().GameObjectReference.FullName, targetObj.GetComponent<TestComponent>().GameObjectReference.FullName);
				Assert.AreNotSame(sourceObj.GetComponent<TestComponent>().ComponentReference, targetObj.GetComponent<TestComponent>().ComponentReference);
				Assert.AreEqual(sourceObj.GetComponent<TestComponent>().ComponentReference.GameObj.FullName, targetObj.GetComponent<TestComponent>().ComponentReference.GameObj.FullName);
			}
		}
		[Test] public void IdentityPreservation()
		{
			{
				IdentityTestObjectA data = new IdentityTestObjectA();
				IdentityTestObjectA dataResult = data.DeepClone();
				IdentityTestObjectA dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
			{
				IdentityTestObjectB data = new IdentityTestObjectB();
				IdentityTestObjectB dataResult = data.DeepClone();
				IdentityTestObjectB dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotSame(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreSame(data.Identity, dataResultNoIdentity.Identity);
			}
			{
				IdentityTestObjectC data = new IdentityTestObjectC();
				IdentityTestObjectC dataResult = data.DeepClone();
				IdentityTestObjectC dataResultNoIdentity = data.DeepClone(new CloneProviderContext(false));

				Assert.AreEqual(data.StringField, dataResult.StringField);
				Assert.AreNotEqual(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreEqual(data.Identity, dataResultNoIdentity.Identity);
			}
		}
		[Test] public void SkippedObjects()
		{
			SkipFieldTestObject data = new SkipFieldTestObject();
			SkipFieldTestObject dataResult = data.DeepClone();

			Assert.AreEqual(data.StringField, dataResult.StringField);
			Assert.AreNotEqual(data.SkipField, dataResult.SkipField);
			Assert.AreNotEqual(data.SkippedObject, dataResult.SkippedObject);
		}
		[Test] public void OwnershipBehavior()
		{
			OwnershipTestObject data = new OwnershipTestObject();

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
			{
				WeakReferenceTestObject dataResultFull = data.DeepClone();
				Assert.AreSame(null, dataResultFull.Parent);
				Assert.AreEqual(2, dataResultFull.Children.Count);
				Assert.AreEqual(2, dataResultFull.Children[1].Children.Count);
				Assert.IsTrue(dataResultFull.CheckChildIntegrity());
				Assert.IsFalse(data.AnyReferenceEquals(dataResultFull));
			}

			// Clone part of the graph and see if the right parts are missing
			{
				WeakReferenceTestObject dataResultPart = dataPart.DeepClone();
				Assert.AreSame(null, dataResultPart.Parent);
				Assert.AreEqual(2, dataResultPart.Children.Count);
				Assert.IsTrue(dataResultPart.CheckChildIntegrity());
				Assert.IsFalse(dataPart.AnyReferenceEquals(dataResultPart));
			}
		}
		[Test] public void Delegates()
		{
			DelegateTestObject data = new DelegateTestObject(new[]
			{
				new DelegateTestObject(),
				new DelegateTestObject(new[]
				{
					new DelegateTestObject(),
					new DelegateTestObject()
				})
			});
			DelegateTestObject dataPart = data.Children[1] as DelegateTestObject;
			DelegateTestObject fireChild;

			// Make sure the event test setup works as expected under normal conditions
			{
				fireChild = data.GetBottomChild();

				Assert.IsFalse(data.EventReceived);
				fireChild.FireEvent();
				Assert.IsTrue(data.EventReceived);

				data.ResetAllEventsReceived();
				Assert.IsFalse(data.EventReceived);
			}

			// See if everything works as expected in a regular clone
			{
				// Does the cloning itself work as expected?
				DelegateTestObject dataResultFull = data.DeepClone();
				Assert.AreSame(null, dataResultFull.Parent);
				Assert.AreEqual(2, dataResultFull.Children.Count);
				Assert.AreEqual(2, dataResultFull.Children[1].Children.Count);
				Assert.IsTrue(dataResultFull.CheckChildIntegrity());
				Assert.IsFalse(data.AnyReferenceEquals(dataResultFull));

				// Does event handling work as expected?
				fireChild = dataResultFull.GetBottomChild();

				Assert.IsFalse(dataResultFull.EventReceived);
				Assert.IsFalse(data.EventReceived);
				fireChild.FireEvent();
				Assert.IsTrue(dataResultFull.EventReceived);
				Assert.IsFalse(data.EventReceived);

				dataResultFull.ResetAllEventsReceived();
				Assert.IsFalse(dataResultFull.EventReceived);
			}

			// See if everything works as expected in a partial clone
			{
				// Does the cloning itself work as expected?
				DelegateTestObject dataResultPart = dataPart.DeepClone();
				Assert.AreSame(null, dataResultPart.Parent);
				Assert.AreEqual(2, dataResultPart.Children.Count);
				Assert.IsTrue(dataResultPart.CheckChildIntegrity());
				Assert.IsFalse(dataPart.AnyReferenceEquals(dataResultPart));

				// Does event handling work as expected?
				fireChild = dataResultPart.GetBottomChild();

				Assert.IsFalse(dataResultPart.EventReceived);
				Assert.IsFalse(data.EventReceived);
				fireChild.FireEvent();
				Assert.IsTrue(dataResultPart.EventReceived);
				Assert.IsFalse(data.EventReceived);

				dataResultPart.ResetAllEventsReceived();
				Assert.IsFalse(dataResultPart.EventReceived);
			}
		}
		[Test] public void CircularOwnership()
		{
			OwnedObject data = new OwnedObject();
			data.TestProperty = new OwnedObject();
			data.TestProperty.TestProperty = data;

			OwnedObject dataResult = data.DeepClone();

			Assert.IsTrue(dataResult.TestProperty.TestProperty == dataResult);
		}
		[Test] public void ExplicitCloning()
		{
			{
				ExplicitCloneTestObjectA data = new ExplicitCloneTestObjectA(5);
				ExplicitCloneTestObjectA dataResult = data.DeepClone();
				
				Assert.IsTrue(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
			}
			{
				ExplicitCloneTestObjectB data = new ExplicitCloneTestObjectB(5);
				ExplicitCloneTestObjectB dataResult = data.DeepClone();
				
				Assert.IsTrue(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
				Assert.IsFalse(data.SpecialSetupDone);
				Assert.IsTrue(dataResult.SpecialSetupDone);
			}
			{
				ExplicitCloneTestObjectC data = new ExplicitCloneTestObjectC(5);
				ExplicitCloneTestObjectC dataResult = data.DeepClone();
				
				Assert.IsFalse(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
			}
		}
		[Test] public void PerformanceTest()
		{
			var watch = new System.Diagnostics.Stopwatch();

			TestObject data = new TestObject(5);
			TestObject[] results = new TestObject[100];

			watch.Start();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = data.DeepClone();
			}
			watch.Stop();
			TestHelper.LogNumericTestResult(this, "PerformanceTest", watch.Elapsed.TotalMilliseconds, "ms");

			Assert.Pass();
		}
	}
}
