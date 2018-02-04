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
		
		[Test] public void TransformFlat()
		{
			GameObject obj = new GameObject("TestObject");
			Transform transform = obj.AddComponent<Transform>();

			// For transform objects without a parent, it shouldn't matter
			// whether we edit local or world values.
			transform.Pos = new Vector3(1, 2, 3);
			AssertEqual(new Vector3(1, 2, 3), transform.Pos, "World Position");
			AssertEqual(new Vector3(1, 2, 3), transform.RelativePos, "Local Position");
			transform.RelativePos = new Vector3(2, 3, 4);
			AssertEqual(new Vector3(2, 3, 4), transform.Pos, "World Position");
			AssertEqual(new Vector3(2, 3, 4), transform.RelativePos, "Local Position");

			transform.Angle = MathF.RadAngle30;
			AssertEqual(MathF.RadAngle30, transform.Angle, "World Angle");
			AssertEqual(MathF.RadAngle30, transform.RelativeAngle, "Local Angle");
			transform.RelativeAngle = MathF.RadAngle45;
			AssertEqual(MathF.RadAngle45, transform.Angle, "World Angle");
			AssertEqual(MathF.RadAngle45, transform.RelativeAngle, "Local Angle");

			transform.Scale = 2.0f;
			AssertEqual(2.0f, transform.Scale, "World Scale");
			AssertEqual(2.0f, transform.RelativeScale, "Local Scale");
			transform.RelativeScale = 3.0f;
			AssertEqual(3.0f, transform.Scale, "World Scale");
			AssertEqual(3.0f, transform.RelativeScale, "Local Scale");
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

				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.Vel, "Absolute child velocity");
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

				AssertEqual(new Vector3(0.0f, 2.0f, 0.0f), transform.Vel, "Absolute child velocity");
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
				
				AssertEqual(new Vector3(2.0f, 0.0f, 0.0f), transform.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), transform.AngleVel, "Absolute child angle velocity");
			}
		}

		[Test] public void VelocityTeleportAndMove()
		{
			GameObject obj = new GameObject("TestObject");
			Transform transform = obj.AddComponent<Transform>();

			// Since velocity values are only updated after the frame ends, we need
			// a full scene setup to simulate an update cycle
			using (Scene testScene = new Scene())
			{
				// Setup and enter the scene
				testScene.AddObject(obj);
				Scene.SwitchTo(testScene, true);
				DualityApp.Update(true);

				// The object is at rest
				transform.Pos = Vector3.Zero;
				transform.Angle = 0.0f;
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, transform.Vel, "Velocity at rest");
				AssertEqual(0.0f, transform.AngleVel, "Angle velocity at rest");

				// The object is teleported
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, transform.Vel, "Velocity after teleport");
				AssertEqual(0.0f, transform.AngleVel, "Angle velocity after teleport");

				// The object is moved
				transform.MoveByAbs(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				DualityApp.Update(true);
				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), transform.Vel, "Velocity after move");
				AssertEqual(MathF.RadAngle90, transform.AngleVel, "Angle velocity after teleport");

				// The object rests for a frame
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, transform.Vel, "Velocity after one frame rest");
				AssertEqual(0.0f, transform.AngleVel, "Angle velocity after one frame rest");

				// The object is moved, then teleported
				transform.MoveByAbs(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, transform.Vel, "Velocity after move, then teleport");
				AssertEqual(0.0f, transform.AngleVel, "Angle velocity after move, then teleport");

				// The object is moved, then teleported, then moved again
				transform.MoveByAbs(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				transform.MoveByAbs(new Vector3(2.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle45);
				DualityApp.Update(true);
				AssertEqual(new Vector3(2.0f, 0.0f, 0.0f), transform.Vel, "Velocity after move, then teleport, then move");
				AssertEqual(MathF.RadAngle45, transform.AngleVel, "Angle velocity after move, then teleport, then move");
			}
		}

		private static void AssertEqual(float expected, float actual, string message)
		{
			float threshold = 0.00001f;
			if (MathF.Abs(expected - actual) > threshold)
			{
				Assert.Fail("{0}: Expected {1}, but got {2} instead.",
					message,
					expected,
					actual);
			}
		}
		private static void AssertEqual(Vector2 expected, Vector2 actual, string message)
		{
			float threshold = 0.00001f;
			if (MathF.Abs(expected.X - actual.X) > threshold ||
				MathF.Abs(expected.Y - actual.Y) > threshold)
			{
				Assert.Fail("{0}: Expected {1}, but got {2} instead.",
					message,
					expected,
					actual);
			}
		}
		private static void AssertEqual(Vector3 expected, Vector3 actual, string message)
		{
			float threshold = 0.00001f;
			if (MathF.Abs(expected.X - actual.X) > threshold ||
				MathF.Abs(expected.Y - actual.Y) > threshold ||
				MathF.Abs(expected.Z - actual.Z) > threshold)
			{
				Assert.Fail("{0}: Expected {1}, but got {2} instead.",
					message,
					expected,
					actual);
			}
		}
	}
}
