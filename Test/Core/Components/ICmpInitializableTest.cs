﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Serialization;
using Duality.Tests.Components;

using NUnit.Framework;


namespace Duality.Tests.Resources
{
	[TestFixture]
	public class ICmpInitializableTest
	{
		private Scene scene;
		private GameObject obj;

		[SetUp] public void Setup()
		{
			this.scene = new Scene();
			this.obj = new GameObject("TestObject");
			this.scene.AddObject(this.obj);
		}
		[TearDown] public void Teardown()
		{
			this.scene.Dispose();
			this.scene		= null;
			this.obj		= null;
		}

		[Test] public void AddRemove()
		{
			InitializableEventReceiver initTester = new InitializableEventReceiver();

			// Adding and Removing
			this.obj.AddComponent(initTester);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.AddToGameObject));
			this.obj.RemoveComponent(initTester);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.RemovingFromGameObject));
		}
		[Test] public void ActivateDeactivate()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();

			// Activate by Scene.Current
			Scene.SwitchTo(this.scene, true);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by Scene.Current");
			initTester.Reset();

			// Deactivate by GameObject.Active
			this.obj.Active = false;
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by GameObject.Active");
			initTester.Reset();

			{
				// Removing inactive object shouldn't trigger events
				this.scene.RemoveObject(this.obj);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Removing inactive object shouldn't trigger events");
				initTester.Reset();

				// Adding inactive object shouldn't trigger events
				this.scene.AddObject(this.obj);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Adding inactive object shouldn't trigger events");
				initTester.Reset();

				// Removing inactive objects Component shouldn't trigger events
				this.obj.RemoveComponent(initTester);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Removing inactive objects Component shouldn't trigger events");
				initTester.Reset();

				// Adding inactive objects Component shouldn't trigger events
				this.obj.AddComponent(initTester);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Adding inactive objects Component shouldn't trigger events");
				initTester.Reset();
			}

			// Activate by GameObject.Active
			this.obj.Active = true;
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by GameObject.Active");
			initTester.Reset();

			// Deactivate by Component.Active
			initTester.Active = false;
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by Component.Active");
			initTester.Reset();
			
			{
				// Removing inactive Components object shouldn't trigger events
				this.scene.RemoveObject(this.obj);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Removing inactive Components object shouldn't trigger events");
				initTester.Reset();

				// Adding inactive Components object shouldn't trigger events
				this.scene.AddObject(this.obj);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Adding inactive Components object shouldn't trigger events");
				initTester.Reset();

				// Removing inactive Component shouldn't trigger events
				this.obj.RemoveComponent(initTester);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Removing inactive Component shouldn't trigger events");
				initTester.Reset();

				// Adding inactive Component shouldn't trigger events
				this.obj.AddComponent(initTester);
				Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Adding inactive Component shouldn't trigger events");
				initTester.Reset();
			}

			// Activate by Component.Active
			initTester.Active = true;
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by Component.Active");
			initTester.Reset();

			// Deactivate by removing from Scene
			this.scene.RemoveObject(this.obj);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by removing from Scene");
			initTester.Reset();

			// Activate by adding to Scene
			this.scene.AddObject(this.obj);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate), "Activate by adding to Scene");
			initTester.Reset();

			// Deactivate by Scene.Current
			Scene.SwitchTo(null, true);
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate), "Deactivate by Scene.Current");
			initTester.Reset();
		}
		[Test] public void DeactivateGameObjectDispose()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();
			Scene.SwitchTo(this.scene, true);
			this.obj.Dispose();
			DualityApp.RunCleanup(); // Need to run cleanup, so disposed GameObjects will be processed.
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
		}
		[Test] public void DeactivateDeepGameObjectDispose()
		{
			GameObject childObj = new GameObject("ChildObject", this.obj);
			InitializableEventReceiver initTester = childObj.AddComponent<InitializableEventReceiver>();
			Scene.SwitchTo(this.scene, true);
			this.obj.Dispose();
			DualityApp.RunCleanup(); // Need to run cleanup, so disposed GameObjects will be processed.
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
		}
		[Test] public void DeactivateComponentDispose()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();
			Scene.SwitchTo(this.scene, true);
			initTester.Dispose();
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
		}
		[Test] public void DeactivateSceneDispose()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();
			Scene.SwitchTo(this.scene, true);
			this.scene.Dispose();
			Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
		}
		[Test] public void IgnoreActiveStateInNonCurrentScene()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();

			// Deactivate by GameObject.Active
			this.obj.Active = false;
			Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
			initTester.Reset();

			// Activate by GameObject.Active
			this.obj.Active = true;
			Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate));
			initTester.Reset();

			// Deactivate by Component.Active
			initTester.Active = false;
			Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Deactivate));
			initTester.Reset();

			// Activate by Component.Active
			initTester.Active = true;
			Assert.IsFalse(initTester.HasReceived(InitializableEventReceiver.EventFlag.Activate));
			initTester.Reset();
		}
		[Test] public void Serialize()
		{
			InitializableEventReceiver initTester = this.obj.AddComponent<InitializableEventReceiver>();
			using (MemoryStream stream = new MemoryStream())
			{
				this.scene.Save(stream);
				Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Saving));
				Assert.IsTrue(initTester.HasReceived(InitializableEventReceiver.EventFlag.Saved));

				stream.Seek(0, SeekOrigin.Begin);
				this.scene = Resource.Load<Scene>(stream);
				Assert.IsTrue(this.scene.FindComponent<InitializableEventReceiver>().HasReceived(InitializableEventReceiver.EventFlag.Loaded));
			}
		}
	}
}
