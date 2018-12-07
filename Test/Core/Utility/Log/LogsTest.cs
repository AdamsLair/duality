using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Duality;
using Duality.Drawing;

using NUnit.Framework;

namespace Duality.Tests.Utility
{
	[TestFixture]
	public class LogsTest
	{
		private TestingLogOutput logWatcher = new TestingLogOutput();

		[SetUp] public void Init()
		{
			this.logWatcher.Reset();
			Logs.AddGlobalOutput(this.logWatcher);
		}
		[TearDown] public void Cleanup()
		{
			Logs.RemoveGlobalOutput(this.logWatcher);
		}

		[Test] public void CustomGlobalLogs()
		{
			// Retrieve a custom global log channel, write a warning
			// and expect it to come through in our log watcher that 
			// was added as a global output before.
			this.logWatcher.AssertNoWarnings();
			Logs.Get<TestLog>().WriteWarning("Test Warning");
			this.logWatcher.AssertWarning();
			this.logWatcher.AssertNoWarnings();
			Logs.Get<TestLog>().WriteWarning("Test Warning");
			this.logWatcher.AssertWarning();
		}

		private class TestLog : CustomLogInfo
		{
			public TestLog() : base("TestLog", "Test") { }
		}
	}
}
