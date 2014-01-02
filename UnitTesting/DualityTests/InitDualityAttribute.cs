using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Duality;

using OpenTK;

using NUnit.Framework;


namespace DualityTests
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityAttribute : Attribute, ITestAction
	{
		private	string	oldEnvDir = null;

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
				Log.Game.AddOutput(new ConsoleLogOutput(ConsoleColor.DarkGray));
				Log.Core.AddOutput(new ConsoleLogOutput(ConsoleColor.DarkBlue));
				Log.Editor.AddOutput(new ConsoleLogOutput(ConsoleColor.DarkMagenta));
			}

			// Initialize Duality
			DualityApp.Init(DualityApp.ExecutionEnvironment.Launcher, DualityApp.ExecutionContext.Game);

			// Manually register pseudo-plugin for the Unit Testing Assembly
			DualityApp.AddPlugin(typeof(DualityTestsPlugin).Assembly, codeBasePath);

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");

			// Just leave it initialized - speeds up re-running individual tests.
			//DualityApp.Terminate();
			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
