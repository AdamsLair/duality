using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.PackageManagement;
using Duality.Editor.Forms;
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
				Downloads,
				PackageType
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
					int itemANum = itemA.Downloads.HasValue ? itemA.Downloads.Value : 0;
					int itemBNum = itemB.Downloads.HasValue ? itemB.Downloads.Value : 0;
					result = itemANum - itemBNum;
				}
				if (result == 0 || this.sortMode == SortMode.Name)
				{
					result = string.Compare(itemA.Title, itemB.Title);
				}
				if (result == 0 || this.sortMode == SortMode.PackageType)
				{
					result = (int)itemA.Type - (int)itemB.Type;
				}
				if (result == 0)
				{
					result = itemA.GetHashCode() - itemB.GetHashCode();
				}

				return (this.sortOrder == SortOrder.Ascending) ? -result : result;
			}
		}
		private class PackageOperationData
		{
			private	PackageManager					manager;
			private	PackageInfo						package;
			private	Action<PackageOperationData>	operation;

			public PackageManager Manager
			{
				get { return this.manager; }
			}
			public PackageInfo Package
			{
				get { return this.package; }
			}
			public Action<PackageOperationData> Operation
			{
				get { return this.operation; }
			}

			public PackageOperationData(PackageManager manager, PackageInfo package, Action<PackageOperationData> operation)
			{
				this.manager = manager;
				this.package = package;
				this.operation = operation;
			}
		}

		private static readonly string[] InvisibleTags = new[] { 
			PackageManager.DualityTag, 
			PackageManager.PluginTag, 
			PackageManager.CoreTag, 
			PackageManager.EditorTag, 
			PackageManager.LauncherTag, 
			PackageManager.SampleTag };

		private	PackageManager					packageManager	= null;
		private	DisplayMode						display			= DisplayMode.None;
		private InstalledPackagesTreeModel		modelInstalled	= null;
		private OnlinePackagesTreeModel			modelOnline		= null;
		private	PackageItem						selectedItem	= null;
		private	bool							restartRequired	= false;
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
			this.treeColumnPackageType.DrawColHeaderBg	+= this.treeColumn_DrawColHeaderBg;
			this.nodeTextBoxDownloads.DrawText			+= this.nodeTextBoxDownloads_DrawText;
			this.toolStripMain.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();

			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Installed, PackageManagerFrontendRes.ItemName_InstalledPackages));
			this.toolStripFilterBox.Items.Add(new FilterBoxItem(DisplayMode.Online, PackageManagerFrontendRes.ItemName_OnlineRepository));
			
		}

		internal void SaveUserData(XElement node) {}
		internal void LoadUserData(XElement node) {}

		private void ScheduleLazyPackageInterfaceUpdate()
		{
			if (!this.timerPackageModelChanged.Enabled)
			{
				this.timerPackageModelChanged.Enabled = true;
			}
		}
		private void UpdateBottomButtons()
		{
			OnlinePackageItem onlineItem = this.selectedItem as OnlinePackageItem;
			if (onlineItem != null) onlineItem.UpdateLocalPackageData(this.packageManager);

			bool isItemSelected = this.selectedItem != null;
			bool isItemInstalled = isItemSelected && this.selectedItem.IsInstalled;
			bool isItemUpdatable = isItemInstalled && this.selectedItem.IsUpdatable;
			bool canUninstall = isItemInstalled && this.packageManager.CanUninstallPackage(this.selectedItem.ItemPackageInfo);
			bool canUpdate = isItemUpdatable && (this.checkBoxShowAdvanced.Checked || this.selectedItem.UpdateCompatibility.Satisfies(PackageCompatibility.Likely));

			this.buttonInstall.Visible			= isItemSelected && !isItemInstalled;
			this.buttonUninstall.Visible		= isItemInstalled && canUninstall;
			this.buttonChangeVersion.Visible	= isItemInstalled && this.checkBoxShowAdvanced.Checked;
			this.buttonUpdate.Visible			= isItemInstalled && canUpdate;
			this.bottomFlowSpacer2.Visible		= this.buttonInstall.Visible || this.buttonUninstall.Visible || this.buttonChangeVersion.Visible || this.buttonUpdate.Visible;
			this.buttonUpdateAll.Visible		= this.packageList.Root.Children.Select(n => n.Tag as PackageItem).Any(n => n.IsUpdatable);
			this.buttonApply.Visible			= this.restartRequired;
			this.labelRequireRestart.Visible	= this.restartRequired;
		}
		private void UpdateInfoArea()
		{
			PackageInfo itemInfo		= this.selectedItem != null ? this.selectedItem.ItemPackageInfo : null;
			PackageInfo installedInfo	= this.selectedItem != null ? this.selectedItem.InstalledPackageInfo : null;
			PackageInfo newestInfo		= this.selectedItem != null ? this.selectedItem.NewestPackageInfo : null;

			if (itemInfo == null)
			{
				this.labelPackageTitle.Text			= PackageManagerFrontendRes.NoPackageSelected;
				this.labelPackageId.Text			= null;
				this.labelPackageDesc.Text			= PackageManagerFrontendRes.NoDescAvailable;
				this.labelPackageAuthor.Text		= null;
				this.labelPackageTags.Text			= null;
				this.labelPackageVersion.Text		= null;
				this.labelPackageUpdated.Text		= null;
				this.labelPackageWebsite.Text		= null;
			}
			else
			{
				bool isItemInstalled = installedInfo != null;
				bool isItemUpdatable = isItemInstalled && newestInfo != null && installedInfo.Version < newestInfo.Version;

				this.labelPackageTitle.Text			= !string.IsNullOrWhiteSpace(itemInfo.Title) ? itemInfo.Title : itemInfo.Id;
				this.labelPackageId.Text			= itemInfo.Id;
				this.labelPackageDesc.Text			= (isItemUpdatable && !string.IsNullOrWhiteSpace(itemInfo.ReleaseNotes)) ? itemInfo.ReleaseNotes : itemInfo.Description;
				this.labelPackageAuthor.Text		= itemInfo.Authors.ToString(", ");
				this.labelPackageTags.Text			= itemInfo.Tags.Except(InvisibleTags).ToString(", ");
				this.labelPackageUpdated.Text		= itemInfo.PublishDate.ToString("yyyy-MM-dd, HH:mm", System.Globalization.CultureInfo.InvariantCulture);
				this.labelPackageWebsite.Text		= itemInfo.ProjectUrl != null ? itemInfo.ProjectUrl.ToString() : string.Empty;
				this.labelPackageVersion.Text		= isItemUpdatable ? 
					string.Format("{0} --> {1}", 
						PackageManager.GetDisplayedVersion(installedInfo.Version), 
						PackageManager.GetDisplayedVersion(newestInfo.Version)) : 
					PackageManager.GetDisplayedVersion(itemInfo.Version);
			}
			
			this.labelPackageAuthor.Visible			= !string.IsNullOrWhiteSpace(this.labelPackageAuthor.Text);
			this.labelPackageTags.Visible			= !string.IsNullOrWhiteSpace(this.labelPackageTags.Text);
			this.labelPackageVersion.Visible		= !string.IsNullOrWhiteSpace(this.labelPackageVersion.Text);
			this.labelPackageUpdated.Visible		= !string.IsNullOrWhiteSpace(this.labelPackageUpdated.Text);
			this.labelPackageWebsite.Visible		= !string.IsNullOrWhiteSpace(this.labelPackageWebsite.Text);

			this.labelPackageAuthorCaption.Visible	= this.labelPackageAuthor.Visible;
			this.labelPackageTagsCaption.Visible	= this.labelPackageTags.Visible;
			this.labelPackageVersionCaption.Visible	= this.labelPackageVersion.Visible;
			this.labelPackageUpdatedCaption.Visible	= this.labelPackageUpdated.Visible;
			this.labelPackageWebsiteCaption.Visible	= this.labelPackageWebsite.Visible;
		}

		private void InstallPackage(PackageInfo info)
		{
			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskInstallPackages_Caption, 
				PackageManagerFrontendRes.TaskInstallPackages_Desc, 
				PackageOperationThread, 
				new PackageOperationData(this.packageManager, info, d => d.Manager.InstallPackage(d.Package)));
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();
			this.modelInstalled.ApplyChanges();
			this.restartRequired = (setupDialog.DialogResult == DialogResult.OK);
			this.UpdateBottomButtons();
		}
		private void UninstallPackage(PackageInfo info)
		{
			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUninstallPackages_Caption, 
				PackageManagerFrontendRes.TaskUninstallPackages_Desc, 
				PackageOperationThread, 
				new PackageOperationData(this.packageManager, info, d => d.Manager.UninstallPackage(d.Package)));
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();
			this.modelInstalled.ApplyChanges();
			this.restartRequired = (setupDialog.DialogResult == DialogResult.OK);
			this.UpdateBottomButtons();
		}
		private void UpdatePackage(PackageInfo info)
		{
			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUpdatePackages_Caption, 
				PackageManagerFrontendRes.TaskUpdatePackages_Desc, 
				PackageOperationThread, 
				new PackageOperationData(this.packageManager, info, d => d.Manager.UpdatePackage(d.Package)));
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();
			this.modelInstalled.ApplyChanges();
			this.restartRequired = (setupDialog.DialogResult == DialogResult.OK);
			this.UpdateBottomButtons();
		}
		private void UpdatePackage(PackageInfo info, Version specificVersion)
		{
			bool success = false;
			bool cantUpdate = false;
			bool packageNotFound = false;
			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUpdatePackages_Caption, 
				PackageManagerFrontendRes.TaskUpdatePackages_Desc, 
				PackageOperationThread, 
				new PackageOperationData(this.packageManager, info, d => 
				{
					PackageInfo targetPackage = d.Manager.QueryPackageInfo(new PackageName(d.Package.Id, specificVersion));
					if (targetPackage == null)
					{
						packageNotFound = true;
						return;
					}
					if (d.Package.Version == specificVersion) return;
					if (!d.Manager.CanUpdatePackage(d.Package, specificVersion))
					{
						cantUpdate = true;
						return;
					}

					d.Manager.UpdatePackage(d.Package, specificVersion);
					success = true;
				}));
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();
			
			if (packageNotFound)
			{
				MessageBox.Show(this, 
					string.Format(PackageManagerFrontendRes.MsgTargetVersionNotFound_Desc, specificVersion, info.Id), 
					PackageManagerFrontendRes.MsgTargetVersionNotFound_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
			if (cantUpdate)
			{
				MessageBox.Show(this, 
					string.Format(PackageManagerFrontendRes.MsgTargetCantUpdate_Desc, specificVersion, info.Id), 
					PackageManagerFrontendRes.MsgTargetCantUpdate_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
			if (!success) return;

			this.modelInstalled.ApplyChanges();
			this.restartRequired = true;
			this.UpdateBottomButtons();
		}
		private void UpdateAllPackages()
		{
			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUpdatePackages_Caption, 
				PackageManagerFrontendRes.TaskUpdatePackages_Desc, 
				PackageUpdateAllThread, 
				this.packageManager);
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();
			this.modelInstalled.ApplyChanges();
			this.restartRequired = true;
			this.UpdateBottomButtons();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.oldTreeViewSize = this.packageList.Size;

			this.packageManager = DualityEditorApp.PackageManager;
			this.nodeTextBoxVersion.PackageManager = this.packageManager;

			this.modelInstalled = new InstalledPackagesTreeModel(this.packageManager);
			this.modelOnline = new OnlinePackagesTreeModel(this.packageManager);

			this.modelInstalled.NodesChanged += this.modelInstalled_NodesChanged;
			this.modelOnline.NodesChanged += this.modelOnline_NodesChanged;

			this.Display = DisplayMode.Installed;

			this.UpdateBottomButtons();
			this.UpdateInfoArea();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			this.modelInstalled.NodesChanged -= this.modelInstalled_NodesChanged;
			this.modelOnline.NodesChanged -= this.modelOnline_NodesChanged;
			this.modelInstalled.Dispose();
			this.modelOnline.Dispose();
		}
		
		private void timerPackageModelChanged_Tick(object sender, EventArgs e)
		{
			this.UpdateInfoArea();
			this.UpdateBottomButtons();
			this.timerPackageModelChanged.Enabled = false;
		}
		private void modelOnline_NodesChanged(object sender, TreeModelEventArgs e)
		{
			this.InvokeEx(this.ScheduleLazyPackageInterfaceUpdate, false);
		}
		private void modelInstalled_NodesChanged(object sender, TreeModelEventArgs e)
		{
			this.InvokeEx(this.ScheduleLazyPackageInterfaceUpdate, false);
		}
		private void toolStripFilterBox_DropDownClosed(object sender, EventArgs e)
		{
			this.packageList.Focus();
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
			else if (e.Column == this.treeColumnPackageType)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.PackageType, e.Column.SortOrder);

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
		private void packageList_SelectionChanged(object sender, EventArgs e)
		{
			this.selectedItem = this.packageList.SelectedNode != null ? this.packageList.SelectedNode.Tag as PackageItem : null;

			this.UpdateBottomButtons();
			this.UpdateInfoArea();
		}
		private void labelPackageWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(this.labelPackageWebsite.Text);
		}
		private void nodeTextBoxDownloads_DrawText(object sender, DrawTextEventArgs e)
		{
			e.TextColor = this.packageList.ForeColor;
		}
		private void toolStripSearchBox_TextChanged(object sender, EventArgs e)
		{
			string filterString = this.toolStripSearchBox.Text;
			Predicate<BaseItem> itemFilter = null;

			if (!string.IsNullOrWhiteSpace(filterString))
			{
				itemFilter = delegate(BaseItem item)
				{
					PackageItem packageItem = item as PackageItem;
					if (packageItem != null)
					{
						return 
							packageItem.Title.Contains(filterString) ||
							packageItem.Id.Contains(filterString) ||
							(packageItem.ItemPackageInfo != null && packageItem.ItemPackageInfo.Tags.Any(t => t != null && t.Contains(filterString)));
					}
					else
					{
						return true;
					}
				};
			}

			this.modelInstalled.ItemFilter = itemFilter;
			this.modelOnline.ItemFilter = itemFilter;
		}
		private void buttonApply_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			DualityEditorApp.SaveAllProjectData();
			this.packageManager.ApplyUpdate();
			Application.Exit();
		}
		private void buttonUninstall_Click(object sender, EventArgs e)
		{
			this.UninstallPackage(this.selectedItem.InstalledPackageInfo);
		}
		private void buttonInstall_Click(object sender, EventArgs e)
		{
			this.InstallPackage(this.selectedItem.NewestPackageInfo);
		}
		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			this.UpdatePackage(this.selectedItem.InstalledPackageInfo);
		}
		private void buttonUpdateAll_Click(object sender, EventArgs e)
		{
			this.UpdateAllPackages();
		}
		private void buttonChangeVersion_Click(object sender, EventArgs e)
		{
			PackageInfo selectedPackage = this.selectedItem.InstalledPackageInfo;

			SelectTargetVersionDialog dialog = new SelectTargetVersionDialog();
			dialog.SelectedVersion = selectedPackage.Version;

			DialogResult result = dialog.ShowDialog(this);
			if (result == DialogResult.Cancel) return;
			if (dialog.SelectedVersion == selectedPackage.Version) return;

			this.UpdatePackage(selectedPackage, dialog.SelectedVersion);
		}
		private void checkBoxShowAdvanced_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateBottomButtons();
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
				this.treeColumnDownloads.IsVisible = false;
				this.treeColumnName.Width += this.treeColumnDownloads.Width;
			}
			else if (next == DisplayMode.Online)
			{
				this.packageList.Model = this.modelOnline;
				this.treeColumnName.Width -= this.treeColumnDownloads.Width;
				this.treeColumnDownloads.IsVisible = true;
			}
		}

		private static System.Collections.IEnumerable PackageOperationThread(ProcessingBigTaskDialog.WorkerInterface workerInterface)
		{
			PackageOperationData data = workerInterface.Data as PackageOperationData;

			workerInterface.Progress = -1.0f;
			if (data.Package.Version != null)
				workerInterface.StateDesc = string.Format("Package '{0}', Version {1}...", data.Package.Id, data.Package.Version);
			else
				workerInterface.StateDesc = string.Format("Package '{0}'...", data.Package.Id);
			yield return null;

			try
			{
				data.Operation(data);
			}
			catch (Exception e)
			{
				Log.Editor.WriteError("An error occurred while processing Package '{0}', Version {1}: {2}", 
					data.Package.Id, 
					data.Package.Version, 
					Log.Exception(e));
				workerInterface.Error = e;
			}

			yield break;
		}
		private static System.Collections.IEnumerable PackageUpdateAllThread(ProcessingBigTaskDialog.WorkerInterface workerInterface)
		{
			PackageManager manager = workerInterface.Data as PackageManager;

			workerInterface.Progress = 0.0f;
			workerInterface.StateDesc = string.Format("Preparing operation...");
			yield return null;

			PackageInfo[] updatePackages = manager.GetSafeUpdateConfig(manager.LocalPackages).ToArray();
			foreach (PackageInfo package in updatePackages)
			{
				workerInterface.Progress += 1.0f / updatePackages.Length;
				workerInterface.StateDesc = string.Format("Package '{0}'...", package.Id);
				yield return null;

				try
				{
					manager.UpdatePackage(package, package.Version);
				}
				catch (Exception e)
				{
					Log.Editor.WriteError("An error occurred while updating Package '{0}', Version {1}: {2}", 
						package.Id, 
						package.Version, 
						Log.Exception(e));
					workerInterface.Error = e;
				}
			}

			workerInterface.Progress = 1.0f;
			yield break;
		}
	}
}
