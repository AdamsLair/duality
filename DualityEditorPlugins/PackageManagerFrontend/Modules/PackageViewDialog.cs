using System;
using System.Collections;
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
		private class PackageListItemComparer : IComparer<BaseItem>
		{
			public enum SortMode
			{
				Name,
				Version,
				Downloads
			}

			private SortOrder sortOrder	= SortOrder.Ascending;
			private SortMode sortMode = SortMode.Name;

			public PackageListItemComparer(SortMode mode, SortOrder order)
			{
				this.sortMode = mode;
				this.sortOrder = order;
			}

			public int Compare(BaseItem x, BaseItem y)
			{
				PackageItem itemA = x as PackageItem;
				PackageItem itemB = y as PackageItem;

				if (itemA == itemB) return 0;
				if (itemA == null) return this.sortOrder == SortOrder.Ascending ? 1 : -1;
				if (itemB == null) return this.sortOrder == SortOrder.Ascending ? -1 : 1;

				int result = 0;
				if (result == 0 || this.sortMode == SortMode.Version)
				{
					if		(itemA.Version < itemB.Version)	result = -1;
					else if (itemA.Version > itemB.Version)	result = 1;
				}
				if (result == 0 || this.sortMode == SortMode.Downloads)
				{
					if (itemA.Downloads.HasValue || itemB.Downloads.HasValue)
					{
						if		(!itemA.Downloads.HasValue || itemA.Downloads < itemB.Downloads)	result = -1;
						else if (!itemB.Downloads.HasValue || itemA.Downloads > itemB.Downloads)	result = 1;
					}
				}
				if (result == 0 || this.sortMode == SortMode.Name)
				{
					result = string.Compare(itemA.Title, itemB.Title);
				}
				if (result == 0)
				{
					result = itemA.GetHashCode() - itemB.GetHashCode();
				}

				return (this.sortOrder == SortOrder.Ascending) ? -result : result;
			}
		}

		private	PackageManager					packageManager	= null;
		private	DisplayMode						display			= DisplayMode.None;
		private InstalledPackagesTreeModel		modelInstalled	= null;
		private OnlinePackagesTreeModel			modelOnline		= null;
		private	Size							oldTreeViewSize	= Size.Empty;

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

			this.oldTreeViewSize = this.packageList.Size;

			this.packageManager = DualityEditorApp.PackageManager;
			this.modelInstalled	= new InstalledPackagesTreeModel(this.packageManager);
			this.modelOnline	= new OnlinePackagesTreeModel(this.packageManager);

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
			e.Column.SortOrder = (e.Column.SortOrder == SortOrder.Descending) ? SortOrder.Ascending : SortOrder.Descending;

			IComparer<BaseItem> comparer = null;
			if (e.Column == this.treeColumnName)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Name, e.Column.SortOrder);
			else if (e.Column == this.treeColumnVersion)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Version, e.Column.SortOrder);
			else if (e.Column == this.treeColumnDownloads)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Downloads, e.Column.SortOrder);

			this.modelInstalled.SortComparer = comparer;
			this.modelOnline.SortComparer = comparer;
		}
		private void packageList_Resize(object sender, EventArgs e)
		{
			Size sizeChange = new Size(
				this.packageList.Width - this.oldTreeViewSize.Width,
				this.packageList.Height - this.oldTreeViewSize.Height);
			this.treeColumnName.Width += sizeChange.Width;
			this.oldTreeViewSize = this.packageList.Size;
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
