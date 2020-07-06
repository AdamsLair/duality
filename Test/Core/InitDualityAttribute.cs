using System;

using Duality.Launcher;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Duality.Tests
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityAttribute : Attribute, ITestAction
	{
		private	string				oldEnvDir			= null;
		private	CorePlugin			unitTestPlugin		= null;

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		private DualityLauncher launcher;
		public InitDualityAttribute() {}
		public void BeforeTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality environment setup -----");

			// Set environment directory to Duality binary directory
			this.oldEnvDir = Environment.CurrentDirectory;
			Console.WriteLine("Testing in working directory: {0}", TestContext.CurrentContext.TestDirectory);
			Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;

			if (this.launcher == null)
			{
				this.launcher = new DualityLauncher();
			}

			// Manually register pseudo-plugin for the Unit Testing Assembly
			this.unitTestPlugin = DualityApp.PluginManager.LoadPlugin(
				typeof(DualityTestsPlugin).Assembly,
				typeof(DualityTestsPlugin).Assembly.Location);

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");

			if (this.launcher != null)
			{
				this.launcher.Dispose();
			}

			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
