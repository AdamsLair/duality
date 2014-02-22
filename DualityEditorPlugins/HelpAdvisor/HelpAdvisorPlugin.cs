using System;
using System.Windows.Forms;
using System.IO;

using Duality.Editor;
using Duality.Editor.Properties;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.HelpAdvisor.Properties;

using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.HelpAdvisor
{
	public class HelpAdvisorPlugin : EditorPlugin
	{
		private	static	HelpAdvisorPlugin	instance	= null;
		internal static HelpAdvisorPlugin Instance
		{
			get { return instance; }
		}


		private	bool				isLoading			= false;
		private	HelpAdvisor			helpAdvisor			= null;
		private	ToolStripMenuItem	menuItemHelpAdvisor	= null;

		public override string Id
		{
			get { return "HelpAdvisor"; }
		}


		public HelpAdvisorPlugin()
		{
			instance = this;
		}
		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(HelpAdvisor))
				result = this.RequestHelpAdvisor();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);

			// Request menus
			this.menuItemHelpAdvisor = main.RequestMenu(Path.Combine(GeneralRes.MenuName_Help, HelpAdvisorRes.MenuItemName_Advisor));

			// Configure menus
			this.menuItemHelpAdvisor.Image = HelpAdvisorResCache.IconHelp;
			this.menuItemHelpAdvisor.Click += new EventHandler(this.menuItemHelpAdvisor_Click);
		}

		public HelpAdvisor RequestHelpAdvisor()
		{
			if (this.helpAdvisor == null || this.helpAdvisor.IsDisposed)
			{
				this.helpAdvisor = new HelpAdvisor();
				this.helpAdvisor.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.helpAdvisor = null; };
			}

			if (!this.isLoading)
			{
				this.helpAdvisor.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.helpAdvisor.Pane != null)
				{
					this.helpAdvisor.Pane.Activate();
					this.helpAdvisor.Focus();
				}
			}

			return this.helpAdvisor;
		}

		private void menuItemHelpAdvisor_Click(object sender, EventArgs e)
		{
			this.RequestHelpAdvisor();
		}
	}
}
