using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Duality.Backend
{
	public class DefaultPluginLoader : IPluginLoader
	{
		private ResolveAssemblyCallback resolveCallback = null;

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
				string execPluginDir = Path.Combine(PathHelper.ExecutingAssemblyDir, DualityApp.PluginDirectory);
				if (!PathHelper.ArePathsEqual(execPluginDir, DualityApp.PluginDirectory) && Directory.Exists(execPluginDir))
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
		
		public void Init(ResolveAssemblyCallback resolveCallback)
		{
			this.resolveCallback = resolveCallback;

			AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
			AppDomain.CurrentDomain.AssemblyLoad += this.CurrentDomain_AssemblyLoad;
		}
		public void Terminate()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= this.CurrentDomain_AssemblyResolve;
			AppDomain.CurrentDomain.AssemblyLoad -= this.CurrentDomain_AssemblyLoad;

			this.resolveCallback = null;
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (this.resolveCallback != null)
				return this.resolveCallback(new ResolveAssemblyEventArgs(args.Name));
			else
				return null;
		}
		private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			Log.Core.Write("Assembly loaded: {0}", args.LoadedAssembly.GetShortAssemblyName());
		}
	}
}
