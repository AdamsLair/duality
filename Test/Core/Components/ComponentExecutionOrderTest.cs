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

		[Test] public void EnforceOrderGameObjectActivate([Values(true, false)] bool deepObjectTree)
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Activate;

			int objectCount = deepObjectTree ? 10 : 1;
			GameObject root = this.GenerateSampleTree(objectCount, true,
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
			this.AssertEventOrder(eventLog, 5 * objectCount, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectDeactivate([Values(true, false)] bool deepObjectTree)
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			int objectCount = deepObjectTree ? 10 : 1;
			GameObject root = this.GenerateSampleTree(objectCount, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			root.Active = false;
			this.AssertEventOrder(eventLog, 5 * objectCount, Component.ExecOrder, true);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectAdd([Values(true, false)] bool deepObjectTree)
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Activate;

			int objectCount = deepObjectTree ? 10 : 1;
			GameObject root = this.GenerateSampleTree(objectCount, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);

			this.AssignEventLog(root, eventLog, true);
			Scene.Current.AddObject(root);
			this.AssertEventOrder(eventLog, 5 * objectCount, Component.ExecOrder, false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectRemove([Values(true, false)] bool deepObjectTree)
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			int objectCount = deepObjectTree ? 10 : 1;
			GameObject root = this.GenerateSampleTree(objectCount, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			Scene.Current.RemoveObject(root);
			this.AssertEventOrder(eventLog, 5 * objectCount, Component.ExecOrder, true);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderGameObjectDispose([Values(true, false)] bool deepObjectTree)
		{
			EventOrderLog eventLog = new EventOrderLog();
			eventLog.EventFilter = EventType.Deactivate;

			int objectCount = deepObjectTree ? 10 : 1;
			GameObject root = this.GenerateSampleTree(objectCount, true,
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));

			Scene.SwitchTo(new Scene(), true);
			Scene.Current.AddObject(root);

			this.AssignEventLog(root, eventLog, true);
			root.Dispose();
			DualityApp.RunCleanup();
			this.AssertEventOrder(eventLog, 5 * objectCount, Component.ExecOrder, true);

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

		[Test] public void ExecOrderAttributeBefore()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentB1));
			types.Add(typeof(TestComponentB2));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentB2), typeof(TestComponentB1) }, 
				types);
		}
		[Test] public void ExecOrderAttributeBeforeAndAfter()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentC1));
			types.Add(typeof(TestComponentC2));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentC2), typeof(TestComponentC1) }, 
				types);
		}
		[Test] public void ExecOrderAttributeAfter()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentD1));
			types.Add(typeof(TestComponentD2));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentD2), typeof(TestComponentD1) }, 
				types);
		}
		[Test] public void ExecOrderLoopResolve()
		{
			// If there is a loop in the execution order requirements, make sure
			// a log informs the user about this.
			TestingLogOutput logWatcher = new TestingLogOutput();
			Logs.AddGlobalOutput(logWatcher);

			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentE1));
			types.Add(typeof(TestComponentE2));
			types.Add(typeof(TestComponentE3));

			// Assert that we're still able to perform exec order sorting and
			// the constraint loop doesn't cause further problems.
			order.SortTypes(types, false);
			
			// Assert that there were no errors, but we got a warning.
			logWatcher.AssertNoErrors();
			logWatcher.AssertWarning();

			Logs.RemoveGlobalOutput(logWatcher);
		}
		[Test] public void ExecOrderBaseTypeRule()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentH1));
			types.Add(typeof(TestComponentI1));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentI1), typeof(TestComponentH1) }, 
				types);
		}
		[Test] public void ExecOrderSpecificOverride()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentH2));
			types.Add(typeof(TestComponentI2));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentH2), typeof(TestComponentI2) }, 
				types);
		}
		[Test] public void RequirementAttribute()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentF1));
			types.Add(typeof(TestComponentF2));

			order.SortTypes(types, false);

			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentF2), typeof(TestComponentF1) }, 
				types);
		}
		[Test] public void RequirementLoopResolve()
		{
			ComponentExecutionOrder order = new ComponentExecutionOrder();

			List<Type> types = new List<Type>();
			types.Add(typeof(TestComponentG1));
			types.Add(typeof(TestComponentG2));

			order.SortTypes(types, false);

			// Even though G1 requires G2 (implicit after), there is an explicit
			// G1 before G2 attribute. We'll expect that to have priority in resolving
			// this loop.
			CollectionAssert.AreEqual(
				new[] { typeof(TestComponentG1), typeof(TestComponentG2) }, 
				types);
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
			Random rnd = new Random(1);
			FieldInfo field = typeof(GameObject).GetField("compList", BindingFlags.NonPublic | BindingFlags.Instance);
			List<Component> list = (List<Component>)field.GetValue(obj);
			rnd.Shuffle(list);
			Component[] shuffled = list.ToArray();
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
				rnd.Shuffle(componentTypes);
				foreach (Type type in componentTypes)
					root.AddComponent(type);
			}

			GameObject[] obj = new GameObject[objectCount];
			for (int i = 0; i < obj.Length; i++)
			{
				obj[i] = new GameObject(string.Format("Object #{0}", i));
				rnd.Shuffle(componentTypes);
				foreach (Type type in componentTypes)
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
				rnd.Shuffle(componentTypes);
				foreach (Type type in componentTypes)
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

		public class TestComponentB1 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentB1))]
		public class TestComponentB2 : TestComponent { }

		[ExecutionOrder(ExecutionRelation.After, typeof(TestComponentC2))]
		public class TestComponentC1 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentC1))]
		public class TestComponentC2 : TestComponent { }

		[ExecutionOrder(ExecutionRelation.After, typeof(TestComponentD2))]
		public class TestComponentD1 : TestComponent { }
		public class TestComponentD2 : TestComponent { }

		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentE1))]
		public class TestComponentE1 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentE3))]
		public class TestComponentE2 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentE2))]
		public class TestComponentE3 : TestComponent { }

		[RequiredComponent(typeof(TestComponentF2))]
		public class TestComponentF1 : TestComponent { }
		public class TestComponentF2 : TestComponent { }

		[RequiredComponent(typeof(TestComponentG2))]
		public class TestComponentG1 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.After, typeof(TestComponentG1))]
		public class TestComponentG2 : TestComponent { }

		public interface ITestComponentH { }
		public class TestComponentH1 : TestComponent, ITestComponentH { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(TestComponentI2))]
		public class TestComponentH2 : TestComponent, ITestComponentH { }

		[ExecutionOrder(ExecutionRelation.Before, typeof(ITestComponentH))]
		public class TestComponentI1 : TestComponent { }
		[ExecutionOrder(ExecutionRelation.Before, typeof(ITestComponentH))]
		public class TestComponentI2 : TestComponent { }
	}
}
