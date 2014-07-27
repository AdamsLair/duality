using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using WeifenLuo.WinFormsUI.Docking;

using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Editor.Forms;

using Duality.Editor.Plugins.PackageManagerFrontend.Properties;

namespace Duality.Editor.Plugins.PackageManagerFrontend
{
	public class PackageManagerFrontendPlugin : EditorPlugin
	{
		private	PackageView	packageView		= null;
		private	bool		isLoading		= false;

		private	ToolStripMenuItem	menuItemPackageView		= null;


		public override string Id
		{
			get { return "PackageManagerFrontend"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(PackageView))
				result = this.RequestPackageView();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void SaveUserData(XElement node)
		{
			if (this.packageView != null)
			{
				XElement packageViewElem = new XElement("PackageView_0");
				node.Add(packageViewElem);
				this.packageView.SaveUserData(packageViewElem);
			}
		}
		protected override void LoadUserData(XElement node)
		{
			this.isLoading = true;
			if (this.packageView != null)
			{
				XElement packageViewElem = node.Element("PackageView_0");
				if (packageViewElem != null)
				{
					this.packageView.LoadUserData(packageViewElem);
				}
			}
			this.isLoading = false;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menu
			//this.menuItemPackageView = main.RequestMenu(GeneralRes.MenuName_File, PackageManagerFrontendRes.MenuItemName_PackageView);
			//this.menuItemPackageView.Image = PackageManagerFrontendResCache.IconPackage.ToBitmap();
			//this.menuItemPackageView.Click += this.menuItemLogView_Click;
		}
		
		public PackageView RequestPackageView()
		{
			if (this.packageView == null || this.packageView.IsDisposed)
			{
				this.packageView = new PackageView();
				this.packageView.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.packageView = null; };
			}

			if (!this.isLoading)
			{
				this.packageView.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.packageView.Pane != null)
				{
					this.packageView.Pane.Activate();
					this.packageView.Focus();
				}
			}

			return this.packageView;
		}

		private void menuItemLogView_Click(object sender, EventArgs e)
		{
			this.RequestPackageView();
		}
	}
}
