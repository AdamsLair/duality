using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace Duality
{
	internal static class AudioLibraryLoader
	{
		private const string SourceFileName32 = "OpenALSoft32.dll";
		private const string SourceFileName64 = "OpenALSoft64.dll";
		private const string TargetFileName = "OpenAL32.dll";

		public static void LoadAudioLibrary()
		{
			// Not running on Windows? External libraries will be handled by Mono / OpenTK anyway.
			bool isWindows = 
				Environment.OSVersion.Platform == PlatformID.Win32NT ||
				Environment.OSVersion.Platform == PlatformID.Win32S ||
				Environment.OSVersion.Platform == PlatformID.Win32Windows ||
				Environment.OSVersion.Platform == PlatformID.WinCE;
			if (!isWindows) return;

			// No fallback libraries present? Nothing to do here.
			if (!File.Exists(SourceFileName64) && !File.Exists(SourceFileName32)) return;

			bool openALInstalled = 
				CheckOpenALDriverPath(Path.Combine(Environment.SystemDirectory, TargetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), TargetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), TargetFileName)) ||
				CheckOpenALDriverPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), TargetFileName));

			// An OpenAL driver is installed. Uninstall the software fallback.
			if (openALInstalled)
			{
				UninstallSoftwareFallback();
			}
			// No OpenAL driver found? Install the software fallback.
			else
			{
				InstallSoftwareFallback();
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
				UninstallSoftwareFallback();
				return true;
			}
			else
			{
				return false;
			}
		}
		private static void InstallSoftwareFallback()
		{
			Log.Core.Write("OpenAL Drivers not found. Using {0} software fallback.", Environment.Is64BitProcess ? "64 Bit" : "32 Bit");
			if (Environment.Is64BitProcess)
			{
				if (!File.Exists(SourceFileName64)) return;
				try
				{
					File.Copy(SourceFileName64, TargetFileName, true);
				}
				catch (Exception) {}
			}
			else
			{
				if (!File.Exists(SourceFileName32)) return;
				try
				{
					File.Copy(SourceFileName32, TargetFileName, true);
				}
				catch (Exception) {}
			}
		}
		private static void UninstallSoftwareFallback()
		{
			try
			{
				if (File.Exists(TargetFileName)) File.Delete(TargetFileName);
			}
			catch (Exception) {}
		}
	}
}
