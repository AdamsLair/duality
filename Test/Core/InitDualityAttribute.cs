using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Duality.Launcher;

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
			string codeBaseURI = typeof(DualityApp).Assembly.CodeBase;
			string codeBasePath = codeBaseURI.StartsWith("file:") ? codeBaseURI.Remove(0, "file:".Length) : codeBaseURI;
			codeBasePath = codeBasePath.TrimStart('/');
			Console.WriteLine("Testing Core Assembly: {0}", codeBasePath);
			Environment.CurrentDirectory = Path.GetDirectoryName(codeBasePath);

			if (this.launcher == null)
			{
				this.launcher = new DualityLauncher();
			}
			
			// Manually register pseudo-plugin for the Unit Testing Assembly
			this.unitTestPlugin = DualityApp.PluginManager.LoadPlugin(
				typeof(DualityTestsPlugin).Assembly, 
				codeBasePath);

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
