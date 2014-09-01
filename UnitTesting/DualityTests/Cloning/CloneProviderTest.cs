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
			TestObject data = new TestObject(rnd, 5);
			TestObject dataResult = data.DeepClone();

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsFalse(data.AnyReferenceEquals(dataResult));
		}
		[Test] public void CopyToTarget()
		{
			Random rnd = new Random();
			TestObject data = new TestObject(rnd, 5);
			TestObject dataResult = new TestObject();
			data.DeepCopyTo(dataResult);

			Assert.IsTrue(data.Equals(dataResult));
			Assert.IsFalse(data.AnyReferenceEquals(dataResult));
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
				Assert.AreNotSame(data.Identity, dataResult.Identity);
				Assert.AreEqual(data.StringField, dataResultNoIdentity.StringField);
				Assert.AreSame(data.Identity, dataResultNoIdentity.Identity);
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
		}
		[Test] public void SkippedObjects()
		{
			Random rnd = new Random();
			SkipFieldTestObject data = new SkipFieldTestObject(rnd);
			SkipFieldTestObject dataResult = data.DeepClone();

			Assert.AreEqual(data.StringField, dataResult.StringField);
			Assert.AreNotEqual(data.SkipField, dataResult.SkipField);
			Assert.AreNotEqual(data.NonSerializedSkipField, dataResult.NonSerializedSkipField);
			Assert.AreEqual(data.NonSerializedField, dataResult.NonSerializedField);
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
			Random rnd = new Random();
			{
				ExplicitCloneTestObjectA data = new ExplicitCloneTestObjectA(rnd, 5);
				ExplicitCloneTestObjectA dataResult = data.DeepClone();
				
				Assert.IsTrue(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
			}
			{
				ExplicitCloneTestObjectB data = new ExplicitCloneTestObjectB(rnd, 5);
				ExplicitCloneTestObjectB dataResult = data.DeepClone();
				
				Assert.IsTrue(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
				Assert.IsFalse(data.SpecialSetupDone);
				Assert.IsTrue(dataResult.SpecialSetupDone);
			}
			{
				ExplicitCloneTestObjectC data = new ExplicitCloneTestObjectC(rnd, 5);
				ExplicitCloneTestObjectC dataResult = data.DeepClone();
				
				Assert.IsFalse(data.Equals(dataResult));
				Assert.IsFalse(data.AnyReferenceEquals(dataResult));
			}
		}
		[Test] public void PerformanceTest()
		{
			var watch = new System.Diagnostics.Stopwatch();

			Random rnd = new Random(0);
			TestObject data = new TestObject(rnd, 5);
			TestObject[] results = new TestObject[200];

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
