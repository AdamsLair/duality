using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.IO;

using Duality.Editor.Properties;
using Duality.Editor.PackageManagement;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;
using Duality.Editor.Plugins.PackageManagerFrontend.TreeModels;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public partial class PackageViewDialog : Form, IToolTipProvider
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
				Date,
				Downloads,
				PackageType,
				CombinedScore
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
				if (result == 0 || this.sortMode == SortMode.Date)
				{
					if		(itemA.ItemPackageInfo.PublishDate < itemB.ItemPackageInfo.PublishDate)	result = -1;
					else if (itemA.ItemPackageInfo.PublishDate > itemB.ItemPackageInfo.PublishDate)	result = 1;
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
				if (result == 0 || this.sortMode == SortMode.CombinedScore)
				{
					float scoreA = (float)itemA.ItemPackageInfo.DownloadCount * (1.0f - MathF.Clamp((float)(DateTime.Now - itemA.ItemPackageInfo.PublishDate).TotalDays / 360.0f, 0.001f, 1.0f));
					float scoreB = (float)itemB.ItemPackageInfo.DownloadCount * (1.0f - MathF.Clamp((float)(DateTime.Now - itemB.ItemPackageInfo.PublishDate).TotalDays / 360.0f, 0.001f, 1.0f));
					if		(scoreA < scoreB)	result = -1;
					else if (scoreA > scoreB)	result = 1;
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
		public string SearchFilter
		{
			get { return this.toolStripSearchBox.Text; }
			set { this.toolStripSearchBox.Text = value; }
		}

		public PackageViewDialog()
		{
			this.InitializeComponent();

			this.packageList.DefaultToolTipProvider = this;
			this.packageList.ShowNodeToolTips = true;
			this.packageList.NodeFilter = this.PackageListNodeFilter;

			this.treeColumnName.DrawColHeaderBg			+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnVersion.DrawColHeaderBg		+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnDate.DrawColHeaderBg			+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnDownloads.DrawColHeaderBg	+= this.treeColumn_DrawColHeaderBg;
			this.treeColumnPackageType.DrawColHeaderBg	+= this.treeColumn_DrawColHeaderBg;
			this.nodeTextBoxDownloads.DrawText			+= this.nodeTextBoxDownloads_DrawText;
			this.toolStripMain.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();

			this.toolStripViewBox.Items.Add(new FilterBoxItem(DisplayMode.Installed, PackageManagerFrontendRes.ItemName_InstalledPackages));
			this.toolStripViewBox.Items.Add(new FilterBoxItem(DisplayMode.Online, PackageManagerFrontendRes.ItemName_OnlineRepository));
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
			bool canUpdate = isItemUpdatable;

			this.buttonInstall.Visible			= isItemSelected && !isItemInstalled;
			this.buttonUninstall.Visible		= isItemInstalled && canUninstall;
			this.buttonUpdate.Visible			= isItemInstalled && canUpdate;
			this.bottomFlowSpacer2.Visible		= this.buttonInstall.Visible || this.buttonUninstall.Visible || this.buttonUpdate.Visible;
			this.buttonUpdateAll.Visible		= this.packageList.Root.Children.Select(n => n.Tag as PackageItem).NotNull().Any(n => n.IsUpdatable);
			this.buttonApply.Visible			= this.restartRequired;
			this.labelRequireRestart.Visible	= this.restartRequired;
		}
		private void UpdateInfoArea()
		{
			PackageInfo itemInfo		= this.selectedItem != null ? this.selectedItem.ItemPackageInfo : null;
			PackageInfo installedInfo	= this.selectedItem != null ? this.selectedItem.InstalledPackageInfo : null;
			PackageInfo newestInfo		= this.selectedItem != null ? this.selectedItem.NewestPackageInfo : null;

			this.SuspendLayout();

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
				this.labelPackageLicense.Text		= null;
				this.textBoxReleaseNotes.Text		= null;
			}
			else
			{
				bool isItemInstalled = installedInfo != null;
				bool isItemUpdatable = isItemInstalled && newestInfo != null && installedInfo.Version < newestInfo.Version;

				string releaseNoteText = (isItemUpdatable && !string.IsNullOrWhiteSpace(newestInfo.ReleaseNotes)) ? newestInfo.ReleaseNotes : string.Empty;
				if (!string.IsNullOrWhiteSpace(releaseNoteText))
					releaseNoteText = Regex.Replace(releaseNoteText, @"\r\n?|\n", Environment.NewLine);

				string websiteLinkCaption = itemInfo.ProjectUrl != null ? itemInfo.ProjectUrl.Host : string.Empty;
				string licenseLinkCaption = itemInfo.LicenseUrl != null ? itemInfo.LicenseUrl.Host : string.Empty;

				if (!string.IsNullOrWhiteSpace(websiteLinkCaption))
					websiteLinkCaption = string.Format(PackageManagerFrontendRes.ItemName_VisitLinkUrl, websiteLinkCaption);
				if (!string.IsNullOrWhiteSpace(licenseLinkCaption))
					licenseLinkCaption = string.Format(PackageManagerFrontendRes.ItemName_VisitLinkUrl, licenseLinkCaption);

				this.labelPackageTitle.Text			= !string.IsNullOrWhiteSpace(itemInfo.Title) ? itemInfo.Title : itemInfo.Id;
				this.labelPackageId.Text			= itemInfo.Id;
				this.labelPackageDesc.Text			= itemInfo.Description;
				this.labelPackageAuthor.Text		= itemInfo.Authors.ToString(", ");
				this.labelPackageTags.Text			= itemInfo.Tags.Except(InvisibleTags).ToString(", ");
				this.labelPackageUpdated.Text		= itemInfo.PublishDate.ToString("yyyy-MM-dd, HH:mm", System.Globalization.CultureInfo.InvariantCulture);
				this.labelPackageWebsite.Text		= websiteLinkCaption;
				this.labelPackageWebsite.Tag		= itemInfo.ProjectUrl;
				this.labelPackageLicense.Text		= licenseLinkCaption;
				this.labelPackageLicense.Tag		= itemInfo.LicenseUrl;
				this.textBoxReleaseNotes.Text		= releaseNoteText;
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
			this.labelPackageLicense.Visible		= !string.IsNullOrWhiteSpace(this.labelPackageLicense.Text);
			this.textBoxReleaseNotes.Visible		= !string.IsNullOrWhiteSpace(this.textBoxReleaseNotes.Text);

			this.labelPackageAuthorCaption.Visible	= this.labelPackageAuthor.Visible;
			this.labelPackageTagsCaption.Visible	= this.labelPackageTags.Visible;
			this.labelPackageVersionCaption.Visible	= this.labelPackageVersion.Visible;
			this.labelPackageUpdatedCaption.Visible	= this.labelPackageUpdated.Visible;
			this.labelPackageWebsiteCaption.Visible	= this.labelPackageWebsite.Visible;
			this.labelPackageLicenseCaption.Visible	= this.labelPackageLicense.Visible;
			this.labelReleaseNotesCaption.Visible	= this.textBoxReleaseNotes.Visible;

			this.ResumeLayout();
		}

		private void InstallPackage(PackageInfo info)
		{
			if (!this.ConfirmCompatibility(info))
				return;

			bool anythingChanged = false;
			EventHandler<PackageEventArgs> listener = delegate (object sender, PackageEventArgs e)
			{
				anythingChanged = true;
			};

			bool operationSuccessful = false;
			this.packageManager.PackageInstalled += listener;
			this.packageManager.PackageUninstalled += listener;
			{ 
				ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
					PackageManagerFrontendRes.TaskInstallPackages_Caption, 
					PackageManagerFrontendRes.TaskInstallPackages_Desc, 
					PackageOperationThread, 
					new PackageOperationData(this.packageManager, info, d => d.Manager.InstallPackage(d.Package)));
				setupDialog.MainThreadRequired = false;
				setupDialog.ShowDialog();
				operationSuccessful = setupDialog.DialogResult == DialogResult.OK;
			}
			this.packageManager.PackageUninstalled -= listener;
			this.packageManager.PackageInstalled -= listener;

			if (anythingChanged)
			{
				this.packageList.Invalidate();
				this.modelInstalled.ApplyChanges();
				this.modelOnline.ApplyChanges();
				this.restartRequired = operationSuccessful;
				this.UpdateBottomButtons();
			}
		}
		private void UninstallPackage(PackageInfo info)
		{
			bool anythingChanged = false;
			EventHandler<PackageEventArgs> listener = delegate (object sender, PackageEventArgs e)
			{
				anythingChanged = true;
			};

			bool operationSuccessful = false;
			this.packageManager.PackageInstalled += listener;
			this.packageManager.PackageUninstalled += listener;
			{
				ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
					PackageManagerFrontendRes.TaskUninstallPackages_Caption, 
					PackageManagerFrontendRes.TaskUninstallPackages_Desc, 
					PackageOperationThread, 
					new PackageOperationData(this.packageManager, info, d => d.Manager.UninstallPackage(d.Package)));
				setupDialog.MainThreadRequired = false;
				setupDialog.ShowDialog();
				operationSuccessful = setupDialog.DialogResult == DialogResult.OK;
			}
			this.packageManager.PackageUninstalled -= listener;
			this.packageManager.PackageInstalled -= listener;
			
			if (anythingChanged)
			{
				this.packageList.Invalidate();
				this.modelInstalled.ApplyChanges();
				this.modelOnline.ApplyChanges();
				this.restartRequired = operationSuccessful;
				this.UpdateBottomButtons();
			}
		}
		private void UpdatePackage(PackageInfo info)
		{
			PackageInfo newestInfo = this.packageManager.QueryPackageInfo(info.PackageName.VersionInvariant);
			if (!this.ConfirmCompatibility(newestInfo))
				return;

			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUpdatePackages_Caption, 
				PackageManagerFrontendRes.TaskUpdatePackages_Desc, 
				PackageOperationThread, 
				new PackageOperationData(this.packageManager, info, d => d.Manager.UpdatePackage(d.Package)));
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();

			this.packageList.Invalidate();
			this.modelInstalled.ApplyChanges();
			this.modelOnline.ApplyChanges();
			this.restartRequired = (setupDialog.DialogResult == DialogResult.OK);
			this.UpdateBottomButtons();
		}
		private void UpdateAllPackages()
		{
			IEnumerable<PackageInfo> newestUpdatablePackages = 
				this.packageManager.GetUpdatablePackages()
				.Select(p => this.packageManager.QueryPackageInfo(p.PackageName.VersionInvariant));
			if (!this.ConfirmCompatibility(newestUpdatablePackages))
				return;

			ProcessingBigTaskDialog setupDialog = new ProcessingBigTaskDialog(
				PackageManagerFrontendRes.TaskUpdatePackages_Caption, 
				PackageManagerFrontendRes.TaskUpdatePackages_Desc, 
				PackageUpdateAllThread, 
				this.packageManager);
			setupDialog.MainThreadRequired = false;
			setupDialog.ShowDialog();

			this.packageList.Invalidate();
			this.modelInstalled.ApplyChanges();
			this.modelOnline.ApplyChanges();
			this.restartRequired = true;
			this.UpdateBottomButtons();
		}
		
		private bool ConfirmCompatibility(PackageInfo package)
		{
			return this.ConfirmCompatibility(this.packageManager.GetCompatibilityLevel(package));
		}
		private bool ConfirmCompatibility(IEnumerable<PackageInfo> packages)
		{
			PackageCompatibility compatibility = PackageCompatibility.Definite;
			foreach (PackageInfo package in packages)
			{
				PackageCompatibility otherCompat = this.packageManager.GetCompatibilityLevel(package);
				compatibility = compatibility.Combine(otherCompat);
			}
			return this.ConfirmCompatibility(compatibility);
		}
		private bool ConfirmCompatibility(PackageCompatibility compatibility)
		{
			if (compatibility.IsAtLeast(PackageCompatibility.Likely)) return true;

			DialogResult result = MessageBox.Show(this,
				PackageManagerFrontendRes.MsgConfirmIncompatibleOperation_Desc,
				PackageManagerFrontendRes.MsgConfirmIncompatibleOperation_Caption,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning,
				MessageBoxDefaultButton.Button2);

			return result == DialogResult.Yes;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			this.oldTreeViewSize = this.packageList.Size;

			this.packageManager = DualityEditorApp.PackageManager;
			this.nodeTextBoxVersion.PackageManager = this.packageManager;

			this.modelOnline = new OnlinePackagesTreeModel(this.packageManager);
			this.modelOnline.SortComparer = new PackageListItemComparer(PackageListItemComparer.SortMode.CombinedScore, SortOrder.Ascending);
			this.modelOnline.NodesChanged += this.modelOnline_NodesChanged;

			this.modelInstalled = new InstalledPackagesTreeModel(this.packageManager);
			this.modelInstalled.NodesChanged += this.modelInstalled_NodesChanged;

			this.OnDisplayModeChanged(DisplayMode.None, this.display);
			this.toolStripSearchBox_TextChanged(this.toolStripSearchBox, EventArgs.Empty);

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
			this.packageList.UpdateNodeFilter();
			this.packageList.Invalidate();
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
		private void toolStripViewBox_DropDownClosed(object sender, EventArgs e)
		{
			this.packageList.Focus();
		}
		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private void toolStripViewBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FilterBoxItem selectedItem = this.toolStripViewBox.SelectedItem as FilterBoxItem;
			this.Display = selectedItem.Display;
		}
		private void packageList_ColumnClicked(object sender, TreeColumnEventArgs e)
		{
			if (e.Column.SortOrder == SortOrder.None)
			{
				if (e.Column == this.treeColumnVersion)
					e.Column.SortOrder = SortOrder.Ascending;
				else if (e.Column == this.treeColumnDownloads)
					e.Column.SortOrder = SortOrder.Ascending;
				else if (e.Column == this.treeColumnDate)
					e.Column.SortOrder = SortOrder.Ascending;
				else
					e.Column.SortOrder = SortOrder.Descending;
			}
			else
			{
				e.Column.SortOrder = (e.Column.SortOrder == SortOrder.Descending) ? SortOrder.Ascending : SortOrder.Descending;
			}

			IComparer<BaseItem> comparer = null;
			if (e.Column == this.treeColumnName)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Name, e.Column.SortOrder);
			else if (e.Column == this.treeColumnVersion)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Version, e.Column.SortOrder);
			else if (e.Column == this.treeColumnDownloads)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Downloads, e.Column.SortOrder);
			else if (e.Column == this.treeColumnDate)
				comparer = new PackageListItemComparer(PackageListItemComparer.SortMode.Date, e.Column.SortOrder);
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
			this.UpdateColumnVisibility();

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
			if (this.labelPackageWebsite.Tag is Uri)
				Process.Start((this.labelPackageWebsite.Tag as Uri).AbsoluteUri);
		}
		private void labelPackageLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.labelPackageLicense.Tag is Uri)
				Process.Start((this.labelPackageLicense.Tag as Uri).AbsoluteUri);
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
						StringComparison ignoreCase = StringComparison.InvariantCultureIgnoreCase;

						// Check title, id and any of the tags for the filter string
						if (packageItem.Title.IndexOf(filterString, ignoreCase) != -1) return true;
						if (packageItem.Id.IndexOf(filterString, ignoreCase) != -1) return true;
						if (packageItem.ItemPackageInfo != null && 
							packageItem.ItemPackageInfo.Tags.Any(t => 
								t != null && 
								t.IndexOf(filterString, ignoreCase) != -1))
							return true;

						// If it isn't contained anywhere, we don't want this item here.
						return false;
					}
					else
					{
						return true;
					}
				};
			}

			if (this.modelInstalled != null) this.modelInstalled.ItemFilter = itemFilter;
			if (this.modelOnline != null) this.modelOnline.ItemFilter = itemFilter;
		}
		private void buttonAdvanced_Click(object sender, EventArgs e)
		{
			Point screenPoint = this.buttonAdvanced.PointToScreen(new Point(this.buttonAdvanced.Left, this.buttonAdvanced.Bottom));
			Screen screen = Screen.FromControl(this.buttonAdvanced);
			if (screenPoint.Y + this.contextMenuAdvanced.Size.Height > screen.WorkingArea.Height)
			{
				this.contextMenuAdvanced.Show(this.buttonAdvanced, new Point(0, -this.contextMenuAdvanced.Size.Height));
			}
			else
			{
				this.contextMenuAdvanced.Show(this.buttonAdvanced, new Point(0, this.buttonAdvanced.Height));
			}   
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
		private void itemReInstallAll_Click(object sender, EventArgs e)
		{
			// Shut down the editor
			CancelEventArgs cancelArgs = new CancelEventArgs();
			Application.Exit(cancelArgs);
			if (cancelArgs.Cancel) return;

			// Delete all files and directories in the local package store, except the icon cache
			foreach (string dir in Directory.EnumerateDirectories(this.packageManager.LocalPackageStoreDirectory))
			{
				if (PathOp.ArePathsEqual(PackageItem.PackageIconCacheDir, dir))
					continue;
				Directory.Delete(dir, true);
			}
			foreach (string file in Directory.EnumerateFiles(this.packageManager.LocalPackageStoreDirectory))
			{
				File.Delete(file);
			}

			// Start the editor again
			Process.Start(Application.ExecutablePath);
		}

		private void OnDisplayModeChanged(DisplayMode prev, DisplayMode next)
		{
			FilterBoxItem selectedItem = this.toolStripViewBox.SelectedItem as FilterBoxItem;
			if (selectedItem == null || next != selectedItem.Display)
			{
				this.toolStripViewBox.SelectedItem = this.toolStripViewBox.Items.OfType<FilterBoxItem>().FirstOrDefault(i => i.Display == next);
			}

			if (next == DisplayMode.Installed)
			{
				this.packageList.Model = this.modelInstalled;
			}
			else if (next == DisplayMode.Online)
			{
				this.packageList.Model = this.modelOnline;
			}
			this.UpdateColumnVisibility();
		}

		private bool PackageListNodeFilter(TreeNodeAdv node)
		{
			PackageItem item = node.Tag as PackageItem;
			if (item == null) return true;

			return item.IsInstalled || item.Compatibility == PackageCompatibility.Unknown || item.Compatibility.IsAtLeast(PackageCompatibility.Likely);
		}

		private void UpdateColumnVisibility()
		{
			int colRefWidth = this.GetPackageListMainColumnWidth(c => 
				c != this.treeColumnDate && 
				c != this.treeColumnDownloads && 
				c != this.treeColumnName);

			this.UpdateColumnVisibility(this.treeColumnDate, ref colRefWidth);
			this.UpdateColumnVisibility(this.treeColumnDownloads, ref colRefWidth, () => this.display == DisplayMode.Online);
		}
		private void UpdateColumnVisibility(TreeColumn column, ref int colRefWidth, Func<bool> visibilityFunc = null)
		{
			bool displayColumn = colRefWidth > 400 && (visibilityFunc == null || visibilityFunc());
			if (displayColumn && !column.IsVisible)
			{
				column.IsVisible = true;
				this.treeColumnName.Width -= column.Width;
			}
			else if (!displayColumn && column.IsVisible)
			{
				column.IsVisible = false;
				this.treeColumnName.Width += column.Width;
			}
			if (column.IsVisible)
				colRefWidth -= this.treeColumnDownloads.Width;
		}
		private int GetPackageListMainColumnWidth(Predicate<TreeColumn> columnPredicate)
		{
			int availWidth = this.packageList.ClientSize.Width;
			foreach (TreeColumn column in this.packageList.Columns)
			{
				if (!column.IsVisible) continue;
				if (!columnPredicate(column)) continue;
				availWidth -= column.Width;
			}
			return availWidth;
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
			workerInterface.StateDesc = GeneralRes.TaskPrepareInfo;
			yield return null;
			
			// Determine which packages need to be updated
			PackageInfo[] updatePackages = manager.GetUpdatablePackages().ToArray();

			// Sort packages by their dependencies so we don't accidentally install multiple versions
			manager.OrderByDependencies(updatePackages);

			// Start the updating process
			foreach (PackageInfo package in updatePackages)
			{
				workerInterface.Progress += 1.0f / updatePackages.Length;
				workerInterface.StateDesc = string.Format("Package '{0}'...", package.Id);
				yield return null;

				try
				{
					manager.UpdatePackage(package);
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

		string IToolTipProvider.GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
		{
			if (nodeControl is IToolTipProvider)
			{
				IToolTipProvider controlProvider = nodeControl as IToolTipProvider;
				return controlProvider.GetToolTip(node, nodeControl);
			}
			return null;
		}
	}
}
