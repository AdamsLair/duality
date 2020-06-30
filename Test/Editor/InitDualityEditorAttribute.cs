using System;

using Duality.Editor.Forms;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Duality.Editor.Tests
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityEditorAttribute : Attribute, ITestAction
	{
		private	string				oldEnvDir			= null;
		private	MainForm			dummyWindow			= null;
		private	TextWriterLogOutput	consoleLogOutput	= null;

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		public InitDualityEditorAttribute() {}
		public void BeforeTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality Editor environment setup -----");

			// Set environment directory to Duality binary directory
			this.oldEnvDir = Environment.CurrentDirectory;
			Console.WriteLine("Testing Editor Assembly: {0}", TestContext.CurrentContext.TestDirectory);
			Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

			// Add some Console logs manually for NUnit
			if (this.consoleLogOutput == null)
				this.consoleLogOutput = new TextWriterLogOutput(Console.Out);
			Logs.AddGlobalOutput(this.consoleLogOutput);

			// Create a dummy window for the editor
			if (this.dummyWindow == null)
				this.dummyWindow = new MainForm();

			// Initialize the Duality Editor
			DualityEditorApp.Init(this.dummyWindow, false);

			Console.WriteLine("----- Duality Editor environment setup complete -----");
		}
		public void AfterTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality Editor environment teardown -----");
			
			// Remove NUnit Console logs
			Logs.RemoveGlobalOutput(this.consoleLogOutput);
			this.consoleLogOutput = null;

			if (this.dummyWindow != null)
			{
				ContentProvider.ClearContent();
			    this.dummyWindow.Dispose();
			    this.dummyWindow = null;
			}

			DualityEditorApp.Terminate(false);
			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality Editor environment teardown complete -----");
		}
	}
}
