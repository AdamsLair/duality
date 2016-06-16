using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using NUnit.Framework;

using Duality.IO;
using Duality.Backend;

namespace Duality.Tests.PluginManager
{
	internal class MockCorePlugin : CorePlugin
	{
		public delegate Assembly MapToAssembly(MockCorePlugin mockPlugin);
		public static event MapToAssembly MapToAssemblyCallback;

		private bool initialized = false;

		public bool Initialized
		{
			get { return this.initialized; }
		}

		protected MockCorePlugin()
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

			// Double-check our mocked values made it through
			if (this.PluginAssembly != assembly) Assert.Inconclusive("Can't create proper MockCorePlugin");
			if (this.AssemblyName != asmName) Assert.Inconclusive("Can't create proper MockCorePlugin");
		}
		protected internal override void InitPlugin()
		{
			base.InitPlugin();
			this.initialized = true;
		}
	}
}
