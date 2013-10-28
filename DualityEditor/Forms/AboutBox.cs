using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

using Duality;

namespace DualityEditor.Forms
{
	partial class AboutBox : Form
	{
		public AboutBox()
		{
			this.InitializeComponent();
			FileVersionInfo versionCore = FileVersionInfo.GetVersionInfo(typeof(DualityApp).Assembly.Location);
			FileVersionInfo versionEditor = FileVersionInfo.GetVersionInfo(typeof(DualityEditorApp).Assembly.Location);
			FileVersionInfo versionLauncher;
			if (File.Exists(EditorHelper.DualityLauncherExecFile))
				versionLauncher = FileVersionInfo.GetVersionInfo(EditorHelper.DualityLauncherExecFile);
			else
				versionLauncher = null;
			this.labelVersionData.Text = string.Format(this.labelVersionData.Text, 
				versionCore.FileVersion,
				versionEditor.FileVersion,
				(versionLauncher != null) ? versionLauncher.FileVersion : "unavailable");
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.linkLabelWebsite.Text);
		}
		private void linkLabelDevWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.linkLabelDevWebsite.Text);
		}
		private void labelVersion_Click(object sender, EventArgs e)
		{
			System.Media.SystemSounds.Asterisk.Play();
			Clipboard.Clear();
			Clipboard.SetText(this.labelVersion.Text);
		}
	}
}
