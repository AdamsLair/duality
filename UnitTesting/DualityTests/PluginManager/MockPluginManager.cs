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
	internal class MockPluginManager : PluginManager<MockPlugin>
	{
		public override void LoadPlugins()
		{
			foreach (string path in this.PluginLoader.AvailableAssemblyPaths)
			{
				this.LoadPlugin(path);
			}
		}
		public override void InitPlugins()
		{
			foreach (MockPlugin plugin in this.LoadedPlugins)
			{
				this.InitPlugin(plugin);
			}
		}
		protected override void OnInitPlugin(MockPlugin plugin)
		{
			plugin.InitPlugin();
		}
	}
}
