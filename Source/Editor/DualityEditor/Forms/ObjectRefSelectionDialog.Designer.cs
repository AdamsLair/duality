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
			this.objectReferenceListing = new Aga.Controls.Tree.TreeViewAdv();
			this.columnName = new Aga.Controls.Tree.TreeColumn();
			this.columnPath = new Aga.Controls.Tree.TreeColumn();
			this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodePath = new Aga.Controls.Tree.NodeControls.NodeTextBox();
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
			this.labelInfo.Text = "Please select the reference you would like to use.";
			// 
			// objectReferenceListing
			// 
			this.objectReferenceListing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.objectReferenceListing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.objectReferenceListing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.objectReferenceListing.ColumnHeaderHeight = 0;
			this.objectReferenceListing.Columns.Add(this.columnName);
			this.objectReferenceListing.Columns.Add(this.columnPath);
			this.objectReferenceListing.DefaultToolTipProvider = null;
			this.objectReferenceListing.DragDropMarkColor = System.Drawing.Color.Black;
			this.objectReferenceListing.FullRowSelect = true;
			this.objectReferenceListing.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.objectReferenceListing.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.objectReferenceListing.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.objectReferenceListing.LoadOnDemand = true;
			this.objectReferenceListing.Location = new System.Drawing.Point(15, 36);
			this.objectReferenceListing.Model = null;
			this.objectReferenceListing.Name = "objectReferenceListing";
			this.objectReferenceListing.NodeControls.Add(this.nodeName);
			this.objectReferenceListing.NodeControls.Add(this.nodePath);
			this.objectReferenceListing.NodeFilter = null;
			this.objectReferenceListing.SelectedNode = null;
			this.objectReferenceListing.Size = new System.Drawing.Size(337, 284);
			this.objectReferenceListing.TabIndex = 0;
			// 
			// columnName
			// 
			this.columnName.Header = "Name";
			this.columnName.Sortable = true;
			this.columnName.SortOrder = System.Windows.Forms.SortOrder.None;
			this.columnName.TooltipText = null;
			// 
			// columnPath
			// 
			this.columnPath.Header = "Path";
			this.columnPath.Sortable = true;
			this.columnPath.SortOrder = System.Windows.Forms.SortOrder.None;
			this.columnPath.TooltipText = null;
			// 
			// nodeName
			// 
			this.nodeName.DataPropertyName = "Name";
			this.nodeName.IncrementalSearchEnabled = true;
			this.nodeName.LeftMargin = 3;
			this.nodeName.ParentColumn = null;
			// 
			// nodePath
			// 
			this.nodePath.DataPropertyName = "Path";
			this.nodePath.IncrementalSearchEnabled = true;
			this.nodePath.LeftMargin = 3;
			this.nodePath.ParentColumn = null;
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
			this.Controls.Add(this.objectReferenceListing);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonOk);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(380, 400);
			this.Name = "ObjectRefSelectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Reference...";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label labelInfo;
		private TreeViewAdv objectReferenceListing;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeName;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodePath;
		private TreeColumn columnName;
		private TreeColumn columnPath;
	}
}