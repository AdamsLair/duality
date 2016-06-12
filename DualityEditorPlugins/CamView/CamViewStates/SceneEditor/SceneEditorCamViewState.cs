using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.Drawing;
using Duality.Editor;
using Duality.Editor.Forms;
using Duality.Editor.UndoRedoActions;
using Duality.Editor.Plugins.CamView.UndoRedoActions;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Allows to move, scale and rotate the objects of a <see cref="Scene"/> while providing a
	/// preview through the lens of the editor <see cref="Camera"/>. User input is consumed
	/// by the editor and not forwarded to the game.
	/// </summary>
	public class SceneEditorCamViewState : ObjectEditorCamViewState
	{
		private ObjectSelection selBeforeDrag			= null;
		private	DateTime		dragTime				= DateTime.Now;
		private	Point			dragLastLoc				= Point.Empty;
		private Point			mousePosOnContextMenu	= Point.Empty;


		public override string StateName
		{
			get { return Properties.CamViewRes.CamViewState_SceneEditor_Name; }
		}
		private bool DragMustWait
		{
			get { return this.DragMustWaitProgress < 1.0f; }
		}
		private float DragMustWaitProgress
		{
			get { return MathF.Clamp((float)(DateTime.Now - this.dragTime).TotalMilliseconds / 500.0f, 0.0f, 1.0f); }
		}


		public SceneEditorCamViewState()
		{
			this.SnapToUserGuides &= (~UserGuideType.Scale);
		}

		internal protected override void OnEnterState()
		{
			base.OnEnterState();

			DualityEditorApp.SelectionChanged		+= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	+= this.EditorForm_ObjectPropertyChanged;

			// Initial selection update
			ObjectSelection current = DualityEditorApp.Selection;
			this.allObjSel = current.GameObjects.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).ToList();
			{
				var selectedGameObj = current.GameObjects;
				var indirectViaCmpQuery = current.Components.GameObject();
				var indirectViaChildQuery = selectedGameObj.ChildrenDeep();
				var indirectQuery = indirectViaCmpQuery.Concat(indirectViaChildQuery).Except(selectedGameObj).Distinct();
				this.indirectObjSel = indirectQuery.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).ToList();
			}
			{
				var parentlessGameObj = current.GameObjects.Where(g => !current.GameObjects.Any(g2 => g.IsChildOf(g2))).ToList();
				this.actionObjSel = parentlessGameObj.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).Where(s => s.HasTransform).ToList();
			}

			this.InvalidateSelectionStats();
		}
		internal protected override void OnLeaveState()
		{
			base.OnLeaveState();

			DualityEditorApp.SelectionChanged		-= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	-= this.EditorForm_ObjectPropertyChanged;
		}
		protected override void OnSceneChanged()
		{
			base.OnSceneChanged();
			this.InvalidateSelectionStats();
		}
		protected override void OnCollectStateOverlayDrawcalls(Canvas canvas)
		{
			base.OnCollectStateOverlayDrawcalls(canvas);
			if (this.ObjAction == ObjectEditorAction.None && this.DragMustWait && !this.dragLastLoc.IsEmpty)
			{
				canvas.PushState();
				canvas.State.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
				canvas.State.ColorTint = ColorRgba.White.WithAlpha(this.DragMustWaitProgress);
				canvas.FillCircle(
					this.dragLastLoc.X, 
					this.dragLastLoc.Y, 
					15.0f);
				canvas.State.ColorTint = ColorRgba.White;
				canvas.DrawCircle(
					this.dragLastLoc.X, 
					this.dragLastLoc.Y, 
					15.0f);
				canvas.PopState();
			}
		}

		public override ObjectEditorSelObj PickSelObjAt(int x, int y)
		{
			Component picked = this.PickRendererAt(x, y) as Component;
			if (picked != null && DesignTimeObjectData.Get(picked.GameObj).IsLocked) picked = null;
			if (picked != null) return new SceneEditorSelGameObj(picked.GameObj);
			return null;
		}
		public override List<ObjectEditorSelObj> PickSelObjIn(int x, int y, int w, int h)
		{
			IEnumerable<ICmpRenderer> picked = this.PickRenderersIn(x, y, w, h);
			return picked
				.OfType<Component>()
				.Where(r => !DesignTimeObjectData.Get(r.GameObj).IsLocked)
				.Select(r => new SceneEditorSelGameObj(r.GameObj) as ObjectEditorSelObj)
				.ToList();
		}

		public override void ClearSelection()
		{
			base.ClearSelection();
			DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
			ClearContextMenu();
		}
		public override void SelectObjects(IEnumerable<ObjectEditorSelObj> selObjEnum, SelectMode mode = SelectMode.Set)
		{
			base.SelectObjects(selObjEnum, mode);
			DualityEditorApp.Select(this, new ObjectSelection(selObjEnum.Select(s => s.ActualObject)), mode);
		}
		protected override void PostPerformAction(IEnumerable<ObjectEditorSelObj> selObjEnum, ObjectEditorAction action)
		{
			base.PostPerformAction(selObjEnum, action);
			if (action == ObjectEditorAction.Move)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos);
			}
			else if (action == ObjectEditorAction.Rotate)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos,
					ReflectionInfo.Property_Transform_RelativeAngle);
			}
			else if (action == ObjectEditorAction.Scale)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos,
					ReflectionInfo.Property_Transform_RelativeScale);
			}
		}

		public override void DeleteObjects(IEnumerable<ObjectEditorSelObj> objEnum)
		{
			var objList = objEnum.Select(s => s.ActualObject as GameObject).ToList();
			if (objList.Count == 0) return;

			// Ask user if he really wants to delete stuff
			ObjectSelection objSel = new ObjectSelection(objList);
			if (!DualityEditorApp.DisplayConfirmDeleteObjects(objSel)) return;
			if (!DualityEditorApp.DisplayConfirmBreakPrefabLinkStructure(objSel)) return;

			UndoRedoManager.Do(new DeleteGameObjectAction(objList));
		}
		public override List<ObjectEditorSelObj> CloneObjects(IEnumerable<ObjectEditorSelObj> objEnum)
		{
			if (objEnum == null || !objEnum.Any()) return base.CloneObjects(objEnum);
			var objList = objEnum.Select(s => s.ActualObject as GameObject).ToList();

			CloneGameObjectAction cloneAction = new CloneGameObjectAction(objList);
			UndoRedoManager.Do(cloneAction);

			return cloneAction.Result.Select(o => new SceneEditorSelGameObj(o) as ObjectEditorSelObj).ToList();
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			DataObject data = e.Data as DataObject;
			if (new ConvertOperation(data, ConvertOperation.Operation.All).CanPerform<GameObject>())
			{
				e.Effect = e.AllowedEffect;
				this.dragTime = DateTime.Now;
				this.dragLastLoc = new Point(e.X, e.Y);
			}
			else
			{
				e.Effect = DragDropEffects.None;
				this.dragLastLoc = Point.Empty;
				this.dragTime = DateTime.Now;
			}
		}
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);

			this.dragLastLoc = Point.Empty;
			this.dragTime = DateTime.Now;
			this.Invalidate();
			if (this.ObjAction == ObjectEditorAction.None) return;
			
			this.EndAction();

			// Destroy temporarily instantiated objects
			foreach (ObjectEditorSelObj obj in this.allObjSel)
			{
				GameObject gameObj = obj.ActualObject as GameObject;
				Scene.Current.RemoveObject(gameObj);
				gameObj.Dispose();
			}
			DualityEditorApp.Select(this, this.selBeforeDrag);
		}
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);

			if (e.Effect == DragDropEffects.None) return;
			if (this.ObjAction == ObjectEditorAction.None && !this.DragMustWait)
				this.DragBeginAction(e);
			
			Point clientCoord = this.PointToClient(new Point(e.X, e.Y));
			if (Math.Abs(clientCoord.X - this.dragLastLoc.X) > 20 || Math.Abs(clientCoord.Y - this.dragLastLoc.Y) > 20)
				this.dragTime = DateTime.Now;
			this.dragLastLoc = clientCoord;
			this.Invalidate();

			if (this.ObjAction != ObjectEditorAction.None) this.UpdateAction();
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);

			if (this.ObjAction == ObjectEditorAction.None)
			{
				this.DragBeginAction(e);
				if (this.ObjAction != ObjectEditorAction.None) this.UpdateAction();
			}
			
			this.dragLastLoc = Point.Empty;
			this.dragTime = DateTime.Now;

			if (this.ObjAction != ObjectEditorAction.None) this.EndAction();

			UndoRedoManager.EndMacro();
		}
		private void DragBeginAction(DragEventArgs e)
		{
			DataObject data = e.Data as DataObject;
			var dragObjQuery = new ConvertOperation(data, ConvertOperation.Operation.All).Perform<GameObject>();
			if (dragObjQuery != null)
			{
				List<GameObject> dragObj = dragObjQuery.ToList();

				bool lockZ = this.CameraComponent.FocusDist <= 0.0f;
				Point mouseLoc = this.PointToClient(new Point(e.X, e.Y));
				Vector3 spaceCoord = this.GetSpaceCoord(new Vector3(
					mouseLoc.X, 
					mouseLoc.Y, 
					lockZ ? 0.0f : this.CameraObj.Transform.Pos.Z + MathF.Abs(this.CameraComponent.FocusDist)));
				if ((this.SnapToUserGuides & UserGuideType.Position) != UserGuideType.None)
				{
					spaceCoord = this.EditingUserGuide.SnapPosition(spaceCoord);
				}

				// Setup GameObjects
				CreateGameObjectAction createAction = new CreateGameObjectAction(null, dragObj);
				DropGameObjectInSceneAction dropAction = new DropGameObjectInSceneAction(dragObj, spaceCoord, this.CameraObj.Transform.Angle);
				UndoRedoManager.BeginMacro(dropAction.Name);
				UndoRedoManager.Do(createAction);
				UndoRedoManager.Do(dropAction);

				// Select them & begin action
				this.selBeforeDrag = DualityEditorApp.Selection;
				this.SelectObjects(createAction.Result.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj));
				this.BeginAction(ObjectEditorAction.Move);

				// Get focused
				this.Focus();

				e.Effect = e.AllowedEffect;
			}
		}

		private void EditorForm_ObjectPropertyChanged(object sender, ObjectPropertyChangedEventArgs e)
		{
			if (e.Objects.Components.Any(c => c is Transform || c is ICmpRenderer))
				this.InvalidateSelectionStats();
		}
		private void EditorForm_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((e.AffectedCategories & ObjectSelection.Category.GameObjCmp) == ObjectSelection.Category.None)
			{
				ClearContextMenu();
				return;
			}
			if (e.SameObjects) return;

			// Update object selection
			this.allObjSel = e.Current.GameObjects.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).ToList();

			// Update indirect object selection
			{
				var selectedGameObj = e.Current.GameObjects;
				var indirectViaCmpQuery = e.Current.Components.GameObject();
				var indirectViaChildQuery = selectedGameObj.ChildrenDeep();
				var indirectQuery = indirectViaCmpQuery.Concat(indirectViaChildQuery).Except(selectedGameObj).Distinct();
				this.indirectObjSel = indirectQuery.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).ToList();
			}

			// Update (parent-free) action object selection
			{
				// Remove removed objects
				foreach (ObjectEditorSelObj s in e.Removed.GameObjects.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj)) this.actionObjSel.Remove(s);
				// Remove objects whichs parents are now added
				this.actionObjSel.RemoveAll(s => e.Added.GameObjects.Any(o => this.IsAffectedByParent(s.ActualObject as GameObject, o)));
				// Add objects whichs parents are not located in the current selection
				var addedParentFreeGameObj = e.Added.GameObjects.Where(o => !this.allObjSel.Any(s => this.IsAffectedByParent(o, s.ActualObject as GameObject)));
				this.actionObjSel.AddRange(addedParentFreeGameObj.Select(g => new SceneEditorSelGameObj(g) as ObjectEditorSelObj).Where(s => s.HasTransform));
			}

			this.InvalidateSelectionStats();
			this.UpdateAction();
			this.Invalidate();
		}

		private bool IsAffectedByParent(GameObject child, GameObject parent)
		{
			return child.IsChildOf(parent) && child.Transform != null && parent.Transform != null && !child.Transform.IgnoreParent;
		}
		private void ClearContextMenu()
		{
			if (this.RenderableSite == null || this.RenderableControl.ContextMenu == null)
				return;

			this.RenderableControl.ContextMenu.MenuItems.Clear();
		}
	}
}
