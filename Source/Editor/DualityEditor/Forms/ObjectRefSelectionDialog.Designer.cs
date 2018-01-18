using Aga.Controls.Tree;
using Duality.Editor.Controls;

namespace Duality.Editor.Forms
{

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
			this.columnImage = new Aga.Controls.Tree.TreeColumn();
			this.columnName = new Aga.Controls.Tree.TreeColumn();
			this.columnPath = new Aga.Controls.Tree.TreeColumn();
			this.nodeImage = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
			this.nodeName = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.nodePath = new Aga.Controls.Tree.NodeControls.NodeTextBox();
			this.txtFilterInput = new Duality.Editor.Controls.CueTextBox();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(454, 361);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(373, 361);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			// 
			// labelInfo
			// 
			this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelInfo.Location = new System.Drawing.Point(12, 9);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(516, 21);
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
			this.objectReferenceListing.Columns.Add(this.columnImage);
			this.objectReferenceListing.Columns.Add(this.columnName);
			this.objectReferenceListing.Columns.Add(this.columnPath);
			this.objectReferenceListing.DefaultToolTipProvider = null;
			this.objectReferenceListing.DragDropMarkColor = System.Drawing.Color.Black;
			this.objectReferenceListing.FullRowSelect = true;
			this.objectReferenceListing.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.objectReferenceListing.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.objectReferenceListing.Indent = 5;
			this.objectReferenceListing.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.objectReferenceListing.LoadOnDemand = true;
			this.objectReferenceListing.Location = new System.Drawing.Point(11, 57);
			this.objectReferenceListing.Model = null;
			this.objectReferenceListing.Name = "objectReferenceListing";
			this.objectReferenceListing.NodeControls.Add(this.nodeImage);
			this.objectReferenceListing.NodeControls.Add(this.nodeName);
			this.objectReferenceListing.NodeControls.Add(this.nodePath);
			this.objectReferenceListing.NodeFilter = null;
			this.objectReferenceListing.SelectedNode = null;
			this.objectReferenceListing.ShowLines = false;
			this.objectReferenceListing.ShowPlusMinus = false;
			this.objectReferenceListing.Size = new System.Drawing.Size(517, 298);
			this.objectReferenceListing.TabIndex = 1;
			this.objectReferenceListing.UseColumns = true;
			// 
			// columnImage
			// 
			this.columnImage.Header = "Icon";
			this.columnImage.MaxColumnWidth = 45;
			this.columnImage.MinColumnWidth = 20;
			this.columnImage.SortOrder = System.Windows.Forms.SortOrder.None;
			this.columnImage.TooltipText = null;
			this.columnImage.Width = 32;
			// 
			// columnName
			// 
			this.columnName.Header = "Name";
			this.columnName.MaxColumnWidth = 300;
			this.columnName.MinColumnWidth = 80;
			this.columnName.Sortable = true;
			this.columnName.SortOrder = System.Windows.Forms.SortOrder.Ascending;
			this.columnName.TooltipText = null;
			this.columnName.Width = 150;
			// 
			// columnPath
			// 
			this.columnPath.Header = "Path";
			this.columnPath.MaxColumnWidth = 800;
			this.columnPath.MinColumnWidth = 200;
			this.columnPath.Sortable = true;
			this.columnPath.SortOrder = System.Windows.Forms.SortOrder.None;
			this.columnPath.TooltipText = null;
			this.columnPath.Width = 300;
			// 
			// nodeImage
			// 
			this.nodeImage.DataPropertyName = "Image";
			this.nodeImage.IncrementalSearchEnabled = true;
			this.nodeImage.LeftMargin = 3;
			this.nodeImage.ParentColumn = this.columnImage;
			this.nodeImage.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeName
			// 
			this.nodeName.DataPropertyName = "Name";
			this.nodeName.IncrementalSearchEnabled = true;
			this.nodeName.LeftMargin = 3;
			this.nodeName.ParentColumn = this.columnName;
			// 
			// nodePath
			// 
			this.nodePath.DataPropertyName = "Path";
			this.nodePath.IncrementalSearchEnabled = true;
			this.nodePath.LeftMargin = 3;
			this.nodePath.ParentColumn = this.columnPath;
			// 
			// txtFilterInput
			// 
			this.txtFilterInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilterInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.txtFilterInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFilterInput.CueText = "Reference Filter";
			this.txtFilterInput.Location = new System.Drawing.Point(11, 32);
			this.txtFilterInput.Margin = new System.Windows.Forms.Padding(2);
			this.txtFilterInput.Name = "txtFilterInput";
			this.txtFilterInput.Size = new System.Drawing.Size(517, 20);
			this.txtFilterInput.TabIndex = 0;
			this.txtFilterInput.TextChanged += new System.EventHandler(this.txtFilterInput_TextChanged);
			// 
			// ObjectRefSelectionDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(540, 396);
			this.Controls.Add(this.txtFilterInput);
			this.Controls.Add(this.objectReferenceListing);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonOk);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(380, 399);
			this.Name = "ObjectRefSelectionDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Reference...";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label labelInfo;
		private TreeViewAdv objectReferenceListing;
		private Aga.Controls.Tree.NodeControls.NodeStateIcon nodeImage;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodeName;
		private Aga.Controls.Tree.NodeControls.NodeTextBox nodePath;
		private TreeColumn columnImage;
		private TreeColumn columnName;
		private TreeColumn columnPath;
		private CueTextBox txtFilterInput;
	}
}