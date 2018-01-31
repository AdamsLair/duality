namespace Duality.Editor.Plugins.UndoHistoryView
{
    partial class UndoHistoryView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndoHistoryView));
            this.undoToolStrip = new System.Windows.Forms.ToolStrip();
            this.undoButton = new System.Windows.Forms.ToolStripButton();
            this.redoButton = new System.Windows.Forms.ToolStripButton();
            this.undoRedoListBox = new System.Windows.Forms.ListBox();
            this.undoToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // undoToolStrip
            // 
            this.undoToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
            this.undoToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.undoToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.undoToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoButton,
            this.redoButton});
            this.undoToolStrip.Location = new System.Drawing.Point(0, 0);
            this.undoToolStrip.Name = "undoToolStrip";
            this.undoToolStrip.Size = new System.Drawing.Size(284, 25);
            this.undoToolStrip.Stretch = true;
            this.undoToolStrip.TabIndex = 0;
            this.undoToolStrip.Text = "undoToolStrip";
            // 
            // undoButton
            // 
            this.undoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoButton.Image = global::Duality.Editor.Plugins.UndoHistoryView.Properties.Resources.arrow_undo;
            this.undoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(23, 22);
            this.undoButton.Text = "Undo";
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // redoButton
            // 
            this.redoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoButton.Image = global::Duality.Editor.Plugins.UndoHistoryView.Properties.Resources.arrow_redo;
            this.redoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redoButton.Name = "redoButton";
            this.redoButton.Size = new System.Drawing.Size(23, 22);
            this.redoButton.Text = "Redo";
            this.redoButton.Click += new System.EventHandler(this.redoButton_Click);
            // 
            // undoRedoListBox
            // 
            this.undoRedoListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.undoRedoListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.undoRedoListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoRedoListBox.FormattingEnabled = true;
            this.undoRedoListBox.Location = new System.Drawing.Point(0, 25);
            this.undoRedoListBox.Name = "undoRedoListBox";
            this.undoRedoListBox.Size = new System.Drawing.Size(284, 236);
            this.undoRedoListBox.TabIndex = 1;
            // 
            // UndoHistoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.ControlBox = false;
            this.Controls.Add(this.undoRedoListBox);
            this.Controls.Add(this.undoToolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UndoHistoryView";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Undo History";
            this.undoToolStrip.ResumeLayout(false);
            this.undoToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip undoToolStrip;
        private System.Windows.Forms.ToolStripButton undoButton;
        private System.Windows.Forms.ToolStripButton redoButton;
        private System.Windows.Forms.ListBox undoRedoListBox;
    }
}
