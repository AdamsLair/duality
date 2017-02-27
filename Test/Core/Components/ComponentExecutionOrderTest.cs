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

			Scene scene = this.GenerateSampleScene(
				typeof(TestComponentA1), 
				typeof(TestComponentA2), 
				typeof(TestComponentA3), 
				typeof(TestComponentA4), 
				typeof(TestComponentA5));
			this.AssignEventLog(scene, eventLog);

			Scene.SwitchTo(scene, true);
			DualityApp.Update();
			Scene.SwitchTo(null, true);

			this.AssertEventOrder(eventLog, new ComponentExecutionOrder());
		}

		private void AssertEventOrder(EventOrderLog eventLog, ComponentExecutionOrder order)
		{
			int lastIndex = int.MinValue;
			foreach (TestComponent component in eventLog.EventOrder)
			{
				Type type = component.GetType();
				int index = order.GetSortIndex(type);
				Assert.GreaterOrEqual(index, lastIndex);
				lastIndex = index;
			}
		}
		private void AssignEventLog(Scene scene, EventOrderLog eventLog)
		{
			foreach (TestComponent component in scene.FindComponents<TestComponent>())
			{
				component.EventLog = eventLog;
			}
		}
		private Scene GenerateSampleScene(params Type[] componentTypes)
		{
			Random rnd = new Random(1);
			Scene scene = new Scene();
			GameObject[] obj = new GameObject[10];
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
			All = Update
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
		public class TestComponent : Component, ICmpUpdatable
		{
			private EventOrderLog eventLog;

			public EventOrderLog EventLog
			{
				get { return this.eventLog; }
				set { this.eventLog = value; }
			}

			void ICmpUpdatable.OnUpdate()
			{
				this.eventLog.Notify(EventType.Update, this);
			}
		}

		public class TestComponentA1 : TestComponent { }
		public class TestComponentA2 : TestComponent { }
		public class TestComponentA3 : TestComponent { }
		public class TestComponentA4 : TestComponent { }
		public class TestComponentA5 : TestComponent { }
	}
}
