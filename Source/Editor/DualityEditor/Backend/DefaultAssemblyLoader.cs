﻿using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Duality.IO;
using System.Text;

namespace Duality.Backend
{
	public class DefaultAssemblyLoader : IAssemblyLoader
	{
		private static readonly Assembly execAssembly = Assembly.GetEntryAssembly() ?? typeof(DualityApp).Assembly;
		private static readonly string execAssemblyDir = Path.GetFullPath(Path.GetDirectoryName(execAssembly.Location));

		public event EventHandler<AssemblyResolveEventArgs> AssemblyResolve;
		public event EventHandler<AssemblyLoadedEventArgs> AssemblyLoaded;

		public IEnumerable<string> BaseDirectories
		{
			get
			{
				List<string> searchDirectories = new List<string>();

				// Add the working directory plugin folder
				if (Directory.Exists(DualityApp.PluginDirectory)) 
				{
					searchDirectories.Add(DualityApp.PluginDirectory);
				}

				// Add the executing directory plugin folder
				string execPluginDir = Path.Combine(execAssemblyDir, DualityApp.PluginDirectory);
				bool sameDir = string.Equals(
					Path.GetFullPath(execPluginDir), 
					Path.GetFullPath(DualityApp.PluginDirectory), 
					StringComparison.InvariantCultureIgnoreCase);
				if (!sameDir && Directory.Exists(execPluginDir))
				{
					searchDirectories.Add(execPluginDir);
				}

				return searchDirectories;
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
		public IEnumerable<Assembly> LoadedAssemblies
		{
			get { return AppDomain.CurrentDomain.GetAssemblies(); }
		}

		public Assembly LoadAssembly(string assemblyPath)
		{
			// Due to complex dependency resolve situations intertwined with our hot-reloadable
			// plugin system, we should manually resolve all dependencies. This is only possible
			// when obscuring where a certain Assembly has been loaded from. We need to load them
			// all as an anonymous data block to circumvent system dependency resolve.

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
			// Log environment specs for diagnostic purposes.
			// Even though more fitted for the system backend, we'll
			// do this here, because the plugin loader available much 
			// sooner, and more reliably.
			{
				string osName = Environment.OSVersion != null ? Environment.OSVersion.ToString() : "Unknown";
				string osFriendlyName = "Other";

				switch (Environment.OSVersion.Platform)
				{
					case PlatformID.Win32NT:
						if (Environment.OSVersion.Version >= new Version(6, 2, 0))
							osFriendlyName = "Windows 8 or higher";
					else if (Environment.OSVersion.Version >= new Version(6, 1, 0))
						osFriendlyName = "Windows 7";
					else if (Environment.OSVersion.Version >= new Version(6, 0, 0))
						osFriendlyName = "Windows Vista";
					else if (Environment.OSVersion.Version >= new Version(5, 2, 0))
						osFriendlyName = "Windows XP 64 Bit Edition";
					else if (Environment.OSVersion.Version >= new Version(5, 1, 0))
						osFriendlyName = "Windows XP";
					else if (Environment.OSVersion.Version >= new Version(5, 0, 0))
						osFriendlyName = "Windows 2000";
						break;

					case PlatformID.MacOSX:
						osFriendlyName = "OSX";
						break;

					case PlatformID.Unix:
						osFriendlyName = "Generic UNIX";
						break;
				}

				StringBuilder envInfo = new StringBuilder();
				envInfo.AppendLine("Environment Info:");
				envInfo.AppendFormat("  Current Directory: {0}", Environment.CurrentDirectory).AppendLine();
				envInfo.AppendFormat("  Command Line: {0}", Environment.CommandLine).AppendLine();
				envInfo.AppendFormat("  Operating System: {0} ({1})", osFriendlyName, osName).AppendLine();
				envInfo.AppendFormat("  64 Bit OS: {0}", Environment.Is64BitOperatingSystem).AppendLine();
				envInfo.AppendFormat("  64 Bit Process: {0}", Environment.Is64BitProcess).AppendLine();
				envInfo.AppendFormat("  CLR Version: {0}", Environment.Version).AppendLine();
				envInfo.AppendFormat("  Processor Count: {0}", Environment.ProcessorCount).AppendLine();

				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					envInfo.AppendFormat("  Processor Type: {0}", Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")).AppendLine();
					envInfo.AppendFormat("  Processor Architecture: {0}", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")).AppendLine();
				}

				Logs.Core.Write(envInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray()));
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
			
			// Admit that we didn't find anything - unless it's a resource Assembly, which
			// is used for WinForms localization. Not finding them is the default / expected.
			bool isResourceAssembly = false;
			if (args.Name != null)
			{
				string token = ".resources";
				int index = args.Name.IndexOf(token);
				int pastEndIndex = index + token.Length;
				if (index != -1 && (pastEndIndex >= args.Name.Length || args.Name[pastEndIndex] == ','))
				{
					isResourceAssembly = true;
				}
			}
			if (!isResourceAssembly)
			{
				if (args.RequestingAssembly != null)
				{
					Logs.Core.WriteWarning(
						"Can't resolve Assembly '{0}' (as requested by '{1}'): None of the available assembly paths matches the requested name.",
						args.Name,
						LogFormat.Assembly(args.RequestingAssembly));
				}
				else
				{
					Logs.Core.WriteWarning(
						"Can't resolve Assembly '{0}': None of the available assembly paths matches the requested name.",
						args.Name);
				}
			}
			return null;
		}
		private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			if (this.AssemblyLoaded != null)
				this.AssemblyLoaded(this, new AssemblyLoadedEventArgs(args.LoadedAssembly));

			Logs.Core.Write("Assembly loaded: {0}", LogFormat.Assembly(args.LoadedAssembly));
		}
	}
}
