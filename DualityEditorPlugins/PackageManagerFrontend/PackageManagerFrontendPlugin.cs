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


		protected override void SaveUserData(XElement node)
		{
			if (this.packageView != null)
			{
				XElement packageViewElem = new XElement("PackageView");
				node.Add(packageViewElem);
				this.packageView.SaveUserData(packageViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			if (this.packageView != null)
			{
				XElement packageViewElem = node.Element("PackageView");
				if (packageViewElem != null)
				{
					this.packageView.LoadUserData(packageViewElem);
				}
			}
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			//MenuModelItem fileItem = main.MainMenu.RequestItem(GeneralRes.MenuName_File);
			//fileItem.AddItem(new MenuModelItem
			//{
			//    Name = PackageManagerFrontendRes.MenuItemName_PackageView,
			//    Icon = PackageManagerFrontendResCache.IconPackage.ToBitmap(),
			//    ActionHandler = this.menuItemLogView_Click
			//});
		}
		
		public void ShowPackageViewDialog()
		{
			if (this.packageView == null || this.packageView.IsDisposed)
			{
				this.packageView = new PackageViewDialog();
				this.packageView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.packageView = null; };
			}

			DialogResult result = this.packageView.ShowDialog(DualityEditorApp.MainForm);
		}

		private void menuItemLogView_Click(object sender, EventArgs e)
		{
			this.ShowPackageViewDialog();
		}
	}
}
