using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Aga.Controls.Tree;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public partial class PackageViewDialog : Form
	{
		public PackageViewDialog()
		{
			this.InitializeComponent();
			this.treeColumnName.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.treeColumnVersion.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.treeColumnDownloads.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.toolStripMain.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		internal void SaveUserData(XElement node) {}
		internal void LoadUserData(XElement node) {}

		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
	}
}
