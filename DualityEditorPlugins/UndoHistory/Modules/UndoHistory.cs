using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.UndoHistory.Modules
{
	public partial class UndoHistory : DockContent
	{
		public UndoHistory()
		{
			this.InitializeComponent();

			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

            UndoRedoManager.StackChanged += UndoRedoManagerOnStackChanged;
            UndoRedoManagerOnStackChanged(null, null);
			
			this.undoHistoryList.ScrollToEnd();

			this.UpdateTabText();
		}
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            UndoRedoManager.StackChanged -= UndoRedoManagerOnStackChanged;
        }

	    private void UndoRedoManagerOnStackChanged(object sender, EventArgs eventArgs)
	    {
            this.undoHistoryList.Clear();
            this.undoHistoryList.AddEntry(UndoRedoManager.AllActions);
	    }

	    private void UpdateTabText()
		{
			this.DockHandler.TabText = this.Text;
		}
	}
}
