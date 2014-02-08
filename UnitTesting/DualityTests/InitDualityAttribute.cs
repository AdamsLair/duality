using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Duality;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using NUnit.Framework;


namespace DualityTests
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public class InitDualityAttribute : Attribute, ITestAction
	{
		private	string		oldEnvDir	= null;
		private	GameWindow	dummyWindow	= null;

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

			// Create a dummy window, to get access to all the device contexts
			if (this.dummyWindow == null)
			{
				this.dummyWindow = new GameWindow(800, 600);
				this.dummyWindow.Context.LoadAll();
				this.dummyWindow.Visible = true;
				this.dummyWindow.Context.Update(this.dummyWindow.WindowInfo);
				this.dummyWindow.MakeCurrent();
				this.dummyWindow.ProcessEvents();
				DualityApp.TargetResolution = new Vector2(this.dummyWindow.Width, this.dummyWindow.Height);
				DualityApp.TargetMode = this.dummyWindow.Context.GraphicsMode;
				ContentProvider.InitDefaultContent();
			}

			Console.WriteLine("----- Duality environment setup complete -----");
		}
		public void AfterTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality environment teardown -----");

			if (this.dummyWindow != null)
			{
			    this.dummyWindow.Dispose();
			    this.dummyWindow = null;
			}
			DualityApp.Terminate();
			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality environment teardown complete -----");
		}
	}
}
