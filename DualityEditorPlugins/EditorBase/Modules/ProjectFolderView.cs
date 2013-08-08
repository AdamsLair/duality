using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using Aga.Controls.Tree;

using Duality;
using Duality.Resources;

using DualityEditor;
using DualityEditor.CorePluginInterface;
using DualityEditor.UndoRedoActions;

namespace EditorBase
{
	public partial class ProjectFolderView : DockContent, IHelpProvider, IToolTipProvider
	{
		public abstract class NodeBase : Node
		{
			public static string GetNodePathId(string nodePath)
			{
				if (nodePath.Contains(':'))
					return nodePath.ToUpper();
				else
					return Path.GetFullPath(nodePath).ToUpper();
			}

			private	string	nodePath		= null;
			private	bool	readOnly		= false;

			public IEnumerable<NodeBase> NodesDeep
			{
				get
				{
					foreach (NodeBase n in this.Nodes.OfType<NodeBase>())
					{
						yield return n;
						foreach (NodeBase c in n.NodesDeep)
							yield return c;
					}
				}
			}
			public string NodePath
			{
				get { return this.nodePath; }
				set
				{
					if (this.nodePath != value)
					{
						string oldPath = this.nodePath;
						this.nodePath = value;
						this.OnNodePathChanged(oldPath);
					}
				}
			}
			public string NodePathId
			{
				get { return GetNodePathId(this.NodePath); }
			}
			public bool ReadOnly
			{
				get { return this.readOnly; }
			}
			public virtual string TypeName
			{
				get { return null; }
			}

			protected NodeBase(string path, string name, bool readOnly = false) : base(name)
			{
				this.nodePath = path;
				this.readOnly = readOnly;
			}
			
			public void NotifyVisible()
			{
				this.OnFirstVisible();
			}
			public void ApplyPathToName()
			{
				this.Text = this.GetNameFromPath(this.nodePath);
			}
			public bool ApplyNameToPath()
			{
				string outVar;
				return this.ApplyNameToPath(out outVar);
			}
			public virtual bool ApplyNameToPath(out string conflictingPath)
			{
				conflictingPath = null;
				return false;
			}
			public virtual string GetNameFromPath(string path)
			{
				if (path.Contains(':'))
				{
					string[] pathSplit = path.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
					return pathSplit[pathSplit.Length - 1];
				}
				else
				{
					return path;
				}
			}

			protected virtual void OnNodePathChanged(string oldPath)
			{

			}
			protected virtual void OnFirstVisible() {}
		}
		public class DirectoryNode : NodeBase
		{
			public DirectoryNode(string path, bool readOnly = false) : base(path, null, readOnly)
			{
				this.ApplyPathToName();
			}
			
			public override bool ApplyNameToPath(out string conflictingPath)
			{
				conflictingPath = null;
 				if (this.ReadOnly) return false;

				string oldPath = this.NodePath;
				string oldDirName = Path.GetFileName(oldPath);
				string newPathBase = oldPath.Remove(oldPath.Length - oldDirName.Length, oldDirName.Length);
				string newPath = newPathBase + this.Text;
				bool equalsCaseInsensitive = newPath.ToUpper() == oldPath.ToUpper();

				if (Directory.Exists(newPath) && !equalsCaseInsensitive)
				{
					conflictingPath = newPath;
					return false;
				}

				if (equalsCaseInsensitive)
				{
					string tempPath = newPath + "_sSJencn83rhfSHhfn3ns456omvmvs28fndDN84ns";
					Directory.Move(oldPath, tempPath);
					Directory.Move(tempPath, newPath);
				}
				else
					Directory.Move(oldPath, newPath);

				// Between performing the move event and it being received by the FileEventManager there will be a
				// short window of inconsistency where the existing Resource is still registered under its old name
				// but the file is already renamed to the new name. To prevent loading the Resource twice, we'll pre-register
				// it under its new name.
				foreach (ResourceNode resNode in this.NodesDeep.OfType<ResourceNode>())
				{
					if (resNode.ResLink.ResWeak != null)
						ContentProvider.RegisterContent(resNode.NodePath.Replace(oldPath, newPath), resNode.ResLink.ResWeak);
				}

				this.NodePath = newPath;
				return true;
			}
			public override string GetNameFromPath(string path)
			{
				if (!path.Contains(':'))
					return Path.GetFileName(path);
				else
					return base.GetNameFromPath(path);
			}

			protected override void OnNodePathChanged(string oldPath)
			{
				base.OnNodePathChanged(oldPath);
				foreach (NodeBase node in this.Nodes)
				{
					node.NodePath = this.NodePath + node.NodePath.Remove(0, oldPath.Length);
				}
			}
		}
		public class ResourceNode : NodeBase
		{
			private	IContentRef	res		= ContentRef<Resource>.Null;
			private	Type		resType	= null;

			public IContentRef ResLink
			{
				get { return this.res; }
			}
			public Type ResType
			{
				get { return this.resType; }
			}
			public override string TypeName
			{
				get { return this.resType != null ? this.resType.GetTypeKeyword() : null; }
			}

			public ResourceNode(string path) : base(path, null, false)
			{
				string[] fileNameSplit = Path.GetFileNameWithoutExtension(path).Split('.');

				this.res.Path = path;
				this.resType = Resource.GetTypeByFileName(path);
				this.ApplyPathToName();
			}
			public ResourceNode(IContentRef res) : base(res.Path, null, res.Path.Contains(':'))
			{
				this.res = res;
				this.resType = res.ResType;
				this.ApplyPathToName();
			}

			public void UpdateImage()
			{
				this.Image = GetTypeImage(this.resType, this.res);				
			}

			public override bool ApplyNameToPath(out string conflictingPath)
			{
				conflictingPath = null;
				if (this.ReadOnly) return false;

				string oldPath = this.NodePath;
				string oldFileName = Path.GetFileName(oldPath);
				string newPathBase = oldPath.Remove(oldPath.Length - oldFileName.Length, oldFileName.Length);
				string newPath = newPathBase + this.Text + Resource.GetFileExtByType(this.resType);
				bool equalsCaseInsensitive = newPath.ToUpper() == oldPath.ToUpper();

				if (File.Exists(newPath) && !equalsCaseInsensitive)
				{
					conflictingPath = newPath;
					return false;
				}

				if (equalsCaseInsensitive)
				{
					string tempPath = newPath + "_sSJencn83rhfSHhfn3ns456omvmvs28fndDN84ns";
					File.Move(oldPath, tempPath);
					File.Move(tempPath, newPath);
				}
				else
					File.Move(oldPath, newPath);

				// Between performing the move event and it being received by the FileEventManager there will be a
				// short window of inconsistency where the existing Resource is still registered under its old name
				// but the file is already renamed to the new name. To prevent loading the Resource twice, we'll pre-register
				// it under its new name.
				if (this.res.ResWeak != null) ContentProvider.RegisterContent(newPath, this.res.ResWeak);

				this.NodePath = newPath;
				return true;
			}
			public override string GetNameFromPath(string path)
			{
				if (!path.Contains(':'))
				{
					string fileName = Path.GetFileNameWithoutExtension(path);
					string[] fileNameSplit = fileName.Split('.');
					return fileNameSplit[0];
				}
				else
					return base.GetNameFromPath(path);
			}

			protected override void OnNodePathChanged(string oldPath)
			{
				base.OnNodePathChanged(oldPath);
				this.res.Path = this.NodePath;
			}
			protected override void OnFirstVisible()
			{
				base.OnFirstVisible();
				this.UpdateImage();
			}

			public static Image GetTypeImage(Type type, IContentRef resLink = null)
			{
				if (resLink == null) resLink = ContentRef<Resource>.Null;

				Image result = null;
				if (type == typeof(Duality.Resources.Prefab))
				{
					bool prefabHasContent = (resLink.IsAvailable && (resLink.Res as Duality.Resources.Prefab).ContainsData);
					result = CorePluginRegistry.GetTypeImage(type, prefabHasContent ? 
						CorePluginRegistry.ImageContext_Icon + "_Full" : 
						CorePluginRegistry.ImageContext_Icon);
				}
				else
				{
					result = CorePluginRegistry.GetTypeImage(type);
				}

				return result ?? PluginRes.EditorBaseResCache.IconResUnknown;
			}
		}
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


		private	Dictionary<string,NodeBase>	pathIdToNode	= new Dictionary<string,NodeBase>();
		private	TreeModel					folderModel		= null;
		private	NodeBase					lastEditedNode	= null;
		private System.Drawing.Font			treeFontItalic	= null;

		private	NodeBase	flashNode		= null;
		private	float		flashDuration	= 0.0f;
		private	float		flashIntensity	= 0.0f;

		private	List<ScheduleSelectEntry>	scheduleSelectPath		= new List<ScheduleSelectEntry>();
		private	List<string>				skipGlobalRenamePath	= new List<string>();

		private	Dictionary<Node,bool>	tempNodeVisibilityCache	= new Dictionary<Node,bool>();
		private	string					tempUpperFilter			= null;
		private	string					tempDropBasePath		= null;
		private	StringCollection		tempFileDropList		= null;
		private bool	tempScheduleSelectionChange	= false;


		public ProjectFolderView()
		{
			this.InitializeComponent();

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

			this.toolStrip.Renderer = new DualityEditor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.InitRessources();
			DualityEditorApp.SelectionChanged += this.EditorForm_SelectionChanged;
			FileEventManager.ResourceCreated += this.FileEventManager_ResourceCreated;
			FileEventManager.ResourceDeleted += this.FileEventManager_ResourceDeleted;
			FileEventManager.ResourceModified += this.FileEventManager_ResourceModified;
			FileEventManager.ResourceRenamed += this.FileEventManager_ResourceRenamed;
			FileEventManager.BeginGlobalRename += this.FileEventManager_BeginGlobalRename;
			DualityEditorApp.ObjectPropertyChanged += this.EditorForm_ObjectPropertyChanged;
			Resource.ResourceSaved += this.Resource_ResourceSaved;
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			DualityEditorApp.SelectionChanged -= this.EditorForm_SelectionChanged;
			FileEventManager.ResourceCreated -= this.FileEventManager_ResourceCreated;
			FileEventManager.ResourceDeleted -= this.FileEventManager_ResourceDeleted;
			FileEventManager.ResourceModified -= this.FileEventManager_ResourceModified;
			FileEventManager.ResourceRenamed -= this.FileEventManager_ResourceRenamed;
			FileEventManager.BeginGlobalRename -= this.FileEventManager_BeginGlobalRename;
			DualityEditorApp.ObjectPropertyChanged -= this.EditorForm_ObjectPropertyChanged;
			Resource.ResourceSaved -= this.Resource_ResourceSaved;
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

		protected IEnumerable<Type> QueryResourceTypes()
		{
			return 
				from t in DualityApp.GetAvailDualityTypes(typeof(Resource))
				where !t.IsAbstract
				select t;
		}

		protected void InitRessources()
		{
			this.toolStripLabelProjectName.Text = String.Format("{0}: {1}",
				PluginRes.EditorBaseRes.ProjectNameLabel,
				EditorHelper.CurrentProjectName);

			Node nodeTree = this.ScanDirectory(DualityApp.DataDirectory);
			nodeTree.Nodes.Insert(0, this.ScanDefaultContent());

			this.folderView.BeginUpdate();
			this.ClearRessources();
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
		protected void ClearRessources()
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
		protected NodeBase ScanDefaultRessource(ContentRef<Resource> resRef)
		{
			if (!resRef.IsAvailable) return null;
			return new ResourceNode(resRef);
		}
		protected DirectoryNode ScanDefaultContent()
		{
			DirectoryNode thisNode = new DirectoryNode(ContentProvider.VirtualContentPath, true);

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

				// Generate virtual ressource node
				NodeBase res = this.ScanDefaultRessource(resRef);
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
					this.tempFileDropList = data.GetFileDropList();

					// Retrieve preferred drop effect (Windows system stuff)
					MemoryStream dropEffect = data.GetData("Preferred DropEffect") as MemoryStream;
					bool move = false;
					if (dropEffect != null)
					{
						byte[] moveEffect = new byte[4];
						dropEffect.Read(moveEffect, 0, 4);
						move = moveEffect[0] == 2 && moveEffect[1] == 0 && moveEffect[2] == 0 && moveEffect[3] == 0;
					}

					// Display context menu if both moving and copying are available
					if (move)
					{
						this.moveHereToolStripMenuItem_Click(this, null);
						Clipboard.Clear();
					}
					else
						this.copyHereToolStripMenuItem_Click(this, null);
				}
			}

			this.folderView.EndUpdate();
		}
		protected void DeleteNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			nodes = nodes.ToList();
			bool allReadOnly = nodes.All(viewNode => (viewNode.Tag as NodeBase).ReadOnly);
			
			if (allReadOnly || !this.DisplayConfirmDeleteSelectedFiles()) return;

			var nodeQuery = 
				from viewNode in nodes
				select this.folderModel.FindNode(this.folderView.GetPath(viewNode)) as NodeBase;

			foreach (NodeBase nodeBase in nodeQuery)
			{
				if (nodeBase.ReadOnly) continue;
				RecycleBin.Send(nodeBase.NodePath);
			}
		}
		protected void CreateFolder(TreeNodeAdv baseNode)
		{
			string basePath = this.GetInsertActionTargetBasePath(baseNode != null ? baseNode.Tag as NodeBase : null);
			string dirPath = PathHelper.GetFreePath(Path.Combine(basePath, PluginRes.EditorBaseRes.NewFolderName), "");

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

			string basePath = this.GetInsertActionTargetBasePath(baseNode);
			string nameExt = Resource.GetFileExtByType(type);
			string resPath = PathHelper.GetFreePath(Path.Combine(basePath, desiredName), nameExt);
			resPath = PathHelper.MakeFilePathRelative(resPath);

			Resource resInstance = ReflectionHelper.CreateInstanceOf(type) as Resource;
			resInstance.Save(resPath);

			// Schedule path for later selection - as soon as it actually exists.
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
				openAction.Perform(resNode.ResLink.Res);
			}
		}
		protected IEditorAction GetResourceOpenAction(TreeNodeAdv node)
		{
			if (node == null) return null;
			ResourceNode resNode = node.Tag as ResourceNode;
			if (resNode == null) return null;

			// Determine applying open actions
			var actions = CorePluginRegistry.GetEditorActions(resNode.ResType, CorePluginRegistry.ActionContext_OpenRes, new[] { resNode.ResLink.Res });

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
				where c.Tag is NodeBase && !(c.Tag as NodeBase).NodePath.Contains(':')
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
					String.Format(PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_CantMove_Text, targetPath), 
					PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_CantMove_Caption, 
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
					String.Format(PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_CantRename_Text, Path.GetFileNameWithoutExtension(conflictPath)), 
					PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_CantRename_Caption, 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
		}
		protected bool DisplayConfirmDeleteSelectedFiles()
		{
			DialogResult result = MessageBox.Show(
				PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Text, 
				PluginRes.EditorBaseRes.ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Caption, 
				MessageBoxButtons.YesNo, 
				MessageBoxIcon.Question);
			return result == DialogResult.Yes;
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
			string baseTargetPath = (baseNode != null) ? baseNode.NodePath : DualityApp.DataDirectory;
			if (!baseTargetPath.Contains(':'))
			{
				if (File.Exists(baseTargetPath)) baseTargetPath = Path.GetDirectoryName(baseTargetPath);
				baseTargetPath = Path.GetFullPath(baseTargetPath);
			}

			return baseTargetPath;
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

			// Some ResourceNodes might need to be loaded to display additional information.
			// After loading the selected Resources, update their image according to possibly available new information
			//foreach (ResourceNode resNode in selResNode)
			//	resNode.UpdateImage();
			// Note: Removed this. They'll now just grab their resource if available as soon as they need their data.

			// Adjust editor-wide selection
			if (!DualityEditorApp.IsSelectionChanging)
			{
				if (selRes.Length > 0)
					DualityEditorApp.Select(this, new ObjectSelection(selRes));
				else
					DualityEditorApp.Deselect(this, ObjectSelection.Category.Resource);
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
				// Dragging files around
				if (data.ContainsFileDropList())
				{
					if (Directory.Exists(baseTargetPath))
					{
						// Do not accept drag if target is located in source
						bool targetInSource = false;
						foreach (string srcPath in data.GetFileDropList())
						{
							if (PathHelper.IsPathLocatedIn(baseTargetPath, srcPath))
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
					data.ContainsGameObjectRefs() &&
					targetResNode != null && 
					targetResNode.ResLink.Is<Duality.Resources.Prefab>() && 
					data.GetGameObjectRefs().Length == 1)
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
				// Dropping files
				if (data.ContainsFileDropList())
				{
					this.tempFileDropList = data.GetFileDropList();

					// Display context menu if both moving and copying are availabled
					if (effectMove && effectCopy)
						this.contextMenuDragMoveCopy.Show(this, this.PointToClient(new Point(e.X, e.Y)));
					else if (effectCopy)
						this.copyHereToolStripMenuItem_Click(this, null);
					else if (effectMove)
						this.moveHereToolStripMenuItem_Click(this, null);
				}
				// Dropping GameObject to Prefab
				else if (
					e.Effect.HasFlag(DragDropEffects.Link) &&
					data.ContainsGameObjectRefs() &&
					targetResNode != null && 
					targetResNode.ResLink.Is<Duality.Resources.Prefab>() && 
					data.GetGameObjectRefs().Length == 1)
				{
					Prefab prefab = targetResNode.ResLink.Res as Prefab;
					if (prefab != null)
					{
						GameObject draggedObj = data.GetGameObjectRefs()[0];

						UndoRedoManager.BeginMacro();
						// Prevent recursion
						UndoRedoManager.Do(new BreakPrefabLinkAction(draggedObj.ChildrenDeep.Where(c => c.PrefabLink != null && c.PrefabLink.Prefab == prefab)));
						// Inject GameObject to Prefab & Establish PrefabLink
						UndoRedoManager.Do(new ApplyToPrefabAction(draggedObj, prefab));
						UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromLast);
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
							string desiredName = res.SourcePath != null ? Path.GetFileNameWithoutExtension(res.SourcePath) : res.Name;
							if (string.IsNullOrEmpty(desiredName)) desiredName = res.GetType().Name;

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
							UndoRedoManager.Do(new ApplyToPrefabAction(data.GetGameObjectRefs(), resList.OfType<Prefab>().Ref()));
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
			NodeBase node = this.folderView.SelectedNode.Tag as NodeBase;
			if (node.ReadOnly) e.Cancel = true;
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
				if (NodeBase.GetNodePathId(conflictPath) != node.NodePathId) this.DisplayErrorRenameFile(conflictPath);
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
			foreach (string p in this.tempFileDropList)
			{
				string srcPath = Path.GetFullPath(p);
				string dstPath =  Path.GetFullPath(Path.Combine(this.tempDropBasePath, Path.GetFileName(p)));

				// Clone if target equals source
				if (srcPath == dstPath)
				{
					string dstPathBase = Path.GetFileNameWithoutExtension(p);
					dstPathBase = Path.GetFileNameWithoutExtension(dstPathBase);
					dstPathBase = Path.Combine(this.tempDropBasePath, dstPathBase);
					string dstPathExt = p.Remove(0, dstPathBase.Length);
					dstPath = PathHelper.GetFreePath(dstPathBase, dstPathExt);
				}
				// Skip if target is located inside source
				if (PathHelper.IsPathLocatedIn(dstPath, srcPath)) continue;

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
					tempResRef.Res.SourcePath = null;
					tempResRef.Res.Save();
				}

				this.ScheduleSelect(dstPath);
			}

			this.folderView.ClearSelection();
		}
		private void moveHereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string dataDirPath = Path.GetFullPath(DualityApp.DataDirectory);
			foreach (string p in this.tempFileDropList)
			{
				string srcPath = Path.GetFullPath(p);
				string dstPath =  Path.GetFullPath(Path.Combine(this.tempDropBasePath, Path.GetFileName(p)));
				bool localAction = srcPath.StartsWith(dataDirPath);

				// Skip if target equals source
				if (srcPath == dstPath) continue;
				// Skip if target is located inside source
				if (PathHelper.IsPathLocatedIn(dstPath, srcPath)) continue;

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
			}

			this.folderView.ClearSelection();
		}

		private void contextMenuNode_Opening(object sender, CancelEventArgs e)
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.folderView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);
			List<IContentRef> selResData = (
				from n in selNodeData
				where n is ResourceNode
				select (n as ResourceNode).ResLink).ToList();

			bool noSelect = selNodeData.Count == 0;
			bool singleSelect = selNodeData.Count == 1;
			bool multiSelect = selNodeData.Count > 1;
			bool anyReadOnly = this.folderView.SelectedNodes.Any(viewNode => (viewNode.Tag as NodeBase).ReadOnly);


			if (anyReadOnly) 
			{ 
				e.Cancel = true; 
				return;
			}

			this.newToolStripMenuItem.Visible = !anyReadOnly && !multiSelect;
			this.toolStripSeparatorNew.Visible = !anyReadOnly && !multiSelect;

			this.renameToolStripMenuItem.Visible = !noSelect && !anyReadOnly;
			this.cutToolStripMenuItem.Visible = !noSelect && !anyReadOnly;
			this.copyToolStripMenuItem.Visible = !noSelect && !anyReadOnly;
			this.deleteToolStripMenuItem.Visible = !noSelect && !anyReadOnly;

			this.pasteToolStripMenuItem.Visible = !anyReadOnly;

			this.pasteToolStripMenuItem.Enabled = this.ClipboardCanPasteNodes(this.folderView.SelectedNode);
			this.renameToolStripMenuItem.Enabled = singleSelect;

			this.showInExplorerToolStripMenuItem.Visible = singleSelect && !anyReadOnly;
			this.toolStripSeparatorShowInExplorer.Visible = singleSelect && !anyReadOnly;

			// Provide custom actions
			Type mainResType = null;
			if (selResData.Any())
			{
				mainResType = selResData.First().ResType;
				// Find mutual type
				foreach (var resRef in selResData)
				{
					Type resType = resRef.ResType;
					while (mainResType != null && !mainResType.IsAssignableFrom(resType))
						mainResType = mainResType.BaseType;
				}
			}
			for (int i = this.contextMenuNode.Items.Count - 1; i >= 0; i--)
			{
				if (this.contextMenuNode.Items[i].Tag is IEditorAction)
					this.contextMenuNode.Items.RemoveAt(i);
			}
			if (mainResType != null)
			{
				this.toolStripSeparatorCustomActions.Visible = true;
				int baseIndex = this.contextMenuNode.Items.IndexOf(this.toolStripSeparatorCustomActions);
				var customActions = CorePluginRegistry.GetEditorActions(
					mainResType, 
					CorePluginRegistry.ActionContext_ContextMenu, 
					selResData.Select(resRef => resRef.Res))
					.ToArray();
				foreach (var actionEntry in customActions)
				{
					ToolStripMenuItem actionItem = new ToolStripMenuItem(actionEntry.Name, actionEntry.Icon);
					actionItem.Click += this.customResourceActionItem_Click;
					actionItem.Tag = actionEntry;
					actionItem.ToolTipText = actionEntry.Description;
					this.contextMenuNode.Items.Insert(baseIndex, actionItem);
					baseIndex++;
				}
				if (customActions.Length == 0) this.toolStripSeparatorCustomActions.Visible = false;
			}
			else
				this.toolStripSeparatorCustomActions.Visible = false;

			// Reset "New" menu to original state
			List<ToolStripItem> oldItems = new List<ToolStripItem>(this.newToolStripMenuItem.DropDownItems.OfType<ToolStripItem>());
			this.newToolStripMenuItem.DropDownItems.Clear();
			foreach (ToolStripItem item in oldItems.Skip(2)) item.Dispose();
			this.newToolStripMenuItem.DropDownItems.AddRange(oldItems.Take(2).ToArray());
			
			// Create dynamic entries
			List<ToolStripItem> newItems = new List<ToolStripItem>();
			foreach (Type resType in this.QueryResourceTypes())
			{
				// Generate category item
				string[] category = CorePluginRegistry.GetTypeCategory(resType);
				ToolStripMenuItem categoryItem = this.newToolStripMenuItem;
				for (int i = 0; i < category.Length; i++)
				{
					ToolStripMenuItem subCatItem;
					if (categoryItem == this.newToolStripMenuItem)
						subCatItem = newItems.FirstOrDefault(item => item.Name == category[i]) as ToolStripMenuItem;
					else
						subCatItem = categoryItem.DropDownItems.Find(category[i], false).FirstOrDefault() as ToolStripMenuItem;

					if (subCatItem == null)
					{
						subCatItem = new ToolStripMenuItem(category[i]);
						subCatItem.Name = category[i];
						subCatItem.Tag = resType.Assembly;
						subCatItem.DropDownItemClicked += this.newToolStripMenuItem_DropDownItemClicked;
						if (categoryItem == this.newToolStripMenuItem)
							EditorBasePlugin.InsertToolStripTypeItem(newItems, subCatItem);
						else
							EditorBasePlugin.InsertToolStripTypeItem(categoryItem.DropDownItems, subCatItem);
					}
					categoryItem = subCatItem;
				}

				ToolStripMenuItem resTypeItem = new ToolStripMenuItem(resType.Name, ResourceNode.GetTypeImage(resType));
				resTypeItem.Tag = resType;
				if (categoryItem == this.newToolStripMenuItem)
					EditorBasePlugin.InsertToolStripTypeItem(newItems, resTypeItem);
				else
					EditorBasePlugin.InsertToolStripTypeItem(categoryItem.DropDownItems, resTypeItem);
			}
			this.newToolStripMenuItem.DropDownItems.AddRange(newItems.ToArray());
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
			string filePath = Path.GetFullPath((this.folderView.SelectedNode.Tag as NodeBase).NodePath);
			string argument = @"/select, " + filePath;
			System.Diagnostics.Process.Start("explorer.exe", argument);
		}

		private void folderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CreateFolder(this.folderView.SelectedNode);
		}
		private void newToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == this.folderToolStripMenuItem) return;
			if (e.ClickedItem.Tag as Type == null) return;
			Type clickedType = e.ClickedItem.Tag as Type;
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

			ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
			IEditorAction action = clickedItem.Tag as IEditorAction;
			action.Perform(selResData.Select(resRef => resRef.Res));
		}

		private void toolStripButtonWorkDir_Click(object sender, EventArgs e)
		{
			string filePath = Path.GetFullPath(DualityApp.DataDirectory);
			string argument = filePath;
			System.Diagnostics.Process.Start("explorer.exe", argument);
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
				this.folderView.Invalidate();
				foreach (Prefab prefab in e.Objects.Resources.OfType<Prefab>())
				{
					ResourceNode resNode = this.NodeFromPath(prefab.Path) as ResourceNode;
					if (resNode == null) continue;
					resNode.UpdateImage();
				}
			}
		}
		private void FileEventManager_ResourceCreated(object sender, ResourceEventArgs e)
		{
			bool alreadyAdded = this.NodeFromPath(e.Path) != null; // Don't add Resources that are already present.

			if (!alreadyAdded)
			{
				// Register newly detected ressource file
				if (File.Exists(e.Path) && Resource.IsResourceFile(e.Path))
				{
					NodeBase newNode = this.ScanFile(e.Path);

					Node parentNode = this.NodeFromPath(Path.GetDirectoryName(e.Path));
					if (parentNode == null) parentNode = this.folderModel.Root;
					
					this.InsertNodeSorted(newNode, parentNode);
					this.RegisterNodeTree(newNode);
					newNode.NotifyVisible();
				}
				// Add new directory tree
				else if (e.IsDirectory)
				{
					// Actually, only add the directory itsself. Each file will trigger its own ResourceCreated event
					DirectoryNode newNode = new DirectoryNode(e.Path);
					//NodeBase newNode = this.ScanDirectory(e.Path);

					Node parentNode = this.NodeFromPath(Path.GetDirectoryName(e.Path));
					if (parentNode == null) parentNode = this.folderModel.Root;

					this.InsertNodeSorted(newNode, parentNode);
					this.RegisterNodeTree(newNode);
					newNode.NotifyVisible();
				}
			}

			// Perform previously scheduled selection
			this.PerformScheduleSelect(Path.GetFullPath(e.Path));
		}
		private void FileEventManager_ResourceDeleted(object sender, ResourceEventArgs e)
		{
			// Remove lost ressource file
			NodeBase node = this.NodeFromPath(e.Path);
			if (node != null)
			{
				this.UnregisterNodeTree(node);
				node.Parent.Nodes.Remove(node);
			}
		}
		private void FileEventManager_ResourceModified(object sender, ResourceEventArgs e)
		{
			// If a Prefab has been modified, update its appearance
			if (e.IsResource && e.Content.Is<Duality.Resources.Prefab>())
			{
				ResourceNode modifiedNode = this.NodeFromPath(e.Content.Path) as ResourceNode;
				if (modifiedNode != null) modifiedNode.UpdateImage();
			}
		}
		private void FileEventManager_ResourceRenamed(object sender, ResourceRenamedEventArgs e)
		{
			NodeBase node = this.NodeFromPath(e.OldPath);
			bool registerRes = false;

			// Modify existing node
			if (node != null)
			{
				// If its a file, remove and add it again
				if (File.Exists(e.Path))
				{
					// Remove it
					this.UnregisterNodeTree(node);
					node.Parent.Nodes.Remove(node);

					// Register it
					registerRes = true;
				}
				// Otherwise: Rename node according to file
				else
				{
					this.UnregisterNodeTree(node);
					node.NodePath = e.Path;
					node.ApplyPathToName();
					this.RegisterNodeTree(node);
				}
			}
			// Register newly detected ressource file
			else if (this.NodeFromPath(e.Path) == null)
			{
				registerRes = true;
			}

			// If neccessary, check if the file is a ressource file and add it, if yes
			if (registerRes && Resource.IsResourceFile(e.Path))
			{
				node = this.ScanFile(e.Path);

				Node parentNode = this.NodeFromPath(Path.GetDirectoryName(e.Path));
				if (parentNode == null) parentNode = this.folderModel.Root;

				this.InsertNodeSorted(node, parentNode);
				this.RegisterNodeTree(node);
				node.NotifyVisible();
			}

			// Perform previously scheduled selection
			this.PerformScheduleSelect(Path.GetFullPath(e.Path));
		}
		private void FileEventManager_BeginGlobalRename(object sender, BeginGlobalRenameEventArgs e)
		{
			if (this.skipGlobalRenamePath.Remove(Path.GetFullPath(e.OldPath)))
				e.Cancel = true;
		}
		private void Resource_ResourceSaved(object sender, ResourceEventArgs e)
		{
			// If a Resources modified state changes, invalidate
			this.folderView.Invalidate();
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;
			Point globalPos = this.PointToScreen(localPos);

			// Hovering context menu
			if (this.contextMenuNode.Visible)
			{
				ToolStripItem item = this.newToolStripMenuItem.DropDown.GetItemAtDeep(globalPos);
				Type itemType = item != null ? item.Tag as Type : null;
				if (itemType != null)
				{
					// "Create Resource"
					result = HelpInfo.FromMember(itemType);
				}
				else 
				{
					// An ordinary items Tooltip
					item = this.contextMenuNode.GetItemAtDeep(globalPos);
					if (item != null && item.ToolTipText != null)
					{
						result = HelpInfo.FromText(item.Text, item.ToolTipText);
					}
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
			IEditorAction action = this.GetResourceOpenAction(viewNode);
			if (action != null) return string.Format(
				EditorBase.PluginRes.EditorBaseRes.ProjectFolderView_Help_Doubleclick,
				action.Description);
			else return null;
		}
	}
}
