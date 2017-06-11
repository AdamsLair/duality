using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Drawing;
using Duality.Components;
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
		private bool          isMovingVertex       = false;
		private PolyShapeInfo activeShape          = null;
		private int           activeVertex         = -1;
		private int           activeEdge           = -1;
		private Vector2       activeEdgeWorldPos   = Vector2.Zero;

		private PolyShapeInfo backedUpShape        = null;
		private Vector2[]     backedUpVertices     = null;


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
		public override bool IsHoveringAction
		{
			get { return this.activeShape != null; }
		}


		public override bool CanBeginAction(MouseButtons mouseButton)
		{
			if (mouseButton == MouseButtons.Left)
				return this.activeShape != null;
			else if (mouseButton == MouseButtons.Right)
				return this.activeShape != null && this.activeVertex != -1;
			else
				return false;
		}
		public override void BeginAction(MouseButtons mouseButton)
		{
			base.BeginAction(mouseButton);

			// Create a backup of the polygons vertices before our edit operation,
			// so we can go back via Undo later.
			this.backedUpShape = this.activeShape;
			this.backedUpVertices = (Vector2[])this.activeShape.Vertices.Clone();
			
			if (mouseButton == MouseButtons.Left)
			{
				if (this.activeEdge != -1)
				{
					int newIndex = this.activeEdge + 1;
					List<Vector2> newVertices = this.activeShape.Vertices.ToList();
					newVertices.Insert(newIndex, this.Environment.ActiveBodyPos);

					this.activeShape.Vertices = newVertices.ToArray();
					this.activeVertex = newIndex;
				}
			}
			else if (mouseButton == MouseButtons.Right)
			{
				if (this.activeShape.Vertices.Length > 3)
				{
					List<Vector2> newVertices = this.activeShape.Vertices.ToList();
					newVertices.RemoveAt(this.activeVertex);

					this.activeShape.Vertices = newVertices.ToArray();
				}
			}
		}
		public override void UpdateAction()
		{
			Vector2 worldPos = this.Environment.ActiveWorldPos.Xy;
			Vector2 localPos = this.Environment.ActiveBodyPos;
			Vector2 oldLocalPos = this.activeShape.Vertices[this.activeVertex];
			if (oldLocalPos != localPos)
			{
				this.activeEdgeWorldPos = worldPos;
				this.activeShape.Vertices[this.activeVertex] = localPos;
				this.activeShape.UpdateShape();
			}
		}
		public override void EndAction()
		{
			UndoRedoManager.Do(new EditRigidBodyPolyShapeAction(
				this.backedUpShape, 
				this.backedUpVertices,
				this.backedUpShape.Vertices));
		}

		public override void OnMouseMove()
		{
			base.OnMouseMove();
			if (!this.isMovingVertex)
				this.UpdateHoverState();
		}
		public override void OnActionKeyReleased(MouseButtons mouseButton)
		{
			base.OnActionKeyReleased(mouseButton);

			this.activeShape = null;
			this.activeVertex = -1;
			this.activeEdge = -1;

			this.Environment.EndToolAction();
		}
		public override void OnWorldOverlayDrawcalls(Canvas canvas)
		{
			float radius = 5.0f;
			float worldRadius = radius / MathF.Max(0.0001f, canvas.DrawDevice.GetScaleAtZ(0.0f));

			// Interaction indicator for an existing vertex
			if (this.activeVertex != -1)
			{
				canvas.DrawCircle(
					this.activeEdgeWorldPos.X, 
					this.activeEdgeWorldPos.Y, 
					worldRadius * 2.0f);
			}
			// Interaction indicator for a vertex-to-be-created
			else if (this.activeEdge != -1)
			{
				canvas.DrawCircle(
					this.activeEdgeWorldPos.X, 
					this.activeEdgeWorldPos.Y, 
					worldRadius * 1.0f);
				canvas.DrawCircle(
					this.activeEdgeWorldPos.X, 
					this.activeEdgeWorldPos.Y, 
					worldRadius * 2.0f);
			}

			// Draw an interaction indicator for every vertex of the active bodies shapes
			RigidBody body = this.Environment.ActiveBody;
			if (body != null)
			{
				// Prepare the transform matrix for this object, so 
				// we can move the RigidBody vertices into world space quickly
				Transform transform = body.GameObj.Transform;
				Vector2 bodyPos = transform.Pos.Xy;
				Vector2 bodyDotX;
				Vector2 bodyDotY;
				MathF.GetTransformDotVec(transform.Angle, transform.Scale, out bodyDotX, out bodyDotY);

				Vector3 mousePosWorld = this.Environment.ActiveWorldPos;
				foreach (ShapeInfo shape in body.Shapes)
				{
					PolyShapeInfo polygon = shape as PolyShapeInfo;
					if (polygon == null) continue;

					Vector2[] vertices = polygon.Vertices;
					Vector2[] worldVertices = new Vector2[vertices.Length];

					// Transform the shapes vertices into world space
					for (int index = 0; index < vertices.Length; index++)
					{
						Vector2 vertex = vertices[index];
						MathF.TransformDotVec(ref vertex, ref bodyDotX, ref bodyDotY);
						worldVertices[index] = bodyPos + vertex;
					}

					// Draw the vertex
					for (int i = 0; i < vertices.Length; i++)
					{
						canvas.FillCircle(
							worldVertices[i].X, 
							worldVertices[i].Y, 
							worldRadius);
					}
				}
			}
		}
		
		private void UpdateHoverState()
		{
			this.activeShape = null;
			this.activeVertex = -1;
			this.activeEdge = -1;
			this.activeEdgeWorldPos = Vector2.Zero;
			
			RigidBody body = this.Environment.ActiveBody;
			if (body == null) return;

			// Prepare the transform matrix for this object, so 
			// we can move the RigidBody vertices into world space quickly
			Transform transform = body.GameObj.Transform;
			Vector2 bodyPos = transform.Pos.Xy;
			Vector2 bodyDotX;
			Vector2 bodyDotY;
			MathF.GetTransformDotVec(transform.Angle, transform.Scale, out bodyDotX, out bodyDotY);

			Vector3 mousePosWorld = this.Environment.ActiveWorldPos;
			foreach (ShapeInfo shape in body.Shapes)
			{
				PolyShapeInfo polygon = shape as PolyShapeInfo;
				if (polygon == null) continue;

				Vector2[] vertices = polygon.Vertices;
				Vector2[] worldVertices = new Vector2[vertices.Length];

				// Transform the shapes vertices into world space
				for (int index = 0; index < vertices.Length; index++)
				{
					Vector2 vertex = vertices[index];
					MathF.TransformDotVec(ref vertex, ref bodyDotX, ref bodyDotY);
					worldVertices[index] = bodyPos + vertex;
				}

				// Determine whether the cursor is hovering something it can interact with
				float radius = 5.0f;
				float worldRadius = radius / MathF.Max(0.0001f, this.Environment.GetScaleAtZ(0.0f));
				bool anythingHovered = 
					this.GetHoveredVertex(worldVertices, mousePosWorld.Xy, worldRadius, out this.activeVertex, out this.activeEdgeWorldPos) ||
					this.GetHoveredEdge(worldVertices, mousePosWorld.Xy, worldRadius, out this.activeEdge, out this.activeEdgeWorldPos);
				if (anythingHovered)
				{
					this.activeShape = polygon;
					break;
				}
			}
		}

		private bool GetHoveredVertex(Vector2[] vertices, Vector2 targetPos, float radius, out int hoverIndex, out Vector2 hoverPos)
		{
			hoverIndex = -1;
			hoverPos = targetPos;

			for (int index = 0; index < vertices.Length; index++)
			{
				Vector2 vertex = vertices[index];

				if ((vertex - targetPos).Length <= radius)
				{
					hoverIndex = index;
					hoverPos = vertex;
					return true;
				}
			}

			return false;
		}
		private bool GetHoveredEdge(Vector2[] vertices, Vector2 targetPos, float radius, out int edgeStartIndex, out Vector2 hoverPos)
		{
			hoverPos = targetPos;
			edgeStartIndex = -1;

			for (int index = 0; index < vertices.Length; index++)
			{
				int nextIndex = (index + 1) % vertices.Length;
				Vector2 vertex = vertices[index];
				Vector2 nextVertex = vertices[nextIndex];
				
				Vector2 pointOnEdge = MathF.PointLineNearestPoint(
					targetPos.X, 
					targetPos.Y, 
					vertex.X, 
					vertex.Y, 
					nextVertex.X, 
					nextVertex.Y);

				if ((pointOnEdge - targetPos).Length <= radius)
				{
					edgeStartIndex = index;
					hoverPos = pointOnEdge;
					return true;
				}
			}

			return false;
		}
	}
}
