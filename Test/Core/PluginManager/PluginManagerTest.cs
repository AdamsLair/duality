using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;

namespace Duality.Tests.PluginManager
{
	[TestFixture]
	public class PluginManagerTest
	{
		[Test] public void PluginLoaderInitTerminate()
		{
			MockPluginLoader pluginLoader = new MockPluginLoader();
			MockPluginManager pluginManager = new MockPluginManager();

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
					new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir2/MockPluginC.dll", typeof(MockPlugin))
				};
				MockAssembly[] mockNoise = new MockAssembly[]
				{
					new MockAssembly("MockDir/MockAuxillaryA.dll"),
					MockAssembly.CreateInvalid("MockDir2/MockPluginF.dll"),
					MockAssembly.CreateInvalid("MockDir2/MockAuxillaryC.dll")
				};
				string[] mockLoadedPaths = new string[] { 
					mockPlugins[0].Location, mockPlugins[1].Location, mockPlugins[2].Location, 
					mockNoise[0].Location, mockNoise[1].Location, mockNoise[2].Location,
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
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Load all plugins
				pluginManager.LoadPlugins();
				MockPlugin[] loadedPlugins = pluginManager.LoadedPlugins.ToArray();

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
				CollectionAssert.Contains(pluginManager.GetTypes(typeof(object)), typeof(MockPlugin));
				Assert.AreEqual(3, pluginManager.GetTypes(typeof(MockPlugin)).Count());

				pluginManager.Terminate();
			}
		}
		[Test] public void PluginLifecycle()
		{
			using (MockPluginLoader pluginLoader = new MockPluginLoader())
			{
				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.dll", typeof(MockPlugin)));

				// Set up a plugin manager using the mock loader
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Register event handler to check if all events are fired as expected
				HashSet<MockPlugin> firedPluginReady = new HashSet<MockPlugin>();
				HashSet<MockPlugin> firedPluginRemoving = new HashSet<MockPlugin>();
				HashSet<MockPlugin> firedPluginRemoved = new HashSet<MockPlugin>();
				pluginManager.PluginsReady += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginReady.Add(plugin);
				};
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Load all plugins and expect them to be not initialized yet
				pluginManager.LoadPlugins();
				MockPlugin[] loadedPlugins = pluginManager.LoadedPlugins
					.Cast<MockPlugin>()
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
				MockAssembly oldAssembly = new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin));
				MockAssembly newAssembly = new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin));

				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(oldAssembly);
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.dll", typeof(MockPlugin)));

				// Set up a plugin manager using the mock loader
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Register event handler to check if all events are fired as expected
				HashSet<MockPlugin> firedPluginReady = new HashSet<MockPlugin>();
				HashSet<MockPlugin> firedPluginRemoving = new HashSet<MockPlugin>();
				HashSet<MockPlugin> firedPluginRemoved = new HashSet<MockPlugin>();
				pluginManager.PluginsReady += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginReady.Add(plugin);
				};
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Silently replace one of the assemblies
				pluginLoader.ReplacePlugin(oldAssembly, newAssembly);

				// Reload the plugin that was replaced
				MockPlugin reloadedPlugin = pluginManager.ReloadPlugin(oldAssembly.Location) as MockPlugin;

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
				MockAssembly lockedAssembly = new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin));

				pluginLoader.AddBaseDir("MockDir");
				pluginLoader.AddPlugin(lockedAssembly);
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)));
				pluginLoader.AddPlugin(new MockAssembly("MockDir/MockPluginC.dll", typeof(MockPlugin)));

				// Set up a plugin manager using the mock loader
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Register event handler to check if all events are fired as expected
				HashSet<MockPlugin> firedPluginRemoving = new HashSet<MockPlugin>();
				HashSet<MockPlugin> firedPluginRemoved = new HashSet<MockPlugin>();
				pluginManager.PluginsRemoving += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoving.Add(plugin);
				};
				pluginManager.PluginsRemoved += (sender, args) =>
				{
					foreach (MockPlugin plugin in args.Plugins)
						firedPluginRemoved.Add(plugin);
				};

				// Lock the plugin we're about to reload
				pluginManager.LockPlugin(lockedAssembly);

				// Attempt to reload the plugin that was locked
				MockPlugin reloadedPlugin = pluginManager.ReloadPlugin(lockedAssembly.Location);

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
					new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir/MockPluginC.dll", typeof(MockPlugin))
				};

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockAssemblies.Length; i++)
				{
					pluginLoader.AddPlugin(mockAssemblies[i]);
				}

				// Set up a plugin manager using the mock loader
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

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
					new MockAssembly("MockDir/MockPluginA.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir/MockPluginB.dll", typeof(MockPlugin)),
					new MockAssembly("MockDir/MockPluginC.dll", typeof(MockPlugin))
				};

				// Set up some mock data for available assemblies
				pluginLoader.AddBaseDir("MockDir");
				for (int i = 0; i < mockPlugins.Length; i++)
				{
					pluginLoader.AddPlugin(mockPlugins[i]);
				}

				// Set up a plugin manager using the mock loader
				MockPluginManager pluginManager = new MockPluginManager();
				pluginManager.Init(pluginLoader);

				// Load and init all plugins
				pluginManager.LoadPlugins();
				pluginManager.InitPlugins();

				// Now load them again
				pluginManager.LoadPlugins();

				// Assert that we do not have any duplicates and no disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				// Assert that we did not load any assembly twice
				Assert.AreEqual(3, pluginLoader.LoadedAssemblyPaths.Count());

				// Let's try loading assembly duplicates manually
				for (int i = 0; i < mockPlugins.Length; i++)
				{
					MockPlugin plugin = pluginManager.LoadPlugin(mockPlugins[i], mockPlugins[i].Location);
					Assert.IsNotNull(plugin);
				}

				// Assert that we do not have any duplicates and no disposed plugins
				Assert.AreEqual(3, pluginManager.LoadedPlugins.Count());
				Assert.IsEmpty(pluginManager.DisposedPlugins);

				// Assert that we did not load any assembly twice
				Assert.AreEqual(3, pluginLoader.LoadedAssemblyPaths.Count());

				pluginManager.Terminate();
			}
		}
	}
}
