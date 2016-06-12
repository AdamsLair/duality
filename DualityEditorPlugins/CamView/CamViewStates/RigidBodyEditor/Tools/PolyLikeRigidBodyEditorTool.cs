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
		private int currentVertex = 0;
		private ShapeInfo actionShape = null;

		protected virtual int MaxVertexCount
		{
			get { return 1024; }
		}

		protected abstract ShapeInfo CreateShapeInfo(Vector2[] vertices);
		protected abstract Vector2[] GetVertices(ShapeInfo shape);
		protected abstract void SetVertices(ShapeInfo shape, Vector2[] vertices);
		protected virtual bool IsValidPolyon(Vector2[] vertices)
		{
			return true;
		}

		public override void BeginAction()
		{
			base.BeginAction();
			this.currentVertex = 1;
			this.actionShape = this.CreateShapeInfo(new Vector2[] 
			{
				this.Environment.ActiveBodyPos, 
				this.Environment.ActiveBodyPos + Vector2.UnitX, 
				this.Environment.ActiveBodyPos + Vector2.One
			});

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
			if (this.currentVertex >= 2)
				this.SetVertices(this.actionShape, vertices);

			DualityEditorApp.NotifyObjPropChanged(this,
				new ObjectSelection(this.Environment.ActiveBody),
				ReflectionInfo.Property_RigidBody_Shapes);
		}
		public override void EndAction()
		{
			base.EndAction();
			List<Vector2> vertices = this.GetVertices(this.actionShape).ToList();
			
			vertices.RemoveAt(this.currentVertex);
			if (vertices.Count < 3 || this.currentVertex < 2)
			{
				this.Environment.SelectShapes(null);
				this.Environment.ActiveBody.RemoveShape(this.actionShape);
			}
			else
			{
				this.SetVertices(this.actionShape, vertices.ToArray());

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

			if (this.currentVertex <= 2 || this.IsValidPolyon(vertices))
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

		public override string GetActionText()
		{
			return string.Format(
				"Vertex X:{0,9:0.00}/nVertex Y:{0,9:0.00}", 
				this.Environment.ActiveBodyPos.X, 
				this.Environment.ActiveBodyPos.Y);
		}
	}
}
