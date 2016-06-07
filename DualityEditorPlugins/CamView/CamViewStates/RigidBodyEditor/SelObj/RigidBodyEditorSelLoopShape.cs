using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class RigidBodyEditorSelLoopShape : RigidBodyEditorSelPolyLikeShape
	{
		protected override Vector2[] Vertices
		{
			get { return (this.Shape as LoopShapeInfo).Vertices; }
			set { (this.Shape as LoopShapeInfo).Vertices = value; }
		}
		public RigidBodyEditorSelLoopShape(LoopShapeInfo shape) : base(shape) { }
	}
}
