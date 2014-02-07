using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Xml;
using CancelEventHandler = System.ComponentModel.CancelEventHandler;
using CancelEventArgs = System.ComponentModel.CancelEventArgs;

using WeifenLuo.WinFormsUI.Docking;
using Aga.Controls.Tree;

using Duality;
using Duality.Resources;
using Duality.ColorFormat;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.CorePluginInterface;
using DualityEditor.UndoRedoActions;

using EditorBase.PluginRes;

namespace EditorBase
{
	public partial class SceneView : DockContent, IHelpProvider, IToolTipProvider
	{
		private class ToolTipProvider : IToolTipProvider
		{
			public string GetToolTip(TreeNodeAdv node, Aga.Controls.Tree.NodeControls.NodeControl nodeControl)
			{
				NodeBase dataNode = node.Tag as NodeBase;
				GameObjectNode objNode = dataNode as GameObjectNode;

				if (dataNode.LinkState == NodeBase.PrefabLinkState.None) return null;
				if (objNode == null) return null;
				if (objNode.Obj.PrefabLink == null) return null;

				return String.Format(PluginRes.EditorBaseRes.SceneView_PrefabLink, objNode.Obj.PrefabLink.Prefab.Path);
			}
		}

		public abstract class NodeBase : Node
		{
			public enum PrefabLinkState
			{
				None,
				Active,
				Broken
			}

			private PrefabLinkState linkState = PrefabLinkState.None;
			private string linkLastPath = null;
			private bool linkLastExisted = false;

			protected virtual int TypeSortIndex { get { return 0; } }
			public PrefabLinkState LinkState
			{
				get { return this.linkState; }
			}

			public NodeBase(string name) : base(name)
			{
			}
			public bool UpdateLinkState(bool checkFileChanged)
			{
				PrefabLinkState lastState = this.linkState;

				ComponentNode cmpNode = this as ComponentNode;
				GameObjectNode objNode = (cmpNode != null ? cmpNode.Parent : this) as GameObjectNode;

				PrefabLink prefabLink = objNode != null ? objNode.Obj.AffectedByPrefabLink : null;
				bool affectedByPrefabLink = prefabLink != null;
				if (cmpNode != null) affectedByPrefabLink = affectedByPrefabLink && prefabLink.AffectsObject(cmpNode.Component);
				if (objNode != null) affectedByPrefabLink = affectedByPrefabLink && prefabLink.AffectsObject(objNode.Obj);

				string filePath = affectedByPrefabLink ? prefabLink.Prefab.Path : null;
				bool fileExists = this.linkLastExisted;
				if (checkFileChanged || this.linkLastPath != filePath)
					fileExists = File.Exists(filePath);

				// Prefab-linked entities
				if (affectedByPrefabLink && fileExists) //prefabLink.Prefab.IsAvailable) // Not sufficient - might be loaded but with a broken path
					this.linkState = PrefabLinkState.Active;
				else if (cmpNode == null && objNode.Obj.PrefabLink != null)
					this.linkState = PrefabLinkState.Broken;
				else
					this.linkState = PrefabLinkState.None;

				this.linkLastExisted = fileExists;
				this.linkLastPath = filePath;
				return this.linkState != lastState;
			}

			public static int Compare(NodeBase first, NodeBase second)
			{
				return first.TypeSortIndex - second.TypeSortIndex;
			}
		}
		public class GameObjectNode : NodeBase
		{
			private	GameObject	obj				= null;
			private	bool		customIcon		= false;
			private	bool		customIconSet	= false;

			protected override int TypeSortIndex
			{
				get { return 1; }
			}
			public GameObject Obj
			{
				get { return this.obj; }
			}
			public bool UseCustomIcon
			{
				get { return this.customIcon; }
			}
			public bool CustomIconSet
			{
				get { return this.customIconSet; }
			}

			public GameObjectNode(GameObject obj, bool customIcon) : base(obj.Name)
			{
				this.obj = obj;
				this.customIcon = customIcon;
				this.UpdateIcon();
			}
			public void UpdateIcon(Component ignoreComponent = null)
			{
				if (this.customIcon)
				{
					Type representant = null;
					List<Type> availCmpTypes = this.obj.GetComponents<Component>().Where(c => c != ignoreComponent).Select(c => c.GetType()).ToList();

					if (representant == null)
					{
						// Find the most unique / significant Component
						int bestScore = int.MinValue;
						foreach (Type cmpType in availCmpTypes)
						{
							if (availCmpTypes.Any(c => Component.RequiresComponent(c, cmpType)))
								continue;
							if (CorePluginRegistry.GetTypeImage(cmpType) == null)
								continue;

							int score = 
								100000 * Component.GetRequiredComponents(cmpType).Count() -
								100 * (cmpType.Name.Length > 0 ? ((int)cmpType.Name[0] - (int)' ') : 0) -
								1 * (cmpType.Name.Length > 1 ? ((int)cmpType.Name[1] - (int)' ') : 0);
							if (score > bestScore)
							{
								bestScore = score;
								representant = cmpType;
							}
						}
					}

					this.customIconSet = (representant != null);
					Image img = CorePluginRegistry.GetTypeImage(representant ?? typeof(GameObject));
					if (this.LinkState != PrefabLinkState.None)
					{
						img = EditorHelper.GetImageWithOverlay(img, this.LinkState == PrefabLinkState.Active ? 
							EditorBaseResCache.OverlayLink : 
							EditorBaseResCache.OverlayLinkBroken);
					}
					this.Image = img;
				}
				else
				{
					Image img = CorePluginRegistry.GetTypeImage(typeof(GameObject));
					if (this.LinkState != PrefabLinkState.None)
					{
						img = EditorHelper.GetImageWithOverlay(img, this.LinkState == PrefabLinkState.Active ? 
							EditorBaseResCache.OverlayLink : 
							EditorBaseResCache.OverlayLinkBroken);
					}
					this.Image = img;
				}
			}
		}
		public class ComponentNode : NodeBase
		{
			private	Component	cmp	= null;

			protected override int TypeSortIndex
			{
				get { return 0; }				
			}
			public Component Component
			{
				get { return this.cmp; }
			}

			public ComponentNode(Component cmp) : base(cmp.GetType().Name)
			{
				this.cmp = cmp;
				this.UpdateIcon();
			}
			public void UpdateIcon()
			{
				this.Image = GetTypeImage(cmp.GetType());
			}

			public static Image GetTypeImage(Type type)
			{
				return CorePluginRegistry.GetTypeImage(type) ?? EditorBaseResCache.IconCmpUnknown;
			}
		}


		private	Dictionary<object,NodeBase>	objToNode		= new Dictionary<object,NodeBase>();
		private	TreeModel					objectModel		= null;
		private	NodeBase					lastEditedNode	= null;

		private	NodeBase	flashNode		= null;
		private	float		flashDuration	= 0.0f;
		private	float		flashIntensity	= 0.0f;

		private	object		tempDropData	= null;
		private	NodeBase	tempDropTarget	= null;
		private	Dictionary<Node,bool>	tempNodeVisibilityCache	= new Dictionary<Node,bool>();
		private	string					tempUpperFilter			= null;
		private bool		tempScheduleSelectionChange	= false;
		private bool		tempIsInitializingObjects	= false;
		private bool		tempIsClearingObjects		= false;
		private	System.Drawing.Imaging.ColorMatrix	inactiveIconMatrix;

		
		public IEnumerable<NodeBase> SelectedNodes
		{
			get
			{
				return 
					from c in this.objectView.SelectedNodes
					select c.Tag as NodeBase;
			}
		}
		public IEnumerable<ComponentNode> SelectedComponentNodes
		{
			get
			{
				return 
					from c in this.objectView.SelectedNodes
					where c.Tag is ComponentNode
					select c.Tag as ComponentNode;
			}
		}
		public IEnumerable<GameObjectNode> SelectedGameObjectNodes
		{
			get
			{
				return 
					from c in this.objectView.SelectedNodes
					where c.Tag is GameObjectNode
					select c.Tag as GameObjectNode;
			}
		}


		public SceneView()
		{
			this.InitializeComponent();

			this.inactiveIconMatrix = new System.Drawing.Imaging.ColorMatrix(new[] {
				new float[] {	0.30f,	0.30f,	0.30f,	0.0f,	0.0f	},
				new float[] {	0.59f,	0.59f,	0.59f,	0.0f,	0.0f	},
				new float[] {	0.11f,	0.11f,	0.11f,	0.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	1.0f,	0.0f	},
				new float[] {	0.0f,	0.0f,	0.0f,	0.0f,	1.0f	}});

			this.objectView.DefaultToolTipProvider = this;
			this.objectModel = new TreeModel();
			this.objectView.Model = this.objectModel;

			this.nodeTextBoxName.ToolTipProvider = this.nodeStateIcon.ToolTipProvider = new ToolTipProvider();
			this.nodeTextBoxName.DrawText += this.nodeTextBoxName_DrawText;
			this.nodeTextBoxName.EditorShowing += new CancelEventHandler(nodeTextBoxName_EditorShowing);
			this.nodeTextBoxName.EditorHided += new EventHandler(nodeTextBoxName_EditorHided);
			this.nodeTextBoxName.ChangesApplied += new EventHandler(nodeTextBoxName_ChangesApplied);

			this.nodeStateIcon.DrawIcon += this.nodeStateIcon_DrawIcon;

			this.toolStrip.Renderer = new DualityEditor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			this.InitObjects();

			DualityEditorApp.SelectionChanged += this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged += this.EditorForm_ObjectPropertyChanged;
			FileEventManager.ResourceCreated += this.EditorForm_ResourceCreated;
			FileEventManager.ResourceDeleted += this.EditorForm_ResourceDeleted;
			FileEventManager.ResourceRenamed += this.EditorForm_ResourceRenamed;

			Scene.Entered += this.Scene_Entered;
			Scene.Leaving += this.Scene_Leaving;
			Scene.GameObjectAdded += this.Scene_GameObjectRegistered;
			Scene.GameObjectRemoved += this.Scene_GameObjectUnregistered;
			Scene.GameObjectParentChanged += this.Scene_GameObjectParentChanged;
			Scene.ComponentAdded += this.Scene_ComponentAdded;
			Scene.ComponentRemoving += this.Scene_ComponentRemoving;

			if (Scene.Current != null) this.Scene_Entered(this, null);
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			DualityEditorApp.SelectionChanged -= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged -= this.EditorForm_ObjectPropertyChanged;
			FileEventManager.ResourceCreated -= this.EditorForm_ResourceCreated;
			FileEventManager.ResourceDeleted -= this.EditorForm_ResourceDeleted;
			FileEventManager.ResourceRenamed -= this.EditorForm_ResourceRenamed;

			Scene.Entered -= this.Scene_Entered;
			Scene.Leaving -= this.Scene_Leaving;
			Scene.GameObjectAdded -= this.Scene_GameObjectRegistered;
			Scene.GameObjectRemoved -= this.Scene_GameObjectUnregistered;
			Scene.GameObjectParentChanged -= this.Scene_GameObjectParentChanged;
			Scene.ComponentAdded -= this.Scene_ComponentAdded;
			Scene.ComponentRemoving -= this.Scene_ComponentRemoving;
		}

		internal void SaveUserData(XmlElement node)
		{
			node.SetAttribute("showComponents", this.buttonShowComponents.Checked.ToString(CultureInfo.InvariantCulture));
		}
		internal void LoadUserData(XmlElement node)
		{
			bool tryParseBool;

			if (bool.TryParse(node.GetAttribute("showComponents"), out tryParseBool))
				this.buttonShowComponents.Checked = tryParseBool;
		}

		public void FlashNode(NodeBase node)
		{
			if (node == null) return;

			this.flashNode = node;
			this.flashDuration = 0.0f;
			this.timerFlashItem.Enabled = true;

			this.objectView.EnsureVisible(this.objectView.FindNode(this.objectModel.GetPath(this.flashNode)));
		}
		public bool SelectNode(NodeBase node, bool select = true)
		{
			if (node == null) return false;
			TreeNodeAdv viewNode = this.objectView.FindNode(this.objectModel.GetPath(node));
			if (viewNode != null)
			{
				viewNode.IsSelected = select;
				if (select) this.objectView.EnsureVisible(viewNode);
				return true;
			}
			return false;
		}
		public void SelectNodes(IEnumerable<NodeBase> nodes, bool select = true)
		{
			this.objectView.BeginUpdate();
			TreeNodeAdv viewNode = null;
			foreach (NodeBase node in nodes.NotNull())
			{
				viewNode = this.objectView.FindNode(this.objectModel.GetPath(node));
				if (viewNode != null) viewNode.IsSelected = select;
			}
			this.objectView.EndUpdate();
			if (select && viewNode != null) this.objectView.EnsureVisible(viewNode);
		}
		public GameObjectNode FindNode(GameObject obj)
		{
			if (obj == null) return null;
			NodeBase result;
			if (!this.objToNode.TryGetValue(obj, out result)) return null;
			return result as GameObjectNode;
		}
		public ComponentNode FindNode(Component cmp)
		{
			if (cmp == null) return null;
			NodeBase result;
			if (!this.objToNode.TryGetValue(cmp, out result)) return null;
			return result as ComponentNode;
		}

		protected void ApplyNodeFilter()
		{
			this.tempUpperFilter = String.IsNullOrEmpty(this.textBoxFilter.Text) ? null : this.textBoxFilter.Text.ToUpper();
			this.tempNodeVisibilityCache.Clear();
			this.objectView.NodeFilter = this.tempUpperFilter != null ? this.objectModel_IsNodeVisible : (Predicate<object>)null;
		}

		protected IEnumerable<Type> QueryComponentTypes()
		{
			return
				from t in DualityApp.GetAvailDualityTypes(typeof(Component))
				where !t.IsAbstract
				select t;
		}

		protected void InitObjects()
		{
			if (this.tempIsInitializingObjects) return;
			this.tempIsInitializingObjects = true;

			this.UpdateSceneLabel();

			Node nodeTree = this.ScanScene(Scene.Current);


			this.objectView.BeginUpdate();
			{
				this.ClearObjects();
				while (nodeTree.Nodes.Count > 0) this.InsertNodeSorted(nodeTree.Nodes[0], this.objectModel.Root);
				this.RegisterNodeTree(this.objectModel.Root);
			}
			this.objectView.EndUpdate();

			// If there is a selection, apply it. We lost all local selection data due to the reset.
			if (DualityEditorApp.Selection.GameObjects.Any() || DualityEditorApp.Selection.Components.Any())
			{
				IEnumerable<NodeBase> selObjQuery;
				selObjQuery = DualityEditorApp.Selection.GameObjects.Select(o => this.FindNode(o));
				selObjQuery = selObjQuery.Concat(DualityEditorApp.Selection.Components.Select(o => this.FindNode(o)));
				this.SelectNodes(selObjQuery, true);
			}

			this.tempIsInitializingObjects = false;
		}
		protected void ClearObjects()
		{
			if (this.tempIsClearingObjects) return;
			this.tempIsClearingObjects = true;

			this.objectView.BeginUpdate();
			this.objectModel.Nodes.Clear();
			this.objToNode.Clear();
			this.objectView.EndUpdate();

			this.tempIsClearingObjects = false;
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
			GameObjectNode nodeObj = node as GameObjectNode;
			ComponentNode nodeCmp = node as ComponentNode;
			if (nodeObj != null) this.objToNode[nodeObj.Obj] = nodeObj;
			if (nodeCmp != null) this.objToNode[nodeCmp.Component] = nodeCmp;
		}
		protected void UnregisterNode(Node node)
		{
			GameObjectNode nodeObj = node as GameObjectNode;
			ComponentNode nodeCmp = node as ComponentNode;
			if (nodeObj != null) this.objToNode.Remove(nodeObj.Obj);
			if (nodeCmp != null) this.objToNode.Remove(nodeCmp.Component);
		}
		protected void InsertNodeSorted(Node newNode, Node parentNode)
		{
			Node insertBeforeNode = parentNode.Nodes.FirstOrDefault(node => 
				(NodeBase.Compare(newNode as NodeBase, node as NodeBase) == 0 && String.Compare(newNode.Text, node.Text) < 0) ||
				NodeBase.Compare(newNode as NodeBase, node as NodeBase) < 0);
			if (insertBeforeNode == null) parentNode.Nodes.Add(newNode);
			else parentNode.Nodes.Insert(parentNode.Nodes.IndexOf(insertBeforeNode), newNode);

			NodeBase baseNode = newNode as NodeBase;
			GameObjectNode objNode = newNode as GameObjectNode;
			if (baseNode != null)
			{
				baseNode.UpdateLinkState(false);
				if (objNode != null)
					objNode.UpdateIcon();
			}
		}
		
		protected ComponentNode ScanComponent(Component cmp)
		{
			if (cmp == null) return null;
			if (!this.buttonShowComponents.Checked) return null;
			ComponentNode thisNode = new ComponentNode(cmp);
			return thisNode;
		}
		protected GameObjectNode ScanGameObject(GameObject obj, bool scanChildren)
		{
			if (obj == null) return null;
			GameObjectNode thisNode = new GameObjectNode(obj, !this.buttonShowComponents.Checked);
			foreach (Component c in obj.GetComponents<Component>())
			{
				ComponentNode compNode = this.ScanComponent(c);
				if (compNode != null) this.InsertNodeSorted(compNode, thisNode);
			}
			if (scanChildren)
			{
				foreach (GameObject c in obj.Children)
				{
					GameObjectNode childNode = this.ScanGameObject(c, scanChildren);
					if (childNode != null) this.InsertNodeSorted(childNode, thisNode);
				}
			}
			return thisNode;
		}
		protected Node ScanScene(Scene scene)
		{
			Node thisNode = new Node("Scene");

			foreach (GameObject obj in scene.RootObjects)
			{
				NodeBase objNode = this.ScanGameObject(obj, true);
				this.InsertNodeSorted(objNode, thisNode);
			}

			return thisNode;
		}
		
		protected void CloneNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			if (!nodes.Any()) return;

			var nodeQuery = 
				from viewNode in nodes
				select this.objectModel.FindNode(this.objectView.GetPath(viewNode)) as NodeBase;
			var objQuery =
				from objNode in nodeQuery
				where objNode is GameObjectNode
				select (objNode as GameObjectNode).Obj;
			var objArray = objQuery.ToArray();
			
			this.objectView.BeginUpdate();
			// Deselect original nodes
			foreach (GameObject obj in objArray)
			{ 
				TreeNodeAdv dragObjViewNode;
				dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(obj)));
				dragObjViewNode.IsSelected = false;
			}

			CloneGameObjectAction cloneAction = new CloneGameObjectAction(objArray);
			UndoRedoManager.Do(cloneAction);

			// Select new nodes
			foreach (GameObject clonedObj in cloneAction.Result)
			{ 
				TreeNodeAdv dragObjViewNode;
				dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(clonedObj)));
				dragObjViewNode.IsSelected = true;
				this.objectView.EnsureVisible(dragObjViewNode);
			}
			this.objectView.EndUpdate();
		}
		protected void DeleteNodes(IEnumerable<TreeNodeAdv> nodes)
		{
			var nodeQuery = 
				from viewNode in nodes
				select this.objectModel.FindNode(this.objectView.GetPath(viewNode)) as NodeBase;
			var cmpQuery =
				from cmpNode in nodeQuery
				where cmpNode is ComponentNode
				select (cmpNode as ComponentNode).Component;
			var objQuery =
				from objNode in nodeQuery
				where objNode is GameObjectNode
				select (objNode as GameObjectNode).Obj;
			var objList = objQuery.ToList();
			for (int i = objList.Count - 1; i >= 0; i--)
			{
				if (objList.Any(p => objList[i].IsChildOf(p)))
					objList.RemoveAt(i);
			}
			var cmpList = cmpQuery.Where(c => !objList.Any(p => c.GameObj == p || c.GameObj.IsChildOf(p))).ToList();
			
			// Check which Components may be removed and which not
			Component conflictComp = this.CheckComponentsRemovable(cmpList, objList);
			if (conflictComp != null)
			{
				this.FlashNode(this.FindNode(conflictComp));
				System.Media.SystemSounds.Beep.Play();
			}
			if (objList.Count == 0 && cmpList.Count == 0) return;

			// Ask user if he really wants to delete stuff
			ObjectSelection objSel = new ObjectSelection(objList.AsEnumerable<object>().Concat(cmpList));
			if (!DualityEditorApp.DisplayConfirmDeleteObjects(objSel)) return;
			if (!DualityEditorApp.DisplayConfirmBreakPrefabLinkStructure(objSel)) return;

			// Delete objects
			this.objectView.BeginUpdate();
			UndoRedoManager.BeginMacro(string.Format(EditorBaseRes.UndoRedo_DeleteObjects, objList.Count + cmpList.Count));
			UndoRedoManager.Do(new DeleteGameObjectAction(objList));
			UndoRedoManager.Do(new DeleteComponentAction(cmpList));
			UndoRedoManager.EndMacro();
			this.objectView.EndUpdate();
		}
		protected Component CheckComponentsRemovable(List<Component> cmpList, List<GameObject> ignoreGameObjList)
		{
			Component firstReqComp = null;
			for (int i = cmpList.Count - 1; i >= 0; --i)
			{
				Component c = cmpList[i];
				if (ignoreGameObjList != null && ignoreGameObjList.Contains(c.GameObj)) continue;

				Component reqComp = null;
				foreach (Component r in c.GameObj.GetComponents<Component>())
				{
					if (cmpList.Contains(r)) continue;
					if (!r.IsComponentRequirementMet(c))
					{
						reqComp = r;
						break;
					}
				}

				if (reqComp != null)
				{
					cmpList.RemoveAt(i);
					if (firstReqComp == null) firstReqComp = reqComp;
				}
			}
			return firstReqComp;
		}
		protected GameObject CreateGameObject(TreeNodeAdv baseNode, string objName = null)
		{
			if (objName == null) objName = typeof(GameObject).Name;

			GameObjectNode baseObjNode = baseNode == null ? null : baseNode.Tag as GameObjectNode;
			GameObject baseObj = baseObjNode == null ? null : baseObjNode.Obj;
			GameObject newObj = new GameObject();
			newObj.Name = objName;

			CreateGameObjectAction action = new CreateGameObjectAction(baseObj, newObj);
			UndoRedoManager.Do(action);

			return action.Result.FirstOrDefault();
		}
		protected Component CreateComponent(TreeNodeAdv baseNode, Type cmpType)
		{
			GameObjectNode baseObjNode = baseNode == null ? null : baseNode.Tag as GameObjectNode;
			GameObject baseObj = baseObjNode == null ? null : baseObjNode.Obj;

			UndoRedoManager.BeginMacro();

			// There is no GameObject? Create one!
			if (baseObj == null)
			{
				baseObj = this.CreateGameObject(baseNode, cmpType.Name);
				baseObjNode = this.FindNode(baseObj);
			}
			// There already is such Component? Return it.
			else
			{
				Component existingComponent = baseObj.GetComponent(cmpType);
				if (existingComponent != null) return existingComponent;
			}

			// Create Components
			CreateComponentAction action = new CreateComponentAction(baseObj, cmpType.CreateInstanceOf() as Component);
			UndoRedoManager.Do(action);
			UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromFirst);

			return action.Result.LastOrDefault();
		}
		protected bool OpenResource(TreeNodeAdv node)
		{
			GameObjectNode objNode = node.Tag as GameObjectNode;
			ComponentNode cmpNode = node.Tag as ComponentNode;

			IEditorAction action = null;
			object subject = null;
			if (objNode != null)
			{
				subject = objNode.Obj;
				action = this.GetResourceOpenAction(objNode.Obj);
			}
			if (cmpNode != null)
			{
				subject = cmpNode.Component;
				action = this.GetResourceOpenAction(cmpNode.Component);	
			}

			if (action != null)
			{
				action.Perform(subject);
				return true;
			}
			else return false;
		}
		protected IEditorAction GetResourceOpenAction(TreeNodeAdv node)
		{
			GameObjectNode objNode = node.Tag as GameObjectNode;
			ComponentNode cmpNode = node.Tag as ComponentNode;
			if (objNode != null) return this.GetResourceOpenAction(objNode.Obj);
			if (cmpNode != null) return this.GetResourceOpenAction(cmpNode.Component);			
			return null;
		}
		protected IEditorAction GetResourceOpenAction(GameObject obj)
		{
			var actions = CorePluginRegistry.GetEditorActions<GameObject>(CorePluginRegistry.ActionContext_OpenRes, new[] { obj });
			return actions == null ? null : actions.FirstOrDefault();
		}
		protected IEditorAction GetResourceOpenAction(Component cmp)
		{
			var actions = CorePluginRegistry.GetEditorActions(cmp.GetType(), CorePluginRegistry.ActionContext_OpenRes, new[] { cmp });
			return actions == null ? null : actions.FirstOrDefault();
		}

		protected void AssureMonoSelection()
		{
			if (this.SelectedGameObjectNodes.Any() && this.SelectedComponentNodes.Any())
			{
				List<TreeNodeAdv> selNodes = new List<TreeNodeAdv>(this.objectView.SelectedNodes);
				if (this.objectView.CurrentNode.Tag is ComponentNode)
				{
					foreach (TreeNodeAdv viewNode in selNodes)
						if (viewNode.Tag is GameObjectNode) viewNode.IsSelected = false;
				}
				else
				{
					foreach (TreeNodeAdv viewNode in selNodes)
						if (viewNode.Tag is ComponentNode) viewNode.IsSelected = false;
				}
			}
		}
		protected void AppendNodesToData(DataObject data, IEnumerable<TreeNodeAdv> nodes, bool guardRequiredComponents)
		{
			if (!guardRequiredComponents)
			{
				data.SetData(nodes.ToArray());
				data.SetComponentRefs(
					from c in nodes
					where c.Tag is ComponentNode
					select (c.Tag as ComponentNode).Component);
				data.SetGameObjectRefs(
					from c in nodes
					where c.Tag is GameObjectNode
					select (c.Tag as GameObjectNode).Obj);
			}
			else
			{
				// Query selected objects and components
				var nodeQuery = 
					from viewNode in this.objectView.SelectedNodes
					select this.objectModel.FindNode(this.objectView.GetPath(viewNode)) as NodeBase;
				var cmpQuery =
					from cmpNode in nodeQuery
					where cmpNode is ComponentNode
					select (cmpNode as ComponentNode).Component;
				var objQuery =
					from objNode in nodeQuery
					where objNode is GameObjectNode
					select (objNode as GameObjectNode).Obj;
				var cmpList = new List<Component>(cmpQuery);
				var objList = new List<GameObject>(objQuery);

				// Check which Components may be removed and which not
				Component conflictComp = this.CheckComponentsRemovable(cmpList, objList);
				if (conflictComp != null)
				{
					this.FlashNode(this.FindNode(conflictComp));
					System.Media.SystemSounds.Beep.Play();
				}

				var viewNodeQuery = 
							cmpList.Select(c => this.objectView.FindNodeByTag(this.FindNode(c))).
					Concat(	objList.Select(o => this.objectView.FindNodeByTag(this.FindNode(o))));

				this.AppendNodesToData(data, viewNodeQuery.ToArray(), false);
			}
		}

		protected void UpdatePrefabLinkStatus(bool checkFileChanged)
		{
			bool anyLinkStateChanged = false;
			foreach (NodeBase node in this.objToNode.Values)
			{
				bool result = node.UpdateLinkState(checkFileChanged);
				anyLinkStateChanged = anyLinkStateChanged || result;
			}

			if (anyLinkStateChanged)
			{
				foreach (GameObjectNode objNode in this.objToNode.Values.OfType<GameObjectNode>())
					objNode.UpdateIcon();

				this.objectView.Invalidate(false);
			}
		}
		protected void UpdateSceneLabel()
		{
			bool sceneAvail = Scene.Current != null;
			this.toolStripLabelSceneName.Text = (!sceneAvail || Scene.Current.IsRuntimeResource) ? 
				PluginRes.EditorBaseRes.SceneNameNotYetSaved : 
				Scene.Current.Name;
			this.toolStripButtonSaveScene.Enabled = !Sandbox.IsActive;
		}
		
		private void textBoxFilter_TextChanged(object sender, EventArgs e)
		{
			this.ApplyNodeFilter();
		}
		private bool objectModel_IsNodeVisible(object obj)
		{
			if (this.tempUpperFilter == null) return true;
			TreeNodeAdv vn = obj as TreeNodeAdv;
			Node n = vn != null ? vn.Tag as Node : obj as Node;
			if (n == null) return true;
			bool isVisible;
			if (!this.tempNodeVisibilityCache.TryGetValue(n, out isVisible))
			{
				isVisible = n.Text.ToUpper().Contains(this.tempUpperFilter);
				if (!isVisible) isVisible = n.Nodes.Any(sub => this.objectModel_IsNodeVisible(sub));
				this.tempNodeVisibilityCache[n] = isVisible;
			}
			return isVisible;
		}
		private void objectView_SelectionChanged(object sender, EventArgs e)
		{
			if (this.tempIsInitializingObjects) return;
			if (this.tempIsClearingObjects) return;

			var selComponent =
					from vn in this.objectView.SelectedNodes
					where (vn.Tag is ComponentNode) && (vn.Tag as ComponentNode).Component != null
					select (vn.Tag as ComponentNode).Component;
			var selGameObj =
					from vn in this.objectView.SelectedNodes
					where (vn.Tag is GameObjectNode) && (vn.Tag as GameObjectNode).Obj != null
					select (vn.Tag as GameObjectNode).Obj;

			if (!DualityEditorApp.IsSelectionChanging)
			{
				var selObj = selGameObj.Union<object>(selComponent);
				if (selObj.Any())
				{
					if (!selGameObj.Any() || !selComponent.Any()) DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
					DualityEditorApp.Select(this, new ObjectSelection(selObj));
				}
				else
					DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
			}
		}
		private void objectView_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.objectView.SelectedNodes.Count > 0)
			{
				// Navigate left / collapse
				if (e.KeyCode == Keys.Back)
				{
					int lowLevel = this.objectView.SelectedNodes.Min(viewNode => viewNode.Level);
					TreeNodeAdv lowLevelNode = this.objectView.SelectedNodes.First(viewNode => viewNode.Level == lowLevel);

					if (this.objectView.SelectedNode.IsExpanded)
						this.objectView.SelectedNode.Collapse();
					else if (lowLevel > 1)
						this.objectView.SelectedNode = lowLevelNode.Parent;
				}
				// Navigate right / expand
				else if (e.KeyCode == Keys.Return)
				{
					if (!this.objectView.SelectedNode.IsExpanded)
						this.objectView.SelectedNode.Expand();
				}
				// Fobus object
				else if (e.KeyCode == Keys.F)
				{
					this.OpenResource(this.objectView.SelectedNode);
				}
			}
		}
		private void objectView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (this.objectView.SelectedNodes.Count > 0)
			{
				this.AssureMonoSelection();

				DataObject dragDropData = new DataObject();
				this.AppendNodesToData(dragDropData, this.objectView.SelectedNodes, false);
				this.DoDragDrop(dragDropData, DragDropEffects.All | DragDropEffects.Link);
			}
		}
		private void objectView_DragOver(object sender, DragEventArgs e)
		{
			DataObject data = e.Data as DataObject;
			if (data != null)
			{
				NodeBase dropParent = this.DragDropGetTargetNode();
				if (data.ContainsGameObjectRefs())
				{
					DragDropEffects effect;
					if ((e.KeyState & 2) != 0)			// Right mouse button
						effect = DragDropEffects.Move | DragDropEffects.Copy;
					else if ((e.KeyState & 32) != 0)	// Alt key
						effect = DragDropEffects.Copy;
					else
						effect = DragDropEffects.Move;
					effect &= e.AllowedEffect;

					if (dropParent == null)
						e.Effect = effect;
					else if (dropParent is GameObjectNode)
					{
						GameObject dropObj = (dropParent as GameObjectNode).Obj;
						GameObject[] draggedObj = data.GetGameObjectRefs();
						bool canDropHere = true;

						// Can't drop in child of dragged objects
						foreach (GameObject dragObj in draggedObj)
						{
							if (dropObj == dragObj || dropObj.IsChildOf(dragObj))
							{
								canDropHere = false;
								break;
							}
						}

						e.Effect = canDropHere ? effect : DragDropEffects.None;
					}
					else
						e.Effect = DragDropEffects.None;
				}
				else if (data.ContainsComponentRefs())
				{
					DragDropEffects effect;
					if ((e.KeyState & 2) != 0)			// Right mouse button
						effect = DragDropEffects.Move | DragDropEffects.Copy;
					else if ((e.KeyState & 32) != 0)	// Alt key
						effect = DragDropEffects.Copy;
					else
						effect = DragDropEffects.Move;
					effect &= e.AllowedEffect;

					if (dropParent is GameObjectNode)
					{
						GameObject dropObj = (dropParent as GameObjectNode).Obj;
						Component[] draggedObj = data.GetComponentRefs();
						bool canDropHere = true;

						canDropHere = canDropHere && draggedObj.All(c => dropObj.GetComponent(c.GetType()) == null);
						canDropHere = canDropHere && draggedObj.All(c => c.IsComponentRequirementMet(dropObj, draggedObj));

						e.Effect = canDropHere ? effect : DragDropEffects.None;
					}
					else
						e.Effect = DragDropEffects.None;
				}
				else if (dropParent != null && new ConvertOperation(data, ConvertOperation.Operation.All).CanPerform<Component>())
				{
					e.Effect = e.AllowedEffect;
				}
				else if (new ConvertOperation(data, ConvertOperation.Operation.All).CanPerform<GameObject>())
				{
					if (dropParent is ComponentNode)
						e.Effect = DragDropEffects.None;
					else
						e.Effect = e.AllowedEffect;
				}
			}

			this.objectView.HighlightDropPosition = (e.Effect != DragDropEffects.None);
		}
		private void objectView_DragDrop(object sender, DragEventArgs e)
		{
			this.objectView.BeginUpdate();

			bool effectMove = (e.Effect & DragDropEffects.Move) != DragDropEffects.None;
			bool effectCopy = (e.Effect & DragDropEffects.Copy) != DragDropEffects.None;
			DataObject data = e.Data as DataObject;
			if (data != null)
			{
				ConvertOperation convertOp = new ConvertOperation(data, ConvertOperation.Operation.All);
				this.tempDropTarget = this.DragDropGetTargetNode();
				if (data.ContainsGameObjectRefs())
				{
					this.tempDropData = data.GetGameObjectRefs();

					// Display context menu if both moving and copying are availabled
					if (effectMove && effectCopy)
						this.contextMenuDragMoveCopy.Show(this, this.PointToClient(new Point(e.X, e.Y)));
					else if (effectCopy)
						this.copyHereToolStripMenuItem_Click(this, null);
					else if (effectMove)
						this.moveHereToolStripMenuItem_Click(this, null);
				}
				else if (data.ContainsComponentRefs())
				{
					this.tempDropData = data.GetComponentRefs();

					// Display context menu if both moving and copying are availabled
					if (effectMove && effectCopy)
						this.contextMenuDragMoveCopy.Show(this, this.PointToClient(new Point(e.X, e.Y)));
					else if (effectCopy)
						this.copyHereToolStripMenuItem_Click(this, null);
					else if (effectMove)
						this.moveHereToolStripMenuItem_Click(this, null);
				}
				else if (this.tempDropTarget != null && convertOp.CanPerform<Component>())
				{
					GameObject dropObj = null;
					if (this.tempDropTarget is GameObjectNode)
						dropObj = (this.tempDropTarget as GameObjectNode).Obj;
					else
						dropObj = (this.tempDropTarget as ComponentNode).Component.GameObj;

					var componentQuery = convertOp.Perform<Component>();
					if (componentQuery != null)
					{
						// Create Components
						CreateComponentAction createAction = new CreateComponentAction(dropObj, componentQuery.Where(c => c.GameObj == null));
						UndoRedoManager.BeginMacro();
						UndoRedoManager.Do(new DeleteComponentAction(componentQuery.Select(c => dropObj.GetComponent(c.GetType())).NotNull()));
						UndoRedoManager.Do(createAction);
						UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromLast);

						bool selCleared = false;
						foreach (Component newComponent in createAction.Result)
						{
							NodeBase dataNode = this.FindNode(newComponent);
							if (dataNode == null) continue;
							TreeNodeAdv viewNode = this.objectView.FindNode(this.objectModel.GetPath(dataNode));
							if (viewNode == null) continue;
							if (!selCleared)
							{
								this.objectView.ClearSelection();
								selCleared = true;
							}
							viewNode.IsSelected = true;
							this.objectView.EnsureVisible(viewNode);
						}
					}
				}
				else if (convertOp.CanPerform<GameObject>())
				{
					GameObject dropObj = (this.tempDropTarget is GameObjectNode) ? (this.tempDropTarget as GameObjectNode).Obj : null;
					var gameObjQuery = convertOp.Perform<GameObject>();
					if (gameObjQuery != null)
					{
						CreateGameObjectAction action = new CreateGameObjectAction(dropObj, gameObjQuery);
						UndoRedoManager.Do(action);

						bool selCleared = false;
						foreach (GameObject newObj in action.Result)
						{
							NodeBase dataNode = this.FindNode(newObj);
							if (dataNode == null) continue;
							TreeNodeAdv viewNode = this.objectView.FindNode(this.objectModel.GetPath(dataNode));
							if (viewNode == null) continue;
							if (!selCleared)
							{
								this.objectView.ClearSelection();
								selCleared = true;
							}
							viewNode.IsSelected = true;
							this.objectView.EnsureVisible(viewNode);
						}
					}
				}
			}

			this.objectView.EndUpdate();
		}
		private void objectView_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			e.Handled = this.OpenResource(e.Node);
		}
		private void objectView_Leave(object sender, EventArgs e)
		{
			this.objectView.Invalidate();
		}
		private void objectView_Enter(object sender, EventArgs e)
		{
			this.tempScheduleSelectionChange = true;
		}
		private void objectView_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.tempScheduleSelectionChange)
			{
				this.tempScheduleSelectionChange = false;
				this.objectView_SelectionChanged(this.objectView, EventArgs.Empty);
			}
		}
		private NodeBase DragDropGetTargetNode()
		{
			TreeNodeAdv dropViewNode		= this.objectView.DropPosition.Node;
			TreeNodeAdv dropViewNodeParent	= (dropViewNode != null && this.objectView.DropPosition.Position != NodePosition.Inside) ? dropViewNode.Parent : dropViewNode;
			NodeBase dropNodeParent			= (dropViewNodeParent != null) ? dropViewNodeParent.Tag as NodeBase : null;
			return dropNodeParent;
		}
		
		private void nodeStateIcon_DrawIcon(object sender, Aga.Controls.Tree.NodeControls.DrawIconEventArgs e)
		{
			if (e.Context.Bounds.IsEmpty) return;
			NodeBase node = e.Node.Tag as NodeBase;

			ComponentNode cmpNode = node as ComponentNode;
			GameObjectNode objNode = (cmpNode != null ? cmpNode.Parent : node) as GameObjectNode;
			DesignTimeObjectData data = objNode != null ? CorePluginRegistry.GetDesignTimeData(objNode.Obj) : null;
			bool lockedOrHidden = data != null ? data.IsLocked || data.IsHidden : false;

			if (lockedOrHidden) e.IconColorMatrix = this.inactiveIconMatrix;
		}
		private void nodeTextBoxName_DrawText(object sender, Aga.Controls.Tree.NodeControls.DrawTextEventArgs e)
		{
			if (e.Context.Bounds.IsEmpty) return;
			NodeBase node = e.Node.Tag as NodeBase;

			ComponentNode cmpNode = node as ComponentNode;
			GameObjectNode objNode = (cmpNode != null ? cmpNode.Parent : node) as GameObjectNode;
			DesignTimeObjectData data = objNode != null ? CorePluginRegistry.GetDesignTimeData(objNode.Obj) : null;
			bool lockedOrHidden = data != null ? data.IsLocked || data.IsHidden : false;

			// Prefab-linked entities
			if (lockedOrHidden)
				e.TextColor = Color.FromArgb(96, 96, 96);
			else if (node.LinkState == NodeBase.PrefabLinkState.Active)
				e.TextColor = Color.Blue;
			else if (node.LinkState == NodeBase.PrefabLinkState.Broken)
				e.TextColor = Color.DarkRed;
			else
				e.TextColor = Color.Black;

			// Flashing
			if (node == this.flashNode)
			{
				float intLower = this.flashIntensity;
				Color hlBase = Color.FromArgb(224, 64, 32);
				Color hlLower = Color.FromArgb(
					(int)(this.objectView.BackColor.R * (1.0f - intLower) + hlBase.R * intLower),
					(int)(this.objectView.BackColor.G * (1.0f - intLower) + hlBase.G * intLower),
					(int)(this.objectView.BackColor.B * (1.0f - intLower) + hlBase.B * intLower));
				e.BackgroundBrush = new SolidBrush(hlLower);
			}
		}
		private void nodeTextBoxName_EditorShowing(object sender, CancelEventArgs e)
		{
			if (e.Cancel) return;
			if (this.objectView.SelectedNode == null)
			{
				e.Cancel = true;
				return;
			}

			NodeBase node = this.objectView.SelectedNode.Tag as NodeBase;
			GameObjectNode objNode = node as GameObjectNode;
			if (objNode == null)
			{
				e.Cancel = true;
			}
			else
			{
				DesignTimeObjectData data = CorePluginRegistry.GetDesignTimeData(objNode.Obj);
				if (data.IsLocked) e.Cancel = true;
			}

			if (!e.Cancel)
			{
				this.lastEditedNode = node;
				this.objectView.ContextMenuStrip = null;
			}
		}
		private void nodeTextBoxName_EditorHided(object sender, EventArgs e)
		{
			this.objectView.ContextMenuStrip = this.contextMenuNode;
		}
		private void nodeTextBoxName_ChangesApplied(object sender, EventArgs e)
		{
			NodeBase node = this.lastEditedNode;
			GameObjectNode objNode = node as GameObjectNode;
			if (objNode != null)
			{
				UndoRedoManager.Do(new SetGameObjectNameAction(objNode.Text, new[] { objNode.Obj }));
			}
		}
		private void timerFlashItem_Tick(object sender, EventArgs e)
		{
			this.flashDuration += (this.timerFlashItem.Interval / 1000.0f);
			this.flashIntensity = 1.0f - (this.flashDuration % 0.33f) / 0.33f;
			this.objectView.Invalidate();

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
			GameObject[] draggedObj = this.tempDropData as GameObject[];
			Component[] draggedComp = this.tempDropData as Component[];
			GameObject dropObj = (this.tempDropTarget is GameObjectNode) ? (this.tempDropTarget as GameObjectNode).Obj : null;

			if (draggedObj != null)
			{
				UndoRedoManager.BeginMacro();
				CloneGameObjectAction cloneAction = new CloneGameObjectAction(draggedObj);
				UndoRedoManager.Do(cloneAction);
				UndoRedoManager.Do(new SetGameObjectParentAction(dropObj, cloneAction.Result));
				UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromFirst);
				
				// Deselect dragged nodes
				foreach (GameObject dragObj in draggedObj)
				{
					TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(dragObj)));
					dragObjViewNode.IsSelected = false;
				}
				// Select new nodes
				foreach (GameObject dragObjClone in cloneAction.Result)
				{
					TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(dragObjClone)));
					dragObjViewNode.IsSelected = true;
					this.objectView.EnsureVisible(dragObjViewNode);
				}
			}
			else if (draggedComp != null)
			{
				// Clone Components
				CreateComponentAction cloneAction = new CreateComponentAction(dropObj, draggedComp.Select(c => c.Clone()));
				UndoRedoManager.Do(cloneAction);
				
				// Deselect dragged nodes
				foreach (Component dragObj in draggedComp)
				{
					TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(dragObj)));
					dragObjViewNode.IsSelected = false;
				}
				// Select new nodes
				foreach (Component dragObjClone in cloneAction.Result)
				{
					TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(this.FindNode(dragObjClone)));
					dragObjViewNode.IsSelected = true;
					this.objectView.EnsureVisible(dragObjViewNode);
				}
			}
		}
		private void moveHereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GameObject[] draggedObj = this.tempDropData as GameObject[];
			Component[] draggedComp = this.tempDropData as Component[];
			GameObject dropObj = (this.tempDropTarget is GameObjectNode) ? (this.tempDropTarget as GameObjectNode).Obj : null;

			if (!DualityEditorApp.DisplayConfirmBreakPrefabLinkStructure(new ObjectSelection(this.tempDropData as IEnumerable<object>))) return;

			if (draggedObj != null)
			{
				// Set parent
				UndoRedoManager.Do(new SetGameObjectParentAction(dropObj, draggedObj));

				// Select nodes
				foreach (GameObject dragObj in draggedObj)
				{
					GameObjectNode dragObjNode = this.FindNode(dragObj);
					TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(dragObjNode));
					dragObjViewNode.IsSelected = true;
					this.objectView.EnsureVisible(dragObjViewNode);
				}
			}
			else if (draggedComp != null)
			{
				List<Component> cmpList = draggedComp.ToList();

				// Check which Components may be removed and which not
				Component conflictComp = this.CheckComponentsRemovable(cmpList, null);
				if (conflictComp != null)
				{
					this.FlashNode(this.FindNode(conflictComp));
					System.Media.SystemSounds.Beep.Play();
				}
				else
				{
					// Set parent
					UndoRedoManager.Do(new SetComponentParentAction(dropObj, cmpList));

					// Select nodes
					foreach (Component dragObj in cmpList)
					{
						ComponentNode dragObjNode = this.FindNode(dragObj);
						TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(dragObjNode));
						dragObjViewNode.IsSelected = true;
						this.objectView.EnsureVisible(dragObjViewNode);
					}
				}
			}
		}

		private void contextMenuNode_Opening(object sender, CancelEventArgs e)
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.objectView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);
			List<object> selObjData = 
				selNodeData.OfType<ComponentNode>().Select(n => n.Component).AsEnumerable<object>().Concat(
				selNodeData.OfType<GameObjectNode>().Select(n => n.Obj)).ToList();

			bool noSelect = selNodeData.Count == 0;
			bool singleSelect = selNodeData.Count == 1;
			bool multiSelect = selNodeData.Count > 1;
			bool gameObjSelect = selNodeData.Any(n => n is GameObjectNode);

			this.newToolStripMenuItem.Visible = (singleSelect && gameObjSelect) || noSelect;
			this.toolStripSeparatorNew.Visible = !multiSelect && gameObjSelect && !noSelect;

			this.renameToolStripMenuItem.Visible = !noSelect && gameObjSelect;
			this.cloneToolStripMenuItem.Visible = !noSelect && gameObjSelect;
			this.deleteToolStripMenuItem.Visible = !noSelect;
			this.toolStripSeparatorGameObject.Visible = gameObjSelect;
			this.lockedToolStripMenuItem.Visible = gameObjSelect;
			this.contextMenuNode_UpdateLockHideItem();

			this.renameToolStripMenuItem.Enabled = singleSelect;

			// Provide custom actions
			Type mainResType = null;
			if (selObjData.Any())
			{
				mainResType = selObjData.First().GetType();
				// Find mutual type
				foreach (var obj in selObjData)
				{
					Type resType = obj.GetType();
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
					selObjData)
					.ToArray();
				foreach (var actionEntry in customActions)
				{
					ToolStripMenuItem actionItem = new ToolStripMenuItem(actionEntry.Name, actionEntry.Icon);
					actionItem.Click += this.customObjectActionItem_Click;
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
			this.gameObjectToolStripMenuItem.Image = CorePluginRegistry.GetTypeImage(typeof(GameObject));
			List<ToolStripItem> oldItems = new List<ToolStripItem>(this.newToolStripMenuItem.DropDownItems.OfType<ToolStripItem>());
			this.newToolStripMenuItem.DropDownItems.Clear();
			foreach (ToolStripItem item in oldItems.Skip(2)) item.Dispose();
			this.newToolStripMenuItem.DropDownItems.AddRange(oldItems.Take(2).ToArray());

			// Populate the "New" menu
			List<ToolStripItem> newItems = new List<ToolStripItem>();
			foreach (Type cmpType in this.QueryComponentTypes())
			{
				// Generate category item
				string[] category = CorePluginRegistry.GetTypeCategory(cmpType);
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
						subCatItem.Tag = cmpType.Assembly;
						subCatItem.DropDownItemClicked += this.newToolStripMenuItem_DropDownItemClicked;
						if (categoryItem == this.newToolStripMenuItem)
							EditorBasePlugin.InsertToolStripTypeItem(newItems, subCatItem);
						else
							EditorBasePlugin.InsertToolStripTypeItem(categoryItem.DropDownItems, subCatItem);
					}
					categoryItem = subCatItem;
				}

				ToolStripMenuItem cmpTypeItem = new ToolStripMenuItem(cmpType.Name, ComponentNode.GetTypeImage(cmpType));
				cmpTypeItem.Tag = cmpType;
				if (categoryItem == this.newToolStripMenuItem)
					EditorBasePlugin.InsertToolStripTypeItem(newItems, cmpTypeItem);
				else
					EditorBasePlugin.InsertToolStripTypeItem(categoryItem.DropDownItems, cmpTypeItem);
			}

			this.newToolStripMenuItem.DropDownItems.AddRange(newItems.ToArray());
		}
		private void contextMenuNode_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			var hoverItem = this.contextMenuNode.GetItemAt(this.contextMenuNode.PointToClient(Cursor.Position));
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked && hoverItem == this.lockedToolStripMenuItem)
				e.Cancel = true;
		}
		private void contextMenuNode_UpdateLockHideItem()
		{
			var selNodeData =
				from vn in this.objectView.SelectedNodes
				where vn.Tag is GameObjectNode
				select CorePluginRegistry.GetDesignTimeData((vn.Tag as GameObjectNode).Obj);
			bool locked = false;
			bool hidden = false;
			foreach (var data in selNodeData)
			{
				if (data.IsLocked) locked = true;
				if (data.IsHidden) hidden = true;
				if (locked && hidden) break;
			}

			if (hidden)
			{
				this.lockedToolStripMenuItem.Text = PluginRes.EditorBaseRes.SceneView_Item_Hidden;
				this.lockedToolStripMenuItem.ToolTipText = PluginRes.EditorBaseRes.SceneView_Item_Hidden_Tooltip;
				this.lockedToolStripMenuItem.Image = PluginRes.EditorBaseResCache.IconEyeCross;
			}
			else if (locked)
			{
				this.lockedToolStripMenuItem.Text = PluginRes.EditorBaseRes.SceneView_Item_Locked;
				this.lockedToolStripMenuItem.ToolTipText = PluginRes.EditorBaseRes.SceneView_Item_Locked_Tooltip;
				this.lockedToolStripMenuItem.Image = PluginRes.EditorBaseResCache.IconLock;
			}
			else
			{
				this.lockedToolStripMenuItem.Text = PluginRes.EditorBaseRes.SceneView_Item_LockHide;
				this.lockedToolStripMenuItem.ToolTipText = PluginRes.EditorBaseRes.SceneView_Item_LockHide_Tooltip;
				this.lockedToolStripMenuItem.Image = null;
			}
		}

		private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CloneNodes(this.objectView.SelectedNodes);
		}
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.DeleteNodes(this.objectView.SelectedNodes);
		}
		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.nodeTextBoxName.BeginEdit();
		}
		private void lockedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selNodes = GetSelNodesFlattened();

			var selNodeData = selNodes.Select(n => CorePluginRegistry.GetDesignTimeData(n.Obj)).ToArray();
			bool locked = true;
			bool hidden = true;
			foreach (var data in selNodeData)
			{
				if (!data.IsLocked) locked = false;
				if (!data.IsHidden) hidden = false;
				if (!locked && !hidden) break;
			}
			foreach (var data in selNodeData)
			{
				if (hidden)
				{
					data.IsLocked = false;
					data.IsHidden = false;
				}
				else if (locked)
				{
					data.IsLocked = true;
					data.IsHidden = true;
				}
				else
				{
					data.IsLocked = true;
					data.IsHidden = false;
				}
			}

			foreach (var gameObjNode in selNodes) gameObjNode.UpdateIcon();
			this.contextMenuNode_UpdateLockHideItem();
			this.objectView.Invalidate();

			// Emit a PropertyChanged event for DesignTimeData.
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(selNodeData));
		}

		private void gameObjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Create the GameObject
			GameObject obj = this.CreateGameObject(this.objectView.SelectedNode);
			GameObjectNode objNode = this.FindNode(obj);

			// Deselect previous
			this.objectView.ClearSelection();

			// Select new node
			if (objNode != null)
			{
				TreeNodeAdv dragObjViewNode;
				dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(objNode));
				if (dragObjViewNode != null)
				{
					dragObjViewNode.IsSelected = true;
					this.objectView.EnsureVisible(dragObjViewNode);
					this.nodeTextBoxName.BeginEdit();
				}
			}
		}
		private void newToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == this.gameObjectToolStripMenuItem) return;
			if (e.ClickedItem.Tag as Type == null) return;

			// Create the Component
			Type clickedType = e.ClickedItem.Tag as Type;
			Component cmp = this.CreateComponent(this.objectView.SelectedNode, clickedType);

			NodeBase cmpNode = (NodeBase)this.FindNode(cmp) ?? this.FindNode(cmp.GameObj);
			if (cmpNode != null)
			{
				// Deselect previous
				this.objectView.ClearSelection();

				// Select new node
				TreeNodeAdv dragObjViewNode = this.objectView.FindNode(this.objectModel.GetPath(cmpNode));
				if (dragObjViewNode != null)
				{
					dragObjViewNode.IsSelected = true;
					this.objectView.EnsureVisible(dragObjViewNode);
				}
				return;
			}
		}
		private void customObjectActionItem_Click(object sender, EventArgs e)
		{
			List<NodeBase> selNodeData = new List<NodeBase>(
				from vn in this.objectView.SelectedNodes
				where vn.Tag is NodeBase
				select vn.Tag as NodeBase);
			List<object> selObjData = 
				selNodeData.OfType<ComponentNode>().Select(n => n.Component).AsEnumerable<object>().Concat(
				selNodeData.OfType<GameObjectNode>().Select(n => n.Obj)).ToList();

			ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
			IEditorAction action = clickedItem.Tag as IEditorAction;
			action.Perform(selObjData);
		}

		private void buttonCreateScene_Click(object sender, EventArgs e)
		{
			DualityEditorApp.SaveCurrentScene(true);
			Scene.Current = null;
		}
		private void buttonSaveScene_Click(object sender, EventArgs e)
		{
			bool isNewScene = Scene.Current.IsRuntimeResource;
			string scenePath = DualityEditorApp.SaveCurrentScene(false);
			this.UpdateSceneLabel();

			if (isNewScene)
			{
				ProjectFolderView projectView = EditorBasePlugin.Instance.RequestProjectView();
				if (projectView != null)
				{
					projectView.ScheduleSelect(scenePath, true);
				}
			}
		}
		private void buttonShowComponents_CheckedChanged(object sender, EventArgs e)
		{
			// Save expand data
			HashSet<object> expandedMap = new HashSet<object>();
			this.objectView.SaveNodesExpanded(this.objectView.Root, expandedMap, NodeIdFuncCoreObject);

			this.ClearObjects();
			this.InitObjects();

			// Restore expand data
			this.objectView.RestoreNodesExpanded(this.objectView.Root, expandedMap, NodeIdFuncCoreObject);
		}
		private object NodeIdFuncCoreObject(TreeNodeAdv node)
		{
			GameObjectNode objNode = node.Tag as GameObjectNode;
			if (objNode != null) return objNode.Obj;

			ComponentNode cmpNode = node.Tag as ComponentNode;
			if (cmpNode != null) return cmpNode.Component;

			return node.Tag;
		}

		private void EditorForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender == this) return;
			if ((e.AffectedCategories & ObjectSelection.Category.GameObjCmp) == ObjectSelection.Category.None) return;
			if (e.SameObjects) return;

			IEnumerable<NodeBase> removedObjQuery;
			removedObjQuery = e.Removed.GameObjects.Select(o => this.FindNode(o));
			removedObjQuery = removedObjQuery.Concat(e.Removed.Components.Select(o => this.FindNode(o)));

			IEnumerable<NodeBase> addedObjQuery;
			addedObjQuery = e.Added.GameObjects.Select(o => this.FindNode(o));
			addedObjQuery = addedObjQuery.Concat(e.Added.Components.Select(o => this.FindNode(o)));

			this.SelectNodes(removedObjQuery, false);
			this.SelectNodes(addedObjQuery, true);
		}
		private void EditorForm_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e is PrefabAppliedEventArgs || 
				(e.HasProperty(ReflectionInfo.Property_GameObject_PrefabLink) && e.HasAnyObject(Scene.Current.AllObjects)) || 
				(e.CompleteChange && e.HasObject(Scene.Current)))
				this.UpdatePrefabLinkStatus(false);

			if (e.HasProperty(ReflectionInfo.Property_GameObject_Name))
			{
				foreach (GameObjectNode node in e.Objects.GameObjects.Select(g => this.FindNode(g)))
					if (node != null) node.Text = node.Obj.Name;
			}
		}
		private void EditorForm_ResourceRenamed(object sender, ResourceRenamedEventArgs e)
		{
			if (e.Path == Scene.CurrentPath) this.UpdateSceneLabel();

			if (!e.IsDirectory && !typeof(Prefab).IsAssignableFrom(e.ContentType)) return;
			this.UpdatePrefabLinkStatus(true);
		}
		private void EditorForm_ResourceCreated(object sender, ResourceEventArgs e)
		{
			if (!e.IsDirectory && !typeof(Prefab).IsAssignableFrom(e.ContentType)) return;
			this.UpdatePrefabLinkStatus(true);
		}
		private void EditorForm_ResourceDeleted(object sender, ResourceEventArgs e)
		{
			if (!e.IsDirectory && !typeof(Prefab).IsAssignableFrom(e.ContentType)) return;
			this.UpdatePrefabLinkStatus(true);
		}

		private void Scene_Leaving(object sender, EventArgs e)
		{
			this.ClearObjects();
			this.UpdateSceneLabel();
		}
		private void Scene_Entered(object sender, EventArgs e)
		{
			this.InitObjects();
			this.UpdateSceneLabel();
		}
		private void Scene_ComponentAdded(object sender, ComponentEventArgs e)
		{
			// Ignore events during transition
			if (Scene.IsSwitching) return;

			if (!this.buttonShowComponents.Checked)
			{
				GameObjectNode objNode = (e.Component != null && e.Component.GameObj != null) ? this.FindNode(e.Component.GameObj) : null;
				if (objNode != null)
					objNode.UpdateIcon();
				return;
			}

			// Find the parent node to add to
			Node parentNode = e.Component.GameObj != null ? this.FindNode(e.Component.GameObj) : this.objectModel.Root;
			
			// No parent node existing? This must be a new object then.
			if (parentNode == null)
			{
				// Let the parent handle it when it registers.
				return;
			}

			ComponentNode newObjNode = this.ScanComponent(e.Component);
			this.InsertNodeSorted(newObjNode, parentNode);
			this.RegisterNodeTree(newObjNode);
		}
		private void Scene_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			// Ignore events during transition
			if (Scene.IsSwitching) return;

			if (!this.buttonShowComponents.Checked)
			{
				GameObjectNode objNode = (e.Component != null && e.Component.GameObj != null) ? this.FindNode(e.Component.GameObj) : null;
				if (objNode != null)
					objNode.UpdateIcon(e.Component);
				return;
			}

			ComponentNode oldObjNode = this.FindNode(e.Component);
			if (oldObjNode == null) return;

			Node parentNode = oldObjNode.Parent;
			parentNode.Nodes.Remove(oldObjNode);
			this.UnregisterNodeTree(oldObjNode);
		}
		private void Scene_GameObjectUnregistered(object sender, GameObjectEventArgs e)
		{
			// Ignore events during transition
			if (Scene.IsSwitching) return;

			GameObjectNode oldObjNode = this.FindNode(e.Object);
			if (oldObjNode == null) return;

			Node parentNode = oldObjNode.Parent;
			parentNode.Nodes.Remove(oldObjNode);
			this.UnregisterNodeTree(oldObjNode);
		}
		private void Scene_GameObjectRegistered(object sender, GameObjectEventArgs e)
		{
			// Ignore events during transition
			if (Scene.IsSwitching) return;

			// Ignore already added nodes
			if (this.FindNode(e.Object) != null) return;

			// Find the parent node to add to
			Node parentNode = e.Object.Parent != null ? this.FindNode(e.Object.Parent) : this.objectModel.Root;
			
			// No parent node existing? This must be a new object then.
			if (parentNode == null)
			{
				// Let the parent handle it when it registers.
				return;
			}

			// Create a new node and add it
			GameObjectNode newObjNode = this.ScanGameObject(e.Object, true);
			this.InsertNodeSorted(newObjNode, parentNode);
			this.RegisterNodeTree(newObjNode);
		}
		private void Scene_GameObjectParentChanged(object sender, GameObjectParentChangedEventArgs e)
		{
			// Ignore events during transition
			if (Scene.IsSwitching) return;

			// Find the moved node
			GameObjectNode dragObjNode = this.FindNode(e.Object);
			Node oldParentNode = dragObjNode.Parent;
			Node newParentNode = e.Object.Parent == null ? this.objectModel.Root : this.FindNode(e.Object.Parent);
			
			// No parent node existing? This must be a new object then.
			if (newParentNode == null)
			{
				// Remove this node and let the parent handle it when it registers.
				oldParentNode.Nodes.Remove(dragObjNode);
				this.UnregisterNodeTree(dragObjNode);
				return;
			}
			
			// Save old state
			HashSet<object> expandedMap = new HashSet<object>();
			TreeNodeAdv dragObjViewNodeOld = this.objectView.FindNode(this.objectModel.GetPath(dragObjNode));
			bool wasSelected = dragObjViewNodeOld.IsSelected;
			this.objectView.SaveNodesExpanded(dragObjViewNodeOld, expandedMap);

			// Remove node
			oldParentNode.Nodes.Remove(dragObjNode);

			// Re-add node
			this.InsertNodeSorted(dragObjNode, newParentNode);
			TreeNodeAdv dragObjViewNodeNew = this.objectView.FindNode(this.objectModel.GetPath(dragObjNode));
			dragObjViewNodeNew.IsSelected = wasSelected;
			this.objectView.RestoreNodesExpanded(dragObjViewNodeNew, expandedMap);
		}

		private List<GameObjectNode> GetSelNodesFlattened()
		{
			Func<IEnumerable<TreeNodeAdv>, IEnumerable<GameObjectNode>> flattenNodes = null;
			flattenNodes = n => n.SelectMany(nodes => flattenNodes(
				nodes.Children.Where(c => c.Tag is GameObjectNode)))
				.Concat(n.Select(c => c.Tag as GameObjectNode));

			return flattenNodes(this.objectView.SelectedNodes).ToList();
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			HelpInfo result = null;
			Point globalPos = this.PointToScreen(localPos);

			// Hovering "Create Object" menu
			if (this.contextMenuNode.Visible)
			{
				ToolStripItem item = this.newToolStripMenuItem.DropDown.GetItemAtDeep(globalPos);
				Type itemType = item != null ? item.Tag as Type : null;
				if (item == this.gameObjectToolStripMenuItem) itemType = typeof(GameObject);
				if (itemType != null)
				{
					result = HelpInfo.FromMember(itemType);
				}
				captured = true;
			}
			// Hovering Object nodes
			else
			{
				Point treeLocalPos = this.objectView.PointToClient(globalPos);
				if (this.objectView.ClientRectangle.Contains(treeLocalPos))
				{
					TreeNodeAdv viewNode = this.objectView.GetNodeAt(treeLocalPos);
					ComponentNode cmpNode = viewNode != null ? viewNode.Tag as ComponentNode : null;
					GameObjectNode objNode = viewNode != null ? viewNode.Tag as GameObjectNode : null;
					if (cmpNode != null)
						result = HelpInfo.FromComponent(cmpNode.Component);
					else if (objNode != null)
						result = HelpInfo.FromGameObject(objNode.Obj);
				}
				captured = false;
			}

			return result;
		}

		string IToolTipProvider.GetToolTip(TreeNodeAdv viewNode, Aga.Controls.Tree.NodeControls.NodeControl nodeControl)
		{
			IEditorAction action = this.GetResourceOpenAction(viewNode);
			if (action != null) return string.Format(
				EditorBase.PluginRes.EditorBaseRes.SceneView_Help_Doubleclick,
				action.Description);
			else return null;
		}
	}
}
