namespace Duality.Editor.Forms
{
	using Aga.Controls.Tree;

	partial class ObjectRefSelectionDialog
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.labelInfo = new System.Windows.Forms.Label();
			this.autoExpandTreeView1 = new Aga.Controls.Tree.TreeViewAdv();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(277, 326);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(196, 326);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			// 
			// labelInfo
			// 
			this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInfo.Location = new System.Drawing.Point(12, 9);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(340, 23);
			this.labelInfo.TabIndex = 6;
			this.labelInfo.Text = "Please select the resource you would like to use.";
			// 
			// autoExpandTreeView1
			// 
			this.autoExpandTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));

			this.autoExpandTreeView1.BackColor = System.Drawing.SystemColors.Window;
			this.autoExpandTreeView1.ColumnHeaderHeight = 0;
			this.autoExpandTreeView1.DefaultToolTipProvider = null;
			this.autoExpandTreeView1.DragDropMarkColor = System.Drawing.Color.Black;
			this.autoExpandTreeView1.FullRowSelectActiveColor = System.Drawing.Color.Empty;
			this.autoExpandTreeView1.FullRowSelectInactiveColor = System.Drawing.Color.Empty;
			this.autoExpandTreeView1.LineColor = System.Drawing.SystemColors.ControlDark;
			this.autoExpandTreeView1.Location = new System.Drawing.Point(15, 36);
			this.autoExpandTreeView1.Model = null;
			this.autoExpandTreeView1.Name = "autoExpandTreeView1";
			this.autoExpandTreeView1.NodeFilter = null;
			this.autoExpandTreeView1.SelectedNode = null;
			this.autoExpandTreeView1.Size = new System.Drawing.Size(337, 284);
			this.autoExpandTreeView1.TabIndex = 7;
			this.autoExpandTreeView1.Text = "autoExpandTreeView1";
			// 
			// ObjectRefSelectionDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(364, 361);
			this.Controls.Add(this.autoExpandTreeView1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonOk);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(380, 400);
			this.Name = "ObjectRefSelectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Resource...";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label labelInfo;
		private TreeViewAdv autoExpandTreeView1;
	}
}