using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;

using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public abstract class CamViewState : CamViewClient, IHelpProvider
	{
		[Flags]
		public enum UserGuideType
		{
			None		= 0x0,

			Position	= 0x1,
			Scale		= 0x2,

			All			= Position | Scale
		}
		public enum CameraAction
		{
			None,
			Move,
			Rotate,

			// Alternate movement (Spacebar pressed)
			DragScene,
			RotateScene
		}


		private static readonly ContentRef<Duality.Resources.Font> OverlayFont = Duality.Resources.Font.GenericMonospace8;

		private Vector3       camVel                 = Vector3.Zero;
		private float         camAngleVel            = 0.0f;
		private Point         camActionBeginLoc      = Point.Empty;
		private Vector3       camActionBeginLocSpace = Vector3.Zero;
		private CameraAction  camAction              = CameraAction.None;
		private bool          camActionAllowed       = true;
		private bool          camTransformChanged    = false;
		private bool          camBeginDragScene      = false;
		private Camera.Pass   camPassBg              = null;
		private Camera.Pass   camPassEdWorld         = null;
		private Camera.Pass   camPassEdScreen        = null;
		private bool          engineUserInput        = false;
		private UserGuideType snapToUserGuides       = UserGuideType.All;
		private bool          mouseover              = false;
		private CameraAction  drawCamGizmoState      = CameraAction.None;
		private FormattedText statusText             = new FormattedText();
		private FormattedText actionText             = new FormattedText();
		private List<Type>    lastActiveLayers       = new List<Type>();
		private List<Type>    lastObjVisibility      = new List<Type>();


		public abstract string StateName { get; }

		protected virtual bool IsActionInProgress
		{
			get { return false; }
		}
		protected virtual bool HasCameraFocusPosition
		{
			get { return false; }
		}
		protected virtual Vector3 CameraFocusPosition
		{
			get { return Vector3.Zero; }
		}

		public bool IsActive
		{
			get { return this.View != null && this.View.ActiveState == this; }
		}
		public bool EngineUserInput
		{
			get { return this.engineUserInput; }
			protected set { this.engineUserInput = value; }
		}
		public bool CameraActionAllowed
		{
			get { return this.camActionAllowed; }
			protected set
			{ 
				this.camActionAllowed = value;
				if (!this.camActionAllowed && this.camAction != CameraAction.None)
				{
					this.camAction = CameraAction.None;
					this.Invalidate();
				}
			}
		}
		public bool Mouseover
		{
			get { return this.mouseover; }
		}
		public bool CamActionRequiresCursor
		{
			get { return this.camBeginDragScene; }
		}
		public CameraAction CamAction
		{
			get { return this.camAction; }
		}
		public UserGuideType SnapToUserGuides
		{
			get { return this.snapToUserGuides; }
			protected set { this.snapToUserGuides = value; }
		}
		public string StatusText
		{
			get { return this.statusText.SourceText; }
		}
		public string ActionText
		{
			get { return this.actionText.SourceText; }
		}


		internal protected virtual void OnEnterState()
		{
			this.RestoreActiveLayers();
			this.RestoreObjectVisibility();

			// Create re-usable render passes for editor gizmos
			this.camPassBg = new Camera.Pass();
			this.camPassBg.MatrixMode = RenderMatrix.OrthoScreen;
			this.camPassBg.ClearFlags = ClearFlag.None;
			this.camPassBg.VisibilityMask = VisibilityFlag.ScreenOverlay;
			this.camPassEdWorld = new Camera.Pass();
			this.camPassEdWorld.ClearFlags = ClearFlag.None;
			this.camPassEdWorld.VisibilityMask = VisibilityFlag.None;
			this.camPassEdScreen = new Camera.Pass();
			this.camPassEdScreen.MatrixMode = RenderMatrix.OrthoScreen;
			this.camPassEdScreen.ClearFlags = ClearFlag.None;
			this.camPassEdScreen.VisibilityMask = VisibilityFlag.ScreenOverlay;

			this.camPassBg.CollectDrawcalls			+= this.camPassBg_CollectDrawcalls;
			this.camPassEdWorld.CollectDrawcalls	+= this.camPassEdWorld_CollectDrawcalls;
			this.camPassEdScreen.CollectDrawcalls	+= this.camPassEdScreen_CollectDrawcalls;

			Control control = this.RenderableSite.Control;
			control.Paint		+= this.RenderableControl_Paint;
			control.MouseDown	+= this.RenderableControl_MouseDown;
			control.MouseUp		+= this.RenderableControl_MouseUp;
			control.MouseMove	+= this.RenderableControl_MouseMove;
			control.MouseWheel	+= this.RenderableControl_MouseWheel;
			control.MouseLeave	+= this.RenderableControl_MouseLeave;
			control.KeyDown		+= this.RenderableControl_KeyDown;
			control.KeyUp		+= this.RenderableControl_KeyUp;
			control.GotFocus	+= this.RenderableControl_GotFocus;
			control.LostFocus	+= this.RenderableControl_LostFocus;
			control.DragDrop	+= this.RenderableControl_DragDrop;
			control.DragEnter	+= this.RenderableControl_DragEnter;
			control.DragLeave	+= this.RenderableControl_DragLeave;
			control.DragOver	+= this.RenderableControl_DragOver;
			control.Resize		+= this.RenderableControl_Resize;
			this.View.PerspectiveChanged	+= this.View_FocusDistChanged;
			this.View.CurrentCameraChanged	+= this.View_CurrentCameraChanged;
			DualityEditorApp.UpdatingEngine += this.DualityEditorApp_UpdatingEngine;
			DualityEditorApp.ObjectPropertyChanged += this.DualityEditorApp_ObjectPropertyChanged;

			Scene.Leaving += this.Scene_Changed;
			Scene.Entered += this.Scene_Changed;
			Scene.GameObjectParentChanged += this.Scene_Changed;
			Scene.GameObjectAdded += this.Scene_Changed;
			Scene.GameObjectRemoved += this.Scene_Changed;
			Scene.ComponentAdded += this.Scene_Changed;
			Scene.ComponentRemoving += this.Scene_Changed;

			if (Scene.Current != null) this.Scene_Changed(this, EventArgs.Empty);
			
			// Initial Camera update
			this.OnCurrentCameraChanged(new CamView.CameraChangedEventArgs(null, this.CameraComponent));
			this.UpdateFormattedTextRenderers();
		}
		internal protected virtual void OnLeaveState() 
		{
			this.Cursor = CursorHelper.Arrow;

			Control control = this.RenderableSite.Control;
			control.Paint		-= this.RenderableControl_Paint;
			control.MouseDown	-= this.RenderableControl_MouseDown;
			control.MouseUp		-= this.RenderableControl_MouseUp;
			control.MouseMove	-= this.RenderableControl_MouseMove;
			control.MouseWheel	-= this.RenderableControl_MouseWheel;
			control.MouseLeave	-= this.RenderableControl_MouseLeave;
			control.KeyDown		-= this.RenderableControl_KeyDown;
			control.KeyUp		-= this.RenderableControl_KeyUp;
			control.GotFocus	-= this.RenderableControl_GotFocus;
			control.LostFocus	-= this.RenderableControl_LostFocus;
			control.DragDrop	-= this.RenderableControl_DragDrop;
			control.DragEnter	-= this.RenderableControl_DragEnter;
			control.DragLeave	-= this.RenderableControl_DragLeave;
			control.DragOver	-= this.RenderableControl_DragOver;
			control.Resize		-= this.RenderableControl_Resize;
			this.View.PerspectiveChanged			-= this.View_FocusDistChanged;
			this.View.CurrentCameraChanged			-= this.View_CurrentCameraChanged;
			DualityEditorApp.UpdatingEngine			-= this.DualityEditorApp_UpdatingEngine;
			DualityEditorApp.ObjectPropertyChanged	-= this.DualityEditorApp_ObjectPropertyChanged;
			
			Scene.Leaving -= this.Scene_Changed;
			Scene.Entered -= this.Scene_Changed;
			Scene.GameObjectParentChanged -= this.Scene_Changed;
			Scene.GameObjectAdded -= this.Scene_Changed;
			Scene.GameObjectRemoved -= this.Scene_Changed;
			Scene.ComponentAdded -= this.Scene_Changed;
			Scene.ComponentRemoving -= this.Scene_Changed;

			this.SaveActiveLayers();
			this.SaveObjectVisibility();

			// Final Camera cleanup
			this.OnCurrentCameraChanged(new CamView.CameraChangedEventArgs(this.CameraComponent, null));
		}
		
		internal protected virtual void SaveUserData(XElement node)
		{
			if (this.IsActive) this.SaveActiveLayers();
			if (this.IsActive) this.SaveObjectVisibility();

			XElement activeLayersNode = new XElement("ActiveLayers");
			foreach (Type t in this.lastActiveLayers)
			{
				XElement typeEntry = new XElement("Item", t.GetTypeId());
				activeLayersNode.Add(typeEntry);
			}
			if (!activeLayersNode.IsEmpty)
				node.Add(activeLayersNode);

			XElement objVisibilityNode = new XElement("ObjectVisibility");
			foreach (Type t in this.lastObjVisibility)
			{
				XElement typeEntry = new XElement("Item", t.GetTypeId());
				objVisibilityNode.Add(typeEntry);
			}
			if (!objVisibilityNode.IsEmpty)
				node.Add(objVisibilityNode);
		}
		internal protected virtual void LoadUserData(XElement node)
		{
			XElement activeLayersNode = node.Element("ActiveLayers");
			if (activeLayersNode != null)
			{
				this.lastActiveLayers.Clear();
				foreach (XElement layerNode in activeLayersNode.Elements("Item"))
				{
					Type layerType = ReflectionHelper.ResolveType(layerNode.Value);
					if (layerType != null) this.lastActiveLayers.Add(layerType);
				}
			}
			
			XElement objVisibilityNode = node.Element("ObjectVisibility");
			if (objVisibilityNode != null)
			{
				this.lastObjVisibility.Clear();
				foreach (XElement typeNode in objVisibilityNode.Elements("Item"))
				{
					Type type = ReflectionHelper.ResolveType(typeNode.Value);
					if (type != null) this.lastObjVisibility.Add(type);
				}
			}

			if (this.IsActive) this.RestoreActiveLayers();
			if (this.IsActive) this.RestoreObjectVisibility();
		}

		protected virtual void OnCollectStateDrawcalls(Canvas canvas)
		{
			// Collect the views layer drawcalls
			this.CollectLayerDrawcalls(canvas);
		}
		protected virtual void OnCollectStateOverlayDrawcalls(Canvas canvas)
		{
			// Gather general data
			Point cursorPos = this.PointToClient(Cursor.Position);

			// Update action text from hovered / selection / action object
			Vector2 actionTextPos = new Vector2(cursorPos.X + 30, cursorPos.Y + 10);
			this.actionText.SourceText = this.UpdateActionText(ref actionTextPos);

			// Collect the views overlay layer drawcalls
			this.CollectLayerOverlayDrawcalls(canvas);

			// Collect the states overlay drawcalls
			canvas.PushState();
			{
				// Draw camera movement indicators
				if (this.camAction != CameraAction.None)
				{
					canvas.PushState();
					canvas.State.ColorTint = ColorRgba.White.WithAlpha(0.5f);
					if (this.camAction == CameraAction.DragScene)
					{
						// Don't draw anything.
					}
					else if (this.camAction == CameraAction.RotateScene)
					{
						canvas.FillCircle(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, 3);
						canvas.DrawLine(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, cursorPos.X, this.camActionBeginLoc.Y);
					}
					else if (this.camAction == CameraAction.Move)
					{
						canvas.FillCircle(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, 3);
						canvas.DrawLine(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, cursorPos.X, cursorPos.Y);
					}
					else if (this.camAction == CameraAction.Rotate)
					{
						canvas.FillCircle(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, 3);
						canvas.DrawLine(this.camActionBeginLoc.X, this.camActionBeginLoc.Y, cursorPos.X, this.camActionBeginLoc.Y);
					}
					canvas.PopState();
				}
				
				// Normalize action text position
				if (this.actionText.Fonts != null && this.actionText.Fonts.Any(r => r.IsAvailable && r.Res.IsPixelGridAligned))
				{
					actionTextPos.X = MathF.Round(actionTextPos.X);
					actionTextPos.Y = MathF.Round(actionTextPos.Y);
				}

				// Draw current action text
				if (!this.actionText.IsEmpty)
				{
					canvas.DrawText(this.actionText, actionTextPos.X, actionTextPos.Y, drawBackground: true);
				}

				// Update / Draw current status text
				{
					this.statusText.SourceText = this.UpdateStatusText();
					if (!this.statusText.IsEmpty)
					{
						Vector2 statusTextSize = this.statusText.Size;
						canvas.DrawText(this.statusText, 10, this.ClientSize.Height - statusTextSize.Y - 10, drawBackground: true);
					}
				}
			}
			canvas.PopState();

			// Draw a focus indicator at the view border
			canvas.PushState();
			{
				ColorRgba focusColor = ColorRgba.Lerp(this.FgColor, this.BgColor, 0.25f).WithAlpha(255);
				ColorRgba noFocusColor = ColorRgba.Lerp(this.FgColor, this.BgColor, 0.75f).WithAlpha(255);
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, this.Focused ? focusColor : noFocusColor));
				canvas.DrawRect(0, 0, this.ClientSize.Width, this.ClientSize.Height);
			}
			canvas.PopState();
		}
		protected virtual void OnCollectStateBackgroundDrawcalls(Canvas canvas)
		{
			// Collect the views overlay layer drawcalls
			this.CollectLayerBackgroundDrawcalls(canvas);
		}
		protected virtual string UpdateStatusText()
		{
			CameraAction visibleCamAction = this.drawCamGizmoState != CameraAction.None ? this.drawCamGizmoState : this.camAction;

			// Draw camera action hints
			if (visibleCamAction == CameraAction.Rotate || visibleCamAction == CameraAction.RotateScene)
			{
				return string.Format("Cam Angle: {0,3:0}°", MathF.RadToDeg(this.CameraObj.Transform.Angle));
			}
			else if (visibleCamAction == CameraAction.Move || visibleCamAction == CameraAction.DragScene || this.camVel.Z != 0.0f)
			{
				if (visibleCamAction == CameraAction.Move || visibleCamAction == CameraAction.DragScene)
				{
					return
						string.Format("Cam X:{0,7:0}/n", this.CameraObj.Transform.Pos.X) +
						string.Format("Cam Y:{0,7:0}/n", this.CameraObj.Transform.Pos.Y) +
						string.Format("Cam Z:{0,7:0}", this.CameraObj.Transform.Pos.Z);
				}
				else if (this.camVel.Z != 0.0f)
				{
					return string.Format("Cam Z:{0,7:0}", this.CameraObj.Transform.Pos.Z);
				}
			}

			// Unhandled
			return null;
		}
		protected virtual string UpdateActionText(ref Vector2 actionTextPos)
		{
			return null;
		}
		protected virtual void OnRenderState()
		{
			// Render CamView
			this.CameraComponent.Render(new Rect(this.ClientSize.Width, this.ClientSize.Height));
		}
		protected virtual void OnUpdateState()
		{
			Camera cam = this.CameraComponent;
			GameObject camObj = this.CameraObj;
			Point cursorPos = this.PointToClient(Cursor.Position);

			float unscaledTimeMult = Time.TimeMult / Time.TimeScale;

			this.camTransformChanged = false;
			
			if (this.camAction == CameraAction.DragScene)
			{
				Vector2 curPos = new Vector2(cursorPos.X, cursorPos.Y);
				Vector2 lastPos = new Vector2(this.camActionBeginLoc.X, this.camActionBeginLoc.Y);
				this.camActionBeginLoc = new Point((int)curPos.X, (int)curPos.Y);

				float refZ = (this.HasCameraFocusPosition && camObj.Transform.Pos.Z < this.CameraFocusPosition.Z - cam.NearZ) ? this.CameraFocusPosition.Z : 0.0f;
				if (camObj.Transform.Pos.Z >= refZ - cam.NearZ)
					refZ = camObj.Transform.Pos.Z + MathF.Abs(cam.FocusDist);

				Vector2 targetOff = (-(curPos - lastPos) / this.GetScaleAtZ(refZ));
				Vector2 targetVel = targetOff / unscaledTimeMult;
				MathF.TransformCoord(ref targetVel.X, ref targetVel.Y, camObj.Transform.Angle);
				this.camVel.Z *= MathF.Pow(0.9f, unscaledTimeMult);
				this.camVel += (new Vector3(targetVel, this.camVel.Z) - this.camVel) * unscaledTimeMult;
				this.camTransformChanged = true;
			}
			else if (this.camAction == CameraAction.Move)
			{
				Vector3 moveVec = new Vector3(
					cursorPos.X - this.camActionBeginLoc.X,
					cursorPos.Y - this.camActionBeginLoc.Y,
					this.camVel.Z);

				const float BaseSpeedCursorLen = 25.0f;
				const float BaseSpeed = 3.0f;
				moveVec.X = BaseSpeed * MathF.Sign(moveVec.X) * MathF.Pow(MathF.Abs(moveVec.X) / BaseSpeedCursorLen, 1.5f);
				moveVec.Y = BaseSpeed * MathF.Sign(moveVec.Y) * MathF.Pow(MathF.Abs(moveVec.Y) / BaseSpeedCursorLen, 1.5f);

				MathF.TransformCoord(ref moveVec.X, ref moveVec.Y, camObj.Transform.Angle);

				if (this.camBeginDragScene)
				{
					float refZ = (this.HasCameraFocusPosition && camObj.Transform.Pos.Z < this.CameraFocusPosition.Z - cam.NearZ) ? this.CameraFocusPosition.Z : 0.0f;
					if (camObj.Transform.Pos.Z >= refZ - cam.NearZ)
						refZ = camObj.Transform.Pos.Z + MathF.Abs(cam.FocusDist);
					moveVec = new Vector3(moveVec.Xy * 0.5f / this.GetScaleAtZ(refZ), moveVec.Z);
				}

				this.camVel = moveVec;
				this.camTransformChanged = true;
			}
			else if (this.camVel.Length > 0.01f)
			{
				this.camVel *= MathF.Pow(0.9f, unscaledTimeMult);
				this.camTransformChanged = true;
			}
			else
			{
				this.camTransformChanged = this.camTransformChanged || (this.camVel != Vector3.Zero);
				this.camVel = Vector3.Zero;
			}
			
			if (this.camAction == CameraAction.RotateScene)
			{
				Vector2 center = new Vector2(this.ClientSize.Width, this.ClientSize.Height) * 0.5f;
				Vector2 curPos = new Vector2(cursorPos.X, cursorPos.Y);
				Vector2 lastPos = new Vector2(this.camActionBeginLoc.X, this.camActionBeginLoc.Y);
				this.camActionBeginLoc = new Point((int)curPos.X, (int)curPos.Y);

				float targetVel = (curPos - lastPos).X * MathF.RadAngle360 / 250.0f;
				targetVel *= (curPos.Y - center.Y) / center.Y;

				this.camAngleVel += (targetVel - this.camAngleVel) * unscaledTimeMult;
				this.camTransformChanged = true;
			}
			else if (this.camAction == CameraAction.Rotate)
			{
				float turnDir = 
					0.000125f * MathF.Sign(cursorPos.X - this.camActionBeginLoc.X) * 
					MathF.Pow(MathF.Abs(cursorPos.X - this.camActionBeginLoc.X), 1.25f);
				this.camAngleVel = turnDir;

				this.camTransformChanged = true;
			}
			else if (Math.Abs(this.camAngleVel) > 0.001f)
			{
				this.camAngleVel *= MathF.Pow(0.9f, unscaledTimeMult);
				this.camTransformChanged = true;
			}
			else
			{
				this.camTransformChanged = this.camTransformChanged || (this.camAngleVel != 0.0f);
				this.camAngleVel = 0.0f;
			}


			if (this.camTransformChanged)
			{
				camObj.Transform.MoveBy(this.camVel * unscaledTimeMult);
				camObj.Transform.TurnBy(this.camAngleVel * unscaledTimeMult);

				this.View.OnCamTransformChanged();
				this.Invalidate();
			}
			
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				this.Invalidate();
			}
		}

		protected virtual void OnSceneChanged()
		{
			this.Invalidate();
		}
		protected virtual void OnCurrentCameraChanged(CamView.CameraChangedEventArgs e) {}
		protected virtual void OnGotFocus() {}
		protected virtual void OnLostFocus() {}

		protected virtual void OnDragEnter(DragEventArgs e) {}
		protected virtual void OnDragOver(DragEventArgs e) {}
		protected virtual void OnDragDrop(DragEventArgs e) {}
		protected virtual void OnDragLeave(EventArgs e) {}

		protected virtual void OnKeyDown(KeyEventArgs e) {}
		protected virtual void OnKeyUp(KeyEventArgs e) {}
		protected virtual void OnMouseDown(MouseEventArgs e) {}
		protected virtual void OnMouseUp(MouseEventArgs e) {}
		protected virtual void OnMouseMove(MouseEventArgs e) {}
		protected virtual void OnMouseWheel(MouseEventArgs e) {}
		protected virtual void OnMouseLeave(EventArgs e) {}
		protected virtual void OnCamActionRequiresCursorChanged(EventArgs e) {}

		protected void OnMouseMove()
		{
			Point mousePos = this.PointToClient(Cursor.Position);
			this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, mousePos.X, mousePos.Y, 0));
		}
		

		protected void StopCameraMovement()
		{
			this.camVel = Vector3.Zero;
			this.camAngleVel = 0.0f;
		}

		protected void SetDefaultActiveLayers(params Type[] activeLayers)
		{
			this.lastActiveLayers = activeLayers.ToList();
		}
		protected void SaveActiveLayers()
		{
			this.lastActiveLayers = this.View.ActiveLayers.Select(l => l.GetType()).ToList();
		}
		protected void RestoreActiveLayers()
		{
			this.View.SetActiveLayers(this.lastActiveLayers);
		}
		protected void SetDefaultObjectVisibility(params Type[] activeLayers)
		{
			this.lastObjVisibility = activeLayers.ToList();
		}
		protected void SaveObjectVisibility()
		{
			this.lastObjVisibility = this.View.ObjectVisibility.ToList();
		}
		protected void RestoreObjectVisibility()
		{
			this.View.SetObjectVisibility(this.lastObjVisibility);
		}
		
		protected void CollectLayerDrawcalls(Canvas canvas)
		{
			var layers = this.View.ActiveLayers.ToArray();
			layers.StableSort((a, b) => a.Priority - b.Priority);
			foreach (var layer in layers)
			{
				canvas.PushState();
				layer.OnCollectDrawcalls(canvas);
				canvas.PopState();
			}
		}
		protected void CollectLayerOverlayDrawcalls(Canvas canvas)
		{
			var layers = this.View.ActiveLayers.ToArray();
			layers.StableSort((a, b) => a.Priority - b.Priority);
			foreach (var layer in layers)
			{
				canvas.PushState();
				layer.OnCollectOverlayDrawcalls(canvas);
				canvas.PopState();
			}
		}
		protected void CollectLayerBackgroundDrawcalls(Canvas canvas)
		{
			var layers = this.View.ActiveLayers.ToArray();
			layers.StableSort((a, b) => a.Priority - b.Priority);
			foreach (var layer in layers)
			{
				canvas.PushState();
				layer.OnCollectBackgroundDrawcalls(canvas);
				canvas.PopState();
			}
		}

		private void UpdateFormattedTextRenderers()
		{
			this.statusText.MaxWidth = this.ClientSize.Width - 20;
			this.statusText.MaxHeight = this.ClientSize.Height - 20;
			this.statusText.Fonts = new [] { OverlayFont };
			this.actionText.MaxWidth = MathF.Min(500, this.ClientSize.Width / 2);
			this.actionText.MaxHeight = MathF.Min(500, this.ClientSize.Height / 2);
			this.actionText.Fonts = new [] { OverlayFont };
		}

		private void RenderableControl_Paint(object sender, PaintEventArgs e)
		{
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Terminated) return;

			// Retrieve OpenGL context
 			try { this.RenderableSite.MakeCurrent(); } catch (Exception) { return; }

			// Perform rendering
			try
			{
				this.CameraComponent.Passes.Add(this.camPassBg);
				this.CameraComponent.Passes.Add(this.camPassEdWorld);
				this.CameraComponent.Passes.Add(this.camPassEdScreen);

				this.OnRenderState();

				this.CameraComponent.Passes.Remove(this.camPassBg);
				this.CameraComponent.Passes.Remove(this.camPassEdWorld);
				this.CameraComponent.Passes.Remove(this.camPassEdScreen);
			}
			catch (Exception exception)
			{
				Log.Editor.WriteError("An error occurred during CamView {1} rendering. The current DrawDevice state may be compromised. Exception: {0}", Log.Exception(exception), this.CameraComponent.ToString());
			}
			
			// Make sure the rendered result ends up on screen
			this.RenderableSite.SwapBuffers();
		}
		private void RenderableControl_MouseMove(object sender, MouseEventArgs e)
		{
			this.mouseover = true;
			if (!this.camBeginDragScene) this.OnMouseMove(e);
		}
		private void RenderableControl_MouseUp(object sender, MouseEventArgs e)
		{
			this.drawCamGizmoState = CameraAction.None;

			if (this.camBeginDragScene)
			{
				this.camAction = CameraAction.None;
				this.Cursor = CursorHelper.HandGrab;
			}
			else
			{
				if (this.camAction == CameraAction.Move && e.Button == MouseButtons.Middle)
					this.camAction = CameraAction.None;
				else if (this.camAction == CameraAction.Rotate && e.Button == MouseButtons.Right)
					this.camAction = CameraAction.None;

				this.OnMouseUp(e);
			}

			this.Invalidate();
		}
		private void RenderableControl_MouseDown(object sender, MouseEventArgs e)
		{
			bool alt = (Control.ModifierKeys & Keys.Alt) != Keys.None;

			this.drawCamGizmoState = CameraAction.None;

			if (this.camBeginDragScene)
			{
				this.camActionBeginLoc = e.Location;
				if (e.Button == MouseButtons.Left)
				{
					this.camAction = CameraAction.DragScene;
					this.camActionBeginLocSpace = this.CameraObj.Transform.RelativePos;
					this.Cursor = CursorHelper.HandGrabbing;
				}
				else if (e.Button == MouseButtons.Right)
				{
					this.camAction = CameraAction.RotateScene;
					this.camActionBeginLocSpace = this.CameraObj.Transform.RelativePos;
					this.Cursor = CursorHelper.HandGrabbing;
				}
				else if (e.Button == MouseButtons.Middle)
				{
					this.camAction = CameraAction.Move;
					this.camActionBeginLocSpace = this.CameraObj.Transform.RelativePos;
				}
			}
			else
			{
				if (this.camActionAllowed && this.camAction == CameraAction.None)
				{
					this.camActionBeginLoc = e.Location;
					if (e.Button == MouseButtons.Middle)
					{
						this.camAction = CameraAction.Move;
						this.camActionBeginLocSpace = this.CameraObj.Transform.RelativePos;
					}
					else if (e.Button == MouseButtons.Right)
					{
						this.camAction = CameraAction.Rotate;
						this.camActionBeginLocSpace = new Vector3(this.CameraObj.Transform.RelativeAngle, 0.0f, 0.0f);
					}
				}

				this.OnMouseDown(e);
			}
		}
		private void RenderableControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!this.mouseover) return;

			this.drawCamGizmoState = CameraAction.None;

			if (e.Delta != 0)
			{
				if (this.View.PerspectiveMode == PerspectiveMode.Parallax)
				{
					GameObject camObj = this.CameraObj;
					float curVel = this.camVel.Length * MathF.Sign(this.camVel.Z);
					Vector2 curTemp = new Vector2(
						(e.X * 2.0f / this.ClientSize.Width) - 1.0f,
						(e.Y * 2.0f / this.ClientSize.Height) - 1.0f);
					MathF.TransformCoord(ref curTemp.X, ref curTemp.Y, camObj.Transform.RelativeAngle);

					if (MathF.Sign(e.Delta) != MathF.Sign(curVel))
						curVel = 0.0f;
					else
						curVel *= 1.5f;
					curVel += 0.015f * e.Delta;
					curVel = MathF.Sign(curVel) * MathF.Min(MathF.Abs(curVel), 500.0f);

					Vector3 movVec = new Vector3(
						MathF.Sign(e.Delta) * MathF.Sign(curTemp.X) * MathF.Pow(curTemp.X, 2.0f), 
						MathF.Sign(e.Delta) * MathF.Sign(curTemp.Y) * MathF.Pow(curTemp.Y, 2.0f), 
						1.0f);
					movVec.Normalize();
					this.camVel = movVec * curVel;
				}
				else
				{
					this.View.FocusDist = this.View.FocusDist + this.View.FocusDistIncrement * e.Delta / 40;
				}
			}

			this.OnMouseWheel(e);
		}
		private void RenderableControl_MouseLeave(object sender, EventArgs e)
		{
			if (!this.camBeginDragScene) this.OnMouseMove();
			this.OnMouseLeave(e);
			this.mouseover = false;

			this.Invalidate();
		}
		private void RenderableControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.camActionAllowed)
			{
				if (e.KeyCode == Keys.Space && !this.IsActionInProgress && !this.camBeginDragScene)
				{
					this.camBeginDragScene = true;
					this.OnCamActionRequiresCursorChanged(EventArgs.Empty);
					this.Cursor = CursorHelper.HandGrab;
				}
				else if (e.KeyCode == Keys.F)
				{
					if (DualityEditorApp.Selection.MainGameObject != null)
						this.View.FocusOnObject(DualityEditorApp.Selection.MainGameObject);
					else
						this.View.ResetCamera();
				}
				else if (e.Control && e.KeyCode == Keys.Left)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.X = MathF.Round(pos.X - 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.X = MathF.Round(pos.X + 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.Y = MathF.Round(pos.Y - 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.Y = MathF.Round(pos.Y + 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Add)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.Z = MathF.Round(pos.Z + 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
				else if (e.Control && e.KeyCode == Keys.Subtract)
				{
					this.drawCamGizmoState = CameraAction.Move;
					Vector3 pos = this.CameraObj.Transform.Pos;
					pos.Z = MathF.Round(pos.Z - 1.0f);
					this.CameraObj.Transform.Pos = pos;
					this.Invalidate();
				}
			}

			this.OnKeyDown(e);
		}
		private void RenderableControl_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space && this.camBeginDragScene)
			{
				this.camBeginDragScene = false;
				this.camAction = CameraAction.None;
				this.Cursor = CursorHelper.Arrow;
				this.OnCamActionRequiresCursorChanged(EventArgs.Empty);
			}

			this.OnKeyUp(e);
		}
		private void RenderableControl_GotFocus(object sender, EventArgs e)
		{
			this.MakeDualityTarget();
			this.OnGotFocus();
		}
		private void RenderableControl_LostFocus(object sender, EventArgs e)
		{
			if (DualityEditorApp.MainForm == null) return;

			this.camAction = CameraAction.None;
			this.OnLostFocus();
			this.Invalidate();
		}
		private void RenderableControl_DragOver(object sender, DragEventArgs e)
		{
			this.OnDragOver(e);
			// Force immediate buffer swap, because there is no event loop while dragging.
			DualityEditorApp.PerformBufferSwap();
		}
		private void RenderableControl_DragLeave(object sender, EventArgs e)
		{
			this.OnDragLeave(e);
		}
		private void RenderableControl_DragEnter(object sender, DragEventArgs e)
		{
			this.OnDragEnter(e);
		}
		private void RenderableControl_DragDrop(object sender, DragEventArgs e)
		{
			this.OnDragDrop(e);
		}
		private void RenderableControl_Resize(object sender, EventArgs e)
		{
			this.UpdateFormattedTextRenderers();
		}
		private void View_FocusDistChanged(object sender, EventArgs e)
		{
			if (!this.camBeginDragScene) this.OnMouseMove();
		}
		private void View_CurrentCameraChanged(object sender, CamView.CameraChangedEventArgs e)
		{
			this.OnCurrentCameraChanged(e);
		}
		private void DualityEditorApp_UpdatingEngine(object sender, EventArgs e)
		{
			this.OnUpdateState();
		}
		private void DualityEditorApp_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.HasAnyProperty(ReflectionInfo.Property_Transform_RelativePos, ReflectionInfo.Property_Transform_RelativeAngle) &&
				e.Objects.Components.Any(c => c.GameObj == this.CameraObj))
			{
				if (!this.camBeginDragScene) this.OnMouseMove();
			}
		}
		private void Scene_Changed(object sender, EventArgs e)
		{
			this.OnSceneChanged();
		}
		private void camPassEdScreen_CollectDrawcalls(object sender, CollectDrawcallEventArgs e)
		{
			Canvas canvas = new Canvas(e.Device);
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, this.FgColor));
			canvas.State.TextFont = OverlayFont;

			this.OnCollectStateOverlayDrawcalls(canvas);
		}
		private void camPassEdWorld_CollectDrawcalls(object sender, CollectDrawcallEventArgs e)
		{
			Canvas canvas = new Canvas(e.Device);
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, this.FgColor));
			canvas.State.TextFont = Duality.Resources.Font.GenericMonospace8;

			this.OnCollectStateDrawcalls(canvas);
		}
		private void camPassBg_CollectDrawcalls(object sender, CollectDrawcallEventArgs e)
		{
			Canvas canvas = new Canvas(e.Device);
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Mask, this.FgColor));
			canvas.State.TextFont = Duality.Resources.Font.GenericMonospace8;

			this.OnCollectStateBackgroundDrawcalls(canvas);
		}

		public virtual HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			if (this.camActionAllowed)
			{
				return HelpInfo.FromText(CamViewRes.CamView_Help_CamActions, 
					CamViewRes.CamView_Help_CamActions_Move + "\n" +
					CamViewRes.CamView_Help_CamActions_MoveAlternate + "\n" +
					CamViewRes.CamView_Help_CamActions_MoveStep + "\n" +
					CamViewRes.CamView_Help_CamActions_Focus);
			}
			else
			{
				return null;
			}
		}
	}
}
