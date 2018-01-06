using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality;
using Duality.Cloning;
using Duality.Components;
using Duality.Components.Physics;

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
