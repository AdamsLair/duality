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
		private	TextWriterLogOutput	consoleLogOutput	= null;

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
			Console.WriteLine("Testing Core Assembly: {0}", codeBasePath);
			Environment.CurrentDirectory = Path.GetDirectoryName(codeBasePath);

			// Add some Console logs manually for NUnit
			if (this.consoleLogOutput == null)
				this.consoleLogOutput = new TextWriterLogOutput(Console.Out);
			Log.AddGlobalOutput(this.consoleLogOutput);

			// Initialize Duality
			DualityApp.Init(
				DualityApp.ExecutionEnvironment.Launcher, 
				DualityApp.ExecutionContext.Game, 
				new DefaultPluginLoader(),
				null);

			// Manually register pseudo-plugin for the Unit Testing Assembly
			this.unitTestPlugin = DualityApp.PluginManager.LoadPlugin(
				typeof(DualityTestsPlugin).Assembly, 
				codeBasePath);

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
			TestHelper.LocalTestMemory = Serializer.TryReadObject<TestMemory>(TestHelper.LocalTestMemoryFilePath, typeof(XmlSerializer));

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");
			
			// Remove NUnit Console logs
			Log.RemoveGlobalOutput(this.consoleLogOutput);
			this.consoleLogOutput = null;

			if (this.dummyWindow != null)
			{
				ContentProvider.ClearContent();
				ContentProvider.DisposeDefaultContent();
				this.dummyWindow.Dispose();
				this.dummyWindow = null;
			}

			// Save local testing memory. As this uses Duality serializers, 
			// it needs to be done before terminating Duality.
			if (TestContext.CurrentContext.Result.Status == TestStatus.Passed && !System.Diagnostics.Debugger.IsAttached)
			{
				Serializer.WriteObject(TestHelper.LocalTestMemory, TestHelper.LocalTestMemoryFilePath, typeof(XmlSerializer));
			}

			DualityApp.Terminate();
			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
