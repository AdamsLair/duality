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
			Update,
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

		private	DisplayMode	display			= DisplayMode.None;
		private ITreeModel	modelInstalled	= null;
		private ITreeModel	modelUpdates	= null;
		private ITreeModel	modelOnline		= null;

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

			this.treeColumnName.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.treeColumnVersion.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.treeColumnDownloads.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.toolStripMain.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();

			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Installed, PackageManagerFrontendRes.ItemName_InstalledPackages));
			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Update, PackageManagerFrontendRes.ItemName_PackageUpdates));
			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Online, PackageManagerFrontendRes.ItemName_OnlineRepository));
			
		}

		internal void SaveUserData(XElement node) {}
		internal void LoadUserData(XElement node) {}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			PackageManager manager = DualityEditorApp.PackageManager;
			this.modelInstalled	= new InstalledPackagesTreeModel(manager);
			this.modelUpdates	= new TreeModel();
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
			else if (next == DisplayMode.Update)
			{
				this.packageList.Model = this.modelUpdates;
			}
			else if (next == DisplayMode.Online)
			{
				this.packageList.Model = this.modelOnline;
			}
		}
	}
}
