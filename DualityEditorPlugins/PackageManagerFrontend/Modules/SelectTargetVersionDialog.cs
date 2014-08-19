using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public partial class SelectTargetVersionDialog : Form
	{
		public Version SelectedVersion
		{
			get
			{
				Version result;
				if (!Version.TryParse(this.targetVersion.Text, out result))
					return null;
				else
					return new Version(result.Major, result.Minor, result.Build == -1 ? 0 : result.Build, result.Revision == -1 ? 0 : result.Revision);
			}
			set
			{
				this.targetVersion.Text = value != null ? value.ToString() : string.Empty;
			}
		}

		public SelectTargetVersionDialog()
		{
			this.InitializeComponent();
		}

		private void targetVersion_TextChanged(object sender, EventArgs e)
		{
			this.buttonOk.Enabled = this.SelectedVersion != null;
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
