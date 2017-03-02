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


namespace Duality.Tests.Components.ExecutionOrderTest
{
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
}
