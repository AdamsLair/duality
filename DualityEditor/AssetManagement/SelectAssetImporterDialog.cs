using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Aga.Controls;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

using Duality.Editor.Properties;

namespace Duality.Editor.AssetManagement
{
	public partial class SelectAssetImporterDialog : Form
	{
		public class ImporterNode : Node
		{
			private IAssetImporter importer = null;
			public IAssetImporter Importer
			{
				get { return this.importer; }
			}
			public ImporterNode(IAssetImporter importer) : base(importer.Name)
			{
				this.importer = importer;
				this.Image = importer.GetType().GetEditorImage();
			}
		}


		private	TreeModel fileModel = null;
		private	TreeModel importerModel = null;
		private IAssetImporter defaultImporter = null;
		private IAssetImporter selectedImporter = null;

		public IAssetImporter DefaultImporter
		{
			get { return this.defaultImporter; }
			set { this.defaultImporter = value; }
		}
		public IAssetImporter SelectedImporter
		{
			get { return this.selectedImporter; }
		}

		public SelectAssetImporterDialog(IEnumerable<IAssetImporter> importers, IAssetImporter defaultImporter, IEnumerable<string> inputFiles)
		{
			this.InitializeComponent();
			
			this.importerTreeNodeName.DrawText += this.importerTreeNodeName_DrawText;
			this.fileTreeNodeName.DrawText += this.fileTreeNodeName_DrawText;
			this.fileViewColumnName.DrawColHeaderBg += this.fileViewColumnName_DrawColHeaderBg;
			this.inputFileView.ColumnHeaderHeight = 1;

			this.importerModel = new TreeModel();
			this.fileModel = new TreeModel();
			
			foreach (string inputFile in inputFiles)
			{
				this.fileModel.Nodes.Add(new Node
				{
					Text = inputFile,
					Image = GeneralResCache.page_white
				});
			}
			foreach (IAssetImporter importer in importers)
			{
				this.importerModel.Nodes.Add(new ImporterNode(importer));
			}
			this.defaultImporter = defaultImporter;
		}

		private void SelectDefaultImporter()
		{
			TreeNodeAdv defaultViewNode = this.importerView.AllNodes
				.Where(viewNode => viewNode.Tag is ImporterNode && (viewNode.Tag as ImporterNode).Importer == this.defaultImporter)
				.FirstOrDefault();
			this.importerView.SelectedNode = defaultViewNode;
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.inputFileView_Resize(this, EventArgs.Empty);
			this.inputFileView.Model = this.fileModel;
			this.importerView.Model = this.importerModel;
			this.SelectDefaultImporter();
		}
		private void buttonOk_Click(object sender, EventArgs e)
		{
			ImporterNode node = this.importerView.SelectedNode != null ? this.importerView.SelectedNode.Tag as ImporterNode : null;
			this.selectedImporter = node != null ? node.Importer : null;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.selectedImporter = null;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void importerTreeNodeName_DrawText(object sender, DrawTextEventArgs e)
		{
			ImporterNode importerNode = e.Node.Tag as ImporterNode;

			e.TextColor = Color.Black;
			if (importerNode != null && importerNode.Importer == this.defaultImporter)
			{
				e.TextColor = Color.Blue;
			}
		}
		private void fileTreeNodeName_DrawText(object sender, DrawTextEventArgs e)
		{
			e.TextColor = Color.FromArgb(192, Color.Black);
		}
		private void inputFileView_Resize(object sender, EventArgs e)
		{
			this.fileViewColumnName.Width = this.inputFileView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 5;
		}
		private void fileViewColumnName_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(this.inputFileView.BackColor), e.Bounds);
			e.Handled = true;
		}
	}
}
