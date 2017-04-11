using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Resources;
using Duality.Drawing;
using Duality.Cloning;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.Plugins.CamView.UndoRedoActions;
using Duality.Editor.Plugins.CamView.Properties;
using Font = Duality.Resources.Font;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Allows you to edit the collision geometry of a <see cref="RigidBody"/>.
	/// </summary>
	public partial class RigidBodyEditorCamViewState : ObjectEditorCamViewState, IRigidBodyEditorToolEnvironment
	{
		private RigidBodyEditorTool       toolNone       = new NoRigidBodyEditorTool();
		private List<RigidBodyEditorTool> tools          = new List<RigidBodyEditorTool>();
		private RigidBody                 selectedBody   = null;
		private RigidBodyEditorTool       selectedTool   = null;
		private Vector3                   lockedWorldPos = Vector3.Zero;
		private Vector3                   activeWorldPos = Vector3.Zero;
		private Vector2                   activeObjPos   = Vector2.Zero;
		private RigidBodyEditorTool       activeTool     = null;
		private RigidBodyEditorTool       actionTool     = null;
		private ToolStrip                 toolstrip      = null;

		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_RigidBodyEditor_Name; }
		}
		public RigidBodyEditorTool SelectedTool
		{
			get { return this.selectedTool; }
			set
			{
				value = value ?? this.toolNone;

				if (this.selectedTool == value)
					return;

				this.selectedTool = value;
				this.UpdateRigidBodyToolButtons();

				// Invalidate cursor state 
				this.OnMouseLeave(EventArgs.Empty);
				this.OnMouseMove();
			}
		}
		public RigidBody ActiveBody
		{
			get { return this.selectedBody; }
		}
		public Vector2 ActiveBodyPos
		{
			get { return this.activeObjPos; }
		}
		public Vector3 ActiveWorldPos
		{
			get { return this.activeWorldPos; }
		}
		public Vector3 LockedWorldPos
		{
			get { return this.lockedWorldPos; }
			set { this.lockedWorldPos = value; }
		}
		bool IRigidBodyEditorToolEnvironment.IsActionKeyPressed
		{
			get { return (Control.MouseButtons & MouseButtons.Left) != MouseButtons.None; }
		}

		public RigidBodyEditorCamViewState()
		{
			this.SetDefaultActiveLayers(
				typeof(CamViewLayers.BackPlateCamViewLayer),
				typeof(CamViewLayers.RigidBodyJointCamViewLayer),
				typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
			this.SetDefaultObjectVisibility(
				typeof(RigidBody));
		}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();
			
			// Init the available toolset, if not done before
			if (this.tools.Count == 0)
			{
				RigidBodyEditorTool[] availableTools = DualityEditorApp.GetAvailDualityEditorTypes(typeof(RigidBodyEditorTool))
					.Where(t => !t.IsAbstract)
					.Select(t => t.CreateInstanceOf() as RigidBodyEditorTool)
					.NotNull()
					.OrderBy(t => t.SortOrder)
					.ToArray();

				this.toolNone = availableTools.OfType<NoRigidBodyEditorTool>().FirstOrDefault();

				this.tools.AddRange(availableTools);
				foreach (RigidBodyEditorTool tool in this.tools)
				{
					tool.Environment = this;
				}
				this.tools.Remove(this.toolNone);

				this.selectedTool = this.toolNone;
				this.activeTool   = this.toolNone;
				this.actionTool   = this.toolNone;
			}

			// Init the custom tile editing toolbar
			{
				this.View.SuspendLayout();
				this.toolstrip = new ToolStrip();
				this.toolstrip.SuspendLayout();

				this.toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
				this.toolstrip.Name = "toolstrip";
				this.toolstrip.Text = "RigidBody Editor Tools";
				this.toolstrip.Renderer = new Duality.Editor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
				this.toolstrip.BackColor = Color.FromArgb(212, 212, 212);

				foreach (RigidBodyEditorTool tool in this.tools)
				{
					tool.InitToolButton();

					if (tool.ToolButton == null)
						continue;

					tool.ToolButton.Tag = tool.HelpInfo;
					tool.ToolButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
					tool.ToolButton.AutoToolTip = true;
					this.toolstrip.Items.Add(tool.ToolButton);
				}

				this.View.Controls.Add(this.toolstrip);
				this.View.Controls.SetChildIndex(this.toolstrip, this.View.Controls.IndexOf(this.View.ToolbarCamera));
				this.toolstrip.ResumeLayout(true);
				this.View.ResumeLayout(true);
			}

			// Register events
			DualityEditorApp.SelectionChanged		+= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	+= this.EditorForm_ObjectPropertyChanged;

			// Initial update
			this.selectedBody = this.QuerySelectedCollider();
			this.InvalidateSelectionStats();
			this.UpdateRigidBodyToolButtons();

			this.View.ActivateLayer(typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
			this.View.LockLayer(typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();

			// Unregister events
			DualityEditorApp.SelectionChanged		-= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	-= this.EditorForm_ObjectPropertyChanged;
			
			// Cleanup the custom tile editing toolbar
			foreach (RigidBodyEditorTool tool in this.tools)
			{
				tool.DisposeToolButton();
			}
			this.toolstrip.Dispose();
			this.toolstrip = null;

			this.View.UnlockLayer(typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
		}

		protected void UpdateRigidBodyToolButtons()
		{
			bool noActionInProgress = this.actionTool == this.toolNone;
			foreach (RigidBodyEditorTool tool in this.tools)
			{
				if (tool.ToolButton == null) continue;
				tool.ToolButton.Enabled = noActionInProgress && tool.IsAvailable;
				tool.ToolButton.Checked = (this.selectedTool == tool);
			}
		}

		public override ObjectEditorSelObj PickSelObjAt(int x, int y)
		{
			RigidBody pickedCollider = null;
			ShapeInfo pickedShape = null;

			RigidBody[] visibleColliders = this.QueryVisibleColliders()
				.Where(r => !DesignTimeObjectData.Get(r.GameObj).IsLocked)
				.ToArray();
			visibleColliders.StableSort(delegate(RigidBody c1, RigidBody c2) 
			{ 
				return MathF.RoundToInt(1000.0f * (c1.GameObj.Transform.Pos.Z - c2.GameObj.Transform.Pos.Z));
			});

			foreach (RigidBody c in visibleColliders)
			{
				Vector3 worldCoord = this.GetSpaceCoord(new Vector3(x, y, c.GameObj.Transform.Pos.Z));

				// Do a physical picking operation
				pickedShape = this.PickShape(c, worldCoord.Xy);

				// Shape picked.
				if (pickedShape != null)
				{
					pickedCollider = c;
					break;
				}
			}

			if (pickedShape != null) return RigidBodyEditorSelShape.Create(pickedShape);
			if (pickedCollider != null) return new RigidBodyEditorSelBody(pickedCollider);

			return null;
		}
		public override List<ObjectEditorSelObj> PickSelObjIn(int x, int y, int w, int h)
		{
			List<ObjectEditorSelObj> result = new List<ObjectEditorSelObj>();
			
			RigidBody pickedCollider = null;
			ShapeInfo pickedShape = null;

			RigidBody[] visibleColliders = this.QueryVisibleColliders()
				.Where(r => !DesignTimeObjectData.Get(r.GameObj).IsLocked)
				.ToArray();
			visibleColliders.StableSort(delegate(RigidBody c1, RigidBody c2) 
			{ 
				return MathF.RoundToInt(1000.0f * (c1.GameObj.Transform.Pos.Z - c2.GameObj.Transform.Pos.Z));
			});

			// Pick a collider
			foreach (RigidBody c in visibleColliders)
			{
				Vector3 worldCoord = this.GetSpaceCoord(new Vector3(x, y, c.GameObj.Transform.Pos.Z));
				float scale = this.GetScaleAtZ(c.GameObj.Transform.Pos.Z);
				pickedShape = this.PickShapes(c, worldCoord.Xy, new Vector2(w / scale, h / scale)).FirstOrDefault();
				if (pickedShape != null)
				{
					pickedCollider = c;
					result.Add(new RigidBodyEditorSelBody(pickedCollider));
					break;
				}
				else pickedShape = null;
			}

			// Pick shapes
			if (pickedCollider != null)
			{
				Vector3 worldCoord = this.GetSpaceCoord(new Vector3(x, y, pickedCollider.GameObj.Transform.Pos.Z));
				float scale = this.GetScaleAtZ(pickedCollider.GameObj.Transform.Pos.Z);
				List<ShapeInfo> picked = this.PickShapes(pickedCollider, worldCoord.Xy, new Vector2(w / scale, h / scale));
				if (picked.Count > 0) result.AddRange(picked.Select(s => RigidBodyEditorSelShape.Create(s) as ObjectEditorSelObj));
			}

			return result;
		}

		private ShapeInfo PickShape(RigidBody body, Vector2 worldCoord)
		{
			// Special case for Loop- and ChainShapes, because they are by definition unpickable
			Rect worldRect = Rect.Align(Alignment.Center, worldCoord.X, worldCoord.Y, 10.0f, 10.0f);
			foreach (ShapeInfo shape in body.Shapes)
			{
				LoopShapeInfo loop = shape as LoopShapeInfo;
				ChainShapeInfo chain = shape as ChainShapeInfo;
				
				Vector2[] vertices = null;
				if (loop != null) vertices = loop.Vertices;
				if (chain != null) vertices = chain.Vertices;

				if (vertices != null && IsOutlineBoxIntersection(body.GameObj.Transform, vertices, worldRect))
					return shape;
			}

			// Do a physical picking operation
			return body.PickShape(worldCoord);
		}
		private List<ShapeInfo> PickShapes(RigidBody body, Vector2 worldCoord, Vector2 worldSize)
		{
			Rect worldRect = new Rect(worldCoord.X, worldCoord.Y, worldSize.X, worldSize.Y);

			// Do a physical picking operation
			List<ShapeInfo> result = body.PickShapes(worldCoord, worldSize);

			// Special case for Loop- and ChainShapes, because they are by definition unpickable
			foreach (ShapeInfo shape in body.Shapes)
			{
				LoopShapeInfo loop = shape as LoopShapeInfo;
				ChainShapeInfo chain = shape as ChainShapeInfo;
				
				Vector2[] vertices = null;
				if (loop != null) vertices = loop.Vertices;
				if (chain != null) vertices = chain.Vertices;

				if (vertices != null && IsOutlineBoxIntersection(body.GameObj.Transform, vertices, worldRect))
				{
					result.Add(shape);
					continue;
				}
			}

			return result;
		}

		private bool IsOutlineBoxIntersection(Transform transform, Vector2[] vertices, Rect box)
		{
			bool hit = false;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 worldV1 = transform.GetWorldPoint(vertices[i]);
				Vector2 worldV2 = transform.GetWorldPoint(vertices[(i + 1) % vertices.Length]);
				hit = hit || MathF.LinesCross(
					box.TopLeft.X, 
					box.TopLeft.Y, 
					box.TopRight.X, 
					box.TopRight.Y, 
					worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
				hit = hit || MathF.LinesCross(
					box.TopLeft.X, 
					box.TopLeft.Y, 
					box.BottomLeft.X, 
					box.BottomLeft.Y, 
					worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
				hit = hit || MathF.LinesCross(
					box.BottomRight.X, 
					box.BottomRight.Y, 
					box.TopRight.X, 
					box.TopRight.Y, 
					worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
				hit = hit || MathF.LinesCross(
					box.BottomRight.X, 
					box.BottomRight.Y, 
					box.BottomLeft.X, 
					box.BottomLeft.Y, 
					worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
				hit = hit || box.Contains(worldV1) || box.Contains(worldV2);
				if (hit) return true;
			}

			return false;
		}

		void IRigidBodyEditorToolEnvironment.SelectShapes(IEnumerable<ShapeInfo> shapes, SelectMode mode)
		{
			// If we're specifying null or no shapes explicitly, deselect all shapes
			if (mode == SelectMode.Set && (shapes == null || !shapes.Any()))
				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
			// Otherwise, forward to regular object selection
			else
				this.SelectObjects(shapes.Select(s =>  RigidBodyEditorSelShape.Create(s)).NotNull(), mode);
		}
		public override void SelectObjects(IEnumerable<ObjectEditorSelObj> selObjEnum, SelectMode mode = SelectMode.Set)
		{
			base.SelectObjects(selObjEnum, mode);
			if (!selObjEnum.Any()) return;
			
			// Change shape selection
			if (selObjEnum.OfType<RigidBodyEditorSelShape>().Any())
			{
				var shapeQuery = selObjEnum.OfType<RigidBodyEditorSelShape>();
				var distinctShapeQuery = shapeQuery.GroupBy(s => s.Body).First(); // Assure there is only one collider active.
				RigidBodyEditorSelShape[] selShapeArray = distinctShapeQuery.ToArray();

				// First, select the associated Collider
				DualityEditorApp.Select(this, new ObjectSelection(selShapeArray[0].Body.GameObj), SelectMode.Set);
				// Then, select actual ShapeInfos
				DualityEditorApp.Select(this, new ObjectSelection(selShapeArray.Select(s => s.ActualObject)), mode);
			}

			// Change collider selection
			else if (selObjEnum.OfType<RigidBodyEditorSelBody>().Any())
			{
				// Deselect ShapeInfos
				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				// Select Collider
				DualityEditorApp.Select(this, new ObjectSelection(selObjEnum.OfType<RigidBodyEditorSelBody>().Select(s => s.ActualObject)), mode);
			}
		}
		public override void ClearSelection()
		{
			base.ClearSelection();
			DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
		}
		public override void DeleteObjects(IEnumerable<ObjectEditorSelObj> objEnum)
		{
			if (objEnum.OfType<RigidBodyEditorSelShape>().Any())
			{
				ShapeInfo[] selShapes = objEnum.OfType<RigidBodyEditorSelShape>().Select(s => s.ActualObject as ShapeInfo).ToArray();

				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				UndoRedoManager.Do(new DeleteRigidBodyShapeAction(selShapes));
			}
		}
		public override List<ObjectEditorSelObj> CloneObjects(IEnumerable<ObjectEditorSelObj> objEnum)
		{
			if (objEnum == null || !objEnum.Any()) return base.CloneObjects(objEnum);
			List<ObjectEditorSelObj> result = new List<ObjectEditorSelObj>();
			if (objEnum.OfType<RigidBodyEditorSelShape>().Any())
			{
				ShapeInfo[] selShapes = objEnum.OfType<RigidBodyEditorSelShape>().Select(s => (s.ActualObject as ShapeInfo).DeepClone()).ToArray();
				CreateRigidBodyShapeAction cloneAction = new CreateRigidBodyShapeAction(this.selectedBody, selShapes);
				UndoRedoManager.Do(cloneAction);
				result.AddRange(cloneAction.Result.Select(s => RigidBodyEditorSelShape.Create(s)));
			}
			return result;
		}

		private void BeginToolAction(RigidBodyEditorTool action)
		{
			if (this.actionTool == action) return;
			if (this.actionTool != this.toolNone)
				this.EndToolAction();

			this.MouseActionAllowed = false;
			this.selectedBody.BeginUpdateBodyShape();
			DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);

			this.lockedWorldPos = this.activeWorldPos;

			this.actionTool = action;
			this.actionTool.BeginAction();

			this.UpdateRigidBodyToolButtons();
			this.Invalidate();

			if (Sandbox.State == SandboxState.Playing)
				Sandbox.Pause();
		}
		private void UpdateToolAction()
		{
			if (this.actionTool == this.toolNone) return;
			
			this.actionTool.UpdateAction();

			if (this.selectedBody != null)
				this.selectedBody.SynchronizeBodyShape();
		}
		private void EndToolAction()
		{
			if (this.actionTool == this.toolNone) return;
			
			this.actionTool.EndAction();
			this.actionTool = this.toolNone;

			this.MouseActionAllowed = true;
			this.selectedBody.EndUpdateBodyShape();
			this.UpdateRigidBodyToolButtons();
			this.Invalidate();
			UndoRedoManager.Finish();

			// Since our tool actions are designed to block out other actions,
			// by default deselect each tool after using it.
			this.SelectedTool = null;
		}
		void IRigidBodyEditorToolEnvironment.EndToolAction()
		{
			this.EndToolAction();
		}

		private void ApplyCursor()
		{
			this.Cursor = this.activeTool.ActionCursor ?? CursorHelper.Arrow;
		}
		private void UpdateActiveState()
		{
			// Update active world and local positions
			GameObject selGameObj = this.selectedBody != null ? this.selectedBody.GameObj : null;
			Transform selTransform = selGameObj != null ? selGameObj.Transform : null;
			Point mousePos = this.PointToClient(Cursor.Position);
			if (selTransform != null)
				this.activeWorldPos = this.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, selTransform.Pos.Z));
			else
				this.activeWorldPos = Vector3.Zero;
			
			// Snap active position to user guides
			if ((this.SnapToUserGuides & UserGuideType.Position) != UserGuideType.None)
				this.activeWorldPos = this.EditingUserGuide.SnapPosition(this.activeWorldPos);

			// Snap active position to active axis locks
			this.activeWorldPos = this.ApplyAxisLock(this.activeWorldPos, this.lockedWorldPos);

			// Calculate object-local active position
			if (selTransform != null)
				this.activeObjPos = selTransform.GetLocalPoint(this.activeWorldPos).Xy;
			else
				this.activeObjPos = Vector2.Zero;

			// If an action is currently being performed, that action will always be the active tool
			if (this.actionTool != this.toolNone)
				this.activeTool = this.actionTool;
			// Otherwise, we'll go with the user-selected tool
			else
				this.activeTool = this.selectedTool;

			// Apply our own cursor. We'll need to do this continuously, since the object editor base
			// class will inject its regular move-scale-select cursors.
			if (this.activeTool != this.toolNone)
				this.ApplyCursor();
		}

		protected IEnumerable<RigidBody> QueryVisibleColliders()
		{
			var allColliders = Scene.Current.FindComponents<RigidBody>();
			return allColliders.Where(r => 
				r.Active && 
				!DesignTimeObjectData.Get(r.GameObj).IsHidden && 
				this.IsCoordInView(r.GameObj.Transform.Pos, r.BoundRadius));
		}
		protected RigidBody QuerySelectedCollider()
		{
			return 
				DualityEditorApp.Selection.Components.OfType<RigidBody>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<RigidBody>().FirstOrDefault();
		}

		protected override void OnCollectStateWorldOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectStateWorldOverlayDrawcalls(canvas);

			GameObject selGameObj = this.selectedBody != null ? this.selectedBody.GameObj : null;
			Transform selTransform = selGameObj != null ? selGameObj.Transform : null;
			if (selTransform == null) return;

			// Draw axis locks when applied
			if (this.actionTool != this.toolNone)
			{
				this.DrawLockedAxes(canvas, 
					this.activeWorldPos.X, 
					this.activeWorldPos.Y, 
					this.activeWorldPos.Z, 
					this.selectedBody.BoundRadius * 4);
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			
			// Check for tool-related keys
			foreach (RigidBodyEditorTool tool in this.tools)
			{
				if (Control.ModifierKeys == Keys.None && 
					tool.ShortcutKey == e.KeyCode && 
					tool.IsAvailable)
				{
					this.SelectedTool = tool;
					e.Handled = true;
					break;
				}
			}

			// Make sure our custom action is updated accordingto axis locks
			if (e.KeyCode == Keys.ShiftKey && this.actionTool != this.toolNone)
				this.OnMouseMove();
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			// Make sure our custom action is updated accordingto axis locks
			if (e.KeyCode == Keys.ShiftKey && this.actionTool != this.toolNone)
				this.OnMouseMove();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			if (e.Button == MouseButtons.Left)
			{
				// Because selection events may change the currently active tool,
				// start by agreeing on what tool we're dealing with in this mouse event.
				RigidBodyEditorTool proposedAction = this.activeTool;

				// If there is no tool active, don't do selection changes or begin an action
				if (proposedAction == this.toolNone) return;

				if (this.actionTool == proposedAction)
				{
					// Notify an already active action that the action key has
					// been pressed again. This can be used for "action checkpoints",
					// such as finishing the current vertex and adding another one.
					this.actionTool.OnActionKeyPressed();
				}
				else
				{
					// Begin a new action with the proposed action tool
					this.BeginToolAction(proposedAction);
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				// A right-click signals the end of our current tool action
				this.EndToolAction();
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// Update what is considered the "active" state, e.g. active tools and data
			this.UpdateActiveState();

			// Update the current tools active action
			this.UpdateToolAction();
		}
		protected override void OnLostFocus()
		{
			base.OnLostFocus();
			this.EndToolAction();
		}
		protected override void OnBeginAction(ObjectEditorAction action)
		{
			base.OnBeginAction(action);

			bool shapeAction = 
				action != ObjectEditorAction.RectSelect && 
				action != ObjectEditorAction.None;
			if (this.selectedBody != null && shapeAction)
			{
				this.selectedBody.BeginUpdateBodyShape();
				this.EditingUserGuide.SnapPosOrigin = this.selectedBody.GameObj.Transform.Pos;
				this.EditingUserGuide.SnapScaleOrigin = Vector3.One * this.selectedBody.GameObj.Transform.Scale;
			}
		}
		protected override void OnEndAction(ObjectEditorAction action)
		{
			base.OnEndAction(action);

			bool shapeAction = 
				action != ObjectEditorAction.RectSelect && 
				action != ObjectEditorAction.None;
			if (this.selectedBody != null && shapeAction)
				this.selectedBody.EndUpdateBodyShape();

			this.EditingUserGuide.SnapPosOrigin = Vector3.Zero;
			this.EditingUserGuide.SnapScaleOrigin = Vector3.One;
		}
		protected override void PostPerformAction(IEnumerable<ObjectEditorSelObj> selObjEnum, ObjectEditorAction action)
		{
			base.PostPerformAction(selObjEnum, action);
			RigidBodyEditorSelShape[] selShapeArray = selObjEnum.OfType<RigidBodyEditorSelShape>().ToArray();

			// Update the body directly after modifying it
			if (this.selectedBody != null) this.selectedBody.SynchronizeBodyShape();

			// Notify property changes
			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.selectedBody),
				ReflectionInfo.Property_RigidBody_Shapes);
			DualityEditorApp.NotifyObjPropChanged(this, new ObjectSelection(selShapeArray.Select(s => s.ActualObject)));
		}
		protected override string UpdateStatusText()
		{
			if (this.activeTool != this.toolNone)
				return this.activeTool.Name;
			else
				return base.UpdateStatusText();
		}
		protected override string UpdateActionText(ref Vector2 actionTextPos)
		{
			string toolText = this.activeTool.GetActionText();
			if (!string.IsNullOrEmpty(toolText))
				return toolText;
			else
				return base.UpdateActionText(ref actionTextPos);
		}
	
		private void EditorForm_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (this.selectedBody != null)
			{
				if (this.selectedBody.Disposed || this.selectedBody.GameObj == null || this.selectedBody.GameObj.Disposed)
				{
					this.ClearSelection();
					return;
				}
			}
			if (e.Objects.Any(o => o is Transform || o is RigidBody || o is ShapeInfo))
			{
				// Applying its Prefab invalidates a Collider's ShapeInfos: Deselect them.
				if (e is PrefabAppliedEventArgs)
					DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				else
				{
					foreach (RigidBodyEditorSelShape selShape in this.allObjSel.OfType<RigidBodyEditorSelShape>())
						selShape.UpdateShapeStats();
					this.InvalidateSelectionStats();
					this.Invalidate();
				}
				this.UpdateRigidBodyToolButtons();
			}
		}
		private void EditorForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.SameObjects) return;
			if (!e.AffectedCategories.HasFlag(ObjectSelection.Category.GameObjCmp) &&
				!e.AffectedCategories.HasFlag(ObjectSelection.Category.Other))
				return;

			// Collider selection changed
			if ((e.AffectedCategories & ObjectSelection.Category.GameObjCmp) != ObjectSelection.Category.None)
			{
				RigidBody newBody = this.QuerySelectedCollider();
				if (newBody != this.selectedBody)
					this.EndToolAction();

				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				this.selectedBody = newBody;
			}
			// Other selection changed
			if ((e.AffectedCategories & ObjectSelection.Category.Other) != ObjectSelection.Category.None)
			{
				if (e.Current.OfType<ShapeInfo>().Any())
					this.allObjSel = e.Current.OfType<ShapeInfo>().Select(s => RigidBodyEditorSelShape.Create(s) as ObjectEditorSelObj).ToList();
				else
					this.allObjSel = new List<ObjectEditorSelObj>();

				// Update indirect object selection
				this.indirectObjSel.Clear();
				// Update (parent-free) action object selection
				this.actionObjSel = this.allObjSel.ToList();
			}

			this.InvalidateSelectionStats();
			this.UpdateRigidBodyToolButtons();
			this.Invalidate();
		}
	}
}
