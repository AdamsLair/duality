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

		public static IEnumerable GetLocalPointVector2Cases
		{
			get
			{
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f) }, new Vector2(3f, 8f), new Vector2(2f, 6f)).SetName("GetLocalPointVector2Position");
				yield return new TestCaseData(new Transform() { Angle = MathF.DegToRad(90) }, new Vector2(3f, 8f), new Vector2(8f, -3f)).SetName("GetLocalPointVector2Rotation");
				yield return new TestCaseData(new Transform() { Scale = 1f / 1.5f }, new Vector2(3f, 8f), new Vector2(4.5f, 12f)).SetName("GetLocalPointVector2Scale");
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f), Angle = MathF.DegToRad(90), Scale = 1f / 1.5f }, new Vector2(3f, 8f), new Vector2(9f, -3f)).SetName("GetLocalPointVector2All");
			}
		}

		[Test]
		[TestCaseSource(typeof(TransformTest), "GetLocalPointVector2Cases")]
		public void GetLocalPointVector2(Transform transform, Vector2 vec, Vector2 expected)
		{
			Vector2 result = transform.GetLocalPoint(vec);
			Assert.AreEqual(expected.X, result.X, 0.00001f);
			Assert.AreEqual(expected.Y, result.Y, 0.00001f);
		}

		public static IEnumerable GetLocalPointVector3Cases
		{
			get
			{
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f) }, new Vector3(3f, 8f, 9f), new Vector3(2f, 6f, 6f)).SetName("GetLocalPointVector3Position"); ;
				yield return new TestCaseData(new Transform() { Angle = MathF.DegToRad(90) }, new Vector3(3f, 8f, 9f), new Vector3(8f, -3f, 9f)).SetName("GetLocalPointVector3Rotation"); ;
				yield return new TestCaseData(new Transform() { Scale = 1f / 1.5f }, new Vector3(3f, 8f, 9f), new Vector3(4.5f, 12f, 13.5f)).SetName("GetLocalPointVector3Scale"); ;
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f), Angle = MathF.DegToRad(90), Scale = 1f / 1.5f }, new Vector3(3f, 8f, 9f), new Vector3(9f, -3f, 9f)).SetName("GetLocalPointVector3All"); ;
			}
		}

		[Test]
		[TestCaseSource(typeof(TransformTest), "GetLocalPointVector3Cases")]
		public void GetLocalPointVector3(Transform transform, Vector3 vec, Vector3 expected)
		{
			Vector3 result = transform.GetLocalPoint(vec);
			Assert.AreEqual(expected.X, result.X, 0.00001f);
			Assert.AreEqual(expected.Y, result.Y, 0.00001f);
			Assert.AreEqual(expected.Z, result.Z, 0.00001f);
		}


		public static IEnumerable GetWorldPointVector2Cases
		{
			get
			{
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f) }, new Vector2(3f, 6f), new Vector2(4f, 8f)).SetName("GetWorldPointVector2Position");
				yield return new TestCaseData(new Transform() { Angle = MathF.DegToRad(90) }, new Vector2(3f, 6f), new Vector2(-6f, 3f)).SetName("GetWorldPointVector2Rotation");
				yield return new TestCaseData(new Transform() { Scale = 1.5f }, new Vector2(3f, 6f), new Vector2(4.5f, 9f)).SetName("GetWorldPointVector2Scale");
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f), Angle = MathF.DegToRad(90), Scale = 1.5f }, new Vector2(3f, 6f), new Vector2(-8f, 6.5f)).SetName("GetWorldPointVector2All");
			}
		}

		[Test]
		[TestCaseSource(typeof(TransformTest), "GetWorldPointVector2Cases")]
		public void GetWorldPointVector2(Transform transform, Vector2 vec, Vector2 expected)
		{
			Vector2 result = transform.GetWorldPoint(vec);
			Assert.AreEqual(expected.X, result.X, 0.00001f);
			Assert.AreEqual(expected.Y, result.Y, 0.00001f);
		}

		public static IEnumerable GetWorldPointVector3Cases
		{
			get
			{
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f) }, new Vector3(3f, 6f, 9f), new Vector3(4f, 8f, 12f)).SetName("GetWorldPointVector3Position"); ;
				yield return new TestCaseData(new Transform() { Angle = MathF.DegToRad(90) }, new Vector3(3f, 6f, 9f), new Vector3(-6f, 3f, 9f)).SetName("GetWorldPointVector3Rotation"); ;
				yield return new TestCaseData(new Transform() { Scale = 1.5f }, new Vector3(3f, 6f, 9f), new Vector3(4.5f, 9f, 13.5f)).SetName("GetWorldPointVector3Scale"); ;
				yield return new TestCaseData(new Transform() { Pos = new Vector3(1f, 2f, 3f), Angle = MathF.DegToRad(90), Scale = 1.5f }, new Vector3(3f, 6f, 9f), new Vector3(-8f, 6.5f, 16.5f)).SetName("GetWorldPointVector3All"); ;
			}
		}

		[Test]
		[TestCaseSource(typeof(TransformTest), "GetWorldPointVector3Cases")]
		public void GetWorldPointVector3(Transform transform, Vector3 vec, Vector3 expected)
		{
			Vector3 result = transform.GetWorldPoint(vec);
			Assert.AreEqual(expected.X, result.X, 0.00001f);
			Assert.AreEqual(expected.Y, result.Y, 0.00001f);
			Assert.AreEqual(expected.Z, result.Z, 0.00001f);
		}
	}
}
