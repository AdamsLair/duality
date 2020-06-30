using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Duality.Tests
{

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityAttribute : Attribute, ITestAction
	{
		private DualityTestLauncher launcher;
		private CorePlugin unitTestPlugin = null;

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		public InitDualityAttribute() { }


		public void BeforeTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality environment setup -----");

			if (this.launcher == null)
			{
				this.launcher = new DualityTestLauncher();
			}

			// Manually register pseudo-plugin for the Unit Testing Assembly
			this.unitTestPlugin = DualityApp.PluginManager.LoadPlugin(
				typeof(DualityTestsPlugin).Assembly,
				this.launcher.CodeBasePath);

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(ITest details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");

			if (this.launcher != null)
			{
				this.launcher.Dispose();
			}

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
