using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using BitArray = System.Collections.BitArray;

using WeifenLuo.WinFormsUI.Docking;

using AdamsLair.WinForms.ColorControls;
using AdamsLair.WinForms.ItemModels;
using AdamsLair.WinForms.ItemViews;

using Duality;
using Duality.Input;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;

using Duality.Editor;
using Duality.Editor.Backend;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.CamView.CamViewStates;
using Duality.Editor.Plugins.CamView.CamViewLayers;

using Key = Duality.Input.Key;
using MouseButton = Duality.Input.MouseButton;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;


namespace Duality.Editor.Plugins.CamView
{
	public partial class CamView : DockContent, IHelpProvider, IMouseInputSource, IKeyboardInputSource
	{
		public class CameraChangedEventArgs : EventArgs
		{
			private	Camera	prevCam	= null;
			private	Camera	nextCam	= null;

			public Camera PreviousCamera
			{
				get { return this.prevCam; }
			}
			public Camera NextCamera
			{
				get { return this.nextCam; }
			}

			public CameraChangedEventArgs(Camera prev, Camera next)
			{
				this.prevCam = prev;
				this.nextCam = next;
			}
		}
		private class CamEntry
		{
			private Camera cam;

			public Camera Camera
			{
				get { return this.cam; }
			}
			public string CameraName
			{
				get { return this.cam.GameObj.FullName; }
			}

			public CamEntry(Camera cam)
			{
				this.cam = cam;
			}

			public override string ToString()
			{
				return this.CameraName;
			}
		}
		private class StateEntry
		{
			private Type stateType;
			private CamViewState state;

			public Type StateType
			{
				get { return this.stateType; }
			}
			public string StateName
			{
				get { return this.state.StateName; }
			}

			public StateEntry(Type stateType, CamViewState state)
			{
				this.stateType = stateType;
				this.state = state;
			}

			public override string ToString()
			{
				return this.StateName;
			}
		}
		private class LayerEntry
		{
			private Type layerType;
			private CamViewLayer layer;

			public Type LayerType
			{
				get { return this.layerType; }
			}
			public string LayerName
			{
				get { return this.layer.LayerName; }
			}
			public string LayerDesc
			{
				get { return this.layer.LayerDesc; }
			}

			public LayerEntry(Type stateType, CamViewLayer layer)
			{
				this.layerType = stateType;
				this.layer = layer;
			}

			public override string ToString()
			{
				return this.LayerName;
			}
		}
		private class ObjectVisibilityEntry
		{
			private Type componentType;

			public Type ComponentType
			{
				get { return this.componentType; }
			}
			public string ComponentName
			{
				get { return this.componentType != null ? this.componentType.GetTypeCSCodeName(true) : "null"; }
			}

			public ObjectVisibilityEntry(Type componentType)
			{
				this.componentType = componentType;
			}

			public override string ToString()
			{
				return this.ComponentName;
			}
		}


		public const float DefaultDisplayBoundRadius = 25.0f;

		private static CamView	activeCamView	= null;

		private	int						runtimeId					= 0;
		private	bool					isHiddenDocument			= false;
		private	INativeRenderableSite	graphicsControl				= null;
		private	GameObject				camObj						= null;
		private	Camera					camComp						= null;
		private	CamViewState			activeState					= null;
		private	List<CamViewLayer>		activeLayers				= null;
		private	HashSet<Type>			lockedLayers				= new HashSet<Type>();
		private	HashSet<Type>			objectVisibility			= null;
		private MenuModel				objectVisibilityMenuModel	= new MenuModel();
		private MenuStripMenuView		objectVisibilityMenuView	= null;
		private	EditingGuide			editingUserGuides			= new EditingGuide();
		private	ColorPickerDialog		bgColorDialog				= null;
		private	GameObject				nativeCamObj				= null;
		private	string					loadTempState				= null;
		private	string					loadTempPerspective			= null;
		private	InputEventMessageRedirector	globalInputFilter		= null;
		private DateTime					globalInputLastOtherKey	= DateTime.Now;
		private DateTime					lastLocalMouseMove		= DateTime.Now;
		private Color					oldColorDialogColor;
		private Color					selectedColorDialogColor;

		private	Dictionary<Type,CamViewLayer>	availLayers	= new Dictionary<Type,CamViewLayer>();
		private	Dictionary<Type,CamViewState>	availStates	= new Dictionary<Type,CamViewState>();

		private	int				inputLastUpdateFrame	= -1;
		private	bool			inputMouseCapture		= false;
		private	int				inputMouseX				= 0;
		private	int				inputMouseY				= 0;
		private	float			inputMouseWheel			= 0.0f;
		private	int				inputMouseButtons		= 0;
		private	bool			inputMouseInView		= false;
		private	bool			inputKeyFocus			= false;
		private	BitArray		inputKeyPressed			= new BitArray((int)Key.Last + 1, false);
		private	string			inputCharInput			= null;
		private	StringBuilder	inputCharInputBuffer	= new StringBuilder();


		public event EventHandler PerspectiveChanged	= null;
		public event EventHandler<CameraChangedEventArgs> CurrentCameraChanged	= null;


		/// <summary>
		/// [GET] Whether this <see cref="CamView"/> is a hidden document in the editor's <see cref="DockPanel"/>.
		/// This is typically true when storing the <see cref="CamView"/> in a <see cref="DockPane"/> with multiple
		/// tabs and then selecting a different tab, resulting in the <see cref="CamView"/> becoming hidden.
		/// </summary>
		public bool IsHiddenDocument
		{
			get { return this.isHiddenDocument; }
		}
		public ColorRgba BgColor
		{
			get { return this.camComp.ClearColor; }
			set { this.camComp.ClearColor = value; }
		}
		public ColorRgba FgColor
		{
			get { return this.BgColor.GetLuminance() < 0.5f ? ColorRgba.White : ColorRgba.Black; }
		}
		public float NearZ
		{
			get { return this.camComp.NearZ; }
			set { this.camComp.NearZ = value; }
		}
		public float FarZ
		{
			get { return this.camComp.FarZ; }
			set { this.camComp.FarZ = value; }
		}
		public float FocusDist
		{
			get { return (float)this.focusDist.Value; }
			set { this.focusDist.Value = Math.Min(Math.Max((decimal)value, this.focusDist.Minimum), this.focusDist.Maximum); }
		}
		public float FocusDistIncrement
		{
			get { return (float)this.focusDist.Increment; }
		}
		public PerspectiveMode PerspectiveMode
		{
			get { return this.camComp.Perspective; }
			set { this.camComp.Perspective = value; }
		}
		public Camera CameraComponent
		{
			get { return this.camComp; }
		}
		public GameObject CameraObj
		{
			get { return this.camObj; }
		}
		public INativeRenderableSite RenderableSite
		{
			get { return this.graphicsControl; }
		}
		public Control RenderableControl
		{
			get { return this.graphicsControl != null ? this.graphicsControl.Control : null; }
		}
		public ToolStrip ToolbarCamera
		{
			get { return this.toolbarCamera; }
		}
		public CamViewState ActiveState
		{
			get { return this.activeState; }
		}
		public IEnumerable<CamViewLayer> ActiveLayers
		{
			get { return this.activeLayers; }
		}
		public IEnumerable<Type> ObjectVisibility
		{
			get { return this.objectVisibility; }
		}
		public EditingGuide EditingUserGuides
		{
			get { return this.editingUserGuides; }
		}


		public CamView(int runtimeId, string initStateTypeName = null)
		{
			this.InitializeComponent();

			this.loadTempState = initStateTypeName;
			this.oldColorDialogColor = Color.FromArgb(64, 64, 64);
			this.selectedColorDialogColor = this.oldColorDialogColor;
			this.Text = Properties.CamViewRes.MenuItemName_CamView + " #" + runtimeId;
			this.runtimeId = runtimeId;
			this.toolbarCamera.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			
			var camViewStateTypeQuery = 
				from t in DualityEditorApp.GetAvailDualityEditorTypes(typeof(CamViewState))
				where !t.IsAbstract
				select t;
			foreach (TypeInfo t in camViewStateTypeQuery)
			{
				CamViewState state = t.CreateInstanceOf() as CamViewState;
				state.View = this;
				this.availStates.Add(t, state);
			}

			var camViewLayerTypeQuery = 
				from t in DualityEditorApp.GetAvailDualityEditorTypes(typeof(CamViewLayer))
				where !t.IsAbstract
				select t;
			foreach (TypeInfo t in camViewLayerTypeQuery)
			{
				CamViewLayer layer = t.CreateInstanceOf() as CamViewLayer;
				layer.View = this;
				this.availLayers.Add(t, layer);
			}

			this.snapToGridInactiveItem.Tag = Vector3.Zero;
			this.snapToGridPixelPerfectItem.Tag = new Vector3(1, 1, 1);
			this.snapToGrid16Item.Tag = new Vector3(16, 16, 16);
			this.snapToGrid32Item.Tag = new Vector3(32, 32, 32);
			this.snapToGrid64Item.Tag = new Vector3(64, 64, 64);
			this.editingUserGuides.GridSize = Vector3.Zero;
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.InitGLControl();
			this.InitNativeCamera();
			if (this.camSelector.Items.Count == 0)						this.InitCameraSelector();
			if (this.stateSelector.Items.Count == 0)					this.InitStateSelector();
			if (this.layerSelector.DropDownItems.Count == 0)			this.InitLayerSelector();
			if (this.objectVisibilitySelector.DropDownItems.Count == 0)	this.InitObjectVisibilitySelector();
			this.SetCurrentCamera(null);

			// Initialize PerspectiveMode Selector
			FieldInfo[] perspectiveModeFields = typeof(PerspectiveMode).GetTypeInfo().GetRuntimeFields().ToArray();
			foreach (string perspectiveName in Enum.GetNames(typeof(PerspectiveMode)))
			{
				ToolStripMenuItem perspectiveItem = new ToolStripMenuItem(perspectiveName);
				var perspectiveField = perspectiveModeFields.FirstOrDefault(m => m.Name == perspectiveName);
				perspectiveItem.Tag = HelpInfo.FromMember(perspectiveField);
				this.perspectiveDropDown.DropDownItems.Add(perspectiveItem);
			}

			// Initialize from loaded state id, if not done yet manually
			if (this.activeState == null)
			{
				Type stateType = ReflectionHelper.ResolveType(this.loadTempState) ?? typeof(SceneEditorCamViewState);
				this.SetCurrentState(stateType);
				this.loadTempState = null;
			}
			// If we set the state explicitly before, we'll still need to fire its enter event. See SetCurrentState.
			else
			{
				this.activeState.OnEnterState();
			}

			// Register DualityApp updater for camera steering behaviour
			this.RegisterEditorEvents();

			// Update Camera values according to GUI (which carries loaded or default settings)
			this.focusDist_ValueChanged(this.focusDist, null);
			this.camComp.ClearColor = this.selectedColorDialogColor.ToDualityRgba().WithAlpha(0);
			if (this.loadTempPerspective != null)
			{
				foreach (var item in this.perspectiveDropDown.DropDownItems.OfType<ToolStripMenuItem>())
				{
					if (item.Text == this.loadTempPerspective)
					{
						this.perspectiveDropDown_DropDownItemClicked(this.perspectiveDropDown, new ToolStripItemClickedEventArgs(item));
						break;
					}
				}
			}

			// Update camera transform properties & GUI
			this.OnPerspectiveChanged();
			this.OnCamTransformChanged();

			// Initially assume ownership of Duality rendering and audio
			this.MakeDualityTarget();

			// Determine whether we've started out as a hidden document (inactive tab) of the dock panel
			this.UpdateHiddenDocumentState();
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			// If this was the active Camera View, stop assuming this.
			if (activeCamView == this)
				activeCamView = null;

			if (this.nativeCamObj != null)
				this.nativeCamObj.Dispose();

			this.UnregisterEditorEvents();

			this.SetCurrentState((CamViewState)null);
		}
		protected override void OnDockStateChanged(EventArgs e)
		{
			base.OnDockStateChanged(e);
			this.UpdateHiddenDocumentState();
		}
		
		private void RegisterEditorEvents()
		{
			FileEventManager.ResourceModified		+= this.FileEventManager_ResourceModified;
			DualityEditorApp.Terminating			+= this.DualityEditorApp_Terminating;
			DualityEditorApp.HighlightObject		+= this.DualityEditorApp_HighlightObject;
			DualityEditorApp.ObjectPropertyChanged	+= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine			+= this.DualityEditorApp_UpdatingEngine;
			Scene.Entered							+= this.Scene_Entered;
			Scene.Leaving							+= this.Scene_Leaving;
			Scene.GameObjectRemoved					+= this.Scene_GameObjectUnregistered;
			Scene.ComponentRemoving					+= this.Scene_ComponentRemoving;

			this.DockPanel.ActiveContentChanged		+= this.DockPanel_ActiveContentChanged;
		}
		private void UnregisterEditorEvents()
		{
			FileEventManager.ResourceModified		-= this.FileEventManager_ResourceModified;
			DualityEditorApp.Terminating			-= this.DualityEditorApp_Terminating;
			DualityEditorApp.HighlightObject		-= this.DualityEditorApp_HighlightObject;
			DualityEditorApp.ObjectPropertyChanged	-= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine			-= this.DualityEditorApp_UpdatingEngine;
			Scene.Entered							-= this.Scene_Entered;
			Scene.Leaving							-= this.Scene_Leaving;
			Scene.GameObjectRemoved					-= this.Scene_GameObjectUnregistered;
			Scene.ComponentRemoving					-= this.Scene_ComponentRemoving;

			this.DockPanel.ActiveContentChanged		-= this.DockPanel_ActiveContentChanged;
		}
		/// <summary>
		/// Updates the <see cref="IsHiddenDocument"/> value of the <see cref="CamView"/> and fires
		/// events in the currently active state according to the detected changes. This will allow
		/// the active <see cref="CamViewState"/> to react to being hidden or becoming visible again.
		/// </summary>
		private void UpdateHiddenDocumentState()
		{
			bool lastHiddenDocument = this.isHiddenDocument;

			// Assume we're hidden unless proven false
			this.isHiddenDocument = true;

			// If the CamView is the DockPanel's currently active document, we're sure it's not hidden.
			if (this.DockPanel.ActiveDocument == this)
				this.isHiddenDocument = false;
			// The CamView is hidden when it's in a DockPane that isn't active (and thus "tabbed away")
			else if (this.Pane != null && this.Pane.ActiveContent == this)
				this.isHiddenDocument = false;

			// If we have an active state, notify it of changes to its visibility
			if (this.activeState != null)
			{
				if (this.isHiddenDocument && !lastHiddenDocument)
					this.activeState.OnHidden();
				else if (!this.isHiddenDocument && lastHiddenDocument)
					this.activeState.OnShown();
			}
		}

		private void InitGLControl()
		{
			this.SuspendLayout();

			// Get rid of a possibly existing old glControl
			if (this.graphicsControl != null)
			{
				Control oldControl = this.graphicsControl.Control;

				oldControl.MouseEnter		-= this.graphicsControl_MouseEnter;
				oldControl.MouseLeave		-= this.graphicsControl_MouseLeave;
				oldControl.MouseDown		-= this.graphicsControl_MouseDown;
				oldControl.MouseUp			-= this.graphicsControl_MouseUp;
				oldControl.MouseWheel		-= this.graphicsControl_MouseWheel;
				oldControl.MouseMove		-= this.graphicsControl_MouseMove;
				oldControl.GotFocus			-= this.graphicsControl_GotFocus;
				oldControl.LostFocus		-= this.graphicsControl_LostFocus;
				oldControl.PreviewKeyDown	-= this.graphicsControl_PreviewKeyDown;
				oldControl.KeyDown			-= this.graphicsControl_KeyDown;
				oldControl.KeyUp			-= this.graphicsControl_KeyUp;
				oldControl.KeyPress			-= this.graphicsControl_KeyPress;
				oldControl.Resize			-= this.graphicsControl_Resize;

				this.graphicsControl.Dispose();
				this.Controls.Remove(oldControl);
			}

			// Create a new glControl
			this.graphicsControl = DualityEditorApp.CreateRenderableSite();
			Control control = this.graphicsControl.Control;

			control.BackColor = Color.Black;
			control.Dock = DockStyle.Fill;
			control.Name = "graphicsControl";
			control.AllowDrop = true;
			control.MouseEnter		+= this.graphicsControl_MouseEnter;
			control.MouseLeave		+= this.graphicsControl_MouseLeave;
			control.MouseDown		+= this.graphicsControl_MouseDown;
			control.MouseUp			+= this.graphicsControl_MouseUp;
			control.MouseWheel		+= this.graphicsControl_MouseWheel;
			control.MouseMove		+= this.graphicsControl_MouseMove;
			control.GotFocus		+= this.graphicsControl_GotFocus;
			control.LostFocus		+= this.graphicsControl_LostFocus;
			control.PreviewKeyDown	+= this.graphicsControl_PreviewKeyDown;
			control.KeyDown			+= this.graphicsControl_KeyDown;
			control.KeyUp			+= this.graphicsControl_KeyUp;
			control.KeyPress		+= this.graphicsControl_KeyPress;
			control.Resize			+= this.graphicsControl_Resize;
			this.Controls.Add(control);
			this.Controls.SetChildIndex(control, 0);

			this.ResumeLayout(true);
		}
		private void InitStateSelector()
		{
			this.stateSelector.BeginUpdate();
			this.stateSelector.Items.Clear();
			foreach (var pair in this.availStates)
			{
				this.stateSelector.Items.Add(new StateEntry(pair.Key, pair.Value));
			}
			this.stateSelector.EndUpdate();
		}
		private void InitLayerSelector()
		{
			this.layerSelector.DropDown.Closing -= this.layerSelector_Closing;
			this.layerSelector.DropDownItems.Clear();

			foreach (var pair in this.availLayers)
			{
				LayerEntry layerEntry = new LayerEntry(pair.Key, pair.Value);
				ToolStripMenuItem layerItem = new ToolStripMenuItem(layerEntry.LayerName);
				layerItem.Tag = layerEntry;
				layerItem.Checked = this.activeLayers != null && this.activeLayers.Any(l => l.GetType() == layerEntry.LayerType);
				layerItem.Enabled = !this.lockedLayers.Contains(layerEntry.LayerType);
				this.layerSelector.DropDownItems.Add(layerItem);
			}
			this.layerSelector.DropDown.Closing += this.layerSelector_Closing;
		}
		private void InitObjectVisibilitySelector()
		{
			this.objectVisibilitySelector.DropDown.Closing -= this.objectVisibilitySelector_Closing;
			
			var typesWithCount = (
				from componentType in DualityApp.GetAvailDualityTypes(typeof(Component))
				where !componentType.IsAbstract && !componentType.GetAttributesCached<EditorHintFlagsAttribute>().Any(attrib => attrib.Flags.HasFlag(MemberFlags.Invisible))
				select new { Type = componentType, Count = Scene.Current.FindComponents(componentType).Count() }
				).ToArray();

			typesWithCount = (
				from typeInfo in typesWithCount
				where typeInfo.Count > 0
				orderby typeInfo.Count descending
				select typeInfo
				).ToArray();
			
			// Remove old items
			this.objectVisibilityMenuModel.ClearItems();

			// Add new items
			bool hierarchial = typesWithCount.Length > 8;
			int index = 0;
			foreach (var typeInfo in typesWithCount)
			{
				ObjectVisibilityEntry entry = new ObjectVisibilityEntry(typeInfo.Type);

				string itemPath;
				int sortVal = 0;
				if (hierarchial && index >= 5)
				{
					itemPath = typeInfo.Type.GetEditorCategory().Concat(new [] { entry.ComponentName }).ToString("/");
					sortVal = -typeInfo.Count;
				}
				else
				{
					itemPath = entry.ComponentName;
					sortVal = MenuModelItem.SortValue_Top - typeInfo.Count;
				}
				
				MenuModelItem typeItem = this.objectVisibilityMenuModel.RequestItem(itemPath);
				typeItem.Name = entry.ComponentName;
				typeItem.Icon = typeInfo.Type.GetEditorImage();
				typeItem.SortValue = sortVal;
				typeItem.Tag = entry;
				typeItem.Checkable = true;
				typeItem.Checked = this.objectVisibility != null && this.objectVisibility.Contains(typeInfo.Type);
				typeItem.ActionHandler = this.objectVisibilitySelector_ItemPerformAction;

				index++;
			}
			if (hierarchial)
			{
				this.objectVisibilityMenuModel.AddItem(new MenuModelItem
				{
					Name		= "TopSeparator",
					TypeHint	= MenuItemTypeHint.Separator,
					SortValue	= MenuModelItem.SortValue_Top + 1
				});
			}
			
			if (this.objectVisibilityMenuView == null)
			{
				this.objectVisibilityMenuView = new MenuStripMenuView(this.objectVisibilitySelector.DropDownItems);
				this.objectVisibilityMenuView.Model = this.objectVisibilityMenuModel;
			}

			this.objectVisibilitySelector.DropDown.Closing += this.objectVisibilitySelector_Closing;
		}
		private void InitCameraSelector()
		{
			this.camSelector.BeginUpdate();
			this.camSelector.Items.Clear();
			this.camSelector.Items.Add(new CamEntry(this.nativeCamObj.GetComponent<Camera>()));
			foreach (Camera c in Scene.Current.AllObjects.GetComponents<Camera>().OrderBy(c => c.GameObj.FullName))
			{
				this.camSelector.Items.Add(new CamEntry(c));
			}
			this.camSelector.EndUpdate();
		}
		private void InitNativeCamera()
		{
			// Create internal Camera object
			this.nativeCamObj = new GameObject();
			this.nativeCamObj.Name = "CamView Camera " + this.runtimeId;
			this.nativeCamObj.AddComponent<Transform>();
			this.nativeCamObj.AddComponent<SoundListener>().MakeCurrent();

			Camera c = this.nativeCamObj.AddComponent<Camera>();
			c.ClearColor = ColorRgba.DarkGrey;
			c.FarZ = 100000.0f;

			this.nativeCamObj.Transform.Pos = new Vector3(0.0f, 0.0f, -c.FocusDist);
			DualityEditorApp.EditorObjects.AddObject(this.nativeCamObj);
		}
		private int GetCameraSelectorIndex(Camera c)
		{
			for (int i = 0; i < this.camSelector.Items.Count; i++)
			{
				if ((this.camSelector.Items[i] as CamEntry).Camera == c)
					return i;
			}
			return -1;
		}

		public void SetCurrentCamera(Camera c)
		{
			if (c == null) c = this.nativeCamObj.GetComponent<Camera>();
			if (c == this.camComp) return;

			Camera prev = this.camComp;

			if (c.GameObj == this.nativeCamObj)
			{
				this.camObj = this.nativeCamObj;
				this.camComp = this.camObj.GetComponent<Camera>();
				this.camSelector.SelectedIndex = 0;
			}
			else
			{
				this.camObj = c.GameObj;
				this.camComp = c;
				this.camSelector.SelectedIndex = this.GetCameraSelectorIndex(c);
			}

			this.OnCurrentCameraChanged(prev, this.camComp);
			this.RenderableControl.Invalidate();
		}
		public void SetCurrentState(Type stateType)
		{
			if (!typeof(CamViewState).IsAssignableFrom(stateType)) return;
			if (this.activeState != null && this.activeState.GetType() == stateType) return;

			this.SetCurrentState(this.availStates[stateType]);
		}
		public void SetCurrentState(CamViewState state)
		{
			if (this.activeState == state) return;
			if (this.activeState != null) this.activeState.OnLeaveState();

			this.activeState = state;
			if (this.activeState != null)
			{
				if (this.stateSelector.Items.Count == 0) this.InitStateSelector();
				this.stateSelector.SelectedIndex = this.stateSelector.Items.IndexOf(this.stateSelector.Items.Cast<StateEntry>().FirstOrDefault(e => e.StateType == this.activeState.GetType()));
			}
			else
				this.stateSelector.SelectedIndex = -1;

			// If we have a graphics control, we have initialized properly and can enter the state right away.
			// Otherwise, this is the initial state and we'll need to wait until initialization. Enter the state later.
			if (this.graphicsControl != null)
			{
				if (this.activeState != null) this.activeState.OnEnterState();
				this.RenderableControl.Invalidate();
			}
		}

		public void SetActiveLayers(IEnumerable<Type> layerTypes)
		{
			if (this.activeLayers == null) this.activeLayers = new List<CamViewLayer>();

			// Deactivate unused layers
			for (int i = this.activeLayers.Count - 1; i >= 0; i--)
			{
				Type layerType = this.activeLayers[i].GetType();
				if (!layerTypes.Contains(layerType)) this.DeactivateLayer(this.activeLayers[i]);
			}

			// Activate not-yet-active layers
			foreach (Type layerType in layerTypes)
				this.ActivateLayer(layerType);
		}
		public void ActivateLayer(CamViewLayer layer)
		{
			if (this.activeLayers == null) this.activeLayers = new List<CamViewLayer>();
			if (this.activeLayers.Contains(layer)) return;
			if (this.activeLayers.Any(l => l.GetType() == layer.GetType())) return;
			if (this.lockedLayers.Contains(layer.GetType())) return;

			this.activeLayers.Add(layer);
			layer.View = this;
			// No glControl yet? We're not initialized properly and this is the initial state. Enter the state later.
			if (this.graphicsControl != null)
			{
				layer.OnActivateLayer();
				this.RenderableControl.Invalidate();
			}
		}
		public void ActivateLayer(Type layerType)
		{
			this.ActivateLayer(this.availLayers[layerType]);
		}
		public void DeactivateLayer(CamViewLayer layer)
		{
			if (this.activeLayers == null) this.activeLayers = new List<CamViewLayer>();
			if (!this.activeLayers.Contains(layer)) return;
			if (this.lockedLayers.Contains(layer.GetType())) return;

			layer.OnDeactivateLayer();
			layer.View = null;
			this.activeLayers.Remove(layer);
			this.RenderableControl.Invalidate();
		}
		public void DeactivateLayer(Type layerType)
		{
			this.DeactivateLayer(this.activeLayers.FirstOrDefault(l => l.GetType() == layerType));
		}
		public void LockLayer(CamViewLayer layer)
		{
			this.LockLayer(layer.GetType());
		}
		public void LockLayer(Type layerType)
		{
			if (this.lockedLayers.Contains(layerType)) return;
			this.lockedLayers.Add(layerType);
		}
		public void UnlockLayer(CamViewLayer layer)
		{
			this.UnlockLayer(layer.GetType());
		}
		public void UnlockLayer(Type layerType)
		{
			this.lockedLayers.Remove(layerType);
		}

		public void SetObjectVisibility(IEnumerable<Type> visibleObjectTypes)
		{
			if (this.objectVisibility == null) this.objectVisibility = new HashSet<Type>();

			// Deactivate unused layers
			foreach (Type visibleType in this.objectVisibility.ToArray())
			{
				if (!visibleObjectTypes.Contains(visibleType))
					this.SetObjectVisibility(visibleType, false);
			}

			// Activate not-yet-active layers
			foreach (Type objectType in visibleObjectTypes)
				this.SetObjectVisibility(objectType, true);
		}
		public void SetObjectVisibility(Type objectType, bool visible)
		{
			if (visible)
			{
				if (this.objectVisibility == null) this.objectVisibility = new HashSet<Type>();
				if (!this.objectVisibility.Contains(objectType))
				{
					this.objectVisibility.Add(objectType);
					this.RenderableControl.Invalidate();
				}
			}
			else
			{
				if (this.objectVisibility != null && this.objectVisibility.Contains(objectType))
				{
					this.objectVisibility.Remove(objectType);
					this.RenderableControl.Invalidate();
				}
			}
		}

		public void MakeDualityTarget()
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			activeCamView = this;

			Control mainControl = (this.graphicsControl != null ? this.graphicsControl.Control : null) ?? this;
			DualityApp.TargetResolution = new Vector2(mainControl.ClientSize.Width, mainControl.ClientSize.Height);
			DualityApp.Mouse.Source = this;
			DualityApp.Keyboard.Source = this;

			SoundListener localListener = this.CameraObj.GetComponent<SoundListener>();
			if (localListener != null && localListener.Active)
				localListener.MakeCurrent();
		}

		internal void SaveUserData(XElement node)
		{
			Camera nativeCamera = this.nativeCamObj.GetComponent<Camera>();

			node.SetElementValue("Perspective", nativeCamera.Perspective);
			node.SetElementValue("FocusDist", nativeCamera.FocusDist);
			XElement bgColorElement = new XElement("BackgroundColor");
			{
				bgColorElement.SetElementValue("R", nativeCamera.ClearColor.R);
				bgColorElement.SetElementValue("G", nativeCamera.ClearColor.G);
				bgColorElement.SetElementValue("B", nativeCamera.ClearColor.B);
				bgColorElement.SetElementValue("A", nativeCamera.ClearColor.A);
			}
			node.Add(bgColorElement);

			XElement snapToGridSizeElement = new XElement("SnapToGridSize");
			node.Add(snapToGridSizeElement);
			snapToGridSizeElement.SetElementValue("X", this.editingUserGuides.GridSize.X);
			snapToGridSizeElement.SetElementValue("Y", this.editingUserGuides.GridSize.Y);
			snapToGridSizeElement.SetElementValue("Z", this.editingUserGuides.GridSize.Z);

			if (this.activeState != null) 
				node.SetElementValue("ActiveState", this.activeState.GetType().GetTypeId());

			XElement stateListNode = new XElement("States");
			foreach (var pair in this.availStates)
			{
				XElement stateNode = new XElement("State");
				stateNode.SetAttributeValue("type", pair.Key.GetTypeId());
				pair.Value.SaveUserData(stateNode);
				if (!stateNode.IsEmpty)
					stateListNode.Add(stateNode);
			}
			if (!stateListNode.IsEmpty)
				node.Add(stateListNode);

			XElement layerListNode = new XElement("Layers");
			foreach (var pair in this.availLayers)
			{
				XElement layerNode = new XElement("Layer");
				layerNode.SetAttributeValue("type", pair.Key.GetTypeId());
				pair.Value.SaveUserData(layerNode);
				if (!layerListNode.IsEmpty)
					layerListNode.Add(layerNode);
			}
			if (!layerListNode.IsEmpty)
				node.Add(layerListNode);
		}
		internal void LoadUserData(XElement node)
		{
			decimal tryParseDecimal;
			if (node.GetElementValue("FocusDist", out tryParseDecimal))
			{
				this.focusDist.Value = Math.Abs(tryParseDecimal);
			}
			XElement bgColorElement = node.Element("BackgroundColor");
			if (bgColorElement != null)
			{
				ColorRgba color = ColorRgba.Black;
				bgColorElement.TryGetElementValue("R", ref color.R);
				bgColorElement.TryGetElementValue("G", ref color.G);
				bgColorElement.TryGetElementValue("B", ref color.B);
				bgColorElement.TryGetElementValue("A", ref color.A);

				this.oldColorDialogColor = Color.FromArgb(color.A, color.R, color.G, color.B);
				this.selectedColorDialogColor = this.oldColorDialogColor;
			}
			this.loadTempPerspective = node.GetElementValue("Perspective", this.loadTempPerspective);
			this.loadTempState = node.GetElementValue("ActiveState", this.loadTempState);

			XElement snapToGridSizeElement = node.Element("SnapToGridSize");
			if (snapToGridSizeElement != null)
			{
				Vector3 size;
				snapToGridSizeElement.GetElementValue("X", out size.X);
				snapToGridSizeElement.GetElementValue("Y", out size.Y);
				snapToGridSizeElement.GetElementValue("Z", out size.Z);
				this.editingUserGuides.GridSize = size;
			}

			XElement stateListNode = node.Element("States");
			if (stateListNode != null)
			{
				Dictionary<string,XElement> stateElementMap = new Dictionary<string,XElement>();
				foreach (XElement stateElement in stateListNode.Elements("State"))
				{
					string type = stateElement.GetAttributeValue("type");
					if (!string.IsNullOrEmpty(type))
						stateElementMap.Add(type, stateElement);
				}
				foreach (var pair in this.availStates)
				{
					XElement stateNode;
					if (!stateElementMap.TryGetValue(pair.Key.GetTypeId(), out stateNode)) continue;

					pair.Value.LoadUserData(stateNode);
				}
			}

			XElement layerListNode = node.Element("Layers");
			if (layerListNode != null)
			{
				Dictionary<string,XElement> layerElementMap = new Dictionary<string,XElement>();
				foreach (XElement layerElement in stateListNode.Elements("Layer"))
				{
					string type = layerElement.GetAttributeValue("type");
					if (!string.IsNullOrEmpty(type))
						layerElementMap.Add(type, layerElement);
				}
				foreach (var pair in this.availLayers)
				{
					XElement layerNode;
					if (!layerElementMap.TryGetValue(pair.Key.GetTypeId(), out layerNode)) continue;

					pair.Value.LoadUserData(layerNode);
				}
			}
		}

		public void OnCamTransformChanged()
		{
			//if (this.camInternal) return;
			DualityEditorApp.NotifyObjPropChanged(
				this, new ObjectSelection(this.camObj.Transform),
				ReflectionInfo.Property_Transform_RelativeVel,
				ReflectionInfo.Property_Transform_RelativeAngleVel,
				ReflectionInfo.Property_Transform_RelativeAngle,
				ReflectionInfo.Property_Transform_RelativePos);
		}
		public void SetToolbarCamSettingsEnabled(bool value)
		{
			this.snapToGridSelector.Enabled = value;
			this.perspectiveDropDown.Enabled = value;
			this.focusDist.Enabled = value;
			this.camSelector.Enabled = value;
			this.showBgColorDialog.Enabled = value;
			this.layerSelector.Enabled = value;
			this.objectVisibilitySelector.Enabled = value;
			this.buttonResetZoom.Enabled = value;
		}

		public void ResetCamera()
		{
			this.FocusOnPos(Vector3.Zero);
			this.camObj.Transform.Angle = 0.0f;
		}
		public void FocusOnPos(Vector3 targetPos)
		{
			if (!this.activeState.CameraActionAllowed) return;
			targetPos -= Vector3.UnitZ * MathF.Abs(this.camComp.FocusDist);
			//targetPos.Z = MathF.Min(this.camObj.Transform.Pos.Z, targetPos.Z);
			this.camObj.Transform.Pos = targetPos;
			this.OnCamTransformChanged();
			this.RenderableControl.Invalidate();
		}
		public void FocusOnObject(GameObject obj)
		{
			this.FocusOnPos((obj == null || obj.Transform == null) ? Vector3.Zero : obj.Transform.Pos);
		}


		private void OnPerspectiveChanged()
		{
			if (this.camObj != this.nativeCamObj)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this, new ObjectSelection(this.camComp),
					ReflectionInfo.Property_Camera_FocusDist);
			}
			this.RenderableControl.Invalidate();

			if (this.PerspectiveChanged != null)
				this.PerspectiveChanged(this, EventArgs.Empty);
		}
		private void OnCurrentCameraChanged(Camera prev, Camera next)
		{
			if (this.CurrentCameraChanged != null)
				this.CurrentCameraChanged(this, new CameraChangedEventArgs(prev, next));
			
			if (prev != null) prev.RemoveEditorRendererFilter(this.RendererFilter);
			if (next != null) next.AddEditorRendererFilter(this.RendererFilter);
		}
		
		private bool RendererFilter(ICmpRenderer r)
		{
			GameObject obj = (r as Component).GameObj;

			if (this.objectVisibility != null && this.objectVisibility.Count > 0)
			{
				bool match = false;
				foreach (Type type in this.objectVisibility)
				{
					if (obj.GetComponent(type) != null)
					{
						match = true;
						break;
					}
				}
				if (!match) return false;
			}

			DesignTimeObjectData data = DesignTimeObjectData.Get(obj);
			return !data.IsHidden;
		}
		
		private void InstallFocusHook()
		{
			if (this.graphicsControl.Control.Focused) return;

			// Hook global message filter
			if (this.globalInputFilter == null)
			{
				this.globalInputFilter = new InputEventMessageRedirector(
					this.graphicsControl.Control, 
					this.FocusHookFilter, 
					InputEventMessageRedirector.MessageType.MouseWheel,
					InputEventMessageRedirector.MessageType.KeyDown);
				Application.AddMessageFilter(this.globalInputFilter);
			}
		}
		private void RemoveFocusHook()
		{
			// Remove global message filter
			if (this.globalInputFilter != null)
			{
				Application.RemoveMessageFilter(this.globalInputFilter);
				this.globalInputFilter = null;
			}
		}
		private bool FocusHookFilter(InputEventMessageRedirector.MessageType type, EventArgs e)
		{
			// Don't capture when the sandbox is active. Input is likely to be needed in the current Game View.
			if (Sandbox.State == SandboxState.Playing) return false;

			// Capture mouse wheel for camera navigation
			if (type == InputEventMessageRedirector.MessageType.MouseWheel)
			{
				return true;
			}
			// Capture space key for alternative camera navigation
			else if (type == InputEventMessageRedirector.MessageType.KeyDown)
			{
				KeyEventArgs keyArgs = e as KeyEventArgs;
				if (keyArgs == null) return false;
				if (keyArgs.KeyCode == Keys.Space)
				{
					// Only capture the space key when we had recent movement and no other input keys.
					// The user might be typing something with the mouse cursor accidentally hovering here.
					if ((DateTime.Now - this.globalInputLastOtherKey).TotalMilliseconds > 1000 &&
						(DateTime.Now - this.lastLocalMouseMove).TotalMilliseconds < 1000)
					{
						return true;
					}
				}
				else
				{
					this.globalInputLastOtherKey = DateTime.Now;
				}
			}
			
			return false;
		}
		private void graphicsControl_MouseLeave(object sender, EventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				this.inputMouseInView = false;
			}

			this.RemoveFocusHook();

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.RenderableControl.Invalidate();
		}
		private void graphicsControl_MouseEnter(object sender, EventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				this.inputMouseInView = true;
			}

			this.InstallFocusHook();

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.RenderableControl.Invalidate();
		}
		private void graphicsControl_MouseDown(object sender, MouseEventArgs e)
		{
			this.inputMouseCapture = true;
			if (this.activeState.EngineUserInput)
			{
				MouseButton inputButton = e.Button.ToDualitySingle();
				this.inputMouseButtons |= e.Button.ToDuality();
			}
		}
		private void graphicsControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				MouseButton inputButton = e.Button.ToDualitySingle();
				this.inputMouseButtons &= ~e.Button.ToDuality();
			}
		}
		private void graphicsControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!this.RenderableControl.Focused) this.RenderableControl.Focus();

			if (this.activeState.EngineUserInput)
			{
				this.inputMouseWheel += e.Delta / 120.0f;
			}
		}
		private void graphicsControl_MouseMove(object sender, MouseEventArgs e)
		{
			this.lastLocalMouseMove = DateTime.Now;

			if (this.activeState.EngineUserInput)
			{
				int lastX = this.inputMouseX;
				int lastY = this.inputMouseY;
				this.inputMouseX = e.X;
				this.inputMouseY = e.Y;
			}

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.RenderableControl.Invalidate();
		}
		private void graphicsControl_GotFocus(object sender, EventArgs e)
		{
			this.RemoveFocusHook();
			this.inputMouseCapture = true;

			if (this.activeState != null)
			{
				if (this.activeState.EngineUserInput)
				{
					this.inputKeyFocus = true;
				}
			}
		}
		private void graphicsControl_LostFocus(object sender, EventArgs e)
		{
			if (this.activeState != null && this.activeState.EngineUserInput)
			{
				this.inputKeyFocus = false;
			}
		}
		private void graphicsControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (this.activeState.EngineUserInput) 
				e.IsInputKey = // Special key blacklist: Do not forward to game
					e.KeyCode != Keys.F1 &&
					e.KeyCode != Keys.F2 &&
					e.KeyCode != Keys.F3 &&
					e.KeyCode != Keys.F4 &&
					e.KeyCode != Keys.F5 &&
					e.KeyCode != Keys.F6 &&
					e.KeyCode != Keys.F7 &&
					e.KeyCode != Keys.F8 &&
					e.KeyCode != Keys.F9 &&
					e.KeyCode != Keys.F10 &&
					e.KeyCode != Keys.F11 &&
					e.KeyCode != Keys.F12;
			else
				e.IsInputKey = // Special key whitelist: Do forward to CamViewState
					e.KeyCode == Keys.Left || 
					e.KeyCode == Keys.Right || 
					e.KeyCode == Keys.Up || 
					e.KeyCode == Keys.Down;
		}
		private void graphicsControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (!this.RenderableControl.Focused) this.RenderableControl.Focus();

			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Alt)
			{
				this.inputMouseCapture = false;
			}

			if (this.activeState.EngineUserInput)
			{
				Key inputKey = e.KeyCode.ToDualitySingle();
				bool wasPressed = this.inputKeyPressed[(int)inputKey];
				this.inputKeyPressed = this.inputKeyPressed.Or(e.KeyCode.ToDuality());
			}
		}
		private void graphicsControl_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				Key inputKey = e.KeyCode.ToDualitySingle();
				this.inputKeyPressed = this.inputKeyPressed.And(e.KeyCode.ToDuality().Not());
			}

			// Use the number keys for a quick-select of states - but only when not consumed by the game
			if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
			{
				bool consumedByGame = this.activeState.EngineUserInput && Sandbox.IsActive;
				if (!consumedByGame && this.stateSelector.Items.Count > e.KeyCode - Keys.D1)
					this.stateSelector.SelectedIndex = e.KeyCode - Keys.D1;
			}
		}
		private void graphicsControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				if (e.KeyChar == '\b') return; // Filter out backspace control character, so we don't get more chars than the regular launcher backend.
				this.inputCharInputBuffer.Append(e.KeyChar);
			}
		}
		private void graphicsControl_Resize(object sender, EventArgs e)
		{
			if (activeCamView == this)
			{
				Control mainControl = (this.graphicsControl != null ? this.graphicsControl.Control : null) ?? this;
				DualityApp.TargetResolution = new Vector2(mainControl.ClientSize.Width, mainControl.ClientSize.Height);
			}
			this.RenderableControl.Invalidate();
		}

		private void focusDist_ValueChanged(object sender, EventArgs e)
		{
			if (this.camComp == null) return;

			if (this.focusDist.Value < 30m)
				this.focusDist.Increment = 1m;
			else if (this.focusDist.Value < 150m)
				this.focusDist.Increment = 5m;
			else if (this.focusDist.Value < 300m)
				this.focusDist.Increment = 10m;
			else if (this.focusDist.Value < 1500m)
				this.focusDist.Increment = 50m;
			else if (this.focusDist.Value < 3000m)
				this.focusDist.Increment = 100m;
			else if (this.focusDist.Value < 15000m)
				this.focusDist.Increment = 500m;
			else if (this.focusDist.Value < 30000m)
				this.focusDist.Increment = 1000m;
			else if (this.focusDist.Value < 150000m)
				this.focusDist.Increment = 5000m;
			else
				this.focusDist.Increment = 10000m;

			this.camComp.FocusDist = (float)this.focusDist.Value;
			this.OnPerspectiveChanged();
		}
		private void showBgColorDialog_Click(object sender, EventArgs e)
		{
			if (this.bgColorDialog == null)
			{
				this.bgColorDialog = new ColorPickerDialog
				{
					BackColor = Color.FromArgb(212, 212, 212),
					OldColor = this.oldColorDialogColor,
					SelectedColor = this.oldColorDialogColor,
					AlphaEnabled = false
				};
			}

			this.bgColorDialog.OldColor = Color.FromArgb(
				255,
				this.camComp.ClearColor.R,
				this.camComp.ClearColor.G,
				this.camComp.ClearColor.B);
			this.bgColorDialog.PrimaryAttribute = ColorPickerDialog.PrimaryAttrib.Hue;
			if (this.bgColorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				this.bgColorDialog_ValueChanged(this.bgColorDialog, null);
		}
		private void bgColorDialog_ValueChanged(object sender, EventArgs e)
		{
			if (this.camObj != this.nativeCamObj)
			{
				UndoRedoManager.Do(new EditPropertyAction(null,
					ReflectionInfo.Property_Camera_ClearColor,
					new[] { this.camComp },
					new[] { new ColorRgba(
						this.bgColorDialog.SelectedColor.R,
						this.bgColorDialog.SelectedColor.G,
						this.bgColorDialog.SelectedColor.B,
						0) as object }));
			}
			else
			{
				this.camComp.ClearColor = this.bgColorDialog.SelectedColor.ToDualityRgba().WithAlpha(0);
			}
			this.RenderableControl.Invalidate();
		}

		private void buttonResetZoom_Click(object sender, EventArgs e)
		{
			this.camObj.Transform.Pos = new Vector3(this.camObj.Transform.Pos.Xy, -MathF.Abs(this.camComp.FocusDist));
			this.RenderableControl.Invalidate();
		}
		
		private void FileEventManager_ResourceModified(object sender, ResourceEventArgs e)
		{
			if (!e.IsResource) return;
			this.RenderableControl.Invalidate();
		}
		private void DualityEditorApp_Terminating(object sender, EventArgs e)
		{
			this.UnregisterEditorEvents();
		}
		private void DualityEditorApp_HighlightObject(object sender, HighlightObjectEventArgs e)
		{
			if (!e.Mode.HasFlag(HighlightMode.Spatial)) return;
			if (sender == this) return;

			GameObject obj = e.Target.MainGameObject;
			Component cmp = e.Target.MainComponent;

			if (obj != null) this.FocusOnObject(obj);
			if (cmp != null) this.FocusOnObject(cmp.GameObj);
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (!e.Objects.Resources.Any() && !e.Objects.OfType<DesignTimeObjectData>().Any()) return;
			this.RenderableControl.Invalidate();
		}
		private void DualityEditorApp_UpdatingEngine(object sender, EventArgs e)
		{
			if (this.camObj != null && this.camObj != this.nativeCamObj)
				DualityEditorApp.UpdateGameObject(this.camObj);
		}
		
		private void Scene_Entered(object sender, EventArgs e)
		{
			if (!Sandbox.IsActive && !Sandbox.IsChangingState) this.ResetCamera();
			this.RenderableControl.Invalidate();
		}
		private void Scene_Leaving(object sender, EventArgs e)
		{
			if (this.camObj != this.nativeCamObj) this.SetCurrentCamera(null);
			this.RenderableControl.Invalidate();
		}
		private void Scene_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this.camComp == e.Component) this.SetCurrentCamera(null);
		}
		private void Scene_GameObjectUnregistered(object sender, GameObjectEventArgs e)
		{
			if (this.camObj == e.Object) this.SetCurrentCamera(null);
		}
		
		private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
		{
			this.UpdateHiddenDocumentState();
		}
		private void camSelector_DropDown(object sender, EventArgs e)
		{
			this.InitCameraSelector();
		}
		private void camSelector_DropDownClosed(object sender, EventArgs e)
		{
			if (this.camSelector.SelectedIndex == -1)
			{
				this.camSelector.SelectedIndex = this.GetCameraSelectorIndex(this.camComp);
				return;
			}
		}
		private void camSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SetCurrentCamera((this.camSelector.SelectedItem as CamEntry).Camera);
		}
		private void stateSelector_DropDown(object sender, EventArgs e)
		{
			this.InitStateSelector();
		}
		private void stateSelector_DropDownClosed(object sender, EventArgs e)
		{
			if (this.stateSelector.SelectedIndex == -1)
			{
				this.stateSelector.SelectedIndex = this.activeState != null ? this.stateSelector.Items.IndexOf(this.stateSelector.Items.Cast<StateEntry>().FirstOrDefault(sce => sce.StateType == this.activeState.GetType())) : -1;
				return;
			}
		}
		private void stateSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.stateSelector.SelectedItem == null) return;
			this.SetCurrentState(((StateEntry)this.stateSelector.SelectedItem).StateType);
		}
		private void layerSelector_DropDownOpening(object sender, EventArgs e)
		{
			this.InitLayerSelector();
		}
		private void layerSelector_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			LayerEntry layerEntry = e.ClickedItem.Tag as LayerEntry;
			ToolStripMenuItem layerItem = e.ClickedItem as ToolStripMenuItem;

			if (layerItem.Checked)
				this.DeactivateLayer(layerEntry.LayerType);
			else
				this.ActivateLayer(layerEntry.LayerType);

			layerItem.Checked = this.activeLayers.Any(l => l.GetType() == layerEntry.LayerType);
		}
		private void layerSelector_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
		}
		private void objectVisibilitySelector_DropDownOpening(object sender, EventArgs e)
		{
			this.InitObjectVisibilitySelector();
		}
		private void objectVisibilitySelector_ItemPerformAction(object sender, EventArgs e)
		{
			MenuModelItem item = sender as MenuModelItem;
			ObjectVisibilityEntry entry = item.Tag as ObjectVisibilityEntry;
			this.SetObjectVisibility(entry.ComponentType, item.Checked);
		}
		private void objectVisibilitySelector_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked) e.Cancel = true;
		}
		private void snapToGridSelector_DropDownOpening(object sender, EventArgs e)
		{
			bool anyChecked = false;
			foreach (ToolStripMenuItem item in this.snapToGridSelector.DropDownItems)
			{
				item.Checked = this.editingUserGuides.GridSize.Equals(item.Tag);
				if (item.Checked) anyChecked = true;
			}
			this.snapToGridCustomItem.Checked = !anyChecked;
		}
		private void snapToGridSelector_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem.Tag == null)
			{
				GridSizeDialog dialog = new GridSizeDialog();
				dialog.GridSize = this.editingUserGuides.GridSize;

				DialogResult result = dialog.ShowDialog();
				if (result == DialogResult.OK)
				{
					this.editingUserGuides.GridSize = dialog.GridSize;
				}
			}
			else
			{
				this.editingUserGuides.GridSize = (Vector3)e.ClickedItem.Tag;
			}

			this.RenderableControl.Invalidate();
		}
		private void perspectiveDropDown_DropDownOpening(object sender, EventArgs e)
		{
			foreach (var item in this.perspectiveDropDown.DropDownItems.OfType<ToolStripMenuItem>())
			{
				item.Checked = (item.Text == this.camComp.Perspective.ToString());
			}
		}
		private void perspectiveDropDown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			PerspectiveMode perspective;
			if (Enum.TryParse(e.ClickedItem.Text, out perspective))
			{
				this.camComp.Perspective = perspective;
				this.OnPerspectiveChanged();
			}
		}

		HelpInfo IHelpProvider.ProvideHoverHelp(Point localPos, ref bool captured)
		{
			Point globalPos = this.PointToScreen(localPos);
			ToolStripItem item;
			object itemTag;
			
			// If the state combobox is dropped down, check its items
			if (this.stateSelector.DroppedDown)
			{
				captured = true;
				StateEntry hoveredState = this.stateSelector.SelectedItem as StateEntry;
				if (hoveredState != null)
				{
					HelpInfo info = HelpInfo.FromMember(hoveredState.StateType);
					info.Topic = hoveredState.StateName;
					return info;
				}
				else
					return null;
			}

			// Retrieve the currently hovered / active item from all child toolstrips
			item = this.GetHoveredToolStripItem(globalPos, out captured);
			itemTag = (item != null) ? item.Tag : null;
			if (item != null || captured)
			{
				if (itemTag is LayerEntry)
				{
					LayerEntry entry = itemTag as LayerEntry;
					return HelpInfo.FromText(entry.LayerName, entry.LayerDesc, entry.LayerType.GetMemberId());
				}
				else if (itemTag is ObjectVisibilityEntry)
				{
					ObjectVisibilityEntry entry = itemTag as ObjectVisibilityEntry;
					return HelpInfo.FromMember(entry.ComponentType);
				}
				else
				{
					return itemTag as HelpInfo;
				}
			}

			// Hovering the viewport
			Point glLocalPos = this.RenderableControl.PointToClient(globalPos);
			if (this.RenderableControl.ClientRectangle.Contains(glLocalPos))
			{
				if (this.activeState != null)
					return this.activeState.ProvideHoverHelp(glLocalPos, ref captured);
			}

			return null;
		}

		int IMouseInputSource.X
		{
			get { return this.inputMouseX; }
			set
			{
				if (this.activeState.EngineUserInput && this.RenderableControl.Focused && this.inputMouseCapture)
				{
					Cursor.Position = this.RenderableControl.PointToScreen(new Point(value, this.RenderableControl.PointToClient(Cursor.Position).Y));
				}
			}
		}
		int IMouseInputSource.Y
		{
			get { return this.inputMouseY; }
			set
			{
				if (this.activeState.EngineUserInput && this.RenderableControl.Focused && this.inputMouseCapture)
				{
					Cursor.Position = this.RenderableControl.PointToScreen(new Point(this.RenderableControl.PointToClient(Cursor.Position).X, value));
				}
			}
		}
		float IMouseInputSource.Wheel
		{
			get { return this.inputMouseWheel; }
		}
		bool IMouseInputSource.this[MouseButton btn]
		{
			get { return (this.inputMouseButtons & (1 << (int)btn)) != 0; }
		}

		string IKeyboardInputSource.CharInput
		{
			get { return this.inputCharInput ?? string.Empty; }
		}
		bool IKeyboardInputSource.this[Key key]
		{
			get { return this.inputKeyPressed[(int)key]; }
		}
		
		string IUserInputSource.Description
		{
			get { return "Camera View"; }
		}
		bool IUserInputSource.IsAvailable
		{
			// These should be separated.. but C# doesn't allow to implement IsAvailable for both sources separately.
			get { return this.inputKeyFocus && this.inputMouseInView; }
		}
		void IUserInputSource.UpdateState()
		{
			if (this.inputLastUpdateFrame == Time.FrameCount) return;
			this.inputLastUpdateFrame = Time.FrameCount;

			this.inputCharInput = this.inputCharInputBuffer.ToString();
			this.inputCharInputBuffer.Clear();
		}
	}
}
