using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using Microsoft.Win32;

using Duality;
using Duality.IO;
using Duality.Serialization;

namespace Duality.Editor
{
	public static class EditorHelper
	{
		public static readonly string DualityLauncherExecFile = "DualityLauncher.exe";
		public static readonly string BackupDirectory = "Backup";
		public static readonly string RootDirectory = "..";

		public static readonly string ImportDirectory = Path.Combine(RootDirectory, "Import");
		public static readonly string SourceDirectory = Path.Combine(RootDirectory, "Source");

		public static readonly string GlobalUserDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Duality");


		/// <summary>
		/// The path to the *.sln solution file. Will throw a <see cref="FileNotFoundException"/> if the solution file cannot be found.
		/// </summary>
		public static string SourceCodeSolutionFilePath
		{
			get
			{
				string sourceCodeSolutionFilePath = null;
				if (Directory.Exists(SourceDirectory))
				{
					sourceCodeSolutionFilePath = Directory.EnumerateFiles(
						SourceDirectory,
						"*.sln",
						SearchOption.AllDirectories)
						.FirstOrDefault();
				}
				return sourceCodeSolutionFilePath ?? throw new FileNotFoundException($"Could not find a solution file in {SourceDirectory}");
			}
		}
		public static string CurrentProjectName
		{
			get
			{
				string dataFullPath = Path.GetFullPath(DualityApp.DataDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
				string dataDir = Path.GetDirectoryName(dataFullPath);
				return Path.GetFileName(dataDir);
			}
		}

		public static void ShowInExplorer(string filePath)
		{
			string fullPath = Path.GetFullPath(filePath);
			string argument = @"/select, " + fullPath;
			System.Diagnostics.Process.Start("explorer.exe", argument);
		}
		public static List<Form> GetZSortedAppWindows()
		{
			List<Form> result = new List<Form>();

			// Generate a list of handles of this application's top-level forms
			List<IntPtr> openFormHandles = new List<IntPtr>();
			foreach (Form form in Application.OpenForms)
			{
				if (!form.TopLevel)
				{
					bool belongsToAnother = Application.OpenForms.OfType<Form>().Contains(form.TopLevelControl);
					if (belongsToAnother)
						continue;
				}
				openFormHandles.Add(form.Handle);
			}

			// Use WinAPI to iterate over windows in correct z-order until we found all our forms
			IntPtr hwnd = NativeMethods.GetTopWindow((IntPtr)null);
			while (hwnd != IntPtr.Zero)
			{
				// Get next window under the current handle
				hwnd = NativeMethods.GetNextWindow(hwnd, NativeMethods.GW_HWNDNEXT);

				try
				{
					if (openFormHandles.Contains(hwnd))
					{
						Form frm = Form.FromHandle(hwnd) as Form;
						result.Add(frm);

						// Found all of our windows? No need to continue.
						if (result.Count == openFormHandles.Count)
							break;
					}
				}
				catch
				{
					// Weird behaviour: In some cases, trying to cast to a Form, a handle of an object 
					// that isn't a Form will just return null. In other cases, will throw an exception.
				}
			}

			return result;
		}
		public static Control GetFocusedControl()
		{
			IntPtr nativeFocusControl = NativeMethods.GetFocus();

			// Early-out if nothing is focused
			if (nativeFocusControl == IntPtr.Zero)
				return null;

			// Retrieve the managed Control reference and return it
			Control focusControl = Control.FromHandle(nativeFocusControl);
			return focusControl;
		}

		private class ImageOverlaySet
		{
			private Image baseImage;
			private Dictionary<Image, Image> overlayDict;

			public Image Base
			{
				get { return this.baseImage; }
			}

			public ImageOverlaySet(Image baseImage)
			{
				this.baseImage = baseImage;
				this.overlayDict = new Dictionary<Image, Image>(); ;
			}
			public Image GetOverlay(Image overlayImage)
			{
				Image baseWithOverlay;
				if (!this.overlayDict.TryGetValue(overlayImage, out baseWithOverlay))
				{
					baseWithOverlay = this.baseImage.Clone() as Image;
					using (Graphics g = Graphics.FromImage(baseWithOverlay))
					{
						g.DrawImageUnscaled(overlayImage, 0, 0);
					}
					this.overlayDict[overlayImage] = baseWithOverlay;
				}
				return baseWithOverlay;
			}
		}
		private static Dictionary<Image, ImageOverlaySet> overlayCache = new Dictionary<Image, ImageOverlaySet>();
		public static Image GetImageWithOverlay(Image baseImage, Image overlayImage)
		{
			ImageOverlaySet overlaySet;
			if (!overlayCache.TryGetValue(baseImage, out overlaySet))
			{
				overlaySet = new ImageOverlaySet(baseImage);
				overlayCache[baseImage] = overlaySet;
			}
			return overlaySet.GetOverlay(overlayImage);
		}
	}
}
