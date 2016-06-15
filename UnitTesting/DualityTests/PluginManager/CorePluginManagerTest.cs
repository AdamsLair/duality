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
		[Test] public void PluginLoaderInitTerminate()
		{
			MockPluginLoader pluginLoader = new MockPluginLoader();
			CorePluginManager pluginManager = new CorePluginManager();

			// We expect the plugin manager not to assume ownership of
			// the plugin loader, e.g. not to initialize or terminate it.

			Assert.IsFalse(pluginLoader.Initialized);
			Assert.IsFalse(pluginLoader.Disposed);

			pluginManager.Init(pluginLoader);

			Assert.IsFalse(pluginLoader.Initialized);
			Assert.IsFalse(pluginLoader.Disposed);

			pluginManager.Terminate();

			Assert.IsFalse(pluginLoader.Initialized);
			Assert.IsFalse(pluginLoader.Disposed);
		}
		[Test] public void LoadPlugins()
		{
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
					pluginLoader.LoadedAssemblies);

				// Assert that we can access all assemblies and types from plugins
				foreach (MockAssembly mockAssembly in mockPlugins)
				{
					CollectionAssert.Contains(pluginManager.GetCoreAssemblies(), mockAssembly);
				}
				CollectionAssert.Contains(pluginManager.GetCoreTypes(typeof(object)), typeof(MockCorePlugin));
				Assert.AreEqual(3, pluginManager.GetCoreTypes(typeof(MockCorePlugin)).Count());

				pluginManager.Terminate();
			}
		}
		[Test] public void PluginLifecycle()
		{
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.core.dll", typeof(MockCorePlugin)));

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// Register event handler to check if all events are fired as expected
				HashSet<CorePlugin> firedPluginReady = new HashSet<CorePlugin>();
				HashSet<CorePlugin> firedPluginRemoving = new HashSet<CorePlugin>();
				HashSet<CorePlugin> firedPluginRemoved = new HashSet<CorePlugin>();
				pluginManager.PluginsReady += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginReady.Add(plugin);
				};
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Load all plugins and expect them to be not initialized yet
				pluginManager.LoadPlugins();
				MockCorePlugin[] loadedPlugins = pluginManager.LoadedPlugins
					.Cast<MockCorePlugin>()
					.ToArray();
				Assert.IsTrue(loadedPlugins.All(plugin => !plugin.Initialized));
				Assert.IsTrue(loadedPlugins.All(plugin => !plugin.Disposed));

				// Initialize plugins and expect them all to be initialized
				pluginManager.InitPlugins();
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.Initialized));
				Assert.IsTrue(loadedPlugins.All(plugin => !plugin.Disposed));
				CollectionAssert.AreEquivalent(loadedPlugins, firedPluginReady);

				// Discard all plugins and expect them to be disposed
				pluginManager.ClearPlugins();
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.Initialized));
				Assert.IsTrue(loadedPlugins.All(plugin => plugin.Disposed));
				CollectionAssert.AreEquivalent(loadedPlugins, firedPluginRemoving);
				CollectionAssert.AreEquivalent(loadedPlugins, firedPluginRemoved);

				pluginManager.Terminate();
			}
		}
		[Test] public void ReloadPlugin()
		{
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				MockAssembly oldAssembly = new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin));
				MockAssembly newAssembly = new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin));

				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(oldAssembly);
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.core.dll", typeof(MockCorePlugin)));

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Register event handler to check if all events are fired as expected
				HashSet<CorePlugin> firedPluginReady = new HashSet<CorePlugin>();
				HashSet<CorePlugin> firedPluginRemoving = new HashSet<CorePlugin>();
				HashSet<CorePlugin> firedPluginRemoved = new HashSet<CorePlugin>();
				pluginManager.PluginsReady += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginReady.Add(plugin);
				};
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Silently replace one of the assemblies
				pluginLoader.ReplacePlugin(oldAssembly, newAssembly);

				// Reload the plugin that was replaced
				MockCorePlugin reloadedPlugin = pluginManager.ReloadPlugin(oldAssembly.Location) as MockCorePlugin;

				// Assert that we got back the new plugin, while the old one is now disposed
				Assert.IsNotNull(reloadedPlugin);
				Assert.AreSame(newAssembly, reloadedPlugin.PluginAssembly);
				CollectionAssert.Contains(pluginManager.LoadedPlugins.Select(p => p.PluginAssembly), newAssembly);
				CollectionAssert.Contains(pluginManager.DisposedPlugins, oldAssembly);
				CollectionAssert.DoesNotContain(pluginManager.LoadedPlugins.Select(p => p.PluginAssembly), oldAssembly);
				CollectionAssert.DoesNotContain(pluginManager.DisposedPlugins, newAssembly);
				CollectionAssert.Contains(firedPluginRemoving.Select(p => p.PluginAssembly), oldAssembly);
				CollectionAssert.Contains(firedPluginRemoved.Select(p => p.PluginAssembly), oldAssembly);

				// Assert that the reloaded plugin is not yet initialized.
				// That's not what the API is supposed to do.
				Assert.IsFalse(reloadedPlugin.Initialized);
				CollectionAssert.DoesNotContain(firedPluginReady, newAssembly);

				pluginManager.Terminate();
			}
		}
		[Test] public void LockedPlugin()
		{
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				MockAssembly lockedAssembly = new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin));

				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(lockedAssembly);
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.core.dll", typeof(MockCorePlugin)));

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Register event handler to check if all events are fired as expected
				HashSet<CorePlugin> firedPluginRemoving = new HashSet<CorePlugin>();
				HashSet<CorePlugin> firedPluginRemoved = new HashSet<CorePlugin>();
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (CorePlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Lock the plugin we're about to reload
				pluginManager.LockPlugin(lockedAssembly);

				// Attempt to reload the plugin that was locked
				CorePlugin reloadedPlugin = pluginManager.ReloadPlugin(lockedAssembly.Location);

				// Assert that nothing has changed and the reload attempt was rejected
				Assert.IsNull(reloadedPlugin);
				CollectionAssert.Contains(pluginManager.LoadedPlugins.Select(p => p.PluginAssembly), lockedAssembly);
				CollectionAssert.DoesNotContain(firedPluginRemoving.Select(p => p.PluginAssembly), lockedAssembly);
				CollectionAssert.DoesNotContain(firedPluginRemoved.Select(p => p.PluginAssembly), lockedAssembly);

				pluginManager.Terminate();
			}
		}
		[Test] public void ResolveAssembly()
		{
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

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					pluginLoader.AddPlugin(mockAssemblies[i]);
				}

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// First, make sure the attempt to resolve a not-yet-loaded plugin
				// will result in loading it immediately to satisfy dependency relations
				Assembly resolvedAssembly = pluginLoader.InvokeResolveAssembly(mockAssemblies[0].FullName);
				Assert.IsNotNull(resolvedAssembly);
				Assert.AreSame(mockAssemblies[0], resolvedAssembly);
				Assert.AreEqual(1, pluginManager.LoadedPlugins.Count());
				Assert.AreSame(mockAssemblies[0], pluginManager.LoadedPlugins.First().PluginAssembly);
				CollectionAssert.Contains(pluginLoader.LoadedAssemblies, mockAssemblies[0].Location);

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
		[Test] public void DuplicateLoad()
		{
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				MockAssembly[] mockPlugins = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockPluginA.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginB.core.dll", typeof(MockCorePlugin)),
					new MockAssembly("MockDir/MockPluginC.core.dll", typeof(MockCorePlugin))
				};
				MockAssembly[] mockAssemblies = new MockAssembly[]
				{
					mockPlugins[0],
					mockPlugins[1],
					mockPlugins[2],
					new MockAssembly("MockDir/MockAuxilliaryA.dll"),
					new MockAssembly("MockDir/MockAuxilliaryB.dll")
				};

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					pluginLoader.AddPlugin(mockAssemblies[i]);
				}

				// Set up a plugin manager using the mock loader
				CorePluginManager pluginManager = new CorePluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Now load them again
				pluginManager.LoadPlugins();

				// Assert that we do not have any duplicates and no disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				// Let's try loading assembly duplicates manually
				for (int i = 0; i < mockPlugins.Length; i++)
				{
					CorePlugin plugin = pluginManager.LoadPlugin(mockAssemblies[i], mockAssemblies[i].Location);
					Assert.IsNotNull(plugin);
				}

				// Assert that we do not have any duplicates and no disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				pluginManager.Terminate();
			}
		}
	}
}
