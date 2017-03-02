using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
			Assert.Inconclusive("Not yet implemented");

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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder(), false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderSceneActivate()
		{
			Assert.Inconclusive("Not yet implemented");

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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder(), false);

			Scene.SwitchTo(null, true);
		}
		[Test] public void EnforceOrderSceneDeactivate()
		{
			Assert.Inconclusive("Not yet implemented");

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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder(), true);
		}

		private void AssertEventOrder(EventOrderLog eventLog, int eventCount, ComponentExecutionOrder order, bool reverseOrder)
		{
			int actualEventCount = eventLog.EventOrder.Count();
			Assert.AreEqual(
				eventCount, 
				actualEventCount, 
				string.Format("Expected {0} events of type/s {2}, but got only {1}", eventCount, actualEventCount, eventLog.EventFilter));

			int lastIndex = int.MinValue;
			Type lastType = null;
			foreach (TestComponent component in eventLog.EventOrder)
			{
				Type type = component.GetType();
				int index = order.GetSortIndex(type);

				if (reverseOrder)
				{
					Assert.LessOrEqual(
						index, 
						lastIndex, 
						string.Format("Found {0} before {1}, which is out of the expected (reversed) order.", type.Name, lastType.Name));
				}
				else
				{
					Assert.GreaterOrEqual(
						index, 
						lastIndex, 
						string.Format("Found {0} after {1}, which is out of order.", type.Name, lastType.Name));
				}

				lastIndex = index;
				lastType = type;
			}
		}
		private void AssignEventLog(Scene scene, EventOrderLog eventLog)
		{
			foreach (TestComponent component in scene.FindComponents<TestComponent>())
			{
				component.EventLog = eventLog;
			}
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
