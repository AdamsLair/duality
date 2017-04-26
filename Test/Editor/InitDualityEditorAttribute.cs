using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Duality;
using Duality.Serialization;
using Duality.Backend;
using Duality.Editor;
using Duality.Editor.Forms;

using NUnit.Framework;


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
		public void BeforeTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality Editor environment setup -----");

			// Set environment directory to Duality binary directory
			this.oldEnvDir = Environment.CurrentDirectory;
			string codeBaseURI = typeof(DualityEditorApp).Assembly.CodeBase;
			string codeBasePath = codeBaseURI.StartsWith("file:") ? codeBaseURI.Remove(0, "file:".Length) : codeBaseURI;
			codeBasePath = codeBasePath.TrimStart('/');
			Console.WriteLine("Testing Editor Assembly: {0}", codeBasePath);
			Environment.CurrentDirectory = Path.GetDirectoryName(codeBasePath);

			// Add some Console logs manually for NUnit
			if (this.consoleLogOutput == null) 
				this.consoleLogOutput = new TextWriterLogOutput(Console.Out);
			Log.AddGlobalOutput(this.consoleLogOutput);

			// Create a dummy window for the editor
			if (this.dummyWindow == null)
				this.dummyWindow = new MainForm();

			// Initialize the Duality Editor
			DualityEditorApp.Init(this.dummyWindow, false);

			Console.WriteLine("----- Duality Editor environment setup complete -----");
		}
		public void AfterTest(TestDetails details)
		{
			Console.WriteLine("----- Beginning Duality Editor environment teardown -----");
			
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

			DualityEditorApp.Terminate(false);
			Environment.CurrentDirectory = this.oldEnvDir;

			Console.WriteLine("----- Duality Editor environment teardown complete -----");
		}
	}
}
