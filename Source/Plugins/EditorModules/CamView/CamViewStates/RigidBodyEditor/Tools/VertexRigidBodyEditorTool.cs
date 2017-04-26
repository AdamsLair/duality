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
		bool selecting = false;
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

		public override void OnWorldOverlayDrawcalls(Canvas canvas)
		{
			RigidBodyEditorCamViewState env = Environment as RigidBodyEditorCamViewState;
			Point mousePos = env.View.RenderableControl.PointToClient(Cursor.Position);
			this.mousePos = canvas.DrawDevice.GetSpaceCoord(new Vector3(mousePos.X, mousePos.Y, 0f));
			overlay.Draw(base.Environment.ActiveBody, canvas, this.mousePos, selecting);
		}

		public override void BeginAction()
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

			if (!selecting)
			{
				if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleNew)
				{
					List<Vector2> temp = overlay.CurrentVertex.shape.Vertices.ToList();
					int id = overlay.CurrentVertex.id + 1;
					temp.Insert(id, overlay.CurrentVertex.pos);
					overlay.CurrentVertex.shape.Vertices = temp.ToArray();

					UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));

					overlay.CurrentVertex.id = id;
					overlay.CurrentVertex.type = PolygonRigidBodyEditorOverlay.VertexType.Selected;

					overlay.SelectedVertices.Clear(); // Clear multiple selection

					return; // If a single vertex selection is found, exit the method
				}
				else if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.PosibleSelect)
				{
					overlay.CurrentVertex.type = PolygonRigidBodyEditorOverlay.VertexType.Selected;

					return; // If a single vertex selection is found, exit the method
				}

				// Uncomment to allow multiple selection
				//selecting = true; // If no single vertex selection is found, start multiple selection mode
			}
		}
		public override void UpdateAction()
		{
			if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
			{
				Vector2 origin = new Vector2(overlay.CurrentVertex.shape.Vertices[overlay.CurrentVertex.id].X, overlay.CurrentVertex.shape.Vertices[overlay.CurrentVertex.id].Y);
				if (overlay.SelectedVertices.Count > 0) // Check if the vertex selected is one of the previously selected (if not, clear the selection)
				{
					if (overlay.SelectedVertices.FirstOrDefault(x => x.pos == overlay.CurrentVertex.pos) == null) overlay.SelectedVertices.Clear();
				}
				if (overlay.SelectedVertices.Count > 0)
				{
					Vector2 diff = new Vector2(this.mousePos.X - origin.X, this.mousePos.Y - origin.Y);
					overlay.CurrentVertex.pos += diff;	
					foreach (PolygonRigidBodyEditorOverlay.VertexInfo vertex in overlay.SelectedVertices)
					{
						vertex.pos += diff;
						overlay.CurrentVertex.shape.Vertices[vertex.id] = vertex.pos;
					}
				}
				else
				{
					overlay.CurrentVertex.pos = this.mousePos.Xy;
					overlay.CurrentVertex.shape.Vertices[overlay.CurrentVertex.id] = this.mousePos.Xy;
				}
			}
		}
		public override void EndAction() { }

		public override void OnActionKeyReleased()
		{
			base.OnActionKeyReleased();

			if (!selecting)
			{
				if (overlay.CurrentVertex.type == PolygonRigidBodyEditorOverlay.VertexType.Selected)
				{
					overlay.CurrentVertex.shape.Vertices = overlay.CurrentVertex.shape.Vertices;

					UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(overlay.CurrentVertex.shape, originalVertices));

					overlay.CurrentVertex = new PolygonRigidBodyEditorOverlay.VertexInfo();
				}
			}
			else
			{
				selecting = false;
			}
			this.Environment.EndToolAction(true);
		}
	}
}
