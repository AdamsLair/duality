using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class RigidBodyEditorSelCircleShape : RigidBodyEditorSelShape
	{
		private	CircleShapeInfo	circle;
			
		public override Vector3 Pos
		{
			get
			{
				return this.Body.GameObj.Transform.GetWorldPoint(new Vector3(this.circle.Position));
			}
			set
			{
				value.Z = this.Body.GameObj.Transform.Pos.Z;
				this.circle.Position = this.Body.GameObj.Transform.GetLocalPoint(value).Xy;
			}
		}
		public override Vector3 Scale
		{
			get
			{
				return Vector3.One * this.circle.Radius;
			}
			set
			{
				this.circle.Radius = value.Length / MathF.Sqrt(3.0f);
			}
		}
		public override float BoundRadius
		{
			get { return this.circle.Radius * this.Body.GameObj.Transform.Scale; }
		}
		public override string DisplayObjectName
		{
			get { return Properties.CamViewRes.RigidBodyCamViewState_SelCircleShapeName; }
		}

		public RigidBodyEditorSelCircleShape(CircleShapeInfo shape) : base(shape)
		{
			this.circle = shape;
		}

		public override bool IsActionAvailable(ObjectEditorCamViewState.ObjectAction action)
		{
			if (action == ObjectEditorCamViewState.ObjectAction.Rotate) return false;
			return base.IsActionAvailable(action);
		}
		public override string UpdateActionText(ObjectEditorCamViewState.ObjectAction action, bool performing)
		{
			if (action == ObjectEditorCamViewState.ObjectAction.Move)
			{
				return
					string.Format("Center X:{0,9:0.00}/n", this.circle.Position.X) +
					string.Format("Center Y:{0,9:0.00}", this.circle.Position.Y);
			}
			else if (action == ObjectEditorCamViewState.ObjectAction.Scale)
			{
				return string.Format("Radius:{0,8:0.00}", this.circle.Radius);
			}
			return base.UpdateActionText(action, performing);
		}
	}
}
