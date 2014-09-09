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
using Duality.Components.Physics;
using Duality.Tests.Cloning.HelperObjects;

using OpenTK;
using NUnit.Framework;

namespace Duality.Tests.Cloning
{
	[TestFixture]
	public class SpecificCloningTest
	{
		[Test] public void CloneContentRef()
		{
			Random rnd = new Random();
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
		}
		[Test] public void CloneResource()
		{
			Random rnd = new Random();
			TestResource source = new TestResource(rnd);
			TestResource target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneComponent()
		{
			Random rnd = new Random();
			TestComponent source = new TestComponent(rnd);
			TestComponent target = source.DeepClone();

			Assert.AreEqual(source, target);
			Assert.AreNotSame(source, target);
			Assert.AreNotSame(source.TestReferenceList, target.TestReferenceList);
		}
		[Test] public void CloneGameObject()
		{
			Random rnd = new Random();
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
			Random rnd = new Random();
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
			Random rnd = new Random();
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = new GameObject("ObjectB");
			TestComponent targetComponent = target.AddComponent(new TestComponent(rnd));

			source.DeepCopyTo(target);
			
			// Make sure that the target Component has been updated, but not re-created.
			Assert.AreSame(targetComponent, target.GetComponent<TestComponent>());
		}
		[Test] public void CopyToGameObjectAddComponentEvent()
		{
			Random rnd = new Random();
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
			Random rnd = new Random();
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject target = new GameObject("ObjectB");
			Scene scene = new Scene();
			scene.AddObject(target);

			source.DeepCopyTo(target);
			
			Assert.AreSame(scene, target.ParentScene);
		}
		[Test] public void CopyToGameObjectParentObject()
		{
			Random rnd = new Random();
			GameObject source = new GameObject("ObjectA");
			source.AddComponent(new TestComponent(rnd));
			GameObject targetParent = new GameObject("Parent");
			GameObject target = new GameObject("ObjectB", targetParent);

			source.DeepCopyTo(target);
			
			Assert.AreSame(targetParent, target.Parent);
		}
		[Test] public void CopyToGameObjectAddGameObjectEvent()
		{
			Random rnd = new Random();
			bool gameObjectAddedEventReceived = false;

			// Prepare a test Scene
			Scene testScene = new Scene();
			GameObject source = new GameObject("ObjectA");
			GameObject sourceChild = new GameObject("Child", source);
			testScene.AddObject(source);
			GameObject target = new GameObject("ObjectB");
			testScene.AddObject(target);
			
			// Listen to object added events
			EventHandler<GameObjectEventArgs> sceneHandler = delegate (object sender, GameObjectEventArgs e)
			{
				gameObjectAddedEventReceived = true;
			};
			Scene.GameObjectAdded += sceneHandler;

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
				Scene.GameObjectAdded -= sceneHandler;
				Scene.SwitchTo(null);
			}
		}
		[Test] public void TransformHierarchyInitialized()
		{
			Random rnd = new Random();

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
				child.RelativePos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);

				childPosRel = rnd.NextVector3();
				child.RelativePos = childPosRel;
			
				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);
			}

			// Clone the object hierarchy
			GameObject targetParentObj = sourceParentObj.DeepClone();
			GameObject targetChildObj = targetParentObj.ChildByName("Child");
			Transform targetParentTransform = targetParentObj.Transform;
			Transform targetChildTransform = targetChildObj.Transform;

			// Test whether transform values also work for the cloned hierarchy
			{
				Transform parent = targetParentTransform;
				Transform child = targetChildTransform;
				Vector3 parentPosAbs = rnd.NextVector3();
				Vector3 childPosRel = rnd.NextVector3();
				parent.Pos = parentPosAbs;
				child.RelativePos = childPosRel;

				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);

				childPosRel = rnd.NextVector3();
				child.RelativePos = childPosRel;
			
				Assert.AreEqual(parentPosAbs.X + childPosRel.X, child.Pos.X, 0.000001f);
				Assert.AreEqual(parentPosAbs.Y + childPosRel.Y, child.Pos.Y, 0.000001f);
				Assert.AreEqual(parentPosAbs.Z + childPosRel.Z, child.Pos.Z, 0.000001f);
			}
		}
		[Test] public void RealWorldPerformanceTest()
		{
			var watch = new System.Diagnostics.Stopwatch();

			Random rnd = new Random(0);
			GameObject data = new GameObject("CloneRoot");
			for (int i = 0; i < 1000; i++)
			{
				GameObject child = new GameObject("Child", data);
				child.AddComponent<Transform>();
				if (i % 3 != 0) child.AddComponent<SpriteRenderer>();
				if (i % 3 == 0) child.AddComponent<RigidBody>();
				if (i % 7 == 0) child.AddComponent<TextRenderer>();
			}
			GameObject[] results = new GameObject[25];

			watch.Start();
			for (int i = 0; i < results.Length; i++)
			{
				results[i] = data.DeepClone();
			}
			watch.Stop();
			TestHelper.LogNumericTestResult(this, "RealWorldPerformanceTest", watch.Elapsed.TotalMilliseconds, "ms");
			// Target: 250

			Assert.Pass();
		}
	}
}
