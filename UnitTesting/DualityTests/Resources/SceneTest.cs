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
		[Test] public void SwitchTo()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();

			// Switch to the first Scene regularly
			{
				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}
			
			// Switch to the second during update
			{
				GameObject obj = new GameObject("SwitchObject");
				var switchComponent = new UpdateSwitchToSceneComponent { Target = scene2 };
				obj.AddComponent(switchComponent);
				scene.AddObject(obj);

				DualityApp.Update();
				Assert.False(switchComponent.Switched);
				Assert.AreEqual(Scene.Current, scene2);

				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}
			
			// Switch to the second while entering
			{
				Scene.SwitchTo(null);

				GameObject obj = new GameObject("SwitchObject");
				var switchComponent = new InitSwitchToSceneComponent { Target = scene2 };
				obj.AddComponent(switchComponent);
				scene.AddObject(obj);

				Scene.SwitchTo(scene);
				DualityApp.Update();
				Assert.False(switchComponent.Switched);
				Assert.AreEqual(Scene.Current, scene2);

				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}
			
			// Switch to the second while leaving
			{
				Scene.SwitchTo(null);

				GameObject obj = new GameObject("SwitchObject");
				var switchComponent = new ShutdownSwitchToSceneComponent { Target = scene2 };
				obj.AddComponent(switchComponent);
				scene.AddObject(obj);

				Scene.SwitchTo(scene);
				DualityApp.Update();
				Assert.False(switchComponent.Switched);
				Assert.AreEqual(Scene.Current, scene2);

				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void AddRemoveGameObjects()
		{
			// Set up some objects
			Scene scene = new Scene();
			GameObject obj = new GameObject("TestObject");
			GameObject objChildA = new GameObject("TestObjectChildA", obj);
			GameObject objChildB = new GameObject("TestObjectChildB", obj);
			scene.AddObject(obj);

			// Basic setup check (Added properly?)
			Assert.AreEqual(scene, obj.ParentScene);
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);

			// Removing root object
			scene.RemoveObject(obj);
			Assert.AreEqual(null, obj.ParentScene);
			CollectionAssert.IsEmpty(scene.AllObjects);

			// Adding removed root object again
			scene.AddObject(obj);
			Assert.AreEqual(scene, obj.ParentScene);
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);

			// Remove non-root object
			scene.RemoveObject(objChildA);
			CollectionAssert.AreEquivalent(new[] { obj, objChildB }, scene.AllObjects);
			CollectionAssert.DoesNotContain(obj.Children, objChildA);
			Assert.IsNull(objChildA.ParentScene);
			Assert.IsNull(objChildA.Parent);

			// Clear Scene
			scene.Clear();
			CollectionAssert.IsEmpty(scene.AllObjects);

			// Clean up
			scene.Dispose();
		}
		[Test] public void GameObjectHierarchy()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();
			GameObject obj = new GameObject("TestObject");
			GameObject obj2 = new GameObject("TestObject2");
			GameObject objChildA = new GameObject("TestObjectChildA", obj);
			GameObject objChildB = new GameObject("TestObjectChildB", obj);
			scene.AddObject(obj);
			scene2.AddObject(obj2);

			// Remove non-root object
			scene.RemoveObject(objChildA);
			CollectionAssert.AreEquivalent(new[] { obj, objChildB }, scene.AllObjects);
			CollectionAssert.DoesNotContain(obj.Children, objChildA);
			Assert.IsNull(objChildA.ParentScene);
			Assert.IsNull(objChildA.Parent);

			// Add object implicitly by parent-child relationship
			objChildA.Parent = obj;
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.Contains(obj.Children, objChildA);
			Assert.AreEqual(scene, objChildA.ParentScene);
			Assert.AreEqual(obj, objChildA.Parent);

			// Change object parent
			objChildA.Parent = objChildB;
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.Contains(objChildB.Children, objChildA);
			Assert.AreEqual(scene, objChildA.ParentScene);
			Assert.AreEqual(objChildB, objChildA.Parent);

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void AddRemoveGameObjectsMultiScene()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();
			GameObject obj = new GameObject("TestObject");
			GameObject obj2 = new GameObject("TestObject2");
			GameObject objChildA = new GameObject("TestObjectChildA", obj);
			GameObject objChildB = new GameObject("TestObjectChildB", obj);
			scene.AddObject(obj);
			scene2.AddObject(obj2);

			// Let object switch Scenes implicitly by parent-child relationship
			objChildA.Parent = obj2;
			Assert.AreEqual(scene2, objChildA.ParentScene);
			CollectionAssert.AreEquivalent(new[] { obj, objChildB }, scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { obj2, objChildA }, scene2.AllObjects);
			CollectionAssert.Contains(obj2.Children, objChildA);
			Assert.AreEqual(obj2, objChildA.Parent);

			// Remove object from a Scene it doesn't belong to
			scene.RemoveObject(obj2);
			CollectionAssert.AreEquivalent(new[] { obj, objChildB }, scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { obj2, objChildA }, scene2.AllObjects);

			// Add object to Scene, despite belonging to a different Scene
			scene.AddObject(obj2);
			CollectionAssert.AreEquivalent(new[] { obj, obj2, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.IsEmpty(scene2.AllObjects);

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void GameObjectActiveLists()
		{
			// Set up some objects
			Scene scene = new Scene();
			GameObject obj = new GameObject("TestObject");
			GameObject obj2 = new GameObject("TestObject2");
			GameObject objChildA = new GameObject("TestObjectChildA", obj);
			GameObject objChildB = new GameObject("TestObjectChildB", obj);
			scene.AddObject(obj);
			scene.AddObject(obj2);

			// Inactive objects shouldn't show up in active lists
			objChildB.Active = false;
			obj2.Active = false;
			CollectionAssert.AreEquivalent(new[] { obj, obj2, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.AreEquivalent(new[] { obj, objChildA }, scene.ActiveObjects);
			CollectionAssert.AreEquivalent(new[] { obj, obj2 }, scene.RootObjects);
			CollectionAssert.AreEquivalent(new[] { obj }, scene.ActiveRootObjects);
			
			// Activating them again should make them show up again
			objChildB.Active = true;
			obj2.Active = true;
			CollectionAssert.AreEquivalent(scene.AllObjects, scene.ActiveObjects);
			CollectionAssert.AreEquivalent(scene.RootObjects, scene.ActiveRootObjects);

			// Clean up
			scene.Dispose();
		}

		private class UpdateSwitchToSceneComponent : Component, ICmpUpdatable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpUpdatable.OnUpdate()
			{
				Scene last = Scene.Current;
				Scene.SwitchTo(Target);
				this.Switched = (last != Scene.Current && Scene.Current == Target);
			}
		}
		private class InitSwitchToSceneComponent : Component, ICmpInitializable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpInitializable.OnInit(InitContext context)
			{
				if (context == InitContext.Activate)
				{
					Scene last = Scene.Current;
					Scene.SwitchTo(Target);
					this.Switched = (last != Scene.Current && Scene.Current == Target);
				}
			}
			void ICmpInitializable.OnShutdown(ShutdownContext context) {}
		}
		private class ShutdownSwitchToSceneComponent : Component, ICmpInitializable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpInitializable.OnInit(InitContext context) {}
			void ICmpInitializable.OnShutdown(ShutdownContext context)
			{
				if (context == ShutdownContext.Deactivate)
				{
					Scene last = Scene.Current;
					Scene.SwitchTo(Target);
					this.Switched = (last != Scene.Current && Scene.Current == Target);
				}
			}
		}
	}
}
