using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;
using Aga.Controls.Tree;
using AdamsLair.WinForms.ItemModels;
using AdamsLair.WinForms.ItemViews;

using Duality;
using Duality.IO;
using Duality.Resources;
using Duality.Editor;
using Duality.Editor.AssetManagement;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.ProjectView.TreeModels;
using System.Xml.Linq;

namespace Duality.Editor.Plugins.ProjectView
{
	public partial class ProjectFolderView : DockContent, IHelpProvider, IToolTipProvider
	{
		private struct ScheduleSelectEntry
		{
			public string Path;
			public bool BeginRename;

			public ScheduleSelectEntry(string path, bool rename)
			{
				this.Path = path;
				this.BeginRename = rename;
			}
		}
		private class CreateContextEntryTag
		{
			public string TypeId;
			public bool IsDualityType;
		}


		private	Dictionary<string,NodeBase>	pathIdToNode		= new Dictionary<string,NodeBase>();
		private	TreeModel					folderModel			= null;
		private	MenuModel					nodeContextModel	= null;
		private	MenuStripMenuView			nodeContextView		= null;
		private	NodeBase					lastEditedNode		= null;
		private System.Drawing.Font			treeFontItalic		= null;

		private	NodeBase	flashNode		= null;
		private	float		flashDuration	= 0.0f;
		private	float		flashIntensity	= 0.0f;

		private	List<ScheduleSelectEntry>	scheduleSelectPath		= new List<ScheduleSelectEntry>();
		private	List<string>				skipGlobalRenamePath	= new List<string>();
		private	HashSet<string>				unsavedResRedrawCache	= new HashSet<string>();
		private string						importSourcePath		= null;

		private	Dictionary<Node,bool>	tempNodeVisibilityCache		= new Dictionary<Node,bool>();
		private	string					tempUpperFilter				= null;
		private	string					tempDropBasePath			= null;
		private	List<string>			tempFileDropList			= null;
		private bool					tempScheduleSelectionChange	= false;

		private MenuModelItem			nodeContextItemNew				= null;
		private MenuModelItem			nodeContextItemImport			= null;
		private MenuModelItem			nodeContextItemCut				= null;
		private MenuModelItem			nodeContextItemCopy				= null;
		private MenuModelItem			nodeContextItemPaste			= null;
		private MenuModelItem			nodeContextItemDelete			= null;
		private MenuModelItem			nodeContextItemRename			= null;
		private MenuModelItem			nodeContextItemShowInExplorer	= null;


		public ProjectFolderView()
		{
			this.InitializeComponent();
			this.InitContextMenu();

			this.treeFontItalic = new System.Drawing.Font(this.folderView.Font, FontStyle.Italic);

			this.folderView.DefaultToolTipProvider = this;
			this.folderModel = new TreeModel();
			this.folderView.Model = this.folderModel;

			this.nodeTextBoxType.DrawText += this.nodeTextBoxType_DrawText;
			this.nodeTextBoxName.DrawText += this.nodeTextBoxName_DrawText;
			this.nodeTextBoxName.EditorShowing += new CancelEventHandler(nodeTextBoxName_EditorShowing);
			this.nodeTextBoxName.EditorHided += new EventHandler(nodeTextBoxName_EditorHided);
			this.nodeTextBoxName.ChangesApplied += new EventHandler(nodeTextBoxName_ChangesApplied);

			this.treeColumnName.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;
			this.treeColumnType.DrawColHeaderBg += this.treeColumn_DrawColHeaderBg;

			this.toolStrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.InitResources();
			DualityEditorApp.HighlightObject       += this.DualityEditorApp_HighlightObject;
			DualityEditorApp.SelectionChanged      += this.EditorForm_SelectionChanged;
			FileEventManager.ResourcesChanged      += this.FileEventManager_ResourcesChanged;
			FileEventManager.BeginGlobalRename     += this.FileEventManager_BeginGlobalRename;
			DualityEditorApp.ObjectPropertyChanged += this.EditorForm_ObjectPropertyChanged;
			Resource.ResourceSaved                 += this.Resource_ResourceSaved;
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			DualityEditorApp.HighlightObject       -= this.DualityEditorApp_HighlightObject;
			DualityEditorApp.SelectionChanged      -= this.EditorForm_SelectionChanged;
			FileEventManager.ResourcesChanged      -= this.FileEventManager_ResourcesChanged;
			FileEventManager.BeginGlobalRename     -= this.FileEventManager_BeginGlobalRename;
			DualityEditorApp.ObjectPropertyChanged -= this.EditorForm_ObjectPropertyChanged;
			Resource.ResourceSaved                 -= this.Resource_ResourceSaved;
		}

		internal void SaveUserData(XElement node)
		{
			node.SetElementValue("ImportSourcePath", this.importSourcePath);
		}
		internal void LoadUserData(XElement node)
		{
			this.importSourcePath = node.GetElementValue("ImportSourcePath", this.importSourcePath);
		}

		public void FlashNode(NodeBase node)
		{
			if (node == null) return;

			this.flashNode = node;
			this.flashDuration = 0.0f;
			this.timerFlashItem.Enabled = true;

			this.folderView.EnsureVisible(this.folderView.FindNode(this.folderModel.GetPath(this.flashNode)));
		}
		public NodeBase NodeFromPath(string path)
		{
			NodeBase result;
			if (!this.pathIdToNode.TryGetValue(NodeBase.GetNodePathId(path), out result)) return null;
			return result;
		}

		public bool SelectNode(NodeBase node, bool select = true, bool exclusive = false)
		{
			if (node == null) return false;
			TreeNodeAdv viewNode = this.folderView.FindNode(this.folderModel.GetPath(node));
			if (viewNode != null)
			{
				if (exclusive) this.folderView.ClearSelection();
				viewNode.IsSelected = select;
				this.folderView.EnsureVisible(viewNode);
				this.folderView.Invalidate();
				return true;
			}
			return false;
		}
		public void ScheduleSelect(string filePath, bool rename = false)
		{
			filePath = Path.GetFullPath(filePath);
			if (!this.SelectNode(this.NodeFromPath(filePath), true, rename))
			{
				this.scheduleSelectPath.Add(new ScheduleSelectEntry(filePath, rename));
			}
			else if (rename)
			{
				this.nodeTextBoxName.BeginEdit();
			}
		}
		protected void PerformScheduleSelect(string incomingFilePath)
		{
			ScheduleSelectEntry entry = this.scheduleSelectPath.FirstOrDefault(e => e.Path == incomingFilePath);
			if (entry.Path != incomingFilePath) return;

			NodeBase selNode = this.NodeFromPath(incomingFilePath);
			if (this.SelectNode(selNode, true, entry.BeginRename))
			{
				this.scheduleSelectPath.Remove(entry);
				if (entry.BeginRename) this.nodeTextBoxName.BeginEdit();
			}
		}

		protected void ApplyNodeFilter()
		{
			this.tempUpperFilter = String.IsNullOrEmpty(this.textBoxFilter.Text) ? null : this.textBoxFilter.Text.ToUpper();
			this.tempNodeVisibilityCache.Clear();
			this.folderView.NodeFilter = this.tempUpperFilter != null ? this.folderModel_IsNodeVisible : (Predicate<object>)null;
		}

		protected void InitResources()
		{
			this.toolStripLabelProjectName.Text = String.Format("{0}: {1}",
				Properties.ProjectViewRes.ProjectNameLabel,
				EditorHelper.CurrentProjectName);

			Node nodeTree = this.ScanDirectory(DualityApp.DataDirectory);
			nodeTree.Nodes.Insert(0, this.ScanDefaultContent());

			this.folderView.BeginUpdate();
			this.ClearResources();
			while (nodeTree.Nodes.Count > 0)
			{
				Node n = nodeTree.Nodes[0];
				NodeBase nb = n as NodeBase;
				this.InsertNodeSorted(n, this.folderModel.Root);
				this.RegisterNodeTree(n);
				if (nb != null) nb.NotifyVisible();
			}
			this.folderView.EndUpdate();
		}
		protected void ClearResources()
		{
			this.folderModel.Nodes.Clear();
			this.pathIdToNode.Clear();
		}
		protected void RegisterNodeTree(Node node)
		{
			this.RegisterNode(node);
			foreach (Node c in node.Nodes) this.RegisterNodeTree(c);
		}
		protected void UnregisterNodeTree(Node node)
		{
			this.UnregisterNode(node);
			foreach (Node c in node.Nodes) this.UnregisterNodeTree(c);
		}
		protected void RegisterNode(Node node)
		{
			NodeBase nodeBase = node as NodeBase;
			if (nodeBase != null) this.pathIdToNode[nodeBase.NodePathId] = nodeBase;
		}
		protected void UnregisterNode(Node node)
		{
			NodeBase nodeBase = node as NodeBase;
			if (nodeBase != null) this.pathIdToNode.Remove(nodeBase.NodePathId);
		}
		protected void InsertNodeSorted(Node newNode, Node parentNode)
		{
			Node insertBeforeNode;
			if (newNode is DirectoryNode)
			{
				insertBeforeNode = parentNode.Nodes.FirstOrDefault(node => node is DirectoryNode && String.Compare(node.Text, newNode.Text) > 0);
				if (insertBeforeNode == null) insertBeforeNode = parentNode.Nodes.FirstOrDefault();
			}
			else
				insertBeforeNode = parentNode.Nodes.FirstOrDefault(node => !(node is DirectoryNode) && String.Compare(node.Text, newNode.Text) > 0);

			if (insertBeforeNode == null) parentNode.Nodes.Add(newNode);
			else parentNode.Nodes.Insert(parentNode.Nodes.IndexOf(insertBeforeNode), newNode);
		}

		protected NodeBase ScanFile(string filePath)
		{
			if (!PathHelper.IsPathVisible(filePath)) return null;

			if (Resource.IsResourceFile(filePath))
				return new ResourceNode(filePath);
			else
				return null;
		}
		protected DirectoryNode ScanDirectory(string dirPath)
		{
			if (!PathHelper.IsPathVisible(dirPath)) return null;
			DirectoryNode thisNode = new DirectoryNode(dirPath);

			foreach (string dir in Directory.EnumerateDirectories(dirPath))
			{
				DirectoryNode dirNode = this.ScanDirectory(dir);
				if (dirNode != null) this.InsertNodeSorted(dirNode, thisNode);
			}

			foreach (string file in Directory.EnumerateFiles(dirPath))
			{
				NodeBase fileNode = this.ScanFile(file);
				if (fileNode != null) this.InsertNodeSorted(fileNode, thisNode);
			}

			return thisNode;
		}
		protected NodeBase ScanDefaultResource(ContentRef<Resource> resRef)
		{
			if (!resRef.IsAvailable) return null;
			return new ResourceNode(resRef);
		}
		protected DirectoryNode ScanDefaultContent()
		{
			DirectoryNode thisNode = new DirectoryNode(Resource.DefaultContentBasePath, true);

			List<ContentRef<Resource>> defaultContent = ContentProvider.GetDefaultContent<Resource>();
			foreach (ContentRef<Resource> resRef in defaultContent)
			{
				string[] pathSplit = resRef.Path.Split(':');
				DirectoryNode curDirNode = thisNode;

				// Generate virtual subdirectory nodes
				string curDirPath = pathSplit[0];
				for (int i = 1; i < pathSplit.Length - 1; i++)
				{
					curDirPath += ":" + pathSplit[i];
					DirectoryNode subNode = curDirNode.Nodes.FirstOrDefault(delegate(Node n) 
						{ 
							return n is DirectoryNode && n.Text == pathSplit[i]; 
						}) as DirectoryNode;

					if (subNode == null)
					{
						subNode = new DirectoryNode(curDirPath + ":", true);
						this.InsertNodeSorted(subNode, curDirNode);
						curDirNode.Nodes.Add(subNode);
					}
					curDirNode = subNode;
				}

				// Generate virtual Resource node
				NodeBase res = this.ScanDefaultResource(resRef);
				if (res != null) this.InsertNodeSorted(res, curDirNode);
			}

			return thisNode;
		}
		
		protected void ClipboardCopyNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			DataObject data = new DataObject();
			this.AppendNodesToData(data, nodes);

			Clipboard.Clear();
			Clipboard.SetDataObject(data);
		}
		protected void ClipboardCutNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			DataObject data = new DataObject();
			this.AppendNodesToData(data, nodes);
			Clipboard.SetDataObject(data, true);

			byte[] moveEffect = new byte[] {2, 0, 0, 0};
			MemoryStream dropEffect = new MemoryStream();
			dropEffect.Write(moveEffect, 0, moveEffect.Length);

			data.SetData("Preferred DropEffect", dropEffect);

			Clipboard.Clear();
			Clipboard.SetDataObject(data);
		}
		protected bool ClipboardHasCutNode(TreeNodeAdv node)
		{
			NodeBase baseNode = node.Tag as NodeBase;
			if (baseNode == null || baseNode.ReadOnly) return false;

			DataObject data = Clipboard.GetDataObject() as DataObject;
			if (data != null)
			{
				// Dropping files
				if (data.ContainsFileDropList())
				{
					// Retrieve preferred drop effect (Windows system stuff)
					MemoryStream dropEffect = data.GetData("Preferred DropEffect") as MemoryStream;
					bool move = false;
					if (dropEffect != null)
					{
						byte[] moveEffect = new byte[4];
						dropEffect.Read(moveEffect, 0, 4);
						move = moveEffect[0] == 2 && moveEffect[1] == 0 && moveEffect[2] == 0 && moveEffect[3] == 0;
					}

					return move && data.GetFileDropList().Contains(Path.GetFullPath(baseNode.NodePath));
				}
			}
			return false;
		}
		protected bool ClipboardCanPasteNodes(TreeNodeAdv baseNode)
		{
			DataObject data = Clipboard.GetDataObject() as DataObject;
			return data != null && data.ContainsFileDropList();
		}
		protected void ClipboardPasteNodes(TreeNodeAdv baseNode)
		{
			this.folderView.BeginUpdate();

			this.tempDropBasePath = this.GetInsertActionTargetBasePath(baseNode != null ? baseNode.Tag as NodeBase : null);
			DataObject data = Clipboard.GetDataObject() as DataObject;
			if (data != null)
			{
				// Dropping files
				if (data.ContainsFileDropList())
				{
					IEnumerable<string> incomingFiles = data.GetFileDropList().OfType<string>();

					// Handle file import operations and filter out files that have been handled that way.
					incomingFiles = this.HandleFileImport(this.tempDropBasePath, incomingFiles);

					// Filter out non-Resource files that might have been dropped accidentally into the data directory
					incomingFiles = incomingFiles.Where(path => Resource.IsResourceFile(path) || Directory.Exists(path));

					// If there's anything left, proceed with a regular file drop operation
					if (incomingFiles.Any())
					{
						this.tempFileDropList = incomingFiles.ToList();

						// Retrieve preferred drop effect (Windows system stuff)
						MemoryStream dropEffect = data.GetData("Preferred DropEffect") as MemoryStream;
						bool move = false;
						if (dropEffect != null)
						{
							byte[] moveEffect = new byte[4];
							dropEffect.Read(moveEffect, 0, 4);
							move = moveEffect[0] == 2 && moveEffect[1] == 0 && moveEffect[2] == 0 && moveEffect[3] == 0;
						}

						// Depending on the preferred effect, either move or copy the specified files
						if (move)
						{
							this.moveHereToolStripMenuItem_Click(this, null);
							Clipboard.Clear();
						}
						else
						{
							this.copyHereToolStripMenuItem_Click(this, null);
						}
					}
				}
			}

			this.folderView.EndUpdate();
		}
		protected void DeleteNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			TreeNodeAdv[] nodeArray = nodes.ToArray();

			// If they're all read-only, don't bother starting an operation
			bool allReadOnly = nodeArray.All(viewNode => (viewNode.Tag as NodeBase).ReadOnly);
			if (allReadOnly)
				return;

			// Ask for user confirmation
			if (!this.DisplayConfirmDeleteSelectedFiles())
				return;

			// Query model nodes from the given view nodes
			var nodeQuery = 
				from viewNode in nodeArray
				select this.folderModel.FindNode(this.folderView.GetPath(viewNode)) as NodeBase;

			// Determine the array of file paths to be deleted and send it to the recycling bin
			string[] deleteFilePaths = nodeQuery
				.Where(n => !n.ReadOnly)
				.Select(n => n.NodePath)
				.ToArray();
			RecycleBin.Send(deleteFilePaths);
		}
		protected void CreateFolder(TreeNodeAdv baseNode)
		{
			string basePath = this.GetInsertActionTargetBasePath(baseNode != null ? baseNode.Tag as NodeBase : null);
			string dirPath = PathHelper.GetFreePath(Path.Combine(basePath, Properties.ProjectViewRes.NewFolderName), "");

			Directory.CreateDirectory(dirPath);
			
			// Skip the global rename action for this path once, because it's empty and clearly unreferenced..
			this.SkipGlobalRenameEventFor(dirPath);

			this.folderView.ClearSelection();
			this.ScheduleSelect(dirPath, true);
		}
		protected IContentRef CreateResource(Type type, TreeNodeAdv baseNode, string desiredName = null)
		{
			return this.CreateResource(type, baseNode != null ? baseNode.Tag as NodeBase : null, desiredName);
		}
		protected IContentRef CreateResource(Type type, NodeBase baseNode, string desiredName = null)
		{
			if (desiredName == null) desiredName = type.Name;

			// Create a new Resource instance of the specified type
			Resource resInstance = type.GetTypeInfo().CreateInstanceOf() as Resource;
			Resource[] actionTargets = new Resource[] { resInstance };

			// Gather all available user editing setup actions
			IEnumerable<IEditorAction> setupActions = DualityEditorApp.GetEditorActions(
				resInstance.GetType(), 
				actionTargets,
				DualityEditorApp.ActionContextSetupObjectForEditing);

			// Invoke all of them on the new Resource
			foreach (IEditorAction setupAction in setupActions)
			{
				setupAction.Perform(actionTargets);
			}

			// Determine the actual name and path of our new Resource
			string basePath = this.GetInsertActionTargetBasePath(baseNode);
			string nameExt = Resource.GetFileExtByType(type);
			string resPath = PathHelper.GetFreePath(Path.Combine(basePath, desiredName), nameExt);
			resPath = PathHelper.MakeFilePathRelative(resPath);

			// Save the new Resource to make it persistent - it will show up in the Project View once it's there
			resInstance.Save(resPath);

			// Schedule path for later selection and rename - as soon as it actually exists.
			this.folderView.ClearSelection();
			this.ScheduleSelect(resPath, true);
			
			// Skip the global rename action for this path once, because there clearly aren't any referencs to a new Resource.
			this.SkipGlobalRenameEventFor(resPath);

			return resInstance.GetContentRef();
		}
		protected void OpenResource(TreeNodeAdv node)
		{
			IEditorAction openAction = this.GetResourceOpenAction(node);
			if (openAction != null)
			{
				ResourceNode resNode = node.Tag as ResourceNode;
				Resource[] actionTargets = new Resource[] { resNode.ResLink.Res };
				openAction.Perform(actionTargets);
			}
		}
		protected IEditorAction GetResourceOpenAction(TreeNodeAdv node, bool loadWhenNecessary = true)
		{
			if (node == null) return null;
			ResourceNode resNode = node.Tag as ResourceNode;
			if (resNode == null) return null;
			if (!resNode.ResLink.IsLoaded && !loadWhenNecessary) return null;

			// Determine applying open actions
			var actions = DualityEditorApp.GetEditorActions(resNode.ResType, new[] { resNode.ResLink.Res }, DualityEditorApp.ActionContextOpenRes);

			// Perform first open action
			return actions.FirstOrDefault();
		}

		protected void AppendNodesToData(DataObject data, IEnumerable<TreeNodeAdv> nodes)
		{
			data.SetData(nodes.ToArray());
			data.SetContentRefs(
				from c in nodes
				where c.Tag is ResourceNode
				select (c.Tag as ResourceNode).ResLink as IContentRef);
			data.SetFiles(
				from c in nodes
				where c.Tag is NodeBase && !Resource.IsDefaultContentPath((c.Tag as NodeBase).NodePath)
				select (c.Tag as NodeBase).NodePath);
		}

		protected void DisplayErrorMoveFile(string targetPath)
		{
			NodeBase conflictNode = this.NodeFromPath(targetPath);
			if (conflictNode != null)
			{
				this.FlashNode(conflictNode);
				System.Media.SystemSounds.Beep.Play();
			}
			else
			{
				MessageBox.Show(
					String.Format(Properties.ProjectViewRes.ProjectFolderView_MsgBox_CantMove_Text, targetPath), 
					Properties.ProjectViewRes.ProjectFolderView_MsgBox_CantMove_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
		}
		protected void DisplayErrorRenameFile(string conflictPath)
		{
			NodeBase conflictNode = this.NodeFromPath(conflictPath);
			if (conflictNode != null)
			{
				this.FlashNode(conflictNode);
				System.Media.SystemSounds.Beep.Play();
			}
			else
			{
				MessageBox.Show(
					String.Format(Properties.ProjectViewRes.ProjectFolderView_MsgBox_CantRename_Text, Path.GetFileNameWithoutExtension(conflictPath)), 
					Properties.ProjectViewRes.ProjectFolderView_MsgBox_CantRename_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
		}
		protected bool DisplayConfirmDeleteSelectedFiles()
		{
			DialogResult result = MessageBox.Show(
				Properties.ProjectViewRes.ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Text, 
				Properties.ProjectViewRes.ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Caption, 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);
			return result == DialogResult.Yes;
		}
		protected void DisplayFileImportDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Title = Properties.ProjectViewRes.ProjectFolderView_ImportFileDialog_Title,
				Multiselect = true,
				InitialDirectory = this.importSourcePath ?? Path.GetFullPath(DualityApp.DataDirectory)
			};
			
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string destinationPath = this.GetInsertActionTargetBasePath(
					this.folderView.SelectedNode != null ? this.folderView.SelectedNode.Tag as NodeBase : null);

				this.HandleFileImport(destinationPath, dialog.FileNames);

				// Save the source directory; the next time the user opens the import
				// dialog, the initial directory will be set to this.
				this.importSourcePath = Path.GetDirectoryName(
					dialog.FileNames.Last());
			}
		}

		protected void SkipGlobalRenameEventFor(string path)
		{
			path = Path.GetFullPath(path);
			if (!this.skipGlobalRenamePath.Contains(path))
				this.skipGlobalRenamePath.Add(path);
		}
		protected void RevokeGlobalRenameEventFor(string path)
		{
			path = Path.GetFullPath(path);
			this.skipGlobalRenamePath.Remove(path);
		}
		protected string GetInsertActionTargetBasePath(NodeBase baseNode)
		{
			if (baseNode == null || Resource.IsDefaultContentPath(baseNode.NodePath))
				return DualityApp.DataDirectory;

			string baseTargetPath = baseNode.NodePath;
			if (File.Exists(baseTargetPath))
				baseTargetPath = Path.GetDirectoryName(baseTargetPath);
			baseTargetPath = Path.GetFullPath(baseTargetPath);
			return baseTargetPath;
		}
		
		protected void InitContextMenu()
		{
			this.nodeContextModel = new MenuModel();
			this.nodeContextView = new MenuStripMenuView(this.contextMenuNode.Items);
			this.nodeContextView.ItemSortComparison = this.ContextMenuItemComparison;
			this.nodeContextView.Model = this.nodeContextModel;

			this.nodeContextModel.AddItems(new MenuModelItem[]
			{
				new MenuModelItem
				{
					Name			= "TopSeparator",
					SortValue		= MenuModelItem.SortValue_UnderTop - 1,
					TypeHint		= MenuItemTypeHint.Separator
				},
				this.nodeContextItemNew = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_New,
					SortValue		= MenuModelItem.SortValue_UnderTop,
					Items			= new MenuModelItem[]
					{
						new MenuModelItem
						{
							Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Folder,
							Icon			= Properties.Resources.folder,
							SortValue		= MenuModelItem.SortValue_Top,
							ActionHandler	= this.folderToolStripMenuItem_Click
						},
						new MenuModelItem
						{
							Name			= "TopSeparator",
							SortValue		= MenuModelItem.SortValue_Top,
							TypeHint		= MenuItemTypeHint.Separator
						}
					}
				},
				this.nodeContextItemImport = new MenuModelItem
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Import,
					SortValue		= MenuModelItem.SortValue_UnderTop + 1,
					ActionHandler	= this.importToolStripMenuItem_Click
				},
				new MenuModelItem
				{
					Name			= "UnderTopSeparator",
					SortValue		= MenuModelItem.SortValue_UnderTop + 1,
					TypeHint		= MenuItemTypeHint.Separator
				},
				this.nodeContextItemCut = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Cut,
					Icon			= Properties.Resources.cut,
					ShortcutKeys	= Keys.Control | Keys.X,
					ActionHandler	= this.cutToolStripMenuItem_Click
				},
				this.nodeContextItemCopy = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Copy,
					Icon			= Properties.Resources.page_copy,
					ShortcutKeys	= Keys.Control | Keys.C,
					ActionHandler	= this.copyToolStripMenuItem_Click
				},
				this.nodeContextItemPaste = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Paste,
					Icon			= Properties.Resources.page_paste,
					ShortcutKeys	= Keys.Control | Keys.V,
					ActionHandler	= this.pasteToolStripMenuItem_Click
				},
				this.nodeContextItemDelete = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Delete,
					Icon			= Properties.Resources.cross,
					ShortcutKeys	= Keys.Delete,
					ActionHandler	= this.deleteToolStripMenuItem_Click
				},
				this.nodeContextItemRename = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_Rename,
					ActionHandler	= this.renameToolStripMenuItem_Click
				},
				new MenuModelItem
				{
					Name			= "BottomSeparator",
					SortValue		= MenuModelItem.SortValue_Bottom,
					TypeHint		= MenuItemTypeHint.Separator
				},
				this.nodeContextItemShowInExplorer = new MenuModelItem 
				{
					Name			= Properties.ProjectViewRes.ProjectFolderView_ContextItemName_ShowInExplorer,
					SortValue		= MenuModelItem.SortValue_Bottom,
					ActionHandler	= this.showInExplorerToolStripMenuItem_Click
				}
			});
		}
		protected void UpdateContextMenu()
		{
			// Update main actions
			this.UpdateContextMenuCommonActions();

			// Provide custom actions
			this.UpdateContextMenuCustomActions();

			// Populate the "New" menu with Resource Types
			this.UpdateContextMenuCreationActions();
		}
		private void UpdateContextMenuCommonActions()
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.folderView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);

			bool noSelect = selNodeData.Count == 0;
			bool singleSelect = selNodeData.Count == 1;
			bool multiSelect = selNodeData.Count > 1;
			bool anyReadOnly = this.folderView.SelectedNodes.Any(viewNode => (viewNode.Tag as NodeBase).ReadOnly);

			this.nodeContextItemNew.Visible = !anyReadOnly && !multiSelect;
			this.nodeContextItemImport.Visible = !anyReadOnly && !multiSelect;

			this.nodeContextItemCut.Visible = !noSelect && !anyReadOnly;
			this.nodeContextItemCopy.Visible = !noSelect && !anyReadOnly;
			this.nodeContextItemPaste.Visible = !anyReadOnly;
			this.nodeContextItemPaste.Enabled = this.ClipboardCanPasteNodes(this.folderView.SelectedNode);
			this.nodeContextItemDelete.Visible = !noSelect && !anyReadOnly;
			this.nodeContextItemRename.Visible = !noSelect && !anyReadOnly;
			this.nodeContextItemRename.Enabled = singleSelect;

			this.nodeContextItemShowInExplorer.Visible = singleSelect && !anyReadOnly;
		}
		private void UpdateContextMenuCustomActions()
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.folderView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);
			List<IContentRef> selResData = (
				from n in selNodeData
				where n is ResourceNode
				select (n as ResourceNode).ResLink).ToList();

			// Determine the mutual Type of all selected items
			Type mainResType = null;
			if (selResData.Any())
			{
				mainResType = selResData.First().ResType;
				foreach (var resRef in selResData)
				{
					Type resType = resRef.ResType;
					while (mainResType != null && !mainResType.IsAssignableFrom(resType))
						mainResType = mainResType.BaseType;
				}
			}
				
			// Prepare old entries for removal
			HashSet<MenuModelItem> oldItems = new HashSet<MenuModelItem>();
			foreach (MenuModelItem item in this.nodeContextModel.Items)
			{
				if (item.Tag is IEditorAction)
					oldItems.Add(item);
			}

			// Add items for the currently available actions
			if (mainResType != null)
			{
				var customActions = DualityEditorApp.GetEditorActions(mainResType, selResData.Res()).ToArray();
				foreach (IEditorAction actionEntry in customActions)
				{
					// Create an item for the current action
					MenuModelItem item = this.nodeContextModel.RequestItem(actionEntry.Name, newItem =>
					{
						newItem.Icon = actionEntry.Icon;
						newItem.ActionHandler = this.customResourceActionItem_Click;
						newItem.Tag = actionEntry;
						newItem.SortValue = MenuModelItem.SortValue_Top;
					});

					// Flag item as still in use
					oldItems.Remove(item);
				}
			}

			// Remove old entries that are not used anymore
			this.nodeContextModel.RemoveItems(oldItems);
		}
		private void UpdateContextMenuCreationActions()
		{
			// Prepare old entries for removal
			HashSet<MenuModelItem> oldItems = new HashSet<MenuModelItem>();
			foreach (MenuModelItem item in this.nodeContextItemNew.ItemsDeep)
			{
				if (item.Tag is CreateContextEntryTag)
					oldItems.Add(item);
			}
			
			// Add items for the currently available types
			var resourceTypeQuery =
				from t in DualityApp.GetAvailDualityTypes(typeof(Resource))
				where !t.IsAbstract
				select t;
			foreach (TypeInfo resType in resourceTypeQuery)
			{
				// Skip invisible Types
			    EditorHintFlagsAttribute editorHintFlags = resType.GetAttributesCached<EditorHintFlagsAttribute>().FirstOrDefault();
			    if (editorHintFlags != null && editorHintFlags.Flags.HasFlag(MemberFlags.Invisible)) continue;

				// Create an item tree for the current Type
				string[] categoryTree = resType.GetEditorCategory();
				string[] fullNameTree = categoryTree.Concat(new[] { resType.Name }).ToArray();
				MenuModelItem item = this.nodeContextItemNew.RequestItem(fullNameTree, newItem =>
				{
					if (newItem.Name == resType.Name)
					{
						newItem.Name = resType.Name;
						newItem.Icon = ResourceNode.GetTypeImage(resType);
						newItem.Tag = new CreateContextEntryTag { TypeId = resType.GetTypeId(), IsDualityType = resType.Assembly == typeof(DualityApp).Assembly };
						newItem.ActionHandler = this.newToolStripMenuItem_ItemClicked;
					}
					else
					{
						newItem.Tag = new CreateContextEntryTag { TypeId = null, IsDualityType = resType.Assembly == typeof(DualityApp).Assembly };
					}
				});

				// Flag item as still in use
				while (item != null)
				{
					oldItems.Remove(item);
					item = item.Parent;
				}
			}

			// Remove old entries that are not used anymore
			foreach (MenuModelItem item in oldItems)
			{
				item.Parent.RemoveItem(item);
			}
		}

		private IEnumerable<string> HandleFileImport(string dropBaseDir, IEnumerable<string> incomingFiles)
		{
			// List all non-Resource files that are candidates to start an import operation
			string mutualBaseDir = PathHelper.GetMutualBaseDirectory(incomingFiles);
			List<string> nonResFiles = new List<string>();
			foreach (string path in incomingFiles)
			{
				// If it's a directory, enumerate all non-Resource files inside
				if (Directory.Exists(path))
				{
					foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
					{
						// Skip Resource files
						if (Resource.IsResourceFile(file))
							continue;

						nonResFiles.Add(file);
					}
				}
				else if (File.Exists(path))
				{
					// Skip Resource files
					if (Resource.IsResourceFile(path))
						continue;

					nonResFiles.Add(path);
				}
			}

			// Do we have any import candidates? Start an import operation.
			if (nonResFiles.Count > 0)
			{
				// Import Resources...
				AssetImportOutput[] importedResources;
				importedResources = AssetManager.ImportAssets(nonResFiles, dropBaseDir, mutualBaseDir);

				// ...and schedule them for selection later
				this.folderView.ClearSelection();
				foreach (AssetImportOutput output in importedResources)
				{
					this.ScheduleSelect(output.Resource.Path);
				}

				// If we imported something, consider all incoming files handled. Don't perform
				// any other operations simultaneously to an Asset import, this is just confusing.
				return Enumerable.Empty<string>();
			}

			// By default, just return the incoming files as is, unfiltered.
			return incomingFiles;
		}

		private void textBoxFilter_TextChanged(object sender, EventArgs e)
		{
			this.ApplyNodeFilter();
		}
		private bool folderModel_IsNodeVisible(object obj)
		{
			if (this.tempUpperFilter == null) return true;
			TreeNodeAdv vn = obj as TreeNodeAdv;
			Node n = vn != null ? vn.Tag as Node : obj as Node;
			if (n == null) return true;
			bool isVisible;
			if (!this.tempNodeVisibilityCache.TryGetValue(n, out isVisible))
			{
				isVisible = n.Text.ToUpper().Contains(this.tempUpperFilter);
				if (!isVisible) isVisible = n.Nodes.Any(sub => this.folderModel_IsNodeVisible(sub));
				this.tempNodeVisibilityCache[n] = isVisible;
			}
			return isVisible;
		}
		private void folderView_Expanding(object sender, TreeViewAdvEventArgs e)
		{
			NodeBase node = e.Node.Tag as NodeBase;
			if (node != null) foreach (NodeBase childNode in node.Nodes) childNode.NotifyVisible();
		}
		private void folderView_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			if (e.Node == null) return;
			this.OpenResource(e.Node);
		}
		private void folderView_SelectionChanged(object sender, EventArgs e)
		{
			// Update context menu
			this.UpdateContextMenuCommonActions();
			
			if (!DualityEditorApp.IsSelectionChanging)
			{
				// If the left mouse button is pressed, reschedule the selection for later - this 
				// might become a dragdrop operation, for which we do no wish selection changes.
				if ((Control.MouseButtons & MouseButtons.Left) != MouseButtons.None)
				{
					this.tempScheduleSelectionChange = true;
				}
				// Otherwise, go ahead and adjust editor-wide selection
				else
				{
					// Retrieve selected ResourceNodes
					ResourceNode[] selResNode = (
						from vn in this.folderView.SelectedNodes
						where vn.Tag is ResourceNode
						select vn.Tag as ResourceNode
						).ToArray();
					// Load their Resource data, if not loaded yet
					Resource[] selRes = (
						from rn in selResNode
						where rn.ResLink.IsAvailable
						select rn.ResLink.Res
						).ToArray();

					if (selRes.Length > 0)
						DualityEditorApp.Select(this, new ObjectSelection(selRes));
					else
						DualityEditorApp.Deselect(this, ObjectSelection.Category.Resource);
				}
			}
		}
		private void folderView_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.folderView.SelectedNodes.Count > 0)
			{
				// Navigate left / collapse
				if (e.KeyCode == Keys.Back)
				{
					int lowLevel = this.folderView.SelectedNodes.Min(viewNode => viewNode.Level);
					TreeNodeAdv lowLevelNode = this.folderView.SelectedNodes.First(viewNode => viewNode.Level == lowLevel);

					if (this.folderView.SelectedNode.IsExpanded)
						this.folderView.SelectedNode.Collapse();
					else if (lowLevel > 1)
						this.folderView.SelectedNode = lowLevelNode.Parent;
				}
				// Navigate right / expand
				else if (e.KeyCode == Keys.Return)
				{
					if (!this.folderView.SelectedNode.IsExpanded)
						this.folderView.SelectedNode.Expand();
				}
			}
		}
		private void folderView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (this.folderView.SelectedNodes.Count > 0)
			{
				// If we've scheduled a selection change, un-schedule it. We don't
				// want to change selection because of dragdrop operations.
				this.tempScheduleSelectionChange = false;

				// Perform the dragdrop operation
				DataObject dragDropData = new DataObject();
				this.AppendNodesToData(dragDropData, this.folderView.SelectedNodes);
				this.DoDragDrop(dragDropData, DragDropEffects.All | DragDropEffects.Link);
			}
		}
		private void folderView_DragOver(object sender, DragEventArgs e)
		{
			NodeBase baseTarget = this.DragDropGetTargetBaseNode();
			ResourceNode targetResNode = baseTarget as ResourceNode;
			DirectoryNode targetDirNode = baseTarget as DirectoryNode;
			string baseTargetPath = this.GetInsertActionTargetBasePath(baseTarget);
			DataObject data = e.Data as DataObject;
			ConvertOperation.Operation convOp = data.GetAllowedConvertOp();
			if (data != null)
			{
				GameObject[] draggedObjects;
				// Dragging files around
				if (data.ContainsFileDropList())
				{
					if (Directory.Exists(baseTargetPath))
					{
						// Do not accept drag if target is located in source
						bool targetInSource = false;
						foreach (string srcPath in data.GetFileDropList())
						{
							if (PathOp.IsPathLocatedIn(baseTargetPath, srcPath))
							{
								targetInSource = true;
								break;
							}
						}
						
						DragDropEffects effect;
						if (targetInSource)
							effect = DragDropEffects.None;
						else if ((e.KeyState & 2) != 0)		// Right mouse button
							effect = DragDropEffects.Move | DragDropEffects.Copy;
						else if ((e.KeyState & 32) != 0)	// Alt key
							effect = DragDropEffects.Copy;
						else
							effect = DragDropEffects.Move;
						e.Effect = effect & e.AllowedEffect;
					}
				}
				// Dragging a single GameObject to Prefab
				else if (
					e.AllowedEffect.HasFlag(DragDropEffects.Link) &&
					data.TryGetGameObjects(DataObjectStorage.Reference, out draggedObjects) &&
					draggedObjects.Length == 1 &&
					targetResNode != null && 
					targetResNode.ResLink.Is<Duality.Resources.Prefab>())
				{
					e.Effect = DragDropEffects.Link & e.AllowedEffect;
				}
				// See if we can retrieve Resources from data
				else if (
					(baseTarget == null || !baseTarget.ReadOnly) &&
					(e.AllowedEffect.HasFlag(DragDropEffects.Copy) || e.AllowedEffect.HasFlag(DragDropEffects.Move)) &&
					(convOp.HasFlag(ConvertOperation.Operation.CreateRes) || convOp.HasFlag(ConvertOperation.Operation.CreateObj)))
				{
					bool canSelectResource = new ConvertOperation(data, ConvertOperation.Operation.All).CanPerform<IContentRef>();
					if (canSelectResource) e.Effect = DragDropEffects.Copy & e.AllowedEffect;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}

			this.folderView.HighlightDropPosition = (e.Effect != DragDropEffects.None);
		}
		private void folderView_DragDrop(object sender, DragEventArgs e)
		{
			this.folderView.BeginUpdate();

			bool effectMove = (e.Effect & DragDropEffects.Move) != DragDropEffects.None;
			bool effectCopy = (e.Effect & DragDropEffects.Copy) != DragDropEffects.None;

			NodeBase baseTarget = this.DragDropGetTargetBaseNode();
			ResourceNode targetResNode = baseTarget as ResourceNode;
			DirectoryNode targetDirNode = baseTarget as DirectoryNode;
			this.tempDropBasePath = this.GetInsertActionTargetBasePath(baseTarget);
			DataObject data = e.Data as DataObject;
			ConvertOperation.Operation convOp = data.GetAllowedConvertOp();
			if (data != null)
			{
				GameObject[] draggedObjects;
				// Dropping files
				if (data.ContainsFileDropList())
				{
					IEnumerable<string> incomingFiles = data.GetFileDropList().OfType<string>();

					// Handle file import operations and filter out files that have been handled that way.
					incomingFiles = this.HandleFileImport(this.tempDropBasePath, incomingFiles);

					// Filter out non-Resource files that might have been dropped accidentally into the data directory
					incomingFiles = incomingFiles.Where(path => Resource.IsResourceFile(path) || Directory.Exists(path));

					// If there's anything left, proceed with a regular file drop operation
					if (incomingFiles.Any())
					{
						this.tempFileDropList = incomingFiles.ToList();
						
						// Display context menu if both moving and copying are availabled
						if (effectMove && effectCopy)
							this.contextMenuDragMoveCopy.Show(this, this.PointToClient(new Point(e.X, e.Y)));
						else if (effectCopy)
							this.copyHereToolStripMenuItem_Click(this, null);
						else if (effectMove)
							this.moveHereToolStripMenuItem_Click(this, null);
					}
				}
				// Dropping GameObject to Prefab
				else if (
					e.Effect.HasFlag(DragDropEffects.Link) &&
					data.TryGetGameObjects(DataObjectStorage.Reference, out draggedObjects) &&
					draggedObjects.Length == 1 && 
					targetResNode != null && 
					targetResNode.ResLink.Is<Duality.Resources.Prefab>())
				{
					Prefab prefab = targetResNode.ResLink.Res as Prefab;
					if (prefab != null)
					{
						GameObject draggedObj = draggedObjects[0];

						if (draggedObj != null)
						{
						UndoRedoManager.BeginMacro();
						// Prevent recursion
						UndoRedoManager.Do(new BreakPrefabLinkAction(draggedObj.GetChildrenDeep().Where(c => c.PrefabLink != null && c.PrefabLink.Prefab == prefab)));
						// Inject GameObject to Prefab & Establish PrefabLink
						UndoRedoManager.Do(new ApplyToPrefabAction(draggedObj, prefab));
						UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromLast);
					}
				}
				}
				// See if we can retrieve Resources from data
				else if (
					(baseTarget == null || !baseTarget.ReadOnly) &&
					(effectCopy || effectMove) &&
					(convOp.HasFlag(ConvertOperation.Operation.CreateRes) || convOp.HasFlag(ConvertOperation.Operation.CreateObj)))
				{
					var resQuery = new ConvertOperation(data, ConvertOperation.Operation.All).Perform<IContentRef>();
					if (resQuery != null)
					{
						// Save or move generated Resources
						List<Resource> resList = resQuery.Res().ToList();
						this.folderView.ClearSelection();
						foreach (Resource res in resList)
						{
							string desiredName = null;
							if (string.IsNullOrEmpty(desiredName) && res.Path != null)
								desiredName = res.Name;
							if (string.IsNullOrEmpty(desiredName) && res.AssetInfo != null)
								desiredName = res.AssetInfo.NameHint;
							if (string.IsNullOrEmpty(desiredName))
								desiredName = res.GetType().Name;

							bool pointsToFile = !res.IsDefaultContent && !res.IsRuntimeResource;
							string basePath = this.GetInsertActionTargetBasePath(targetDirNode);
							string nameExt = Resource.GetFileExtByType(res.GetType());
							string resPath = Path.Combine(basePath, desiredName) + nameExt;
							if (!pointsToFile || Path.GetFullPath(resPath) != Path.GetFullPath(res.Path))
								resPath = PathHelper.GetFreePath(Path.Combine(basePath, desiredName), nameExt);
							resPath = PathHelper.MakeFilePathRelative(resPath);

							if (pointsToFile && File.Exists(res.Path))
								File.Move(res.Path, resPath);
							else
								res.Save(resPath);

							this.ScheduleSelect(resPath);
						}

						// If we happened to generate a Prefab, link possible existing GameObjects to it
						if (resList.OfType<Prefab>().Any())
						{
							UndoRedoManager.Do(new ApplyToPrefabAction(data.GetGameObjects(), resList.OfType<Prefab>().Ref()));
						}

					}
				}
			}

			this.folderView.EndUpdate();
		}
		private void folderView_Leave(object sender, EventArgs e)
		{
			this.folderView.Invalidate();
		}
		private void folderView_Enter(object sender, EventArgs e)
		{
			this.tempScheduleSelectionChange = true;
		}
		private void folderView_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.tempScheduleSelectionChange)
			{
				this.tempScheduleSelectionChange = false;
				this.folderView_SelectionChanged(this.folderView, EventArgs.Empty);
			}
		}
		private void treeColumn_DrawColHeaderBg(object sender, DrawColHeaderBgEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 212, 212, 212)), e.Bounds);
			e.Handled = true;
		}
		private NodeBase DragDropGetTargetBaseNode()
		{
			TreeNodeAdv dropViewNode		= this.folderView.DropPosition.Node;
			TreeNodeAdv dropViewNodeParent	= (dropViewNode != null && this.folderView.DropPosition.Position != NodePosition.Inside) ? dropViewNode.Parent : dropViewNode;
			NodeBase dropNodeParent			= (dropViewNodeParent != null) ? dropViewNodeParent.Tag as NodeBase : null;
			return dropNodeParent;
		}

		private void nodeTextBoxName_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			NodeBase node = e.Node.Tag as NodeBase;
			ResourceNode resNode = node as ResourceNode;

			// Readonly nodes
			if (node.ReadOnly)
				e.TextColor = Color.FromArgb(192, 0, 0, 32);
			else
				e.TextColor = Color.Black;

			// Unsaved nodes
			if (resNode != null && DualityEditorApp.IsResourceUnsaved(resNode.ResLink))
				e.Font = this.treeFontItalic;

			// Flashing
			if (node == this.flashNode && !e.Context.Bounds.IsEmpty)
			{
				float intLower = this.flashIntensity;
				Color hlBase = Color.FromArgb(224, 64, 32);
				Color hlLower = Color.FromArgb(
					(int)(this.folderView.BackColor.R * (1.0f - intLower) + hlBase.R * intLower),
					(int)(this.folderView.BackColor.G * (1.0f - intLower) + hlBase.G * intLower),
					(int)(this.folderView.BackColor.B * (1.0f - intLower) + hlBase.B * intLower));
				e.BackgroundBrush = new SolidBrush(hlLower);
			}
		}
		private void nodeTextBoxName_EditorShowing(object sender, CancelEventArgs e)
		{
			NodeBase node = this.folderView.SelectedNode != null ? this.folderView.SelectedNode.Tag as NodeBase : null;
			if (node == null || node.ReadOnly) e.Cancel = true;
			if (!e.Cancel)
			{
				this.lastEditedNode = node;
				this.folderView.ContextMenuStrip = null;
			}
		}
		private void nodeTextBoxName_EditorHided(object sender, EventArgs e)
		{
			this.folderView.ContextMenuStrip = this.contextMenuNode;
		}
		private void nodeTextBoxName_ChangesApplied(object sender, EventArgs e)
		{
			NodeBase node = this.lastEditedNode;
			string oldName = node.GetNameFromPath(node.NodePath);

			// This piece of code blocks the user from setting an empty string as a Resource's name,
			// avoiding the appearence of the behavior described in https://github.com/AdamsLair/duality/issues/773
			// If an invalid name is set, it is reverted back to the previous one, and the Resource 
			// flashes to warn the user
			if (string.IsNullOrWhiteSpace(node.Text))
			{
				node.Text = oldName;
				this.FlashNode(node);
				this.RevokeGlobalRenameEventFor(node.NodePath);
				return;
			}

			if (oldName == node.Text)
			{
				this.RevokeGlobalRenameEventFor(node.NodePath);
				return;
			}

			Node parentNode = node.Parent;
			this.UnregisterNodeTree(node);
			node.Parent.Nodes.Remove(node);

			string conflictPath;
			if (!node.ApplyNameToPath(out conflictPath))
			{
				if (conflictPath != null && NodeBase.GetNodePathId(conflictPath) != node.NodePathId)
					this.DisplayErrorRenameFile(conflictPath);
				node.Text = null;
				node.ApplyPathToName();
			}

			this.InsertNodeSorted(node, parentNode);
			this.RegisterNodeTree(node);
		}
		private void nodeTextBoxType_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			e.TextColor = Color.FromArgb(128, 0, 0, 0);
			e.BackgroundBrush = null;
		}
		private void timerFlashItem_Tick(object sender, EventArgs e)
		{
			this.flashDuration += (this.timerFlashItem.Interval / 1000.0f);
			this.flashIntensity = 1.0f - (this.flashDuration % 0.33f) / 0.33f;
			this.folderView.Invalidate();

			if (this.flashDuration > 0.66f)
			{
				this.flashNode = null;
				this.flashIntensity = 0.0f;
				this.flashDuration = 0.0f;
				this.timerFlashItem.Enabled = false;
			}
		}
		
		private void copyHereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool anyOperationPerformed = false;
			foreach (string p in this.tempFileDropList)
			{
				string srcPath = Path.GetFullPath(p);
				string dstPath =  Path.GetFullPath(Path.Combine(this.tempDropBasePath, Path.GetFileName(p)));

				// Clone if target equals source
				if (srcPath == dstPath)
				{
					string dstPathBase = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(p));
					string dstPathExt = Path.GetFileName(p).Remove(0, dstPathBase.Length);
					dstPathBase = Path.Combine(this.tempDropBasePath, dstPathBase);
					dstPath = PathHelper.GetFreePath(dstPathBase, dstPathExt);
				}
				// Skip if target is located inside source
				if (PathOp.IsPathLocatedIn(dstPath, srcPath)) continue;

				// From here, continue with relative destination path
				dstPath = PathHelper.MakeFilePathRelative(dstPath);

				// Move or copy files / directories
				bool errorMove = false;
				if (File.Exists(srcPath) && !File.Exists(dstPath))
					File.Copy(srcPath, dstPath);
				else if (Directory.Exists(srcPath) && !Directory.Exists(dstPath))
					PathHelper.CopyDirectory(srcPath, dstPath);
				else
					errorMove = true;
						
				// Display error message if moving wasn't possible
				if (errorMove) this.DisplayErrorMoveFile(dstPath);

				// Adjust / Initialize cloned Resource
				if (Resource.IsResourceFile(dstPath))
				{
					IContentRef tempResRef = ContentProvider.RequestContent(dstPath);
					tempResRef.Res.AssetInfo = null;
					tempResRef.Res.Save();
				}

				this.ScheduleSelect(dstPath);
				anyOperationPerformed = true;
			}

			if (anyOperationPerformed)
			{
				this.folderView.ClearSelection();
			}
		}
		private void moveHereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool anyOperationPerformed = false;
			string dataDirPath = Path.GetFullPath(DualityApp.DataDirectory);
			foreach (string p in this.tempFileDropList)
			{
				string srcPath = Path.GetFullPath(p);
				string dstPath =  Path.GetFullPath(Path.Combine(this.tempDropBasePath, Path.GetFileName(p)));
				bool localAction = PathOp.IsPathLocatedIn(srcPath, dataDirPath);

				// Skip if target equals source
				if (srcPath == dstPath) continue;
				// Skip if target is located inside source
				if (PathOp.IsPathLocatedIn(dstPath, srcPath)) continue;

				// From here, continue with relative destination path
				dstPath = PathHelper.MakeFilePathRelative(dstPath);

				// Move or copy files / directories
				bool errorMove = false;
				if (localAction)
				{
					if (File.Exists(srcPath) && !File.Exists(dstPath))
						File.Move(srcPath, dstPath);
					else if (Directory.Exists(srcPath) && !Directory.Exists(dstPath))
						Directory.Move(srcPath, dstPath);
					else
						errorMove = true;
				}
				else
				{
					if (File.Exists(srcPath) && !File.Exists(dstPath))
						File.Copy(srcPath, dstPath);
					else if (Directory.Exists(srcPath) && !Directory.Exists(dstPath))
						PathHelper.CopyDirectory(srcPath, dstPath);
					else
						errorMove = true;
				}
						
				// Display error message if moving wasn't possible
				if (errorMove) this.DisplayErrorMoveFile(dstPath);

				this.ScheduleSelect(dstPath);
				anyOperationPerformed = true;
			}

			if (anyOperationPerformed)
			{
				this.folderView.ClearSelection();
			}
		}

		private void contextMenuNode_Opening(object sender, CancelEventArgs e)
		{
			bool anyReadOnly = this.folderView.SelectedNodes.Any(viewNode => (viewNode.Tag as NodeBase).ReadOnly);
			if (anyReadOnly) 
			{ 
				e.Cancel = true; 
				return;
			}

			this.UpdateContextMenu();
		}

		private void importToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.DisplayFileImportDialog();
		}
		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClipboardCutNodes(this.folderView.SelectedNodes);
		}
		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClipboardCopyNodes(this.folderView.SelectedNodes);
		}
		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClipboardPasteNodes(this.folderView.SelectedNode);
		}
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.DeleteNodes(this.folderView.SelectedNodes);
		}
		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.nodeTextBoxName.BeginEdit();
		}
		private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditorHelper.ShowInExplorer((this.folderView.SelectedNode.Tag as NodeBase).NodePath);
		}

		private void folderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CreateFolder(this.folderView.SelectedNode);
		}
		private void newToolStripMenuItem_ItemClicked(object sender, EventArgs e)
		{
			MenuModelItem clickedItem = sender as MenuModelItem;
			if (clickedItem == null) return;
			if (!(clickedItem.Tag is CreateContextEntryTag)) return;
			
			CreateContextEntryTag clickedEntry = clickedItem.Tag as CreateContextEntryTag;
			Type clickedType = ReflectionHelper.ResolveType(clickedEntry.TypeId);
			if (clickedType == null) return;

			this.CreateResource(clickedType, this.folderView.SelectedNode);
		}
		private void customResourceActionItem_Click(object sender, EventArgs e)
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.folderView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);
			List<IContentRef> selResData = (
				from n in selNodeData
				where n is ResourceNode
				select (n as ResourceNode).ResLink).ToList();

			MenuModelItem clickedItem = sender as MenuModelItem;
			IEditorAction action = clickedItem.Tag as IEditorAction;
			action.Perform(selResData.Select(resRef => resRef.Res));
		}

		private void toolStripButtonWorkDir_Click(object sender, EventArgs e)
		{
			string filePath = Path.GetFullPath(DualityApp.DataDirectory);
			string argument = filePath;
			System.Diagnostics.Process.Start("explorer.exe", argument);
		}
		
		private void DualityEditorApp_HighlightObject(object sender, HighlightObjectEventArgs e)
		{
			if (!e.Mode.HasFlag(HighlightMode.Conceptual)) return;
			if (sender == this) return;

			Resource res = e.Target.MainResource;
			NodeBase node = null;
			if (res != null) node = this.NodeFromPath(res.Path);
			if (node != null) this.FlashNode(node);
		}
		private void EditorForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender == this) return;
			if ((e.AffectedCategories & ObjectSelection.Category.Resource) == ObjectSelection.Category.None) return;
			if (e.SameObjects) return;

			foreach (Resource r in e.Removed.Resources)	this.SelectNode(this.NodeFromPath(r.Path), false);
			foreach (Resource r in e.Added.Resources)
			{
				if (!this.SelectNode(this.NodeFromPath(r.Path)))
					this.ScheduleSelect(r.Path);
			}
		}
		private void EditorForm_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			// If a Resources modified state changes, invalidate
			if (e.Objects.Resources.Any())
			{
				// Since every "current Scene" change will be a Resource change,
				// we'd redraw all the time, which can decrease editor performance.
				// Instead, keep track of the Resources that have been marked unsaved
				// to avoid duplicate redraws.
				bool anyVisibleStateChange = false;
				foreach (Resource res in e.Objects.Resources)
				{
					if (DualityEditorApp.IsResourceUnsaved(res) && this.unsavedResRedrawCache.Add(res.Path))
						anyVisibleStateChange = true;
				}

				// Did we encounter any visible state change? Redraw if that's the case.
				if (anyVisibleStateChange)
				{
					this.folderView.Invalidate();
				}
			}
		}
		private void FileEventManager_ResourcesChanged(object sender, ResourceFilesChangedEventArgs e)
		{
			foreach (FileEvent item in e.FileEvents)
			{
				if (item.Type == FileEventType.Created)
				{
					// Don't add Resources that are already present.
					bool alreadyAdded = this.NodeFromPath(item.Path) != null;

					if (!alreadyAdded)
					{
						// Register newly detected Resource file
						if (File.Exists(item.Path))
						{
							NodeBase newNode = this.ScanFile(item.Path);

							Node parentNode = this.NodeFromPath(Path.GetDirectoryName(item.Path));
							if (parentNode == null) parentNode = this.folderModel.Root;

							this.InsertNodeSorted(newNode, parentNode);
							this.RegisterNodeTree(newNode);
							newNode.NotifyVisible();
						}
						// Add new directory tree
						else if (item.IsDirectory)
						{
							// Actually, only add the directory itsself. Each file will trigger its own ResourceCreated event
							DirectoryNode newNode = new DirectoryNode(item.Path);
							//NodeBase newNode = this.ScanDirectory(e.Path);

							Node parentNode = this.NodeFromPath(Path.GetDirectoryName(item.Path));
							if (parentNode == null) parentNode = this.folderModel.Root;

							this.InsertNodeSorted(newNode, parentNode);
							this.RegisterNodeTree(newNode);
							newNode.NotifyVisible();
						}
					}

					// Perform previously scheduled selection
					this.PerformScheduleSelect(Path.GetFullPath(item.Path));
				}
				else if (item.Type == FileEventType.Deleted)
				{
					// Remove lost Resource file
					NodeBase node = this.NodeFromPath(item.Path);
					if (node != null)
					{
						this.UnregisterNodeTree(node);
						node.Parent.Nodes.Remove(node);
					}
				}
				else if (item.Type == FileEventType.Changed)
				{
					// If a Prefab has been modified, update its appearance
					ContentRef<Resource> content = new ContentRef<Resource>(item.Path);
					if (!item.IsDirectory && content.Is<Prefab>())
					{
						ResourceNode modifiedNode = this.NodeFromPath(item.Path) as ResourceNode;
						if (modifiedNode != null) modifiedNode.UpdateImage();
					}
				}
				else if (item.Type == FileEventType.Renamed)
				{
					NodeBase node = this.NodeFromPath(item.OldPath);
					bool scanResFile = false;

					// Modify existing node
					if (node != null)
					{
						string newDirectory = Path.GetDirectoryName(item.Path);
						bool moved = !PathOp.ArePathsEqual(Path.GetDirectoryName(item.OldPath), newDirectory);

						// If its a file, remove and add it again
						if (File.Exists(item.Path))
						{
							// Remove it
							this.UnregisterNodeTree(node);
							node.Parent.Nodes.Remove(node);

							// Register it
							scanResFile = true;
						}
						// Otherwise: Rename node according to file
						else
						{
							this.UnregisterNodeTree(node);
							if (moved)
							{
								node.Parent.Nodes.Remove(node);
								Node newParent = this.NodeFromPath(newDirectory) ?? this.folderModel.Root;
								this.InsertNodeSorted(node, newParent);
							}
							node.NodePath = item.Path;
							node.ApplyPathToName();
							this.RegisterNodeTree(node);
						}
					}
					// Register newly detected Resource file
					else if (this.NodeFromPath(item.Path) == null)
					{
						scanResFile = true;
					}

					// If neccessary, check if the file is a Resource file and add it, if yes
					if (scanResFile && Resource.IsResourceFile(item.Path))
					{
						node = this.ScanFile(item.Path);

						Node parentNode = this.NodeFromPath(Path.GetDirectoryName(item.Path));
						if (parentNode == null) parentNode = this.folderModel.Root;

						this.InsertNodeSorted(node, parentNode);
						this.RegisterNodeTree(node);
						node.NotifyVisible();
					}

					// Perform previously scheduled selection
					this.PerformScheduleSelect(Path.GetFullPath(item.Path));
				}
			}
		}
		private void FileEventManager_BeginGlobalRename(object sender, BeginGlobalRenameEventArgs e)
		{
			if (this.skipGlobalRenamePath.Remove(Path.GetFullPath(e.OldPath)))
				e.Cancel();
		}
		private void Resource_ResourceSaved(object sender, ResourceSaveEventArgs e)
		{
			// If a Resources modified state changes, invalidate
			this.folderView.Invalidate();
			this.unsavedResRedrawCache.Remove(e.SaveAsPath);
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;
			Point globalPos = this.PointToScreen(localPos);

			// Hovering context menu
			if (this.contextMenuNode.Visible)
			{
				ToolStripItem item = this.contextMenuNode.GetItemAtDeep(globalPos);
				if (item != null)
				{
					// "Create Resource"
					if (item.Tag is CreateContextEntryTag)
						result = HelpInfo.FromMember(ReflectionHelper.ResolveType((item.Tag as CreateContextEntryTag).TypeId));
					// Editor Actions
					else if (item.Tag is IEditorAction && (item.Tag as IEditorAction).HelpInfo != null)
						result = (item.Tag as IEditorAction).HelpInfo;
					// A HelpInfo attached to the item
					else if (item.Tag is HelpInfo)
						result = item.Tag as HelpInfo;
					// An ordinary items Tooltip
					else if (item.ToolTipText != null)
						result = HelpInfo.FromText(item.Text, item.ToolTipText);
				}
				captured = true;
			}
			// Hovering Resource nodes
			else
			{
				Point treeLocalPos = this.folderView.PointToClient(globalPos);
				if (this.folderView.ClientRectangle.Contains(treeLocalPos))
				{
					TreeNodeAdv viewNode = this.folderView.GetNodeAt(treeLocalPos);
					ResourceNode resNode = viewNode != null ? viewNode.Tag as ResourceNode : null;
					if (resNode != null)
					{
						// Resource type info
						result = HelpInfo.FromResource(resNode.ResLink);
					}
					else
					{
						// An ordinary items Tooltip
						string tooltip = (this as IToolTipProvider).GetToolTip(viewNode, null);
						if (tooltip != null)
						{
							result = HelpInfo.FromText(resNode.Text, tooltip);
						}
					}
				}
				captured = false;
			}

			return result;
		}
		string IToolTipProvider.GetToolTip(TreeNodeAdv viewNode, Aga.Controls.Tree.NodeControls.NodeControl nodeControl)
		{
			IEditorAction action = this.GetResourceOpenAction(viewNode, false);
			if (action != null && action.HelpInfo != null)
			{
				return string.Format(
					Duality.Editor.Plugins.ProjectView.Properties.ProjectViewRes.ProjectFolderView_Help_Doubleclick,
					action.HelpInfo.Description);
			}
			else
			{
				return null;
			}
		}

		private int ContextMenuItemComparison(IMenuModelItem itemA, IMenuModelItem itemB)
		{
			int result;

			// SortValue overrides all
			result = itemA.SortValue - itemB.SortValue;
			if (result != 0) return result;

			// Don't sort any further unless within the new menu
			bool isInNewMenu = false;
			IMenuModelItem item = itemA;
			while (item != null)
			{
				if (item == this.nodeContextItemNew)
				{
					isInNewMenu = true;
					break;
				}
				item = item.Parent;
			}
			if (!isInNewMenu) return 0;
			
			CreateContextEntryTag createEntryA = itemA.Tag as CreateContextEntryTag;
			CreateContextEntryTag createEntryB = itemB.Tag as CreateContextEntryTag;

			// Duality-internal Types first
			result = 
				((createEntryB != null && createEntryB.IsDualityType) ? 1 : 0) -
				((createEntryA != null && createEntryA.IsDualityType) ? 1 : 0);
			if (result != 0) return result;

			// Type entries first
			result = 
				((createEntryB != null && createEntryB.TypeId != null) ? 1 : 0) - 
				((createEntryA != null && createEntryA.TypeId != null) ? 1 : 0);
			if (result != 0) return result;

			// Sort by Item Name
			result = string.Compare(itemA.Name, itemB.Name);
			return result;
		}
	}
}
