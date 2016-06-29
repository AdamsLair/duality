using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Duality.IO;

namespace Duality.Backend
{
	public class DefaultPluginLoader : IPluginLoader
	{
		private static readonly Assembly execAssembly = Assembly.GetEntryAssembly() ?? typeof(DualityApp).Assembly;
		private static readonly string execAssemblyDir = Path.GetFullPath(Path.GetDirectoryName(execAssembly.Location));

		public event EventHandler<AssemblyResolveEventArgs> AssemblyResolve;
		public event EventHandler<AssemblyLoadedEventArgs> AssemblyLoaded;

		public IEnumerable<string> BaseDirectories
		{
			get
			{
				List<string> availLibFiles = new List<string>();

				// Add the working directory plugin folder
				if (Directory.Exists(DualityApp.PluginDirectory)) 
				{
					availLibFiles.Add(DualityApp.PluginDirectory);
				}

				// Add the executing directory plugin folder
				string execPluginDir = Path.Combine(execAssemblyDir, DualityApp.PluginDirectory);
				bool sameDir = string.Equals(
					Path.GetFullPath(execPluginDir), 
					Path.GetFullPath(DualityApp.PluginDirectory), 
					StringComparison.InvariantCultureIgnoreCase);
				if (!sameDir && Directory.Exists(execPluginDir))
				{
					availLibFiles.Add(execPluginDir);
				}

				return availLibFiles;
			}
		}
		public IEnumerable<string> AvailableAssemblyPaths
		{
			get
			{
				IEnumerable<string> availLibFiles = Enumerable.Empty<string>();
				foreach (string baseDir in this.BaseDirectories)
				{
					availLibFiles = availLibFiles.Concat(Directory.EnumerateFiles(baseDir, "*.dll", SearchOption.AllDirectories));
				}
				return availLibFiles;
			}
		}

		public Assembly LoadAssembly(string assemblyPath, bool anonymous)
		{
			// The regular assembly load can just lock the file and be done with it.
			if (!anonymous)
			{
				return Assembly.LoadFrom(assemblyPath);
			}
			// Loading an Assembly anonymously requires not locking the path and hiding its identity.
			else
			{
				// Guess the path of the symbol file
				string pluginDebugInfoPath = Path.Combine(
					Path.GetDirectoryName(assemblyPath), 
					Path.GetFileNameWithoutExtension(assemblyPath)) + ".pdb";
				if (!File.Exists(pluginDebugInfoPath))
					pluginDebugInfoPath = null;

				// Load the assembly - and its symbols, if provided
				if (pluginDebugInfoPath != null)
					return Assembly.Load(File.ReadAllBytes(assemblyPath), File.ReadAllBytes(pluginDebugInfoPath));
				else
					return Assembly.Load(File.ReadAllBytes(assemblyPath));
			}
		}
		public int GetAssemblyHash(string assemblyPath)
		{
			if (!File.Exists(assemblyPath)) return 0;

			using (BufferedStream stream = new BufferedStream(File.OpenRead(assemblyPath), 512000))
			{
				var sha = System.Security.Cryptography.MD5.Create();
				byte[] hash = sha.ComputeHash(stream);
				return BitConverter.ToInt32(hash, 0);
			}
		}
		
		public void Init()
		{
			// Log some information about working directories, so we can debug missing libraries
			{
				Log.Core.Write(
					"Current Directory:  {0}" + Environment.NewLine +
					"Command Line:       {1}" + Environment.NewLine +
					"Entry Assembly:     {2}" + Environment.NewLine +
					"Entry Assembly Dir: {3}",
					Environment.CurrentDirectory,
					Environment.CommandLine,
					Log.Assembly(execAssembly),
					execAssemblyDir);
			}
			// Log all currently loaded assemblies as a diagnostic consistency check
			{
				Log.Core.Write("Loaded Assemblies:");
				Log.Core.PushIndent();
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					Log.Core.Write(Log.Assembly(assembly));
				}
				Log.Core.PopIndent();
			}
			AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
			AppDomain.CurrentDomain.AssemblyLoad += this.CurrentDomain_AssemblyLoad;
		}
		public void Terminate()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= this.CurrentDomain_AssemblyResolve;
			AppDomain.CurrentDomain.AssemblyLoad -= this.CurrentDomain_AssemblyLoad;
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			// First, trigger a resolve event and see if we found a matching Assembly.
			// This will give core and editor plugin managers to load plugin Assemblies
			// their own way, or resolve with an already loaded one.
			if (this.AssemblyResolve != null)
			{
				AssemblyResolveEventArgs resolveArgs = new AssemblyResolveEventArgs(args.Name);
				this.AssemblyResolve(this, resolveArgs);

				if (resolveArgs.IsResolved)
					return resolveArgs.ResolvedAssembly;
			}
			
			// Admit that we didn't find anything.
			if (args.RequestingAssembly != null)
			{
				Log.Core.WriteWarning(
					"Can't resolve Assembly '{0}' (as requested by '{1}'): None of the available assembly paths matches the requested name.",
					args.Name,
					Log.Assembly(args.RequestingAssembly));
			}
			else
			{
				Log.Core.WriteWarning(
					"Can't resolve Assembly '{0}': None of the available assembly paths matches the requested name.",
					args.Name);
			}
			return null;
		}
		private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			if (this.AssemblyLoaded != null)
				this.AssemblyLoaded(this, new AssemblyLoadedEventArgs(args.LoadedAssembly));

			Log.Core.Write("Assembly loaded: {0}", args.LoadedAssembly.GetShortAssemblyName());
		}
	}
}
