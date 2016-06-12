using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	internal class RigidBodyEditorSelBody : ObjectEditorSelObj
	{
		private GameObject bodyObj;

		public override object ActualObject
		{
			get { return this.bodyObj == null || this.bodyObj.Disposed ? null : this.bodyObj; }
		}
		public override string DisplayObjectName
		{
			get { return Properties.CamViewRes.RigidBodyCamViewState_SelBodyName; }
		}
		public override bool HasTransform
		{
			get { return this.bodyObj != null && !this.bodyObj.Disposed && this.bodyObj.Transform != null; }
		}
		public override Vector3 Pos
		{
			get { return this.bodyObj.Transform.Pos; }
			set { }
		}
		public override float Angle
		{
			get { return this.bodyObj.Transform.Angle; }
			set { }
		}
		public override Vector3 Scale
		{
			get { return Vector3.One * this.bodyObj.Transform.Scale; }
			set { }
		}
		public override float BoundRadius
		{
			get
			{
				ICmpRenderer r = this.bodyObj.GetComponent<ICmpRenderer>();
				return r == null ? CamView.DefaultDisplayBoundRadius : r.BoundRadius;
			}
		}
		public override bool ShowPos
		{
			get { return false; }
		}

		public RigidBodyEditorSelBody(RigidBody obj)
		{
			this.bodyObj = obj != null ? obj.GameObj : null;
		}

		public override bool IsActionAvailable(ObjectEditorAction action)
		{
			return false;
		}
	}
}
