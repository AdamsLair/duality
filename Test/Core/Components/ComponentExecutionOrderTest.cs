using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using Duality;
using Duality.Resources;
using Duality.Components;
using Duality.Serialization;

using Duality.Tests.Components;
using Duality.Tests.Components.ExecutionOrderTest;

using NUnit.Framework;


namespace Duality.Tests.Components
{
	[TestFixture]
	public class ComponentExecutionOrderTest
	{
		[Test] public void BasicFunctionality()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			int sortIndexA1 = order.GetSortIndex(typeof(TestComponentA1));
			int sortIndexA2 = order.GetSortIndex(typeof(TestComponentA2));

			// Expect no particular order, but consistent results
			Assert.AreEqual(sortIndexA1, order.GetSortIndex(typeof(TestComponentA1)));
			Assert.AreEqual(sortIndexA2, order.GetSortIndex(typeof(TestComponentA2)));

			// Expect that clearing the type cache does not change any results
			order.ClearTypeCache();
			Assert.AreEqual(sortIndexA1, order.GetSortIndex(typeof(TestComponentA1)));
			Assert.AreEqual(sortIndexA2, order.GetSortIndex(typeof(TestComponentA2)));
		}

		[Test] public void EnforceOrderSceneUpdate()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Update;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(scene, true);

			this.AssignEventLog(scene, eventLog);
			DualityApp.Update();
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderSceneFind()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Update;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Since scene iteration is the basis for delivering broadcasted messages, we'll
			// expect all related API to behave consistenly with regard to component execution order.
			this.AssertComponentOrder(scene.FindComponents<TestComponent>().ToArray(), 5 * 10, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderSceneActivate()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Activate;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			this.AssignEventLog(scene, eventLog);
			Scene.SwitchTo(scene, true);
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderSceneDeactivate()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(scene, true);

			this.AssignEventLog(scene, eventLog);
			Scene.SwitchTo(null, true);
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, true);
		}
		[Test] public void EnforceOrderSceneLoad()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Loaded;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Assign the event log so it gets serialized and it already
			// there when the scene is initialized after loading
			this.AssignEventLog(scene, eventLog);

			// Save the scene and reload it so the OnLoaded code is triggered
			using (MemoryStream data = new MemoryStream())
			{
				scene.Save(data);
				data.Position = 0;
				scene = Resource.Load<Scene>(data);
			}

			// Retrieve the deserialized event log again and evaluate results
			eventLog = scene.FindComponent<TestComponent>().EventLog;
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderSceneSaved()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Saved;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			this.AssignEventLog(scene, eventLog);
			using (MemoryStream data = new MemoryStream())
			{
				scene.Save(data);
			}
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderSceneSaving()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Saving;

			Scene scene = this.GenerateSampleScene(10,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			this.AssignEventLog(scene, eventLog);
			using (MemoryStream data = new MemoryStream())
			{
				scene.Save(data);
			}
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, true);
		}

		[Test] public void EnforceOrderPrefabLoad()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Loaded;

			GameObject root = this.GenerateSampleTree(10, false,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Assign the event log so it gets serialized and it already
			// there when the scene is initialized after loading
			this.AssignEventLog(root, eventLog, false);

			// Save the scene and reload it so the OnLoaded code is triggered
			using (MemoryStream data = new MemoryStream())
			{
				Prefab prefab = new Prefab(root);
				prefab.Save(data);
				data.Position = 0;
				prefab = Resource.Load<Prefab>(data);

				// Note that due to lack of API to directly access the prefab object tree,
				// we're creating a clone by instantiation here - however, since all the
				// event logs are cloned as well, this won't invalidate the test results.
				root = prefab.Instantiate();
			}

			// Retrieve the deserialized event log again and evaluate results
			eventLog = root.GetComponentsInChildren<TestComponent>().FirstOrDefault().EventLog;
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderPrefabSaving()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Saving;

			GameObject root = this.GenerateSampleTree(10, false,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Assign the event log so it gets serialized and it already
			// there when the scene is initialized after loading
			this.AssignEventLog(root, eventLog, false);

			// Injecting an object into a prefab will invoke its Saving and Saved events
			Prefab prefab = new Prefab();
			prefab.Inject(root);

			// Evaluate results
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, true);
		}
		[Test] public void EnforceOrderPrefabSaved()
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Saved;

			GameObject root = this.GenerateSampleTree(10, false,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Assign the event log so it gets serialized and it already
			// there when the scene is initialized after loading
			this.AssignEventLog(root, eventLog, false);

			// Injecting an object into a prefab will invoke its Saving and Saved events
			Prefab prefab = new Prefab();
			prefab.Inject(root);

			// Evaluate results
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);
		}

		[Test] public void EnforceOrderGameObjectActivate()
		{
			Assert.Inconclusive("Not yet implemented");

			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Activate;

			GameObject root = this.GenerateSampleTree(10, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));
			root.Active = false;

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			root.Active = true;
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectDeactivate()
		{
			Assert.Inconclusive("Not yet implemented");

			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			GameObject root = this.GenerateSampleTree(10, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			root.Active = false;
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, true);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectAdd()
		{
			Assert.Inconclusive("Not yet implemented");

			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Activate;

			GameObject root = this.GenerateSampleTree(10, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);

			this.AssignEventLog(root, eventLog, true);
			Scene.Current.AddObject(root);
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectRemove()
		{
			Assert.Inconclusive("Not yet implemented");

			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			GameObject root = this.GenerateSampleTree(10, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			Scene.Current.RemoveObject(root);
			this.AssertEventOrder(eventLog, 5 * 10, Component.ExecOrder, true);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectIterateNew()
		{
			// We'll actually generate a flat tree with only the root element here
			GameObject root = this.GenerateSampleTree(1, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));
			
			// Check iteration order
			List<TestComponent> iterateOrder = new List<TestComponent>();
			root.IterateComponents<TestComponent>(component => 
			{
				iterateOrder.Add(component);
			});
			this.AssertComponentOrder(iterateOrder, 5, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderGameObjectIterateLoadedScene()
		{
			// We'll actually generate a flat tree with only the root element here
			GameObject root = this.GenerateSampleTree(1, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Shuffle component order in the GameObject so we can see if it's fixed again
			// after loading. This will ensure changes in exec order will be applied properly.
			this.ShuffleComponentOrder(root);
			
			// Save and reload the object in a scene, make sure iteration order remains consistent
			using (MemoryStream data = new MemoryStream())
			{
				Scene scene = new Scene();
				scene.AddObject(root);
				scene.Save(data);
				data.Position = 0;
				scene = Resource.Load<Scene>(data);
				root = scene.RootObjects.FirstOrDefault();
			}
			
			// Check iteration order
			List<TestComponent> iterateOrder = new List<TestComponent>();
			root.IterateComponents<TestComponent>(component => 
			{
				iterateOrder.Add(component);
			});
			this.AssertComponentOrder(iterateOrder, 5, Component.ExecOrder, false);
		}
		[Test] public void EnforceOrderGameObjectIterateLoadedPrefab()
		{
			// We'll actually generate a flat tree with only the root element here
			GameObject root = this.GenerateSampleTree(1, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			// Shuffle component order in the GameObject so we can see if it's fixed again
			// after loading. This will ensure changes in exec order will be applied properly.
			this.ShuffleComponentOrder(root);
			
			// Save and reload the object in a Prefab, make sure iteration order remains consistent
			using (MemoryStream data = new MemoryStream())
			{
				Prefab prefab = new Prefab();
				prefab.Inject(root);
				prefab.Save(data);
				data.Position = 0;
				prefab = Resource.Load<Prefab>(data);
				root = prefab.Instantiate();
			}
			
			// Check iteration order
			List<TestComponent> iterateOrder = new List<TestComponent>();
			root.IterateComponents<TestComponent>(component => 
			{
				iterateOrder.Add(component);
			});
			this.AssertComponentOrder(iterateOrder, 5, Component.ExecOrder, false);
		}


		private void AssertEventOrder(EventOrderLog eventLog, int eventCount, ComponentExecutionOrder order, bool reverseOrder)
		{
			int actualEventCount = eventLog.EventOrder.Count();
			Assert.AreEqual(
				eventCount, 
				actualEventCount, 
				string.Format("Expected {0} events of type/s {2}, but got {1}", eventCount, actualEventCount, eventLog.EventFilter));

			this.AssertComponentOrder(eventLog.EventOrder, eventCount, order, reverseOrder);
		}
		private void AssertComponentOrder(IEnumerable<Component> components, int count, ComponentExecutionOrder order, bool reverseOrder)
		{
			int actualCount = components.Count();
			Assert.AreEqual(
				count, 
				actualCount, 
				string.Format("Expected {0} Components, but got {1}", count, actualCount));

			int lastIndex = reverseOrder ? int.MaxValue : int.MinValue;
			Type lastType = null;
			foreach (TestComponent component in components)
			{
				Type type = component.GetType();
				int index = order.GetSortIndex(type);

				if (reverseOrder)
				{
					Assert.LessOrEqual(
						index, 
						lastIndex, 
						string.Format(
							"Found {0} before {1}, which is out of the expected (reversed) order.", 
							type.Name, 
							lastType != null ? lastType.Name : "null"));
				}
				else
				{
					Assert.GreaterOrEqual(
						index, 
						lastIndex, 
						string.Format(
							"Found {0} after {1}, which is out of order.", 
							type.Name, 
							lastType != null ? lastType.Name : "null"));
				}

				lastIndex = index;
				lastType = type;
			}
		}

		private void AssignEventLog(GameObject root, EventOrderLog eventLog, bool includeRoot)
		{
			IEnumerable<TestComponent> components;
			if (includeRoot)
				components = root.GetComponentsDeep<TestComponent>();
			else
				components = root.GetComponentsInChildren<TestComponent>();

			foreach (TestComponent component in components)
				component.EventLog = eventLog;
		}
		private void AssignEventLog(Scene scene, EventOrderLog eventLog)
		{
			foreach (TestComponent component in scene.FindComponents<TestComponent>())
			{
				component.EventLog = eventLog;
			}
		}

		private void ShuffleComponentOrder(GameObject obj)
		{
			FieldInfo field = typeof(GameObject).GetField("compList", BindingFlags.NonPublic | BindingFlags.Instance);
			List<Component> list = (List<Component>)field.GetValue(obj);
			Component[] shuffled = list.Shuffle(new Random(1)).ToArray();
			list.Clear();
			list.AddRange(shuffled);
		}
		private GameObject GenerateSampleTree(int objectCount, bool includeRoot, params Type[] componentTypes)
		{
			Random rnd = new Random(1);

			GameObject root = new GameObject("Root");
			if (includeRoot)
			{
				objectCount--;
				foreach (Type type in componentTypes.Shuffle(rnd))
					root.AddComponent(type);
			}

			GameObject[] obj = new GameObject[objectCount];
			for (int i = 0; i < obj.Length; i++)
			{
				obj[i] = new GameObject(string.Format("Object #{0}", i));
				foreach (Type type in componentTypes.Shuffle(rnd))
					obj[i].AddComponent(type);

				if ((i % 2) == 0)
					obj[i].Parent = root;
				else
					obj[i].Parent = obj[i - 1];
			}
			return root;
		}
		private Scene GenerateSampleScene(int objectCount, params Type[] componentTypes)
		{
			Random rnd = new Random(1);
			Scene scene = new Scene();
			GameObject[] obj = new GameObject[objectCount];
			for (int i = 0; i < obj.Length; i++)
			{
				obj[i] = new GameObject(string.Format("Object #{0}", i));
				foreach (Type type in componentTypes.Shuffle(rnd))
				{
					obj[i].AddComponent(type);
				}

				if ((i % 2) == 0)
					scene.AddObject(obj[i]);
				else
					obj[i].Parent = obj[i - 1];
			}
			return scene;
		}
	}

	namespace ExecutionOrderTest
	{
		public class TestComponentA1 : TestComponent { }
		public class TestComponentA2 : TestComponent { }
		public class TestComponentA3 : TestComponent { }
		public class TestComponentA4 : TestComponent { }
		public class TestComponentA5 : TestComponent { }
	}
}
