using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Duality;
using Duality.Serialization;
using Duality.Backend;

using NUnit.Framework;


namespace Duality.Tests
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityAttribute : Attribute, ITestAction
	{
		private	string				oldEnvDir			= null;
		private	CorePlugin			unitTestPlugin		= null;
		private	INativeWindow		dummyWindow			= null;
		private	ConsoleLogOutput	consoleLogOutput	= null;

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		public InitDualityAttribute() {}
		public void BeforeTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality environment setup -----");

			// Set environment directory to Duality binary directory
			this.oldEnvDir = Environment.CurrentDirectory;
			string codeBaseURI = typeof(DualityApp).Assembly.CodeBase;
			string codeBasePath = codeBaseURI.StartsWith("file:") ? codeBaseURI.Remove(0, "file:".Length) : codeBaseURI;
			codeBasePath = codeBasePath.TrimStart('/');
			Environment.CurrentDirectory = Path.GetDirectoryName(codeBasePath);

			// Add some Console logs manually for NUnit
			if (!Log.Game.Outputs.OfType<ConsoleLogOutput>().Any())
			{
				if (this.consoleLogOutput == null) this.consoleLogOutput = new ConsoleLogOutput();
				Log.Game.AddOutput(this.consoleLogOutput);
				Log.Core.AddOutput(this.consoleLogOutput);
				Log.Editor.AddOutput(this.consoleLogOutput);
			}

			// Initialize Duality
			DualityApp.Init(DualityApp.ExecutionEnvironment.Launcher, DualityApp.ExecutionContext.Game);

			// Manually register pseudo-plugin for the Unit Testing Assembly
			this.unitTestPlugin = DualityApp.LoadPlugin(typeof(DualityTestsPlugin).Assembly, codeBasePath);

			// Create a dummy window, to get access to all the device contexts
			if (this.dummyWindow == null)
			{
				WindowOptions options = new WindowOptions
				{
					Width = 800,
					Height = 600
				};
				this.dummyWindow = DualityApp.OpenWindow(options);
			}

			// Load local testing memory
			TestHelper.LocalTestMemory = Serializer.TryReadObject<TestMemory>(TestHelper.LocalTestMemoryFilePath, SerializeMethod.Xml);

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");
			
			// Remove NUnit Console logs
			Log.Game.RemoveOutput(this.consoleLogOutput);
			Log.Core.RemoveOutput(this.consoleLogOutput);
			Log.Editor.RemoveOutput(this.consoleLogOutput);
			this.consoleLogOutput = null;

			if (this.dummyWindow != null)
			{
				ContentProvider.ClearContent();
				ContentProvider.DisposeDefaultContent();
			    this.dummyWindow.Dispose();
			    this.dummyWindow = null;
			}
			DualityApp.Terminate();
			Environment.CurrentDirectory = this.oldEnvDir;

			// Save local testing memory
			if (TestContext.CurrentContext.Result.Status == TestStatus.Passed && !System.Diagnostics.Debugger.IsAttached)
			{
				Serializer.WriteObject(TestHelper.LocalTestMemory, TestHelper.LocalTestMemoryFilePath, SerializeMethod.Xml);
			}

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
