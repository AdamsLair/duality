using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Duality.Editor.Plugins.UndoHistoryView
{
    public partial class UndoHistoryView : DockContent
    {      
        private BindingSource undoStackSource;

        public UndoHistoryView()
        {
            InitializeComponent();

            //Can't bind directly to a IReadOnlyList, so we create a binding source
            this.undoStackSource = new BindingSource();
            this.undoStackSource.DataSource = UndoRedoManager.ActionStack;

            //Setup data binding and display 
            this.undoRedoListBox.DataSource = undoStackSource;
            this.undoRedoListBox.DisplayMember = "Name";

            //handle clicks to undo/redo
            this.undoRedoListBox.Click += UndoRedoListBox_Click;

            //Make sure the UI stays in sync with any undo/redo changes
            UndoRedoManager.StackChanged += UndoRedoManager_StackChanged;     
        }

        private void UndoRedoListBox_Click(object sender, EventArgs e)
        {
            //Where in the stack we need to end up
            int selectedIndex = ((ListBox)sender).SelectedIndex;

            //Walk the stack until the selected index and position in undo/redo stack is equal
            while (selectedIndex != UndoRedoManager.ActionIndex)
            {
                if (selectedIndex < UndoRedoManager.ActionIndex)
                {
                    UndoRedoManager.Undo();
                }
                else if (selectedIndex > UndoRedoManager.ActionIndex)
                {
                    UndoRedoManager.Redo();
                }
            }
        }

        private void UndoRedoManager_StackChanged(object sender, EventArgs e)
        {
            if (UndoRedoManager.CanUndo)
            {
                this.undoButton.Enabled = true;
            }
            else
            {
                this.undoButton.Enabled = false;
            }

            if (UndoRedoManager.CanRedo)
            {
                this.redoButton.Enabled = true;
            }
            else
            {
                this.redoButton.Enabled = false;
            }

            //update the list
            this.undoStackSource.ResetBindings(false);

            //update the selection
            this.undoRedoListBox.SelectedIndex = UndoRedoManager.ActionIndex;
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            if (UndoRedoManager.CanUndo)
                UndoRedoManager.Undo();
        }

        private void redoButton_Click(object sender, EventArgs e)
        {
            if (UndoRedoManager.CanRedo)
                UndoRedoManager.Redo();
        }
    }
}
