using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Duality;
using Duality.Serialization;
using Duality.Serialization.MetaFormat;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.CorePluginInterface;

using Aga.Controls.Tree;
using WeifenLuo.WinFormsUI.Docking;

namespace ResourceHacker
{
	public partial class ResourceHacker : DockContent
	{
		private class BatchActionTaskData
		{
			private string folderPath;
			private Action<DataNode> action;

			public string FolderPath
			{
				get { return this.folderPath; }
			}
			public Action<DataNode> Action
			{
				get { return this.action; }
			}

			public BatchActionTaskData(string folderPath, Action<DataNode> action)
			{
				this.folderPath = folderPath;
				this.action = action;
			}
		}

		protected class DataTreeNode : Node
		{
			protected	DataNode	data;

			public DataNode Data
			{
				get { return this.data; }
			}
			public string NodeTypeName
			{
				get { return this.data.GetType().GetTypeKeyword(); }
			}

			protected DataTreeNode(DataNode data)
			{
				this.data = data;
				this.Text = this.NodeTypeName;
				this.Image = GetIcon(this.data);
			}

			public static DataTreeNode Create(DataNode data)
			{
				if (data is ObjectNode)
					return new ObjectTreeNode(data as ObjectNode);
				else if (data is ObjectRefNode)
					return new ObjectRefTreeNode(data as ObjectRefNode);
				else if (data is PrimitiveNode)
					return new PrimitiveTreeNode(data as PrimitiveNode);
				else if (data is EnumNode)
					return new EnumTreeNode(data as EnumNode);
				else if (data is StringNode)
					return new StringTreeNode(data as StringNode);
				else
					return new DataTreeNode(data);
			}
			public static Image GetIcon(DataNode data)
			{
				return CorePluginRegistry.GetTypeImage(data.GetType()) ?? CorePluginRegistry.GetTypeImage(typeof(DataNode));
			}
		}
		protected class PrimitiveTreeNode : DataTreeNode
		{
			protected	PrimitiveNode	primitiveData;

			public string ResolvedTypeName
			{
				get 
				{ 
					Type actualType = this.primitiveData.NodeType.ToActualType();
					return actualType != null ? actualType.GetTypeKeyword() : "Unknown";
				}
			}
			public object DataValue
			{
				get { return this.primitiveData.PrimitiveValue; }
			}

			public PrimitiveTreeNode(PrimitiveNode data) : base(data)
			{
				this.primitiveData = data;
			}
		}
		protected class EnumTreeNode : DataTreeNode
		{
			protected	EnumNode	enumData;

			public string ResolvedTypeName
			{
				get 
				{ 
					Type actualType = ReflectionHelper.ResolveType(this.enumData.EnumType, false);
					return actualType != null ? actualType.GetTypeKeyword() : "Unknown";
				}
			}
			public object DataValue
			{
				get { return this.enumData.ValueName; }
			}

			public EnumTreeNode(EnumNode data) : base(data)
			{
				this.enumData = data;
			}
		}
		protected class StringTreeNode : DataTreeNode
		{
			protected	StringNode	stringData;

			public string ResolvedTypeName
			{
				get { return typeof(string).GetTypeKeyword(); }
			}
			public object DataValue
			{
				get { return this.stringData.StringValue; }
			}

			public StringTreeNode(StringNode data) : base(data)
			{
				this.stringData = data;
			}
		}
		protected class ObjectTreeNode : DataTreeNode
		{
			protected	ObjectNode	objData;

			public uint ObjId
			{
				get { return this.objData.ObjId; }
			}
			public MemberInfo ResolvedMember
			{
				get
				{
					if (this.data.NodeType.IsMemberInfoType() && this.data.NodeType != DataType.Type)
						return ReflectionHelper.ResolveMember(this.objData.TypeString, false);
					else
						return ReflectionHelper.ResolveType(this.objData.TypeString, false);
				}
			}
			public string ResolvedTypeName
			{
				get 
				{ 
					MemberInfo resMember = this.ResolvedMember;
					Type resType = resMember as Type;
					if (resType != null)
						return resType.GetTypeCSCodeName(true);
					else if (resMember != null)
						return resMember.GetMemberId();
					else
						return objData.TypeString;
				}
			}

			public ObjectTreeNode(ObjectNode data) : base(data)
			{
				this.objData = data;
			}
		}
		protected class ObjectRefTreeNode : DataTreeNode
		{
			protected	ObjectRefNode	objRefData;

			public uint ObjId
			{
				get { return this.objRefData.ObjRefId; }
			}

			public ObjectRefTreeNode(ObjectRefNode data) : base(data)
			{
				this.objRefData = data;
			}
		}

		private	string				filePath	= null;
		private	TreeModel			dataModel	= new TreeModel();
		private	List<DataTreeNode>	rootData	= new List<DataTreeNode>();

		private	Dictionary<string,TypeDataLayoutNode>
			typeDataLayout = new Dictionary<string,TypeDataLayoutNode>();

		public ResourceHacker()
		{
			this.InitializeComponent();
			this.treeView.Model = this.dataModel;
			this.ClearData();

			this.nodeTextBoxObjId.DrawText += this.nodeTextBoxObjId_DrawText;
			this.nodeTextBoxType.DrawText += this.nodeTextBoxType_DrawText;
			this.nodeTextBoxName.DrawText += this.nodeTextBoxName_DrawText;
			this.nodeTextBoxValue.DrawText += this.nodeTextBoxValue_DrawText;
			this.treeViewColumnName.DrawColHeaderBg += this.treeViewColumn_DrawColHeaderBg;
			this.treeViewColumnObjId.DrawColHeaderBg += this.treeViewColumn_DrawColHeaderBg;
			this.treeViewColumnType.DrawColHeaderBg += this.treeViewColumn_DrawColHeaderBg;
			this.treeViewColumnValue.DrawColHeaderBg += this.treeViewColumn_DrawColHeaderBg;
			this.propertyGrid.EditingFinished += this.propertyGrid_EditingFinished;

			this.openFileDialog.InitialDirectory = DualityApp.DataDirectory;
			this.openFileDialog.Filter = "Duality Resource|*" + Resource.FileExt;
			this.saveFileDialog.InitialDirectory = this.openFileDialog.InitialDirectory;
			this.saveFileDialog.Filter = this.openFileDialog.Filter;

			this.mainToolStrip.Renderer = new DualityEditor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		protected override void OnShown(EventArgs e)
		{
			this.propertyGrid.RegisterEditorProvider(CorePluginRegistry.GetPropertyEditorProviders());
			base.OnShown(e);
		}

		public void LoadFile(string filePath)
		{
			if (!File.Exists(filePath)) throw new FileNotFoundException("Can't open Resource file. File not found.", filePath);

			this.ClearData();
			this.actionRenameType.Enabled = true;
			this.actionSave.Enabled = true;
			this.filePath = filePath;
			using (FileStream fileStream = File.OpenRead(this.filePath))
			{
				using (var formatter = Formatter.CreateMeta(fileStream))
				{
					DataNode dataNode;
					try
					{
						this.treeView.BeginUpdate();
						while ((dataNode = formatter.ReadObject() as DataNode) != null)
						{
							DataTreeNode data = this.AddData(dataNode);
							this.rootData.Add(data);
						}
					}
					catch (EndOfStreamException) {}
					catch (Exception e)
					{
						Log.Editor.WriteError("Can't load file {0} because an error occurred in the process: \n{1}",
							this.filePath,
							Log.Exception(e));
						return;
					}
					finally
					{
						foreach (DataTreeNode n in this.rootData) this.dataModel.Nodes.Add(n);
						this.treeView.EndUpdate(); 
					}
				}
			}
		}
		public void SaveFile(string filePath)
		{
			this.filePath = filePath;

			using (FileStream fileStream = File.Open(this.filePath, FileMode.Create, FileAccess.Write))
			{
				using (var formatter = Formatter.CreateMeta(fileStream))
				{
					foreach (DataTreeNode dataNode in this.dataModel.Nodes)
						formatter.WriteObject(dataNode.Data);
				}
			}

			// Assure reloading the modified resource
			if (PathHelper.IsPathLocatedIn(this.filePath, "."))
			{
				string dataPath = PathHelper.MakeFilePathRelative(this.filePath);
				ContentProvider.UnregisterContent(dataPath, true);
			}
		}

		protected void ClearData()
		{
			this.dataModel.Nodes.Clear();
			this.rootData.Clear();
			this.typeDataLayout.Clear();

			this.actionRenameType.Enabled = false;
			this.actionSave.Enabled = false;
		}
		protected DataTreeNode AddData(DataNode data)
		{
			// Register type data layout nodes
			if (data is TypeDataLayoutNode)
				this.typeDataLayout[(data.Parent as ObjectNode).TypeString] = data as TypeDataLayoutNode;

			DataTreeNode dataNode = DataTreeNode.Create(data);
			foreach (DataNode child in data.SubNodes)
			{
				DataTreeNode childDataNode = this.AddData(child);
				childDataNode.Parent = dataNode;
			}
			this.UpdateTypeDataLayout(dataNode, false);

			return dataNode;
		}
		protected void UpdateTypeDataLayout(Node updateNode = null, bool recursive = true)
		{
			if (updateNode == null) updateNode = this.dataModel.Root;
			foreach (DataTreeNode n in updateNode.Nodes)
			{
				if (!String.IsNullOrEmpty(n.Data.Name))
					n.Text = n.Data.Name;
				else
					n.Text = n.NodeTypeName;
			}

			if (recursive)
			{
				foreach (Node n in updateNode.Nodes)
					this.UpdateTypeDataLayout(n);
			}
		}
		protected bool IsObjectIdExisting(uint objId, DataNode baseNode = null)
		{
			if (objId == 0) return false;
			
			foreach (DataTreeNode dataNode in this.rootData)
				if (dataNode.Data.IsObjectIdDefined(objId)) return true;

			return false;
		}
		protected string[] GetAvailTypes(DataNode baseNode = null)
		{
			List<string> availTypes = new List<string>();

			foreach (DataTreeNode dataNode in this.rootData)
				availTypes.AddRange(dataNode.Data.GetTypeStrings(true));

			return availTypes.Distinct().ToArray();
		}

		protected void CurrentPerformAction(Action<DataNode> action)
		{
			foreach (DataTreeNode dataNode in this.rootData)
				action(dataNode.Data);
		}

		private void actionOpen_Click(object sender, EventArgs e)
		{
			this.openFileDialog.InitialDirectory = Path.GetFullPath(DualityApp.DataDirectory);
			this.openFileDialog.ShowDialog(this);
		}
		private void actionSave_Click(object sender, EventArgs e)
		{
			this.saveFileDialog.InitialDirectory = Path.GetFullPath(DualityApp.DataDirectory);
			this.saveFileDialog.ShowDialog(this);
		}
		private void openFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			this.LoadFile(this.openFileDialog.FileName);
			this.openFileDialog.FileName = "";
		}
		private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
		{
			this.SaveFile(this.saveFileDialog.FileName);
			this.saveFileDialog.FileName = "";
		}
		private void treeView_SelectionChanged(object sender, EventArgs e)
		{
			TreeNodeAdv viewSelNode = this.treeView.SelectedNode;
			DataTreeNode selNode = viewSelNode != null ? viewSelNode.Tag as DataTreeNode : null;
			this.propertyGrid.SelectObject(selNode != null ? selNode.Data : null);
		}
		private void nodeTextBoxObjId_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			DataTreeNode node = e.Node.Tag as DataTreeNode;
			ObjectRefTreeNode objRefNode = node as ObjectRefTreeNode;
			if (e.Text == "0") 
				e.TextColor = SystemColors.GrayText;
			else if (objRefNode != null)
			{
				if (this.IsObjectIdExisting(objRefNode.ObjId))
					e.TextColor = Color.Blue;
				else 
					e.TextColor = Color.Red;
			}
			else
				e.TextColor = Color.Black;
		}
		private void nodeTextBoxType_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			DataTreeNode node = e.Node.Tag as DataTreeNode;
			ObjectTreeNode objNode = node as ObjectTreeNode;
			if (objNode != null && objNode.ResolvedMember == null)
				e.TextColor = Color.Red;
			else
				e.TextColor = Color.Black;
		}
		private void nodeTextBoxName_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			e.TextColor = Color.Black;
		}
		private void nodeTextBoxValue_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			e.TextColor = Color.Black;
		}
		private void treeViewColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private void propertyGrid_EditingFinished(object sender, AdamsLair.PropertyGrid.PropertyEditorValueEventArgs e)
		{
			if (typeof(TypeDataLayoutNode).IsAssignableFrom(e.Editor.EditedType))
			{
				this.UpdateTypeDataLayout();
			}
		}

		private void actionRenameType_Click(object sender, EventArgs e)
		{
			RenameTypeDialog dialog = new RenameTypeDialog(this.GetAvailTypes());
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				int replaced = 0;
				this.CurrentPerformAction(n => replaced += n.ReplaceTypeStrings(dialog.SearchFor, dialog.ReplaceWith));
				MessageBox.Show(
					string.Format(PluginRes.ResourceHackerRes.MessageBox_RenameType_Text, replaced, dialog.SearchFor, dialog.ReplaceWith), 
					PluginRes.ResourceHackerRes.MessageBox_RenameType_Title, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Information);
			}
		}
		private void batchActionButton_Click(object sender, EventArgs e)
		{
			this.batchActionButton.ShowDropDown();
		}
		private void batchActionRenameType_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderDialog = new FolderBrowserDialog();
			folderDialog.ShowNewFolderButton = false;
			folderDialog.SelectedPath = Path.GetFullPath(DualityApp.DataDirectory);
			folderDialog.Description = "Select a folder to process..";
			if (folderDialog.ShowDialog(this) == DialogResult.OK)
			{
				RenameTypeDialog dialog = new RenameTypeDialog(this.GetAvailTypes());
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					int replaced = 0;
					ProcessingBigTaskDialog taskDialog = new ProcessingBigTaskDialog(
						PluginRes.ResourceHackerRes.TaskBatchRenameType_Caption,
						string.Format(PluginRes.ResourceHackerRes.TaskBatchRenameType_Desc, dialog.SearchFor, dialog.ReplaceWith), 
						this.async_PerformBatchAction,
						new BatchActionTaskData(folderDialog.SelectedPath, n => replaced += n.ReplaceTypeStrings(dialog.SearchFor, dialog.ReplaceWith)));
					taskDialog.MainThreadRequired = false;
					taskDialog.ShowDialog();

					MessageBox.Show(
						string.Format(PluginRes.ResourceHackerRes.MessageBox_RenameType_Text, replaced, dialog.SearchFor, dialog.ReplaceWith), 
						PluginRes.ResourceHackerRes.MessageBox_RenameType_Title, 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
				}
			}
		}

		private System.Collections.IEnumerable async_PerformBatchAction(ProcessingBigTaskDialog.WorkerInterface state)
		{
			BatchActionTaskData data = state.Data as BatchActionTaskData;

			// Retrieve files to perform action on
			List<string> files = Resource.GetResourceFiles(data.FolderPath);
			state.Progress += 0.05f; yield return null;

			// Perform action on files
			foreach (string file in files)
			{
				state.StateDesc = file; yield return null;

				MetaFormatHelper.FilePerformAction(file, data.Action);

				state.Progress += 0.9f / files.Count; yield return null;
			}

			// Assure reloading the modified resources
			if (PathHelper.IsPathLocatedIn(data.FolderPath, "."))
			{
				string dataPath = PathHelper.MakeFilePathRelative(data.FolderPath);
				ContentProvider.UnregisterContentTree(dataPath);
			}
			state.Progress += 0.05f;
		}
	}
}
