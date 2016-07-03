using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;

namespace Duality.Tests.PluginManager
{
	[TestFixture]
	public class CorePluginManagerTest
	{
		[Test] public void LoadPlugins()
		{
			//
			// In this test, we're going to check overall LoadPlugins behavior,
			// as well as the specific case of selective loading, where non-core
			// plugins and non-plugins are filtered out before loading.
			//
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				MockAssembly[] mockPlugins = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir2/MockPluginC.core.dll", typeof(MockCorePlugin))
				};
				MockAssembly[] mockNoise = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockAuxillaryA.dll"),
					new MockAssembly("MockDir/MockPluginD.editor.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginE.core.dll"),
					new MockAssembly("MockDir2/MockAuxillaryB.dll", typeof(MockCorePlugin)),
					MockAssembly.CreateInvalid("MockDir2/MockPluginF.core.dll"),
					MockAssembly.CreateInvalid("MockDir2/MockAuxillaryC.dll")
				};
				string[] mockLoadedPaths = new string[] { 
					mockPlugins[0].Location, mockPlugins[1].Location, mockPlugins[2].Location, 
					mockNoise[0].Location, mockNoise[2].Location, mockNoise[3].Location, mockNoise[4].Location, mockNoise[5].Location,
					"MockDir2/MockAuxillaryD.dll"};

				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddBaseDir("MockDir2");
				for (int i = 0; i < mockPlugins.Length; i++)
				{
					pluginLoader.AddPlugin(mockPlugins[i]);
				}
				for (int i = 0; i < mockNoise.Length; i++)
				{
					pluginLoader.AddPlugin(mockNoise[i]);
				}
				pluginLoader.AddIncompatibleDll("MockDir2/MockAuxillaryD.dll");

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// Load all plugins
				pluginManager.LoadPlugins();
				CorePlugin[] loadedPlugins = pluginManager.LoadedPlugins.ToArray();

				// Assert that we loaded all expected plugins, but nothing more
				Assert.AreEqual(3, loadedPlugins.Length);
				CollectionAssert.AreEquivalent(mockPlugins, loadedPlugins.Select(plugin => plugin.PluginAssembly));

				// Assert that we properly assigned all plugin properties
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.FilePath == plugin.PluginAssembly.Location));
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.FileHash == pluginLoader.GetAssemblyHash(plugin.FilePath)));
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.AssemblyName == plugin.PluginAssembly.GetShortAssemblyName()));

				// Assert that we loaded core plugin and auxilliary libraries, but not editor plugins
				CollectionAssert.AreEquivalent(
					mockLoadedPaths, 
					pluginLoader.LoadedAssemblyPaths);

				// Assert that we can access all assemblies and types from plugins
				foreach (MockAssembly mockAssembly in mockPlugins)
				{
					CollectionAssert.Contains(pluginManager.GetAssemblies(), mockAssembly);
				}
				CollectionAssert.Contains(pluginManager.GetTypes(typeof(object)), typeof(MockCorePlugin));
				Assert.AreEqual(3, pluginManager.GetTypes(typeof(MockCorePlugin)).Count());

				pluginManager.Terminate();
			}
		}
		[Test] public void ResolveAssembly()
		{
			//
			// In this test, we're going to check the CorePluginManager's ability
			// to load requested core plugins on-demand (to satisfy inter-plugin dependencies)
			// as well as to reject non-core and non-plugins in those requests.
			//
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				MockAssembly[] mockAssemblies = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginC.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockAuxilliaryA.dll"),
					new MockAssembly("MockDir/MockAuxilliaryB.dll")
				};
				MockAssembly mockEditorAssembly = new MockAssembly("MockDir/MockPluginD.editor.dll");

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					pluginLoader.AddPlugin(mockAssemblies[i]);
				}
				pluginLoader.AddPlugin(mockEditorAssembly);

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				{
					// First, make sure the attempt to resolve a not-yet-loaded plugin
					// will result in loading it immediately to satisfy dependency relations
					Assembly resolvedAssembly = pluginLoader.InvokeResolveAssembly(mockAssemblies[0].FullName);

					// Assert that we successfully resolved it with a plugin (not just an assembly)
					Assert.IsNotNull(resolvedAssembly);
					Assert.AreSame(mockAssemblies[0], resolvedAssembly);
					Assert.AreEqual(1, pluginManager.LoadedPlugins.Count());
					Assert.AreSame(mockAssemblies[0], pluginManager.LoadedPlugins.First().PluginAssembly);
					Assert.AreEqual(1, pluginLoader.LoadedAssemblyPaths.Count());
					CollectionAssert.Contains(pluginLoader.LoadedAssemblyPaths, mockAssemblies[0].Location);
				}

				{
					// Attempt to resolve a not-yet-loaded editor plugin
					Assembly resolvedAssembly = pluginLoader.InvokeResolveAssembly(mockEditorAssembly.FullName);

					// Assert that we did not resolve this, nor load any assemblies.
					// Leave this to the EditorPluginManager, which can properly load them as a plugin.
					Assert.IsNull(resolvedAssembly);
					Assert.AreEqual(1, pluginManager.LoadedPlugins.Count());
					Assert.AreEqual(1, pluginLoader.LoadedAssemblyPaths.Count());
				}

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Assert that we do not have any duplicates and no disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				// Assert that other resolve calls will map to existing assemblies,
				// both for plugins and auxilliary libraries
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					Assert.AreSame(mockAssemblies[i], pluginLoader.InvokeResolveAssembly(mockAssemblies[i].FullName));
				}
				
				// Assert that we still have the expected amount of loaded and disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				pluginManager.Terminate();
			}
		}
	}
}
