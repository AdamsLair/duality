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
			AssertEqual(new Vector3(1, 2, 3), transform.LocalPos, "Local Position");
			transform.LocalPos = new Vector3(2, 3, 4);
			AssertEqual(new Vector3(2, 3, 4), transform.Pos, "World Position");
			AssertEqual(new Vector3(2, 3, 4), transform.LocalPos, "Local Position");

			transform.Angle = MathF.RadAngle30;
			AssertEqual(MathF.RadAngle30, transform.Angle, "World Angle");
			AssertEqual(MathF.RadAngle30, transform.LocalAngle, "Local Angle");
			transform.LocalAngle = MathF.RadAngle45;
			AssertEqual(MathF.RadAngle45, transform.Angle, "World Angle");
			AssertEqual(MathF.RadAngle45, transform.LocalAngle, "Local Angle");

			transform.Scale = 2.0f;
			AssertEqual(2.0f, transform.Scale, "World Scale");
			AssertEqual(2.0f, transform.LocalScale, "Local Scale");
			transform.LocalScale = 3.0f;
			AssertEqual(3.0f, transform.Scale, "World Scale");
			AssertEqual(3.0f, transform.LocalScale, "Local Scale");
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
			transform.LocalPos = new Vector3(1.0f, 2.0f, 3.0f);
			transform.LocalAngle = MathF.DegToRad(90.0f);
			transform.LocalScale = 2.0f;
			AssertEqual(new Vector3(1.0f, 4.0f, 3.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Child transform with identity parent");

			// Now we'll adjust the parent transform and see if the child gets it
			parentTransform.LocalPos = new Vector3(4.0f, 5.0f, 6.0f);
			parentTransform.LocalAngle = MathF.DegToRad(90.0f);
			parentTransform.LocalScale = 2.0f;
			AssertEqual(new Vector3(-4.0f, 7.0f, 12.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Child transform with parent");

			// Adjust the child transform to identity and make sure we still have the parent transformation
			transform.LocalPos = Vector3.Zero;
			transform.LocalAngle = 0.0f;
			transform.LocalScale = 1.0f;
			AssertEqual(new Vector3(4.0f, 7.0f, 6.0f), transform.GetWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)), "Identity child transform with parent");
		}
		[Test] public void SwitchParent()
		{
			GameObject parentA = new GameObject("ParentA");
			GameObject parentB = new GameObject("ParentB");
			GameObject obj = new GameObject("Child");
			Transform transformParentA = parentA.AddComponent<Transform>();
			Transform transformParentB = parentB.AddComponent<Transform>();
			Transform transform = obj.AddComponent<Transform>();

			transformParentA.Pos = new Vector3(1, 2, 3);
			transformParentA.Angle = MathF.RadAngle90;
			transformParentA.Scale = 2.0f;

			transformParentB.Pos = new Vector3(2, 3, 4);
			transformParentB.Angle = MathF.RadAngle180;
			transformParentB.Scale = 4.0f;

			// Set up the test object without parent.
			// Expect its world space to match local space.
			transform.Pos = new Vector3(3, 4, 5);
			transform.Angle = MathF.RadAngle270;
			transform.Scale = 1.0f;

			AssertEqual(transform.LocalPos, transform.Pos, "Position");
			AssertEqual(transform.LocalAngle, transform.Angle, "Angle");
			AssertEqual(transform.LocalScale, transform.Scale, "Scale");

			// Re-parent the test object to parent A.
			// Expect its world space values to remain, its local space to be recalculated.
			obj.Parent = parentA;

			AssertEqual(new Vector3(3, 4, 5), transform.Pos, "Position");
			AssertEqual(MathF.RadAngle270, transform.Angle, "Angle");
			AssertEqual(1.0f, transform.Scale, "Scale");

			AssertEqual(new Vector3(1, -1, 1), transform.LocalPos, "Position");
			AssertEqual(MathF.RadAngle180, transform.LocalAngle, "Angle");
			AssertEqual(0.5f, transform.LocalScale, "Scale");

			// Re-parent the test object to parent B.
			// Expect its world space values to remain, its local space to be recalculated.
			obj.Parent = parentB;

			AssertEqual(new Vector3(3, 4, 5), transform.Pos, "Position");
			AssertEqual(MathF.RadAngle270, transform.Angle, "Angle");
			AssertEqual(1.0f, transform.Scale, "Scale");

			AssertEqual(new Vector3(-0.25f, -0.25f, 0.25f), transform.LocalPos, "Position");
			AssertEqual(MathF.RadAngle90, transform.LocalAngle, "Angle");
			AssertEqual(0.25f, transform.LocalScale, "Scale");
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
