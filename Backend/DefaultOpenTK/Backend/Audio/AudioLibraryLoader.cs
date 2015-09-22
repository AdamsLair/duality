using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Duality.Backend.DefaultOpenTK
{
	internal static class AudioLibraryLoader
	{
		public static void LoadAudioLibrary()
		{
			// Not running on Windows? External libraries will be handled by Mono / OpenTK anyway.
			bool isWindows = 
				Environment.OSVersion.Platform == PlatformID.Win32NT ||
				Environment.OSVersion.Platform == PlatformID.Win32S ||
				Environment.OSVersion.Platform == PlatformID.Win32Windows ||
				Environment.OSVersion.Platform == PlatformID.WinCE;
			if (!isWindows) return;

			// Determine working data
			Assembly execAssembly = Assembly.GetEntryAssembly() ?? typeof(DualityApp).Assembly;
			string execAssemblyDir = Path.GetFullPath(Path.GetDirectoryName(execAssembly.Location));
			string sourceFileName32 = Path.Combine(DualityApp.PluginDirectory, "OpenALSoft32.dll");
			string sourceFileName64 = Path.Combine(DualityApp.PluginDirectory, "OpenALSoft64.dll");
			string targetFileName = "OpenAL32.dll";

			// Determine the location of fallback libraries
			string sourceFilePath32 = sourceFileName32;
			string sourceFilePath64 = sourceFileName64;
			if (!File.Exists(sourceFilePath32)) sourceFilePath32 = Path.Combine(execAssemblyDir, sourceFileName32);
			if (!File.Exists(sourceFilePath64)) sourceFilePath64 = Path.Combine(execAssemblyDir, sourceFileName64);

			// No fallback libraries present? Nothing to do here.
			if (!File.Exists(sourceFilePath64) && !File.Exists(sourceFilePath32)) return;

			bool openALInstalled = 
				CheckOpenALDriverPath(Path.Combine(Environment.SystemDirectory, targetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), targetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), targetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), targetFileName));

			// An OpenAL driver is installed. Uninstall the software fallback.
			if (openALInstalled)
			{
				UninstallSoftwareFallback(targetFileName);
			}
			// No OpenAL driver found? Install the software fallback.
			else
			{
				InstallSoftwareFallback(sourceFilePath32, sourceFilePath64, targetFileName);
			}
		}
		public static void UnloadAudioLibrary()
		{
		}

		private static bool CheckOpenALDriverPath(string path)
		{
			if (File.Exists(path))
			{
				Log.Core.Write("OpenAL Drivers installed in {0}", path);
				return true;
			}
			else
			{
				return false;
			}
		}
		[System.Diagnostics.DebuggerNonUserCode]
		private static void InstallSoftwareFallback(string sourceFilePath32, string sourceFilePath64, string targetFilePath)
		{
			Log.Core.Write("OpenAL Drivers not found. Using {0} software fallback.", Environment.Is64BitProcess ? "64 Bit" : "32 Bit");
			if (Environment.Is64BitProcess)
			{
				if (!File.Exists(sourceFilePath64)) return;
				try
				{
					File.Copy(sourceFilePath64, targetFilePath, true);
				}
				catch (Exception) {}
			}
			else
			{
				if (!File.Exists(sourceFilePath32)) return;
				try
				{
					File.Copy(sourceFilePath32, targetFilePath, true);
				}
				catch (Exception) {}
			}
		}
		[System.Diagnostics.DebuggerNonUserCode]
		private static void UninstallSoftwareFallback(string targetFilePath)
		{
			try
			{
				if (File.Exists(targetFilePath)) File.Delete(targetFilePath);
			}
			catch (Exception) {}
		}
	}
}
