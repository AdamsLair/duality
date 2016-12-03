using System;
using Duality.Tests;
using NUnit.Framework;

namespace Duality.Tests
{
	public class TestingLogOutput : ILogOutput
	{
		private string lastErrorMessage;
		private string lastWarningMessage;

		public void Reset()
		{
			this.lastErrorMessage = null;
			this.lastWarningMessage = null;
		}

		public void AssertError()
		{
			if (this.lastErrorMessage == null)
				Assert.Fail("Expected an error log, but none was logged.");
			this.lastErrorMessage = null;
		}
		public void AssertWarning()
		{
			if (this.lastWarningMessage == null)
				Assert.Fail("Expected a warning log, but none was logged.");
			this.lastWarningMessage = null;
		}
		public void AssertErrorOrWarning()
		{
			if (this.lastErrorMessage == null && this.lastWarningMessage == null)
				Assert.Fail("Expected an error or warning log, but neither was logged.");
			this.lastErrorMessage = null;
			this.lastWarningMessage = null;
		}
		public void AssertNoErrorsOrWarnings()
		{
			this.AssertNoErrors();
			this.AssertNoWarnings();
		}
		public void AssertNoErrors()
		{
			if (this.lastErrorMessage != null)
			{
				Assert.Fail(
					string.Format("Expected no error logs, but an error was logged: '{0}'",
					this.lastErrorMessage));
			}
		}
		public void AssertNoWarnings()
		{
			if (this.lastWarningMessage != null)
			{
				Assert.Fail(
					string.Format("Expected no warning logs, but a warning was logged: '{0}'",
					this.lastWarningMessage));
			}
		}

		void ILogOutput.Write(LogEntry entry, object context, Log source)
		{
			if (entry.Type == LogMessageType.Error)
				this.lastErrorMessage = entry.Message;
			else if (entry.Type == LogMessageType.Warning)
				this.lastWarningMessage = entry.Message;
		}
	}
}
