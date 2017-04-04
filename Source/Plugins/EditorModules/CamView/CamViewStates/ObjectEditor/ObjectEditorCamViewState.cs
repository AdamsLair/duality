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
	public abstract class ObjectEditorCamViewState : CamViewState
	{
		private   bool                 actionAllowed       = true;
		private   bool                 actionIsClone       = false;
		private   Point                actionBeginLoc      = Point.Empty;
		private   Vector3              actionBeginLocSpace = Vector3.Zero;
		private   Vector3              actionLastLocSpace  = Vector3.Zero;
		private   ObjectEditorAxisLock actionLockedAxis    = ObjectEditorAxisLock.None;
		private   ObjectEditorAction   action              = ObjectEditorAction.None;
		private   bool                 selectionStatsValid = false;
		private   Vector3              selectionCenter     = Vector3.Zero;
		private   float                selectionRadius     = 0.0f;
		private   ObjectSelection      activeRectSel       = new ObjectSelection();
		private   ObjectEditorAction   mouseoverAction     = ObjectEditorAction.None;
		private   ObjectEditorSelObj   mouseoverObject     = null;
		private   bool                 mouseoverSelect     = false;
		private   ObjectEditorAction   drawSelGizmoState   = ObjectEditorAction.None;
		protected List<ObjectEditorSelObj> actionObjSel    = new List<ObjectEditorSelObj>();
		protected List<ObjectEditorSelObj> allObjSel       = new List<ObjectEditorSelObj>();
		protected List<ObjectEditorSelObj> indirectObjSel  = new List<ObjectEditorSelObj>();


		public ObjectEditorAction ObjAction
		{
			get { return this.action; }
		}
		public IEnumerable<ObjectEditorSelObj> SelectedObjects
		{
			get { return this.allObjSel; }
		}
		public bool MouseActionAllowed
		{
			get { return this.actionAllowed; }
			protected set
			{
				this.actionAllowed = value;
				if (!this.actionAllowed)
				{
					this.mouseoverAction = ObjectEditorAction.None;
					this.mouseoverObject = null;
					this.mouseoverSelect = false;
					if (this.action != ObjectEditorAction.None)
					{
						this.EndAction();
						this.UpdateAction();
					}
				}
			}
		}
		private ObjectEditorAction VisibleObjAction
		{
			get
			{
				return 
					(this.drawSelGizmoState != ObjectEditorAction.None ? this.drawSelGizmoState : 
					(this.action != ObjectEditorAction.None ? this.action :
					(this.mouseoverAction != ObjectEditorAction.RectSelect ? this.mouseoverAction :
					ObjectEditorAction.None)));
			}
		}

		protected override bool HasCameraFocusPosition
		{
			get { return this.allObjSel.Any(); }
		}
		protected override Vector3 CameraFocusPosition
		{
			get { return this.selectionCenter; }
		}
		protected override bool IsActionInProgress
		{
			get { return base.IsActionInProgress || this.action != ObjectEditorAction.None; }
		}
		

		public virtual ObjectEditorSelObj PickSelObjAt(int x, int y)
		{
			return null;
		}
		public virtual List<ObjectEditorSelObj> PickSelObjIn(int x, int y, int w, int h)
		{
			return new List<ObjectEditorSelObj>();
		}
		public virtual void SelectObjects(IEnumerable<ObjectEditorSelObj> selObjEnum, SelectMode mode = SelectMode.Set) {}
		public virtual void ClearSelection() {}
		protected virtual void PostPerformAction(IEnumerable<ObjectEditorSelObj> selObjEnum, ObjectEditorAction action) {}

		public virtual void DeleteObjects(IEnumerable<ObjectEditorSelObj> objEnum) {}
		public virtual List<ObjectEditorSelObj> CloneObjects(IEnumerable<ObjectEditorSelObj> objEnum) { return new List<ObjectEditorSelObj>(); }
		public void MoveSelectionBy(Vector3 move)
		{
			if (move == Vector3.Zero) return;

			UndoRedoManager.Do(new MoveCamViewObjAction(
				this.actionObjSel, 
				obj => this.PostPerformAction(obj, ObjectEditorAction.Move), 
				move));

			this.drawSelGizmoState = ObjectEditorAction.Move;
			this.InvalidateSelectionStats();
			this.Invalidate();
		}
		public void MoveSelectionTo(Vector3 target)
		{
			this.MoveSelectionBy(target - this.selectionCenter);
		}
		public void MoveSelectionToCursor()
		{
			Point mousePos = this.PointToClient(Cursor.Position);
			Vector3 mouseSpaceCoord = this.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, this.selectionCenter.Z));

			// Apply user guide snapping
			if ((this.SnapToUserGuides & UserGuideType.Position) != UserGuideType.None)
			{
				mouseSpaceCoord = this.EditingUserGuide.SnapPosition(mouseSpaceCoord);
			}

			this.MoveSelectionTo(mouseSpaceCoord);
		}
		public void RotateSelectionBy(float rotation)
		{
			if (rotation == 0.0f) return;
			
			UndoRedoManager.Do(new RotateCamViewObjAction(
				this.actionObjSel, 
				obj => this.PostPerformAction(obj, ObjectEditorAction.Rotate), 
				rotation));

			this.drawSelGizmoState = ObjectEditorAction.Rotate;
			this.InvalidateSelectionStats();
			this.Invalidate();
		}
		public void ScaleSelectionBy(float scale)
		{
			if (scale == 1.0f) return;

			UndoRedoManager.Do(new ScaleCamViewObjAction(
				this.actionObjSel, 
				obj => this.PostPerformAction(obj, ObjectEditorAction.Scale), 
				scale));

			this.drawSelGizmoState = ObjectEditorAction.Scale;
			this.InvalidateSelectionStats();
			this.Invalidate();
		}

		protected void DrawSelectionMarkers(Canvas canvas, IEnumerable<ObjectEditorSelObj> obj)
		{
			// Determine turned Camera axes for angle-independent drawing
			Vector2 catDotX, catDotY;
			float camAngle = this.CameraObj.Transform.Angle;
			MathF.GetTransformDotVec(camAngle, out catDotX, out catDotY);
			Vector3 right = new Vector3(1.0f, 0.0f, 0.0f);
			Vector3 down = new Vector3(0.0f, 1.0f, 0.0f);
			MathF.TransformDotVec(ref right, ref catDotX, ref catDotY);
			MathF.TransformDotVec(ref down, ref catDotX, ref catDotY);

			canvas.PushState();
			canvas.State.DepthOffset = -1.0f;
			foreach (ObjectEditorSelObj selObj in obj)
			{
				if (!selObj.HasTransform) continue;
				Vector3 posTemp = selObj.Pos;
				float scaleTemp = 1.0f;
				float radTemp = selObj.BoundRadius;

				if (!canvas.DrawDevice.IsCoordInView(posTemp, radTemp)) continue;

				// Draw selection marker
				if (selObj.ShowPos)
				{
					canvas.DrawDevice.PreprocessCoords(ref posTemp, ref scaleTemp);
					posTemp.Z = 0.0f;
					{
						ColorRgba color = canvas.State.ColorTint * canvas.State.Material.MainColor;
						VertexC1P3[] vertices = new VertexC1P3[4];
						vertices[0].Pos = posTemp - right * 5.0f;
						vertices[1].Pos = posTemp + right * 5.0f;
						vertices[2].Pos = posTemp - down * 5.0f;
						vertices[3].Pos = posTemp + down * 5.0f;
						vertices[0].Color = color;
						vertices[1].Color = color;
						vertices[2].Color = color;
						vertices[3].Color = color;
						canvas.DrawDevice.AddVertices(canvas.State.Material, VertexMode.Lines, vertices);
					}
				}

				// Draw angle marker
				if (selObj.ShowAngle)
				{
					posTemp = selObj.Pos + 
						radTemp * right * MathF.Sin(selObj.Angle - camAngle) - 
						radTemp * down * MathF.Cos(selObj.Angle - camAngle);
					canvas.DrawLine(selObj.Pos.X, selObj.Pos.Y, selObj.Pos.Z, posTemp.X, posTemp.Y, posTemp.Z);
				}

				// Draw boundary
				if (selObj.ShowBoundRadius && radTemp > 0.0f)
					canvas.DrawCircle(selObj.Pos.X, selObj.Pos.Y, selObj.Pos.Z, radTemp);
			}
			canvas.PopState();
		}
		protected void DrawLockedAxes(Canvas canvas, float x, float y, float z, float r)
		{
			Vector3 refPos = canvas.DrawDevice.RefCoord;
			float nearZ = canvas.DrawDevice.NearZ;

			canvas.PushState();
			if (this.actionLockedAxis == ObjectEditorAxisLock.X)
			{
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, ColorRgba.Red, 0.5f)));
				canvas.DrawLine(x - r, y, z, x + r, y, z);
			}
			if (this.actionLockedAxis == ObjectEditorAxisLock.Y)
			{
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, ColorRgba.Green, 0.5f)));
				canvas.DrawLine(x, y - r, z, x, y + r, z);
			}
			if (this.actionLockedAxis == ObjectEditorAxisLock.Z)
			{
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, ColorRgba.Blue, 0.5f)));
				canvas.DrawLine(x, y, MathF.Max(z - r, refPos.Z + nearZ + 10), x, y, z);
				canvas.DrawLine(x, y, z, x, y, z + r);
			}
			canvas.PopState();
		}

		/// <summary>
		/// Returns an axis-locked version of the specified vector, if requested by the user. Doesn't
		/// do anything when no axis lock is in currently active.
		/// </summary>
		/// <param name="baseVec">The base vector without any locking in place.</param>
		/// <param name="lockedVec">A reference vector that represents the base vector being locked to all axes at once.</param>
		/// <param name="beginToTarget">The movement vector to evaluate in order to determine the axes to which the base vector will be locked.</param>
		/// <returns></returns>
		protected Vector3 ApplyAxisLock(Vector3 baseVec, Vector3 lockedVec, Vector3 beginToTarget)
		{
			bool shift = (Control.ModifierKeys & Keys.Shift) != Keys.None;
			if (!shift)
			{
				this.actionLockedAxis = ObjectEditorAxisLock.None;
				return baseVec;
			}
			else
			{
				float xWeight = MathF.Abs(Vector3.Dot(beginToTarget.Normalized, Vector3.UnitX));
				float yWeight = MathF.Abs(Vector3.Dot(beginToTarget.Normalized, Vector3.UnitY));
				float zWeight = MathF.Abs(Vector3.Dot(beginToTarget.Normalized, Vector3.UnitZ));
				
				if (xWeight >= yWeight && xWeight >= zWeight)
				{
					this.actionLockedAxis = ObjectEditorAxisLock.X;
					return new Vector3(baseVec.X, lockedVec.Y, lockedVec.Z);
				}
				else if (yWeight >= xWeight && yWeight >= zWeight)
				{
					this.actionLockedAxis = ObjectEditorAxisLock.Y;
					return new Vector3(lockedVec.X, baseVec.Y, lockedVec.Z);
				}
				else if (zWeight >= yWeight && zWeight >= xWeight)
				{
					this.actionLockedAxis = ObjectEditorAxisLock.Z;
					return new Vector3(lockedVec.X, lockedVec.Y, baseVec.Z);
				}
				return lockedVec;
			}
		}
		protected Vector2 ApplyAxisLock(Vector2 baseVec, Vector2 lockedVec, Vector2 beginToTarget)
		{
			return this.ApplyAxisLock(new Vector3(baseVec), new Vector3(lockedVec), new Vector3(beginToTarget)).Xy;
		}
		protected Vector3 ApplyAxisLock(Vector3 targetVec, Vector3 lockedVec)
		{
			return targetVec + this.ApplyAxisLock(Vector3.Zero, lockedVec - targetVec, lockedVec - targetVec);
		}
		protected Vector2 ApplyAxisLock(Vector2 targetVec, Vector2 lockedVec)
		{
			return targetVec + this.ApplyAxisLock(Vector2.Zero, lockedVec - targetVec, lockedVec - targetVec);
		}
		
		protected void BeginAction(ObjectEditorAction action)
		{
			if (action == ObjectEditorAction.None) return;
			Point mouseLoc = this.PointToClient(Cursor.Position);

			this.ValidateSelectionStats();

			this.StopCameraMovement();

			this.action = action;
			this.actionBeginLoc = mouseLoc;
			this.actionBeginLocSpace = this.GetSpaceCoord(new Vector3(
				mouseLoc.X, 
				mouseLoc.Y, 
				(this.action == ObjectEditorAction.RectSelect) ? 0.0f : this.selectionCenter.Z));

			if (this.action == ObjectEditorAction.Move)
				this.actionBeginLocSpace.Z = this.CameraObj.Transform.Pos.Z;

			this.actionLastLocSpace = this.actionBeginLocSpace;

			if (Sandbox.State == SandboxState.Playing)
				Sandbox.Freeze();

			this.OnBeginAction(this.action);
		}
		protected void EndAction()
		{
			if (this.action == ObjectEditorAction.None) return;
			Point mouseLoc = this.PointToClient(Cursor.Position);

			if (this.action == ObjectEditorAction.RectSelect)
			{
				this.activeRectSel = new ObjectSelection();
			}

			if (Sandbox.State == SandboxState.Playing)
				Sandbox.UnFreeze();

			this.OnEndAction(this.action);
			this.action = ObjectEditorAction.None;

			if (this.actionIsClone)
			{
				this.actionIsClone = false;
				UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromFirst);
			}
			UndoRedoManager.Finish();
		}
		protected void UpdateAction()
		{
			Point mouseLoc = this.PointToClient(Cursor.Position);

			if (this.action == ObjectEditorAction.RectSelect)
				this.UpdateRectSelection(mouseLoc);
			else if (this.action == ObjectEditorAction.Move)
				this.UpdateObjMove(mouseLoc);
			else if (this.action == ObjectEditorAction.Rotate)
				this.UpdateObjRotate(mouseLoc);
			else if (this.action == ObjectEditorAction.Scale)
				this.UpdateObjScale(mouseLoc);
			else
				this.UpdateMouseover(mouseLoc);

			if (this.action != ObjectEditorAction.None)
				this.InvalidateSelectionStats();
		}

		protected void InvalidateSelectionStats()
		{
			this.selectionStatsValid = false;
		}
		private void ValidateSelectionStats()
		{
			if (this.selectionStatsValid) return;
			
			List<ObjectEditorSelObj> transformObjSel = this.allObjSel.Where(s => s.HasTransform).ToList();

			this.selectionCenter = Vector3.Zero;
			this.selectionRadius = 0.0f;

			foreach (ObjectEditorSelObj s in transformObjSel)
				this.selectionCenter += s.Pos;
			if (transformObjSel.Count > 0) this.selectionCenter /= transformObjSel.Count;

			foreach (ObjectEditorSelObj s in transformObjSel)
				this.selectionRadius = MathF.Max(this.selectionRadius, s.BoundRadius + (s.Pos - this.selectionCenter).Length);

			this.selectionStatsValid = true;
		}

		protected void UpdateMouseover(Point mouseLoc)
		{
			bool lastMouseoverSelect = this.mouseoverSelect;
			ObjectEditorSelObj lastMouseoverObject = this.mouseoverObject;
			ObjectEditorAction lastMouseoverAction = this.mouseoverAction;

			if (this.actionAllowed && !this.CamActionRequiresCursor && this.CamAction == CameraAction.None)
			{
				this.ValidateSelectionStats();

				// Determine object at mouse position
				this.mouseoverObject = this.PickSelObjAt(mouseLoc.X, mouseLoc.Y);

				// Determine action variables
				Vector3 mouseSpaceCoord = this.GetSpaceCoord(new Vector3(mouseLoc.X, mouseLoc.Y, this.selectionCenter.Z));
				float scale = this.GetScaleAtZ(this.selectionCenter.Z);
				const float boundaryThickness = 10.0f;
				bool tooSmall = this.selectionRadius * scale <= boundaryThickness * 2.0f;
				bool mouseOverBoundary = MathF.Abs((mouseSpaceCoord - this.selectionCenter).Length - this.selectionRadius) * scale < boundaryThickness;
				bool mouseInsideBoundary = !mouseOverBoundary && (mouseSpaceCoord - this.selectionCenter).Length < this.selectionRadius;
				bool mouseAtCenterAxis = 
					MathF.Abs(mouseSpaceCoord.X - this.selectionCenter.X) * scale < boundaryThickness || 
					MathF.Abs(mouseSpaceCoord.Y - this.selectionCenter.Y) * scale < boundaryThickness;
				bool shift = (Control.ModifierKeys & Keys.Shift) != Keys.None;
				bool ctrl = (Control.ModifierKeys & Keys.Control) != Keys.None;

				bool anySelection = this.actionObjSel.Count > 0;
				bool canMove = this.actionObjSel.Any(s => s.IsActionAvailable(ObjectEditorAction.Move));
				bool canRotate = (canMove && this.actionObjSel.Count > 1) || this.actionObjSel.Any(s => s.IsActionAvailable(ObjectEditorAction.Rotate));
				bool canScale = (canMove && this.actionObjSel.Count > 1) || this.actionObjSel.Any(s => s.IsActionAvailable(ObjectEditorAction.Scale));

				// Select which action to propose
				this.mouseoverSelect = false;
				if (ctrl)
					this.mouseoverAction = ObjectEditorAction.RectSelect;
				else if (anySelection && !tooSmall && mouseOverBoundary && mouseAtCenterAxis && this.selectionRadius > 0.0f && canScale)
					this.mouseoverAction = ObjectEditorAction.Scale;
				else if (anySelection && !tooSmall && mouseOverBoundary && canRotate)
					this.mouseoverAction = ObjectEditorAction.Rotate;
				else if (anySelection && mouseInsideBoundary && canMove)
					this.mouseoverAction = ObjectEditorAction.Move;
				else if (shift) // Lower prio than Ctrl, because Shift also modifies mouse actions
					this.mouseoverAction = ObjectEditorAction.RectSelect;
				else if (this.mouseoverObject != null && this.mouseoverObject.IsActionAvailable(ObjectEditorAction.Move))
				{
					this.mouseoverAction = ObjectEditorAction.Move; 
					this.mouseoverSelect = true;
				}
				else
					this.mouseoverAction = ObjectEditorAction.RectSelect;
			}
			else
			{
				this.mouseoverObject = null;
				this.mouseoverSelect = false;
				this.mouseoverAction = ObjectEditorAction.None;
			}

			// If mouseover changed..
			if (this.mouseoverObject != lastMouseoverObject || 
				this.mouseoverSelect != lastMouseoverSelect ||
				this.mouseoverAction != lastMouseoverAction)
			{
				// Adjust mouse cursor based on proposed action
				if (this.mouseoverAction == ObjectEditorAction.Move)
					this.Cursor = CursorHelper.ArrowActionMove;
				else if (this.mouseoverAction == ObjectEditorAction.Rotate)
					this.Cursor = CursorHelper.ArrowActionRotate;
				else if (this.mouseoverAction == ObjectEditorAction.Scale)
					this.Cursor = CursorHelper.ArrowActionScale;
				else
					this.Cursor = CursorHelper.Arrow;
			}
			
			// Redraw if action gizmos might be visible
			if (this.actionAllowed)
				this.Invalidate();
		}
		private void UpdateRectSelection(Point mouseLoc)
		{
			if (DualityEditorApp.IsSelectionChanging) return; // Prevent Recursion in case SelectObjects triggers UpdateAction.

			bool shift = (Control.ModifierKeys & Keys.Shift) != Keys.None;
			bool ctrl = (Control.ModifierKeys & Keys.Control) != Keys.None;

			// Determine picked rect
			int pX = Math.Max(Math.Min(mouseLoc.X, this.actionBeginLoc.X), 0);
			int pY = Math.Max(Math.Min(mouseLoc.Y, this.actionBeginLoc.Y), 0);
			int pX2 = Math.Max(mouseLoc.X, this.actionBeginLoc.X);
			int pY2 = Math.Max(mouseLoc.Y, this.actionBeginLoc.Y);
			int pW = Math.Max(pX2 - pX, 1);
			int pH = Math.Max(pY2 - pY, 1);

			// Check which renderers are picked
			List<ObjectEditorSelObj> picked = this.PickSelObjIn(pX, pY, pW, pH);

			// Store in internal rect selection
			ObjectSelection oldRectSel = this.activeRectSel;
			this.activeRectSel = new ObjectSelection(picked);

			// Apply internal selection to actual editor selection
			if (shift || ctrl)
			{
				if (this.activeRectSel.ObjectCount > 0)
				{
					ObjectSelection added = (this.activeRectSel - oldRectSel) + (oldRectSel - this.activeRectSel);
					this.SelectObjects(added.OfType<ObjectEditorSelObj>(), shift ? SelectMode.Append : SelectMode.Toggle);
				}
			}
			else if (this.activeRectSel.ObjectCount > 0)
				this.SelectObjects(this.activeRectSel.OfType<ObjectEditorSelObj>());
			else
				this.ClearSelection();

			this.Invalidate();
		}
		private void UpdateObjMove(Point mouseLoc)
		{
			this.ValidateSelectionStats();

			// Determine where to move the object
			float zMovement = this.CameraObj.Transform.Pos.Z - this.actionLastLocSpace.Z;
			Vector3 mousePosSpace = this.GetSpaceCoord(new Vector3(mouseLoc.X, mouseLoc.Y, this.selectionCenter.Z + zMovement)); mousePosSpace.Z = 0;
			Vector3 resetMovement = this.actionBeginLocSpace - this.actionLastLocSpace;
			Vector3 targetMovement = mousePosSpace - this.actionLastLocSpace; targetMovement.Z = zMovement;

			// Apply user guide snapping
			if ((this.SnapToUserGuides & UserGuideType.Position) != UserGuideType.None)
			{
				Vector3 snappedCenter = this.selectionCenter;
				Vector3 targetPosSpace = snappedCenter + targetMovement;

				// When moving multiple objects, snap only relative to the original selection center, so individual grid alignment is retained
				if (this.actionObjSel.Count > 1)
					snappedCenter = this.EditingUserGuide.SnapPosition(this.selectionCenter);

				targetPosSpace = this.EditingUserGuide.SnapPosition(targetPosSpace);
				targetMovement = targetPosSpace - snappedCenter;
			}

			// Apply user axis locks
			targetMovement = this.ApplyAxisLock(targetMovement, resetMovement, mousePosSpace - this.actionBeginLocSpace + new Vector3(0.0f, 0.0f, this.CameraObj.Transform.Pos.Z));

			// Move the selected objects accordingly
			this.MoveSelectionBy(targetMovement);

			this.actionLastLocSpace += targetMovement;
		}
		private void UpdateObjRotate(Point mouseLoc)
		{
			this.ValidateSelectionStats();

			Vector3 spaceCoord = this.GetSpaceCoord(new Vector3(mouseLoc.X, mouseLoc.Y, this.selectionCenter.Z));
			float lastAngle = MathF.Angle(this.selectionCenter.X, this.selectionCenter.Y, this.actionLastLocSpace.X, this.actionLastLocSpace.Y);
			float curAngle = MathF.Angle(this.selectionCenter.X, this.selectionCenter.Y, spaceCoord.X, spaceCoord.Y);
			float rotation = curAngle - lastAngle;

			this.RotateSelectionBy(rotation);

			this.actionLastLocSpace = spaceCoord;
		}
		private void UpdateObjScale(Point mouseLoc)
		{
			this.ValidateSelectionStats();
			if (this.selectionRadius == 0.0f) return;

			Vector3 spaceCoord = this.GetSpaceCoord(new Vector3(mouseLoc.X, mouseLoc.Y, this.selectionCenter.Z));
			float lastRadius = this.selectionRadius;
			float curRadius = (this.selectionCenter - spaceCoord).Length;

			if ((this.SnapToUserGuides & UserGuideType.Scale) != UserGuideType.None)
			{
				curRadius = this.EditingUserGuide.SnapSize(curRadius);
			}

			float scale = MathF.Clamp(curRadius / lastRadius, 0.0001f, 10000.0f);
			this.ScaleSelectionBy(scale);

			this.actionLastLocSpace = spaceCoord;
			this.Invalidate();
		}

		protected virtual void OnBeginAction(ObjectEditorAction action) {}
		protected virtual void OnEndAction(ObjectEditorAction action) {}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();
		}

		protected override void OnCollectStateWorldOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectStateWorldOverlayDrawcalls(canvas);

			// Assure we know how to display the current selection
			this.ValidateSelectionStats();

			List<ObjectEditorSelObj> transformObjSel = this.allObjSel.Where(s => s.HasTransform).ToList();
			Point cursorPos = this.PointToClient(Cursor.Position);
			canvas.PushState();
			canvas.State.DepthOffset = -1.0f;
			
			// Draw indirectly selected object overlay
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, this.BgColor, 0.75f)));
			this.DrawSelectionMarkers(canvas, this.indirectObjSel);
			if (this.mouseoverObject != null && (this.mouseoverAction == ObjectEditorAction.RectSelect || this.mouseoverSelect) && !transformObjSel.Contains(this.mouseoverObject)) 
				this.DrawSelectionMarkers(canvas, new [] { this.mouseoverObject });

			// Draw selected object overlay
			canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, this.FgColor));
			this.DrawSelectionMarkers(canvas, transformObjSel);

			// Draw overall selection boundary
			if (transformObjSel.Count > 1)
			{
				float midZ = transformObjSel.Average(t => t.Pos.Z);
				float maxZDiff = transformObjSel.Max(t => MathF.Abs(t.Pos.Z - midZ));
				if (maxZDiff > 0.001f)
				{
					canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, this.BgColor, 0.5f)));
					canvas.DrawSphere(
						this.selectionCenter.X, 
						this.selectionCenter.Y, 
						this.selectionCenter.Z, 
						this.selectionRadius);
				}
				else
				{
					canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, ColorRgba.Lerp(this.FgColor, this.BgColor, 0.5f)));
					canvas.DrawCircle(
						this.selectionCenter.X, 
						this.selectionCenter.Y, 
						this.selectionCenter.Z, 
						this.selectionRadius);
				}
			}

			// Draw scale action dots
			bool canMove = this.actionObjSel.Any(s => s.IsActionAvailable(ObjectEditorAction.Move));
			bool canScale = (canMove && this.actionObjSel.Count > 1) || this.actionObjSel.Any(s => s.IsActionAvailable(ObjectEditorAction.Scale));
			if (canScale)
			{
				float dotR = 3.0f / this.GetScaleAtZ(this.selectionCenter.Z);
				canvas.State.DepthOffset -= 0.1f;
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Solid, this.FgColor));
				canvas.FillCircle(
					this.selectionCenter.X + this.selectionRadius, 
					this.selectionCenter.Y, 
					this.selectionCenter.Z,
					dotR);
				canvas.FillCircle(
					this.selectionCenter.X - this.selectionRadius, 
					this.selectionCenter.Y, 
					this.selectionCenter.Z,
					dotR);
				canvas.FillCircle(
					this.selectionCenter.X, 
					this.selectionCenter.Y + this.selectionRadius, 
					this.selectionCenter.Z,
					dotR);
				canvas.FillCircle(
					this.selectionCenter.X, 
					this.selectionCenter.Y - this.selectionRadius, 
					this.selectionCenter.Z,
					dotR);
				canvas.State.DepthOffset += 0.1f;
			}

			if (this.action != ObjectEditorAction.None)
			{
				// Draw action lock axes
				this.DrawLockedAxes(canvas, this.selectionCenter.X, this.selectionCenter.Y, this.selectionCenter.Z, this.selectionRadius * 4);
			}

			canvas.PopState();
		}
		protected override void OnCollectStateOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectStateOverlayDrawcalls(canvas);
			
			Point cursorPos = this.PointToClient(Cursor.Position);
			canvas.PushState();
			{
				// Draw rect selection
				if (this.action == ObjectEditorAction.RectSelect)
					canvas.DrawRect(this.actionBeginLoc.X, this.actionBeginLoc.Y, cursorPos.X - this.actionBeginLoc.X, cursorPos.Y - this.actionBeginLoc.Y);
			}
			canvas.PopState();
		}

		protected override string UpdateActionText(ref Vector2 actionTextPos)
		{
			string actionText = null;
			ObjectEditorAction visibleObjectAction = this.VisibleObjAction;
			if (visibleObjectAction != ObjectEditorAction.None && ((this.mouseoverObject != null && this.mouseoverSelect) || this.actionObjSel.Count == 1))
			{
				ObjectEditorSelObj obj;
				if (this.mouseoverObject != null || this.mouseoverAction == visibleObjectAction)
				{
					obj = (this.mouseoverObject != null && this.mouseoverSelect) ? this.mouseoverObject : this.actionObjSel[0];
				}
				else
				{
					obj = this.actionObjSel[0];
					actionTextPos = this.GetScreenCoord(this.actionObjSel[0].Pos).Xy;
				}

				// If the SelObj is valid, let it determine the current action text
				if (obj.ActualObject != null)
				{
					actionText = obj.UpdateActionText(visibleObjectAction, this.action != ObjectEditorAction.None);
				}
			}

			if (actionText != null)
				return actionText;
			
			return base.UpdateActionText(ref actionTextPos);
		}
		protected override string UpdateStatusText()
		{
			ObjectEditorAction visibleObjectAction = this.VisibleObjAction;

			if (visibleObjectAction == ObjectEditorAction.Move)            return Properties.CamViewRes.CamView_Action_Move;
			else if (visibleObjectAction == ObjectEditorAction.Rotate)     return Properties.CamViewRes.CamView_Action_Rotate;
			else if (visibleObjectAction == ObjectEditorAction.Scale)      return Properties.CamViewRes.CamView_Action_Scale;
			else if (visibleObjectAction == ObjectEditorAction.RectSelect) return Properties.CamViewRes.CamView_Action_Select_Active;

			return base.UpdateStatusText();
		}

		protected override void OnUpdateState()
		{
			this.ValidateSelectionStats();
			base.OnUpdateState();
			if (DualityApp.ExecContext == DualityApp.ExecutionContext.Game)
			{
				this.InvalidateSelectionStats();
			}
		}
		protected override void OnSceneChanged()
		{
			base.OnSceneChanged();
			if (this.mouseoverObject != null && this.mouseoverObject.IsInvalid)
				this.mouseoverObject = null;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.UpdateAction();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.drawSelGizmoState = ObjectEditorAction.None;

			if (this.action == ObjectEditorAction.RectSelect && this.actionBeginLoc == e.Location)
				this.UpdateRectSelection(e.Location);

			if (e.Button == MouseButtons.Left)
				this.EndAction();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			bool alt = (Control.ModifierKeys & Keys.Alt) != Keys.None;

			this.drawSelGizmoState = ObjectEditorAction.None;
			
			if (this.action == ObjectEditorAction.None)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (this.mouseoverSelect)
					{
						// To interact with an object that isn't selected yet: Select it.
						if (!this.allObjSel.Contains(this.mouseoverObject))
							this.SelectObjects(new [] { this.mouseoverObject });
					}
					if (alt)
					{
						UndoRedoManager.BeginMacro();
						this.actionIsClone = true;
						this.SelectObjects(this.CloneObjects(this.actionObjSel));
					}
					this.BeginAction(this.mouseoverAction);
				}
			}
		}
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			this.drawSelGizmoState = ObjectEditorAction.None;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			this.mouseoverAction = ObjectEditorAction.None;
			this.mouseoverObject = null;
			this.mouseoverSelect = false;
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			this.drawSelGizmoState = ObjectEditorAction.None;

			if (this.actionAllowed)
			{
				if (e.KeyCode == Keys.Menu)
				{
					// Capture the left Alt key, so focus doesn't jump to the menu.
					// We'll need Alt keys right here for those drag-clone actions.
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Delete)
				{
					List<ObjectEditorSelObj> deleteList = this.actionObjSel.ToList();
					this.ClearSelection();
					this.DeleteObjects(deleteList);
				}
				else if (e.KeyCode == Keys.C && e.Control)
				{
					List<ObjectEditorSelObj> cloneList = this.CloneObjects(this.actionObjSel);
					this.SelectObjects(cloneList);
				}
				else if (e.KeyCode == Keys.G)
				{
					if (e.Alt)
					{
						this.SelectObjects(this.CloneObjects(this.actionObjSel));
						e.SuppressKeyPress = true; // Prevent menustrip from getting focused
					}
					this.MoveSelectionToCursor();
				}
				else if (!e.Control && e.KeyCode == Keys.Left)		this.MoveSelectionBy(-Vector3.UnitX);
				else if (!e.Control && e.KeyCode == Keys.Right)		this.MoveSelectionBy(Vector3.UnitX);
				else if (!e.Control && e.KeyCode == Keys.Up)		this.MoveSelectionBy(-Vector3.UnitY);
				else if (!e.Control && e.KeyCode == Keys.Down)		this.MoveSelectionBy(Vector3.UnitY);
				else if (!e.Control && e.KeyCode == Keys.Add)		this.MoveSelectionBy(Vector3.UnitZ);
				else if (!e.Control && e.KeyCode == Keys.Subtract)	this.MoveSelectionBy(-Vector3.UnitZ);
				else if (e.KeyCode == Keys.ShiftKey)				this.UpdateAction();
				else if (e.KeyCode == Keys.ControlKey)				this.UpdateAction();
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.ShiftKey)
			{
				this.actionLockedAxis = ObjectEditorAxisLock.None;
				this.UpdateAction();
			}
			else if (e.KeyCode == Keys.ControlKey)
			{
				this.UpdateAction();
			}

			base.OnKeyUp(e);
		}
		protected override void OnCamActionRequiresCursorChanged(EventArgs e)
		{
			base.OnCamActionRequiresCursorChanged(e);
			this.UpdateAction();
		}
		protected override void OnGotFocus()
		{
			base.OnGotFocus();

			// Re-apply the current selection to trigger a global event focusing on our local selection again.
			if (DualityApp.ExecContext != DualityApp.ExecutionContext.Terminated)
				this.SelectObjects(this.allObjSel);
		}
		protected override void OnLostFocus()
		{
			base.OnLostFocus();
			this.EndAction();
		}

		public override HelpInfo ProvideHoverHelp(Point localPos, ref bool captured)
		{
			if (this.actionAllowed && this.allObjSel.Any())
			{
				return HelpInfo.FromText(CamViewRes.CamView_Help_ObjActions, 
					CamViewRes.CamView_Help_ObjActions_Delete + "\n" +
					CamViewRes.CamView_Help_ObjActions_Clone + "\n" +
					CamViewRes.CamView_Help_ObjActions_EditClone + "\n" +
					CamViewRes.CamView_Help_ObjActions_MoveStep + "\n" +
					CamViewRes.CamView_Help_ObjActions_Focus + "\n" +
					CamViewRes.CamView_Help_ObjActions_AxisLock);
			}
			else
			{
				return base.ProvideHoverHelp(localPos, ref captured);
			}
		}
	}
}
