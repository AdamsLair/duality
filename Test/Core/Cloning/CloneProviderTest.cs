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
		[Test] public void CloneRecursiveGraphs()
		{
			// Make sure that object referencing themselves can be cloned without problems
			OwnedObject source = new OwnedObject();
			source.TestData = 42;
			source.TestProperty = source;
			OwnedObject target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.TestData, target.TestData);
			Assert.AreSame(target, target.TestProperty);
		}
		[Test] public void SelfReferentialObject()
		{
			// Objects that assign a self-reference to one of their fields during construction
			// have been known to cause problems to the cloning algorithm. Test if that still works.
			SelfReferenceObject source = new SelfReferenceObject();
			SelfReferenceObject target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreSame(source, source.SelfRef);
			Assert.AreSame(target, target.SelfRef);
		}
		[Test] public void CloneMemberInfo()
		{
			Random rnd = new Random();
			TestMemberInfoData data = new TestMemberInfoData(rnd);
			TestMemberInfoData dataResult = data.DeepClone();

			Assert.AreNotSame(data, dataResult);
			Assert.AreEqual(data, dataResult);
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
		[Test] public void CopyToTargetPreservation()
		{
			// Create two identical test objects, then change one field
			TestObject source = new TestObject(new Random(0), 5);
			TestObject target = new TestObject(new Random(0), 5);
			TestObject targetFirstDictChild = target.DictField.First().Value;
			source.StringField = "NewValue";

			// Copy data from source to target
			source.DeepCopyTo(target);

			// Make sure the data is equal and old object instances have been preserved
			Assert.IsTrue(source.Equals(target));
			Assert.IsFalse(source.AnyReferenceEquals(target));
			Assert.AreSame(targetFirstDictChild, target.DictField.First().Value);

			// Note that we can't expect hashset child objects to be preserved, as
			// there is no defined item identity in a hashset. It cannot be indexed
			// by any value, so there is no definition of an "equivalent" item in
			// source and target.
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
		[Test] public void InterfaceFieldsOwnership()
		{
			InterfaceFieldTestObject data = new InterfaceFieldTestObject
			{
				IntValue = 17,
				InterfaceValue = new int[1] { 42 }
			};
			InterfaceFieldTestObject dataResult = data.DeepClone();

			Assert.IsTrue(data.IntValue.Equals(dataResult.IntValue));
			CollectionAssert.AreEqual(data.InterfaceValue, dataResult.InterfaceValue);
			Assert.AreNotSame(data, dataResult);
			Assert.AreNotSame(data.InterfaceValue, dataResult.InterfaceValue);
		}
		[Test] public void ReferenceBehavior()
		{
			ReferenceBehaviourTestObject data = new ReferenceBehaviourTestObject();
			data.OwnedObject = new ReferencedObject();
			data.ReferencedObject = new ReferencedObject();
			data.WeakReferencedObject = new ReferencedObject();

			// Clone the reference holding object
			ReferenceBehaviourTestObject dataClone = data.DeepClone();

			// Make sure the owned object is cloned, while the referenced object is still referenced
			Assert.IsNotNull(dataClone.OwnedObject);
			Assert.IsNotNull(dataClone.ReferencedObject);
			Assert.IsNull(dataClone.WeakReferencedObject);
			Assert.AreSame(data.ReferencedObject, dataClone.ReferencedObject);
			Assert.AreNotSame(data.OwnedObject, dataClone.OwnedObject);
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
		[Test] public void CombinedReferenceAndWeakReference()
		{
			// See here: https://github.com/AdamsLair/duality/issues/665
			Assert.Inconclusive(
				"Weak references are broken right now, see issue #665. " +
				"Re-activate assert as soon as the issue was addressed.");

			// In this test, we want to check for an edge case where an object is referenced
			// both using a weak and a regular reference, but not part of the ownership graph.
			ReferenceBehaviourTestObject data = new ReferenceBehaviourTestObject();
			data.OwnedObject = new ReferencedObject();
			data.ReferencedObject = new ReferencedObject();
			data.WeakReferencedObject = data.ReferencedObject;

			// Clone the reference holding object
			ReferenceBehaviourTestObject dataClone = data.DeepClone();

			// Expect that the weak reference is gone, but the regular reference persisted
			Assert.IsNotNull(dataClone.OwnedObject);
			Assert.IsNull(dataClone.WeakReferencedObject);
			Assert.IsNotNull(dataClone.ReferencedObject);
			Assert.AreSame(data.ReferencedObject, dataClone.ReferencedObject);
			Assert.AreNotSame(data.OwnedObject, dataClone.OwnedObject);
		}
		[Test] public void SimpleDelegates()
		{
			SimpleDelegateTestObject source = new SimpleDelegateTestObject();
			SimpleDelegateTestObject target;
			source.ListenTo(source);
				
			// Does the event work as expected?
			source.FireEvent();
			Assert.IsTrue(source.PopEventReceived());

			target = source.DeepClone();

			// Does the cloned event work the same?
			target.FireEvent();
			Assert.IsFalse(source.PopEventReceived());
			Assert.IsTrue(target.PopEventReceived());
		}
		[Test] public void StaticDelegates()
		{
			SimpleDelegateTestObject source = new SimpleDelegateTestObject();
			SimpleDelegateTestObject target;

			bool staticEventReceived = false;
			source.SomeEvent += delegate (object sender, EventArgs e)
			{
				staticEventReceived = true;
			};
				
			// Does the static event work as expected?
			source.FireEvent();
			Assert.IsTrue(staticEventReceived);
			staticEventReceived = false;

			target = source.DeepClone();

			// We expect static events to not be cloned due to conceptual ownership inversion in delegates
			target.FireEvent();
			Assert.IsFalse(staticEventReceived);

			// The source should still trigger it though.
			source.FireEvent();
			Assert.IsTrue(staticEventReceived);
		}
		[Test] public void AdditiveDelegates()
		{
			SimpleDelegateTestObject source = new SimpleDelegateTestObject();
			SimpleDelegateTestObject target = new SimpleDelegateTestObject();
			SimpleDelegateTestObject neutral = new SimpleDelegateTestObject();
			neutral.ListenTo(target);
			
			{
				// Does the neutral object receive events?
				target.FireEvent();
				Assert.IsTrue(neutral.PopEventReceived());

				source.DeepCopyTo(target);

				// Does it still receive them after copying the source onto it?
				target.FireEvent();
				Assert.IsTrue(neutral.PopEventReceived());
			}
			
			{
				source.ListenTo(source);

				// Does the source receive its own events?
				source.FireEvent();
				Assert.IsTrue(source.PopEventReceived());
				Assert.IsFalse(source.PopEventReceived());

				source.DeepCopyTo(target);

				// Do both neutral object and target itself receive both target events?
				target.FireEvent();
				Assert.IsTrue(neutral.PopEventReceived());
				Assert.IsFalse(neutral.PopEventReceived());
				Assert.IsTrue(target.PopEventReceived());
				Assert.IsFalse(target.PopEventReceived());
				Assert.IsFalse(source.PopEventReceived());
			}
		}
		[Test] public void ComplexDelegates()
		{
			ComplexDelegateTestObject data = new ComplexDelegateTestObject(new[]
			{
				new ComplexDelegateTestObject(),
				new ComplexDelegateTestObject(new[]
				{
					new ComplexDelegateTestObject(),
					new ComplexDelegateTestObject()
				})
			});
			ComplexDelegateTestObject dataPart = data.Children[1] as ComplexDelegateTestObject;
			ComplexDelegateTestObject fireChild;

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
				ComplexDelegateTestObject dataResultFull = data.DeepClone();
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
				ComplexDelegateTestObject dataResultPart = dataPart.DeepClone();
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
		[Test] public void OverwriteWithNull()
		{
			TestObject source = new TestObject();
			TestObject target = new TestObject(new Random(0), 0);

			Assert.IsNull(source.ListField);
			Assert.IsNotNull(target.ListField);

			source.DeepCopyTo(target);
			
			Assert.IsNull(source.ListField);
			Assert.IsNull(target.ListField);
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
		[Test] public void StructInvestigate()
		{
			Random rnd = new Random();
			TestStructInvestigate source = new TestStructInvestigate(rnd);
			TestStructInvestigate target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreEqual(source, target);
		}
		[Test] public void PartialCloning()
		{
			Random rnd = new Random();
			TestObject sourceStatic = new TestObject {
				DictField = new Dictionary<string,TestObject>
				{
					{ "A", new TestObject() },
					{ "B", new TestObject() },
					{ "C", new TestObject() },
				},
				HashsetField = new HashSet<TestObject>
				{
					new TestObject()
				}
			};
			TestObject sourceDynamic = new TestObject {
				DictField = new Dictionary<string,TestObject>
				{
					{ "A", sourceStatic.DictField["A"] },
					{ "B", sourceStatic.DictField["B"] },
					{ "C", sourceStatic.DictField["C"] },
					{ "Root", sourceStatic },
					{ "Local", new TestObject() },
				},
				HashsetField = new HashSet<TestObject>
				{
					sourceStatic.HashsetField.First()
				}
			};

			// Copy only the static source objects
			CloneProvider provider = new CloneProvider();
			TestObject targetStatic = provider.CloneObject(sourceStatic, true);

			// When copying them again, they shouldn't actually be copied because they're still in the cache
			TestObject targetStatic2 = provider.CloneObject(sourceStatic, true);
			Assert.AreSame(targetStatic, targetStatic2);

			// Now copy the dynamic source objects and expect all static references to be resolved correctly
			TestObject targetDynamic = provider.CloneObject(sourceDynamic, true);
			Assert.AreSame(targetStatic, targetDynamic.DictField["Root"]);
			Assert.AreSame(targetStatic.DictField["A"], targetDynamic.DictField["A"]);
			Assert.AreSame(targetStatic.DictField["B"], targetDynamic.DictField["B"]);
			Assert.AreSame(targetStatic.DictField["C"], targetDynamic.DictField["C"]);
			Assert.AreSame(targetStatic.HashsetField.First(), targetDynamic.HashsetField.First());

			// Now clear the cache and expect new objects from clone operations
			provider.ClearCachedMapping();
			TestObject targetStatic3 = provider.CloneObject(sourceStatic, true);
			Assert.AreNotSame(targetStatic, targetStatic3);

			// Expect an exception when attempting to clone the old results without clearing the cache
			Assert.Throws<InvalidOperationException>(() => provider.CloneObject(targetStatic3, true));

			// It should work fine after clearing the cache though.
			provider.ClearCachedMapping();
			Assert.DoesNotThrow(() => provider.CloneObject(targetStatic3, true));
		}
		[Test] public void PerformanceTest()
		{
			Random rnd = new Random(0);
			TestObject data = new TestObject(rnd, 5);
			TestObject[] results = new TestObject[200];

			GC.Collect();

			var watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = data.DeepClone();
			}
			watch.Stop();
			TestHelper.LogNumericTestResult(this, "CloneTestObjectGraph", watch.Elapsed.TotalMilliseconds, "ms");

			GC.Collect();

			var watch2 = new System.Diagnostics.Stopwatch();
			watch2.Start();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = new TestObject(rnd, 5);
			}
			watch2.Stop();
			TestHelper.LogNumericTestResult(this, "CreateWithoutClone", watch2.Elapsed.TotalMilliseconds, "ms");
			TestHelper.LogNumericTestResult(this, "CloneVersusRaw", watch.Elapsed.TotalMilliseconds / watch2.Elapsed.TotalMilliseconds, null);

			GC.Collect();
			Assert.Pass();
		}
	}
}
