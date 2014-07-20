using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Ionic.Zip;
using Duality.Editor.Properties;

namespace Duality.Editor.Forms
{
	public partial class PublishGameDialog : Form
	{
		private static readonly string[] TemporaryProjectFiles = new string[] {
			@".\Backup",
			@".\Source\Media",
			@".\logfile*",
			@".\perflog*",
			@"*.vshost.*",
			@".\OpenAL32.dll" };
		private static readonly string[] SourcePathBlacklist = new string[] {
			@".\Source",
			@"*.pdb",
			@".\FarseerDuality.xml",
			@".\NVorbis.xml",
			@".\OpenTK.GLControl.xml" };
		private static readonly string[] EditorPathBlacklist = new string[] {
			@".\Plugins\*.xml",
			@".\Plugins\*.editor.dll",
			@".\DualityEditor.*",
			@".\Duality.xml",
			@".\OpenTK.xml",
			@".\DDoc.chm",
			@".\" + DualityEditorApp.UserDataFile,
			@".\" + DualityEditorApp.DesignTimeDataFile,
			@".\AdamsLair.WinForms.*",
			@".\Aga.Controls.*",
			@".\VistaBridgeLibrary.*",
			@".\WeifenLuo.WinFormsUI.Docking.*",
			@".\Windows7.DesktopIntegration.*",
			@".\PopupControl.*",
			@".\DualityUpdater.*",
			@".\NuGet.Core.*",
			@".\Ionic.Zip.*",
			@".\OpenTK.GLControl.*",
			@".\PackageConfig.xml" };


		public PublishGameDialog()
		{
			this.InitializeComponent();
		}
		
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			publishFolder.SelectedPath = Environment.CurrentDirectory;
			if (publishFolder.ShowDialog() == DialogResult.OK)
			{
				textboxFolderPath.Text = publishFolder.SelectedPath;
			}
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void buttonPublish_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(textboxFolderPath.Text))
			{
				MessageBox.Show(GeneralRes.Msg_ErrorDirectoryNotFound_Desc, GeneralRes.Msg_ErrorDirectoryNotFound_Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			PublishProject(
				textboxFolderPath.Text, 
				checkboxSource.Checked, 
				checkboxEditor.Checked, 
				checkboxCompress.Checked, 
				checkBoxShortcut.Checked,
				targetDir => MessageBox.Show(
					string.Format(GeneralRes.Msg_PublishConfirmDeleteTargetDir_Desc, Path.GetFileName(targetDir)), 
					GeneralRes.Msg_PublishConfirmDeleteTargetDir_Caption, 
					MessageBoxButtons.OKCancel, 
					MessageBoxIcon.Warning) == DialogResult.OK);
			this.Close();
		}

		public static void PublishProject(string targetDir, bool includeSource, bool includeEditor, bool compress, bool createShortcuts, Func<string,bool> targetExistsCallback = null)
		{
			// Determine a valid directory name for the game
			string gameDirName = PathHelper.GetValidFileName(DualityApp.AppData.AppName);
			string targetGameDir = Path.Combine(targetDir, gameDirName);
			string archiveBaseDir = targetGameDir;

			// If we're creating shortcuts, move everything into a distinct subfolder to hide it from the user
			if (createShortcuts)
			{
				targetGameDir = Path.Combine(targetGameDir, "GameData");
			}

			// Make sure everything is saved before copying stuff
			DualityEditorApp.SaveAllProjectData();

			// If the dynamically created target directory already exists, delete it.
			if (Directory.Exists(archiveBaseDir))
			{
				bool empty = !Directory.EnumerateFiles(archiveBaseDir).Any();
				if (!empty && targetExistsCallback == null)
				{
					throw new ArgumentException("The target directory already contains a non-empty folder named '" + gameDirName + "'.", "targetDir");
				}
				else if (empty || targetExistsCallback(archiveBaseDir))
				{
					Directory.Delete(archiveBaseDir, true);
				}
				else
				{
					return;
				}
			}

			// Create the target directories
			Directory.CreateDirectory(archiveBaseDir);
			Directory.CreateDirectory(targetGameDir);

			// Copy files to the target directory
			PathHelper.CopyDirectory(Environment.CurrentDirectory, targetGameDir, true, delegate (string path)
			{
				string matchPath = Path.Combine(".", PathHelper.MakeFilePathRelative(path));

				// Exclude temporary files
				if (TemporaryProjectFiles.Any(entry => Regex.IsMatch(matchPath, PathWildcardToRegex(entry), RegexOptions.IgnoreCase)))
				{
					return false;
				}
				else if (!includeSource && SourcePathBlacklist.Any(entry => Regex.IsMatch(matchPath, PathWildcardToRegex(entry), RegexOptions.IgnoreCase)))
				{
					return false;
				}
				else if (!includeEditor && EditorPathBlacklist.Any(entry => Regex.IsMatch(matchPath, PathWildcardToRegex(entry), RegexOptions.IgnoreCase)))
				{
					return false;
				}

				return true;
			});

			// Create shortcuts when requested
			if (createShortcuts)
			{
				// Create the shortcut to the game
				string shortcutFilePath = Path.Combine(archiveBaseDir, gameDirName + ".bat");
				File.WriteAllText(
					shortcutFilePath,
					"cd GameData && start " + DualityEditorApp.LauncherAppPath);

				// Create a shortcut to the editor
				if (includeEditor)
				{
					File.WriteAllText(
						Path.Combine(archiveBaseDir, gameDirName + " Editor.bat"),
						"cd GameData && start DualityEditor.exe");
				}
			}

			// Compress the directory
			if (compress)
			{
				string archivePath = Path.Combine(targetDir, gameDirName + ".zip");
				using (ZipFile archive = new ZipFile())
				{
					archive.AddDirectory(targetGameDir);
					archive.Save(archivePath);
				}
				Directory.Delete(archiveBaseDir, true);

				// Display compressed file to the user
				EditorHelper.ShowInExplorer(archivePath);
			}
			else
			{
				// Display directory to user
				EditorHelper.ShowInExplorer(targetGameDir);
			}

			return;
		}
		private static string PathWildcardToRegex(string pattern)
		{
			string regEx = "^" + Regex.Escape(pattern) + "$";
			regEx = Regex.Replace(regEx, @"(\\\\|/)", @"[\\/]");
			regEx = regEx.Replace(@"\*", @".*");
			regEx = regEx.Replace(@"\?", @".");
			return regEx;
		}
	}
}
