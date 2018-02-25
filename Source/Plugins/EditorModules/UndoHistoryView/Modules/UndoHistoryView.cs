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
            this.undoRedoListBox.DrawMode = DrawMode.OwnerDrawVariable;
                       
            //handle clicks to undo/redo
            this.undoRedoListBox.Click += UndoRedoListBox_Click;

            //Make sure the UI stays in sync with any undo/redo changes
            UndoRedoManager.StackChanged += undoRedoManager_StackChanged;     
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

        private void undoRedoManager_StackChanged(object sender, EventArgs e)
        {
            //Gray out the undo/redo buttons based on their ability to act
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

        //TODO: fix issue where text is being indented
        private void undoRedoListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ListBox lb = (ListBox)sender;

            string displayValue = lb.GetItemText(lb.Items[e.Index]);
            
            //Style the items based on if they are undo, selected or redo items
            if (e.Index < lb.SelectedIndex)
            {
                e.DrawBackground();
                Font undoFont = new Font(e.Font, FontStyle.Regular);
                Brush undoBrush = Brushes.Black;
                e.Graphics.DrawString(displayValue,
                      undoFont, 
                      undoBrush, 
                      new Rectangle(0, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height), 
                      StringFormat.GenericDefault);
            }
            else if (e.Index == lb.SelectedIndex)
            {
                //Prevent flicker of blur on previously selected item
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) 
                {
                    Font selectedFont = new Font(e.Font, FontStyle.Bold);
                    Brush selectedBrush = Brushes.Black;

                    e = new DrawItemEventArgs(e.Graphics,
                                      selectedFont,
                                      e.Bounds,
                                      e.Index,
                                      e.State ^ DrawItemState.Selected,
                                      e.ForeColor,
                                      Color.LightGray);

                    e.DrawBackground();

                    e.Graphics.DrawString(displayValue,
                          selectedFont, 
                          selectedBrush,
                          new Rectangle(0, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height),
                          StringFormat.GenericDefault);

                    e.DrawFocusRectangle();
                }
            }
            else
            {
                e.DrawBackground();
                Font redoFont = new Font(e.Font, FontStyle.Regular);
                Brush redoBrush = Brushes.DarkGray;
                e.Graphics.DrawString(displayValue,
                      redoFont, 
                      redoBrush,
                      new Rectangle(0, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height),
                      StringFormat.GenericDefault);
            }
        }
    }
}
