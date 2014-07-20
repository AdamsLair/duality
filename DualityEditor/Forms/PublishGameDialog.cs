using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Duality.Editor.Forms {
	public partial class PublishGameDialog : Form {
		public PublishGameDialog() {
			InitializeComponent();
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			this.Close();
		}

		private void buttonBrowse_Click(object sender, EventArgs e) {
			publishFolder.SelectedPath = Environment.CurrentDirectory;
			if (publishFolder.ShowDialog() == DialogResult.OK) {
				textboxFolderPath.Text = publishFolder.SelectedPath;
			}
		}

		private void buttonPublish_Click(object sender, EventArgs e) {
			if (!Directory.Exists(textboxFolderPath.Text)) {
				MessageBox.Show("The specified directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (String.IsNullOrEmpty(textboxTitle.Text)) {
				MessageBox.Show("Please enter a name for the game", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			PublishGame(textboxTitle.Text, textboxFolderPath.Text, checkboxSource.Checked, checkboxEditor.Checked, checkboxCompress.Checked);
			this.Close();
		}

		// The files that the game needs
		string[] gameFiles = new string[] {
			"AppData.dat",
			"DualityLauncher.exe",

			"Duality.dll",
			"FarseerDuality.dll",
			"NVorbis.dll",
			"OpenALSoft32.dll",
			"OpenALSoft64.dll",
			"OpenTK.dll",
			"OpenTK.GLControl.dll"
		};

		// The files that the editor needs
		string[] editorFiles = new string[] {
			"DualityUpdater.exe",
			"DesignTimeData.dat",
			"AdamsLair.WinForms.dll",
			"Aga.Controls.dll",
			"DualityEditor.exe",
			"Ionic.Zip.dll",
			"NuGet.Core.dll",
			"PopupControl.dll",
			"VistaBridgeLibrary.dll",
			"WeifenLuo.WinFormsUI.Docking.dll",
			"Windows7.DesktopIntegration.dll"
		};

		private void PublishGame(string title, string directory, bool includeSource, bool includeEditor, bool compress) {
			// Application path (source files)
			string basePath = Path.Combine(Application.StartupPath);

			// Hide files from enduser
			string fileDirectory = Path.Combine(directory, @"Files\");
			if (Directory.Exists(fileDirectory))
				CleanDirectory(fileDirectory);
			else Directory.CreateDirectory(fileDirectory);

			// copy all requested files
			try {
				CopyFiles(gameFiles, fileDirectory);
				CopyDirectory(Path.Combine(basePath, "Data"), Path.Combine(fileDirectory, "Data"), true);

				if (includeEditor) {
					CopyFiles(editorFiles, fileDirectory);
					CopyDirectory(Path.Combine(basePath, "Plugins"), Path.Combine(fileDirectory, "Plugins"), true);
				}

				if (includeSource)
					CopyDirectory(Path.Combine(basePath, "Source"), Path.Combine(fileDirectory, "Source"), true);

			} catch (Exception e) {
				MessageBox.Show("An error occured while trying to copy the game files\n" + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// Create the shortcut to the game
			File.WriteAllText(
				Path.Combine(directory, title + ".bat"),
				"cd Files && start DualityLauncher.exe");

			// If we have to, create a shortcut to the editor
			if (includeEditor)
				File.WriteAllText(
					Path.Combine(directory, "Duality Editor.bat"),
					"cd Files && start DualityEditor.exe");

			// compress the directory
			if (compress) {
				string filePath = Path.Combine(new DirectoryInfo(Path.Combine(directory, @"..\")).FullName, "Game.zip");
				using (ZipFile file = new ZipFile()) {
					file.AddDirectory(directory);
					file.Save(filePath);
				}
				Directory.Delete(directory, true);

				// display compressed file to user and stop
				Process.Start(filePath);
				return;
			}

			// display directory to user
			Process.Start(directory);
		}

		private void CopyFiles(string[] files, string destination) {
			foreach (string file in files) {
				File.Copy(file, Path.Combine(destination, file), true);
			}
		}

		// Taken from MSDN
		private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs) {
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the destination directory doesn't exist, create it. 
			if (!Directory.Exists(destDirName))
				Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files) {
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, true);
			}

			// If copying subdirectories, copy them and their contents to new location. 
			if (copySubDirs) {
				foreach (DirectoryInfo subdir in dirs) {
					string temppath = Path.Combine(destDirName, subdir.Name);
					CopyDirectory(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		private static void CleanDirectory(string name) {
			DirectoryInfo info = new DirectoryInfo(name);
			foreach (FileInfo file in info.GetFiles())
				file.Delete();

			foreach (DirectoryInfo directory in info.GetDirectories())
				directory.Delete(true);
		}
	}
}
