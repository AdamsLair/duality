using System.Drawing;
using System.Linq;

using Duality;
using Duality.Components;
using Duality.Components.Physics;

namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public class RigidBodyEditorSelChainShape : RigidBodyEditorSelPolyLikeShape
	{
		protected override Vector2[] Vertices
		{
			get { return (this.Shape as ChainShapeInfo).Vertices; }
			set { (this.Shape as ChainShapeInfo).Vertices = value; }
		}
		public RigidBodyEditorSelChainShape(ChainShapeInfo shape) : base(shape) { }
	}
}
