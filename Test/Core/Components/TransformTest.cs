using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Resources;

using NUnit.Framework;
using System.Collections;

namespace Duality.Tests.Components
{
	[TestFixture]
	public class TransformTest
	{
		[Test] public void CommitChangesPropagation()
		{
			// Create a linear transform hierarchy from root to leaf
			GameObject rootObj = new GameObject("Root");
			GameObject childObj = new GameObject("Child", rootObj);
			GameObject child2Obj = new GameObject("Child2", childObj);
			GameObject leafObj = new GameObject("Leaf", child2Obj);
			Transform rootTransform = rootObj.AddComponent<Transform>();
			Transform childTransform = childObj.AddComponent<Transform>();
			Transform child2Transform = child2Obj.AddComponent<Transform>();
			Transform leafTransform = leafObj.AddComponent<Transform>();

			// Subscribe to change events in root and leaf
			Transform.DirtyFlags rootChanges = Transform.DirtyFlags.None;
			Transform.DirtyFlags leafChanges = Transform.DirtyFlags.None;
			rootTransform.EventTransformChanged += (s, e) => { rootChanges |= e.Changes; };
			leafTransform.EventTransformChanged += (s, e) => { leafChanges |= e.Changes; };

			// Change the leaf and make sure all changes are committed
			leafTransform.MoveBy(new Vector2(1.0f, 0.0f));
			leafTransform.CommitChanges();
			rootTransform.CommitChanges();

			// Expect a change event only on the leaf object
			Assert.AreEqual(Transform.DirtyFlags.None, rootChanges, "Root changes after leaf movement");
			Assert.AreEqual(Transform.DirtyFlags.Pos, leafChanges, "Leaf changes after leaf movement");
			rootChanges = Transform.DirtyFlags.None;
			leafChanges = Transform.DirtyFlags.None;

			// Change the root and make sure all changes are committed
			rootTransform.MoveBy(new Vector2(1.0f, 0.0f));
			leafTransform.CommitChanges();
			rootTransform.CommitChanges();

			// Expect a change event on both root and leaf objects
			Assert.AreEqual(Transform.DirtyFlags.Pos, rootChanges, "Root changes after root movement");
			Assert.AreEqual(Transform.DirtyFlags.Pos, leafChanges, "Leaf changes after root movement");
			rootChanges = Transform.DirtyFlags.None;
			leafChanges = Transform.DirtyFlags.None;
		}

		[Test] public void GetLocalPoint()
		{
			Transform transform;

			transform = new Transform()
			{
				Pos = new Vector3(1.0f, 2.0f, 3.0f)
			};
			AssertEqual(new Vector3(2.0f, 6.0f, 6.0f), transform.GetLocalPoint(new Vector3(3.0f, 8.0f, 9.0f)), "Position");
			AssertEqual(new Vector2(2.0f, 6.0f), transform.GetLocalPoint(new Vector2(3.0f, 8.0f)), "2D Position");

			transform = new Transform()
			{
				Angle = MathF.DegToRad(90.0f)
			};
			AssertEqual(new Vector3(8.0f, -3.0f, 9.0f), transform.GetLocalPoint(new Vector3(3.0f, 8.0f, 9.0f)), "Rotation");
			AssertEqual(new Vector2(8.0f, -3.0f), transform.GetLocalPoint(new Vector2(3.0f, 8.0f)), "2D Rotation");

			transform = new Transform()
			{
				Scale = 1.0f / 1.5f
			};
			AssertEqual(new Vector3(4.5f, 12.0f, 13.5f), transform.GetLocalPoint(new Vector3(3.0f, 8.0f, 9.0f)), "Scale");
			AssertEqual(new Vector2(4.5f, 12.0f), transform.GetLocalPoint(new Vector2(3.0f, 8.0f)), "2D Scale");

			transform = new Transform()
			{
				Pos = new Vector3(1.0f, 2.0f, 3.0f),
				Angle = MathF.DegToRad(90.0f),
				Scale = 1.0f / 1.5f
			};
			AssertEqual(new Vector3(9.0f, -3.0f, 9.0f), transform.GetLocalPoint(new Vector3(3.0f, 8.0f, 9.0f)), "Position, Rotation, Scale");
			AssertEqual(new Vector2(9.0f, -3.0f), transform.GetLocalPoint(new Vector2(3.0f, 8.0f)), "2D Position, Rotation, Scale");
		}
		[Test] public void GetWorldPoint()
		{
			Transform transform;

			transform = new Transform()
			{
				Pos = new Vector3(1.0f, 2.0f, 3.0f)
			};
			AssertEqual(new Vector3(4.0f, 8.0f, 12.0f), transform.GetWorldPoint(new Vector3(3.0f, 6.0f, 9.0f)), "Position");
			AssertEqual(new Vector2(4.0f, 8.0f), transform.GetWorldPoint(new Vector2(3.0f, 6.0f)), "2D Position");

			transform = new Transform()
			{
				Angle = MathF.DegToRad(90.0f)
			};
			AssertEqual(new Vector3(-6.0f, 3.0f, 9.0f), transform.GetWorldPoint(new Vector3(3.0f, 6.0f, 9.0f)), "Rotation");
			AssertEqual(new Vector2(-6.0f, 3.0f), transform.GetWorldPoint(new Vector2(3.0f, 6.0f)), "2D Rotation");

			transform = new Transform()
			{
				Scale = 1.5f
			};
			AssertEqual(new Vector3(4.5f, 9.0f, 13.5f), transform.GetWorldPoint(new Vector3(3.0f, 6.0f, 9.0f)), "Scale");
			AssertEqual(new Vector2(4.5f, 9.0f), transform.GetWorldPoint(new Vector2(3.0f, 6.0f)), "2D Scale");

			transform = new Transform()
			{
				Pos = new Vector3(1.0f, 2.0f, 3.0f),
				Angle = MathF.DegToRad(90.0f),
				Scale = 1.5f
			};
			AssertEqual(new Vector3(-8.0f, 6.5f, 16.5f), transform.GetWorldPoint(new Vector3(3.0f, 6.0f, 9.0f)), "Position, Rotation, Scale");
			AssertEqual(new Vector2(-8.0f, 6.5f), transform.GetWorldPoint(new Vector2(3.0f, 6.0f)), "2D Position, Rotation, Scale");
		}

		[Test] public void TransformHierarchy()
		{
			// In this test case, we'll set up a parent and a child transform
			// to see if changes to the parent propagate to the child.
			GameObject parentObj = new GameObject("Parent");
			GameObject obj = new GameObject("Child", parentObj);
			Transform parentTransform = parentObj.AddComponent<Transform>();
			Transform transform = obj.AddComponent<Transform>();

			// Start with a parent transform that does nothing
			transform.RelativePos = new Vector3(1.0f, 2.0f, 3.0f);
			transform.RelativeAngle = MathF.DegToRad(90.0f);
			transform.RelativeScale = 2.0f;
			AssertEqual(new Vector3(1.0f, 4.0f, 3.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Child transform with identity parent");

			// Now we'll adjust the parent transform and see if the child gets it
			parentTransform.RelativePos = new Vector3(4.0f, 5.0f, 6.0f);
			parentTransform.RelativeAngle = MathF.DegToRad(90.0f);
			parentTransform.RelativeScale = 2.0f;
			AssertEqual(new Vector3(-4.0f, 7.0f, 12.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Child transform with parent");

			// Adjust the child transform to identity and make sure we still have the parent transformation
			transform.RelativePos = Vector3.Zero;
			transform.RelativeAngle = 0.0f;
			transform.RelativeScale = 1.0f;
			AssertEqual(new Vector3(4.0f, 7.0f, 6.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Identity child transform with parent");
		}
		[Test] public void TransformHierarchyVelocity()
		{
			// In this test case, we'll set up a parent and a child transform
			// to see if changes to the parent affect the child transforms velocity value
			GameObject parentObj = new GameObject("Parent");
			GameObject obj = new GameObject("Child", parentObj);
			Transform parentTransform = parentObj.AddComponent<Transform>();
			Transform transform = obj.AddComponent<Transform>();

			// Since velocity values are only updated after the frame ends, we need
			// a full scene setup to simulate an update cycle
			using (Scene testScene = new Scene())
			{
				// Setup and enter the scene
				testScene.AddObject(parentObj);
				Scene.SwitchTo(testScene, true);
				DualityApp.Update(true);

				// Identity parent and a moving child
				parentTransform.Pos = Vector3.Zero;
				parentTransform.Angle = 0.0f;
				parentTransform.Scale = 1.0f;
				transform.Pos = Vector3.Zero;
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;

				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.RelativeAngleVel, "Relative child angle velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.AngleVel, "Absolute child angle velocity");

				// Transformed parent and a moving child
				parentTransform.Pos = new Vector3(1.0f, 2.0f, 3.0f);
				parentTransform.Angle = MathF.DegToRad(90.0f);
				parentTransform.Scale = 2.0f;
				transform.Pos = Vector3.Zero;
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;

				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				AssertEqual(new Vector3(0.0f, 2.0f, 0.0f), transform.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.RelativeAngleVel, "Relative child angle velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.AngleVel, "Absolute child angle velocity");

				// Moving parent and a transformed child
				parentTransform.Pos = Vector3.Zero;
				parentTransform.Angle = 0.0f;
				parentTransform.Scale = 1.0f;
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;
				
				parentTransform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				DualityApp.Update(true);
				
				AssertEqual(new Vector3(0.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.Vel, "Absolute child velocity");

				// Moving parent and a transformed, moving child
				parentTransform.Pos = Vector3.Zero;
				parentTransform.Angle = 0.0f;
				parentTransform.Scale = 1.0f;
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;
				
				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				parentTransform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				DualityApp.Update(true);
				
				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				AssertEqual(new Vector3(2.0f, 0.0f, 0.0f), transform.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.RelativeAngleVel, "Relative child angle velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.AngleVel, "Absolute child angle velocity");

				// ToDo: Fix how Transform adds up velocity due to parent transform
				// rotation in order to make the test cases below work. The current 
				// implementation only works with small per-frame rotations, not big, 
				// sudden ones.

				// Moving parent and a transformed child
				//parentTransform.Pos = Vector3.Zero;
				//parentTransform.Angle = 0.0f;
				//parentTransform.Scale = 1.0f;
				//transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				//transform.Angle = 0.0f;
				//transform.Scale = 1.0f;
				//
				//parentTransform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				//parentTransform.TurnBy(MathF.DegToRad(90.0f));
				//DualityApp.Update(true);
				//
				//AssertEqual(new Vector3(0.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				//AssertEqual(new Vector3(0.0f, 1.0f, 0.0f), transform.Vel, "Absolute child velocity");
				//AssertEqual(MathF.DegToRad(0.0f), transform.RelativeAngleVel, "Relative child angle velocity");
				//AssertEqual(MathF.DegToRad(90.0f), transform.AngleVel, "Absolute child angle velocity");

				// Moving parent and a transformed, moving child
				//parentTransform.Pos = Vector3.Zero;
				//parentTransform.Angle = 0.0f;
				//parentTransform.Scale = 1.0f;
				//transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				//transform.Angle = 0.0f;
				//transform.Scale = 1.0f;
				//
				//transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				//transform.TurnBy(MathF.DegToRad(90.0f));
				//parentTransform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				//parentTransform.TurnBy(MathF.DegToRad(90.0f));
				//DualityApp.Update(true);
				//
				//AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.RelativeVel, "Relative child velocity");
				//AssertEqual(new Vector3(-1.0f, 3.0f, 0.0f), transform.Vel, "Absolute child velocity");
				//AssertEqual(MathF.DegToRad(90.0f), transform.RelativeAngleVel, "Relative child angle velocity");
				//AssertEqual(MathF.DegToRad(180.0f), transform.AngleVel, "Absolute child angle velocity");
			}
		}

		private static void AssertEqual(float expected, float actual, string message)
		{
			float threshold = 0.00001f;
			Assert.AreEqual(expected, actual, threshold, message);
		}
		private static void AssertEqual(Vector2 expected, Vector2 actual, string message)
		{
			float threshold = 0.00001f;
			Assert.AreEqual(expected.X, actual.X, threshold, message);
			Assert.AreEqual(expected.Y, actual.Y, threshold, message);
		}
		private static void AssertEqual(Vector3 expected, Vector3 actual, string message)
		{
			float threshold = 0.00001f;
			Assert.AreEqual(expected.X, actual.X, threshold, message);
			Assert.AreEqual(expected.Y, actual.Y, threshold, message);
			Assert.AreEqual(expected.Z, actual.Z, threshold, message);
		}
	}
}
