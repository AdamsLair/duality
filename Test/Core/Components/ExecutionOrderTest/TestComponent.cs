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
			else if (context == InitContext.Loaded)
				this.NotifyEvent(EventType.Loaded);
			else if (context == InitContext.Saved)
				this.NotifyEvent(EventType.Saved);
		}
		void ICmpInitializable.OnShutdown(Component.ShutdownContext context)
		{
			if (context == ShutdownContext.Deactivate)
				this.NotifyEvent(EventType.Deactivate);
			else if (context == ShutdownContext.Saving)
				this.NotifyEvent(EventType.Saving);
		}
	}
}
