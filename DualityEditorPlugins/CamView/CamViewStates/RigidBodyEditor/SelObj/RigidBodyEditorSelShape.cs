using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public abstract class RigidBodyEditorSelShape : ObjectEditorSelObj
	{
		private ShapeInfo shape;
			
		public override bool HasTransform
		{
			get { return this.Body != null && !this.Body.Disposed && this.Body.GameObj.Transform != null; }
		}
		public override object ActualObject
		{
			get { return this.shape; }
		}
		public override string DisplayObjectName
		{
			get { return Properties.CamViewRes.RigidBodyCamViewState_SelShapeName; }
		}
		public RigidBody Body
		{
			get { return this.shape.Parent; }
		}
		public ShapeInfo Shape
		{
			get { return this.shape; }
		}

		protected RigidBodyEditorSelShape(ShapeInfo shape)
		{
			this.shape = shape;
		}

		public override bool IsActionAvailable(ObjectEditorAction action)
		{
			if (action == ObjectEditorAction.Move) return true;
			if (action == ObjectEditorAction.Rotate) return true;
			if (action == ObjectEditorAction.Scale) return true;
			return false;
		}
		public virtual void UpdateShapeStats() { }

		public static RigidBodyEditorSelShape Create(ShapeInfo shape)
		{
			if      (shape is CircleShapeInfo) return new RigidBodyEditorSelCircleShape(shape as CircleShapeInfo);
			else if (shape is PolyShapeInfo  ) return new RigidBodyEditorSelPolyShape  (shape as PolyShapeInfo  );
			else if (shape is ChainShapeInfo ) return new RigidBodyEditorSelChainShape (shape as ChainShapeInfo );
			else if (shape is LoopShapeInfo  ) return new RigidBodyEditorSelLoopShape  (shape as LoopShapeInfo  );
			else                               return null;
		}
	}
}
