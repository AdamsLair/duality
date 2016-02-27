using System;
using System.Windows.Forms;
using System.Xml.Linq;
using AdamsLair.WinForms.ItemModels;
using Duality;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.UndoHistory.Properties;
using Duality.Editor.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.UndoHistory
{
	public class UndoHistoryPlugin : EditorPlugin
	{
		private	Modules.UndoHistory	undoHistory		= null;
		private	bool                isLoading       = false;


		public override string Id
		{
			get { return "UndoHistory"; }
		}


		protected override IDockContent DeserializeDockContent(Type dockContentType)
		{
			this.isLoading = true;
			IDockContent result;
			if (dockContentType == typeof(Modules.UndoHistory))
				result = this.RequestUndoHistory();
			else
				result = base.DeserializeDockContent(dockContentType);
			this.isLoading = false;
			return result;
		}
		
		protected override void InitPlugin(MainForm main)
		{
			base.InitPlugin(main);
			
			// Request menu
			MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_Edit);
			viewItem.AddItem(new MenuModelItem
			{
                Name = UndoHistoryRes.MenuItemName_UndoHistory,
				Icon = UndoHistoryResCache.IconUndoHistory,
				ActionHandler = this.menuItemUndoHistory_Click
			});
		}
		
		public Modules.UndoHistory RequestUndoHistory()
		{
			if (this.undoHistory == null || this.undoHistory.IsDisposed)
			{
				this.undoHistory = new Modules.UndoHistory();
				this.undoHistory.FormClosed += delegate(object sender, FormClosedEventArgs e) { this.undoHistory = null; };
			}

			if (!this.isLoading)
			{
				this.undoHistory.Show(DualityEditorApp.MainForm.MainDockPanel);
				if (this.undoHistory.Pane != null)
				{
					this.undoHistory.Pane.Activate();
					this.undoHistory.Focus();
				}
			}

			return this.undoHistory;
		}

		private void menuItemUndoHistory_Click(object sender, EventArgs e)
		{
			this.RequestUndoHistory();
		}
	}
}
