namespace Duality.Editor.Forms
{
	partial class CreateObjectDialog
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
			this.objectTypeView = new Aga.Controls.Tree.TreeViewAdv();
			this.treeNodeIcon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.treeNodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// objectTypeView
			// 
			this.objectTypeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.objectTypeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.objectTypeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.objectTypeView.DefaultToolTipProvider = null;
			this.objectTypeView.DragDropMarkColor = System.Drawing.Color.Black;
			this.objectTypeView.FullRowSelect = true;
			this.objectTypeView.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.objectTypeView.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.objectTypeView.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.objectTypeView.LoadOnDemand = true;
			this.objectTypeView.Location = new System.Drawing.Point(12, 46);
			this.objectTypeView.Model = null;
			this.objectTypeView.Name = "objectTypeView";
			this.objectTypeView.NodeControls.Add(this.treeNodeIcon);
			this.objectTypeView.NodeControls.Add(this.treeNodeName);
			this.objectTypeView.NodeFilter = null;
			this.objectTypeView.SelectedNode = null;
			this.objectTypeView.Size = new System.Drawing.Size(268, 101);
			this.objectTypeView.TabIndex = 0;
			this.objectTypeView.SelectionChanged += new System.EventHandler(this.objectTypeView_SelectionChanged);
			// 
			// treeNodeIcon
			// 
			this.treeNodeIcon.DataPropertyName = "Icon";
			this.treeNodeIcon.LeftMargin = 1;
			this.treeNodeIcon.ParentColumn = null;
			this.treeNodeIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// treeNodeName
			// 
			this.treeNodeName.DataPropertyName = "Name";
			this.treeNodeName.IncrementalSearchEnabled = true;
			this.treeNodeName.LeftMargin = 3;
			this.treeNodeName.ParentColumn = null;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Enabled = false;
			this.buttonOk.Location = new System.Drawing.Point(124, 162);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(205, 162);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelInfo
			// 
			this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelInfo.Location = new System.Drawing.Point(12, 9);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(268, 34);
			this.labelInfo.TabIndex = 3;
			this.labelInfo.Text = "Please select the Type of object you want to create.";
			// 
			// CreateObjectDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(292, 197);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.objectTypeView);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateObjectDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create Object...";
			this.ResumeLayout(false);

		}

		#endregion

		private Aga.Controls.Tree.TreeViewAdv objectTypeView;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon treeNodeIcon;
		private Aga.Controls.Tree.NodeControls.NodeTextBox treeNodeName;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelInfo;
	}
}