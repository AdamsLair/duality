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
	internal class MockPlugin : DualityPlugin
	{
		public delegate Assembly MapToAssembly(MockPlugin mockPlugin);
		public static event MapToAssembly MapToAssemblyCallback;

		private bool initialized = false;

		public bool Initialized
		{
			get { return this.initialized; }
		}

		protected MockPlugin()
		{
			// Create mocked assembly and assembly name values via test callback
			FieldInfo assemblyField = typeof(DualityPlugin).GetField("assembly", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo assemblyNameField = typeof(DualityPlugin).GetField("asmName", BindingFlags.Instance | BindingFlags.NonPublic);

			// Can't proceed with test, if we're unable to properly mock the CorePlugin implementation
			if (assemblyField == null) Assert.Inconclusive("Can't create proper MockPlugin");
			if (assemblyNameField == null) Assert.Inconclusive("Can't create proper MockPlugin");

			// Assign values we got via callback
			Assembly assembly = MapToAssemblyCallback(this);
			string asmName = assembly.GetShortAssemblyName();
			assemblyField.SetValue(this, assembly);
			assemblyNameField.SetValue(this, asmName);

			// Double-check our mocked values made it through
			if (this.PluginAssembly != assembly) Assert.Inconclusive("Can't create proper MockPlugin");
			if (this.AssemblyName != asmName) Assert.Inconclusive("Can't create proper MockPlugin");
		}
		public void InitPlugin()
		{
			this.initialized = true;
		}
	}
}
