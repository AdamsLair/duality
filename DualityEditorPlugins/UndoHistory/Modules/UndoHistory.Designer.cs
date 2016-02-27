namespace Duality.Editor.Plugins.UndoHistory.Modules
{
	partial class UndoHistory
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UndoHistory));
            this.undoHistoryList = new Duality.Editor.Plugins.UndoHistory.Modules.UndoHistoryList();
            this.SuspendLayout();
            // 
            // undoHistoryList
            // 
            this.undoHistoryList.AutoScroll = true;
            this.undoHistoryList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
            this.undoHistoryList.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.undoHistoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoHistoryList.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.undoHistoryList.Location = new System.Drawing.Point(0, 0);
            this.undoHistoryList.Name = "undoHistoryList";
            this.undoHistoryList.ScrollOffset = 0;
            this.undoHistoryList.SelectedEntry = null;
            this.undoHistoryList.Size = new System.Drawing.Size(683, 195);
            this.undoHistoryList.TabIndex = 3;
            // 
            // UndoHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 195);
            this.Controls.Add(this.undoHistoryList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UndoHistory";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
            this.Text = "Undo/Redo...";
            this.ResumeLayout(false);

		}

		#endregion

        private UndoHistoryList undoHistoryList;
	}
}