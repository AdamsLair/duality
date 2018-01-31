using AdamsLair.WinForms.ItemModels;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.UndoHistoryView.Properties;
using Duality.Editor.Properties;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.UndoHistoryView
{
    public class UndoHistoryViewPlugin : EditorPlugin
    {        
        UndoHistoryView _undoHistoryView;
        private bool isLoading = false;

        public override string Id
        {
            get { return "UndoHistoryView"; }
        }

        protected override IDockContent DeserializeDockContent(Type dockContentType)
        {
            this.isLoading = true;
            IDockContent result;
            if (dockContentType == typeof(UndoHistoryView))
                result = this.RequestUndoHistoryView();
            else
                result = base.DeserializeDockContent(dockContentType);
            this.isLoading = false;
            return result;
        }

        protected override void InitPlugin(MainForm main)
        {
            base.InitPlugin(main);

            // Request menu
            MenuModelItem viewItem = main.MainMenu.RequestItem(GeneralRes.MenuName_View);
            viewItem.AddItem(new MenuModelItem
            {
                Name = UndoHistoryViewRes.MenuItemName_UndoHistoryView,
                Icon = UndoHistoryViewRes.IconUndoHistory,//.ToBitmap(),
                ActionHandler = this.undoHistoryViewItemView_Click
            });
        }

        private void undoHistoryViewItemView_Click(object sender, EventArgs e)
        {            
            this.RequestUndoHistoryView();
        }

        public UndoHistoryView RequestUndoHistoryView()
        {
            if (this._undoHistoryView == null || this._undoHistoryView.IsDisposed)
            {
                this._undoHistoryView = new UndoHistoryView();
                this._undoHistoryView.FormClosed += delegate (object sender, FormClosedEventArgs e) { this._undoHistoryView = null; };
            }

            if (!this.isLoading)
            {
                this._undoHistoryView.Show(DualityEditorApp.MainForm.MainDockPanel);
                if (this._undoHistoryView.Pane != null)
                {
                    this._undoHistoryView.Pane.Activate();
                    this._undoHistoryView.Focus();
                }
            }

            return this._undoHistoryView;
        }
    }
}
