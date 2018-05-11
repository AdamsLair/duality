namespace Duality.Editor.Plugins.Base.Forms.PixmapSlicer
{
	partial class PixmapSlicerForm
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
			this.horizontalScrollBar = new System.Windows.Forms.HScrollBar();
			this.verticalScrollBar = new System.Windows.Forms.VScrollBar();
			this.stateControlToolStrip = new System.Windows.Forms.ToolStrip();
			this.SuspendLayout();
			// 
			// horizontalScrollBar
			// 
			this.horizontalScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.horizontalScrollBar.Location = new System.Drawing.Point(0, 237);
			this.horizontalScrollBar.Name = "horizontalScrollBar";
			this.horizontalScrollBar.Size = new System.Drawing.Size(428, 17);
			this.horizontalScrollBar.TabIndex = 1;
			// 
			// verticalScrollBar
			// 
			this.verticalScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.verticalScrollBar.Location = new System.Drawing.Point(411, 0);
			this.verticalScrollBar.Name = "verticalScrollBar";
			this.verticalScrollBar.Size = new System.Drawing.Size(17, 237);
			this.verticalScrollBar.TabIndex = 2;
			// 
			// stateControlToolStrip
			// 
			this.stateControlToolStrip.AutoSize = false;
			this.stateControlToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.stateControlToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
			this.stateControlToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.stateControlToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.stateControlToolStrip.Location = new System.Drawing.Point(0, 0);
			this.stateControlToolStrip.Name = "stateControlToolStrip";
			this.stateControlToolStrip.Size = new System.Drawing.Size(411, 25);
			this.stateControlToolStrip.TabIndex = 3;
			this.stateControlToolStrip.Text = "toolStrip2";
			this.stateControlToolStrip.Visible = false;
			// 
			// PixmapSlicerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(428, 254);
			this.Controls.Add(this.stateControlToolStrip);
			this.Controls.Add(this.verticalScrollBar);
			this.Controls.Add(this.horizontalScrollBar);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "PixmapSlicerForm";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.Text = "PixmapSlicer";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.HScrollBar horizontalScrollBar;
		private System.Windows.Forms.VScrollBar verticalScrollBar;
		private System.Windows.Forms.ToolStrip stateControlToolStrip;
	}
}