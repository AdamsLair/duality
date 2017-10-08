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
	}
}
