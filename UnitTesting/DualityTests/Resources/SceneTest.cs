using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Renderers;
using Duality.Tests.Components;

using OpenTK;
using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class SceneTest
	{
		private Scene scene;
		private Scene scene2;
		private GameObject obj;
		private GameObject obj2;
		private GameObject objChildA;
		private GameObject objChildB;

		[SetUp] public void Setup()
		{
			this.scene = new Scene();
			this.scene2 = new Scene();

			this.obj = new GameObject("TestObject");
			this.obj2 = new GameObject("TestObject2");
			this.objChildA = new GameObject("TestObjectChildA", this.obj);
			this.objChildB = new GameObject("TestObjectChildB", this.obj);

			this.scene.AddObject(this.obj);
			this.scene2.AddObject(this.obj2);
		}
		[TearDown] public void Teardown()
		{
			this.scene.Dispose();
			this.scene2.Dispose();

			this.scene		= null;
			this.scene2		= null;
			this.obj		= null;
			this.obj2		= null;
			this.objChildA	= null;
			this.objChildB	= null;
		}

		[Test] public void AddRemoveGameObjects()
		{
			// Basic setup check (Added properly?)
			Assert.AreEqual(this.scene, this.obj.ParentScene);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildA, this.objChildB }, this.scene.AllObjects);

			// Removing root object
			this.scene.RemoveObject(this.obj);
			Assert.AreEqual(null, this.obj.ParentScene);
			CollectionAssert.IsEmpty(this.scene.AllObjects);

			// Adding removed root object again
			this.scene.AddObject(this.obj);
			Assert.AreEqual(this.scene, this.obj.ParentScene);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildA, this.objChildB }, this.scene.AllObjects);

			// Remove non-root object
			this.scene.RemoveObject(this.objChildA);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.DoesNotContain(this.obj.Children, this.objChildA);
			Assert.IsNull(this.objChildA.ParentScene);
			Assert.IsNull(this.objChildA.Parent);

			// Clear Scene
			this.scene.Clear();
			CollectionAssert.IsEmpty(this.scene.AllObjects);
		}
		[Test] public void GameObjectHierarchy()
		{
			// Remove non-root object
			this.scene.RemoveObject(this.objChildA);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.DoesNotContain(this.obj.Children, this.objChildA);
			Assert.IsNull(this.objChildA.ParentScene);
			Assert.IsNull(this.objChildA.Parent);

			// Add object implicitly by parent-child relationship
			this.objChildA.Parent = this.obj;
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildA, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.Contains(this.obj.Children, this.objChildA);
			Assert.AreEqual(this.scene, this.objChildA.ParentScene);
			Assert.AreEqual(this.obj, this.objChildA.Parent);

			// Change object parent
			this.objChildA.Parent = this.objChildB;
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildA, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.Contains(this.objChildB.Children, this.objChildA);
			Assert.AreEqual(this.scene, this.objChildA.ParentScene);
			Assert.AreEqual(this.objChildB, this.objChildA.Parent);
		}
		[Test] public void AddRemoveGameObjectsMultiScene()
		{
			// Let object switch Scenes implicitly by parent-child relationship
			this.objChildA.Parent = this.obj2;
			Assert.AreEqual(this.scene2, this.objChildA.ParentScene);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { this.obj2, this.objChildA }, this.scene2.AllObjects);
			CollectionAssert.Contains(this.obj2.Children, this.objChildA);
			Assert.AreEqual(this.obj2, this.objChildA.Parent);

			// Remove object from a Scene it doesn't belong to
			this.scene.RemoveObject(this.obj2);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { this.obj2, this.objChildA }, this.scene2.AllObjects);

			// Add object to Scene, despite belonging to a different Scene
			this.scene.AddObject(this.obj2);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.obj2, this.objChildA, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.IsEmpty(this.scene2.AllObjects);
		}
		[Test] public void GameObjectActiveLists()
		{
			this.scene.AddObject(this.obj2);

			// Inactive objects shouldn't show up in active lists
			this.objChildB.Active = false;
			this.obj2.Active = false;
			CollectionAssert.AreEquivalent(new[] { this.obj, this.obj2, this.objChildA, this.objChildB }, this.scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.objChildA }, this.scene.ActiveObjects);
			CollectionAssert.AreEquivalent(new[] { this.obj, this.obj2 }, this.scene.RootObjects);
			CollectionAssert.AreEquivalent(new[] { this.obj }, this.scene.ActiveRootObjects);
			
			// Activating them again should make them show up again
			this.objChildB.Active = true;
			this.obj2.Active = true;
			CollectionAssert.AreEquivalent(this.scene.AllObjects, this.scene.ActiveObjects);
			CollectionAssert.AreEquivalent(this.scene.RootObjects, this.scene.ActiveRootObjects);
		}
	}
}
