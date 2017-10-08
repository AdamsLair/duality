using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// Creates a new circle shape in the selected <see cref="RigidBody"/>.
	/// </summary>
	public class CircleRigidBodyEditorTool : RigidBodyEditorTool
	{
		private CircleShapeInfo actionCircle = null;
		private Vector2 beginLocalPos = Vector2.Zero;

		public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_CreateCircle; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconCmpCircleCollider; }
		}
		public override Cursor ActionCursor
		{
			get { return CamViewResCache.CursorArrowCreateCircle; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.C; }
		}
		public override int SortOrder
		{
			get { return 1; }
		}
		public override bool IsHoveringAction
		{
			get { return true; }
		}

		public override bool CanBeginAction(MouseButtons mouseButton)
		{
			if (mouseButton != MouseButtons.Left) return false;
			return true;
		}
		public override void BeginAction(MouseButtons mouseButton)
		{
			base.BeginAction(mouseButton);

			this.beginLocalPos = this.Environment.ActiveBodyPos;
			this.actionCircle = new CircleShapeInfo(1.0f, this.Environment.ActiveBodyPos, 1.0f);
			UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.Environment.ActiveBody, this.actionCircle));
			this.Environment.SelectShapes(new ShapeInfo[] { this.actionCircle });
		}
		public override void UpdateAction()
		{
			base.UpdateAction();

			float radius = MathF.Max((this.Environment.ActiveBodyPos - this.beginLocalPos).Length, 1.0f);
			if (radius != this.actionCircle.Radius)
			{
				this.actionCircle.Radius = radius;
				DualityEditorApp.NotifyObjPropChanged(this,
					new ObjectSelection(this.Environment.ActiveBody),
					ReflectionInfo.Property_RigidBody_Shapes);
			}

			if (!this.Environment.IsActionKeyPressed)
				this.Environment.EndToolAction();
		}
		public override void EndAction()
		{
			base.EndAction();
			this.Environment.SelectTool(typeof(VertexRigidBodyEditorTool));
			this.actionCircle = null;
		}
	}
}
