using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using BitArray = System.Collections.BitArray;

using WeifenLuo.WinFormsUI.Docking;
using AdamsLair.WinForms.ColorControls;

using Duality;
using Duality.Components;
using Duality.Drawing;
using Duality.Resources;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.CamView.CamViewStates;
using Duality.Editor.Plugins.CamView.CamViewLayers;

using OpenTK;
using Key = OpenTK.Input.Key;
using MouseButton = OpenTK.Input.MouseButton;
using MouseButtonEventArgs = OpenTK.Input.MouseButtonEventArgs;
using KeyboardKeyEventArgs = OpenTK.Input.KeyboardKeyEventArgs;
using MouseMoveEventArgs = OpenTK.Input.MouseMoveEventArgs;
using MouseWheelEventArgs = OpenTK.Input.MouseWheelEventArgs;

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

		public const float DefaultDisplayBoundRadius = 25.0f;

		private	int					runtimeId		= 0;
		private	GLControl			glControl		= null;
		private	GameObject			camObj			= null;
		private	Camera				camComp			= null;
		private	CamViewState		activeState		= null;
		private	List<CamViewLayer>	activeLayers	= null;
		private	List<Type>			lockedLayers	= new List<Type>();
		private	ColorPickerDialog	bgColorDialog	= new ColorPickerDialog { BackColor = Color.FromArgb(212, 212, 212) };
		private	GameObject			nativeCamObj	= null;
		private	string				loadTempState	= null;
		private	string				loadTempPerspective	= null;
		private	InputEventMessageRedirector	waitForInputFilter	= null;
		private	ToolStripItem		activeToolItem	= null;

		private	Dictionary<Type,CamViewLayer>	availLayers	= new Dictionary<Type,CamViewLayer>();
		private	Dictionary<Type,CamViewState>	availStates	= new Dictionary<Type,CamViewState>();

		private	bool	inputMouseCapture	= false;
		private	int		inputMouseX			= 0;
		private	int		inputMouseY			= 0;
		private	float	inputMouseWheel		= 0.0f;
		private	int		inputMouseButtons	= 0;
		private	bool	inputMouseInView	= false;

		private	bool		inputKeyRepeat		= false;
		private	bool		inputKeyFocus		= false;
		private	int			inputKeyRepeatCount	= 0;
		private	BitArray	inputKeyPressed		= new BitArray((int)Key.LastKey + 1, false);

		public event EventHandler PerspectiveChanged	= null;
		public event EventHandler<CameraChangedEventArgs> CurrentCameraChanged	= null;

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
		public GLControl LocalGLControl
		{
			get { return this.glControl; }
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

		public CamView(int runtimeId, string initStateTypeName = null)
		{
			this.InitializeComponent();
			this.loadTempState = initStateTypeName;
			this.bgColorDialog.OldColor = Color.FromArgb(64, 64, 64);
			this.bgColorDialog.SelectedColor = this.bgColorDialog.OldColor;
			this.bgColorDialog.AlphaEnabled = false;
			this.Text = Properties.CamViewRes.MenuItemName_CamView + " #" + runtimeId;
			this.runtimeId = runtimeId;
			this.toolbarCamera.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			
			var camViewStateTypeQuery = 
				from t in DualityEditorApp.GetAvailDualityEditorTypes(typeof(CamViewState))
				where !t.IsAbstract
				select t;
			foreach (Type t in camViewStateTypeQuery)
			{
				CamViewState state = t.CreateInstanceOf() as CamViewState;
				state.View = this;
				this.availStates.Add(t, state);
			}

			var camViewLayerTypeQuery = 
				from t in DualityEditorApp.GetAvailDualityEditorTypes(typeof(CamViewLayer))
				where !t.IsAbstract
				select t;
			foreach (Type t in camViewLayerTypeQuery)
			{
				CamViewLayer layer = t.CreateInstanceOf() as CamViewLayer;
				layer.View = this;
				this.availLayers.Add(t, layer);
			}
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			this.InitGLControl();
			this.InitNativeCamera();
			if (this.camSelector.Items.Count == 0)				this.InitCameraSelector();
			if (this.stateSelector.Items.Count == 0)			this.InitStateSelector();
			if (this.layerSelector.DropDownItems.Count == 0)	this.InitLayerSelector();
			this.SetCurrentCamera(null);

			// Initialize PerspectiveMode Selector
			foreach (string perspectiveName in Enum.GetNames(typeof(PerspectiveMode)))
			{
				ToolStripMenuItem perspectiveItem = new ToolStripMenuItem(perspectiveName);
				var perspectiveField = typeof(PerspectiveMode).GetField(perspectiveName, ReflectionHelper.BindAll);
				perspectiveItem.Tag = HelpInfo.FromMember(perspectiveField);
				this.perspectiveDropDown.DropDownItems.Add(perspectiveItem);
			}

			// Initialize from loaded state id, if not done yet manually
			if (this.activeState == null)
			{
				Type stateType = ReflectionHelper.ResolveType(this.loadTempState, false) ?? typeof(SceneEditorCamViewState);
				this.SetCurrentState(stateType);
				this.loadTempState = null;
			}
			else
			{
				// If we set the state explicitly before, we'll still need to fire its enter event. See SetCurrentState.
				this.activeState.OnEnterState();
			}

			// Register DualityApp updater for camera steering behaviour
			FileEventManager.ResourceModified		+= this.FileEventManager_ResourceModified;
			DualityEditorApp.HighlightObject		+= this.DualityEditorApp_HighlightObject;
			DualityEditorApp.ObjectPropertyChanged	+= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine			+= this.DualityEditorApp_UpdatingEngine;
			Scene.Entered							+= this.Scene_Entered;
			Scene.Leaving							+= this.Scene_Leaving;
			Scene.GameObjectRemoved					+= this.Scene_GameObjectUnregistered;
			Scene.ComponentRemoving					+= this.Scene_ComponentRemoving;

			// Update Camera values according to GUI (which carries loaded or default settings)
			this.focusDist_ValueChanged(this.focusDist, null);
			this.bgColorDialog_ValueChanged(this.bgColorDialog, null);
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
		}
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if (this.nativeCamObj != null) this.nativeCamObj.Dispose();

			FileEventManager.ResourceModified		-= this.FileEventManager_ResourceModified;
			DualityEditorApp.HighlightObject		-= this.DualityEditorApp_HighlightObject;
			DualityEditorApp.ObjectPropertyChanged	-= this.DualityEditorApp_ObjectPropertyChanged;
			DualityEditorApp.UpdatingEngine			-= this.DualityEditorApp_UpdatingEngine;
			Scene.Entered							-= this.Scene_Entered;
			Scene.Leaving							-= this.Scene_Leaving;
			Scene.GameObjectRemoved					-= this.Scene_GameObjectUnregistered;
			Scene.ComponentRemoving					-= this.Scene_ComponentRemoving;

			this.SetCurrentState((CamViewState)null);
		}
		
		private void InitGLControl()
		{
			this.SuspendLayout();

			// Get rid of a possibly existing old glControl
			if (this.glControl != null)
			{
				this.glControl.MouseEnter -= this.glControl_MouseEnter;
				this.glControl.MouseLeave -= this.glControl_MouseLeave;
				this.glControl.MouseDown -= this.glControl_MouseDown;
				this.glControl.MouseUp -= this.glControl_MouseUp;
				this.glControl.MouseWheel -= this.glControl_MouseWheel;
				this.glControl.MouseMove -= this.glControl_MouseMove;
				this.glControl.GotFocus -= this.glControl_GotFocus;
				this.glControl.LostFocus -= this.glControl_LostFocus;
				this.glControl.PreviewKeyDown -= glControl_PreviewKeyDown;
				this.glControl.KeyDown -= this.glControl_KeyDown;
				this.glControl.KeyUp -= this.glControl_KeyUp;
				this.glControl.Resize -= this.glControl_Resize;

				this.glControl.Dispose();
				this.Controls.Remove(this.glControl);
			}

			// Create a new glControl
			this.glControl = DualityEditorApp.GLCreateControl();
			this.glControl.BackColor = Color.Black;
			this.glControl.Dock = DockStyle.Fill;
			this.glControl.Name = "glControl";
			this.glControl.VSync = false;
			this.glControl.AllowDrop = true;
			this.glControl.MouseEnter += this.glControl_MouseEnter;
			this.glControl.MouseLeave += this.glControl_MouseLeave;
			this.glControl.MouseDown += this.glControl_MouseDown;
			this.glControl.MouseUp += this.glControl_MouseUp;
			this.glControl.MouseWheel += this.glControl_MouseWheel;
			this.glControl.MouseMove += this.glControl_MouseMove;
			this.glControl.GotFocus += this.glControl_GotFocus;
			this.glControl.LostFocus += this.glControl_LostFocus;
			this.glControl.PreviewKeyDown += glControl_PreviewKeyDown;
			this.glControl.KeyDown += this.glControl_KeyDown;
			this.glControl.KeyUp += this.glControl_KeyUp;
			this.glControl.Resize += this.glControl_Resize;
			this.Controls.Add(this.glControl);
			this.Controls.SetChildIndex(this.glControl, 0);

			this.ResumeLayout(true);
		}
		private void InitStateSelector()
		{
			this.stateSelector.Items.Clear();

			foreach (var pair in this.availStates)
				this.stateSelector.Items.Add(new StateEntry(pair.Key, pair.Value));
		}
		private void InitLayerSelector()
		{
			this.layerSelector.DropDown.Closing -= this.layerSelector_Closing;
			this.layerSelector.DropDownItems.Clear();

			IEnumerable<Type> camViewStateTypeQuery = 
				from t in DualityEditorApp.GetAvailDualityEditorTypes(typeof(CamViewLayer))
				where !t.IsAbstract
				select t;

			foreach (var pair in this.availLayers)
			{
				LayerEntry layerEntry = new LayerEntry(pair.Key, pair.Value);
				ToolStripMenuItem layerItem = new ToolStripMenuItem(layerEntry.LayerName);
				layerItem.Tag = layerEntry;
				layerItem.ToolTipText = layerEntry.LayerDesc;
				layerItem.Checked = this.activeLayers != null && this.activeLayers.Any(l => l.GetType() == layerEntry.LayerType);
				layerItem.Enabled = !this.lockedLayers.Contains(layerEntry.LayerType);
				this.layerSelector.DropDownItems.Add(layerItem);
			}
			this.layerSelector.DropDown.Closing += this.layerSelector_Closing;
		}
		private void InitCameraSelector()
		{
			this.camSelector.Items.Clear();
			this.camSelector.Items.Add(new CamEntry(this.nativeCamObj.Camera));

			foreach (Camera c in Scene.Current.AllObjects.GetComponents<Camera>().OrderBy(c => c.GameObj.FullName))
				this.camSelector.Items.Add(new CamEntry(c));
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
			if (c == null) c = this.nativeCamObj.Camera;
			if (c == this.camComp) return;

			Camera prev = this.camComp;

			if (c.GameObj == this.nativeCamObj)
			{
				this.camObj = this.nativeCamObj;
				this.camComp = this.camObj.Camera;
				this.camSelector.SelectedIndex = 0;
			}
			else
			{
				this.camObj = c.GameObj;
				this.camComp = c;
				this.camSelector.SelectedIndex = this.GetCameraSelectorIndex(c);
			}

			this.OnCurrentCameraChanged(prev, this.camComp);
			this.glControl.Invalidate();
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

			// No glControl yet? We're not initialized properly and this is the initial state. Enter the state later.
			if (this.glControl != null)
			{
				if (this.activeState != null) this.activeState.OnEnterState();
				this.glControl.Invalidate();
			}
		}

		public void SetActiveLayers(params Type[] layerTypes)
		{
			this.SetActiveLayers((IEnumerable<Type>)layerTypes);
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
			if (this.glControl != null)
			{
				layer.OnActivateLayer();
				this.glControl.Invalidate();
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
			this.glControl.Invalidate();
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

		internal void SaveUserData(XElement node)
		{
			node.SetAttributeValue("perspective", this.nativeCamObj.Camera.Perspective.ToString());
			node.SetAttributeValue("focusDist", this.nativeCamObj.Camera.FocusDist.ToString(CultureInfo.InvariantCulture));
			node.SetAttributeValue("bgColorArgb", this.nativeCamObj.Camera.ClearColor.ToIntArgb().ToString(CultureInfo.InvariantCulture));

			if (this.activeState != null) 
				node.SetAttributeValue("activeState", this.activeState.GetType().GetTypeId());

			XElement stateListNode = new XElement("states");
			foreach (var pair in this.availStates)
			{
				XElement stateNode = new XElement(pair.Key.GetTypeId());
				pair.Value.SaveUserData(stateNode);
				stateListNode.Add(stateNode);
			}
			node.Add(stateListNode);

			XElement layerListNode = new XElement("layers");
			foreach (var pair in this.availLayers)
			{
				XElement layerNode = new XElement(pair.Key.GetTypeId());
				pair.Value.SaveUserData(layerNode);
				layerListNode.Add(layerNode);
			}
			node.Add(layerListNode);
		}
		internal void LoadUserData(XElement node)
		{
			decimal tryParseDecimal;
			int tryParseInt;

			if (decimal.TryParse(node.GetAttributeValue("focusDist"), out tryParseDecimal))
				this.focusDist.Value = Math.Abs(tryParseDecimal);
			if (int.TryParse(node.GetAttributeValue("bgColorArgb"), out tryParseInt))
			{
				this.bgColorDialog.OldColor = Color.FromArgb(tryParseInt);
				this.bgColorDialog.SelectedColor = this.bgColorDialog.OldColor;
			}

			this.loadTempPerspective = node.GetAttributeValue("perspective");
			this.loadTempState = node.GetAttributeValue("activeState");

			XElement stateListNode = node.Element("states");
			if (stateListNode != null)
			{
				foreach (var pair in this.availStates)
				{
					XElement stateNode = stateListNode.Element(pair.Key.GetTypeId());
					if (stateNode == null) continue;
					pair.Value.LoadUserData(stateNode);
				}
			}

			XElement layerListNode = node.Element("layers");
			if (layerListNode != null)
			{
				foreach (var pair in this.availLayers)
				{
					XElement layerNode = layerListNode.Element(pair.Key.GetTypeId());
					if (layerNode == null) continue;
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
			this.perspectiveDropDown.Enabled = value;
			this.focusDist.Enabled = value;
			this.camSelector.Enabled = value;
			this.showBgColorDialog.Enabled = value;
			this.layerSelector.Enabled = value;
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
			this.glControl.Invalidate();
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
			this.glControl.Invalidate();

			if (this.PerspectiveChanged != null)
				this.PerspectiveChanged(this, EventArgs.Empty);
		}
		private void OnCurrentCameraChanged(Camera prev, Camera next)
		{
			if (this.CurrentCameraChanged != null)
				this.CurrentCameraChanged(this, new CameraChangedEventArgs(prev, next));
		}
		
		
		private void InstallFocusHook()
		{
			if (this.glControl.Focused) return;

			// Hook global message filter
			if (this.waitForInputFilter == null)
			{
				this.waitForInputFilter = new InputEventMessageRedirector(this.glControl, this.FocusHookFilter,
					InputEventMessageRedirector.MessageType.MouseWheel);
				Application.AddMessageFilter(this.waitForInputFilter);
			}
		}
		private void RemoveFocusHook()
		{
			// Remove global message filter
			if (this.waitForInputFilter != null)
			{
				Application.RemoveMessageFilter(this.waitForInputFilter);
				this.waitForInputFilter = null;
			}
		}
		private bool FocusHookFilter(InputEventMessageRedirector.MessageType type, EventArgs e)
		{
			// Don't capture stuff in sandbox mode. Input is likely to be needed in the current Game View
			if (Sandbox.State == SandboxState.Playing)
				return false;
			
			return true;
		}
		private void glControl_MouseLeave(object sender, EventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				this.inputMouseInView = false;
			}

			this.RemoveFocusHook();

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.glControl.Invalidate();
		}
		private void glControl_MouseEnter(object sender, EventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				this.inputMouseInView = true;
			}

			this.InstallFocusHook();

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.glControl.Invalidate();
		}
		private void glControl_MouseDown(object sender, MouseEventArgs e)
		{
			this.inputMouseCapture = true;
			if (this.activeState.EngineUserInput)
			{
				MouseButton inputButton = e.Button.ToOpenTKSingle();
				this.inputMouseButtons |= e.Button.ToOpenTK();
			}
		}
		private void glControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				MouseButton inputButton = e.Button.ToOpenTKSingle();
				this.inputMouseButtons &= ~e.Button.ToOpenTK();
			}
		}
		private void glControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!this.glControl.Focused) this.glControl.Focus();

			if (this.activeState.EngineUserInput)
			{
				this.inputMouseWheel += e.Delta / 120.0f;
			}
		}
		private void glControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				int lastX = this.inputMouseX;
				int lastY = this.inputMouseY;
				this.inputMouseX = e.X;
				this.inputMouseY = e.Y;
			}

			if (this.activeLayers.Any(l => l.MouseTracking))
				this.glControl.Invalidate();
		}
		private void glControl_GotFocus(object sender, EventArgs e)
		{
			this.RemoveFocusHook();
			this.inputMouseCapture = true;

			if (this.activeState != null)
			{
				if (this.activeState.EngineUserInput)
				{
					this.inputKeyFocus = true;
				}

				if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
					this.activeState.SelectObjects(this.activeState.SelectedObjects);
			}
		}
		private void glControl_LostFocus(object sender, EventArgs e)
		{
			if (this.activeState != null && this.activeState.EngineUserInput)
			{
				this.inputKeyFocus = false;
			}
		}
		private void glControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
		private void glControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (!this.glControl.Focused) this.glControl.Focus();

			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Alt)
			{
				this.inputMouseCapture = false;
			}

			if (this.activeState.EngineUserInput)
			{
				Key inputKey = e.KeyCode.ToOpenTKSingle();
				bool wasPressed = this.inputKeyPressed[(int)inputKey];
				this.inputKeyPressed = this.inputKeyPressed.Or(e.KeyCode.ToOpenTK());
				this.inputKeyRepeatCount++;
			}
		}
		private void glControl_KeyUp(object sender, KeyEventArgs e)
		{
			if (this.activeState.EngineUserInput)
			{
				Key inputKey = e.KeyCode.ToOpenTKSingle();
				this.inputKeyPressed = this.inputKeyPressed.And(e.KeyCode.ToOpenTK().Not());
			}
		}
		private void glControl_Resize(object sender, EventArgs e)
		{
			this.glControl.Invalidate();
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
				this.camComp.ClearColor = new ColorRgba(
					this.bgColorDialog.SelectedColor.R,
					this.bgColorDialog.SelectedColor.G,
					this.bgColorDialog.SelectedColor.B,
					0);
			}
			this.glControl.Invalidate();
		}
		private void buttonResetZoom_Click(object sender, EventArgs e)
		{
			this.camObj.Transform.Pos = new Vector3(this.camObj.Transform.Pos.Xy, -MathF.Abs(this.camComp.FocusDist));
			this.glControl.Invalidate();
		}
		
		private void FileEventManager_ResourceModified(object sender, ResourceEventArgs e)
		{
			if (!e.IsResource) return;
			this.glControl.Invalidate();
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
			this.glControl.Invalidate();
		}
		private void DualityEditorApp_UpdatingEngine(object sender, EventArgs e)
		{
			if (this.camObj != null && this.camObj != this.nativeCamObj)
				DualityEditorApp.UpdateGameObject(this.camObj);
		}
		
		private void Scene_Entered(object sender, EventArgs e)
		{
			if (!Sandbox.IsActive && !Sandbox.IsChangingState) this.ResetCamera();
			this.glControl.Invalidate();
		}
		private void Scene_Leaving(object sender, EventArgs e)
		{
			if (this.camObj != this.nativeCamObj) this.SetCurrentCamera(null);
			this.glControl.Invalidate();
		}
		private void Scene_ComponentRemoving(object sender, ComponentEventArgs e)
		{
			if (this.camComp == e.Component) this.SetCurrentCamera(null);
		}
		private void Scene_GameObjectUnregistered(object sender, GameObjectEventArgs e)
		{
			if (this.camObj == e.Object) this.SetCurrentCamera(null);
		}
		
		private void camSelector_DropDown(object sender, EventArgs e)
		{
			this.activeToolItem = this.camSelector;
			this.InitCameraSelector();
		}
		private void camSelector_DropDownClosed(object sender, EventArgs e)
		{
			if (this.activeToolItem == this.camSelector)
				this.activeToolItem = null;
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
			this.activeToolItem = this.stateSelector;
			this.InitStateSelector();
		}
		private void stateSelector_DropDownClosed(object sender, EventArgs e)
		{
			if (this.activeToolItem == this.stateSelector)
				this.activeToolItem = null;
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
			this.activeToolItem = this.layerSelector;
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
		private void layerSelector_DropDownClosed(object sender, EventArgs e)
		{
			if (this.activeToolItem == this.layerSelector)
				this.activeToolItem = null;
		}
		private void perspectiveDropDown_DropDownOpening(object sender, EventArgs e)
		{
			this.activeToolItem = this.perspectiveDropDown;
			foreach (var item in this.perspectiveDropDown.DropDownItems.OfType<ToolStripMenuItem>())
			{
				item.Checked = (item.Text == this.camComp.Perspective.ToString());
			}
		}
		private void perspectiveDropDown_DropDownClosed(object sender, EventArgs e)
		{
			if (this.activeToolItem == this.perspectiveDropDown)
				this.activeToolItem = null;
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
			HelpInfo result = null;
			Point globalPos = this.PointToScreen(localPos);
			
			ToolStripItem	item	= this.toolbarCamera.GetItemAtDeep(globalPos);
			object			itemTag	= item != null ? item.Tag : null;

			// Hovering a menu
			if (item != null || this.activeToolItem != null)
			{
				result = itemTag as HelpInfo;
				captured = (this.activeToolItem != null);
			}
			// Hovering the viewport
			else
			{
				Point glLocalPos = this.glControl.PointToClient(globalPos);
				if (this.glControl.ClientRectangle.Contains(glLocalPos))
				{
					if (this.activeState != null)
						result = this.activeState.ProvideHoverHelp(glLocalPos, ref captured);
				}
			}

			return result;
		}

		int IMouseInputSource.X
		{
			get { return this.inputMouseX; }
			set
			{
				if (this.activeState.EngineUserInput && this.glControl.Focused && this.inputMouseCapture)
				{
					Cursor.Position = this.glControl.PointToScreen(new Point(value, this.glControl.PointToClient(Cursor.Position).Y));
				}
			}
		}
		int IMouseInputSource.Y
		{
			get { return this.inputMouseY; }
			set
			{
				if (this.activeState.EngineUserInput && this.glControl.Focused && this.inputMouseCapture)
				{
					Cursor.Position = this.glControl.PointToScreen(new Point(this.glControl.PointToClient(Cursor.Position).X, value));
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

		bool IKeyboardInputSource.KeyRepeat
		{
			get { return this.inputKeyRepeat; }
			set { this.inputKeyRepeat = value; }
		}
		int IKeyboardInputSource.KeyRepeatCounter
		{
			get { return this.inputKeyRepeatCount; }
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
		void IUserInputSource.UpdateState() {}
	}
}
