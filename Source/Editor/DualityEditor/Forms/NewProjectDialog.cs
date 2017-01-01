﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

using Aga.Controls;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality;
using Duality.IO;

using Duality.Editor.Controls.TreeModels.FileSystem;
using Duality.Editor.Properties;

namespace Duality.Editor.Forms
{
	public partial class NewProjectDialog : Form
	{
		private	FolderBrowserTreeModel	folderModel				= null;
		private	string					selectedTemplatePath	= null;
		private	ProjectTemplateInfo		selectedTemplate		= null;
		private	ProjectTemplateInfo		templateEmpty			= null;
		private	ProjectTemplateInfo		templateCurrent			= null;
		private	string					resultEditorBinary		= null;
		private	Color					defaultTextBoxBackColor;

		public string ResultEditorBinary
		{
			get { return this.resultEditorBinary; }
		}

		public NewProjectDialog()
		{
			this.InitializeComponent();
			this.templateView_Resize(this, EventArgs.Empty); // Trigger update tile size

			this.defaultTextBoxBackColor = this.textBoxFolder.BackColor;

			this.folderModel = new FolderBrowserTreeModel(EditorHelper.GlobalProjectTemplateDirectory);
			this.folderModel.Filter = s => Directory.Exists(s); // Only show directories
			this.folderView.Model = this.folderModel;
			this.folderViewControlName.DrawText += this.folderViewControlName_DrawText;

			this.selectedTemplatePath = this.folderModel.BasePath;

			// Create hardcoded templates
			this.templateEmpty = new ProjectTemplateInfo();
			this.templateEmpty.Icon = GeneralResCache.ImageTemplateEmpty;
			this.templateEmpty.Name = GeneralRes.Template_Empty_Name;
			this.templateEmpty.Description = GeneralRes.Template_Empty_Desc;
			this.templateEmpty.SpecialTag = ProjectTemplateInfo.SpecialInfo.Empty;

			this.templateCurrent = new ProjectTemplateInfo();
			this.templateCurrent.Icon = GeneralResCache.ImageTemplateCurrent;
			this.templateCurrent.Name = GeneralRes.Template_Current_Name;
			this.templateCurrent.Description = GeneralRes.Template_Current_Desc;
			this.templateCurrent.SpecialTag = ProjectTemplateInfo.SpecialInfo.Current;

			// Hilde folder selector, if empty
			if (!Directory.Exists(this.folderModel.BasePath) || Directory.GetDirectories(this.folderModel.BasePath).Length == 0)
			{
				this.folderView.Enabled = false;
				this.splitFolderTemplate.Panel1Collapsed = true;
			}

			this.UpdateTemplateList();
		}

		protected void UpdateTemplateList()
		{
			this.templateView.BeginUpdate();
			this.templateView.Items.Clear();
			this.imageListTemplateView.Images.Clear();

			// Scan for template files
			string[] templateFiles = Directory.Exists(this.selectedTemplatePath) ? Directory.GetFiles(this.selectedTemplatePath, "*.zip", SearchOption.TopDirectoryOnly) : new string[0];
			List<ProjectTemplateInfo> templateEntries = new List<ProjectTemplateInfo>();
			foreach (string templateFile in templateFiles)
			{
				try
				{
					ProjectTemplateInfo entry = new ProjectTemplateInfo(templateFile);
					templateEntries.Add(entry);
				}
				catch (Exception e)
				{
					Logs.Editor.WriteError("Can't load project template {0} because an error occurred in the process: {1}", templateFile, LogFormat.Exception(e));
				}
			}

			// Add hardcoded templates
			if (this.selectedTemplatePath == this.folderModel.BasePath)
			{
				templateEntries.Insert(0, this.templateCurrent);
				templateEntries.Insert(0, this.templateEmpty);
			}

			// Add template entries to view
			foreach (ProjectTemplateInfo entry in templateEntries)
			{
				Bitmap icon = entry.Icon;
				if (icon != null)
				{
					if (icon.Size != this.imageListTemplateView.ImageSize)
						icon = icon.Rescale(this.imageListTemplateView.ImageSize.Width, this.imageListTemplateView.ImageSize.Height);
					this.imageListTemplateView.Images.Add(entry.FilePath ?? entry.Name, icon);
				}

				ListViewItem item = new ListViewItem(new string[] { entry.Name, entry.Description }, entry.FilePath ?? entry.Name);
				item.Tag = entry;
				item.ToolTipText = entry.Description;
				this.templateView.Items.Add(item);
			}

			this.templateView.Sort();
			this.templateView.EndUpdate();
		}
		protected bool CheckInputValid()
		{
			Color errorColor = this.defaultTextBoxBackColor.MixWith(Color.Red, 0.25f, true);

			this.textBoxFolder.BackColor = this.defaultTextBoxBackColor;
			this.textBoxTemplate.BackColor = this.defaultTextBoxBackColor;
			this.textBoxName.BackColor = this.defaultTextBoxBackColor;

			if (this.selectedTemplate == null)
			{
				this.textBoxTemplate.BackColor = errorColor;
				return false;
			}
			if (string.IsNullOrWhiteSpace(this.textBoxName.Text) || !PathHelper.IsPathValid(this.textBoxName.Text))
			{
				this.textBoxName.BackColor = errorColor;
				return false;
			}
			if (string.IsNullOrWhiteSpace(this.textBoxFolder.Text) || !PathHelper.IsPathValid(this.textBoxFolder.Text))
			{
				this.textBoxFolder.BackColor = errorColor;
				return false;
			}
			
			string targetDir = Path.Combine(this.textBoxFolder.Text, this.textBoxName.Text);
			if (Directory.Exists(targetDir))
			{
				if (Directory.EnumerateFiles(targetDir, "*", SearchOption.AllDirectories).Any())
				{
					this.textBoxFolder.BackColor = errorColor;
					MessageBox.Show(
						GeneralRes.Msg_CreateProjectErrorTargetExists_Desc,
						GeneralRes.Msg_CreateProjectErrorTargetExists_Caption,
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
					return false;
				}
			}
			if (File.Exists(targetDir))
			{
				this.textBoxFolder.BackColor = errorColor;
				MessageBox.Show(
					GeneralRes.Msg_CreateProjectErrorTargetIsFilePath_Desc,
					GeneralRes.Msg_CreateProjectErrorTargetExists_Caption,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return false;
			}
			if (PathOp.IsPathLocatedIn(targetDir, Environment.CurrentDirectory))
			{
				this.textBoxFolder.BackColor = errorColor;
				MessageBox.Show(
					GeneralRes.Msg_CreateProjectErrorNestedFolder_Desc,
					GeneralRes.Msg_CreateProjectErrorNestedFolder_Caption,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return false;
			}

			return true;
		}

		private void folderViewControlName_DrawText(object sender, DrawTextEventArgs e)
		{
			e.TextColor = Color.Black;
		}

		private void templateView_Resize(object sender, EventArgs e)
		{
			this.templateView.TileSize = new Size(this.templateView.ClientSize.Width, this.templateView.TileSize.Height);
		}
		private void templateView_SelectedIndexChanged(object sender, EventArgs e)
		{
			ProjectTemplateInfo entry = this.templateView.SelectedItems.Count > 0 ? this.templateView.SelectedItems[0].Tag as ProjectTemplateInfo : null;
			if (entry == null) return;
			
			if (entry.FilePath == null)
				this.textBoxTemplate.Text = entry.Name;
			else
				this.textBoxTemplate.Text = entry.FilePath;
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			// Make sure the input is valid and display an error if not
			if (!this.CheckInputValid()) return;

			// Ask if the selected template should be copied to the template directory, if not located there (auto-install)
			if (this.selectedTemplate.SpecialTag == ProjectTemplateInfo.SpecialInfo.None && 
				!PathOp.IsPathLocatedIn(this.selectedTemplate.FilePath, EditorHelper.GlobalProjectTemplateDirectory))
			{
				DialogResult result = MessageBox.Show(
					Properties.GeneralRes.Msg_InstallNewTemplate_Desc,
					Properties.GeneralRes.Msg_InstallNewTemplate_Caption,
					MessageBoxButtons.YesNoCancel,
					MessageBoxIcon.Question);
				if (result == System.Windows.Forms.DialogResult.Cancel) return;
				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					if (!Directory.Exists(EditorHelper.GlobalProjectTemplateDirectory))
						Directory.CreateDirectory(EditorHelper.GlobalProjectTemplateDirectory);
					File.Copy(
						this.selectedTemplate.FilePath, 
						Path.Combine(EditorHelper.GlobalProjectTemplateDirectory, Path.GetFileName(this.selectedTemplate.FilePath)));
				}
			}

			// Create a new project
			try
			{
				this.resultEditorBinary = EditorHelper.CreateNewProject(this.textBoxName.Text, this.textBoxFolder.Text, this.selectedTemplate);
			}
			catch (Exception exception)
			{
				this.resultEditorBinary = null;
				MessageBox.Show(
					string.Format(Properties.GeneralRes.Msg_ErrorCantCreateProject_Desc, LogFormat.Exception(exception), Environment.NewLine),
					Properties.GeneralRes.Msg_ErrorCantCreateProject_Caption, 
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// Close successfully
			this.DialogResult = this.resultEditorBinary != null ? DialogResult.OK : DialogResult.Cancel;
			this.Close();
		}

		private void buttonBrowseTemplate_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.CheckFileExists = true;
			fileDialog.CheckPathExists = true;
			fileDialog.Multiselect = false;
			fileDialog.Title = Properties.GeneralRes.OpenTemplateDialog_Title;
			fileDialog.RestoreDirectory = true;
			fileDialog.InitialDirectory = Environment.CurrentDirectory;
			fileDialog.AddExtension = true;
			fileDialog.Filter = Properties.GeneralRes.OpenTemplateDialog_Filters;

			DialogResult result = fileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				this.textBoxTemplate.Text = Path.GetFullPath(fileDialog.FileName);
			}
		}
		private void buttonBrowseFolder_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.ShowNewFolderButton = true;
			folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			folderDialog.Description =Properties.GeneralRes.SelectNewProjectFolderDialog_Desc;
			
			DialogResult result = folderDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				this.textBoxFolder.Text = Path.GetFullPath(folderDialog.SelectedPath);
			}
		}

		private void textBoxTemplate_TextChanged(object sender, EventArgs e)
		{
			if (this.textBoxTemplate.Text == this.templateEmpty.Name)
				this.selectedTemplate = this.templateEmpty;
			else if (this.textBoxTemplate.Text == this.templateCurrent.Name)
				this.selectedTemplate = this.templateCurrent;
			else
			{
				try { this.selectedTemplate = new ProjectTemplateInfo(this.textBoxTemplate.Text); } 
				catch (Exception) { this.selectedTemplate = null; }
			}
		}

		private void folderView_SelectionChanged(object sender, EventArgs e)
		{
			FolderItem folderItem = this.folderView.SelectedNode != null ? this.folderView.SelectedNode.Tag as FolderItem : null;
			this.selectedTemplatePath = folderItem != null ? folderItem.ItemPath : this.folderModel.BasePath;
			this.UpdateTemplateList();
		}
	}
}
