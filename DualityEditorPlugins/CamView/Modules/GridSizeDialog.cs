using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duality.Editor.Plugins.CamView
{
	public partial class GridSizeDialog : Form
	{
		public Vector3 GridSize
		{
			get
			{
				return new Vector3(
					this.activeX.Checked ? (float)this.editorX.Value : 0.0f,
					this.activeY.Checked ? (float)this.editorY.Value : 0.0f,
					this.activeZ.Checked ? (float)this.editorZ.Value : 0.0f);
			}
			set
			{
				this.activeX.Checked = value.X > 0.001f;
				this.activeY.Checked = value.Y > 0.001f;
				this.activeZ.Checked = value.Z > 0.001f;
				this.editorX.Value = Math.Max((int)value.X, 1);
				this.editorY.Value = Math.Max((int)value.Y, 1);
				this.editorZ.Value = Math.Max((int)value.Z, 1);
			}
		}

		public GridSizeDialog()
		{
			this.InitializeComponent();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void activeX_CheckedChanged(object sender, EventArgs e)
		{
			this.editorX.Enabled = this.activeX.Checked;
		}
		private void activeY_CheckedChanged(object sender, EventArgs e)
		{
			this.editorY.Enabled = this.activeY.Checked;
		}
		private void activeZ_CheckedChanged(object sender, EventArgs e)
		{
			this.editorZ.Enabled = this.activeZ.Checked;
		}
	}
}
