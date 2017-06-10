using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Components.Physics;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	/// <summary>
	/// A tool that represents performing a move vertex operation on the edited <see cref="Duality.Components.Physics.RigidBody"/>.
	/// </summary>
	public class VertexRigidBodyEditorTool : RigidBodyEditorTool
	{
		private PolygonRigidBodyEditorOverlay overlay = new PolygonRigidBodyEditorOverlay();
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
			get { return -950; }
		}


		private void SetOriginalVertices()
		{
			if (overlay.CurrentVertex.shape != null)
			{
				Vector2[] vertices = overlay.CurrentVertex.shape.Vertices;
				originalVertices = new Vector2[vertices.Length];
				for (int i = 0; i < vertices.Length; i++)
				{
					this.originalVertices[i] = new Vector2(vertices[i].X, vertices[i].Y);
				}
			}
		}

		public override void BeginAction(MouseButtons mouseButton)
		{
			SetOriginalVertices();

			if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleNew)
			{
				List<Vector2> temp = overlay.CurrentVertex.shape.Vertices.ToList();
				int id = overlay.CurrentVertex.id + 1;
				temp.Insert(id, overlay.CurrentVertex.pos);
				overlay.CurrentVertex.shape.Vertices = temp.ToArray();

				UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));

				SetOriginalVertices(); // Needed for a good undo experience (the move vertext starts from here)
				overlay.CurrentVertex.id = id;
				overlay.CurrentVertex.type = PolygonRigidBodyEditorOverlay.VertexType.Selected;
			}
			else if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleSelect)
			{
				if (mouseButton == MouseButtons.Left) // Move current vertex
				{
					overlay.CurrentVertex.type = PolygonRigidBodyEditorOverlay.VertexType.Selected;
				}
				else if (mouseButton == MouseButtons.Right) // Delete current vertex
				{
					if (overlay.CurrentVertex.shape.Vertices.Length > 3)
					{
						List<Vector2> vertices = overlay.CurrentVertex.shape.Vertices.ToList();
						vertices.RemoveAt(overlay.CurrentVertex.id);
						overlay.CurrentVertex.shape.Vertices = vertices.ToArray();

						UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));
					}
				}
			}
		}
		public override void UpdateAction()
		{
			if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
			{
				Vector2 worldPos = this.Environment.ActiveWorldPos.Xy;
				Vector2 localPos = this.Environment.ActiveBodyPos;
				overlay.CurrentVertex.pos = worldPos;
				overlay.CurrentVertex.shape.Vertices[overlay.CurrentVertex.id] = localPos;
			}
		}
		public override void EndAction() { }

		public override void OnActionKeyReleased()
		{
			base.OnActionKeyReleased();

			if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
			{
				// If the vertex is not moved, don't do anything
				if (originalVertices[overlay.CurrentVertex.id] != overlay.CurrentVertex.pos)
				{
					overlay.CurrentVertex.shape.Vertices = overlay.CurrentVertex.shape.Vertices;

					UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));
				}
				overlay.CurrentVertex = new PolygonRigidBodyEditorOverlay.VertexInfo();
			}
			this.Environment.EndToolAction();
		}
		public override void OnWorldOverlayDrawcalls(Canvas canvas)
		{
			RigidBodyEditorCamViewState env = Environment as RigidBodyEditorCamViewState;
			Point mousePos = env.View.RenderableControl.PointToClient(Cursor.Position);
			Vector3 mousePosVector = canvas.DrawDevice.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, 0f));
			overlay.Draw(base.Environment.ActiveBody, canvas, mousePosVector);
		}
	}
}
