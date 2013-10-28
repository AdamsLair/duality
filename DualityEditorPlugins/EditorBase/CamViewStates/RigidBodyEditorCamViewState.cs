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
using Duality.ColorFormat;
using Font = Duality.Resources.Font;

using DualityEditor;
using DualityEditor.CorePluginInterface;
using DualityEditor.Forms;

using EditorBase.UndoRedoActions;
using EditorBase.PluginRes;

using OpenTK;

namespace EditorBase.CamViewStates
{
	public partial class RigidBodyEditorCamViewState : CamViewState
	{
		public static readonly Cursor ArrowCreateCircle		= CursorHelper.CreateCursor(EditorBaseResCache.CursorArrowCreateCircle, 0, 0);
		public static readonly Cursor ArrowCreatePolygon	= CursorHelper.CreateCursor(EditorBaseResCache.CursorArrowCreatePolygon, 0, 0);
		public static readonly Cursor ArrowCreateEdge		= CursorHelper.CreateCursor(EditorBaseResCache.CursorArrowCreateEdge, 0, 0);
		public static readonly Cursor ArrowCreateLoop		= CursorHelper.CreateCursor(EditorBaseResCache.CursorArrowCreateLoop, 0, 0);

		private enum CursorState
		{
			Normal,
			CreateCircle,
			CreatePolygon,
		//	CreateEdge,
			CreateLoop
		}

		private	CursorState			mouseState			= CursorState.Normal;
		private	bool				createAction		= false;
		private	int					createPolyIndex		= 0;
		private	RigidBody			selectedBody		= null;
		private	ToolStrip			toolstrip			= null;
		private	ToolStripButton		toolCreateCircle	= null;
		private	ToolStripButton		toolCreatePoly		= null;
	//	private	ToolStripButton		toolCreateEdge		= null;
		private	ToolStripButton		toolCreateLoop		= null;

		public override string StateName
		{
			get { return PluginRes.EditorBaseRes.CamViewState_RigidBodyEditor_Name; }
		}

		public RigidBodyEditorCamViewState()
		{
			this.SetDefaultActiveLayers(
				typeof(CamViewLayers.RigidBodyJointCamViewLayer),
				typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
		}

		protected internal override void OnEnterState()
		{
			base.OnEnterState();

			// Init GUI
			this.View.SuspendLayout();
			this.toolstrip = new ToolStrip();
			this.toolstrip.SuspendLayout();

			this.toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolstrip.Name = "toolstrip";
			this.toolstrip.Text = "Collider Editor Tools";

			this.toolCreateCircle = new ToolStripButton("Create Circle Shape (C)", PluginRes.EditorBaseResCache.IconCmpCircleCollider, this.toolCreateCircle_Clicked);
			this.toolCreateCircle.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolCreateCircle.AutoToolTip = true;
			this.toolstrip.Items.Add(this.toolCreateCircle);

			this.toolCreatePoly = new ToolStripButton("Create Polygon Shape (P)", PluginRes.EditorBaseResCache.IconCmpRectCollider, this.toolCreatePoly_Clicked);
			this.toolCreatePoly.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolCreatePoly.AutoToolTip = true;
			this.toolstrip.Items.Add(this.toolCreatePoly);

		//	this.toolCreateEdge = new ToolStripButton("Create Edge Shape (E)", PluginRes.EditorBaseResCache.IconCmpEdgeCollider, this.toolCreateEdge_Clicked);
		//	this.toolCreateEdge.DisplayStyle = ToolStripItemDisplayStyle.Image;
		//	this.toolCreateEdge.AutoToolTip = true;
		//	this.toolstrip.Items.Add(this.toolCreateEdge);

			this.toolCreateLoop = new ToolStripButton("Create Loop Shape (L)", PluginRes.EditorBaseResCache.IconCmpLoopCollider, this.toolCreateLoop_Clicked);
			this.toolCreateLoop.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolCreateLoop.AutoToolTip = true;
			this.toolstrip.Items.Add(this.toolCreateLoop);

			this.toolstrip.Renderer = new DualityEditor.Controls.ToolStrip.DualitorToolStripProfessionalRenderer();
			this.toolstrip.BackColor = Color.FromArgb(212, 212, 212);

			this.View.Controls.Add(this.toolstrip);
			this.View.Controls.SetChildIndex(this.toolstrip, this.View.Controls.IndexOf(this.View.ToolbarCamera));
			this.toolstrip.ResumeLayout(true);
			this.View.ResumeLayout(true);

			// Register events
			DualityEditorApp.SelectionChanged		+= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	+= this.EditorForm_ObjectPropertyChanged;

			// Initial update
			this.selectedBody = this.QuerySelectedCollider();
			this.InvalidateSelectionStats();
			this.UpdateToolbar();

			this.View.LockLayer(typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
		}
		protected internal override void OnLeaveState()
		{
			base.OnLeaveState();

			// Unregister events
			DualityEditorApp.SelectionChanged		-= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	-= this.EditorForm_ObjectPropertyChanged;

			// Cleanup GUI
			this.toolstrip.Dispose();
			this.toolCreateCircle.Dispose();

			this.toolstrip = null;
			this.toolCreateCircle = null;

			this.View.UnlockLayer(typeof(CamViewLayers.RigidBodyShapeCamViewLayer));
		}

		protected void UpdateToolbar()
		{
			this.toolCreateCircle.Enabled = this.selectedBody != null && this.mouseState == CursorState.Normal;
			this.toolCreatePoly.Enabled = this.toolCreateCircle.Enabled;
		//	this.toolCreateEdge.Enabled = this.toolCreateCircle.Enabled;
			this.toolCreateLoop.Enabled = this.toolCreateCircle.Enabled && this.selectedBody.BodyType == BodyType.Static;
		}

		public override CamViewState.SelObj PickSelObjAt(int x, int y)
		{
			RigidBody pickedCollider = null;
			ShapeInfo pickedShape = null;

			RigidBody[] visibleColliders = this.QueryVisibleColliders()
				.Where(r => !CorePluginRegistry.GetDesignTimeData(r.GameObj).IsLocked)
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

			if (pickedShape != null) return SelShape.Create(pickedShape);
			if (pickedCollider != null) return new SelBody(pickedCollider);

			return null;
		}
		public override List<CamViewState.SelObj> PickSelObjIn(int x, int y, int w, int h)
		{
			List<CamViewState.SelObj> result = new List<SelObj>();
			
			RigidBody pickedCollider = null;
			ShapeInfo pickedShape = null;

			RigidBody[] visibleColliders = this.QueryVisibleColliders()
				.Where(r => !CorePluginRegistry.GetDesignTimeData(r.GameObj).IsLocked)
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
					result.Add(new SelBody(pickedCollider));
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
				if (picked.Count > 0) result.AddRange(picked.Select(s => SelShape.Create(s) as SelObj));
			}

			return result;
		}

		private ShapeInfo PickShape(RigidBody body, Vector2 worldCoord)
		{
			// Special case for EdgeShapes, because they are by definition unpickable
		//	foreach (EdgeShapeInfo edge in body.Shapes.OfType<EdgeShapeInfo>())
		//	{
		//		Vector2 worldV1 = body.GameObj.Transform.GetWorldPoint(edge.VertexStart);
		//		Vector2 worldV2 = body.GameObj.Transform.GetWorldPoint(edge.VertexEnd);
		//		float dist = MathF.PointLineDistance(worldCoord.X, worldCoord.Y, worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
		//		if (dist < 5.0f) return edge;
		//	}

			// Special case for LoopShapes, because they are by definition unpickable
			foreach (LoopShapeInfo loop in body.Shapes.OfType<LoopShapeInfo>())
			{
				for (int i = 0; i < loop.Vertices.Length; i++)
				{
					Vector2 worldV1 = body.GameObj.Transform.GetWorldPoint(loop.Vertices[i]);
					Vector2 worldV2 = body.GameObj.Transform.GetWorldPoint(loop.Vertices[(i + 1) % loop.Vertices.Length]);
					float dist = MathF.PointLineDistance(worldCoord.X, worldCoord.Y, worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
					if (dist < 5.0f) return loop;
				}
			}

			// Do a physical picking operation
			return body.PickShape(worldCoord);
		}
		private List<ShapeInfo> PickShapes(RigidBody body, Vector2 worldCoord, Vector2 worldSize)
		{
			Rect worldRect = new Rect(worldCoord.X, worldCoord.Y, worldSize.X, worldSize.Y);

			// Do a physical picking operation
			List<ShapeInfo> result = body.PickShapes(worldCoord, worldSize);

			// Special case for EdgeShapes, because they are by definition unpickable
			//foreach (EdgeShapeInfo edge in body.Shapes.OfType<EdgeShapeInfo>())
			//{
			//    Vector2 worldV1 = body.GameObj.Transform.GetWorldPoint(edge.VertexStart);
			//    Vector2 worldV2 = body.GameObj.Transform.GetWorldPoint(edge.VertexEnd);
			//    bool hit = false;
			//    hit = hit || MathF.LinesCross(
			//        worldRect.TopLeft.X, 
			//        worldRect.TopLeft.Y, 
			//        worldRect.TopRight.X, 
			//        worldRect.TopRight.Y, 
			//        worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
			//    hit = hit || MathF.LinesCross(
			//        worldRect.TopLeft.X, 
			//        worldRect.TopLeft.Y, 
			//        worldRect.BottomLeft.X, 
			//        worldRect.BottomLeft.Y, 
			//        worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
			//    hit = hit || MathF.LinesCross(
			//        worldRect.BottomRight.X, 
			//        worldRect.BottomRight.Y, 
			//        worldRect.TopRight.X, 
			//        worldRect.TopRight.Y, 
			//        worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
			//    hit = hit || MathF.LinesCross(
			//        worldRect.BottomRight.X, 
			//        worldRect.BottomRight.Y, 
			//        worldRect.BottomLeft.X, 
			//        worldRect.BottomLeft.Y, 
			//        worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
			//    hit = hit || worldRect.Contains(worldV1) || worldRect.Contains(worldV2);
			//    if (hit)
			//    {
			//        result.Add(edge);
			//        continue;
			//    }
			//}

			// Special case for LoopShapes, because they are by definition unpickable
			foreach (LoopShapeInfo loop in body.Shapes.OfType<LoopShapeInfo>())
			{
				bool hit = false;
				for (int i = 0; i < loop.Vertices.Length; i++)
				{
					Vector2 worldV1 = body.GameObj.Transform.GetWorldPoint(loop.Vertices[i]);
					Vector2 worldV2 = body.GameObj.Transform.GetWorldPoint(loop.Vertices[(i + 1) % loop.Vertices.Length]);
					hit = hit || MathF.LinesCross(
						worldRect.TopLeft.X, 
						worldRect.TopLeft.Y, 
						worldRect.TopRight.X, 
						worldRect.TopRight.Y, 
						worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
					hit = hit || MathF.LinesCross(
						worldRect.TopLeft.X, 
						worldRect.TopLeft.Y, 
						worldRect.BottomLeft.X, 
						worldRect.BottomLeft.Y, 
						worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
					hit = hit || MathF.LinesCross(
						worldRect.BottomRight.X, 
						worldRect.BottomRight.Y, 
						worldRect.TopRight.X, 
						worldRect.TopRight.Y, 
						worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
					hit = hit || MathF.LinesCross(
						worldRect.BottomRight.X, 
						worldRect.BottomRight.Y, 
						worldRect.BottomLeft.X, 
						worldRect.BottomLeft.Y, 
						worldV1.X, worldV1.Y, worldV2.X, worldV2.Y);
					hit = hit || worldRect.Contains(worldV1) || worldRect.Contains(worldV2);
					if (hit) break;
				}
				if (hit)
				{
					result.Add(loop);
					continue;
				}
			}

			return result;
		}

		public override void SelectObjects(IEnumerable<CamViewState.SelObj> selObjEnum, SelectMode mode = SelectMode.Set)
		{
			base.SelectObjects(selObjEnum, mode);
			if (!selObjEnum.Any()) return;
			
			// Change shape selection
			if (selObjEnum.OfType<SelShape>().Any())
			{
				var shapeQuery = selObjEnum.OfType<SelShape>();
				var distinctShapeQuery = shapeQuery.GroupBy(s => s.Body).First(); // Assure there is only one collider active.
				SelShape[] selShapeArray = distinctShapeQuery.ToArray();

				// First, select the associated Collider
				DualityEditorApp.Select(this, new ObjectSelection(selShapeArray[0].Body.GameObj), SelectMode.Set);
				// Then, select actual ShapeInfos
				DualityEditorApp.Select(this, new ObjectSelection(selShapeArray.Select(s => s.ActualObject)), mode);
			}

			// Change collider selection
			else if (selObjEnum.OfType<SelBody>().Any())
			{
				// Deselect ShapeInfos
				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				// Select Collider
				DualityEditorApp.Select(this, new ObjectSelection(selObjEnum.OfType<SelBody>().Select(s => s.ActualObject)), mode);
			}
		}
		public override void ClearSelection()
		{
			base.ClearSelection();
			//DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp | ObjectSelection.Category.Other);
			DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
		}
		public override void DeleteObjects(IEnumerable<CamViewState.SelObj> objEnum)
		{
			if (objEnum.OfType<SelShape>().Any())
			{
				ShapeInfo[] selShapes = objEnum.OfType<SelShape>().Select(s => s.ActualObject as ShapeInfo).ToArray();

				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				UndoRedoManager.Do(new DeleteRigidBodyShapeAction(selShapes));
			}
		}
		public override List<CamViewState.SelObj> CloneObjects(IEnumerable<CamViewState.SelObj> objEnum)
		{
			if (objEnum == null || !objEnum.Any()) return base.CloneObjects(objEnum);
			List<SelObj> result = new List<SelObj>();
			if (objEnum.OfType<SelShape>().Any())
			{
				ShapeInfo[] selShapes = objEnum.OfType<SelShape>().Select(s => (s.ActualObject as ShapeInfo).Clone()).ToArray();
				CreateRigidBodyShapeAction cloneAction = new CreateRigidBodyShapeAction(this.selectedBody, selShapes);
				UndoRedoManager.Do(cloneAction);
				result.AddRange(cloneAction.Result.Select(s => SelShape.Create(s)));
			}
			return result;
		}

		private void EnterCursorState(CursorState state)
		{
			if (this.mouseState != CursorState.Normal)
				this.LeaveCursorState();

			this.mouseState = state;
			this.createPolyIndex = 0;
			this.selectedBody.BeginUpdateBodyShape();
			this.MouseActionAllowed = false;
			this.UpdateToolbar();
			this.UpdateCursorImage();
			this.Invalidate();

			if (Sandbox.State == SandboxState.Playing)
				Sandbox.Pause();
			DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
		}
		private void LeaveCursorState()
		{
			if (this.mouseState == CursorState.Normal)
				return;

			this.mouseState = CursorState.Normal;
			this.MouseActionAllowed = true;
			this.selectedBody.EndUpdateBodyShape();
			this.UpdateToolbar();
			this.Invalidate();
		}
		private void UpdateCursorImage()
		{
			switch (this.mouseState)
			{
				default:						this.Cursor = CursorHelper.Arrow;	break;
				case CursorState.CreatePolygon:	this.Cursor = ArrowCreatePolygon;	break;
				case CursorState.CreateLoop:	this.Cursor = ArrowCreateLoop;		break;
				case CursorState.CreateCircle:	this.Cursor = ArrowCreateCircle;	break;
			}
		}

		protected IEnumerable<RigidBody> QueryVisibleColliders()
		{
			var allColliders = Scene.Current.FindComponents<RigidBody>();
			return allColliders.Where(r => 
				r.Active && 
				!CorePluginRegistry.GetDesignTimeData(r.GameObj).IsHidden && 
				this.IsCoordInView(r.GameObj.Transform.Pos, r.BoundRadius));
		}
		protected RigidBody QuerySelectedCollider()
		{
			return 
				DualityEditorApp.Selection.Components.OfType<RigidBody>().FirstOrDefault() ?? 
				DualityEditorApp.Selection.GameObjects.GetComponents<RigidBody>().FirstOrDefault();
		}
		protected bool RendererFilter(ICmpRenderer r)
		{
			GameObject obj = (r as Component).GameObj;
			if (obj.RigidBody == null || !(r as Component).Active) return false;

			DesignTimeObjectData data = CorePluginRegistry.GetDesignTimeData(obj);
			return !data.IsHidden;
		}

		protected override void OnCollectStateDrawcalls(Canvas canvas)
		{
			base.OnCollectStateDrawcalls(canvas);

			GameObject selGameObj = this.selectedBody != null ? this.selectedBody.GameObj : null;
			Transform selTransform = selGameObj != null ? selGameObj.Transform : null;
			if (selTransform == null) return;

			if (this.mouseState == CursorState.CreatePolygon)
			{
				SelPolyShape selPolyShape = this.allObjSel.OfType<SelPolyShape>().FirstOrDefault();
				if (selPolyShape != null)
				{
					PolyShapeInfo polyShape = selPolyShape.ActualObject as PolyShapeInfo;
					Vector2 lockedPos = this.createPolyIndex > 0 ? polyShape.Vertices[this.createPolyIndex - 1] : Vector2.Zero;
					Vector3 lockedPosWorld = selTransform.GetWorldPoint(new Vector3(lockedPos));
					this.DrawLockedAxes(canvas, lockedPosWorld.X, lockedPosWorld.Y, lockedPosWorld.Z, polyShape.AABB.BoundingRadius * 4);
				}
			}
			else if (this.mouseState == CursorState.CreateLoop)
			{
				SelLoopShape selLoopShape = this.allObjSel.OfType<SelLoopShape>().FirstOrDefault();
				if (selLoopShape != null)
				{
					LoopShapeInfo loopShape = selLoopShape.ActualObject as LoopShapeInfo;
					Vector2 lockedPos = this.createPolyIndex > 0 ? loopShape.Vertices[this.createPolyIndex - 1] : Vector2.Zero;
					Vector3 lockedPosWorld = selTransform.GetWorldPoint(new Vector3(lockedPos));
					this.DrawLockedAxes(canvas, lockedPosWorld.X, lockedPosWorld.Y, lockedPosWorld.Z, loopShape.AABB.BoundingRadius * 4);
				}
			}
		}
		protected override void OnCurrentCameraChanged(CamView.CameraChangedEventArgs e)
		{
			base.OnCurrentCameraChanged(e);

			if (e.PreviousCamera != null) e.PreviousCamera.RemoveEditorRendererFilter(this.RendererFilter);
			if (e.NextCamera != null) e.NextCamera.AddEditorRendererFilter(this.RendererFilter);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (Control.ModifierKeys == Keys.None)
			{
				if (e.KeyCode == Keys.C && this.toolCreateCircle.Enabled)
					this.toolCreateCircle_Clicked(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.P && this.toolCreatePoly.Enabled)
					this.toolCreatePoly_Clicked(this, EventArgs.Empty);
				else if (e.KeyCode == Keys.L && this.toolCreateLoop.Enabled)
					this.toolCreateLoop_Clicked(this, EventArgs.Empty);
			}

			// Make sure our custom action is updated accordingto axis locks
			if (e.KeyCode == Keys.ShiftKey && this.mouseState != CursorState.Normal)
				this.OnMouseMove();
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);

			// Make sure our custom action is updated accordingto axis locks
			if (e.KeyCode == Keys.ShiftKey && this.mouseState != CursorState.Normal)
				this.OnMouseMove();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			GameObject selGameObj = this.selectedBody != null ? this.selectedBody.GameObj : null;
			Transform selTransform = selGameObj != null ? selGameObj.Transform : null;
			if (selTransform == null) return;

			Vector3 spaceCoord = this.GetSpaceCoord(new Vector3(e.X, e.Y, selTransform.Pos.Z));
			Vector2 localPos = selTransform.GetLocalPoint(spaceCoord).Xy;

			if (this.mouseState == CursorState.CreateCircle)
			{
				#region CreateCircle
				if (e.Button == MouseButtons.Left)
				{
					CircleShapeInfo newShape = new CircleShapeInfo(1.0f, localPos, 1.0f);

					UndoRedoManager.BeginMacro();
					UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.selectedBody, newShape));

					this.createAction = true;
					this.LeaveCursorState();
					this.SelectObjects(new[] { SelShape.Create(newShape) });
					this.BeginAction(ObjectAction.Scale);
				}
				else if (e.Button == MouseButtons.Right)
				{
					this.LeaveCursorState();
				}
				#endregion
			}
			else if (this.mouseState == CursorState.CreatePolygon)
			{
				#region CreatePolygon
				if (e.Button == MouseButtons.Left)
				{
					bool success = false;
					if (!this.allObjSel.Any(sel => sel is SelPolyShape))
					{
						PolyShapeInfo newShape = new PolyShapeInfo(new Vector2[] { localPos, localPos, localPos }, 1.0f);
						UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.selectedBody, newShape));
						this.SelectObjects(new[] { SelShape.Create(newShape) });
						this.createPolyIndex++;
					}
					else
					{
						SelPolyShape selPolyShape = this.allObjSel.OfType<SelPolyShape>().First();
						PolyShapeInfo polyShape = selPolyShape.ActualObject as PolyShapeInfo;
						if (this.createPolyIndex <= 2 || MathF.IsPolygonConvex(polyShape.Vertices))
						{
							Vector2 lockedPos = this.createPolyIndex > 0 ? polyShape.Vertices[this.createPolyIndex - 1] : Vector2.Zero;
							MathF.TransformCoord(ref lockedPos.X, ref lockedPos.Y, selTransform.Angle);
							MathF.TransformCoord(ref localPos.X, ref localPos.Y, selTransform.Angle);
							localPos = this.ApplyAxisLock(localPos, lockedPos);
							MathF.TransformCoord(ref localPos.X, ref localPos.Y, -selTransform.Angle);

							if (polyShape.Vertices.Length < PolyShapeInfo.MaxVertices)
							{
								List<Vector2> vertices = polyShape.Vertices.ToList();

								vertices[this.createPolyIndex] = localPos;
								if (this.createPolyIndex >= vertices.Count - 1)
									vertices.Add(localPos);

								polyShape.Vertices = vertices.ToArray();
								selPolyShape.UpdatePolyStats();
								this.createPolyIndex++;
							}
							else
							{
								Vector2[] vertices = polyShape.Vertices;

								vertices[this.createPolyIndex] = localPos;
								polyShape.Vertices = vertices;
								selPolyShape.UpdatePolyStats();

								this.LeaveCursorState();
							}
						}
					}

					if (success)
					{
						DualityEditorApp.NotifyObjPropChanged(this,
							new ObjectSelection(this.selectedBody),
							ReflectionInfo.Property_RigidBody_Shapes);
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (this.allObjSel.Any(sel => sel is SelPolyShape))
					{
						SelPolyShape selPolyShape = this.allObjSel.OfType<SelPolyShape>().First();
						PolyShapeInfo polyShape = selPolyShape.ActualObject as PolyShapeInfo;
						List<Vector2> vertices = polyShape.Vertices.ToList();

						vertices.RemoveAt(this.createPolyIndex);
						if (vertices.Count < 3 || this.createPolyIndex < 2)
						{
							this.DeleteObjects(new SelPolyShape[] { selPolyShape });
						}
						else
						{
							polyShape.Vertices = vertices.ToArray();
							selPolyShape.UpdatePolyStats();
						}

						DualityEditorApp.NotifyObjPropChanged(this,
							new ObjectSelection(this.selectedBody),
							ReflectionInfo.Property_RigidBody_Shapes);
					}

					this.LeaveCursorState();
				}
				#endregion
			}
			else if (this.mouseState == CursorState.CreateLoop)
			{
				#region CreateLoop
				if (e.Button == MouseButtons.Left)
				{
					bool success = false;
					if (!this.allObjSel.Any(sel => sel is SelLoopShape))
					{
						LoopShapeInfo newShape = new LoopShapeInfo(new Vector2[] { localPos, localPos + Vector2.UnitX, localPos + Vector2.One });
						UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.selectedBody, newShape));
						this.SelectObjects(new[] { SelShape.Create(newShape) });
						success = true;
					}
					else
					{
						SelLoopShape selPolyShape = this.allObjSel.OfType<SelLoopShape>().First();
						LoopShapeInfo polyShape = selPolyShape.ActualObject as LoopShapeInfo;
						List<Vector2> vertices = polyShape.Vertices.ToList();
						Vector2 lockedPos = this.createPolyIndex > 0 ? vertices[this.createPolyIndex - 1] : Vector2.Zero;
						MathF.TransformCoord(ref lockedPos.X, ref lockedPos.Y, selTransform.Angle);
						MathF.TransformCoord(ref localPos.X, ref localPos.Y, selTransform.Angle);
						localPos = this.ApplyAxisLock(localPos, lockedPos);
						MathF.TransformCoord(ref localPos.X, ref localPos.Y, -selTransform.Angle);

						vertices[this.createPolyIndex] = localPos;
						if (this.createPolyIndex >= vertices.Count - 1)
							vertices.Add(localPos);

						polyShape.Vertices = vertices.ToArray();
						selPolyShape.UpdateLoopStats();
						success = true;
					}

					if (success)
					{
						this.createPolyIndex++;
						DualityEditorApp.NotifyObjPropChanged(this,
							new ObjectSelection(this.selectedBody),
							ReflectionInfo.Property_RigidBody_Shapes);
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (this.allObjSel.Any(sel => sel is SelLoopShape))
					{
						SelLoopShape selPolyShape = this.allObjSel.OfType<SelLoopShape>().First();
						LoopShapeInfo polyShape = selPolyShape.ActualObject as LoopShapeInfo;
						List<Vector2> vertices = polyShape.Vertices.ToList();

						vertices.RemoveAt(this.createPolyIndex);
						if (vertices.Count < 3 || this.createPolyIndex < 2)
						{
							this.DeleteObjects(new SelLoopShape[] { selPolyShape });
						}
						else
						{
							polyShape.Vertices = vertices.ToArray();
							selPolyShape.UpdateLoopStats();
						}

						DualityEditorApp.NotifyObjPropChanged(this,
							new ObjectSelection(this.selectedBody),
							ReflectionInfo.Property_RigidBody_Shapes);
					}

					this.LeaveCursorState();
				}
				#endregion
			}
			//else if (this.mouseState == CursorState.CreateEdge)
			//{
			//    #region CreateEdge
			//    if (e.Button == MouseButtons.Left)
			//    {
			//        bool success = false;
			//        if (!this.allObjSel.Any(sel => sel is SelEdgeShape))
			//        {
			//            EdgeShapeInfo newShape = new EdgeShapeInfo(localPos, localPos + Vector2.UnitX);

			//            this.selectedCollider.AddShape(newShape);
			//            this.SelectObjects(new[] { SelShape.Create(newShape) });
			//            success = true;
			//        }
			//        else
			//        {
			//            SelEdgeShape selEdgeShape = this.allObjSel.OfType<SelEdgeShape>().First();
			//            EdgeShapeInfo edgeShape = selEdgeShape.ActualObject as EdgeShapeInfo;
						
			//            switch (this.createPolyIndex)
			//            {
			//                case 0:	edgeShape.VertexStart = localPos;	break;
			//                case 1:	edgeShape.VertexEnd = localPos;		break;
			//            }

			//            selEdgeShape.UpdateEdgeStats();
			//            success = true;
			//        }

			//        if (success)
			//        {
			//            this.createPolyIndex++;
			//            DualityEditorApp.NotifyObjPropChanged(this,
			//                new ObjectSelection(this.selectedCollider),
			//                ReflectionInfo.Property_RigidBody_Shapes);

			//            if (this.createPolyIndex >= 2)
			//                this.LeaveCursorState();
			//        }
			//    }
			//    else if (e.Button == MouseButtons.Right)
			//    {
			//        if (this.allObjSel.Any(sel => sel is SelEdgeShape))
			//        {
			//            SelEdgeShape selEdgeShape = this.allObjSel.OfType<SelEdgeShape>().First();
			//            EdgeShapeInfo edgeShape = selEdgeShape.ActualObject as EdgeShapeInfo;

			//            if (this.createPolyIndex < 1)
			//                this.DeleteObjects(new SelEdgeShape[] { selEdgeShape });
			//            else
			//                selEdgeShape.UpdateEdgeStats();

			//            DualityEditorApp.NotifyObjPropChanged(this,
			//                new ObjectSelection(this.selectedCollider),
			//                ReflectionInfo.Property_RigidBody_Shapes);
			//        }

			//        this.LeaveCursorState();
			//    }
			//    #endregion
			//}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Transform selTransform = this.selectedBody != null && this.selectedBody.GameObj != null ? this.selectedBody.GameObj.Transform : null;
			Vector3 spaceCoord = selTransform != null ? this.GetSpaceCoord(new Vector3(e.X, e.Y, selTransform.Pos.Z)) : Vector3.Zero;
			Vector2 localPos = selTransform != null ? selTransform.GetLocalPoint(spaceCoord).Xy : Vector2.Zero;

			if (this.mouseState != CursorState.Normal) this.UpdateCursorImage();

			if (this.mouseState == CursorState.CreatePolygon && this.allObjSel.Any(sel => sel is SelPolyShape))
			{
				SelPolyShape selPolyShape = this.allObjSel.OfType<SelPolyShape>().First();
				PolyShapeInfo polyShape = selPolyShape.ActualObject as PolyShapeInfo;
				List<Vector2> vertices = polyShape.Vertices.ToList();
				Vector2 lockedPos = this.createPolyIndex > 0 ? vertices[this.createPolyIndex - 1] : Vector2.Zero;
				MathF.TransformCoord(ref lockedPos.X, ref lockedPos.Y, selTransform.Angle);
				MathF.TransformCoord(ref localPos.X, ref localPos.Y, selTransform.Angle);
				localPos = this.ApplyAxisLock(localPos, lockedPos);
				MathF.TransformCoord(ref localPos.X, ref localPos.Y, -selTransform.Angle);

				vertices[this.createPolyIndex] = localPos;

				polyShape.Vertices = vertices.ToArray();
				selPolyShape.UpdatePolyStats();
				
				// Update the body directly after modifying it
				this.selectedBody.SynchronizeBodyShape();

				DualityEditorApp.NotifyObjPropChanged(this,
					new ObjectSelection(this.selectedBody),
					ReflectionInfo.Property_RigidBody_Shapes);
			}
			else if (this.mouseState == CursorState.CreateLoop && this.allObjSel.Any(sel => sel is SelLoopShape))
			{
				SelLoopShape selPolyShape = this.allObjSel.OfType<SelLoopShape>().First();
				LoopShapeInfo polyShape = selPolyShape.ActualObject as LoopShapeInfo;
				List<Vector2> vertices = polyShape.Vertices.ToList();
				Vector2 lockedPos = this.createPolyIndex > 0 ? vertices[this.createPolyIndex - 1] : Vector2.Zero;
				MathF.TransformCoord(ref lockedPos.X, ref lockedPos.Y, selTransform.Angle);
				MathF.TransformCoord(ref localPos.X, ref localPos.Y, selTransform.Angle);
				localPos = this.ApplyAxisLock(localPos, lockedPos);
				MathF.TransformCoord(ref localPos.X, ref localPos.Y, -selTransform.Angle);

				vertices[this.createPolyIndex] = localPos;

				polyShape.Vertices = vertices.ToArray();
				selPolyShape.UpdateLoopStats();
				
				// Update the body directly after modifying it
				this.selectedBody.SynchronizeBodyShape();

				DualityEditorApp.NotifyObjPropChanged(this,
					new ObjectSelection(this.selectedBody),
					ReflectionInfo.Property_RigidBody_Shapes);
			}
		}
		protected override void OnLostFocus()
		{
			base.OnLostFocus();
			this.LeaveCursorState();
		}
		protected override void OnBeginAction(CamViewState.ObjectAction action)
		{
			base.OnBeginAction(action);
			bool shapeAction = 
				action != ObjectAction.RectSelect && 
				action != ObjectAction.None;
			if (this.selectedBody != null && shapeAction) this.selectedBody.BeginUpdateBodyShape();
		}
		protected override void OnEndAction(CamViewState.ObjectAction action)
		{
			base.OnEndAction(action);
			bool shapeAction = 
				action != ObjectAction.RectSelect && 
				action != ObjectAction.None;
			if (this.selectedBody != null && shapeAction)
			{
				this.selectedBody.EndUpdateBodyShape();
			}
			if (this.createAction)
			{
				this.createAction = false;
				UndoRedoManager.EndMacro(UndoRedoManager.MacroDeriveName.FromFirst);
			}
		}
		protected override void PostPerformAction(IEnumerable<CamViewState.SelObj> selObjEnum, CamViewState.ObjectAction action)
		{
			base.PostPerformAction(selObjEnum, action);
			SelShape[] selShapeArray = selObjEnum.OfType<SelShape>().ToArray();

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
			if (this.mouseState != CursorState.Normal)
			{
				if (this.mouseState == CursorState.CreateCircle)		return PluginRes.EditorBaseRes.ColliderEditor_CreateCircle;
				else if (this.mouseState == CursorState.CreatePolygon)	return PluginRes.EditorBaseRes.ColliderEditor_CreatePolygon;
				else if (this.mouseState == CursorState.CreateLoop)		return PluginRes.EditorBaseRes.ColliderEditor_CreateLoop;
			}

			return base.UpdateStatusText();
		}
		protected override string UpdateActionText()
		{
			Vector2 vertex = Vector2.Zero;
			if (this.mouseState == CursorState.CreatePolygon || this.mouseState == CursorState.CreateLoop)
			{
				Point mousePos = this.PointToClient(Cursor.Position);
				GameObject selGameObj = this.selectedBody != null ? this.selectedBody.GameObj : null;
				Transform selTransform = selGameObj != null ? selGameObj.Transform : null;
				SelPolyShape selPolyShape = this.allObjSel.OfType<SelPolyShape>().FirstOrDefault();
				SelLoopShape selLoopShape = this.allObjSel.OfType<SelLoopShape>().FirstOrDefault();
				bool hasData = false;
				if (selPolyShape != null)
				{
					PolyShapeInfo polyShape = selPolyShape.ActualObject as PolyShapeInfo;
					vertex = polyShape.Vertices[this.createPolyIndex];
					hasData = true;
				}
				else if (selLoopShape != null)
				{
					LoopShapeInfo loopShape = selLoopShape.ActualObject as LoopShapeInfo;
					vertex = loopShape.Vertices[this.createPolyIndex];
					hasData = true;
				}
				else if (selTransform != null)
				{
					Vector3 spaceCoord = this.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, selTransform.Pos.Z));
					vertex = selTransform.GetLocalPoint(spaceCoord).Xy;
					hasData = true;
				}

				if (hasData)
				{
					return 
						string.Format("Vertex X:{0,9:0.00}/n", vertex.X) +
						string.Format("Vertex Y:{0,9:0.00}", vertex.Y);
				}
			}
			return base.UpdateActionText();
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
					foreach (SelPolyShape sps in this.allObjSel.OfType<SelPolyShape>()) sps.UpdatePolyStats();
				//	foreach (SelEdgeShape sps in this.allObjSel.OfType<SelEdgeShape>()) sps.UpdateEdgeStats();
					foreach (SelLoopShape sps in this.allObjSel.OfType<SelLoopShape>()) sps.UpdateLoopStats();
					this.InvalidateSelectionStats();
					this.Invalidate();
				}
				this.UpdateToolbar();
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
					this.LeaveCursorState();

				DualityEditorApp.Deselect(this, ObjectSelection.Category.Other);
				this.selectedBody = newBody;
			}
			// Other selection changed
			if ((e.AffectedCategories & ObjectSelection.Category.Other) != ObjectSelection.Category.None)
			{
				if (e.Current.OfType<ShapeInfo>().Any())
					this.allObjSel = e.Current.OfType<ShapeInfo>().Select(s => SelShape.Create(s) as SelObj).ToList();
				else
					this.allObjSel = new List<SelObj>();

				// Update indirect object selection
				this.indirectObjSel.Clear();
				// Update (parent-free) action object selection
				this.actionObjSel = this.allObjSel.ToList();
			}

			this.InvalidateSelectionStats();
			this.UpdateToolbar();
			this.Invalidate();
		}

		private void toolCreateCircle_Clicked(object sender, EventArgs e)
		{
			if (this.selectedBody == null) return;
			this.EnterCursorState(CursorState.CreateCircle);
		}
		private void toolCreatePoly_Clicked(object sender, EventArgs e)
		{
			if (this.selectedBody == null) return;
			this.EnterCursorState(CursorState.CreatePolygon);
		}
		private void toolCreateLoop_Clicked(object sender, EventArgs e)
		{
			if (this.selectedBody == null) return;
			this.EnterCursorState(CursorState.CreateLoop);
		}
	}
}
