using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ItemModels;

using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class PackageManagerFrontendPlugin : EditorPlugin
	{
		private	PackageViewDialog	packageView		= null;


		public override string Id
		{
			get { return "PackageManagerFrontend"; }
		}
		public PackageViewDialog PackageView
		{
			get
			{
				if (this.packageView == null)
					this.packageView = new PackageViewDialog();
				return this.packageView;
			}
		}


		protected override void SaveUserData(XElement node)
		{
			XElement packageViewElem = new XElement("PackageView");
			this.PackageView.SaveUserData(packageViewElem);
			if (!packageViewElem.IsEmpty)
				node.Add(packageViewElem);
		}
		protected override void LoadUserData(XElement node)
		{
			XElement packageViewElem = node.Element("PackageView");
			if (packageViewElem != null)
			{
				this.PackageView.LoadUserData(packageViewElem);
			}
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			MenuModelItem fileItem = main.MainMenu.RequestItem(GeneralRes.MenuName_File);
			fileItem.AddItem(new MenuModelItem
			{
			    Name = PackageManagerFrontendRes.MenuItemName_PackageView,
			    Icon = PackageManagerFrontendResCache.IconPackage.ToBitmap(),
			    ActionHandler = this.menuItemManagePackages_Click
			});
			MenuModelItem helpItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Help);
			helpItem.AddItem(new MenuModelItem
			{
			    Name = PackageManagerFrontendRes.MenuItemName_BrowseSamples,
			    ActionHandler = this.menuItemBrowseSamples_Click
			});
		}
		
		public void ShowPackageViewDialog()
		{
			this.PackageView.Display = PackageViewDialog.DisplayMode.Installed;
			this.PackageView.SearchFilter = string.Empty;
			this.PackageView.ShowDialog(DualityEditorApp.MainForm);
		}
		public void ShowSamplePackageBrowser()
		{
			this.PackageView.Display = PackageViewDialog.DisplayMode.Online;
			this.PackageView.SearchFilter = PackageManagement.PackageManager.SampleTag;
			this.PackageView.ShowDialog(DualityEditorApp.MainForm);
		}

		private void menuItemManagePackages_Click(object sender, EventArgs e)
		{
			this.ShowPackageViewDialog();
		}
		private void menuItemBrowseSamples_Click(object sender, EventArgs e)
		{
			this.ShowSamplePackageBrowser();
		}
	}
}
