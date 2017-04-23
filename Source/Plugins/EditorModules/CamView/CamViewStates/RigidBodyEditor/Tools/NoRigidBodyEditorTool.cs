using Duality.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// A dummy tool that represents not performing any operation on the edited <see cref="Duality.Components.Physics.RigidBody"/>.
	/// </summary>
	public class NoRigidBodyEditorTool : RigidBodyEditorTool
	{
		private PolygonRigidBodyEditorOverlay overlay = new PolygonRigidBodyEditorOverlay();

		public override string Name
		{
			get { return null; }
		}
		public override Image Icon
		{
			get { return null; }
		}
		public override Cursor ActionCursor
		{
			get { return CursorHelper.Arrow; }
		}

		// RigidBodyEditorSelVertices Test 2
		public override void OnCollectStateWorldOverlayDrawcalls(Canvas canvas)
		{
			RigidBodyEditorCamViewState env = Environment as RigidBodyEditorCamViewState;
			Point mousePos = env.View.RenderableControl.PointToClient(Cursor.Position);
			overlay.Draw(base.Environment.ActiveBody, canvas, new Vector3(mousePos.X, mousePos.Y, 0f));
		}

		// RigidBodyEditorSelVertices Test 2
		public override void OnActionKeyPressed()
		{
			if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleNew)
			{
				List<Vector2> temp = overlay.CurrentVertex.shape.Vertices.ToList();
				int id = overlay.CurrentVertex.id;
				temp.Insert(id + 1, overlay.CurrentVertex.pos);
				overlay.CurrentVertex.shape.Vertices = temp.ToArray();
				overlay.CurrentVertex = new PolygonRigidBodyEditorOverlay.VertexInfo();
			}
		}
	}
}
