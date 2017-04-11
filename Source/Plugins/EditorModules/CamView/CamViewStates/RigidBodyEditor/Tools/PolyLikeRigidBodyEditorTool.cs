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
			List<Vector2> prevVertices = this.GetVertices(this.actionShape).ToList();
			prevVertices.RemoveAt(this.currentVertex);

			Vector2[] vertices = prevVertices.ToArray();
			bool isValidShape = this.actionShape != null && this.actionShape.IsValid;
			if (vertices.Length < this.initialVertexCount || this.currentVertex < this.initialVertexCount - 1 || !isValidShape)
			{
				this.Environment.SelectShapes(null);
				this.Environment.ActiveBody.RemoveShape(this.actionShape);
			}
			else
			{
				this.SetVertices(this.actionShape, vertices);

				// Remove the shape and re-add it properly using an UndoRedoAction.
				// Now that we're sure the shape is valid, we want its creation to
				// show up in the UndoRedo stack.
				this.Environment.ActiveBody.RemoveShape(this.actionShape);
				UndoRedoManager.Do(new CreateRigidBodyShapeAction(this.Environment.ActiveBody, this.actionShape));
			}
			
			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.Environment.ActiveBody),
				ReflectionInfo.Property_RigidBody_Shapes);
			this.actionShape = null;
		}
		public override void OnActionKeyPressed()
		{
			base.OnActionKeyPressed();

			Vector2[] vertices = this.GetVertices(this.actionShape);
			vertices[this.currentVertex] = this.Environment.ActiveBodyPos;

			// Apply the current vertex and move on to the next.
			// Note that it is possible to place vertices in a way that
			// the polygon becomes invalid now, but will be valid again
			// later when some more vertices are in place.
			bool allowAdvance = this.CheckVertexPlacement(vertices, this.currentVertex);
			if (allowAdvance)
			{
				this.Environment.LockedWorldPos = this.Environment.ActiveWorldPos;

				List<Vector2> vertexList = vertices.ToList();
			
				if (this.currentVertex >= vertexList.Count - 1)
					vertexList.Add(this.Environment.ActiveBodyPos);
			
				this.SetVertices(this.actionShape, vertexList.ToArray());
				this.currentVertex++;

				if (vertexList.Count >= this.MaxVertexCount)
					this.Environment.EndToolAction();
			}

			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.Environment.ActiveBody),
				ReflectionInfo.Property_RigidBody_Shapes);
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
