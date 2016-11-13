using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;

namespace Duality.Editor.Tests.PluginManager
{
	[TestFixture]
	public class EditorPluginManagerTest
	{
		[Test] public void LoadPlugins()
		{
			//
			// In this test, we're going to check overall LoadPlugins behavior,
			// as well as the specific case of selective loading, where non-editor
			// plugins and non-plugins are filtered out before loading.
			//
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				MockAssembly[] mockPlugins = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockPluginA.editor.dll", typeof(MockEditorPlugin)),
					new MockAssembly("MockDir/MockPluginB.editor.dll", typeof(MockEditorPlugin)),
					new MockAssembly("MockDir2/MockPluginC.editor.dll", typeof(MockEditorPlugin))
				};
				MockAssembly[] mockNoise = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockAuxillaryA.dll"),
					new MockAssembly("MockDir/MockPluginD.core.dll", typeof(MockEditorPlugin)),
					new MockAssembly("MockDir/MockPluginE.editor.dll"),
					new MockAssembly("MockDir2/MockAuxillaryB.dll", typeof(MockEditorPlugin)),
					MockAssembly.CreateInvalid("MockDir2/MockPluginF.editor.dll"),
					MockAssembly.CreateInvalid("MockDir2/MockAuxillaryC.dll")
				};
				string[] mockLoadedPaths = new string[] { 
					mockPlugins[0].Location, mockPlugins[1].Location, mockPlugins[2].Location, 
					mockNoise[2].Location, mockNoise[4].Location };

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
				EditorPluginManager pluginManager = new EditorPluginManager();
				pluginManager.Init(pluginLoader);

				// Load all plugins
				pluginManager.LoadPlugins();
				EditorPlugin[] loadedPlugins = pluginManager.LoadedPlugins.ToArray();

				// Assert that we loaded all expected plugins, but nothing more
				Assert.AreEqual(3, loadedPlugins.Length);
				CollectionAssert.AreEquivalent(mockPlugins, loadedPlugins.Select(plugin => plugin.PluginAssembly));

				// Assert that we properly assigned all plugin properties
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
				CollectionAssert.Contains(pluginManager.GetTypes(typeof(object)), typeof(MockEditorPlugin));
				Assert.AreEqual(3, pluginManager.GetTypes(typeof(MockEditorPlugin)).Count());

				pluginManager.Terminate();
			}
		}
		[Test] public void ResolveAssembly()
		{
			//
			// In this test, we're going to check the CorePluginManager's ability
			// to load requested editor plugins on-demand (to satisfy inter-plugin dependencies)
			// as well as to reject non-editor and non-plugins in those requests.
			//
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				MockAssembly[] mockAssemblies = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockPluginA.editor.dll", typeof(MockEditorPlugin)),
					new MockAssembly("MockDir/MockPluginB.editor.dll", typeof(MockEditorPlugin)),
					new MockAssembly("MockDir/MockPluginC.editor.dll", typeof(MockEditorPlugin))
				};
				MockAssembly mockCoreAssembly = new MockAssembly("MockDir/MockPluginD.core.dll");

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					pluginLoader.AddPlugin(mockAssemblies[i]);
				}
				pluginLoader.AddPlugin(mockCoreAssembly);

				// Set up a plugin manager using the mock loader
				EditorPluginManager pluginManager = new EditorPluginManager();
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
					// Attempt to resolve a not-yet-loaded core plugin
					Assembly resolvedAssembly = pluginLoader.InvokeResolveAssembly(mockCoreAssembly.FullName);

					// Assert that we did not resolve this, nor load any assemblies.
					// Leave this to the CorePluginManager, which can properly load them as a plugin.
					Assert.IsNull(resolvedAssembly);
					Assert.AreEqual(1, pluginManager.LoadedPlugins.Count());
					Assert.AreEqual(1, pluginLoader.LoadedAssemblyPaths.Count());
				}

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Assert that we do not have any duplicates and still the same count of plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());

				// Assert that other resolve calls will map to existing assemblies,
				// both for plugins and auxilliary libraries
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					Assert.AreSame(mockAssemblies[i], pluginLoader.InvokeResolveAssembly(mockAssemblies[i].FullName));
				}
				
				// Assert that we do not have any duplicates and still the same count of plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());

				pluginManager.Terminate();
			}
		}
	}
}
