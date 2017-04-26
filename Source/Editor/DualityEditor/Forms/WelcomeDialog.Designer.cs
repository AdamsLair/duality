namespace Duality.Editor.Forms
{
	partial class WelcomeDialog
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
			this.labelHeader = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.labelDialogDesc = new System.Windows.Forms.Label();
			this.actionList = new Aga.Controls.Tree.TreeViewAdv();
			this.treeColumnMain = new Aga.Controls.Tree.TreeColumn();
			this.nodeActionIcon = new Aga.Controls.Tree.NodeControls.NodeIcon();
			this.nodeActionSummary = new Duality.Editor.Forms.WelcomeDialog.SummaryNodeControl();
			this.buttonOk = new System.Windows.Forms.Button();
			this.labelReOpenDesc = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// labelHeader
			// 
			this.labelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHeader.Location = new System.Drawing.Point(69, 9);
			this.labelHeader.Margin = new System.Windows.Forms.Padding(0);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(213, 22);
			this.labelHeader.TabIndex = 19;
			this.labelHeader.Text = "Your First Editor Session";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Image = global::Duality.Editor.Properties.Resources.DualityIcon48;
			this.pictureBoxLogo.Location = new System.Drawing.Point(12, 9);
			this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(48, 48);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxLogo.TabIndex = 18;
			this.pictureBoxLogo.TabStop = false;
			// 
			// labelDialogDesc
			// 
			this.labelDialogDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelDialogDesc.Location = new System.Drawing.Point(69, 36);
			this.labelDialogDesc.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.labelDialogDesc.Name = "labelDialogDesc";
			this.labelDialogDesc.Size = new System.Drawing.Size(213, 40);
			this.labelDialogDesc.TabIndex = 17;
			this.labelDialogDesc.Text = "This is probably the first time you\'re working with Duality - so here are some th" +
	"ings that might help you get started.";
			// 
			// actionList
			// 
			this.actionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.actionList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.actionList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.actionList.ColumnHeaderHeight = 1;
			this.actionList.Columns.Add(this.treeColumnMain);
			this.actionList.DefaultToolTipProvider = null;
			this.actionList.DragDropMarkColor = System.Drawing.Color.Black;
			this.actionList.FullRowSelect = true;
			this.actionList.FullRowSelectActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.actionList.FullRowSelectInactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(196)))), ((int)(((byte)(196)))));
			this.actionList.HideSelection = true;
			this.actionList.Indent = 0;
			this.actionList.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
			this.actionList.Location = new System.Drawing.Point(12, 87);
			this.actionList.Model = null;
			this.actionList.Name = "actionList";
			this.actionList.NodeControls.Add(this.nodeActionIcon);
			this.actionList.NodeControls.Add(this.nodeActionSummary);
			this.actionList.NodeFilter = null;
			this.actionList.RowHeight = 48;
			this.actionList.SelectedNode = null;
			this.actionList.ShowLines = false;
			this.actionList.ShowNodeToolTips = true;
			this.actionList.ShowPlusMinus = false;
			this.actionList.Size = new System.Drawing.Size(270, 206);
			this.actionList.TabIndex = 20;
			this.actionList.TabStop = false;
			this.actionList.Text = "packageList";
			this.actionList.UseColumns = true;
			this.actionList.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.actionList_NodeMouseClick);
			this.actionList.SelectionChanged += new System.EventHandler(this.actionList_SelectionChanged);
			this.actionList.RowDraw += new System.EventHandler<Aga.Controls.Tree.TreeViewRowDrawEventArgs>(this.actionList_RowDraw);
			this.actionList.MouseLeave += new System.EventHandler(this.actionList_MouseLeave);
			this.actionList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.actionList_MouseMove);
			this.actionList.Resize += new System.EventHandler(this.actionList_Resize);
			// 
			// treeColumnMain
			// 
			this.treeColumnMain.Header = "";
			this.treeColumnMain.SortOrder = System.Windows.Forms.SortOrder.None;
			this.treeColumnMain.TooltipText = null;
			// 
			// nodeActionIcon
			// 
			this.nodeActionIcon.DataPropertyName = "Image";
			this.nodeActionIcon.LeftMargin = 1;
			this.nodeActionIcon.ParentColumn = this.treeColumnMain;
			this.nodeActionIcon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
			// 
			// nodeActionSummary
			// 
			this.nodeActionSummary.LeftMargin = 3;
			this.nodeActionSummary.ParentColumn = this.treeColumnMain;
			// 
			// buttonOk
			// 
			this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOk.Location = new System.Drawing.Point(207, 304);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 3;
			this.buttonOk.Text = "Ok, thanks!";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// labelReOpenDesc
			// 
			this.labelReOpenDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.labelReOpenDesc.Location = new System.Drawing.Point(9, 301);
			this.labelReOpenDesc.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this.labelReOpenDesc.Name = "labelReOpenDesc";
			this.labelReOpenDesc.Size = new System.Drawing.Size(192, 26);
			this.labelReOpenDesc.TabIndex = 21;
			this.labelReOpenDesc.Text = "You can re-open this Dialog later using the Help menu in the upper right.";
			// 
			// WelcomeDialog
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(212)))), ((int)(((byte)(212)))));
			this.ClientSize = new System.Drawing.Size(294, 339);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.actionList);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.labelDialogDesc);
			this.Controls.Add(this.labelReOpenDesc);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(310, 350);
			this.Name = "WelcomeDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Welcome to Duality";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelDialogDesc;
		private Aga.Controls.Tree.TreeViewAdv actionList;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label labelReOpenDesc;
		private Aga.Controls.Tree.NodeControls.NodeIcon nodeActionIcon;
		private Duality.Editor.Forms.WelcomeDialog.SummaryNodeControl nodeActionSummary;
		private Aga.Controls.Tree.TreeColumn treeColumnMain;
	}
}