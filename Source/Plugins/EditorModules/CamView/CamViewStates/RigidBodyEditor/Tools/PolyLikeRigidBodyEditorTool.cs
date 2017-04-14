using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Duality.Components.Physics;
using Duality.Editor.Plugins.CamView.Properties;
using Duality.Editor.Plugins.CamView.UndoRedoActions;


namespace Duality.Editor.Plugins.CamView.CamViewStates
{
	public abstract class PolyLikeRigidBodyEditorTool : RigidBodyEditorTool
	{
		private int initialVertexCount = 0;
		private int currentVertex = 0;
		private bool pendingAdvance = false;
		private Vector2 lastPlacedVertexPos = Vector2.Zero;
		private ShapeInfo actionShape = null;

		protected virtual int MaxVertexCount
		{
			get { return 1024; }
		}

		protected virtual Vector2[] GetInitialVertices(Vector2 basePos)
		{
			return new Vector2[] 
			{
				basePos, 
				basePos + Vector2.UnitX, 
				basePos + Vector2.One
			};
		}
		protected abstract ShapeInfo CreateShapeInfo(Vector2[] vertices);
		protected abstract Vector2[] GetVertices(ShapeInfo shape);
		protected abstract void SetVertices(ShapeInfo shape, Vector2[] vertices);

		public override void BeginAction()
		{
			base.BeginAction();

			Vector2[] initialVertices = this.GetInitialVertices(this.Environment.ActiveBodyPos);

			this.currentVertex = 1;
			this.actionShape = this.CreateShapeInfo(initialVertices);
			this.initialVertexCount = initialVertices.Length;

			// Add the shape to the body. We're not doing an actual UndoRedoAction
			// just yet, because we don't want a potentially invalid under-construction
			// polygon to show up in the UndoRedo stack. We'll do a proper UndoRedoAction
			// when finishing up the shape.
			this.Environment.ActiveBody.AddShape(this.actionShape);
			DualityEditorApp.NotifyObjPropChanged(this, 
				new ObjectSelection(this.Environment.ActiveBody), 
				ReflectionInfo.Property_RigidBody_Shapes);

			this.Environment.SelectShapes(new ShapeInfo[] { this.actionShape });
		}
		public override void UpdateAction()
		{
			base.UpdateAction();

			// Apply pending vertex additions
			if (this.pendingAdvance)
			{
				// Ignore until the cursor has moved away far enough, so we can avoid entering
				// an invalid polygon state and annoying the user with intermediate validation errors.
				if ((this.lastPlacedVertexPos - this.Environment.ActiveBodyPos).Length < 2.5f)
					return;

				this.pendingAdvance = false;
				this.AdvanceToNextVertex();
			}

			Vector2[] vertices = this.GetVertices(this.actionShape);
			vertices[this.currentVertex] = this.Environment.ActiveBodyPos;

			// Before we've defined the first two vertices, we won't have
			// a valid shape. Don't trigger a physics update before something
			// useful can come out of it. Not updating yet will result in
			// remaining with the initially defined, valid dummy shape.
			if (this.currentVertex >= this.initialVertexCount - 1)
				this.SetVertices(this.actionShape, vertices);

			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.Environment.ActiveBody),
				ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void EndAction()
		{
			base.EndAction();
			Vector2[] vertices = this.GetVertices(this.actionShape);

			// If we're in the process of placing another vertex, remove that unplaced one
			if (!this.pendingAdvance)
			{
				List<Vector2> prevVertices = vertices.ToList();
				prevVertices.RemoveAt(this.currentVertex);
				vertices = prevVertices.ToArray();
			}

			// Apply the new polygon and force an immediate body update / sync, so
			// we can get a useful result from the shape's IsValid method.
			this.SetVertices(this.actionShape, vertices);
			this.Environment.ActiveBody.SynchronizeBodyShape();

			// Check if we have a fully defined, valid polygon to apply
			bool isValidShape = this.actionShape != null && this.actionShape.IsValid;
			bool isMinVertexCountReached = 
				vertices.Length >= this.initialVertexCount && 
				this.currentVertex >= this.initialVertexCount - 1;
			if (isValidShape && isMinVertexCountReached)
			{
				// Remove the shape and re-add it properly using an UndoRedoAction.
				// Now that we're sure the shape is valid, we want its creation to
				// show up in the UndoRedo stack.
				this.Environment.ActiveBody.RemoveShape(this.actionShape);
				UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.Environment.ActiveBody, this.actionShape));
			}
			else
			{
				this.Environment.SelectShapes(null);
				this.Environment.ActiveBody.RemoveShape(this.actionShape);
			}
			
			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.Environment.ActiveBody),
				ReflectionInfo.Property_RigidBody_Shapes);
			this.actionShape = null;
		}
		public override void OnActionKeyPressed()
		{
			base.OnActionKeyPressed();

			// Ignore further placements while waiting for the next vertex advance
			if (this.pendingAdvance)
				return;

			Vector2[] vertices = this.GetVertices(this.actionShape);
			vertices[this.currentVertex] = this.Environment.ActiveBodyPos;

			// Apply the current vertex and move on to the next.
			// Note that it is possible to place vertices in a way that
			// the polygon becomes invalid now, but will be valid again
			// later when some more vertices are in place.
			bool allowAdvance = this.actionShape.IsValid || this.CheckVertexPlacement(vertices, this.currentVertex);
			if (allowAdvance)
			{
				// Defer advancing to the next vertex until the cursor moved, so
				// we don't immediately enter an invalid state with the polygon we're creating.
				if (vertices.Length < this.MaxVertexCount)
				{
					this.pendingAdvance = true;
					this.lastPlacedVertexPos = this.Environment.ActiveBodyPos;
				}
				else
				{
					this.AdvanceToNextVertex();
					this.Environment.EndToolAction();
				}

				DualityEditorApp.NotifyObjPropChanged(this,
					new ObjectSelection(this.Environment.ActiveBody),
					ReflectionInfo.Property_RigidBody_Shapes);
			}
		}

		private void AdvanceToNextVertex()
		{
			this.Environment.LockedWorldPos = new Vector3(this.lastPlacedVertexPos, 0.0f);

			Vector2[] vertices = this.GetVertices(this.actionShape);
			List<Vector2> vertexList = vertices.ToList();
			
			if (this.currentVertex >= vertexList.Count - 1)
				vertexList.Add(this.Environment.ActiveBodyPos);
			
			this.SetVertices(this.actionShape, vertexList.ToArray());
			this.currentVertex++;
		}
		private bool CheckVertexPlacement(Vector2[] vertices, int currentVertex)
		{
			Vector2 current = vertices[this.currentVertex];
			Vector2 last = vertices[this.currentVertex - 1];

			// Require a minimum distance between current and last vertex to
			// avoid accidental double clicks and prevent ridiculously detailed
			// collision shapes.
			float distanceToLast = (current - last).Length;
			if (distanceToLast < 2.5f) return false;

			// Do not allow to cross any already existing edges, as this would produce
			// a non-simple polygon that can not be fixed again by placing more vertices.
			for (int i = 1; i < this.currentVertex; i++)
			{
				Vector2 start = vertices[i - 1];
				Vector2 end = vertices[i];
				if (MathF.LinesCross(start.X, start.Y, end.X, end.Y, current.X, current.Y, last.X, last.Y))
					return false;
			}

			return true;
		}

		public override string GetActionText()
		{
			return string.Format(
				"Vertex X:{0,9:0.00}/nVertex Y:{1,9:0.00}", 
				this.Environment.ActiveBodyPos.X, 
				this.Environment.ActiveBodyPos.Y);
		}
	}
}
