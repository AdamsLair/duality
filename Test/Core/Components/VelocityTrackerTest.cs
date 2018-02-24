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
	public class VelocityTrackerTest
	{
		[Test] public void VelocityTeleportAndMove()
		{
			GameObject obj = new GameObject("TestObject");
			Transform transform = obj.AddComponent<Transform>();
			VelocityTracker tracker = obj.AddComponent<VelocityTracker>();

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
				AssertEqual(Vector3.Zero, tracker.Vel, "Velocity at rest");
				AssertEqual(0.0f, tracker.AngleVel, "Angle velocity at rest");

				// The object is teleported
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, tracker.Vel, "Velocity after teleport");
				AssertEqual(0.0f, tracker.AngleVel, "Angle velocity after teleport");

				// The object is moved
				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				DualityApp.Update(true);
				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), tracker.Vel, "Velocity after move");
				AssertEqual(MathF.RadAngle90, tracker.AngleVel, "Angle velocity after teleport");

				// The object rests for a frame
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, tracker.Vel, "Velocity after one frame rest");
				AssertEqual(0.0f, tracker.AngleVel, "Angle velocity after one frame rest");

				// The object is moved, then teleported
				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				DualityApp.Update(true);
				AssertEqual(Vector3.Zero, tracker.Vel, "Velocity after move, then teleport");
				AssertEqual(0.0f, tracker.AngleVel, "Angle velocity after move, then teleport");

				// The object is moved, then teleported, then moved again
				transform.MoveBy(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle90);
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = MathF.RadAngle90;
				transform.MoveBy(new Vector3(2.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.RadAngle45);
				DualityApp.Update(true);
				AssertEqual(new Vector3(2.0f, 0.0f, 0.0f), tracker.Vel, "Velocity after move, then teleport, then move");
				AssertEqual(MathF.RadAngle45, tracker.AngleVel, "Angle velocity after move, then teleport, then move");
			}
		}
		[Test] public void VelocityInHierarchy()
		{
			// In this test case, we'll set up a parent and a child transform
			// to see if changes to the parent affect the child transforms velocity value
			GameObject parentObj = new GameObject("Parent");
			GameObject obj = new GameObject("Child", parentObj);
			Transform parentTransform = parentObj.AddComponent<Transform>();
			Transform transform = obj.AddComponent<Transform>();
			VelocityTracker tracker = obj.AddComponent<VelocityTracker>();

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

				transform.MoveByLocal(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), tracker.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), tracker.AngleVel, "Absolute child angle velocity");

				// Transformed parent and a moving child
				parentTransform.Pos = new Vector3(1.0f, 2.0f, 3.0f);
				parentTransform.Angle = MathF.DegToRad(90.0f);
				parentTransform.Scale = 2.0f;
				transform.Pos = Vector3.Zero;
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;

				transform.MoveByLocal(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(0.0f, 2.0f, 0.0f), tracker.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), tracker.AngleVel, "Absolute child angle velocity");

				// Moving parent and a transformed child
				parentTransform.Pos = Vector3.Zero;
				parentTransform.Angle = 0.0f;
				parentTransform.Scale = 1.0f;
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;

				parentTransform.MoveByLocal(new Vector3(1.0f, 0.0f, 0.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(1.0f, 0.0f, 0.0f), tracker.Vel, "Absolute child velocity");

				// Moving parent and a transformed, moving child
				parentTransform.Pos = Vector3.Zero;
				parentTransform.Angle = 0.0f;
				parentTransform.Scale = 1.0f;
				transform.Pos = new Vector3(1.0f, 0.0f, 0.0f);
				transform.Angle = 0.0f;
				transform.Scale = 1.0f;

				transform.MoveByLocal(new Vector3(1.0f, 0.0f, 0.0f));
				transform.TurnBy(MathF.DegToRad(90.0f));
				parentTransform.MoveByLocal(new Vector3(1.0f, 0.0f, 0.0f));
				DualityApp.Update(true);

				AssertEqual(new Vector3(2.0f, 0.0f, 0.0f), tracker.Vel, "Absolute child velocity");
				AssertEqual(MathF.DegToRad(90.0f), tracker.AngleVel, "Absolute child angle velocity");
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
