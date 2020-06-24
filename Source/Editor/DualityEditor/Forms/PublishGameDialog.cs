using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Compression;

using Duality.IO;

using Duality.Editor.Properties;

namespace Duality.Editor.Forms
{
	public partial class PublishGameDialog : Form
	{
		private static readonly string[] TemporaryProjectFiles = new string[] {
			@"*\.git",
			@"*\.svn",
			@".\Backup",
			@".\Temp",
			$@".\{EditorHelper.ImportDirectory}",
			@".\logfile*",
			@".\perflog*",
			@"*.vshost.*",
			@".\OpenAL32.dll" };
		private static readonly string[] SourcePathBlacklist = new string[] {
			$@".\{EditorHelper.SourceDirectory}",
			@"*.pdb",
			@"*.sln",
			@".\FarseerDuality.xml",
			@".\NVorbis.xml" };
		private static readonly string[] EditorPathBlacklist = new string[] {
			@".\Plugins\*.xml",
			@".\Plugins\*.editor.dll",
			@".\DualityEditor.*",
			@".\Duality.xml",
			@".\DualityPrimitives.xml",
			@".\DualityPhysics.xml",
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
			@".\Plugins\OpenTK.GLControl.*"};
		private static readonly Regex[] RegExTemporaryProjectFiles = TemporaryProjectFiles.Select(w => PathWildcardToRegex(w)).ToArray();
		private static readonly Regex[] RegExSourcePathBlacklist = SourcePathBlacklist.Select(w => PathWildcardToRegex(w)).ToArray();
		private static readonly Regex[] RegExEditorPathBlacklist = EditorPathBlacklist.Select(w => PathWildcardToRegex(w)).ToArray();

		private	Color	defaultTextBoxBackColor;

		public PublishGameDialog()
		{
			this.InitializeComponent();
			this.defaultTextBoxBackColor = this.textboxFolderPath.BackColor;
		}
		
		protected bool CheckInputValid()
		{
			Color errorColor = this.defaultTextBoxBackColor.MixWith(Color.Red, 0.25f, true);

			this.textboxFolderPath.BackColor = this.defaultTextBoxBackColor;
			
			if (string.IsNullOrWhiteSpace(this.textboxFolderPath.Text) || !PathHelper.IsPathValid(this.textboxFolderPath.Text))
			{
				this.textboxFolderPath.BackColor = errorColor;
				return false;
			}

			if (PathOp.ArePathsEqual(this.textboxFolderPath.Text, Environment.CurrentDirectory) ||
				PathOp.IsPathLocatedIn(this.textboxFolderPath.Text, Environment.CurrentDirectory))
			{
				this.textboxFolderPath.BackColor = errorColor;
				MessageBox.Show(
					GeneralRes.Msg_PublishProjectErrorNestedFolder_Desc,
					GeneralRes.Msg_PublishProjectErrorNestedFolder_Caption,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return false;
			}

			return true;
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
			// Make sure the input is valid and display an error if not
			if (!this.CheckInputValid()) return;

			if (!Directory.Exists(textboxFolderPath.Text))
				Directory.CreateDirectory(textboxFolderPath.Text);

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
			string gameDirName = PathOp.GetValidFileName(DualityApp.AppData.AppName);
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

				// Exclude hidden files and folders
				if (!PathHelper.IsPathVisible(path))
				{
					string fileName = Path.GetFileName(path);
					if (!string.Equals(fileName, "desktop.ini", StringComparison.InvariantCultureIgnoreCase) &&
						!string.Equals(fileName, "WorkingFolderIcon.ico", StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}

				// Exclude temporary files
				if (RegExTemporaryProjectFiles.Any(entry => entry.IsMatch(matchPath)))
				{
					return false;
				}
				else if (!includeSource && RegExSourcePathBlacklist.Any(entry => entry.IsMatch(matchPath)))
				{
					return false;
				}
				else if (!includeEditor && RegExEditorPathBlacklist.Any(entry => entry.IsMatch(matchPath)))
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
					"cd GameData && start " + PathHelper.MakeFilePathRelative(DualityEditorApp.ProjectSettings.LauncherPath));

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
				using (FileStream archiveStream = File.Open(archivePath, FileMode.Create))
				using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
				{
					archive.AddDirectory(archiveBaseDir);
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
		private static Regex PathWildcardToRegex(string pattern)
		{
			string regExString = "^" + Regex.Escape(pattern) + "$";
			regExString = Regex.Replace(regExString, @"(\\\\|/)", @"[\\/]");
			regExString = regExString.Replace(@"\*", @".*");
			regExString = regExString.Replace(@"\?", @".");
			Regex regEx = new Regex(regExString, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
			return regEx;
		}
	}
}
