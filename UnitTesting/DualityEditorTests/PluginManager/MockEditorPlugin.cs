using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using NUnit.Framework;

using Duality.IO;
using Duality.Backend;

namespace Duality.Editor.Tests.PluginManager
{
	internal class MockEditorPlugin : EditorPlugin
	{
		public delegate Assembly MapToAssembly(MockEditorPlugin mockPlugin);
		public static event MapToAssembly MapToAssemblyCallback;

		private string id = null;
		private bool initialized = false;

		public bool Initialized
		{
			get { return this.initialized; }
		}
		public override string Id
		{
			get { return this.id; }
		}

		protected MockEditorPlugin()
		{
			// Create mocked assembly and assembly name values via test callback
			FieldInfo assemblyField = typeof(DualityPlugin).GetField("assembly", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo assemblyNameField = typeof(DualityPlugin).GetField("asmName", BindingFlags.Instance | BindingFlags.NonPublic);

			// Can't proceed with test, if we're unable to properly mock the CorePlugin implementation
			if (assemblyField == null) Assert.Inconclusive("Can't create proper MockCorePlugin");
			if (assemblyNameField == null) Assert.Inconclusive("Can't create proper MockCorePlugin");

			// Assign values we got via callback
			Assembly assembly = MapToAssemblyCallback(this);
			string asmName = assembly.GetShortAssemblyName();
			assemblyField.SetValue(this, assembly);
			assemblyNameField.SetValue(this, asmName);
			this.id = asmName;

			// Double-check our mocked values made it through
			if (this.PluginAssembly != assembly) Assert.Inconclusive("Can't create proper MockCorePlugin");
			if (this.AssemblyName != asmName) Assert.Inconclusive("Can't create proper MockCorePlugin");
		}
		protected internal override void InitPlugin(Forms.MainForm main)
		{
			base.InitPlugin(main);
			this.initialized = true;
		}
	}
}
