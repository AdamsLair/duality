using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class RigidBodyEditorSelPolyShape : RigidBodyEditorSelPolyLikeShape
	{
		protected override Vector2[] Vertices
		{
			get { return (this.Shape as PolyShapeInfo).Vertices; }
			set { (this.Shape as PolyShapeInfo).Vertices = value; }
		}
		public RigidBodyEditorSelPolyShape(PolyShapeInfo shape) : base(shape) { }
	}
}
