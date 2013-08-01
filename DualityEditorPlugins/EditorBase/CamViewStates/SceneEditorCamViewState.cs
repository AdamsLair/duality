using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality;
using Duality.Components;
using Duality.Resources;
using Duality.ColorFormat;

using DualityEditor;
using DualityEditor.Forms;
using DualityEditor.CorePluginInterface;
using DualityEditor.UndoRedoActions;

using EditorBase.UndoRedoActions;

using OpenTK;

namespace EditorBase.CamViewStates
{
	public class SceneEditorCamViewState : CamViewState
	{
		public class SelGameObj : SelObj
		{
			private	GameObject	gameObj;

			public override object ActualObject
			{
				get { return this.gameObj == null || this.gameObj.Disposed ? null : this.gameObj; }
			}
			public override bool HasTransform
			{
				get { return this.gameObj != null && !this.gameObj.Disposed && this.gameObj.Transform != null; }
			}
			public override Vector3 Pos
			{
				get { return this.gameObj.Transform.Pos; }
				set { this.gameObj.Transform.Pos = value; }
			}
			public override float Angle
			{
				get { return this.gameObj.Transform.Angle; }
				set { this.gameObj.Transform.Angle = value; }
			}
			public override Vector3 Scale
			{
				get { return Vector3.One * this.gameObj.Transform.Scale; }
				set { this.gameObj.Transform.Scale = value.Length / MathF.Sqrt(3.0f); }
			}
			public override float BoundRadius
			{
				get
				{
					ICmpRenderer r = this.gameObj.Renderer;
					if (r == null)
					{
						if (this.gameObj.Transform != null)
							return CamView.DefaultDisplayBoundRadius * this.gameObj.Transform.Scale;
						else
							return CamView.DefaultDisplayBoundRadius;
					}
					else
						return r.BoundRadius;
				}
			}
			public override bool ShowAngle
			{
				get { return true; }
			}

			public SelGameObj(GameObject obj)
			{
				this.gameObj = obj;
			}

			public override bool IsActionAvailable(ObjectAction action)
			{
				if (action == ObjectAction.Move) return true;
				if (action == ObjectAction.Rotate) return true;
				if (action == ObjectAction.Scale) return true;
				return false;
			}
			public override string UpdateActionText(ObjectAction action, bool performing)
			{
				if (action == ObjectAction.Move)
				{
					return
						string.Format("X:{0,7:0}/n", this.gameObj.Transform.RelativePos.X) +
						string.Format("Y:{0,7:0}/n", this.gameObj.Transform.RelativePos.Y) +
						string.Format("Z:{0,7:0}", this.gameObj.Transform.RelativePos.Z);
				}
				else if (action == ObjectAction.Scale)
				{
					return string.Format("Scale:{0,5:0.00}", this.gameObj.Transform.RelativeScale);
				}
				else if (action == ObjectAction.Rotate)
				{
					return string.Format("Angle:{0,5:0}°", MathF.RadToDeg(this.gameObj.Transform.RelativeAngle));
				}

				return base.UpdateActionText(action, performing);
			}
		}

		private ObjectSelection selBeforeDrag	= null;
		private	DateTime		dragTime		= DateTime.Now;
		private	Point			dragLastLoc		= Point.Empty;

		public override string StateName
		{
			get { return PluginRes.EditorBaseRes.CamViewState_SceneEditor_Name; }
		}
		private bool DragMustWait
		{
			get { return this.DragMustWaitProgress < 1.0f; }
		}
		private float DragMustWaitProgress
		{
			get { return MathF.Clamp((float)(DateTime.Now - this.dragTime).TotalMilliseconds / 500.0f, 0.0f, 1.0f); }
		}

		internal protected override void OnEnterState()
		{
			base.OnEnterState();

			DualityEditorApp.SelectionChanged		+= this.EditorForm_SelectionChanged;
			DualityEditorApp.ObjectPropertyChanged	+= this.EditorForm_ObjectPropertyChanged;

			// Initial selection update
			ObjectSelection current = DualityEditorApp.Selection;
			this.allObjSel = current.GameObjects.Select(g => new SelGameObj(g) as SelObj).ToList();
			{
				var selectedGameObj = current.GameObjects;
				var indirectViaCmpQuery = current.Components.GameObject();
				var indirectViaChildQuery = selectedGameObj.ChildrenDeep();
				var indirectQuery = indirectViaCmpQuery.Concat(indirectViaChildQuery).Except(selectedGameObj).Distinct();
				this.indirectObjSel = indirectQuery.Select(g => new SelGameObj(g) as SelObj).ToList();
			}
			{
				var parentlessGameObj = current.GameObjects.Where(g => !current.GameObjects.Any(g2 => g.IsChildOf(g2))).ToList();
				this.actionObjSel = parentlessGameObj.Select(g => new SelGameObj(g) as SelObj).Where(s => s.HasTransform).ToList();
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
			if (this.SelObjAction == ObjectAction.None && this.DragMustWait && !this.dragLastLoc.IsEmpty)
			{
				canvas.PushState();
				canvas.CurrentState.SetMaterial(new BatchInfo(DrawTechnique.Alpha, ColorRgba.White));
				canvas.CurrentState.ColorTint = ColorRgba.White.WithAlpha(this.DragMustWaitProgress);
				canvas.FillCircle(
					this.dragLastLoc.X, 
					this.dragLastLoc.Y, 
					15.0f);
				canvas.CurrentState.ColorTint = ColorRgba.White;
				canvas.DrawCircle(
					this.dragLastLoc.X, 
					this.dragLastLoc.Y, 
					15.0f);
				canvas.PopState();
			}
		}

		public override CamViewState.SelObj PickSelObjAt(int x, int y)
		{
			Component picked = this.PickRendererAt(x, y) as Component;
			if (picked != null && CorePluginRegistry.RequestDesignTimeData(picked.GameObj).IsLocked) picked = null;
			if (picked != null) return new SelGameObj(picked.GameObj);
			return null;
		}
		public override List<CamViewState.SelObj> PickSelObjIn(int x, int y, int w, int h)
		{
			HashSet<ICmpRenderer> picked = this.PickRenderersIn(x, y, w, h);
			return picked
				.OfType<Component>()
				.Where(r => !CorePluginRegistry.RequestDesignTimeData(r.GameObj).IsLocked)
				.Select(r => new SelGameObj(r.GameObj) as SelObj)
				.ToList();
		}

		public override void ClearSelection()
		{
			base.ClearSelection();
			DualityEditorApp.Deselect(this, ObjectSelection.Category.GameObjCmp);
		}
		public override void SelectObjects(IEnumerable<CamViewState.SelObj> selObjEnum, SelectMode mode = SelectMode.Set)
		{
			base.SelectObjects(selObjEnum, mode);
			DualityEditorApp.Select(this, new ObjectSelection(selObjEnum.Select(s => s.ActualObject)), mode);
		}
		protected override void PostPerformAction(IEnumerable<CamViewState.SelObj> selObjEnum, CamViewState.ObjectAction action)
		{
			base.PostPerformAction(selObjEnum, action);
			if (action == ObjectAction.Move)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos);
			}
			else if (action == ObjectAction.Rotate)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos,
					ReflectionInfo.Property_Transform_RelativeAngle);
			}
			else if (action == ObjectAction.Scale)
			{
				DualityEditorApp.NotifyObjPropChanged(
					this,
					new ObjectSelection(selObjEnum.Select(s => (s.ActualObject as GameObject).Transform)),
					ReflectionInfo.Property_Transform_RelativePos,
					ReflectionInfo.Property_Transform_RelativeScale);
			}
		}

		public override void DeleteObjects(IEnumerable<SelObj> objEnum)
		{
			var objList = objEnum.Select(s => s.ActualObject as GameObject).ToList();
			if (objList.Count == 0) return;

			// Ask user if he really wants to delete stuff
			ObjectSelection objSel = new ObjectSelection(objList);
			if (!DualityEditorApp.DisplayConfirmDeleteObjects(objSel)) return;
			if (!DualityEditorApp.DisplayConfirmBreakPrefabLinkStructure(objSel)) return;

			UndoRedoManager.Do(new DeleteGameObjectAction(objList));
		}
		public override List<SelObj> CloneObjects(IEnumerable<SelObj> objEnum)
		{
			var objList = objEnum.Select(s => s.ActualObject as GameObject).ToList();

			CloneGameObjectAction cloneAction = new CloneGameObjectAction(objList);
			UndoRedoManager.Do(cloneAction);

			return cloneAction.Result.Select(o => new SelGameObj(o) as SelObj).ToList();
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
			if (this.SelObjAction == ObjectAction.None) return;
			
			this.EndAction();

			// Destroy temporarily instantiated objects
			foreach (SelObj obj in this.allObjSel)
			{
				GameObject gameObj = obj.ActualObject as GameObject;
				Scene.Current.UnregisterObj(gameObj);
				gameObj.Dispose();
			}
			DualityEditorApp.Select(this, this.selBeforeDrag);
		}
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);

			if (e.Effect == DragDropEffects.None) return;
			if (this.SelObjAction == ObjectAction.None && !this.DragMustWait)
				this.DragBeginAction(e);
			
			Point clientCoord = this.PointToClient(new Point(e.X, e.Y));
			if (Math.Abs(clientCoord.X - this.dragLastLoc.X) > 20 || Math.Abs(clientCoord.Y - this.dragLastLoc.Y) > 20)
				this.dragTime = DateTime.Now;
			this.dragLastLoc = clientCoord;
			this.Invalidate();

			if (this.SelObjAction != ObjectAction.None) this.UpdateAction();
		}
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);

			if (this.SelObjAction == ObjectAction.None)
			{
				this.DragBeginAction(e);
				if (this.SelObjAction != ObjectAction.None) this.UpdateAction();
			}
			
			this.dragLastLoc = Point.Empty;
			this.dragTime = DateTime.Now;

			if (this.SelObjAction != ObjectAction.None) this.EndAction();

			UndoRedoManager.EndMacro();
		}
		protected override void OnCurrentCameraChanged(CamView.CameraChangedEventArgs e)
		{
			base.OnCurrentCameraChanged(e);

			if (e.PreviousCamera != null) e.PreviousCamera.RemoveEditorRendererFilter(this.RendererFilter);
			if (e.NextCamera != null) e.NextCamera.AddEditorRendererFilter(this.RendererFilter);
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

				// Setup GameObjects
				CreateGameObjectAction createAction = new CreateGameObjectAction(null, dragObj);
				DropGameObjectInSceneAction dropAction = new DropGameObjectInSceneAction(dragObj, spaceCoord, this.CameraObj.Transform.Angle);
				UndoRedoManager.BeginMacro(dropAction.Name);
				UndoRedoManager.Do(createAction);
				UndoRedoManager.Do(dropAction);

				// Select them & begin action
				this.selBeforeDrag = DualityEditorApp.Selection;
				this.SelectObjects(createAction.Result.Select(g => new SelGameObj(g) as SelObj));
				this.BeginAction(ObjectAction.Move);

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
			if ((e.AffectedCategories & ObjectSelection.Category.GameObjCmp) == ObjectSelection.Category.None) return;
			if (e.SameObjects) return;

			// Update object selection
			this.allObjSel = e.Current.GameObjects.Select(g => new SelGameObj(g) as SelObj).ToList();

			// Update indirect object selection
			{
				var selectedGameObj = e.Current.GameObjects;
				var indirectViaCmpQuery = e.Current.Components.GameObject();
				var indirectViaChildQuery = selectedGameObj.ChildrenDeep();
				var indirectQuery = indirectViaCmpQuery.Concat(indirectViaChildQuery).Except(selectedGameObj).Distinct();
				this.indirectObjSel = indirectQuery.Select(g => new SelGameObj(g) as SelObj).ToList();
			}

			// Update (parent-free) action object selection
			{
				// Remove removed objects
				foreach (SelObj s in e.Removed.GameObjects.Select(g => new SelGameObj(g) as SelObj)) this.actionObjSel.Remove(s);
				// Remove objects whichs parents are now added
				this.actionObjSel.RemoveAll(s => e.Added.GameObjects.Any(o => this.IsAffectedByParent(s.ActualObject as GameObject, o)));
				// Add objects whichs parents are not located in the current selection
				var addedParentFreeGameObj = e.Added.GameObjects.Where(o => !this.allObjSel.Any(s => this.IsAffectedByParent(o, s.ActualObject as GameObject)));
				this.actionObjSel.AddRange(addedParentFreeGameObj.Select(g => new SelGameObj(g) as SelObj).Where(s => s.HasTransform));
			}

			this.InvalidateSelectionStats();
			this.UpdateAction();
			this.Invalidate();
		}

		private bool IsAffectedByParent(GameObject child, GameObject parent)
		{
			return child.IsChildOf(parent) && child.Transform != null && parent.Transform != null && !child.Transform.IgnoreParent;
		}
		private bool RendererFilter(ICmpRenderer r)
		{
			GameObject obj = (r as Component).GameObj;
			DesignTimeObjectData data = CorePluginRegistry.RequestDesignTimeData(obj);
			return !data.IsHidden;
		}
	}
}
