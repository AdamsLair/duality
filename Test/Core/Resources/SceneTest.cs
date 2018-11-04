using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Duality;
using Duality.Drawing;
using Duality.Resources;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Components.Renderers;

using Duality.Tests.Components;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class SceneTest
	{
		[Test] public void SwitchToNonExistent()
		{
			// Switching to an explicit null Scene should work
			Scene.SwitchTo(null);
			Assert.IsTrue(Scene.Current.IsEmpty);

			// Switching to a Scene that doesn't exist should throw an exception
			Assert.Throws<ArgumentException>(() => Scene.SwitchTo(new ContentRef<Scene>(null, "I_Dont_Exist\\Invalid_Path")));
		}
		[Test] public void SwitchToRegular()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();

			// Switch to the first Scene regularly
			{
				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void SwitchToDuringUpdate()
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

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void SwitchToDuringEnter()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();

			// Switch to the first Scene regularly
			{
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
			}

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void SwitchToDuringLeave()
		{
			// Set up some objects
			Scene scene = new Scene();
			Scene scene2 = new Scene();

			// Switch to the first Scene regularly
			{
				Scene.SwitchTo(scene);
				Assert.AreEqual(Scene.Current, scene);
			}

			// Switch to the second while leaving
			{
				Scene.SwitchTo(scene);

				GameObject obj = new GameObject("SwitchObject");
				var switchComponent = new ShutdownSwitchToSceneComponent { Target = scene2 };
				obj.AddComponent(switchComponent);
				scene.AddObject(obj);

				Scene.SwitchTo(null);
				DualityApp.Update();
				Assert.False(switchComponent.Switched);
				Assert.AreEqual(Scene.Current, scene2);
			}

			// Clean up
			scene.Dispose();
			scene2.Dispose();
		}
		[Test] public void SwitchReloadDeferredDispose()
		{
			// Inject some pseudo loading code for our Scene.
			const string TestSceneName = "TestScene";
			EventHandler<ResourceResolveEventArgs> resolveHandler = delegate(object sender, ResourceResolveEventArgs e)
			{
				if (e.RequestedContent == TestSceneName)
				{
					e.Resolve(new Scene());
				}
			};
			ContentProvider.ResourceResolve += resolveHandler;

			// Retrieve the Scene and make sure it's always the same
			Scene scene = ContentProvider.RequestContent<Scene>(TestSceneName).Res;
			Assert.IsNotNull(scene);
			{
				Scene scene2 = ContentProvider.RequestContent<Scene>(TestSceneName).Res;
				Assert.AreSame(scene, scene2);
			}
			
			// Switch to the Scene regularly
			{
				Scene.SwitchTo(scene);
				Assert.AreSame(Scene.Current, scene);
			}

			// Reload the Scene during update while using deferred disposal
			{
				GameObject obj = new GameObject("SwitchObject");
				obj.AddComponent<UpdateReloadSceneComponent>();
				scene.AddObject(obj);

				Assert.AreSame(Scene.Current, scene);
				DualityApp.Update();
				Assert.AreNotSame(Scene.Current, scene);
				Assert.AreEqual(TestSceneName, Scene.Current.Path);
			}

			// Clean up
			ContentProvider.ResourceResolve -= resolveHandler;
			scene.Dispose();
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
			Assert.AreEqual(scene, obj.Scene);
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);

			// Removing root object
			scene.RemoveObject(obj);
			Assert.AreEqual(null, obj.Scene);
			CollectionAssert.IsEmpty(scene.AllObjects);

			// Adding removed root object again
			scene.AddObject(obj);
			Assert.AreEqual(scene, obj.Scene);
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);

			// Remove non-root object
			scene.RemoveObject(objChildA);
			CollectionAssert.AreEquivalent(new[] { obj, objChildB }, scene.AllObjects);
			CollectionAssert.DoesNotContain(obj.Children, objChildA);
			Assert.IsNull(objChildA.Scene);
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
			Assert.IsNull(objChildA.Scene);
			Assert.IsNull(objChildA.Parent);

			// Add object implicitly by parent-child relationship
			objChildA.Parent = obj;
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.Contains(obj.Children, objChildA);
			Assert.AreEqual(scene, objChildA.Scene);
			Assert.AreEqual(obj, objChildA.Parent);

			// Change object parent
			objChildA.Parent = objChildB;
			CollectionAssert.AreEquivalent(new[] { obj, objChildA, objChildB }, scene.AllObjects);
			CollectionAssert.Contains(objChildB.Children, objChildA);
			Assert.AreEqual(scene, objChildA.Scene);
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
			Assert.AreEqual(scene2, objChildA.Scene);
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
		[Test] public void GameObjectParentLoop()
		{
			// Set up some objects
			Scene scene = new Scene();
			GameObject obj = new GameObject("TestObject");
			GameObject objChild = new GameObject("TestObjectChild", obj);
			GameObject objChildChild = new GameObject("TestObjectChildChild", objChild);
			scene.AddObject(obj);

			// Attempt to create a closed parent loop. Expect the operation to be rejected.
			Assert.Throws<ArgumentException>(() => obj.Parent = objChildChild);
			Assert.AreEqual(null, obj.Parent);
			Assert.AreEqual(obj, objChild.Parent);
			Assert.AreEqual(objChild, objChildChild.Parent);

			// Attempt to create a direct self-reference loop. Expect the operation to be rejected.
			Assert.Throws<ArgumentException>(() => obj.Parent = obj);
			Assert.AreEqual(null, obj.Parent);
			Assert.AreEqual(obj, objChild.Parent);
			Assert.AreEqual(objChild, objChildChild.Parent);

			// Clean up
			scene.Dispose();
		}

		/// <summary>
		/// This test will determine whether a <see cref="Scene"/> can be activated, updated and deactivated
		/// in isolation from the global <see cref="Scene.Current"/> setting.
		/// </summary>
		[Test] public void IsolatedSimulation()
		{
			// Create an isolated new test scene with a ball and a platform
			Scene scene = new Scene();

			GameObject ball = new GameObject("Ball");
			Transform ballTransform = ball.AddComponent<Transform>();
			RigidBody ballBody = ball.AddComponent<RigidBody>();
			ballBody.AddShape(new CircleShapeInfo(32.0f, new Vector2(0.0f, 0.0f), 1.0f));
			ball.AddComponent<RigidBodyRenderer>();
			scene.AddObject(ball);

			GameObject platform = new GameObject("Platform");
			Transform platformTransform = platform.AddComponent<Transform>();
			platformTransform.Pos = new Vector3(0.0f, 128.0f, 0.0f);
			RigidBody platformBody = platform.AddComponent<RigidBody>();
			platformBody.AddShape(new ChainShapeInfo(new[] { new Vector2(-128.0f, 0.0f), new Vector2(128.0f, 0.0f) }));
			platformBody.BodyType = BodyType.Static;
			platform.AddComponent<RigidBodyRenderer>();
			scene.AddObject(platform);

			// Do a single, fixed-step Duality update in order to set up
			// Time.DeltaTime in a predictable way.
			DualityApp.Update(true);

			// Assert that the isolated scene remains unaffected
			Assert.AreEqual(new Vector3(0.0f, 0.0f, 0.0f), ballTransform.Pos);
			Assert.AreEqual(new Vector3(0.0f, 128.0f, 0.0f), platformTransform.Pos);

			// Activate the scene to prepare for simulation
			scene.Activate();

			// Run the simulation for a few fixed-step frames
			for (int i = 0; i < 100; i++)
			{
				scene.Update();
			}

			// Deactivate the scene again
			scene.Deactivate();

			// Assert that the balls position is within expected values
			Assert.IsTrue(ballTransform.Pos.Y > 96.0f);
			Assert.IsTrue(ballTransform.Pos.Y < 128.0f);
			Assert.IsTrue(Math.Abs(ballTransform.Pos.X) < 1.00f);
			Assert.IsTrue(Math.Abs(ballTransform.Pos.Z) < 1.00f);
		}
		/// <summary>
		/// This test will determine whether a <see cref="Scene"/> can be rendered
		/// in isolation from the global <see cref="Scene.Current"/> setting.
		/// </summary>
		[Test, Category("Rendering")]
		public void IsolatedRendering()
		{
			// Create an isolated new test scene with a ball and a platform
			Scene scene = new Scene();

			GameObject ball = new GameObject("Ball");
			Transform ballTransform = ball.AddComponent<Transform>();
			SpriteRenderer ballRenderer = ball.AddComponent<SpriteRenderer>();
			scene.AddObject(ball);

			GameObject cameraObj = new GameObject("Camera");
			Transform cameraTransform = cameraObj.AddComponent<Transform>();
			cameraTransform.Pos = new Vector3(0.0f, 0.0f, -500.0f);
			Camera camera = cameraObj.AddComponent<Camera>();
			camera.ClearColor = new ColorRgba(128, 192, 255);
			scene.AddObject(cameraObj);

			// Activate the scene to prepare for simulation
			scene.Activate();

			// Render the scene to an image target
			PixelData renderedImage = null;
			using (Texture texture = new Texture(800, 600, TextureSizeMode.NonPowerOfTwo, TextureMagFilter.Nearest, TextureMinFilter.Nearest))
			using (RenderTarget renderTarget = new RenderTarget(AAQuality.Off, true, texture))
			{
				scene.Render(renderTarget, new Rect(renderTarget.Size), renderTarget.Size);
				renderedImage = texture.GetPixelData();
			}

			// Deactivate the scene again
			scene.Deactivate();

			// Assert that the image target has the correct background color
			// and deviates from it at the balls position
			Vector2 ballPosInOutputImage = camera.GetScreenPos(ballTransform.Pos);
			Assert.AreEqual(camera.ClearColor, renderedImage[0, 0]);
			Assert.AreNotEqual(camera.ClearColor, renderedImage[(int)ballPosInOutputImage.X, (int)ballPosInOutputImage.Y]);
		}
		/// <summary>
		/// This test will determine whether a <see cref="Scene"/> that was activated in isolation / without
		/// being <see cref="Scene.Current"/> will ensure the initialization and shutdown of objects added
		/// or removed during its lifetime.
		/// </summary>
		[Test] public void IsolatedInitAndShutdown()
		{
			// Create an isolated new test scene
			Scene scene = new Scene();

			GameObject testObj = new GameObject("TestObject");
			InitializableEventReceiver testReceiver = testObj.AddComponent<InitializableEventReceiver>();
			scene.AddObject(testObj);

			// Ensure events are delivered in scene activation
			testReceiver.Reset();
			scene.Activate();
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by Scene activation");

			// Ensure events are delivered in scene deactivation
			testReceiver.Reset();
			scene.Deactivate();
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by Scene deactivation");

			// Ensure no events are delivered for removal while deactivated
			testReceiver.Reset();
			scene.RemoveObject(testObj);
			scene.Activate();
			Assert.AreEqual(InitializableEventReceiver.EventFlag.None, testReceiver.ReceivedEvents, "Removal from Scene, activation of Scene");

			// Ensure events are delivered post-activation
			testReceiver.Reset();
			scene.AddObject(testObj);
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by addition to active Scene");

			testReceiver.Reset();
			testObj.RemoveComponent(testReceiver);
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Removal from object in active Scene");
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.RemovingFromGameObject), "Removal from object in active Scene");

			testReceiver.Reset();
			testObj.AddComponent(testReceiver);
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Addition to object in active Scene");
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.AddToGameObject), "Addition to object in active Scene");

			testReceiver.Reset();
			testObj.Active = false;
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by GameObject.Active property in active Scene");

			testReceiver.Reset();
			testObj.Active = true;
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by GameObject.Active property in active Scene");

			testReceiver.Reset();
			testReceiver.Active = false;
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by Component.Active property in active Scene");

			testReceiver.Reset();
			testReceiver.Active = true;
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by Component.Active property in active Scene");

			testReceiver.Reset();
			scene.RemoveObject(testObj);
			Assert.IsTrue(testReceiver.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by removal from active Scene");

			// Ensure no de/activation events are delivered in an inactive scene
			scene.Deactivate();
			testReceiver.Reset();
			scene.AddObject(testObj);
			testObj.Active = false;
			testObj.Active = true;
			testReceiver.Active = false;
			testReceiver.Active = true;
			scene.RemoveObject(testObj);
			Assert.AreEqual(InitializableEventReceiver.EventFlag.None, testReceiver.ReceivedEvents, "No de/activation events in inactive Scene");
		}

		private class UpdateSwitchToSceneComponent : Component, ICmpUpdatable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpUpdatable.OnUpdate()
			{
				Scene last = Scene.Current;
				Scene.SwitchTo(this.Target);
				this.Switched = (last != Scene.Current && Scene.Current == this.Target);
			}
		}
		private class UpdateReloadSceneComponent : Component, ICmpUpdatable
		{
			void ICmpUpdatable.OnUpdate()
			{
				Scene.Reload();
			}
		}
		private class InitSwitchToSceneComponent : Component, ICmpInitializable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpInitializable.OnActivate()
			{
				Scene last = Scene.Current;
				Scene.SwitchTo(this.Target);
				this.Switched = (last != Scene.Current && Scene.Current == this.Target);
			}
			void ICmpInitializable.OnDeactivate() {}
		}
		private class ShutdownSwitchToSceneComponent : Component, ICmpInitializable
		{
			public ContentRef<Scene> Target { get; set; }
			public bool Switched { get; private set; }
			void ICmpInitializable.OnActivate() {}
			void ICmpInitializable.OnDeactivate()
			{
				Scene last = Scene.Current;
				Scene.SwitchTo(this.Target);
				this.Switched = (last != Scene.Current && Scene.Current == this.Target);
			}
		}
	}
}
