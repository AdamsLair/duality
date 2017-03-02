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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder());

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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder());

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
			this.AssertEventOrder(eventLog, 5 * 10, new ComponentExecutionOrder());
		}

		private void AssertEventOrder(EventOrderLog eventLog, int eventCount, ComponentExecutionOrder order)
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

				Assert.GreaterOrEqual(
					index, 
					lastIndex, 
					string.Format("Found {0} after {1}, which is out of order.", type.Name, lastType.Name));

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
		[Flags]
		public enum EventType
		{
			None = 0x0,

			Update = 0x1,
			Activate = 0x2,
			Deactivate = 0x4,

			All = Update | Activate | Deactivate
		}
		public class EventOrderLog
		{
			private EventType eventFilter = EventType.All;
			private List<TestComponent> eventOrder = new List<TestComponent>();

			public EventType EventFilter
			{
				get { return this.eventFilter; }
				set { this.eventFilter = value; }
			}
			public IEnumerable<TestComponent> EventOrder
			{
				get { return this.eventOrder; }
			}

			public void Clear()
			{
				this.eventOrder.Clear();
			}
			public void Notify(EventType eventType, TestComponent receiver)
			{
				if ((this.eventFilter & eventFilter) == EventType.None) return;
				this.eventOrder.Add(receiver);
			}
		}
		public class TestComponent : Component, ICmpUpdatable, ICmpInitializable
		{
			private EventOrderLog eventLog;

			public EventOrderLog EventLog
			{
				get { return this.eventLog; }
				set { this.eventLog = value; }
			}

			private void NotifyEvent(EventType type)
			{
				if (this.eventLog == null) return;
				this.eventLog.Notify(type, this);
			}

			void ICmpUpdatable.OnUpdate()
			{
				this.NotifyEvent(EventType.Update);
			}
			void ICmpInitializable.OnInit(Component.InitContext context)
			{
				if (context == InitContext.Activate)
					this.NotifyEvent(EventType.Activate);
			}
			void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
			{
				if (context == ShutdownContext.Deactivate)
					this.NotifyEvent(EventType.Deactivate);
			}
		}

		public class TestComponentA1 : TestComponent { }
		public class TestComponentA2 : TestComponent { }
		public class TestComponentA3 : TestComponent { }
		public class TestComponentA4 : TestComponent { }
		public class TestComponentA5 : TestComponent { }
	}
}
