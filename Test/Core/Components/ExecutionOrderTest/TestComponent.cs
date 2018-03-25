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
	public class TestComponent : Component, ICmpUpdatable, ICmpInitializable, ICmpSerializeListener
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
		void ICmpInitializable.OnActivate()
		{
			this.NotifyEvent(EventType.Activate);
		}
		void ICmpInitializable.OnDeactivate()
		{
			this.NotifyEvent(EventType.Deactivate);
		}
		void ICmpSerializeListener.OnLoaded()
		{
			this.NotifyEvent(EventType.Loaded);
		}
		void ICmpSerializeListener.OnSaved()
		{
			this.NotifyEvent(EventType.Saved);
		}
		void ICmpSerializeListener.OnSaving()
		{
			this.NotifyEvent(EventType.Saving);
		}
	}
}
