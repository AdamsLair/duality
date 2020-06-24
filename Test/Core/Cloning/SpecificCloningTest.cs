using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Components.Physics;
using Duality.Tests.Cloning.HelperObjects;

using NUnit.Framework;

namespace Duality.Tests.Cloning
{
	[TestFixture]
	public class SpecificCloningTest
	{
		[Test] public void CloneRegex()
		{
			// Expect the source to match one phrase, but not another
			Regex source = new Regex("H[aeu]llo", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(3));
			Assert.IsTrue(source.IsMatch("Test this: Hello World"));
			Assert.IsFalse(source.IsMatch("Test this: Hillo World"));

			// Expect the target to show the same matches
			Regex target = source.DeepClone();
			Assert.IsTrue(target.IsMatch("Test this: Hello World"));
			Assert.IsFalse(target.IsMatch("Test this: Hillo World"));

			// Expect both instances to be different, but equal
			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.Options, target.Options);
			Assert.AreEqual(source.MatchTimeout, target.MatchTimeout);
		}
		[Test] public void CloneCultureInfo()
		{
			CultureInfo source = new CultureInfo("en-US");
			CultureInfo target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreEqual(source, target);
		}

		[Test] public void CloneHashSetValueType()
		{
			Random rnd = new Random(1);
			IEnumerable<int> values = Enumerable.Range(0, 50).Select(i => rnd.Next());
			HashSet<int> source = new HashSet<int>(values);
			HashSet<int> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}
		[Test] public void CloneHashSetReferenceType()
		{
			Random rnd = new Random(1);
			IEnumerable<TestObject> values = Enumerable
				.Range(0, 50)
				.Select(i => new TestObject(rnd, 0));

			HashSet<TestObject> source = new HashSet<TestObject>(values);
			HashSet<TestObject> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}
		[Test] public void CloneHashSetReferenceAndNulls()
		{
			Random rnd = new Random(1);
			IEnumerable<TestObject> values = Enumerable
				.Range(0, 50)
				.Select(i => new TestObject(rnd, 0))
				.Concat(new TestObject[] { null });

			HashSet<TestObject> source = new HashSet<TestObject>(values);
			HashSet<TestObject> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}

		[Test] public void CloneDictionaryValueType()
		{
			Random rnd = new Random(1);
			Dictionary<int, Vector2> source = new Dictionary<int, Vector2>();
			for (int i = 0; i < 50; i++)
			{
				source[i] = rnd.NextVector2();
			}
			Dictionary<int, Vector2> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}
		[Test] public void CloneDictionaryReferenceValue()
		{
			Random rnd = new Random(1);
			Dictionary<int, TestObject> source = new Dictionary<int, TestObject>();
			for (int i = 0; i < 50; i++)
			{
				source[i] = rnd.NextBool() ? new TestObject(rnd, 0) : null;
			}
			Dictionary<int, TestObject> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}
		[Test] public void CloneDictionaryReferenceKey()
		{
			Random rnd = new Random(1);
			Dictionary<TestObject, int> source = new Dictionary<TestObject, int>();
			for (int i = 0; i < 50; i++)
			{
				source[new TestObject(rnd, 0)] = i;
			}
			Dictionary<TestObject, int> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}
		[Test] public void CloneDictionaryReferenceKeyValue()
		{
			Random rnd = new Random(1);
			Dictionary<TestObject, TestObject> source = new Dictionary<TestObject, TestObject>();
			for (int i = 0; i < 50; i++)
			{
				source[new TestObject(rnd, 0)] = rnd.NextBool() ? new TestObject(rnd, 0) : null;
			}
			Dictionary<TestObject, TestObject> target = source.DeepClone();

			Assert.AreNotSame(source, target);
			CollectionAssert.AreEquivalent(source, target);
		}

		[Test] public void CloneContentRef()
		{
			Random rnd = new Random(1);
			TestResource source = new TestResource(rnd);
			ContentRef<TestResource> sourceRef = new ContentRef<TestResource>(source, "SomeTestPath");

			// Expect the Resource to be cloned
			TestResource target = source.DeepClone();
			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);

			// Expect only the reference to be cloned
			ContentRef<TestResource> targetRef = sourceRef.DeepClone();
			Assert.AreEqual(sourceRef.Path, targetRef.Path);
			Assert.AreEqual(sourceRef.ResWeak, targetRef.ResWeak);

			// Let the source reference itself
			source.TestContentRef = source;
			Assert.AreSame(source, source.TestContentRef.ResWeak);

			// Expect the Resource to be cloned and holding a ContentRef to itself
			TestResource targetWithSelfRef = source.DeepClone();
			Assert.AreSame(targetWithSelfRef, targetWithSelfRef.TestContentRef.ResWeak);
		}
		[Test] public void CloneResource()
		{
			Random rnd = new Random(1);
			TestResource source = new TestResource(rnd);
			TestResource target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneComponent()
		{
			Random rnd = new Random(1);
			TestComponent source = new TestComponent(rnd);
			TestComponent target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneGameObject()
		{
			Random rnd = new Random(1);
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = source.DeepClone();

			Assert.AreNotSame(source, target);
			Assert.AreEqual(source.Name, target.Name);
			Assert.AreEqual(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
			Assert.AreNotSame(source.GetComponent<TestComponent>(), target.GetComponent<TestComponent>());
			Assert.AreNotSame(source.GetComponent<TestComponent>().TestReferenceList, target.GetComponent<TestComponent>().TestReferenceList);
		}
		[Test] public void CloneScene()
		{
			Random rnd = new Random(1);
			Scene source = new Scene();
			{
				// Create a basic object hierarchy
				GameObject objA = new GameObject("ObjectA");
				GameObject objB = new GameObject("ObjectB");
				GameObject objC = new GameObject("ObjectC", objA);

				// Create some Components containing data
				objA.AddComponent(new TestComponent(rnd));
				objB.AddComponent(new TestComponent(rnd));
				objC.AddComponent(new TestComponent(rnd));

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

				TestComponent sourceCmp = sourceObj.GetComponent<TestComponent>();
				TestComponent targetCmp = targetObj.GetComponent<TestComponent>();

				// Makre sure the back-references to the scene are set
				Assert.AreSame(sourceObj.Scene, source);
				Assert.AreSame(targetObj.Scene, target);

				// See if the data Components are cloned and intact
				Assert.AreEqual(sourceCmp, targetCmp);
				Assert.AreNotSame(sourceCmp, targetCmp);
				Assert.AreNotSame(sourceCmp.TestReferenceList, targetCmp.TestReferenceList);

				// Check cross-object references
				Assert.AreNotSame(sourceCmp.GameObjectReference, targetCmp.GameObjectReference);
				Assert.AreEqual(sourceCmp.GameObjectReference.FullName, targetCmp.GameObjectReference.FullName);
				Assert.AreNotSame(sourceCmp.ComponentReference, targetCmp.ComponentReference);
				Assert.AreEqual(sourceCmp.ComponentReference.GameObj.FullName, targetCmp.ComponentReference.GameObj.FullName);
			}
		}

		[Test] public void CopyToGameObjectPreservation()
		{
			Random rnd = new Random(1);
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = new GameObject("ObjectB");
			TestComponent targetComponent = new TestComponent(rnd);
			target.AddComponent(targetComponent);

			source.DeepCopyTo(target);

			// Make sure that the target Component has been updated, but not re-created.
			Assert.AreSame(targetComponent, target.GetComponent<TestComponent>());
		}
		[Test] public void CopyToGameObjectAddComponentEvent()
		{
			Random rnd = new Random(1);
			bool componentAddedEventReceived = false;
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = new GameObject("ObjectB");
			target.EventComponentAdded += delegate (object sender, ComponentEventArgs e)
			{
				componentAddedEventReceived = true;
			};

			source.DeepCopyTo(target);

			// Make sure that events are fired properly when adding completely new Components
			Assert.IsTrue(componentAddedEventReceived);

			componentAddedEventReceived = false;
			source.DeepCopyTo(target);

			// Don't fire the event when the Component was already there
			Assert.IsFalse(componentAddedEventReceived);
		}
		[Test] public void CopyToGameObjectParentScene()
		{
			Random rnd = new Random(1);
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = new GameObject("ObjectB");
			Scene scene = new Scene();
			scene.AddObject(target);

			source.DeepCopyTo(target);

			Assert.AreSame(scene, target.Scene);
		}
		[Test] public void CopyToGameObjectParentObject()
		{
			Random rnd = new Random(1);
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject targetParent = new GameObject("Parent");
			GameObject target = new GameObject("ObjectB", targetParent);

			source.DeepCopyTo(target);

			Assert.AreSame(targetParent, target.Parent);
		}
		[Test] public void CopyToGameObjectAddGameObjectEvent()
		{
			Random rnd = new Random(1);
			bool gameObjectAddedEventReceived = false;

			// Prepare a test Scene
			Scene testScene = new Scene();
			GameObject source = new GameObject("ObjectA");
			GameObject sourceChild = new GameObject("Child", source);
			testScene.AddObject(source);
			GameObject target = new GameObject("ObjectB");
			testScene.AddObject(target);

			// Listen to object added events
			EventHandler<GameObjectGroupEventArgs> sceneHandler = delegate (object sender, GameObjectGroupEventArgs e)
			{
				gameObjectAddedEventReceived = true;
			};
			Scene.GameObjectsAdded += sceneHandler;

			// Enter the test Scene, so global Scene events will be fired
			Scene.SwitchTo(testScene);
			try
			{
				// Copy source object to target object
				source.DeepCopyTo(target);

				// Make sure that events are fired properly when adding completely new Components
				Assert.IsTrue(gameObjectAddedEventReceived);

				gameObjectAddedEventReceived = false;
				source.DeepCopyTo(target);

				// Don't fire the event when the Component was already there
				Assert.IsFalse(gameObjectAddedEventReceived);
			}
			finally
			{
				Scene.GameObjectsAdded -= sceneHandler;
				Scene.SwitchTo(null);
			}
		}

		[Test] public void TransformHierarchyInitialized()
		{
			Random rnd = new Random(1);

			// Create a simple parent-child relation
			GameObject sourceParentObj = new GameObject("Parent");
			GameObject sourceChildObj = new GameObject("Child", sourceParentObj);
			Transform sourceParentTransform = sourceParentObj.AddComponent<Transform>();
			Transform sourceChildTransform = sourceChildObj.AddComponent<Transform>();

			// Test whether transform values work relative as expected
			{
				Transform parent = sourceParentTransform;
				Transform child = sourceChildTransform;
				Vector3 parentPosAbs = rnd.NextVector3();
				Vector3 childPosRel = rnd.NextVector3();
				parent.Pos = parentPosAbs;
				child.LocalPos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);

				childPosRel = rnd.NextVector3();
				child.LocalPos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);
			}

			// Clone the object hierarchy
			GameObject targetParentObj = sourceParentObj.DeepClone();
			GameObject targetChildObj = targetParentObj.GetChildByName("Child");
			Transform targetParentTransform = targetParentObj.Transform;
			Transform targetChildTransform = targetChildObj.Transform;

			// Test whether transform values also work for the cloned hierarchy
			{
				Transform parent = targetParentTransform;
				Transform child = targetChildTransform;
				Vector3 parentPosAbs = rnd.NextVector3();
				Vector3 childPosRel = rnd.NextVector3();
				parent.Pos = parentPosAbs;
				child.LocalPos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);

				childPosRel = rnd.NextVector3();
				child.LocalPos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);
			}
		}
		[Test] public void CloneJointRigidBodies()
		{
			// Create two joint bodies
			GameObject sourceA = new GameObject("ObjectA");
			GameObject sourceB = new GameObject("ObjectB", sourceA);
			sourceA.AddComponent<Transform>();
			sourceB.AddComponent<Transform>();
			RigidBody sourceBodyA = sourceA.AddComponent<RigidBody>();
			RigidBody sourceBodyB = sourceB.AddComponent<RigidBody>();
			sourceBodyA.AddJoint(new DistanceJointInfo(), sourceBodyB);

			// Are the two bodies joint together as expected?
			Assert.AreEqual(1, sourceBodyA.Joints.Count());
			Assert.AreSame(sourceBodyA.Joints.First().OtherBody, sourceBodyB);
			Assert.IsTrue(sourceBodyB.Joints == null || !sourceBodyB.Joints.Any());

			// Clone the object hierarchy
			GameObject targetA = sourceA.DeepClone();
			GameObject targetB = targetA.Children[0];
			RigidBody targetBodyA = targetA.GetComponent<RigidBody>();
			RigidBody targetBodyB = targetB.GetComponent<RigidBody>();

			// Is the cloned hierarchy joint together as expected?
			Assert.AreEqual(1, targetBodyA.Joints.Count());
			Assert.IsTrue(targetBodyB.Joints == null || !targetBodyB.Joints.Any());
			Assert.AreSame(targetBodyA.Joints.First().OtherBody, targetBodyB);
			Assert.AreNotSame(sourceBodyA.Joints.First(), targetBodyA.Joints.First());

			// Clone only the source joint, but not any body
			JointInfo isolatedSourceJoint = sourceBodyA.Joints.FirstOrDefault();
			JointInfo isolatedTargetJoint = isolatedSourceJoint.DeepClone();

			// Is the cloned joint still isolated, and not attached to any body?
			Assert.IsNotNull(isolatedSourceJoint.ParentBody);
			Assert.IsNotNull(isolatedSourceJoint.OtherBody);
			Assert.IsNull(isolatedTargetJoint.ParentBody);
			Assert.IsNull(isolatedTargetJoint.OtherBody);
			Assert.AreEqual(1, sourceBodyA.Joints.Count());
		}
		[Test] public void CloneShapeRigidBodies()
		{
			// Create a body with a simple shape
			GameObject source = new GameObject("ObjectA");
			source.AddComponent<Transform>();
			RigidBody sourceBody = source.AddComponent<RigidBody>();
			CircleShapeInfo sourceShape = new CircleShapeInfo(32.0f, Vector2.Zero, 1.0f);
			sourceBody.AddShape(sourceShape);

			// Clone the object hierarchy
			GameObject target = source.DeepClone();
			RigidBody targetBody = target.GetComponent<RigidBody>();
			CircleShapeInfo targetShape = targetBody.Shapes.FirstOrDefault() as CircleShapeInfo;

			// Is the cloned shape set up like the source shape?
			Assert.AreEqual(1, targetBody.Shapes.Count());
			Assert.AreSame(targetShape.Parent, targetBody);
			Assert.AreSame(sourceShape.Parent, sourceBody);
			Assert.AreNotSame(targetShape.Parent, sourceBody);
			Assert.AreNotSame(sourceShape.Parent, targetBody);

			// Clone only the source shape, but not any body
			CircleShapeInfo isolatedSourceShape = sourceShape;
			CircleShapeInfo isolatedTargetShape = isolatedSourceShape.DeepClone();

			// Is the cloned joint still isolated, and not attached to any body?
			Assert.IsNotNull(isolatedSourceShape.Parent);
			Assert.IsNull(isolatedTargetShape.Parent);
			Assert.AreEqual(1, sourceBody.Shapes.Count());
		}
	}
}
