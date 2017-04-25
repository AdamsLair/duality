using Duality.Components.Physics;
using Duality.Drawing;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
    /// <summary>
    /// A tool that represents performing a move vertex operation on the edited <see cref="Duality.Components.Physics.RigidBody"/>.
    /// </summary>
    public class VertexRigidBodyEditorTool : RigidBodyEditorTool
	{
		private PolygonRigidBodyEditorOverlay overlay = new PolygonRigidBodyEditorOverlay();
        private Vector3 mousePos;
        private Vector2[] originalVertices;

        public override string Name
		{
			get { return CamViewRes.RigidBodyCamViewState_ItemName_EditVertices; }
		}
		public override Image Icon
		{
			get { return CamViewResCache.IconCmpEditVertices; }
		}
		public override Cursor ActionCursor
		{
			get { return CamViewResCache.CursorArrowEditVertices; }
		}
		public override Keys ShortcutKey
		{
			get { return Keys.M; }
		}
		public override int SortOrder
		{
			get { return 4; }
		}

		public override void BeginAction()
		{
            Vector2[] vertices = overlay.CurrentVertex.shape.Vertices;
            originalVertices = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                this.originalVertices[i] = new Vector2(vertices[i].X, vertices[i].Y);
            }

            if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleNew)
            {
                List<Vector2> temp = overlay.CurrentVertex.shape.Vertices.ToList();
                int id = overlay.CurrentVertex.id;
                temp.Insert(id + 1, overlay.CurrentVertex.pos);
                overlay.CurrentVertex.shape.Vertices = temp.ToArray();
                this.Environment.EndToolAction(true);

                UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));

                overlay.CurrentVertex = new PolygonRigidBodyEditorOverlay.VertexInfo();
            }
            else if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleSelect)
            {
                overlay.CurrentVertex.type = PolygonRigidBodyEditorOverlay.VertexType.Selected;
            }
        }
		public override void UpdateAction()
		{
            if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
            {
                overlay.CurrentVertex.shape.Vertices[overlay.CurrentVertex.id] = this.mousePos.Xy;
            }
        }
		public override void EndAction()
        {
        }
        
		public override void OnWorldOverlayDrawcalls(Canvas canvas)
        {
            RigidBodyEditorCamViewState env = Environment as RigidBodyEditorCamViewState;
			Point mousePos = env.View.RenderableControl.PointToClient(Cursor.Position);
            this.mousePos = canvas.DrawDevice.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, 0f));
            overlay.Draw(base.Environment.ActiveBody, canvas, this.mousePos);
        }

        public override void OnActionKeyReleased()
        {
            base.OnActionKeyReleased();

            if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
            {
                overlay.CurrentVertex.shape.Vertices = overlay.CurrentVertex.shape.Vertices;

                UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));

                overlay.CurrentVertex = new PolygonRigidBodyEditorOverlay.VertexInfo();
            }
            this.Environment.EndToolAction(true);
        }
    }
}
