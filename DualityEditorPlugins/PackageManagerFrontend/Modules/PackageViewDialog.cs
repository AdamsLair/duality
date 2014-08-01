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
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.PackageManagement;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;
using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public partial class PackageViewDialog : Form
	{
		public enum DisplayMode
		{
			None,

			Installed,
			Online
		}
		private class FilterBoxItem
		{
			private DisplayMode	display;
			private	string		name;

			public DisplayMode Display
			{
				get { return this.display; }
			}
			public string Name
			{
				get { return this.name; }
			}

			public FilterBoxItem(DisplayMode display, string name)
			{
				this.display = display;
				this.name = name;
			}

			public override string ToString()
			{
				return this.name;
			}
		}

		private	PackageManager	packageManager	= null;
		private	DisplayMode		display			= DisplayMode.None;
		private ITreeModel		modelInstalled	= null;
		private ITreeModel		modelOnline		= null;

		public DisplayMode Display
		{
			get { return this.display; }
			set
			{
				if (this.display != value)
				{
					DisplayMode previous = this.display;
					this.display = value;
					this.OnDisplayModeChanged(previous, this.display);
				}
			}
		}

		public PackageViewDialog()
		{
			this.InitializeComponent();

			this.treeColumnName.DrawColHeaderBg			+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnVersion.DrawColHeaderBg		+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnDownloads.DrawColHeaderBg	+= this.treeColumn_DrawColHeaderBg;
			this.nodeTextBoxName.DrawText				+= this.nodeTextBoxName_DrawText;
			this.nodeTextBoxVersion.DrawText			+= this.nodeTextBoxVersion_DrawText;
			this.nodeTextBoxDownloads.DrawText			+= this.nodeTextBoxDownloads_DrawText;
			this.toolStripMain.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();

			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Installed, PackageManagerFrontendRes.ItemName_InstalledPackages));
			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Online, PackageManagerFrontendRes.ItemName_OnlineRepository));
			
		}

		internal void SaveUserData(XElement node) {}
		internal void LoadUserData(XElement node) {}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.packageManager = DualityEditorApp.PackageManager;
			this.modelInstalled	= new InstalledPackagesTreeModel(this.packageManager);
			this.modelOnline	= new TreeModel();

			this.Display = DisplayMode.Installed;
		}

		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private void toolStripFilterBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterBoxItem selectedItem = this.toolStripFilterBox.SelectedItem as FilterBoxItem;
			this.Display = selectedItem.Display;
		}
		private void packageList_ColumnClicked(object sender, TreeColumnEventArgs e)
		{
			// ToDo: Implement User Sorting
			// May require SortedTreeModel wrapper or similar
		}
		private void nodeTextBoxName_DrawText(object sender, DrawTextEventArgs e)
		{
			e.TextColor = this.packageList.ForeColor;
		}
		private void nodeTextBoxVersion_DrawText(object sender, DrawTextEventArgs e)
		{
			PackageItem packageItem = e.Node.Tag as PackageItem;
			if (packageItem != null && packageItem.PackageInfo != null&& packageItem.NewestPackageInfo != null)
			{
				if (packageItem.NewestPackageInfo.Version == packageItem.PackageInfo.Version)
					e.BackgroundBrush = new SolidBrush(Color.FromArgb(32, 160, 255, 0));
				else
					e.BackgroundBrush = new SolidBrush(Color.FromArgb(32, 255, 160, 0));
			}
			else
			{
				e.BackgroundBrush = null;
			}
			e.TextColor = this.packageList.ForeColor;
		}
		private void nodeTextBoxDownloads_DrawText(object sender, DrawTextEventArgs e)
		{
			e.TextColor = this.packageList.ForeColor;
		}

		private void OnDisplayModeChanged(DisplayMode prev, DisplayMode next)
		{
			FilterBoxItem selectedItem = this.toolStripFilterBox.SelectedItem as FilterBoxItem;
			if (selectedItem == null || next != selectedItem.Display)
			{
				this.toolStripFilterBox.SelectedItem = this.toolStripFilterBox.Items.OfType<FilterBoxItem>().FirstOrDefault(i => i.Display == next);
			}

			if (next == DisplayMode.Installed)
			{
				this.packageList.Model = this.modelInstalled;
			}
			else if (next == DisplayMode.Online)
			{
				this.packageList.Model = this.modelOnline;
			}
		}
	}
}
